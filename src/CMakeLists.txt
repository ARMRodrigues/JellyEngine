cmake_minimum_required(VERSION 3.30)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

project(Jelly)

set(DOTNET_SDK "net8.0")
set(OUTPUT_DIR "${CMAKE_SOURCE_DIR}/../output/${CMAKE_BUILD_TYPE}/${DOTNET_SDK}")

add_subdirectory(Jelly)