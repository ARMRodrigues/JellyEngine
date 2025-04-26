using System.Numerics;
using JellyEngine;
using JellyEngine.Rendering;
using JellyGame;

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new Vector2(1280, 720),
    Vsync = true,
    Title = "Jelly Engine",
    GraphicsAPI = GraphicsAPI.OpenGL
};

var game = new GameApplication(nativeWindowSettings);
var gameManager = new GameManager();
game.Play(gameManager.GameEntryScene);