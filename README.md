# VR Horror Exploration Prototype

## Project Overview

This project is a **VR horror-exploration prototype developed with Unity and XR Interaction Toolkit**, targeting **PICO VR headsets**.  
The experience focuses on environmental interaction, psychological pressure, and spatial awareness through a combination of room-based systems, interactive objects, and player state management.

The core gameplay revolves around **exploring multiple rooms**, **measuring temperature**, **interacting with doors and objects**, and **maintaining the player’s sanity (SAN)** under dynamically changing conditions. Certain rooms are randomly designated as special or hostile areas, encouraging cautious exploration and observation.

---

## Key Features

- **XR-based Interaction System**  
  Ray-based and controller-based interactions supporting object manipulation and environment control, designed to work both with empty hands and while holding objects.

- **Room Detection & Management**  
  Trigger-based room system with support for nested rooms, allowing accurate detection of the player’s current location.

- **Dynamic Temperature System**  
  Each room is initialized with a temperature value. A randomly selected “special room” features abnormal temperature behavior, detectable using a thermometer tool.

- **Sanity (SAN) System**  
  The player’s sanity decreases over time at a rate influenced by environmental conditions and interactive elements such as candles.

- **Interactive Objects**  
  Includes doors, candles, and tools that respond to controller input and ray interaction, designed specifically for VR usability.

- **VR UI (HUD)**  
  World-space UI attached to the player’s view, displaying real-time SAN and temperature information.

---

## Technology Stack

- **Engine:** Unity 2022 LTS  
- **VR Framework:** XR Interaction Toolkit  
- **Target Platform:** PICO VR  
- **UI:** TextMeshPro (World Space Canvas)

---

## Notes

This project is intended as a **prototype / experimental VR experience** and focuses on system architecture, interaction design, and VR-specific usability considerations rather than polished content.
