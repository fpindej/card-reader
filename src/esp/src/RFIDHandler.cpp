#include <Arduino.h>

#include <SPI.h>
#include <MFRC522.h>
#include "RFIDHandler.h"
#include "CardHandler.h"
#include "RelayHandler.h"
#include "LogHandler.h"

#define SS_PIN 5  // SDA
#define RST_PIN 0 // RST

MFRC522 rfid(SS_PIN, RST_PIN);

void RFIDHandler::init() 
{
    SPI.begin();            // Initialize SPI bus
    rfid.PCD_Init();       // Initialize MFRC522
    Serial.println("RFID reader initialized.");
}

void RFIDHandler::checkCard() 
{
    RelayHandler::checkAndUpdateRelay();

    if (!rfid.PICC_IsNewCardPresent()) return;  // Check for a new card
    if (!rfid.PICC_ReadCardSerial()) return;   // Read card serial

    CardHandler::fetchCardsFromServer();
    String rfidId = constructRfidId();

    bool isAccessGranted = CardHandler::isCardActive(rfidId);

    if (isAccessGranted) 
    {
        Serial.println("Access granted for: " + rfidId);
        RelayHandler::activateRelay();
    }
    else 
    {
        Serial.println("Access denied for: " + rfidId);
        // Add code to deny access
    }

    LogHandler::logAccess(rfidId, isAccessGranted);

    // Halt PICC and stop encryption
    rfid.PICC_HaltA();
    rfid.PCD_StopCrypto1();
}

String RFIDHandler::constructRfidId() 
{
    String rfidId = "";
    for (byte i = 0; i < rfid.uid.size; i++) 
    {
        if (rfid.uid.uidByte[i] < 0x10) 
        {
            rfidId += "0"; // Leading zero for single hex digits
        }
        rfidId += String(rfid.uid.uidByte[i], HEX);
    }
    return rfidId;
}
