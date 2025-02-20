using System;

namespace JellyEngine.InputManagement;

public class KeyboardState
{
    private readonly HashSet<KeyCode> currentPressedKeys = [];
    private readonly HashSet<KeyCode> previousPressedKeys = [];

    // Update the state of a key
    public void SetKeyPressed(int glfwKey, bool isPressed)
    {
        var key = (KeyCode)glfwKey;

        if (isPressed)
        {
            currentPressedKeys.Add(key);
        }
        else
        {
            currentPressedKeys.Remove(key);
        }
    }

    public void Update()
    {
        previousPressedKeys.Clear();
        foreach (var key in currentPressedKeys)
        {
            previousPressedKeys.Add(key);
        }
    }

    public bool IsKeyPressed(KeyCode key)
    {
        return currentPressedKeys.Contains(key);
    }

    public bool IsKeyReleased(KeyCode key)
    {
        return previousPressedKeys.Contains(key) && !currentPressedKeys.Contains(key);
    }
}

