using JellyEngine;
using JellyGame.Scenes.Cubes;
using JellyGame.Scenes.Guild;
using JellyGame.Scenes.Map2D;
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

        GameEntryScene = new GuildManager("Scene2D");
    }
}