using JellyEngine;
using JellyEngine.InputManagement;

namespace JellyGame;

public class ScriptTest : GameSystem
{
    public override void Update()
    {
        if (Input.IsActionJustPressed("Jump"))
        {
            Console.WriteLine("Jump");
        }
    }
}