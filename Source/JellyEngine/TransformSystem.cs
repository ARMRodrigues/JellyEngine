using System.Numerics;

namespace JellyEngine;

public class TransformSystem (EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Update()
    {
        foreach (var transform in _entityManager.GetComponents<Transform>())
        {
            transform.WorldMatrix = transform.LocalMatrix;
        }
        
        foreach (var (entity, transform) in _entityManager.Query<Transform>())
        {
            if (!_entityManager.TryGetComponent(entity, out Hierarchy? children)) continue;
            
            foreach (var childId in children.ChildrenId)
            {
                var childTransform = _entityManager.GetComponent<Transform>(new Entity(childId));
                childTransform.WorldMatrix = childTransform.LocalMatrix * transform.WorldMatrix;
            }
        }
    }
}