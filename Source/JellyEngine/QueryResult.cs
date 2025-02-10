namespace JellyEngine;

public struct QueryResult<T1, T2>
{
    public Entity Entity { get; }
    public T1 Component1 { get; }
    public T2 Component2 { get; }

    public QueryResult(Entity entity, T1 c1, T2 c2)
    {
        Entity = entity;
        Component1 = c1;
        Component2 = c2;
    }

    public void Deconstruct(out T1 c1, out T2 c2) => (c1, c2) = (Component1, Component2);
    public void Deconstruct(out Entity entity, out T1 c1, out T2 c2) => (entity, c1, c2) = (Entity, Component1, Component2);
}