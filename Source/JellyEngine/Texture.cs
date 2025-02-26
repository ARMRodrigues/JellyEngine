using System.Runtime.InteropServices;
using JellyAssembly.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace JellyEngine;

public class Texture
{
    private uint _textureId;
    private byte[] _pixelData = [];
    
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public Texture()
    {
        LoadTexture("");
    }

    public Texture(string path)
    {
        LoadTexture(path);
    }

    public Texture(int width, int height, Color[] pixels)
    {
        Width = width;
        Height = height;
        
        _pixelData = new byte[Width * Height * 4];
        
        for (var i = 0; i < pixels.Length; i++)
        {
            _pixelData[i * 4] = (byte)(pixels[i].R * 255);
            _pixelData[i * 4 + 1] = (byte)(pixels[i].G * 255);
            _pixelData[i * 4 + 2] = (byte)(pixels[i].B * 255);
            _pixelData[i * 4 + 3] = (byte)(pixels[i].A * 255);
        }

        LoadTextureFromPixelData();
        
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private void LoadTexture(string path)
    {
        _textureId = GL.GenTexture();
        
        GL.BindTexture(TextureTarget.Texture2D, _textureId);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        
        if (path != string.Empty && IsValidPath(path))
        {
            using var image = Image.Load<Rgba32>(path);
            
            image.Mutate(x => x.Flip(FlipMode.Vertical));
                
            Width = image.Width;
            Height = image.Height;
                
            _pixelData = new byte[Width * Height * 4];
                
            image.CopyPixelDataTo(_pixelData);
                
            UploadTextureData(TextureTarget.Texture2D);
        }
        else
        {
            using var image = new Image<Rgba32>(64, 64, SixLabors.ImageSharp.Color.WhiteSmoke);
            
            Width = image.Width;
            Height = image.Height;

            _pixelData = new byte[Width * Height * 4];
                
            image.CopyPixelDataTo(_pixelData);
                
            UploadTextureData(TextureTarget.Texture2D);
        }
        
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }
    
    private void LoadTextureFromPixelData()
    {
        _textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _textureId);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        UploadTextureData(TextureTarget.Texture2D);

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private void UploadTextureData(TextureTarget target)
    {
        var ptr = Marshal.AllocHGlobal(_pixelData.Length);
        Marshal.Copy(_pixelData, 0, ptr, _pixelData.Length);
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
    
    public void SaveToDisk(string filePath)
    {
        using var image = new Image<Rgba32>(Width, Height);
        
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var index = (y * Width + x) * 4;
                var r = _pixelData[index];
                var g = _pixelData[index + 1];
                var b = _pixelData[index + 2];
                var a = _pixelData[index + 3];
                image[x, y] = new Rgba32(r, g, b, a);
            }
        }
            
        image.Save(filePath);
    }

    public void Dispose()
    {
        GL.DeleteTexture(_textureId);
    }

    private bool IsValidPath(string path)
    {
        try
        {
            if (string.IsNullOrEmpty(path))
                return false;
            
            if (Path.GetInvalidPathChars().Any(path.Contains))
            {
                return false;
            }
            
            return File.Exists(path) || Directory.Exists(path);
        }
        catch (Exception)
        {
            Console.WriteLine("Texture was not found, please check the path");
            return false;
        }
    }
}