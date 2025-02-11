using System.Numerics;
using System.Reflection;
using JellyAssembly.OpenGL;

namespace JellyEngine;

public class Shader : IDisposable
{
     public string VertexSource { get; private set; }
    public string FragmentSource { get; private set; }
    public uint ShaderProgram { get; private set; }

    private bool _disposed;

    public Shader()
    {
        VertexSource = LoadShaderFilePath("JellyEngine.Resources.Shaders.DefaultVertexShader.glsl");
        FragmentSource = LoadShaderFilePath("JellyEngine.Resources.Shaders.DefaultFragmentShader.glsl");

        Initialize();
    }
    
    public Shader(string vertexPath, string fragmentPath)
    {
        VertexSource = LoadShaderFilePath(vertexPath);
        FragmentSource = LoadShaderFilePath(fragmentPath);

        Initialize();
    }

    private void Initialize()
    {
        // Create and compile vertex shader
        var vertexShader = CompileShader(ShaderType.VertexShader, VertexSource);

        // Create and compile fragment shader
        var fragmentShader = CompileShader(ShaderType.FragmentShader, FragmentSource);

        // Create shader program and attach shaders
        ShaderProgram = GL.CreateProgram();
        GL.AttachShader(ShaderProgram, vertexShader);
        GL.AttachShader(ShaderProgram, fragmentShader);
        GL.LinkProgram(ShaderProgram);
        var result = GL.GetProgramiv(ShaderProgram, GetProgramParameterName.LinkStatus);
        if (result <= 0)
        {
            var error = GL.GetProgramInfoLog(ShaderProgram, 2048);
            Console.WriteLine($"The shader result was: {error}");
        }

        // Delete the shaders (they are now linked into the program and no longer needed)
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);
    }

    private static uint CompileShader(ShaderType type, string source)
    {
        var shader = GL.CreateShader(type);
        GL.ShaderSource(shader, 1, source, IntPtr.Zero);
        GL.CompileShader(shader);

        var result = GL.GetShaderiv(shader, ShaderParameter.CompileStatus);
        if (result == 0)
        {
            var infoLog = GL.GetShaderInfoLog(shader, 2048);
            //throw new Exception($"Error compiling {type}: {infoLog}");
            Console.WriteLine($"Error compiling {type}: {infoLog}");
        }

        return shader;
    }

    public void Use()
    {

        GL.UseProgram(ShaderProgram);
    }

    public int GetUniformLocation(string uniformName)
    {
        var uniform = GL.GetUniformLocation(ShaderProgram, uniformName);

        if (uniform < 0)
        {
            Console.WriteLine($"ERROR: {uniformName} uniform not found in shader");
        }

        return uniform;
    }

    public void SetVector3(int uniformLocation, Vector3 value)
    {
        GL.Uniform3f(uniformLocation, value);
    }

    public void SetMatrix4(int uniformLocation, Matrix4x4 matrix)
    {
        GL.UniformMatrix4fv(uniformLocation, 1, false, MathUtils.ToOpenGLMatrixArray( matrix ));
    }

    private static string LoadShaderFilePath(string path)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(path);

        if (stream == null)
        {
            throw new FileNotFoundException("Shader resource not found.");
        }

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
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
            GL.DeleteProgram(ShaderProgram);

            _disposed = true;
        }
    }

    ~Shader()
    {
        Cleanup();
    }
}