#pragma once

/// Base interface for graphics APIs (e.g., Vulkan, OpenGL).
class IGraphicsAPI {
public:
    virtual ~IGraphicsAPI() = default;

    /// Initializes the graphics API (e.g., create device, setup context).
    virtual void Init() = 0;

    /// Begins rendering a new frame.
    virtual void BeginFrame() = 0;

    /// Ends rendering of the current frame.
    virtual void EndFrame() = 0;
};
