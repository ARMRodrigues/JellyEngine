using System.Numerics;
using JellyEngine.InputManagement;

public sealed class InputSystemManager
{
    // Instância única do Singleton
    private static readonly InputSystemManager instance = new InputSystemManager();
    public static InputSystemManager Instance => instance;

    private readonly InputSystem inputSystem;
    private KeyboardState currentKeyboardState;
    private MouseState currentMouseState;
    
    private InputSystemManager()
    {
        inputSystem = new InputSystem();
    }

    public Vector2 MousePosition => new Vector2(currentMouseState.X, currentMouseState.Y);
    public Vector2 DeltaMousePosition => new Vector2(currentMouseState.DeltaX, currentMouseState.DeltaY);

    public void RegisterAction(InputAction inputAction)
    {
        inputSystem.RegisterAction(inputAction);
    }

    public void UpdateInputStates(KeyboardState keyboardState, MouseState mouseState)
    {
        currentKeyboardState = keyboardState;
        currentMouseState = mouseState;

        inputSystem.Update(keyboardState, mouseState);
    }

    public void ResetState()
    {
        currentMouseState?.ResetMouseDelta();
    }

    public bool IsActionPressed(string actionName)
    {
        return inputSystem.IsActionPressed(actionName, currentKeyboardState, currentMouseState);
    }

    public bool IsActionJustPressed(string actionName)
    {
        return inputSystem.IsActionJustPressed(actionName, currentKeyboardState, currentMouseState);
    }
}