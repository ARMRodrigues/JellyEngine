#include "Graphics/Vulkan/VulkanGraphicsAPI.h"

#include "Logger.h"
#include "Graphics/GraphicsApiException.h"
#include "Window/IWindowSystem.h"

#include <algorithm>
#include <cassert>

// -----------------------------------------------------------------------------
// Disabled default initializer. Forces users to provide a window system for proper Vulkan setup.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::Initialize()
{
    throw GraphicsApiException("Use Initialize(IWindowSystem*) instead");
}

// -----------------------------------------------------------------------------
// Initializes the Vulkan API using the provided window system.
// This is the main entry point for setting up Vulkan and must be called before rendering.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::Initialize(IWindowSystem *windowSystem)
{
    windowProvider = dynamic_cast<INativeWindowHandleProvider*>(windowSystem);
    if (!windowProvider) {
        throw GraphicsApiException("Window system lacks native handle interface");
    }

    CreateInstance();
    CreateSurface();
    PickPhysicalDevice();
    CreateLogicalDevice();
    CreateSwapChain();
    CreateImageViews();
    CreateRenderPass();
    CreateFramebuffers();
    CreateCommandPool();
    CreateCommandBuffers();
    CreateSyncObjects();
}

// -----------------------------------------------------------------------------
// Creates the Vulkan instance, which is the base of the Vulkan context.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateInstance() {
    std::vector<const char*> extensions = windowProvider->GetVulkanRequiredExtensions();

    VkApplicationInfo appInfo{};
    appInfo.sType = VK_STRUCTURE_TYPE_APPLICATION_INFO;
    appInfo.pApplicationName = "Jelly";
    appInfo.applicationVersion = VK_MAKE_VERSION(1, 0, 0);
    appInfo.pEngineName = "Jelly Engine";
    appInfo.engineVersion = VK_MAKE_VERSION(1, 0, 0);
    appInfo.apiVersion = VK_API_VERSION_1_0;

    VkInstanceCreateInfo createInfo{};
    createInfo.sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
    createInfo.pApplicationInfo = &appInfo;
    createInfo.enabledExtensionCount = static_cast<uint32_t>(extensions.size());
    createInfo.ppEnabledExtensionNames = extensions.data();
    createInfo.enabledLayerCount = 0;
    createInfo.pNext = nullptr;

    if (vkCreateInstance(&createInfo, nullptr, &instance) != VK_SUCCESS) {
        throw GraphicsApiException("Failed to create Vulkan instance!");
    }

    uint32_t apiVersion = 0;
    if (vkEnumerateInstanceVersion(&apiVersion) == VK_SUCCESS) {
        char message[128];
        snprintf(message, sizeof(message),
                "Vulkan instance created (API version %u.%u.%u)",
                VK_VERSION_MAJOR(apiVersion),
                VK_VERSION_MINOR(apiVersion),
                VK_VERSION_PATCH(apiVersion));
        Logger::Log(LogLevel::Info, message);
    }
}

// -----------------------------------------------------------------------------
// Creates a platform-specific window surface to present rendered images.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateSurface() {
    auto *surfaceProvider = windowProvider;
    if (!surfaceProvider)
        throw GraphicsApiException("Window system does not support Vulkan surface creation!");

    surface = surfaceProvider->CreateVulkanSurface(instance);
}

