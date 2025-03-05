using System.Numerics;
using System.Runtime.InteropServices;
using JellyAssembly.OpenGL;

namespace JellyEngine;

public class SceneEnvironmentRendererSystem : GameSystem
{
    private uint _vao, _vbo, _skyboxTexture;
    private int _viewMatrixUniformLocation, _projectionMatrixUniformLocation;

    public override void Initialize()
    {
        PrepareSkyboxMesh();
        LoadSkyboxTextures();

        var shader = SceneEnvironment.Main.Shader;
        _viewMatrixUniformLocation = shader.GetUniformLocation("u_View");
        _projectionMatrixUniformLocation = shader.GetUniformLocation("u_Projection");
    }

    public override void Render()
    {
        GL.DepthMask(false);
        GL.DepthFunc(DepthFunction.Lequal);

        var shader = SceneEnvironment.Main.Shader;
        shader.Use();

        var viewMatrix = Camera.Main.ViewMatrix;
        viewMatrix.M14 = viewMatrix.M24 = viewMatrix.M34 = 0;

        shader.SetMatrix4(_viewMatrixUniformLocation, viewMatrix);
        shader.SetMatrix4(_projectionMatrixUniformLocation, Camera.Main.ProjectionMatrix);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.TextureCubeMap, _skyboxTexture);

        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        GL.BindVertexArray(0);

        GL.UseProgram(0);
        GL.DepthMask(true);
        GL.DepthFunc(DepthFunction.Less);
    }

    private void PrepareSkyboxMesh()
    {
        float[] vertices = {
            // Back face (Z negativo)
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,

            // Front face (Z positivo)
            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            // Left face (X negativo)
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,

            // Right face (X positivo)
            1.0f,  1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f, -1.0f,

            // Bottom face (Y negativo)
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,

            // Top face (Y positivo)
            -1.0f,  1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f
        };

        _vao = GL.GenVertexArrays();
        _vbo = GL.GenBuffers();

        GL.BindVertexArray(_vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.BindVertexArray(0);
    }

    private void LoadSkyboxTextures()
    {
        _skyboxTexture = GL.GenTexture();
        GL.BindTexture(TextureTarget.TextureCubeMap, _skyboxTexture);

        var skybox = SceneEnvironment.Main.Skybox;

        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

        LoadImage(skybox.Right, 0);
        LoadImage(skybox.Left, 1);
        LoadImage(skybox.Top, 2);
        LoadImage(skybox.Bottom, 3);
        LoadImage(skybox.Front, 4);
        LoadImage(skybox.Back, 5);

        GL.BindTexture(TextureTarget.TextureCubeMap, 0);
    }

    private static void LoadImage(Image image, int index)
    {
        var ptr = Marshal.AllocHGlobal(image.Data.Length);
        Marshal.Copy(image.Data, 0, ptr, image.Data.Length);

        GL.TexImage2D(
            TextureTarget.TextureCubeMapPositiveX + index, 0,
            PixelInternalFormat.Rgba,
            image.Width, image.Height, 0,
            GLPixelFormat.Rgba, GLPixelType.UnsignedByte, ptr);

        Marshal.FreeHGlobal(ptr);
    }

    public override void Shutdown()
    {
        GL.DeleteTexture(_skyboxTexture);
    }
}
