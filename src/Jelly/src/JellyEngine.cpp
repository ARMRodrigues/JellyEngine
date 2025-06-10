#include "JellyEngine.h"

#include "Graphics/GraphicsAPIType.h"
#include "Graphics/GraphicsAPIFactory.h"
#include "Window/GLFWindowSystem.h"

// -----------------------------------------------------------------------------
// Initializes the engine with the selected graphics API and window settings.
// -----------------------------------------------------------------------------
bool JellyEngine::Initialize(GraphicsAPIType apiType, const WindowSettings& settings) {
    window = std::make_unique<GLFWindowSystem>();
    window->CreateWindow(settings);

    graphics = GraphicsAPIFactory::Create(apiType);
    if (!graphics) {
        std::cerr << "Unsupported graphics API\n";
        return false;
    }

    graphics->Init();
    return true;
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
// Shuts down the engine and releases window resources.
// -----------------------------------------------------------------------------
void JellyEngine::Shutdown() {
    if (window) {
        window->DestroyWindow();
    }
}
