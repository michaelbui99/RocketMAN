# Weapon Specifications
All weapons should follow this spec for consistency

## Assumptions
- Each weapon has a prefab
- The weapon prefab should contain an AudioSource in it's object root that is to be played when firing the weapon

## Adding new weapons
- Create a folder with the weapons name with the following structure:
  - Audio
  - Prefabs
  - Scripts
- Create prefab for weapon
- Add a weapon module that implements IWeaponModule
- Register weapon name in <em>Weapons/Common/SupportWeaponModules</em>
- Register weapon prefab in <em>Weapons/Common/WeaponPrefabRefs</em>

## Component Responsibilities
- 