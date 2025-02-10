using System.Numerics;

namespace JellyEngine;

public class Material : IDisposable
{
    private readonly Shader _shader;
    private Texture _albedoTexture;

    private readonly int _modelLocation;
    private readonly int _viewLocation;
    private readonly int _projectionLocation;

    private bool _disposed;

    private readonly int _lightAmbientLocation;
    private readonly int _lightDiffuseLocation;
    private readonly int _lightSpecularLocation;

    public Texture Albedo
    {
        get => _albedoTexture;
        set
        {
            _albedoTexture = value;
        }
    }

    public Material()
    {
        _shader = new Shader();

        _albedoTexture = new Texture();

        _modelLocation = _shader.GetUniformLocation("model");
        _viewLocation = _shader.GetUniformLocation("view");
        _projectionLocation = _shader.GetUniformLocation("projection");
        _lightAmbientLocation = _shader.GetUniformLocation("light.ambient");
        _lightDiffuseLocation = _shader.GetUniformLocation("light.diffuse");
        _lightSpecularLocation = _shader.GetUniformLocation("light.specular");
    }

    public Material(Texture albedo)
    {
        _shader = new Shader();

        _albedoTexture = albedo;

        _modelLocation = _shader.GetUniformLocation("model");
        _viewLocation = _shader.GetUniformLocation("view");
        _projectionLocation = _shader.GetUniformLocation("projection");
        _lightAmbientLocation = _shader.GetUniformLocation("light.ambient");
        _lightDiffuseLocation = _shader.GetUniformLocation("light.diffuse");
        _lightSpecularLocation = _shader.GetUniformLocation("light.specular");
    }

    public void Use()
    {
        _albedoTexture.Bind();

        _shader.Use();
    }

    public void SetMVP(Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection)
    {
        _shader.SetVector3(_lightAmbientLocation, new Vector3(1.0f, 0.94f, 0.76f));
        _shader.SetVector3(_lightDiffuseLocation, new Vector3(0.5f, 0.5f, 0.5f));
        _shader.SetVector3(_lightSpecularLocation, new Vector3(1.0f, 1.0f, 1.0f));

        _shader.SetMatrix4(_modelLocation, model);
        _shader.SetMatrix4(_viewLocation, view);
        _shader.SetMatrix4(_projectionLocation, projection);
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
            _albedoTexture.Dispose();

            _disposed = true;
        }
    }

    ~Material()
    {
        Cleanup();
    }
}