// -----------------------------------------------------------------------------
// Selects a suitable physical GPU that supports the required Vulkan features.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::PickPhysicalDevice() {
    uint32_t deviceCount = 0;
    vkEnumeratePhysicalDevices(instance, &deviceCount, nullptr);
    if (deviceCount == 0) {
        throw GraphicsApiException("Failed to find GPUs with Vulkan support!");
    }

    std::vector<VkPhysicalDevice> devices(deviceCount);
    vkEnumeratePhysicalDevices(instance, &deviceCount, devices.data());

    for (const auto &device : devices) {
        QueueFamilyIndices indices = FindQueueFamilies(device, surface);

        bool extensionsSupported = false;
        {
            uint32_t extCount = 0;
            vkEnumerateDeviceExtensionProperties(device, nullptr, &extCount, nullptr);
            std::vector<VkExtensionProperties> availableExtensions(extCount);
            vkEnumerateDeviceExtensionProperties(device, nullptr, &extCount, availableExtensions.data());

            std::set<std::string> requiredExtensions = {VK_KHR_SWAPCHAIN_EXTENSION_NAME};
            for (const auto &ext : availableExtensions)
                requiredExtensions.erase(ext.extensionName);

            extensionsSupported = requiredExtensions.empty();
        }

        bool swapchainAdequate = false;
        if (extensionsSupported) {
            SwapChainSupportDetails swapChainSupport = QuerySwapChainSupport(device, surface);
            swapchainAdequate = !swapChainSupport.formats.empty() && !swapChainSupport.presentModes.empty();
        }

        if (indices.IsComplete() && extensionsSupported && swapchainAdequate) {
            physicalDevice = device;
            break;
        }
    }

    if (physicalDevice == VK_NULL_HANDLE) {
        throw GraphicsApiException("Failed to find a suitable GPU with Vulkan support and swapchain capabilities!");
    }
}

// -----------------------------------------------------------------------------
// Creates a logical device and retrieves queue handles for graphics and presentation.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateLogicalDevice() {
    QueueFamilyIndices indices = FindQueueFamilies(physicalDevice, surface);

    std::vector<VkDeviceQueueCreateInfo> queueCreateInfos;
    std::set<uint32_t> uniqueFamilies = {
        indices.graphicsFamily.value(), indices.presentFamily.value()};

    float queuePriority = 1.0f;
    for (uint32_t family : uniqueFamilies) {
        VkDeviceQueueCreateInfo queueInfo{};
        queueInfo.sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO;
        queueInfo.queueFamilyIndex = family;
        queueInfo.queueCount = 1;
        queueInfo.pQueuePriorities = &queuePriority;
        queueCreateInfos.push_back(queueInfo);
    }

    VkPhysicalDeviceFeatures deviceFeatures{};

    VkDeviceCreateInfo createInfo{};
    createInfo.sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;
    createInfo.queueCreateInfoCount = static_cast<uint32_t>(queueCreateInfos.size());
    createInfo.pQueueCreateInfos = queueCreateInfos.data();
    createInfo.pEnabledFeatures = &deviceFeatures;

    const char *extensions[] = {VK_KHR_SWAPCHAIN_EXTENSION_NAME};
    createInfo.enabledExtensionCount = 1;
    createInfo.ppEnabledExtensionNames = extensions;

    if (vkCreateDevice(physicalDevice, &createInfo, nullptr, &device) != VK_SUCCESS) {
        throw GraphicsApiException("Failed to create logical device!");
    }

    vkGetDeviceQueue(device, indices.graphicsFamily.value(), 0, &graphicsQueue);
    vkGetDeviceQueue(device, indices.presentFamily.value(), 0, &presentQueue);
}

