#ifndef CARD_HANDLER_H
#define CARD_HANDLER_H

class CardHandler
{
public:
    static void updateActiveCards();
    static void fetchCardsFromServer();
    static bool isCardActive(const String& rfidId);

private:
    static const char* endpointGetActiveCards;
    static void parseAndSaveCardList(const String& response);
};

#endif