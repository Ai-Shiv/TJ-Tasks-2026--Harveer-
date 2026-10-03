# TJ-Tasks-2026--Harveer-
This Repo is for TJ Interview 2026!

# Top-Down Survival Game

A simple 2D top-down survival game made in Unity. The objective is to survive against continuously spawning enemies for as long as possible while defeating enemies to increase the score.

## Gameplay

- Move using **WASD**.
- The player continuously faces the **mouse cursor**.
- Hold **Right Mouse Button** to shoot toward the mouse.
- Normal enemies continuously move toward the player and damage the player on contact.
- Ranged enemies move more slowly and periodically shoot projectiles at the player.
- Defeating enemies increases the score:
  - Normal enemy: **+1**
  - Ranged enemy: **+2**
- Enemies can drop collectible health pickups.
- The difficulty increases over time through a wave system.
- The game ends when the player's health reaches zero.
- A Game Over screen displays the final score and provides a restart button.

## Approach and Algorithm

The game is implemented using separate Unity components for movement, aiming, combat, health, enemy behaviour, spawning, game state, UI, and audio. This keeps each major responsibility isolated and makes the project easier to modify or extend.

### Main Game Loop

```text
Start Game
    |
    v
Initialize player, GameManager, UI and EnemySpawner
    |
    v
While game is active:
    - Read WASD input and move the player
    - Read mouse position and rotate player toward it
    - Check Right Mouse Button and fire projectiles
    - Spawn enemies at random positions around the player
    - Move enemies toward the player
    - Let ranged enemies stop at a safe distance and shoot
    - Process projectile collisions and enemy damage
    - Process player contact/ranged damage
    - Update score and health UI
    - Increase difficulty as survival time increases
    |
    v
Player health <= 0
    |
    v
Set Game Over state
Stop gameplay using Time.timeScale = 0
Show final score
    |
    v
Restart scene
```

## Major Systems

### Player Movement

`PlayerMovement` uses Unity's **New Input System**. The `Move` action is a `Vector2` created with a 2D Vector Composite for W/A/S/D. The input is clamped to prevent faster diagonal movement, and the Rigidbody2D velocity is updated in `FixedUpdate`.

**Algorithm:**

```text
Read Move Vector2
      |
Clamp magnitude to 1
      |
Multiply by move speed
      |
Apply to Rigidbody2D velocity
```

### Player Aiming

`PlayerAim` reads the mouse screen position, converts it into world coordinates using the main camera, calculates the direction from the player to the mouse, and converts that direction into a 2D rotation using `atan2`.

The player root rotates toward the cursor so the triangle character visibly faces the mouse. The `AimPivot`/`FirePoint` are kept at the player's tip for projectile spawning.

**Algorithm:**

```text
Mouse screen position
        |
ScreenToWorldPoint()
        |
Mouse world position - Player position
        |
Normalize direction
        |
atan2(direction.y, direction.x)
        |
Rotate Player toward mouse
```

### Player Shooting

`Weapon` checks the `Shoot` action from the New Input System. The shoot action is bound to the **Right Mouse Button**. A fire-rate timer prevents shooting faster than the configured shots-per-second value.

A projectile is instantiated at the `FirePoint` and receives the calculated aim direction.

**Algorithm:**

```text
Right Mouse Button held?
        |
       Yes
        |
Is cooldown finished?
        |
       Yes
        |
Create projectile at FirePoint
        |
Give projectile aim direction
        |
Start next-shot cooldown
```

### Projectile System

`Projectile` uses a Rigidbody2D for movement. The projectile receives a normalized direction and moves at a fixed speed. A lifetime automatically destroys missed projectiles.

The projectile collider is configured as a trigger. When it enters an enemy collider, it calls `EnemyHealth.TakeDamage()` and destroys itself.

### Normal Enemy

`EnemyMovement` finds the object tagged `Player` and calculates a normalized direction toward the player. Its Rigidbody2D velocity is set to that direction multiplied by the enemy's movement speed.

`EnemyDamage` uses `OnCollisionStay2D` with a damage cooldown so an enemy touching the player does not deal damage every physics frame.

### Ranged Enemy

