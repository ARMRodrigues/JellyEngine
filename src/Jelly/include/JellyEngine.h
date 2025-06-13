#pragma once

#include <memory>

#include "Graphics/GraphicsAPIType.h"
#include "Graphics/IGraphicsAPI.h"
#include "Window/IWindowSystem.h"
#include "Window/WindowSettings.h"

/// Core engine class responsible for managing the window and graphics API.
class JellyEngine {
public:
    /// Initializes the engine with the selected graphics API and window settings.
    /// @param apiType The enum of the graphics API (e.g., "Vulkan").
    /// @param settings The configuration for the window.
    /// @return True if initialization succeeded.
    bool Initialize(GraphicsAPIType apiType, const WindowSettings& settings);

    /// Returns true if the engine should keep running (i.e., window is open).
    bool IsRunning();

    /// Polls input and window events.
    void PollEvents();

    /// Renders a single frame by beginning and ending the graphics API frame.
    /// Typically called once per loop iteration.
    void Render();

    /// Shuts down the engine and releases window resources.
    void Shutdown();

private:
    std::unique_ptr<IWindowSystem> window;  ///< Active window system instance.
    std::unique_ptr<IGraphicsAPI> graphics; ///< Active graphics API instance.
};
