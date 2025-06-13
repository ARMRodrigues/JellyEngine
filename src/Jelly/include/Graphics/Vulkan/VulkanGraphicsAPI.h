#pragma once

#include "QueueFamilyIndices.h"
#include "SwapChainSupportDetails.h"
#include "Graphics/IGraphicsAPI.h"
#include "Window/INativeWindowHandleProvider.h"

#include <iostream>
#include <vector>
#include <set>

#include "vulkan/vulkan.h"

class VulkanGraphicsAPI final : public IGraphicsAPI {
public:
    void Initialize() override;
    void Initialize(IWindowSystem* windowSystem) override;
    void BeginFrame() override;
    void EndFrame() override;
    void Shutdown() override;

private:
    // Window system
    INativeWindowHandleProvider* windowProvider = nullptr;

    // Vulkan instance and device
    VkInstance       instance       = VK_NULL_HANDLE;
    VkPhysicalDevice physicalDevice = VK_NULL_HANDLE;
    VkDevice         device         = VK_NULL_HANDLE;

    // Queues
    VkQueue graphicsQueue = VK_NULL_HANDLE;
    VkQueue presentQueue  = VK_NULL_HANDLE;

    // Surface and swapchain
    VkSurfaceKHR     surface            = VK_NULL_HANDLE;
    VkSwapchainKHR   swapchain          = VK_NULL_HANDLE;
    VkFormat         swapchainImageFormat;
    VkExtent2D       swapchainExtent;
    std::vector<VkImage>     swapchainImages;
    std::vector<VkImageView> swapchainImageViews;

    // Render pass and framebuffers
    VkRenderPass                renderPass = VK_NULL_HANDLE;
    std::vector<VkFramebuffer> swapChainFramebuffers;

    // Command pool and buffers
    VkCommandPool                 commandPool = VK_NULL_HANDLE;
    std::vector<VkCommandBuffer> commandBuffers;

    // Synchronization
    std::vector<VkSemaphore> imageAvailableSemaphores;
    std::vector<VkSemaphore> renderFinishedSemaphores;
    std::vector<VkFence>     inFlightFences;

    // Frame state
    size_t   currentFrame      = 0;
    uint32_t currentImageIndex = 0;
    const int MAX_FRAMES_IN_FLIGHT = 2;

    // Internal methods
    void CreateInstance();
    void CreateSurface();
    void PickPhysicalDevice();
    void CreateLogicalDevice();
    void CreateSwapChain();
    void CreateImageViews();
    void CreateRenderPass();
    void CreateFramebuffers();
    void CreateCommandPool();
    void CreateCommandBuffers();
    void CreateSyncObjects();
    void RecreateSwapChain();
    void CleanupSwapChain();
    void RecordCommandBuffer(VkCommandBuffer, uint32_t imageIndex);

    // Helpers functions
    static SwapChainSupportDetails QuerySwapChainSupport(VkPhysicalDevice device, VkSurfaceKHR surface);
    static QueueFamilyIndices FindQueueFamilies(VkPhysicalDevice device, VkSurfaceKHR surface);
    static VkSurfaceFormatKHR ChooseSurfaceFormat(const std::vector<VkSurfaceFormatKHR> &formats);
    static VkPresentModeKHR ChoosePresentMode(const std::vector<VkPresentModeKHR> &modes);
    static VkExtent2D ChooseSwapExtent(const VkSurfaceCapabilitiesKHR &caps, INativeWindowHandleProvider *win);
};
