#include <SPI.h>
#include <MFRC522.h>
#include "RFIDHandler.h"
#include "CardHandler.h"

// Pin Definitions
#define SS_PIN 5
#define RST_PIN 0
#define RELAY_PIN 26

MFRC522 rfid(SS_PIN, RST_PIN);  // Define the RFID object

unsigned long relayStartTime = 0;
bool relayActive = false;

void RFIDHandler::init() 
{
    SPI.begin();            // Initialize SPI bus
    rfid.PCD_Init();       // Initialize MFRC522
    pinMode(RELAY_PIN, OUTPUT);
    digitalWrite(RELAY_PIN, LOW);
    Serial.println("RFID reader initialized.");
}

void RFIDHandler::checkCard() 
{
    if (relayActive && (millis() - relayStartTime >= 2000))
    {
      digitalWrite(RELAY_PIN, LOW);
      relayActive = false;
      Serial.println("Relay turned OFF.");
    }

    if (!rfid.PICC_IsNewCardPresent()) return;  // Check for a new card
    if (!rfid.PICC_ReadCardSerial()) return;   // Read card serial

    CardHandler::fetchCardsFromServer();
    String rfidId = constructRfidId();

    // Check if the card is active
    if (CardHandler::isCardActive(rfidId)) 
    {
        Serial.println("Access granted for: " + rfidId);
        digitalWrite(RELAY_PIN, HIGH); // Turn on the relay
        relayStartTime = millis(); // Record when the relay was turned on
        relayActive = true; // Mark relay as active
        Serial.println("Relay turned ON.");
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
