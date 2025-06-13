#pragma once

#include <memory>

#include "GraphicsAPIType.h"
#include "IGraphicsAPI.h"
#include "Vulkan/VulkanGraphicsAPI.h"

/// Factory responsible for creating IGraphicsAPI instances based on enum type.
class GraphicsAPIFactory {
public:
    /// Creates an IGraphicsAPI instance based on the given API type.
    static std::unique_ptr<IGraphicsAPI> Create(GraphicsAPIType apiType) {
        switch (apiType) {
            case GraphicsAPIType::Vulkan:
                return std::make_unique<VulkanGraphicsAPI>();
            default:
                return nullptr;
        }
    }
};
