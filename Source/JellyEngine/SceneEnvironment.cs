namespace JellyEngine;

public class SceneEnvironment
{
    public static SceneEnvironment Main { get; private set; } = null!;
    public DirectionalLight DirectionalLight { get; set; } = new();
    public Skybox Skybox { get; set; }
    public Shader Shader { get; set; }

    public SceneEnvironment()
    {
        Main = this;
        Skybox = new Skybox
        {
            Right = new Image("EngineData/Assets/Skybox/right.png"),
            Left = new Image("EngineData/Assets/Skybox/left.png"),
            Bottom = new Image("EngineData/Assets/Skybox/bottom.png"),
            Top = new Image("EngineData/Assets/Skybox/top.png"),
            Front = new Image("EngineData/Assets/Skybox/front.png"),
            Back = new Image("EngineData/Assets/Skybox/back.png")
        };
        Shader = new Shader("JellyEngine.Resources.Shaders.SkyboxVertex.glsl",
            "JellyEngine.Resources.Shaders.SkyboxFragment.glsl");
    }

    public SceneEnvironment(Skybox skybox)
    {
        Main = this;
        Skybox = skybox;
        Shader = new Shader("JellyEngine.Resources.Shaders.SkyboxVertex.glsl",
            "JellyEngine.Resources.Shaders.SkyboxFragment.glsl");
    }

}