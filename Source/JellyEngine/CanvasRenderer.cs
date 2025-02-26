using System.Numerics;
using JellyAssembly.OpenGL;

namespace JellyEngine;

public class CanvasRenderer : GameComponent, IDisposable
{
    private Sprite _sprite;
    private uint _vao;
    private uint _vbo;
    private uint _ebo;
    private int _indicesSize;
    private bool _disposed = false;

    public SpriteMaterial Material { get; }

    public Color Color => _sprite.Color;

    public CanvasRenderer(Sprite sprite)
    {
        _sprite = sprite;
        Material = new SpriteMaterial();

        Initialize();
    }

    private void Initialize()
    {
        var size = _sprite.Size;
        var pixelsPerUnit = _sprite.PixelsPerUnit;
        CreateQuad((size.X / 2f) / pixelsPerUnit,
        (size.Y / 2f) / pixelsPerUnit);
    }

    private void CreateQuad(float width, float height)
    {
        float[] vertices =
        [
            // Positions         // UVs
            -width, -height, 0,  0.0f, 0.0f,  // Bottom left
             width, -height, 0,  1.0f, 0.0f,  // Bottom right
             width,  height, 0,  1.0f, 1.0f,  // Top right
            -width,  height, 0,  0.0f, 1.0f   // Top left
        ];

        uint[] indices = [0, 1, 3, 1, 2, 3];

        _indicesSize = indices.Length;

        _vao = GL.GenVertexArrays();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffers();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        _ebo = GL.GenBuffers();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        const int stride = 5 * sizeof(float);

        // Positions (location 0)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
        GL.EnableVertexAttribArray(0);

        // UVs (location 1)
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
    }

    public void PrepareRender()
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendFuncSeparate(
            BlendingFactorSrc.SrcAlpha,
            BlendingFactorDest.OneMinusSrcAlpha,
            BlendingFactorSrc.One,
            BlendingFactorDest.OneMinusSrcAlpha
        );
        GL.BlendEquationSeparate(BlendEquationMode.FuncAdd, BlendEquationMode.FuncAdd);
        GL.DepthFunc(DepthFunction.Always);
        GL.DepthMask(true);
        GL.Disable(EnableCap.CullFace);
        _sprite.Texture.Bind();
    }

    public void Render()
    {
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _indicesSize, DrawElementsType.UnsignedInt, 0);
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

    ~CanvasRenderer()
    {
        Cleanup();
    }
}