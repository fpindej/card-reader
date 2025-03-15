#ifndef TIME_MANAGER_H
#define TIME_MANAGER_H

#include <Arduino.h>

class TimeManager {
public:
    static void initialize();
    static bool isTimeSet();
    static void sync();
    static String getCurrentUTCTime(); // Returns time in YYYY-MM-DD HH:MM:SS format

private:
    static bool timeInitialized;
    static const char* NTP_SERVER_1;
    static const char* NTP_SERVER_2;
    static const long GMT_OFFSET_SEC = 0;     // UTC time
    static const int DAYLIGHT_OFFSET_SEC = 0; // No daylight saving
};

#endif // TIME_MANAGER_H