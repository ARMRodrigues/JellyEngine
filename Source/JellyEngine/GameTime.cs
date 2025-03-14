using System.Diagnostics;

namespace JellyEngine;

public class GameTime
{
    private readonly Stopwatch _stopwatch;
    
    private static float deltaTime;
    private static int frameCount;
    private static float totalTime;

    public static float DeltaTime => deltaTime;
    public static float TotalTime => totalTime;
    public static long FrameCount => frameCount;

    public GameTime()
    {
        _stopwatch = Stopwatch.StartNew();
        totalTime = 0f;
        frameCount = 0;
    }

    public void Update()
    {
        float currentTime = (float)_stopwatch.Elapsed.TotalSeconds;
        deltaTime = currentTime - TotalTime;
        totalTime = currentTime;
        frameCount++;
    }
}