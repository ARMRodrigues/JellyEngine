using System.Numerics;
using JellyAssembly.GLFW;
using JellyAssembly.OpenGL;

namespace JellyEngine.Core.Rendering.OpenGL;

public class OpenGLRenderer : Renderer
{
    private Window _window;

    public OpenGLRenderer(Window window)
    {
        _window = window;

        GL.Instance.Initialize(GLFW.GetProcAddress);

        // Set the window-size callback
        GLFW.SetFramebufferSizeCallback(_window.Handle, (width, height) =>
        {
            var newSize = new Vector2(width, height);
            Display.WindowSize = newSize;
            Display.ViewportSize = newSize;
            GL.Viewport(0, 0, width, height);
        });

        GL.ClearColor(0.478f, 0.173f, 0.741f, 1.0f);

        GL.Enable(EnableCap.DepthTest);

        _window.CenterWindow();
    }

    public override bool IsWindowOpen()
    {
        return _window.IsWindowOpen();
    }

    public override void BeginRender()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public override void EndRender()
    {
        _window.SwapBuffers();
    }
}
