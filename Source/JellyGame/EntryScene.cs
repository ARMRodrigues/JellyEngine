using JellyEngine.Core.SceneManagement;

namespace JellyGame;

public class EntryScene : Scene
{
    public EntryScene(string name) : base(name)
    {
        var testEntity = EntityManager.CreateEntity();
        
        Console.WriteLine(testEntity.Id.ToString());
    }
}