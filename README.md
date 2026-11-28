# DOTS Tower Defense

A 3D Tower Defense game project developed using **Unity DOTS (Data-Oriented Technology Stack)** and the Entity Component System (ECS).

The project focuses on building a scalable gameplay architecture using Entities, Components, Authoring Bakers and Systems.

## Overview

The objective is to build a Tower Defense game where enemies follow a predefined path towards the player's base while defensive towers attack enemies within their range.

The player must defend the base by eliminating incoming enemies, earning rewards and surviving progressively challenging waves.

## Tech Stack

- **Game Engine:** Unity 6 (6000.0.30f1)
- **Programming Language:** C#
- **Architecture:** Unity DOTS / ECS
- **Entities:** 1.3.8
- **Entities Graphics:** 1.4.5
- **Render Pipeline:** Universal Render Pipeline (URP)

## Project Structure

```text
DOTS_Project/
│
├── Assets/
│   ├── 1-HealthRegen/
│   │
│   ├── Assets/
│   │   ├── Scripts/
│   │   │   ├── Authoring/
│   │   │   ├── Components/
│   │   │   └── Systems/
│   │   │
│   │   ├── Tower/
│   │   ├── Tower Defence/
│   │   ├── RPG Monster DUO PBR Polyart/
│   │   └── ...
│   │
│   ├── Scenes/
│   ├── Fonts/
│   └── Settings/
│
├── Packages/
├── ProjectSettings/
└── README.md
```

## Gameplay Systems

### 1. Enemy Management
- Enemy entities are created through Authoring and Baking.
- Enemy health, movement speed and reward values are stored as ECS components.
- Enemies are spawned from an entity prefab.

### 2. Enemy Movement
- Enemies follow a predefined waypoint path.
- Movement is processed through an ECS System.
- Enemies reaching the final waypoint damage the player's base.

### 3. Tower Combat
- Towers detect enemies within their attack range.
- The nearest valid enemy is selected as the target.
- Towers apply damage according to their configured damage and fire rate.

### 4. Enemy Death and Rewards
- Enemies are destroyed when their health reaches zero.
- The player receives currency for eliminating enemies.

### 5. Wave Progression
- Enemies spawn in successive waves.
- A delay is introduced between waves.
- Enemy health and movement speed increase with each new wave.

### 6. Base Health and Game State
- The player's base loses health when enemies reach it.
- The game enters the Game Over state when base health reaches zero.

## Getting Started

### Prerequisites

- Unity Hub
- Unity 6 (6000.0.30f1)
- Visual Studio or Visual Studio Code
- Unity Entities package

### Setup Instructions

1. Clone or download this repository.
2. Open the project using Unity Hub.
3. Use the specified Unity editor version.
4. Open the project scene.
5. Create an enemy prefab and configure its `EnemyAuthoring` component.
6. Create a Game Manager object and attach `GameConfigAuthoring`.
7. Assign the enemy prefab and configure the waypoint transforms.
8. Add `TowerAuthoring` to tower objects.
9. Configure enemy, tower and wave settings.

## ECS Architecture

The project follows the Entity Component System architecture:

| Layer | Responsibility |
|---|---|
| Authoring | Configures GameObjects and converts them into entities |
| Components | Stores gameplay data |
| Systems | Processes gameplay logic |
| Entities | Represents enemies, towers and game configuration |

Gameplay logic is separated from data to keep the architecture modular and suitable for DOTS-based development.

## Current Development Status

The project currently contains the foundation for:

- Enemy spawning
- Waypoint-based enemy movement
- Tower targeting and damage
- Enemy death and rewards
- Wave progression
- Base health management
- Game Over state

### Planned Improvements

- Projectile-based tower attacks
- Tower placement and purchasing
- Gameplay UI and currency display
- Wave information and progression UI
- Tower animations and visual effects
- Victory conditions
- Audio and gameplay polish

## Project Goal

The long-term goal is to develop a complete playable Tower Defense game while exploring Unity DOTS, ECS architecture, entity-based gameplay systems and data-oriented programming.
