namespace JellyEngine.Core.EntityManagement;

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