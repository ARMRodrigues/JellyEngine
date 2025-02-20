using System;

namespace JellyEngine.InputManagement;

public class MouseState
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int DeltaX { get; private set; }
    public int DeltaY { get; private set; }

    private readonly bool[] buttons = new bool[8];
    private readonly bool[] previousButtons = new bool[8];

    private const int FilterSize = 5;
    private readonly int[] deltaXBuffer = new int[FilterSize];
    private readonly int[] deltaYBuffer = new int[FilterSize];
    private int bufferIndex = 0;

    public void UpdateCursorPosition(double newX, double newY)
    {
        var newXInt = (int)newX;
        var newYInt = (int)newY;

        var rawDeltaX = newXInt - X;
        var rawDeltaY = newYInt - Y;

        deltaXBuffer[bufferIndex] = rawDeltaX;
        deltaYBuffer[bufferIndex] = rawDeltaY;
        bufferIndex = (bufferIndex + 1) % FilterSize;

        DeltaX = (int)deltaXBuffer.Average();
        DeltaY = (int)deltaYBuffer.Average();

        X = newXInt;
        Y = newYInt;
    }

    public void UpdateButtonState(int button, bool isPressed)
    {
        if (button >= 0 && button < buttons.Length)
        {
            previousButtons[button] = buttons[button];
            buttons[button] = isPressed;
        }
    }

    public void ResetMouseDelta()
    {
        DeltaX = 0;
        DeltaY = 0;
    }

    public bool IsButtonPressed(MouseButton button) =>
        button >= 0 && (int)button < buttons.Length && buttons[(int)button];

    public bool IsButtonJustPressed(int button) =>
        button >= 0 && button < buttons.Length && buttons[button] && !previousButtons[button];

    public bool IsButtonJustReleased(int button) =>
        button >= 0 && button < buttons.Length && !buttons[button] && previousButtons[button];
}
