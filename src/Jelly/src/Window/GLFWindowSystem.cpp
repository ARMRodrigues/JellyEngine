#include "Window/GLFWindowSystem.h"

#include <cstdio>
#include <cstdlib>
#include <iostream>

#include "Logger.h"

// -----------------------------------------------------------------------------
// GLFW error callback.
// -----------------------------------------------------------------------------
static void ErrorCallback(int error, const char* description) {
    std::fprintf(stderr, "GLFW error %d: %s\n", error, description);
}

// -----------------------------------------------------------------------------
// Creates the window and initialises GLFW.
// -----------------------------------------------------------------------------
void GLFWindowSystem::CreateWindow(const WindowSettings& settings) {
    glfwSetErrorCallback(ErrorCallback);

    if (!glfwInit()) {
        Logger::Log(LogLevel::Error, "GLFW initialisation failed");
        std::exit(EXIT_FAILURE);
    }

    glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);

    window = glfwCreateWindow (
        settings.width,
        settings.height,
        settings.title,
        nullptr,
        nullptr
    );

    if (!window) {
        glfwTerminate();
        Logger::Log(LogLevel::Error, "Window creation failed");
        std::exit(EXIT_FAILURE);
    }
    
    Logger::Log (
        LogLevel::Highlight, 
        "Window created: " + std::string(settings.title) +
        " (" + std::to_string(settings.width) + "x" + std::to_string(settings.height) + ")"
    );
}

// -----------------------------------------------------------------------------
// Indicates whether the main window should remain open.
// -----------------------------------------------------------------------------
bool GLFWindowSystem::IsWindowOpen() {
    return window && !glfwWindowShouldClose(window);
}

// -----------------------------------------------------------------------------
// Polls platform events (keyboard, mouse, window resize, …).
// -----------------------------------------------------------------------------
void GLFWindowSystem::PollEvents() {
    glfwPollEvents();
}


// -----------------------------------------------------------------------------
// Destroys the current window and releases associated resources.
// -----------------------------------------------------------------------------
void GLFWindowSystem::DestroyWindow() {
    glfwDestroyWindow(window);
    glfwTerminate();
}
