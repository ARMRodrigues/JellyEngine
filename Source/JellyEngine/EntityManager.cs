namespace JellyEngine;

public class EntityManager
{
    private readonly HashSet<int> _usedIds = new HashSet<int>(); //// Para evitar reutilização de IDs
    private readonly Dictionary<Entity, Dictionary<Type, GameComponent>> _components = [];
    
    public Entity CreateEntity()
    {
        int id;
        
        do
        {
            id = GenerateUniqueId();
        } 
        while (_usedIds.Contains(id));

        _usedIds.Add(id);
        var entity = new Entity(id);
        _components[entity] = [];
        return entity;
    }
    
    public void AddChild(Entity parent, Entity child)
    {
        TryGetComponent(parent, out Hierarchy? childrenComponent);

        if (childrenComponent == null)
        {
            AddComponent(parent, new Hierarchy()
            {
                ParentId = parent.Id,
                ChildrenId = [child.Id]
            });
        }
        else
        {
            childrenComponent.ChildrenId.Add(child.Id);
        }
    }
    
    public void AddComponent<T>(Entity entity, T component) where T : GameComponent
    {
        _components[entity][typeof(T)] = component;
    }
    
    public T GetComponent<T>(Entity entity) where T : GameComponent
    {
        if (_components.TryGetValue(entity, out var components) && 
            components.TryGetValue(typeof(T), out var component))
        {
            return (T)component;
        }

        throw new KeyNotFoundException($"Component {typeof(T).Name} not found for Entity {entity.Id}");
    }
    
    public IEnumerable<T> GetComponents<T>() where T : GameComponent
    {
        foreach (var kvp in _components)
        {
            var components = kvp.Value;
        
            if (components.TryGetValue(typeof(T), out var component))
            {
                if (component is T typedComponent)
                {
                    yield return typedComponent;
                }
            }
        }
    }
    
    public bool TryGetComponent<T>(Entity entity, out T? component) where T : GameComponent
    {
        component = default;
    
        return _components.TryGetValue(entity, out var components) &&
               components.TryGetValue(typeof(T), out var baseComponent) &&
               (component = baseComponent as T) != null;
    }
    
    public IEnumerable<QueryResult<T>> Query<T>()
        where T : GameComponent
    {
        foreach (var (entity, components) in _components)
        {
            if (components.TryGetValue(typeof(T), out var component))
            {
                yield return new QueryResult<T>(entity, (T)component);
            }
        }
    }
    
    public IEnumerable<QueryResult<T1, T2>> Query<T1, T2>()
        where T1 : GameComponent
        where T2 : GameComponent
    {
        foreach (var (entity, components) in _components)
        {
            if (components.TryGetValue(typeof(T1), out var c1) &&
                components.TryGetValue(typeof(T2), out var c2))
            {
                yield return new QueryResult<T1, T2>(entity, (T1)c1, (T2)c2);
            }
        }
    }
    
    private static int GenerateUniqueId()
    {
        var timestamp = DateTime.UtcNow.Ticks;
        var hash = MurmurHash3(Random.Shared.Next());
        return (int)(timestamp ^ hash);  // Combina o timestamp e o hash
    }
    
    private static int MurmurHash3(int input)
    {
        unchecked
        {
            var hash = 0xc58f1a7b;
            var key = (uint)input;
            key *= 0xcc9e2d51;
            key = (key << 15) | (key >> 17);
            key *= 0x1b873593;
            hash ^= key;
            hash = (hash << 13) | (hash >> 19);
            hash = hash * 5 + 0xe6546b64;
            return (int)hash;
        }
    }
}