#pragma once

/// Struct containing window creation settings.
struct WindowSettings {
    int width;           ///< Width of the window in pixels.
    int height;          ///< Height of the window in pixels.
    bool vsync;          ///< Whether VSync should be enabled.
    const char* title;   ///< Title of the window.
};