// -----------------------------------------------------------------------------
// Creates the swapchain, which manages the images to be presented to the screen.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateSwapChain() {
    SwapChainSupportDetails support = QuerySwapChainSupport(physicalDevice, surface);

    VkSurfaceFormatKHR surfaceFmt = ChooseSurfaceFormat(support.formats);
    VkPresentModeKHR present = ChoosePresentMode(support.presentModes);
    VkExtent2D extent = ChooseSwapExtent(support.capabilities, windowProvider);

    uint32_t imageCount = support.capabilities.minImageCount + 1;
    if (support.capabilities.maxImageCount &&
        imageCount > support.capabilities.maxImageCount) {
        imageCount = support.capabilities.maxImageCount;
    }

    VkSwapchainCreateInfoKHR sci{};
    sci.sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
    sci.surface = surface;
    sci.minImageCount = imageCount;
    sci.imageFormat = surfaceFmt.format;
    sci.imageColorSpace = surfaceFmt.colorSpace;
    sci.imageExtent = extent;
    sci.imageArrayLayers = 1;
    sci.imageUsage = VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT;

    QueueFamilyIndices idx = FindQueueFamilies(physicalDevice, surface);
    uint32_t qfams[] = {idx.graphicsFamily.value(), idx.presentFamily.value()};

    if (idx.graphicsFamily != idx.presentFamily) {
        sci.imageSharingMode = VK_SHARING_MODE_CONCURRENT;
        sci.queueFamilyIndexCount = 2;
        sci.pQueueFamilyIndices = qfams;
    }
    else {
        sci.imageSharingMode = VK_SHARING_MODE_EXCLUSIVE;
    }

    sci.preTransform = support.capabilities.currentTransform;
    sci.compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;
    sci.presentMode = present;
    sci.clipped = VK_TRUE;

    if (vkCreateSwapchainKHR(device, &sci, nullptr, &swapchain) != VK_SUCCESS) {
        throw GraphicsApiException("Failed to create swapchain");
    }

    swapchainImageFormat = surfaceFmt.format;
    swapchainExtent = extent;

    uint32_t count = 0;
    vkGetSwapchainImagesKHR(device, swapchain, &count, nullptr);
    swapchainImages.resize(count);
    vkGetSwapchainImagesKHR(device, swapchain, &count, swapchainImages.data());
}

// -----------------------------------------------------------------------------
// Creates image views for each image in the swapchain.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateImageViews() {
    swapchainImageViews.resize(swapchainImages.size());

    for (size_t i = 0; i < swapchainImages.size(); ++i) {
        VkImageViewCreateInfo ivci{};
        ivci.sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
        ivci.image = swapchainImages[i];
        ivci.viewType = VK_IMAGE_VIEW_TYPE_2D;
        ivci.format = swapchainImageFormat;
        ivci.components = {
            VK_COMPONENT_SWIZZLE_IDENTITY, VK_COMPONENT_SWIZZLE_IDENTITY,
            VK_COMPONENT_SWIZZLE_IDENTITY, VK_COMPONENT_SWIZZLE_IDENTITY};
        ivci.subresourceRange = {
            VK_IMAGE_ASPECT_COLOR_BIT, 0, 1, 0, 1};

        if (vkCreateImageView(device, &ivci, nullptr, &swapchainImageViews[i]) != VK_SUCCESS) {
            throw GraphicsApiException("Failed to create image views!");
        }
    }
}

// -----------------------------------------------------------------------------
// Defines the rendering process, including attachments, subpasses, and dependencies.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateRenderPass() {
    VkAttachmentDescription colorAttachment{};
    colorAttachment.format = swapchainImageFormat;
    colorAttachment.samples = VK_SAMPLE_COUNT_1_BIT;
    colorAttachment.loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR;
    colorAttachment.storeOp = VK_ATTACHMENT_STORE_OP_STORE;
    colorAttachment.stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE;
    colorAttachment.stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE;
    colorAttachment.initialLayout = VK_IMAGE_LAYOUT_UNDEFINED;
    colorAttachment.finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;

    VkAttachmentReference colorAttachmentRef{};
    colorAttachmentRef.attachment = 0;
    colorAttachmentRef.layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;

    VkSubpassDescription subpass{};
    subpass.pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS;
    subpass.colorAttachmentCount = 1;
    subpass.pColorAttachments = &colorAttachmentRef;

    VkRenderPassCreateInfo renderPassInfo{};
    renderPassInfo.sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO;
    renderPassInfo.attachmentCount = 1;
    renderPassInfo.pAttachments = &colorAttachment;
    renderPassInfo.subpassCount = 1;
    renderPassInfo.pSubpasses = &subpass;

    if (vkCreateRenderPass(device, &renderPassInfo, nullptr, &renderPass) != VK_SUCCESS) {
        throw GraphicsApiException("Failed to create render pass!");
    }
}

