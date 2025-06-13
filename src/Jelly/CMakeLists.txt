include(FetchContent)

FetchContent_Declare(
    glfw
    GIT_REPOSITORY https://github.com/glfw/glfw.git
    GIT_TAG 3.4
)

FetchContent_MakeAvailable(glfw)

set(INCLUDE_DIR ${CMAKE_CURRENT_SOURCE_DIR}/include)
set(SRC_DIR ${CMAKE_CURRENT_SOURCE_DIR}/src)
set(API_DIR ${CMAKE_CURRENT_SOURCE_DIR}/api)

set(API_HEADER_FILES
    ${API_DIR}/LoggerAPI.h
    ${API_DIR}/JellyTypes.h
    ${API_DIR}/JellyEngineAPI.h
)

set(API_SOURCE_FILES
    ${API_DIR}/LoggerAPI.cpp
    ${API_DIR}/JellyEngineAPI.cpp
)

set(HEADERS
    ${INCLUDE_DIR}/JellyExport.h
    ${INCLUDE_DIR}/Logger.h
    ${INCLUDE_DIR}/Graphics/GraphicsAPIType.h
    ${INCLUDE_DIR}/Graphics/GraphicsApiException.h
    ${INCLUDE_DIR}/Graphics/IGraphicsAPI.h
    ${INCLUDE_DIR}/Graphics/GraphicsAPIFactory.h
    ${INCLUDE_DIR}/Graphics/Vulkan/QueueFamilyIndices.h
    ${INCLUDE_DIR}/Graphics/Vulkan/SwapChainSupportDetails.h
    ${INCLUDE_DIR}/Graphics/Vulkan/VulkanGraphicsAPI.h
    ${INCLUDE_DIR}/Window/WindowSettings.h
    ${INCLUDE_DIR}/Window/IWindowSystem.h
    ${INCLUDE_DIR}/Window/INativeWindowHandleProvider.h
    ${INCLUDE_DIR}/Window/GLFWindowSystem.h
    ${API_HEADER_FILES}
    ${INCLUDE_DIR}/JellyEngine.h
)

set(SRC_FILES
    ${SRC_DIR}/Logger.cpp
    ${SRC_DIR}/Window/GLFWindowSystem.cpp
    ${SRC_DIR}/Graphics/Vulkan/VulkanGraphicsAPI.cpp
    ${SRC_DIR}/Graphics/Vulkan/VulkanGraphicsAPIHelpers.cpp
    ${API_SOURCE_FILES}
    ${SRC_DIR}/JellyEngine.cpp
)

add_library(Jelly SHARED ${SRC_FILES} ${HEADERS})

find_package(Vulkan REQUIRED)
target_link_libraries(Jelly PRIVATE Vulkan::Vulkan)

target_include_directories(Jelly
    PUBLIC ${CMAKE_CURRENT_SOURCE_DIR}/include
)

target_link_libraries(Jelly PRIVATE glfw)
target_include_directories(Jelly PUBLIC ${glfw_SOURCE_DIR}/include)

set_target_properties(Jelly PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${OUTPUT_DIR}"   # .dll ou .so
    LIBRARY_OUTPUT_DIRECTORY "${OUTPUT_DIR}"   # .so (Linux) ou .dylib (macOS)
    ARCHIVE_OUTPUT_DIRECTORY "${OUTPUT_DIR}"   # .lib ou .a ou .dll.a
)

# Rename library in MinGW on Windows (ex: libJelly.dll.a → Jelly.a)
if(WIN32 AND MINGW)
    set_target_properties(Jelly PROPERTIES
        ARCHIVE_OUTPUT_NAME "Jelly"  # libJelly.dll.a → Jelly.a
        PREFIX ""                    # remove the "lib" prefix
    )
endif()
