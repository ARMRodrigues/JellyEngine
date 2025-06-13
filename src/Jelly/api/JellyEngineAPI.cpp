#include "JellyEngineAPI.h"

#include <string>
#include <algorithm>
#include <iostream>

#include "JellyEngine.h"
#include "Graphics/GraphicsAPIType.h"
#include "Graphics/GraphicsApiException.h"

namespace {
    // -----------------------------------------------------------------------------
    // Converts a string to lowercase and returns the corresponding GraphicsAPIType enum.
    // Throws std::invalid_argument if the API is not recognized.
    // -----------------------------------------------------------------------------
    GraphicsAPIType ParseGraphicsAPIType(const char *name) {
        std::string lower{name};
        std::transform(
            lower.begin(), lower.end(), lower.begin(),
            [](unsigned char c)
            { return std::tolower(c);
        });

        if (lower == "vulkan")
            return GraphicsAPIType::Vulkan;
        // Add more options as needed (e.g., OpenGL, DirectX)

        throw GraphicsApiException("Unknown Graphics API: " + lower);
    }
}

// -----------------------------------------------------------------------------
// Creates and initializes a new JellyEngine instance.
// Returns a handle to the engine or nullptr on failure.
// -----------------------------------------------------------------------------
JELLY_API JellyEngineHandle jellyEngineInitialize(int width, int height, bool vsync, const char *title, const char *apiName) {
    WindowSettings settings = {width, height, vsync, title};

    GraphicsAPIType apiNameEnum;
    try
    {
        apiNameEnum = ParseGraphicsAPIType(apiName);
    }
    catch (const std::exception& e)
    {
        return nullptr;
    }

    auto engine = new JellyEngine();

    if (!engine->Initialize(apiNameEnum, settings))
    {
        delete engine;
        return nullptr;
    }

    return engine;
}

// -----------------------------------------------------------------------------
// Returns true if the engine is still running.
// -----------------------------------------------------------------------------
JELLY_API bool jellyEngineIsRunning(JellyEngineHandle handle) {
    auto engine = static_cast<JellyEngine *>(handle);
    return engine->IsRunning();
}

// -----------------------------------------------------------------------------
// Polls window and input events for the engine.
// -----------------------------------------------------------------------------
JELLY_API void jellyEnginePoll(JellyEngineHandle handle) {
    auto engine = static_cast<JellyEngine *>(handle);
    engine->PollEvents();
}

// -----------------------------------------------------------------------------
// Renders a single frame by beginning and ending the graphics API frame.
// -----------------------------------------------------------------------------
JELLY_API void jellyEngineRender(JellyEngineHandle handle) {
    auto engine = static_cast<JellyEngine *>(handle);
    engine->Render();
}

// -----------------------------------------------------------------------------
// Shuts down the engine and releases all associated resources.
// The handle becomes invalid after this call.
// -----------------------------------------------------------------------------
JELLY_API void jellyEngineShutdown(JellyEngineHandle handle) {
    auto engine = static_cast<JellyEngine *>(handle);
    engine->Shutdown();
    delete engine;
}
