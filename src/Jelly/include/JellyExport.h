#pragma once

// Define export macro
#ifdef _WIN32
  #define JELLY_API __declspec(dllexport)
#else
  #define JELLY_API __attribute__((visibility("default")))
#endif

// Handle C linkage for external C APIs
#ifdef __cplusplus
  #define JELLY_API_BEGIN extern "C" {
  #define JELLY_API_END   }
#else
  #define JELLY_API_BEGIN
  #define JELLY_API_END
#endif
