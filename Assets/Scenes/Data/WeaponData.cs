using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float fireRate;
    public int ammoCapacity;
    public bool isUnlocked;
}