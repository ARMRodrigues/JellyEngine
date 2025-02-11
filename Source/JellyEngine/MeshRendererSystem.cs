﻿using System.Numerics;

namespace JellyEngine;

public class MeshRendererSystem(EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Render()
    {
        foreach (var (entity, transform, meshProcessor) in _entityManager.Query<Transform, MeshProcessor>())
        {
            meshProcessor.Material.Use();
            meshProcessor.Material.SetMVP(transform.WorldMatrix, Camera.Main.ViewMatrix, Camera.Main.ProjectionMatrix);
            meshProcessor.Render();
        }
    }

    public override void Shutdown()
    {
        foreach (var (entity, transform, meshProcessor) in _entityManager.Query<Transform, MeshProcessor>())
        {
            _entityManager.GetComponent<MeshProcessor>(entity).Dispose();;
        }
    }
}