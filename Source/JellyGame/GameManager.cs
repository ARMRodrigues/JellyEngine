using JellyEngine.Core.SceneManagement;

namespace JellyGame;

public class GameManager
{
    public string GameName { get; }
    public Scene GameEntryScene { get; }
    public GameSettings GameSettings { get; }

    public GameManager()
    {
        GameName = "Jelly Game";
        
        GameSettings = new GameSettings()
        {
            Width = 800,
            Height = 600,
            FullScreen = false,
            Vsync = true
        };
        
        GameEntryScene = new EntryScene("EntryScene");
    }
}