// -----------------------------------------------------------------------------
// Creates framebuffers for each image view, used in the render pass.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateFramebuffers() {
    swapChainFramebuffers.resize(swapchainImageViews.size());

    for (size_t i = 0; i < swapchainImageViews.size(); ++i) {
        VkImageView attachments[] = {
            swapchainImageViews[i]
        };

        VkFramebufferCreateInfo framebufferInfo{};
        framebufferInfo.sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
        framebufferInfo.renderPass = renderPass;
        framebufferInfo.attachmentCount = 1;
        framebufferInfo.pAttachments = attachments;
        framebufferInfo.width = swapchainExtent.width;
        framebufferInfo.height = swapchainExtent.height;
        framebufferInfo.layers = 1;

        if (vkCreateFramebuffer(device, &framebufferInfo, nullptr, &swapChainFramebuffers[i]) != VK_SUCCESS) {
            throw GraphicsApiException("Failed to create framebuffer!");
        }
    }
}

// -----------------------------------------------------------------------------
// Creates a command pool from which command buffers will be allocated.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateCommandPool() {
    QueueFamilyIndices queueFamilyIndices = FindQueueFamilies(physicalDevice, surface);

    VkCommandPoolCreateInfo poolInfo {};
    poolInfo.sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
    poolInfo.flags = VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;
    poolInfo.queueFamilyIndex = queueFamilyIndices.graphicsFamily.value();

    if (vkCreateCommandPool(device, &poolInfo, nullptr, &commandPool) != VK_SUCCESS) {
        throw GraphicsApiException("Failed to create command pool!");
    }
}

// -----------------------------------------------------------------------------
// Allocates and prepares command buffers used to record rendering commands.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateCommandBuffers() {
    if (!commandBuffers.empty()) {
        vkFreeCommandBuffers(device, commandPool, static_cast<uint32_t>(commandBuffers.size()), commandBuffers.data());
        commandBuffers.clear();
    }

    commandBuffers.resize(swapchainImages.size());

    VkCommandBufferAllocateInfo allocInfo{};
    allocInfo.sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
    allocInfo.commandPool = commandPool;
    allocInfo.level = VK_COMMAND_BUFFER_LEVEL_PRIMARY;
    allocInfo.commandBufferCount = static_cast<uint32_t>(commandBuffers.size());

    if (vkAllocateCommandBuffers(device, &allocInfo, commandBuffers.data()) != VK_SUCCESS) {
        throw GraphicsApiException("Failed to allocate command buffers!");
    }
}

// -----------------------------------------------------------------------------
// Creates synchronization objects such as semaphores and fences to coordinate rendering.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CreateSyncObjects() {
    for (VkFence f : inFlightFences) {
        if (f) {
            vkDestroyFence(device, f, nullptr);
        }
    }

    for (VkSemaphore s : imageAvailableSemaphores)
        if (s)
            vkDestroySemaphore(device, s, nullptr);
    for (VkSemaphore s : renderFinishedSemaphores)
        if (s)
            vkDestroySemaphore(device, s, nullptr);

    inFlightFences.clear();
    imageAvailableSemaphores.clear();
    renderFinishedSemaphores.clear();

    // 2) Redimensiona corretamente
    inFlightFences.resize(MAX_FRAMES_IN_FLIGHT, VK_NULL_HANDLE);
    imageAvailableSemaphores.resize(MAX_FRAMES_IN_FLIGHT, VK_NULL_HANDLE);
    renderFinishedSemaphores.resize(MAX_FRAMES_IN_FLIGHT, VK_NULL_HANDLE);

    // 3) Infos de criação
    VkSemaphoreCreateInfo semInfo{VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO};
    VkFenceCreateInfo fenceInfo{VK_STRUCTURE_TYPE_FENCE_CREATE_INFO};
    fenceInfo.flags = VK_FENCE_CREATE_SIGNALED_BIT;

    // 4) Use o tamanho real dos vetores para iterar
    for (std::size_t i = 0; i < inFlightFences.size(); ++i)
    {
        if (vkCreateSemaphore(device, &semInfo, nullptr, &imageAvailableSemaphores[i]) != VK_SUCCESS ||
            vkCreateSemaphore(device, &semInfo, nullptr, &renderFinishedSemaphores[i]) != VK_SUCCESS ||
            vkCreateFence(device, &fenceInfo, nullptr, &inFlightFences[i]) != VK_SUCCESS)
        {
            throw GraphicsApiException("Failed to create sync objects!");
        }
    }
}

