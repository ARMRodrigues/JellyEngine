using JellyEngine;
using JellyEngine.Core.Rendering;

var nativeWindowSettings = new NativeWindowSettings
{
    Size = new(800, 600),
    Vsync = true,
    Title = "Jelly Engine",
    GraphicsAPI = GraphicsAPI.OpenGL
};

var game = new GameApplication(nativeWindowSettings);

game.Play();