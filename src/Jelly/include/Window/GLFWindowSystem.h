#pragma once

#define GLFW_INCLUDE_VULKAN
#include <GLFW/glfw3.h>

#include "IWindowSystem.h"
#include "INativeWindowHandleProvider.h"

/// GLFW-based implementation of IWindowSystem.
class GLFWindowSystem final : public IWindowSystem, public INativeWindowHandleProvider {
public:
    void CreateWindow(const WindowSettings& settings) override;
    void ShowWindow() override;
    bool IsWindowOpen() override;
    void PollEvents() override;
    void DestroyWindow() override;

    void *GetNativeWindowHandle() override;
    void GetFramebufferSize(uint32_t& w, uint32_t& h) override;
    void WaitEvents() override;
    VkSurfaceKHR CreateVulkanSurface(VkInstance instance) override;
    std::vector<const char*>GetVulkanRequiredExtensions() override;

private:
    GLFWwindow* window = nullptr; ///< Pointer to the GLFW window instance.
};
