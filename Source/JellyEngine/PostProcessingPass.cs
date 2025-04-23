using JellyAssembly.OpenGL;

namespace JellyEngine.Rendering.OpenGL;

public class PostProcessingPass
{
    private readonly float[] _quadVertices =
    {
        // Posições         // Coordenadas de textura
        -1.0f,  1.0f, 0.0f,  0.0f, 1.0f,
        -1.0f, -1.0f, 0.0f,  0.0f, 0.0f,
         1.0f, -1.0f, 0.0f,  1.0f, 0.0f,

         1.0f, -1.0f, 0.0f,  1.0f, 0.0f,
         1.0f,  1.0f, 0.0f,  1.0f, 1.0f,
        -1.0f,  1.0f, 0.0f,  0.0f, 1.0f
    };

    private uint _quadVAO;
    private uint _quadVBO;
    private Shader _shader;

    public PostProcessingPass()
    {
        InitializeQuad();
    }

    private void InitializeQuad()
    {
        _quadVAO = GL.GenVertexArrays();
        GL.BindVertexArray(_quadVAO);

        _quadVBO = GL.GenBuffers();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _quadVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, _quadVertices.Length * sizeof(float), _quadVertices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        _shader = new Shader(
            "JellyEngine.Resources.Shaders.FramebufferVertex.glsl",
            "JellyEngine.Resources.Shaders.FramebufferFragment.glsl");
    }

    public void Render(uint textureColorBuffer)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _shader.Use();
        GL.BindVertexArray(_quadVAO);
        GL.BindTexture(TextureTarget.Texture2D, textureColorBuffer);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public void Dispose()
    {
        GL.DeleteBuffers(1, _quadVBO);
        GL.DeleteVertexArrays(1, _quadVAO);
        _shader.Dispose();
    }
}
