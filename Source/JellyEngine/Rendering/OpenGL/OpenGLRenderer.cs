using System.Numerics;
using JellyAssembly.GLFW;
using JellyAssembly.OpenGL;

namespace JellyEngine.Rendering.OpenGL;

public class OpenGLRenderer : Renderer
{
    private readonly Window _window;
    private Framebuffer _gameViewFramebuffer;
    private PostProcessingPass _postProcessingPass;

    public OpenGLRenderer(Window window)
    {
        _window = window;
        GL.Instance.Initialize(GLFW.GetProcAddress);

        GLFW.SetFramebufferSizeCallback(_window.Handle, (width, height) =>
        {
            var size = new Vector2(width, height);
            Display.WindowSize = size;
            Display.ViewportSize = size;
            GL.Viewport(0, 0, width, height);
        });

        GL.ClearColor(0.478f, 0.173f, 0.741f, 1.0f);

        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.DepthTest);
        GL.DepthFunc(DepthFunction.Lequal);
        GL.Enable(EnableCap.Multisample);

        _gameViewFramebuffer = new Framebuffer(Display.ViewportSize);
        _postProcessingPass = new PostProcessingPass();

        _window.CenterWindow();
    }

    public override bool IsWindowOpen() => _window.IsWindowOpen();

    public override void BeginRender()
    {
        _gameViewFramebuffer.Bind();
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public override void EndRender()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        _window.SwapBuffers();
    }

    public override void RenderFramebufferToScreen()
    {
        _postProcessingPass.Render(_gameViewFramebuffer.TextureId);
    }    

    public override void PollEvents() => _window.PollEvents();

    public override void Shutdown()
    {
        _postProcessingPass.Dispose();
        _window.Dispose();
    }
}