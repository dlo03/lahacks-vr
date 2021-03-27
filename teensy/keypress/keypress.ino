// USB Type: "All of the Above" 
// (or "Serial+Keyboard+Mouse+Joystick")
// (or keyboard as long as COM port is numbered)

// be sure to check COM port and modify the Unity's Serial Controller Scrip
// under "Hand object", select "DetectBox" and under "Serial Controller (Script)"
// change the COM port to your current COM.

bool pump = false;

void setup() {
  Serial.begin(9600);
  // modify the pin that gets activate through metal strip touching:
  pinMode(7,INPUT);
  pinMode(LED_BUILTIN,OUTPUT);
}

void loop() {
  // Unity will output A if in viscinity of object and pump should turn on.
  if (Serial.available()){
    char c = Serial.read();
    if (c){
      if (c == 'A'){pump = true;} 
      else if (c == 'Z'){pump = false;}
    }
  }
  
  while (digitalRead(7) == HIGH){
    Keyboard.press(KEY_P);
  }
  Keyboard.release(KEY_P);
  
  if (pump){
    digitalWrite(LED_BUILTIN, HIGH);
    Serial.println("on");
    //insert pump code:
    
  } else{
    digitalWrite(LED_BUILTIN, LOW);
    Serial.println("off");
    //insert depump code:
    
  }
  delay(100);
}
