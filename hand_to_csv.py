import cv2
import mediapipe as mp
import numpy as np
import csv
mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands


coords = np.array([0.0, 0.0, 0.0, 0.0])
out = coords
cap = cv2.VideoCapture(0)
hands =  mp_hands.Hands(
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5,
    max_num_hands = 1
    )
while cap.isOpened():
    success, image = cap.read()
    if not success:
        print("Ignoring empty camera frame.")
        continue

    image = cv2.cvtColor(cv2.flip(image, 1), cv2.COLOR_BGR2RGB)

    image.flags.writeable = False
    results = hands.process(image)

    image_height, image_width, _ = image.shape
    annotated_image = image.copy()

    image.flags.writeable = True
    image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
    if results.multi_hand_landmarks:
        for hand_landmarks in results.multi_hand_landmarks:
            mp_drawing.draw_landmarks(image, hand_landmarks, mp_hands.HAND_CONNECTIONS)
            coords[0] = hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].x * image_width
            coords[1] = hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].y * image_height
            coords[2] = hand_landmarks.landmark[mp_hands.HandLandmark.WRIST].z * image_height
            print(coords)
            out = np.vstack((out,coords))
    cv2.imshow('MediaPipe Hands', image)
    if cv2.waitKey(5) & 0xFF == 27:
        break
hands.close()
cap.release()
cv2.destroyAllWindows()
with open("C:/Users/m2394/Desktop/lahacks/HandData.csv", "w") as output:
    writer = csv.writer(output, lineterminator='\n')
    writer.writerow(["X", "Y", "Z", "GRASP"])
    writer.writerows(out)
cv2.destroyAllWindows()