using JellyAssembly.GLFW;
using JellyEngine.Rendering;

namespace JellyEngine;

public class Window : IDisposable
{
    private IntPtr _window;
    private GraphicsAPI _currentGraphicsAPI;
    private bool _disposed;

    public IntPtr Handle => _window;
    public GraphicsAPI CurrentRendererAPI => _currentGraphicsAPI;

    public Window(NativeWindowSettings nativeWindowSettings)
    {
        if (GLFW.Init() == 0)
        {
            Console.WriteLine("Failed to initialize GLFW.");
            return;
        }

        _currentGraphicsAPI = nativeWindowSettings.GraphicsAPI;

        ConfigureGLFWWindowHint();

        Display.WindowSize = nativeWindowSettings.Size;
        Display.ViewportSize = nativeWindowSettings.Size;

        _window = GLFW.CreateWindow(
            (int)nativeWindowSettings.Size.X,
            (int)nativeWindowSettings.Size.Y,
            nativeWindowSettings.Title,
            IntPtr.Zero, IntPtr.Zero
        );

        GLFW.MakeContextCurrent(_window);

        GLFW.SwapInterval(nativeWindowSettings.Vsync ? 1 : 0);
    }

    public bool IsWindowOpen()
    {
        return !GLFW.WindowShouldClose(_window);
    }

    private void ConfigureGLFWWindowHint()
    {
        GLFW.WindowHint(WindowHint.Visible, GLFWBool.False);
        GLFW.WindowHint(WindowHint.Decorated, GLFWBool.True);

        // Configuração específica para cada backend pode ser feita aqui.
        if (_currentGraphicsAPI == GraphicsAPI.OpenGL)
        {
            GLFW.WindowHint(WindowHint.ContextVersionMajor, 3);
            GLFW.WindowHint(WindowHint.ContextVersionMinor, 3);
            GLFW.WindowHint(WindowHint.OpenGLProfile, OpenGLProfile.OpenGLCoreProfile);
        }
    }

    public void SwapBuffers()
    {
        GLFW.SwapBuffers(_window);
        GLFW.PollEvents();
    }

    public void CenterWindow()
    {
        GLFW.HideWindow(_window);

        var monitor = GLFW.GetPrimaryMonitor();
        if (monitor == IntPtr.Zero) return;

        var videoMode = GLFW.GetVideoMode(monitor);
        var (windowWidth, windowHeight) = GLFW.GetWindowSize(_window);

        GLFW.SetWindowPos(_window,
            (videoMode.Width - windowWidth) / 2,
            (videoMode.Height - windowHeight) / 2
        );

        GLFW.ShowWindow(_window);
    }
    
    public void Dispose()
    {
        Cleanup();
        GC.SuppressFinalize(this);
    }

    private void Cleanup()
    {
        if (!_disposed)
        {            
            GLFW.DestroyWindow(_window);
            GLFW.Terminate();

            _disposed = true;
        }
    }

    ~Window()
    {
        Cleanup();
    }
}
