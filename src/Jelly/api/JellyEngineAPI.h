#pragma once

#include "JellyExport.h"
#include "JellyTypes.h"
#include "JellyEngine.h"
#include "Window/WindowSettings.h"

JELLY_API_BEGIN

// Creates and initializes a new JellyEngine instance.
JELLY_API JellyEngineHandle jellyEngineInitialize(int width, int height, bool vsync, const char* title, const char* apiName);

// Checks if the engine is still running.
JELLY_API bool jellyEngineIsRunning(JellyEngineHandle handle);

// Polls input and window events.
JELLY_API void jellyEnginePoll(JellyEngineHandle handle);

// Shuts down the engine and releases all associated resources.
JELLY_API void jellyEngineShutdown(JellyEngineHandle handle);

JELLY_API_END
