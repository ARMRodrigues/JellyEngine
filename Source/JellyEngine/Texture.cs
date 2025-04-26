using System.Runtime.InteropServices;
using JellyAssembly.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace JellyEngine;

public enum WrapMode
{
    Repeat = 0,
    Clamp = 1,
    Mirror = 2,
    MirrorOnce = 3
}

public enum FilterMode
{
    Point = 0,
    Bilinear = 1,
    Trilinear = 2
}

public class Texture : IDisposable
{
    private uint _textureId;
    private bool _disposed = false;
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    private FilterMode _filterMode = FilterMode.Bilinear;
    private WrapMode _wrapMode = WrapMode.Repeat;

    public WrapMode WrapMode
    {
        get => _wrapMode;
        set
        {
            if (_wrapMode != value)
            {
                _wrapMode = value;
                UpdateWrapSettings();
            }
        }
    }
    
    public FilterMode FilterMode
    {
        get => _filterMode;
        set
        {
            if (_filterMode != value)
            {
                _filterMode = value;
                UpdateFilterSettings();
            }
        }
    }

    private byte[] _pixelData = [];

    public Texture()
    {
        CreateDefaultTexture();
    }

    public Texture(string path)
    {
        try
        {
            var image = new Image(path, flip: true);
            Width = image.Width;
            Height = image.Height;
            _pixelData = image.Data;
            LoadTextureFromPixelData();
        }
        catch (Exception)
        {
            CreateDefaultTexture();
        }
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

    private void CreateDefaultTexture()
    {
        Width = 1;
        Height = 1;
        _pixelData = [255, 255, 255, 255];
        LoadTextureFromPixelData();
    }

    private void LoadTextureFromPixelData()
    {
        _textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _textureId);
        ApplyTextureSettings();
        UploadTextureData(TextureTarget.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private void ApplyTextureSettings()
    {
        var wrapMode = GetWrapMode(WrapMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);

        var (minFilter, magFilter) = GetFilterMode(FilterMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

        if (FilterMode == FilterMode.Trilinear)
        {
            GL.GenerateMipmap(TextureTarget.Texture2D);
        }
    }

    private static TextureWrapMode GetWrapMode(WrapMode mode) => mode switch
    {
        WrapMode.Repeat => TextureWrapMode.Repeat,
        WrapMode.Clamp => TextureWrapMode.ClampToEdge,
        WrapMode.Mirror => TextureWrapMode.MirroredRepeat,
        WrapMode.MirrorOnce => TextureWrapMode.MirrorClampToEdge,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Invalid wrap mode")
    };

    private static (TextureMinFilter min, TextureMagFilter mag) GetFilterMode(FilterMode mode) => mode switch
    {
        FilterMode.Point => (TextureMinFilter.Nearest, TextureMagFilter.Nearest),
        FilterMode.Bilinear => (TextureMinFilter.Linear, TextureMagFilter.Linear),
        FilterMode.Trilinear => (TextureMinFilter.LinearMipmapLinear, TextureMagFilter.Linear),
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Invalid filter mode")
    };
    
    private void UpdateFilterSettings()
    {
        GL.BindTexture(TextureTarget.Texture2D, _textureId);
        
        var (minFilter, magFilter) = GetFilterMode(FilterMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

        if (FilterMode == FilterMode.Trilinear)
        {
            GL.GenerateMipmap(TextureTarget.Texture2D);
        }
        
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private void UpdateWrapSettings()
    {
        GL.BindTexture(TextureTarget.Texture2D, _textureId);

        var wrapMode = GetWrapMode(WrapMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);

    }

    private void UploadTextureData(TextureTarget target)
    {
        var ptr = Marshal.AllocHGlobal(_pixelData.Length);
        Marshal.Copy(_pixelData, 0, ptr, _pixelData.Length);
        GL.TexImage2D(target, 0, PixelInternalFormat.Rgba, Width, Height, 0, GLPixelFormat.Rgba, GLPixelType.UnsignedByte, ptr);
        Marshal.FreeHGlobal(ptr);
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

    public void Bind() => GL.BindTexture(TextureTarget.Texture2D, _textureId);
    public void Unbind() => GL.BindTexture(TextureTarget.Texture2D, 0);

    public void Dispose()
    {
        Cleanup();
        GC.SuppressFinalize(this);
    }

    private void Cleanup()
    {
        if (!_disposed)
        {
            GL.DeleteTexture(_textureId);
            _disposed = true;
        }
    }

    ~Texture()
    {
        Cleanup();
    }
}
