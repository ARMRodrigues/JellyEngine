namespace JellyAssembly;

public struct NativeHelperName
{
    public const string GLFWLibraryName =
#if WINDOWS
	"glfw3";
#elif LINUX
	"libJelly.so";
#else
    "";
#endif

    public const string PhysXLibraryName =
#if WINDOWS
	"JellyNative.dll";
#elif LINUX
	"libImGuiBinding.so";
#else
     "";
#endif

    public const string ImGuiLibraryName =
#if WINDOWS
	"libImGuiBinding.dll";
#elif LINUX
	"libImGuiBinding.so";
#else
     "";
#endif
}