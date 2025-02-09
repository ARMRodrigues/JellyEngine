namespace JellyEngine;

public class TransformSystem (EntityManager entityManager) : GameSystem
{
    private readonly EntityManager _entityManager = entityManager;
    
    public override void Update()
    {
        foreach (var transform in _entityManager.GetComponents<Transform>())
        {
            if (_entityManager.TryGetComponent(new Entity(transform.ParentId), out Transform? parentTransform) && 
                parentTransform != null)
            {
                transform.WorldMatrix = transform.LocalMatrix * parentTransform.WorldMatrix;
            }
            else
            {
                transform.WorldMatrix = transform.LocalMatrix;
            }

        }
    }
}