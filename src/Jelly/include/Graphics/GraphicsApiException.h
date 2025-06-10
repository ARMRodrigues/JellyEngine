#include "Logger.h"

#include <stdexcept>

/// Exception class representing errors related to the graphics API.
/// Logs the error message upon construction.
class GraphicsApiException : public std::runtime_error {
public:
    explicit GraphicsApiException(const std::string& msg) : std::runtime_error(msg) {
        Logger::Log(LogLevel::Error, msg);
    }
};
