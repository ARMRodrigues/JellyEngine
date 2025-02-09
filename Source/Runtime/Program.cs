using System.Numerics;
using JellyEngine;
using JellyEngine.Core.Rendering;
using JellyGame;

var gameManager = new GameManager();

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new Vector2(gameManager.GameSettings.Width, gameManager.GameSettings.Height),
    Vsync = gameManager.GameSettings.Vsync,
    Title = gameManager.GameName,
    GraphicsAPI = GraphicsAPI.OpenGL
};

var game = new GameApplication(nativeWindowSettings);

game.Play(gameManager.GameEntryScene);