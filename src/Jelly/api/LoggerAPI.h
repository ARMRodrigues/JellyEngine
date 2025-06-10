#pragma once

#include "JellyExport.h"
#include "Logger.h"

JELLY_API_BEGIN

/// Logs a message using the engine's logging system.
///
/// @param level The log severity (Info, Highlight, Warning, Error).
/// @param message The message to log.
JELLY_API void jellyLogMessage(LogLevel level, const char* message);

JELLY_API_END