`RangedEnemy` is a second enemy type. It moves more slowly than a normal enemy and stops once it reaches its shooting distance. It then periodically creates an `EnemyProjectile` aimed at the player.

This creates two simple enemy behaviours:

```text
Normal Enemy  -> chase player -> contact damage
Ranged Enemy  -> approach -> stop at range -> shoot
```

### Enemy Spawning

`EnemySpawner` creates enemies at random directions and random distances around the player, keeping them away from spawning directly on top of the player.

A maximum enemy count prevents unlimited objects from accumulating.

The spawner also controls the wave-based difficulty. As the current wave increases, the spawn interval decreases, and the chance of spawning a ranged enemy increases up to a maximum value.

**Algorithm:**

```text
Every spawn interval:
    Choose random direction
    Choose random distance
    Position = Player position + direction * distance

    Calculate current wave
    Calculate ranged-enemy chance

    Random chance succeeds?
        -> Spawn Ranged Enemy
    Otherwise
        -> Spawn Normal Enemy
```

### Wave System

`GameManager` tracks survival time. A new wave is reached after a fixed amount of time (currently 15 seconds per wave).

The wave number is calculated from survival time:

```text
CurrentWave = floor(SurvivalTime / waveDuration) + 1
```

The wave number is used by `EnemySpawner` to increase difficulty. The wave is intentionally not displayed in the HUD, but it still affects gameplay.

### Score System

`GameManager` stores the current score. When an enemy dies, `EnemyHealth` calls `GameManager.AddScore()` using the enemy's configured score value.

```text
Normal enemy death  -> +1
Ranged enemy death  -> +2
```

Because the score is awarded inside the enemy's death method, a projectile hit only gives points when the enemy is actually defeated.

### Player Health and Game Over

`PlayerHealth` stores the current and maximum health. Damage decreases current health, while pickups restore health without exceeding the maximum.

When health reaches zero, `GameManager.GameOver()` is called.

The GameManager then:

1. Sets `IsGameOver = true`.
2. Plays the player death sound.
3. Sets `Time.timeScale = 0` to freeze gameplay.
4. Allows the UI to display the Game Over screen.

Gameplay scripts check the GameManager's game-over state and stop their normal behaviour when the game has ended.

### Health Pickups

`HealthPickup` uses a trigger collider. When collected by the player, it calls `PlayerHealth.Heal()` and then destroys itself.

Enemies have a configurable health-drop chance. The current prototype uses an increased drop chance of **30%** to make pickups appear regularly during testing/gameplay.

### UI

`GameUI` updates the HUD every frame with:

- Current score
- Current health

When the GameManager enters the Game Over state, it activates the Game Over panel and displays the final score.

The Restart button reloads the active scene and resets the game state.

### Audio

An `AudioManager` centralizes audio playback.

It provides separate sounds for:

- Background music
- Player shooting
- Enemy shooting
- Enemy death
- Health pickup
- Player death / Game Over

The background music loops through a dedicated AudioSource, while gameplay effects use `PlayOneShot()` through the SFX AudioSource.

## Project Structure

```text
Assets/
├── Scenes/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerMovement.cs
│   │   ├── PlayerAim.cs
│   │   ├── PlayerHealth.cs
│   │   └── HealthPickup.cs
│   ├── Enemy/
│   │   ├── EnemyMovement.cs
│   │   ├── EnemyDamage.cs
│   │   ├── EnemyHealth.cs
│   │   ├── EnemySpawner.cs
│   │   └── RangedEnemy.cs
│   ├── Combat/
│   │   ├── Weapon.cs
│   │   ├── Projectile.cs
│   │   └── EnemyProjectile.cs
│   ├── Systems/
│   │   ├── GameManager.cs
│   │   └── AudioManager.cs
│   └── UI/
│       └── GameUI.cs
├── Prefabs/
├── Sprites/
├── Audio/
└── UI/
```

## Controls

| Action | Input |
|---|---|
| Move | W / A / S / D |
| Aim | Mouse |
| Shoot | Right Mouse Button |

## Future Extensions

Possible extensions include more enemy types, additional weapons, stronger visual effects, particle effects, screen shake, more detailed animations, obstacles/arena layouts, power-ups, and more advanced wave patterns.

