using Jelly.Engine;

var settings = new WindowSettings()
{
    Width = 1280,
    Height = 720,
    Vsync = true,
    Title = "Jelly"
};

var jellyApplication = new JellyApplication(settings);

jellyApplication.Play();

jellyApplication.Stop();