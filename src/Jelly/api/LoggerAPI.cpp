#include "LoggerAPI.h"

// -----------------------------------------------------------------------------
// Logs a message using the engine's logging system.
// -----------------------------------------------------------------------------
JELLY_API void jellyLogMessage(LogLevel level, const char *message) {
    Logger::Log(level, std::string(message));
}
