using System.Numerics;
using System.Reflection;
using JellyAssembly.OpenGL;

namespace JellyEngine;

public class Shader : IDisposable
{
    private uint _shaderProgram;
    private readonly string _vertexSource;
    private readonly string _fragmentSource;
    private bool _disposed;

    public Shader()
    {
        _vertexSource = LoadShaderFilePath("JellyEngine.Resources.Shaders.DefaultVertexShader.glsl");
        _fragmentSource = LoadShaderFilePath("JellyEngine.Resources.Shaders.DefaultFragmentShader.glsl");

        Initialize();
    }
    
    public Shader(string vertexPath, string fragmentPath)
    {
        _vertexSource = LoadShaderFilePath(vertexPath);
        _fragmentSource = LoadShaderFilePath(fragmentPath);

        Initialize();
    }

    private void Initialize()
    {
        var vertexShader = CompileShader(ShaderType.VertexShader, _vertexSource);
        var fragmentShader = CompileShader(ShaderType.FragmentShader, _fragmentSource);

        _shaderProgram = GL.CreateProgram();
        GL.AttachShader(_shaderProgram, vertexShader);
        GL.AttachShader(_shaderProgram, fragmentShader);
        GL.LinkProgram(_shaderProgram);
        var result = GL.GetProgramiv(_shaderProgram, GetProgramParameterName.LinkStatus);
        if (result <= 0)
        {
            var error = GL.GetProgramInfoLog(_shaderProgram, 2048);
            Console.WriteLine($"The shader result was: {error}");
        }
        
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
        GL.UseProgram(_shaderProgram);
    }

    public void Unbind()
    {
        GL.UseProgram(0);
    }

    public int GetUniformLocation(string uniformName)
    {
        var uniform = GL.GetUniformLocation(_shaderProgram, uniformName);

        if (uniform < 0)
        {
            Console.WriteLine($"ERROR: {uniformName} uniform not found in shader");
        }

        return uniform;
    }

    public void SetFloat(int uniformLocation, float value)
    {
        GL.Uniform1f(uniformLocation, value);
    }

    public void SetVector3(int uniformLocation, Vector3 value)
    {
        GL.Uniform3f(uniformLocation, value);
    }
    
    public void SetVector4(int uniformLocation, Vector4 value)
    {
        GL.Uniform4f(uniformLocation, value);
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
            GL.DeleteProgram(_shaderProgram);

            _disposed = true;
        }
    }

    ~Shader()
    {
        Cleanup();
    }
}