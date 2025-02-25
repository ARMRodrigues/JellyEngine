﻿using System.Numerics;

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

    public bool HasTransparency { get; private set; } = false;
    public bool HasTexture { get; private set; } = false;

    public Texture Albedo
    {
        get => _albedoTexture;
        set
        {
            _albedoTexture = value;
            HasTexture = true;
        }
    }

    public Material()
    {
        _shader = new Shader();

        Albedo = new Texture();

        _modelLocation = _shader.GetUniformLocation("u_Model");
        _viewLocation = _shader.GetUniformLocation("u_View");
        _projectionLocation = _shader.GetUniformLocation("u_Projection");
        _lightAmbientLocation = _shader.GetUniformLocation("u_Light.ambient");
        _lightDiffuseLocation = _shader.GetUniformLocation("u_Light.diffuse");
        _lightSpecularLocation = _shader.GetUniformLocation("u_Light.specular");
    }

    public Material(Texture albedo)
    {
        _shader = new Shader();

        Albedo = albedo;

        _modelLocation = _shader.GetUniformLocation("u_Model");
        _viewLocation = _shader.GetUniformLocation("u_View");
        _projectionLocation = _shader.GetUniformLocation("u_Projection");
        _lightAmbientLocation = _shader.GetUniformLocation("u_Light.ambient");
        _lightDiffuseLocation = _shader.GetUniformLocation("u_Light.diffuse");
        _lightSpecularLocation = _shader.GetUniformLocation("u_Light.specular");
    }

    public void Use()
    {
        _albedoTexture.Bind();

        _shader.Use();
    }

    public void Unbind()
    {
        _albedoTexture.Unbind();
        _shader.Unbind();
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