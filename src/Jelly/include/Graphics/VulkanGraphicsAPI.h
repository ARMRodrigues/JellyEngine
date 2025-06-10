#pragma once

#include <iostream>

#include "IGraphicsAPI.h"

class VulkanGraphicsAPI final : public IGraphicsAPI {
public:
    void Init() override       { std::cout << "[Vulkan] Init\n"; }
    void BeginFrame() override { std::cout << "[Vulkan] BeginFrame\n"; }
    void EndFrame() override   { std::cout << "[Vulkan] EndFrame\n"; }
};