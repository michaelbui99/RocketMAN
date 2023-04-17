# Weapon Specifications
All weapons should follow this spec for consistency

## Assumptions
- Each weapon has a prefab
- The weapon prefab <strong>must</strong> contains a MonoBehaviour that implements IWeapon at its root

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