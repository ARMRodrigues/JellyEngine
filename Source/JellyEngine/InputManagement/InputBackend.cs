using System;
using JellyAssembly.GLFW;

namespace JellyEngine.InputManagement;

public class InputBackend
{
    private static IntPtr _windowHandle;
    private static readonly KeyboardState keyboardState = new();
    private static readonly MouseState mouseState = new();

    public static void InitializeCallbacks(IntPtr window)
    {
        _windowHandle = window;

        GLFW.SetKeyCallback(window, KeyCallback);

        GLFW.SetMouseButtonCallback(window, MouseButtonCallback);

        GLFW.SetCursorPositionCallback(window, CursorPositionCallback);
    }

    private static void KeyCallback(IntPtr window, int key, int scancode, int action, int mods)
    {
        bool isPressed = action == 1 || action == 2; // GLFW_PRESS or GLFW_REPEAT
        keyboardState.SetKeyPressed(key, isPressed);
    }
    
    private static void MouseButtonCallback(IntPtr window, int button, int action, int mods)
    {
        bool isPressed = action == 1 || action == 2;
        mouseState.UpdateButtonState(button, isPressed);
    }

    private static void CursorPositionCallback(IntPtr window, double xpos, double ypos)
    {
        mouseState.UpdateCursorPosition(xpos, ypos);
    }

    public static KeyboardState GetKeyboardState() => keyboardState;

    public static MouseState GetMouseState() => mouseState;

    public static void SetCursor(bool value)
    {
        if (value)
        {
            GLFW.SetInputMode(_windowHandle, InputMode.Cursor, CursorMode.Normal);
        }
        else
        {
            GLFW.SetInputMode(_windowHandle, InputMode.Cursor, CursorMode.Disabled);
        }        
    }
}
