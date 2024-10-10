#include <SPI.h>
#include <MFRC522.h>
#include <WiFi.h>
#include <HTTPClient.h>

// Pin Definitions
#define SS_PIN 5
#define RST_PIN 0

// Wi-Fi credentials
const char* ssid = "<SSID>";
const char* password = "<WIFI_PASSWORD>"; 
const char* endpoint = "<ENDPOINT_URL>";

MFRC522 rfid(SS_PIN, RST_PIN);  // Instance of the class
MFRC522::MIFARE_Key key;        // Key for MIFARE Classic cards

byte nuidPICC[4];  // Array to store the card's NUID

void setup() {
  Serial.begin(9600);
  SPI.begin();             // Init SPI bus
  rfid.PCD_Init();         // Init MFRC522
  initWiFi();              // Connect to Wi-Fi

  // Default key for MIFARE cards
  for (byte i = 0; i < 6; i++) {
    key.keyByte[i] = 0xFF;
  }

  Serial.println(F("Ready to scan RFID cards."));
}

void loop() {
  // Check for new card
  if (!rfid.PICC_IsNewCardPresent()) return;

  // Check if the card can be read
  if (!rfid.PICC_ReadCardSerial()) return;

  // Print card details
  Serial.println(F("A card has been detected!:"));
  printCardDetails();

  // Send the card details via HTTP POST
  sendCardDataToServer();

  // Halt PICC and stop encryption
  rfid.PICC_HaltA();
  rfid.PCD_StopCrypto1();
}

// Function to print the card details
void printCardDetails() {
  Serial.print(F("In Hex: "));
  printHex(rfid.uid.uidByte, rfid.uid.size);
  Serial.println();

  Serial.print(F("In Dec: "));
  printDec(rfid.uid.uidByte, rfid.uid.size);
  Serial.println();
}

// Function to send card data to the server
void sendCardDataToServer() {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(endpoint);
    http.addHeader("Content-Type", "application/json");

    String payload = "{\"nuidHex\":\"";

    // Properly format each byte in HEX with leading zeros if necessary
    for (byte i = 0; i < rfid.uid.size; i++) {
      if (rfid.uid.uidByte[i] < 0x10) {
        payload += "0";  // Leading zero for single hex digits
      }
      payload += String(rfid.uid.uidByte[i], HEX);
    }
    payload += "\"}";

    // Debug: print the final payload to the serial monitor
    Serial.println("Payload: " + payload);

    // Send POST request
    int httpResponseCode = http.POST(payload);
    if (httpResponseCode > 0) {
      String response = http.getString();
      Serial.println("HTTP Response: " + response);
    } else {
      Serial.println("Error in sending POST request");
    }

    http.end();
  } else {
    Serial.println("WiFi not connected");
  }
}

// Helper to print a byte array in HEX
void printHex(byte *buffer, byte bufferSize) {
  for (byte i = 0; i < bufferSize; i++) {
    Serial.print(buffer[i] < 0x10 ? " 0" : " ");
    Serial.print(buffer[i], HEX);
  }
}

// Helper to print a byte array in DEC
void printDec(byte *buffer, byte bufferSize) {
  for (byte i = 0; i < bufferSize; i++) {
    Serial.print(' ');
    Serial.print(buffer[i], DEC);
  }
}

void initWiFi() {
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.print(".");
  }
  Serial.println();
  Serial.println("WiFi connected");
}
