#ifndef USER_STORE_H
#define USER_STORE_H

class UserStore
{
public:
    static void updateValidUsers();
    static void fetchUsersFromServer();
    static bool isUserValid(const String& rfidId);

private:
    static const char* endpointGetValidUsers;
    static void parseAndSaveUserList(const String& response);
};

#endif
