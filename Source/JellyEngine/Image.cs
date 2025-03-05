using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace JellyEngine;

public class Image
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public byte[] Data { get; }

    public Image(string path, bool flip = false)
    {
        if (!IsValidPath(path))
            throw new FileNotFoundException("Texture was not found, please check the path", path);

        try
        {
            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(path);
            
            if (flip) image.Mutate(x => x.Flip(FlipMode.Vertical));

            Width = image.Width;
            Height = image.Height;

            Data = new byte[Width * Height * 4];
            image.CopyPixelDataTo(Data);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load image: {ex.Message}");
            throw;
        }
    }

    private static bool IsValidPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;
        
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        var fullPath = Path.Combine(baseDirectory, path);

        return File.Exists(fullPath);
    }

}