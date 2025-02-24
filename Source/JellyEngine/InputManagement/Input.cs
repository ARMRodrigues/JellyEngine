using System.Numerics;

namespace JellyEngine.InputManagement;

public class Input
{
    public static bool SetCursorMode
    {
        set => InputBackend.SetCursor(value);
    }
    public static Vector2 MousePosition => InputSystemManager.Instance.MousePosition;
    public static Vector2 DeltaMousePosition => InputSystemManager.Instance.DeltaMousePosition;

    public static void RegisterAction(InputAction inputAction)
    {
        InputSystemManager.Instance.RegisterAction(inputAction);
    }

    public static void UpdateInputStates(KeyboardState keyboardState, MouseState mouseState)
    {
        InputSystemManager.Instance.UpdateInputStates(keyboardState, mouseState);
    }

    public static void ResetState()
    {
        InputSystemManager.Instance.ResetState();
    }

    public static bool IsActionPressed(string actionName)
    {
        return InputSystemManager.Instance.IsActionPressed(actionName);
    }

    public static bool IsActionJustPressed(string actionName)
    {
        return InputSystemManager.Instance.IsActionJustPressed(actionName);
    }
}