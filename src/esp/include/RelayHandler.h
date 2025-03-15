#ifndef RELAY_HANDLER_H
#define RELAY_HANDLER_H

class RelayHandler 
{
public:
    static void init();
    static void checkAndUpdateRelay();
    static void activateRelay();
private:
    static const unsigned long RELAY_ACTIVE_DURATION = 2000; // 2 seconds
    static unsigned long relayStartTime;
    static bool relayActive;
};

#endif
