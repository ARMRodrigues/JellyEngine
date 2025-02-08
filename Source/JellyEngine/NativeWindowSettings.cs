using System.Numerics;
using JellyEngine.Core.Rendering;

namespace JellyEngine;

public class NativeWindowSettings
{
    public Vector2 Size { get; set; }
    public bool Vsync { get; set; } = true;
    public string Title { get; set; } = "";
    public GraphicsAPI GraphicsAPI { get; set; }

}
