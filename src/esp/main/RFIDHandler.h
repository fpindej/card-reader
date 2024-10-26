#ifndef RFID_HANDLER_H
#define RFID_HANDLER_H

class RFIDHandler 
{
public:
    static void init();
    static void checkCard();

private:
    static String constructRfidId();
};

#endif
