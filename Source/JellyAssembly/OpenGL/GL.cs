using System.Runtime.InteropServices;

namespace JellyAssembly.OpenGL
{
#pragma warning disable CS8618
    public partial class GL
    {
        // Singleton instance
        private static readonly GL _instance = new GL();

        // Function loader delegate
        private Func<string, IntPtr>? _getProcAddress;

        // Public property to access the singleton instance
        public static GL Instance => _instance;

        // Private constructor to enforce singleton
        private GL() { }

        [DllImport("opengl32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint glGetError();  // Returns the last error code.

        public static void CheckForErrors()
        {
            uint error = glGetError();
            if (error != 0)
            {
                Console.WriteLine($"OpenGL Error: {error}");
            }
        }

        // Initialize the OpenGL class
        public void Initialize(Func<string, IntPtr> getProcAddress)
        {
            if (_getProcAddress != null)
            {
                throw new InvalidOperationException("OpenGL is already initialized.");
            }

            _getProcAddress = getProcAddress;

            // Load required OpenGL functions
            LoadFunction("glGetString", out _glGetString!);
            LoadFunction("glGetStringi", out _glGetStringi!);
            LoadFunction("glClear", out _glClear!);
            LoadFunction("glClearColor", out _glClearColor!);
            LoadFunction("glGenVertexArrays", out _glGenVertexArrays!);
            LoadFunction("glDeleteVertexArrays", out _glDeleteVertexArrays!);
            LoadFunction("glBindVertexArray", out _glBindVertexArray!);
            LoadFunction("glGenBuffers", out _glGenBuffers!);
            LoadFunction("glDeleteBuffers", out _glDeleteBuffers!);
            LoadFunction("glBindBuffer", out _glBindBuffer!);
            LoadFunction("glBufferData", out _glBufferData!);
            LoadFunction("glDrawArrays", out _glDrawArrays!);
            LoadFunction("glVertexAttribPointer", out _glVertexAttribPointer!);
            LoadFunction("glEnableVertexAttribArray", out _glEnableVertexAttribArray!);
            LoadFunction("glCreateShader", out _glCreateShader!);
            LoadFunction("glShaderSource", out _glShaderSource!);
            LoadFunction("glCompileShader", out _glCompileShader!);
            LoadFunction("glGetProgramiv", out _glGetProgramiv!);
            LoadFunction("glGetShaderInfoLog", out _glGetShaderInfoLog!);
            LoadFunction("glGetProgramInfoLog", out _glGetProgramInfoLog!);
            LoadFunction("glGetShaderiv", out _glGetShaderiv!);
            LoadFunction("glCreateProgram", out _glCreateProgram!);
            LoadFunction("glAttachShader", out _glAttachShader!);
            LoadFunction("glLinkProgram", out _glLinkProgram!);
            LoadFunction("glDeleteShader", out _glDeleteShader!);
            LoadFunction("glUseProgram", out _glUseProgram!);
            LoadFunction("glDeleteProgram", out _glDeleteProgram!);
            LoadFunction("glGetUniformLocation", out _glGetUniformLocation!);
            LoadFunction("glUniform1f", out _glUniform1f!);
            LoadFunction("glUniform3f", out _glUniform3f!);
            LoadFunction("glUniformMatrix4fv", out _glUniformMatrix4fv!);
            LoadFunction("glDrawElements", out _glDrawElements!);
            LoadFunction("glEnable", out _glEnable!);
            LoadFunction("glDisable", out _glDisable!);
            LoadFunction("glDepthFunc", out _glDepthFunc!);
            LoadFunction("glBindFramebuffer", out _glBindFramebuffer!);
            LoadFunction("glGenFramebuffers", out _glGenFramebuffers!);
            LoadFunction("glGenTextures", out _glGenTextures!);
            LoadFunction("glBindTexture", out _glBindTexture!);
            LoadFunction("glTexImage2D", out _glTexImage2D!);
            LoadFunction("glTexParameteri", out _glTexParameteri!);
            LoadFunction("glTexParameterf", out _glTexParameterf!);
            LoadFunction("glFramebufferTexture2D", out _glFramebufferTexture2D!);
            LoadFunction("glGenRenderbuffers", out _glGenRenderbuffers!);
            LoadFunction("glRenderbufferStorage", out _glRenderbufferStorage!);
            LoadFunction("glBindRenderbuffer", out _glBindRenderbuffer!);
            LoadFunction("glFramebufferRenderbuffer", out _glFramebufferRenderbuffer!);
            LoadFunction("glCheckFramebufferStatus", out _glCheckFramebufferStatus!);
            LoadFunction("glDeleteFramebuffers", out _glDeleteFramebuffers!);
            LoadFunction("glDeleteTextures", out _glDeleteTextures!);
            LoadFunction("glDeleteRenderbuffers", out _glDeleteRenderbuffers!);
            LoadFunction("glViewport", out _glViewport!);
            LoadFunction("glBlendFunc", out _glBlendFunc!);
            LoadFunction("glDepthMask", out _glDepthMask!);
            LoadFunction("glCullFace", out _glCullFace!);
        }


        // Generalized function loader
        private void LoadFunction<T>(string name, out T field) where T : Delegate
        {
            if (_getProcAddress == null)
            {
                throw new InvalidOperationException("Function loader not initialized.");
            }

            var funcPtr = _getProcAddress(name);
            if (funcPtr != IntPtr.Zero)
            {
                field = Marshal.GetDelegateForFunctionPointer<T>(funcPtr);
            }
            else
            {
                field = default!; // Non-nullable but ensures safety
                throw new InvalidOperationException($"Failed to load OpenGL function: {name}");
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glEnable_d(uint cap);
        private static glEnable_d _glEnable;
        /// <summary>
        /// Enable or disable server-side GL capabilities.
        /// </summary>
        /// <param name="cap">Specifies a symbolic constant indicating a GL capability.</param>
        public static void Enable(EnableCap cap)
        {
            _glEnable((uint)cap);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void glDisable_d(uint cap);
        private static glDisable_d _glDisable;

        /// <summary>
        /// Disable server-side GL capabilities.
        /// </summary>
        /// <param name="cap">Specifies a symbolic constant indicating a GL capability.</param>
        public static void Disable(EnableCap cap)
        {
            _glDisable((uint)cap);
        }
    }
}
