#pragma once

#include <vector>
#include <vulkan/vulkan.h>

/// Interface for providing platform-specific window integration for Vulkan.
class INativeWindowHandleProvider {
public:
    virtual ~INativeWindowHandleProvider() = default;

    /// Returns a pointer to the native window handle (e.g., GLFWwindow*, HWND, etc.).
    virtual void* GetNativeWindowHandle() = 0;

    /// Retrieves the current framebuffer size in pixels (width × height).
    virtual void GetFramebufferSize(uint32_t& width, uint32_t& height) = 0;

    /// Waits for window system events (e.g., resizing, input).
    virtual void WaitEvents() = 0;

    /// Returns the required Vulkan instance extensions for the window system.
    virtual std::vector<const char*> GetVulkanRequiredExtensions() = 0;

    /// Creates a Vulkan surface tied to the current window.
    virtual VkSurfaceKHR CreateVulkanSurface(VkInstance instance) = 0;
};
