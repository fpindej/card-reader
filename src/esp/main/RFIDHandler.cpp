#include <SPI.h>
#include <MFRC522.h>
#include "RFIDHandler.h"
#include "CardHandler.h"

// Pin Definitions
#define SS_PIN 5
#define RST_PIN 0

MFRC522 rfid(SS_PIN, RST_PIN);  // Define the RFID object

void RFIDHandler::init() 
{
    SPI.begin();            // Initialize SPI bus
    rfid.PCD_Init();       // Initialize MFRC522
    Serial.println("RFID reader initialized.");
}

void RFIDHandler::checkCard() 
{
    if (!rfid.PICC_IsNewCardPresent()) return;  // Check for a new card
    if (!rfid.PICC_ReadCardSerial()) return;   // Read card serial

    CardHandler::fetchCardsFromServer();
    String rfidId = constructRfidId();

    if (CardHandler::isCardActive(rfidId)) 
    {
        Serial.println("Access granted for: " + rfidId);
        // Add code to grant access
    } 
    else 
    {
        Serial.println("Access denied for: " + rfidId);
        // Add code to deny access
    }

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
