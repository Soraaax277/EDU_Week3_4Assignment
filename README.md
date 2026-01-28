# Week 3 Tower Defense Assignment (Isolated)

## Project Structure
- **Scripts**: `Assets/TowerDefense/Scripts/` (All TD scripts use `TD_` prefix for managers/turrets)
- **Editor Tool**: `Assets/TowerDefense/Editor/UIBuilder.cs`
- **GP3 Scripts**: Your original `Assets/Scripts/` (GP3) have been reverted and left untouched.

## Requirements Implemented
- [x] Enemy Spawning (Quadratic & Cubic Lerp)
- [x] Player HP System (20 HP, Damage on enemy reach)
- [x] HP Bar UI (Ghost HP with casing)
- [x] Coin System (Spawn on death, move to UI, lerp value)
- [x] Turret Integration (One-hit kill, no physics)
- [x] Fail UI on 0 HP
- [x] Builder Unit (Max 3 builds, detection highlight)
- [x] Wire Specialist (Max 8 wires)
- [x] Wiring Connectivity System

## How to Play
1. Open the project in Unity.
2. Go to `Tools > Build Tower Defense UI` to generate the Canvas.
3. Set up two Spawn Points and one Target Location in the scene.
4. Assign these to the `WaveManager` component.
5. Pre-place some turrets.
6. Press Play!

## Video Demonstration
[Watch the Demonstration Video](YOUR_VIDEO_LINK_HERE)

## Technical Notes
- **Strictly Math**: All movement and easing use custom math functions in `EasingFunctions.cs` and `LerpMovement.cs`.
- **No Physics**: Collision detection uses custom circle/cone math in `MathCollision.cs`.
- **Ghost HP**: Uses an exponential ease-out to create a smooth trailing effect behind the main health bar.
