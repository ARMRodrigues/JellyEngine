namespace JellyAssembly.GLFW
{
    public enum GLFWBool
    {
        False = 0,
        True = 1
    }

    public enum WindowHint
    {
        ClientAPI = 0x00022001,
        ContextVersionMajor = 0x00022002,
        ContextVersionMinor = 0x00022003,
        Visible = 0x00020004,
        Decorated = 0x00020005,
        OpenGLProfile = 0x00022008,
        Samples = 0x0002100D,
    }

    public enum GLFWAPI
    {
        NoAPI = 0,
        OpenGLAPI = 0x00030001,
        OpenGLESAPI = 0x00030002
    }

    public enum OpenGLProfile
    {
        OpenGLAnyProfile = 0,
        OpenGLCoreProfile = 0x00032001,
        OpenGLCompatProfile = 0x00032002
    }

    public enum CursorMode
    {
        /// <summary>
        ///     The cursor is visible and behaves normally.
        /// </summary>
        Normal = 0x00034001,

        /// <summary>
        ///     The cursor is invisible when it is over the client area of the window but does not restrict the cursor from
        ///     leaving.
        /// </summary>
        Hidden = 0x00034002,

        /// <summary>
        ///     Hides and grabs the cursor, providing virtual and unlimited cursor movement. This is useful for implementing for
        ///     example 3D camera controls.
        /// </summary>
        Disabled = 0x00034003
    }

     public enum InputMode
    {
	    /// <summary>
	    ///     If specified, enables setting the mouse behavior.
	    ///     <para>See <see cref="CursorMode" /> for possible values.</para>
	    /// </summary>
	    Cursor = 0x00033001,

	    /// <summary>
	    ///     If specified, enables setting sticky keys, where <see cref="Glfw.GetKey" /> will return
	    ///     <see cref="InputState.Press" /> the first time you call it for a key that was pressed, even if that key has already
	    ///     been released.
	    /// </summary>
	    StickyKeys = 0x00033002,

	    /// <summary>
	    ///     If specified, enables setting sticky mouse buttons, where <see cref="Glfw.GetMouseButton" /> will return
	    ///     <see cref="InputState.Press" /> the first time you call it for a mouse button that was pressed, even if that mouse
	    ///     button has already been released.
	    /// </summary>
	    StickyMouseButton = 0x00033003,

	    /// <summary>
	    ///     When this input mode is enabled, any callback that receives modifier bits will have the
	    ///     <see cref="ModifierKeys.CapsLock" /> bit set if caps lock was on when the event occurred and the
	    ///     <see cref="ModifierKeys.NumLock" /> bit set if num lock was on.
	    /// </summary>
	    LockKeyMods = 0x00033004,

	    /// <summary>
	    ///     When the cursor is disabled, raw (unscaled and unaccelerated) mouse motion can be enabled if available.
	    ///     <seealso cref="Glfw.RawMouseMotionSupported" />
	    /// </summary>
	    RawMouseMotion = 0x00033005
    }
}
