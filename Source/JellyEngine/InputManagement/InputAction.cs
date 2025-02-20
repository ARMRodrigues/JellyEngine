using System;

namespace JellyEngine.InputManagement;

public class InputAction(string name)
{
    public string Name { get; private set; } = name;
    public List<KeyCode> Keys { get; private set; } = [];
    public List<MouseButton> MouseButtons { get; private set; } = [];

    public void AddKey(KeyCode key)
    {
        if (!Keys.Contains(key))
        {
            Keys.Add(key);
        }
    }

    public bool IsPressed(KeyboardState keyboardState, MouseState mouseState)
    {
        foreach (var key in Keys)
        {
            if (keyboardState.IsKeyPressed(key))
            {
                return true;
            }
        }        

        foreach (var button in MouseButtons)
        {
            if (mouseState.IsButtonPressed(button))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsReleased(KeyboardState keyboardState, MouseState mouseState)
    {
        foreach (var key in Keys)
        {
            if (keyboardState.IsKeyReleased(key))
            {
                return true;
            }
        }

        return false;
    }
}
