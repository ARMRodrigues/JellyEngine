#include "JellyEngine.h"

#include "Graphics/GraphicsAPIType.h"
#include "Graphics/GraphicsAPIFactory.h"
#include "Window/GLFWindowSystem.h"

// -----------------------------------------------------------------------------
// Initializes the engine with the selected graphics API and window settings.
// -----------------------------------------------------------------------------
bool JellyEngine::Initialize(GraphicsAPIType apiType, const WindowSettings& settings) {
    try {
        window = std::make_unique<GLFWindowSystem>();
        window->CreateWindow(settings);

        graphics = GraphicsAPIFactory::Create(apiType);

        if (!graphics) {
            std::cerr << "Unsupported graphics API\n";
            return false;
        }

        graphics->Initialize(window.get());

        graphics->BeginFrame();
        graphics->EndFrame();

        window->ShowWindow();

        return true;
    } catch (const std::exception& e) {
        std::cerr << "Failed to initialize JellyEngine: " << e.what() << "\n";
        return false;
    }
}

// -----------------------------------------------------------------------------
// Returns true if the window is still open.
// -----------------------------------------------------------------------------
bool JellyEngine::IsRunning() {
    return window && window->IsWindowOpen();
}

// -----------------------------------------------------------------------------
// Polls input and window system events.
// -----------------------------------------------------------------------------
void JellyEngine::PollEvents() {
    if (window) {
        window->PollEvents();
    }
}

// -----------------------------------------------------------------------------
/// Renders a single frame by beginning and ending the graphics API frame.
/// Typically called once per loop iteration.
// -----------------------------------------------------------------------------
void JellyEngine::Render() {
    graphics->BeginFrame();
    graphics->EndFrame();
}

// -----------------------------------------------------------------------------
// Shuts down the engine and releases window resources.
// -----------------------------------------------------------------------------
void JellyEngine::Shutdown() {
    if (graphics) {
        graphics->Shutdown();
    }
    if (window) {
        window->DestroyWindow();
    }
}
