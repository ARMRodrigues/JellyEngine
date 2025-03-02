using JellyAssembly.OpenGL;

namespace JellyEngine;

public class MeshRenderer : Renderer, IDisposable
{
    private uint _vao, _vbo, _ebo;
    private int _indicesSize;
    private bool _disposed = false;
    private Material _material;
    
    public Material Material => _material;

    public MeshRenderer(Mesh mesh)
    {
        InitializeComp(mesh);
        _material = new Material();
    }
    
    public MeshRenderer(Mesh mesh, Material material)
    {
        InitializeComp(mesh);
        _material = material;
    }

    private void InitializeComp(Mesh mesh)
    {
        _indicesSize = mesh.Indices.Length;
        _vao = GL.GenVertexArrays();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffers();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, mesh.Vertices.Length, mesh.Vertices, BufferUsageHint.StaticDraw);

        _ebo = GL.GenBuffers();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indicesSize, mesh.Indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 18 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // positions
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 18 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // normals
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 18 * sizeof(float), (3 * sizeof(float)));
        GL.EnableVertexAttribArray(1);

        // tangent
        GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 18 * sizeof(float), (6 * sizeof(float))); // 4 floats for tangent (x, y, z, w)
        GL.EnableVertexAttribArray(2);

        // color
        GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 18 * sizeof(float), (10 * sizeof(float))); // 4 floats for color (RGBA)
        GL.EnableVertexAttribArray(3);

        // UV0 (location 4)
        GL.VertexAttribPointer(4, 2, VertexAttribPointerType.Float, false, 18 * sizeof(float), (14 * sizeof(float))); // 2 floats for UV0 (main UV)
        GL.EnableVertexAttribArray(4);

        // UV1 (location 5)
        GL.VertexAttribPointer(5, 2, VertexAttribPointerType.Float, false, 18 * sizeof(float), (16 * sizeof(float))); // 2 floats for UV1 (secondary UV)
        GL.EnableVertexAttribArray(5);

        GL.BindVertexArray(0);
    }

    public void BeginRender()
    {
        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(DepthFunction.Lequal);
        
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Back);

        if (Material.HasTransparency)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }
        
        Material.Use();
    }

    private void EndRender()
    {
        Material.Unbind();

        if (Material.HasTransparency)
            GL.Disable(EnableCap.Blend);
    }

    public void Render()
    {
        BeginRender();
        
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _indicesSize, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
        
        EndRender();
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

    ~MeshRenderer()
    {
        Cleanup();
    }
}
