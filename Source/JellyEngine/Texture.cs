using System.Runtime.InteropServices;
using JellyAssembly.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace JellyEngine;

public class Texture
{
    private uint _textureId;
    
    public int Width { get; set; }
    public int Height { get; set; }
    public float PixelsPerUnit { get; set; } = 100.0f;
    
    public Texture()
    {
        LoadTexture("");
    }

    public Texture(string path)
    {
        LoadTexture(path);
    }

    private void LoadTexture(string path)
    {
        _textureId = GL.GenTexture();
        
        Console.WriteLine($"Loading texture {_textureId}");
        
        GL.BindTexture(TextureTarget.Texture2D, _textureId);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        
        if (path != string.Empty && IsValidPath(path))
        {
            using (var image = Image.Load<Rgba32>(path))
            {
                image.Mutate(x => x.Flip(FlipMode.Vertical));
                
                Width = image.Width;
                Height = image.Height;
                
                var pixelData = new byte[Width * Height * 4];
                
                image.CopyPixelDataTo(pixelData);
                
                UploadTextureData(TextureTarget.Texture2D, _textureId, pixelData);
            }
        }
        else
        {
            //Logger.Error("String is empty");
            using (var image = new Image<Rgba32>(2, 2, Color.WhiteSmoke))
            { // Save the image to a file image.Save("empty_red_image.png");
                Width = image.Width;
                Height = image.Height;

                var pixelData = new byte[Width * Height * 4];
                
                image.CopyPixelDataTo(pixelData);
                
                UploadTextureData(TextureTarget.Texture2D, _textureId, pixelData);
 
            }
        }
        
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private void UploadTextureData(TextureTarget target, uint textureId, byte[] data)
    {
        var ptr = Marshal.AllocHGlobal(data.Length);
        // Copy the byte array data to the unmanaged memory
        Marshal.Copy(data, 0, ptr, data.Length);
        GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, Width, Height, 0, GLPixelFormat.Rgba, GLPixelType.UnsignedByte, ptr);
        Marshal.FreeHGlobal(ptr);
    }

    public void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, _textureId);
    }

    public void Unbind()
    {
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public void Dispose()
    {
        GL.DeleteTexture(_textureId);
    }

    private bool IsValidPath(string path)
    {
        try
        {
            // Check if the path is null or empty
            if (string.IsNullOrEmpty(path))
                return false;

            // Check for invalid characters
            foreach (char c in Path.GetInvalidPathChars())
            {
                if (path.Contains(c))
                    return false;
            }

            // Check if the path exists
            return File.Exists(path) || Directory.Exists(path);
        }
        catch (Exception)
        {
            Console.WriteLine("Texture was not found, please check the path");
            return false;
        }
    }
}