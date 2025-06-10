#pragma once

#include "WindowSettings.h"

/// Interface for platform-specific window systems (e.g., GLFW, SDL).
class IWindowSystem {
public:
    virtual ~IWindowSystem() = default;

    /// Creates a window using the given settings.
    virtual void CreateWindow(const WindowSettings& settings) = 0;

    /// Returns true if the window is still open.
    virtual bool IsWindowOpen() = 0;

    /// Polls window system events (input, window resize, etc.).
    virtual void PollEvents() = 0;

    /// Destroys the current window and releases associated resources.
    virtual void DestroyWindow() = 0;
};
