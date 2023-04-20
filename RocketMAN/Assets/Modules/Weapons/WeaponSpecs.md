# Weapon Specifications
All weapons should follow this spec for consistency

## Assumptions
- Each weapon has a prefab
- The weapon prefab <strong>must</strong> contains a MonoBehaviour that implements IWeapon at its root
- The weapon prefab has a projectile launcher at it's root

## Adding new weapons
- Create a folder with the weapons name with the following structure:
  - Audio
  - Prefabs
  - Scripts
  - Models
  - Materials
  - Particles
- Create prefab for weapon
- Add a weapon module that implements IWeaponModule
- Register weapon name in <em>Weapons/Common/SupportWeaponModules</em>
- Register weapon prefab in <em>Weapons/Common/WeaponPrefabRefs</em>


## Concept
- Weapon Manager
  - Handles which weapon is currently selected
  - Ensures weapon is following the player
  - Emits events about the state of the current weapon
  - Handles weapon inputs and delegates them to the weapon
- WeaponModule
  - Exports the correct prefab such that a weapon can be instantiated by the WeaponManager
  - Contains metadata about the weapon such as reload behaviour, weapon name etc.
- Weapon
  - Handles the logic of the weapon
- Projectile Launcher
  - Launches projectiles 
  - The type of the launcher will determine the projectile's trajectory 
- Projectile
  - The projectile that is actually launched when firing the weapon
  - Contains logic about what should happen when the projectile hits a target