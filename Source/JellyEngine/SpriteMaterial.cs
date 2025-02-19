using System.Numerics;

namespace JellyEngine;

public class SpriteMaterial : IDisposable
{
    private Shader _shader;
    private readonly int _modelLocation;
    private readonly int _viewLocation;
    private readonly int _projectionLocation;
    private readonly int _colorLocation;
    private bool _disposed;

    public SpriteMaterial()
    {
        _shader = new Shader("JellyEngine.Resources.Shaders.2DVertexShader.glsl", "JellyEngine.Resources.Shaders.2DFragmentShader.glsl");
        
        _modelLocation = _shader.GetUniformLocation("model");
        _viewLocation = _shader.GetUniformLocation("view");
        _projectionLocation = _shader.GetUniformLocation("projection");
        _colorLocation = _shader.GetUniformLocation("color");
    }
    
    public void Use()
    {
        _shader.Use();
    }

    public void SetMVP(Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection)
    {
        _shader.SetMatrix4(_modelLocation, model);
        _shader.SetMatrix4(_viewLocation, view);
        _shader.SetMatrix4(_projectionLocation, projection);
    }

    public void SetColor(Color color)
    {
        _shader.SetVector3(_colorLocation, color.ToVector3());
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
            _shader.Dispose();

            _disposed = true;
        }
    }

    ~SpriteMaterial()
    {
        Cleanup();
    }
}