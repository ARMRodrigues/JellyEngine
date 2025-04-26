using JellyEngine;
using JellyGame.Scenes.Cubes;
using JellyGame.Scenes.Guild;
using JellyGame.Scenes.Map2D;
using JellyGame.Scenes.MeshLoader;
using JellyGame.Scenes.Terrain;
using JellyGame.Scenes.WalkAround;

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
            Width = 1280,
            Height = 720,
            FullScreen = false,
            Vsync = true
        };

<<<<<<< HEAD
        GameEntryScene = new GuildManager("Scene2D");
=======
        GameEntryScene = new MeshLoaderScene("Scene2D");
>>>>>>> 094e66c0731709164f7d753bebaf00d68a27135d
    }
}