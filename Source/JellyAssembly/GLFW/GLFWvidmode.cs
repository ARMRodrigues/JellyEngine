namespace JellyAssembly.GLFW;

/// <summary>
/// Represents the video mode of a monitor.
/// </summary>
public struct GLFWvidmode
{
    /// <summary>
    /// The width, in screen coordinates, of the video mode.
    /// </summary>
    public int Width;

    /// <summary>
    /// The height, in screen coordinates, of the video mode.
    /// </summary>
    public int Height;

    /// <summary>
    /// The bit depth of the red channel of the video mode.
    /// </summary>
    public int RedBits;

    /// <summary>
    /// The bit depth of the green channel of the video mode.
    /// </summary>
    public int GreenBits;

    /// <summary>
    /// The bit depth of the blue channel of the video mode.
    /// </summary>
    public int BlueBits;

    /// <summary>
    /// The refresh rate, in Hz, of the video mode.
    /// </summary>
    public int RefreshRate;
}