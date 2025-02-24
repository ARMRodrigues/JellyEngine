namespace JellyEngine;

public class Hierarchy : GameComponent
{
    public int ParentId { get; set; }
    public List<int> ChildrenId { get; set; } = new();
}