// -----------------------------------------------------------------------------
// Begins the frame by waiting for the previous frame to finish, acquiring the next image from the swapchain,
// and recording rendering commands into the appropriate command buffer.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::BeginFrame()
{
    // Aguarda o frame atual terminar
    vkWaitForFences(device, 1, &inFlightFences[currentFrame], VK_TRUE, UINT64_MAX);
    vkResetFences(device, 1, &inFlightFences[currentFrame]);

    // Adquire imagem do swapchain
    VkResult result = vkAcquireNextImageKHR(
        device,
        swapchain,
        UINT64_MAX,
        imageAvailableSemaphores[currentFrame], // sinaliza quando a imagem estiver disponível
        VK_NULL_HANDLE,
        &currentImageIndex);

    if (result == VK_ERROR_OUT_OF_DATE_KHR)
    {
        RecreateSwapChain();
        return;
    }
    else if (result != VK_SUCCESS && result != VK_SUBOPTIMAL_KHR)
    {
        throw GraphicsApiException("Failed to acquire swap chain image!");
    }

    // Grava comandos de renderização
    RecordCommandBuffer(commandBuffers[currentImageIndex], currentImageIndex);
}

// -----------------------------------------------------------------------------
// Ends the frame by submitting recorded command buffers for execution,
// presenting the rendered image to the swapchain, and advancing to the next frame.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::EndFrame()
{
    VkSubmitInfo submitInfo{VK_STRUCTURE_TYPE_SUBMIT_INFO};

    VkSemaphore waitSemaphores[] = {imageAvailableSemaphores[currentFrame]};
    VkPipelineStageFlags waitStages[] = {VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT};
    submitInfo.waitSemaphoreCount = 1;
    submitInfo.pWaitSemaphores = waitSemaphores;
    submitInfo.pWaitDstStageMask = waitStages;

    submitInfo.commandBufferCount = 1;
    submitInfo.pCommandBuffers = &commandBuffers[currentImageIndex];

    VkSemaphore signalSemaphores[] = {renderFinishedSemaphores[currentFrame]};
    submitInfo.signalSemaphoreCount = 1;
    submitInfo.pSignalSemaphores = signalSemaphores;

    // Envia os comandos para execução
    if (vkQueueSubmit(graphicsQueue, 1, &submitInfo, inFlightFences[currentFrame]) != VK_SUCCESS)
    {
        throw GraphicsApiException("Failed to submit draw command buffer!");
    }

    // Apresenta a imagem no swapchain
    VkPresentInfoKHR presentInfo{VK_STRUCTURE_TYPE_PRESENT_INFO_KHR};
    presentInfo.waitSemaphoreCount = 1;
    presentInfo.pWaitSemaphores = signalSemaphores;
    presentInfo.swapchainCount = 1;
    presentInfo.pSwapchains = &swapchain;
    presentInfo.pImageIndices = &currentImageIndex;

    VkResult result = vkQueuePresentKHR(presentQueue, &presentInfo);

    if (result == VK_ERROR_OUT_OF_DATE_KHR || result == VK_SUBOPTIMAL_KHR)
    {
        RecreateSwapChain();
    }
    else if (result != VK_SUCCESS)
    {
        throw GraphicsApiException("Failed to present swap chain image!");
    }

    // Avança para o próximo frame
    currentFrame = (currentFrame + 1) % MAX_FRAMES_IN_FLIGHT;
}

// -----------------------------------------------------------------------------
// Recreates the swapchain and all related resources when the window is resized or becomes incompatible.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::RecreateSwapChain()
{
    uint32_t width = 0, height = 0;
    windowProvider->GetFramebufferSize(width, height);

    while (width == 0 || height == 0) {
        windowProvider->GetFramebufferSize(width, height);
        windowProvider->WaitEvents();
    }

    vkDeviceWaitIdle(device);

    CleanupSwapChain();

    CreateSwapChain();
    CreateImageViews();
    CreateRenderPass();
    CreateFramebuffers();
    CreateCommandBuffers();
    //CreateSyncObjects();
}

