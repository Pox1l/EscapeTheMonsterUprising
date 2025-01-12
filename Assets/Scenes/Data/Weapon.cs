using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;        // Jméno zbranì
    public Sprite weaponSprite;      // Ikona nebo obrázek zbranì
    public GameObject weaponPrefab;  // Prefab pro zbraò
    public GameObject bulletPrefab;  // Prefab pro støelu
    public float fireRate = 0.5f;    // Rychlost støelby
    public int damage = 10;          // Poškození
    public int cost = 100;           // Cena
}
