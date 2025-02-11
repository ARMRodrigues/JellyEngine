using System.Numerics;
using JellyAssembly.OpenGL;

namespace JellyEngine;

public class SpriteRenderer : GameComponent, IDisposable
{
    private Sprite _sprite;
    private uint _vao;
    private uint _vbo;
    private uint _ebo;
    private int _indicesSize;
    private bool _disposed = false;
    
    public SpriteMaterial Material { get; }
    
    public SpriteRenderer(Sprite sprite)
    {
        _sprite = sprite;
        Material = new SpriteMaterial();
        Initialize();
    }

    private void Initialize()
    {
        var size = _sprite.Size;
        var pixelsPerUnit = _sprite.PixelsPerUnit;
        CreateQuad((size.X/2f) / pixelsPerUnit, 
        (size.Y/2f) / pixelsPerUnit);
    }
    
    public void CreateQuad(float width, float height)
    {
        float[] vertices =
        [
            // Position          Texture coordinates 
            width, height, 0, 1.0f, 1.0f, // Top right
            width, -height, 0, 1.0f, 0.0f, // Bottom right
            -width, -height, 0, 0.0f, 0.0f, // Bottom left
            -width, height, 0, 0.0f, 1.0f // Top left
        ];
        
        var uv = new List<Vector2>
        {
            new(0.0f, 0.0f), // Bottom left
            new(1.0f, 0.0f), // Bottom right
            new(1.0f, 1.0f), // Top right
            new(0.0f, 1.0f)  // Top left
        };

        uint[] indices =
        [
            0, 1, 3, // First triangle
            1, 2, 3 // Second triangle
        ];
        
        _indicesSize = indices.Length;
        
        _vao = GL.GenVertexArrays();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffers();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length, vertices, BufferUsageHint.StaticDraw);

        _ebo = GL.GenBuffers();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indicesSize, indices, BufferUsageHint.StaticDraw);

        // positions (location 2)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        // UV0 (location 1)
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (3 * sizeof(float))); // 2 floats for UV0 (main UV)
        GL.EnableVertexAttribArray(1);
    }
    
    public void Render()
    {
        _sprite.Texture.Bind();
        
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _indicesSize, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
        
        _sprite.Texture.Unbind();
    }

    public void Dispose()
    {
        Cleanup();
        GC.SuppressFinalize(this);
    }

    private void Cleanup()
    {
        if (!_disposed)
        {
            GL.DeleteBuffers(1, _vbo);
            GL.DeleteBuffers(1, _ebo);
            GL.DeleteVertexArrays(1, _vao);

            _disposed = true;
        }
    }

    ~SpriteRenderer()
    {
        Cleanup();
    }
}