// -----------------------------------------------------------------------------
// Cleans up all Vulkan resources related to the swapchain, including framebuffers, image views, and the render pass.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::CleanupSwapChain()
{
    for (auto framebuffer : swapChainFramebuffers)
    {
        vkDestroyFramebuffer(device, framebuffer, nullptr);
    }
    swapChainFramebuffers.clear();

    for (auto imageView : swapchainImageViews)
    {
        vkDestroyImageView(device, imageView, nullptr);
    }
    swapchainImageViews.clear();

    vkDestroySwapchainKHR(device, swapchain, nullptr);

    if (renderPass != VK_NULL_HANDLE) {
        vkDestroyRenderPass(device, renderPass, nullptr);
        renderPass = VK_NULL_HANDLE;
    }
}

// -----------------------------------------------------------------------------
// Records rendering commands into the specified command buffer for the given swapchain image.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::RecordCommandBuffer(VkCommandBuffer commandBuffer, uint32_t imageIndex)
{
    VkCommandBufferBeginInfo beginInfo{VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO};

    vkBeginCommandBuffer(commandBuffer, &beginInfo);

    VkRenderPassBeginInfo renderPassInfo{VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO};
    renderPassInfo.renderPass = renderPass;
    renderPassInfo.framebuffer = swapChainFramebuffers[imageIndex];
    renderPassInfo.renderArea.offset = {0, 0};
    renderPassInfo.renderArea.extent = swapchainExtent;

    VkClearValue clearColor = {0.468f, 0.177f, 0.741f, 1.0f};
    renderPassInfo.clearValueCount = 1;
    renderPassInfo.pClearValues = &clearColor;

    vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);

    // TODO: vkCmdDraw / vkCmdBindPipeline etc aqui...

    vkCmdEndRenderPass(commandBuffer);
    vkEndCommandBuffer(commandBuffer);
}

// -----------------------------------------------------------------------------
// Cleans up and destroys all Vulkan resources before shutting down the application.
// -----------------------------------------------------------------------------
void VulkanGraphicsAPI::Shutdown()
{
    for (auto view : swapchainImageViews)
        vkDestroyImageView(device, view, nullptr);
    swapchainImageViews.clear();

    if (swapchain != VK_NULL_HANDLE)
    {
        vkDestroySwapchainKHR(device, swapchain, nullptr);
        swapchain = VK_NULL_HANDLE;
    }

    if (renderPass != VK_NULL_HANDLE)
    {
        vkDestroyRenderPass(device, renderPass, nullptr);
        renderPass = VK_NULL_HANDLE;
    }

    for (VkFramebuffer framebuffer : swapChainFramebuffers)
    {
        vkDestroyFramebuffer(device, framebuffer, nullptr);
    }
    swapChainFramebuffers.clear();

    if (commandPool != VK_NULL_HANDLE)
    {
        vkDestroyCommandPool(device, commandPool, nullptr);
        commandPool = VK_NULL_HANDLE;
    }

    for (std::size_t i = 0; i < imageAvailableSemaphores.size(); ++i)
    {
        vkDestroySemaphore(device, renderFinishedSemaphores[i], nullptr);
        vkDestroySemaphore(device, imageAvailableSemaphores[i], nullptr);
        vkDestroyFence(device, inFlightFences[i], nullptr);
    }
    imageAvailableSemaphores.clear();
    renderFinishedSemaphores.clear();
    inFlightFences.clear();

    if (device != VK_NULL_HANDLE)
    {
        vkDestroyDevice(device, nullptr);
        device = VK_NULL_HANDLE;
    }

    if (surface != VK_NULL_HANDLE)
    {
        vkDestroySurfaceKHR(instance, surface, nullptr);
        surface = VK_NULL_HANDLE;
    }

    if (instance != VK_NULL_HANDLE)
    {
        vkDestroyInstance(instance, nullptr);
        instance = VK_NULL_HANDLE;
    }
}