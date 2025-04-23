using System.Numerics;
using JellyAssembly.OpenGL;

namespace JellyEngine.Rendering.OpenGL;

public class Framebuffer : IDisposable
{
    private readonly uint _framebufferId;
    private readonly uint _textureColorBuffer;
    private readonly uint _renderbufferId;
    private Vector2 _size;

    public uint TextureId => _textureColorBuffer;

    public Framebuffer(Vector2 size)
    {
        _size = size;

        _framebufferId = GL.GenFramebuffers();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferId);

        _textureColorBuffer = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)size.X, (int)size.Y, 0, GLPixelFormat.Rgba, GLPixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, GLFramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _textureColorBuffer, 0);

        _renderbufferId = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _renderbufferId);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, PixelInternalFormat.Depth24Stencil8, (int)size.X, (int)size.Y);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, GLFramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, _renderbufferId);

        if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
        {
            throw new Exception("Failed to create framebuffer!");
        }

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebufferId);
        GL.Viewport(0, 0, (int)_size.X, (int)_size.Y);
    }

    public void Resize(Vector2 newSize)
    {
        if (_size == newSize)
            return;

        _size = newSize;

        GL.BindTexture(TextureTarget.Texture2D, _textureColorBuffer);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)newSize.X, (int)newSize.Y, 0, GLPixelFormat.Rgba, GLPixelType.UnsignedByte, IntPtr.Zero);

        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _renderbufferId);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, PixelInternalFormat.Depth24Stencil8, (int)newSize.X, (int)newSize.Y);
    }

    public void Dispose()
    {
        GL.DeleteFramebuffer(_framebufferId);
        GL.DeleteTexture(_textureColorBuffer);
        GL.DeleteRenderbuffer(_renderbufferId);
    }
}
