using System.Numerics;

namespace JellyEngine;

public class Sprite
{
    public Texture Texture { get; set; }
    public float PixelsPerUnit { get; set; }
    public Vector2 Size => new(Texture.Width, Texture.Height);
    public bool HorizontalFlip { get; set; }
    public bool VerticalFlip { get; set; }
    public Color Color { get; set; } = Color.White;

    public Sprite()
    {
        Texture = new Texture();
        PixelsPerUnit = 100;
    }
    
    public Sprite(string filename)
    {
        Texture = new Texture(filename);
        PixelsPerUnit = 100;
    }

    public Sprite(Texture texture, float pixelsPerUnit)
    {
        Texture = texture;
        pixelsPerUnit = pixelsPerUnit;
    }
}