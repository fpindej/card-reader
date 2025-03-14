#ifndef LOG_HANDLER_H
#define LOG_HANDLER_H

#include <Arduino.h>

class LogHandler {
public:
    static void init();
    static void logAccess(const String& cardNumber, bool isSuccessful);

private:
    static const char* LOG_ENDPOINT;
};

#endif