using System.Numerics;
using JellyAssembly.OpenGL;

namespace JellyEngine;

public class Material : IDisposable
{
    public string _name;
    private int _modelLocation;
    private int _viewLocation;
    private int _projectionLocation;
    private int _colorLocation;
    private int _shininessLocation;
    private int _viewPositionLocation;
    private int _lightDirectionLocation;
    private int _lightIntensityLocation;
    private int _lightAmbientLocation;
    private int _lightDiffuseLocation;
    private int _lightSpecularLocation;
    private readonly Shader _shader;
    private Texture _albedoTexture;
    private Color _color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private bool _disposed;
    
    public Vector3 Diffuse { get; set; } = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 Specular { get; set; } = new Vector3(1.0f, 1.0f, 1.0f);
    public float Shininess { get; set; } = 16f;
    public Texture Albedo
    {
        get => _albedoTexture;
        set => _albedoTexture = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Color Color
    {
        get => _color;
        set => _color = value;
    }
    
    public Material(string name = "Material")
    {
        _name = name;
        _shader = new Shader();
        _albedoTexture = new Texture();

        GetUniforms();
    }

    public Material(Texture albedo, string name = "Material")
    {
        _name = name;
        _shader = new Shader();
        _albedoTexture = albedo ?? throw new ArgumentNullException(nameof(albedo));
        
        GetUniforms();
    }
    
    public Material(Shader shader, string name = "Material")
    {
        _name = name;
        _shader = shader ?? throw new ArgumentNullException(nameof(shader));
        _albedoTexture = new Texture();
        
        GetUniforms();
    }
    
    public Material(Shader shader, Texture? albedo = null, string name = "Material")
    {
        _name = name;
        _shader = shader ?? throw new ArgumentNullException(nameof(shader));
        _albedoTexture = albedo ?? throw new ArgumentNullException(nameof(albedo));
        
        GetUniforms();
    }

    private void GetUniforms()
    {
        _modelLocation = _shader.GetUniformLocation("u_Model");
        _viewLocation = _shader.GetUniformLocation("u_View");
        _projectionLocation = _shader.GetUniformLocation("u_Projection");
        _colorLocation = _shader.GetUniformLocation("u_Color");
        _shininessLocation = _shader.GetUniformLocation("u_Shininess");
        _viewPositionLocation = _shader.GetUniformLocation("u_ViewPosition");
        _lightDirectionLocation = _shader.GetUniformLocation("u_Light.direction");
        _lightIntensityLocation = _shader.GetUniformLocation("u_Light.intensity");
        _lightAmbientLocation = _shader.GetUniformLocation("u_Light.ambient");
        _lightDiffuseLocation = _shader.GetUniformLocation("u_Light.diffuse");
        _lightSpecularLocation = _shader.GetUniformLocation("u_Light.specular");
    }

    public void Use()
    {
        if (_color.A < 1.0)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }
        else
        {
            GL.Disable(EnableCap.Blend);
        }

        _shader.Use();
        _albedoTexture?.Bind();
        _shader.SetVector4(_colorLocation, _color.ToVector4());
    }

    public void Unbind()
    {
        _albedoTexture.Unbind();
        _shader.Unbind();
    }

    public void SetMatrices(Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection)
    {
        _shader.SetMatrix4(_modelLocation, model);
        _shader.SetMatrix4(_viewLocation, view);
        _shader.SetMatrix4(_projectionLocation, projection);
    }

    public void SetLightData(Vector3 viewPosition, DirectionalLight directionalLight)
    {
        _shader.SetVector3(_viewPositionLocation, viewPosition);
        _shader.SetVector3(_lightDirectionLocation, directionalLight.Direction);
        _shader.SetFloat(_lightIntensityLocation, directionalLight.Intensity);
        _shader.SetVector3(_lightAmbientLocation, directionalLight.Color.ToVector3());
        _shader.SetFloat(_shininessLocation, Shininess);
        _shader.SetVector3(_lightDiffuseLocation, Diffuse);
        _shader.SetVector3(_lightSpecularLocation, Specular);
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
            _albedoTexture?.Dispose();
            _disposed = true;
        }
    }

    ~Material()
    {
        Cleanup();
    }
}