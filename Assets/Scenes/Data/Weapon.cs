using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;        // Jm�no zbran�
    public Sprite weaponSprite;      // Ikona nebo obr�zek zbran�
    public GameObject weaponPrefab;  // Prefab pro zbra�
    public GameObject bulletPrefab;  // Prefab pro st�elu
    public float fireRate = 0.5f;    // Rychlost st�elby
    public int damage = 10;          // Po�kozen�
    public int cost = 100;           // Cena
}
