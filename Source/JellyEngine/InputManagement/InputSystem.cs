using System;

namespace JellyEngine.InputManagement;

public class InputSystem
{
    private readonly Dictionary<string, InputAction> actions = new();
    private readonly HashSet<string> currentPressedActions = new();
    private readonly HashSet<string> previousPressedActions = new();

    public void RegisterAction(InputAction inputAction)
    {
        actions[inputAction.Name] = inputAction;
    }

    public bool IsActionPressed(string actionName, KeyboardState keyboardState, MouseState mouseState)
    {
        if (actions.TryGetValue(actionName, out var action))
        {
            return IsAnyInputPressed(action, keyboardState, mouseState);
        }
        return false;
    }

    public bool IsActionJustPressed(string actionName, KeyboardState keyboardState, MouseState mouseState)
    {
        if (actions.TryGetValue(actionName, out var action))
        {
            bool isCurrentlyPressed = IsAnyInputPressed(action, keyboardState, mouseState);

            if (isCurrentlyPressed && !previousPressedActions.Contains(actionName))
            {
                currentPressedActions.Add(actionName);
                return true;
            }
        }
        return false;
    }

    public void Update(KeyboardState keyboardState, MouseState mouseState)
    {

        // Update previous pressed actions
        previousPressedActions.Clear();
        foreach (var actionName in currentPressedActions)
        {
            previousPressedActions.Add(actionName);
        }
        currentPressedActions.Clear();

        foreach (var kvp in actions)
        {
            if (IsAnyInputPressed(kvp.Value, keyboardState, mouseState))
            {
                currentPressedActions.Add(kvp.Key);
            }
        }
    }

    private bool IsAnyInputPressed(InputAction action, KeyboardState keyboardState, MouseState mouseState)
    {
        foreach (var key in action.Keys)
        {
            if (keyboardState.IsKeyPressed(key)) return true;
        }

        foreach (var button in action.MouseButtons)
        {
            if (mouseState.IsButtonPressed(button)) return true;
        }

        return false;
    }
}
