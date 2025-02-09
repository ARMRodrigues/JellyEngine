using System.Numerics;
using JellyEngine.Rendering;

namespace JellyEngine;

public class NativeWindowSettings
{
    public Vector2 Size { get; set; }
    public bool Vsync { get; set; } = true;
    public string Title { get; set; } = "";
    public GraphicsAPI GraphicsAPI { get; set; }

}
