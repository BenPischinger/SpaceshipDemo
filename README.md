# Spaceship Demo

A quick demo for spaceship controls including movement, shoting lasers, a laserbeam and rockets.

## Movement

<p align="center">
<img src="https://i.imgur.com/NmgLG3P.png" width="1385">
</p>

- The spaceship is based on a rigidbody and propelled by applying force to it
- Turning happens when the mouse is moved or the 'WASD' keys are pressed 
- Rotating the ship is done by lerping between positions to achieve the floaty space controls
- By pressing space the speed of the ship can increased

## Laser 

<p align="center">
<img src="https://i.imgur.com/O2OcPMt.png" width="1385">
</p>

- Left mouse button shoots laser that are also based on a rigidbody and have a collider
- Upon collision, the laser object is destroyed and a hit animation is played
- The laser shooting system is based on a heat resource
- If the heat bar in the lower left corner is full, the laser can not be shot anymore
- The heat bar will cooldown when the laser is not shot for at least 2 seconds

## Laser Beam

<p align="center">
<img src="https://i.imgur.com/RgBENrD.png" width="1385">
</p>

- The laserbeam is a simple particle system that is played and stopped when the right mouse button is pressed
- It uses the energy bar in the lower right corner as a resource
- Once the bar is depleted, the beam can not be shot anymore
- Energy starts regenerating after a while

## Rockets

<p align="center">
<img src="https://i.imgur.com/YSNnUB6.png" width="1385">
</p>

- The rockets are also based on a rigidbody and can be fired by pressing 'R'
- They explode upon impact
