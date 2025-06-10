#pragma once

#include <GLFW/glfw3.h>

#include "IWindowSystem.h"

/// GLFW-based implementation of IWindowSystem.
class GLFWindowSystem final : public IWindowSystem {
public:
    void CreateWindow(const WindowSettings& settings) override;
    bool IsWindowOpen() override;
    void PollEvents() override;
    void DestroyWindow() override;
    
private:
    GLFWwindow* window = nullptr; ///< Pointer to the GLFW window instance.
};
