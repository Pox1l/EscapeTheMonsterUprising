using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int money = 1000; // Hráèovy peníze
    public List<string> ownedWeapons = new List<string>(); // Seznam vlastnìných zbraní
    public string equippedWeapon = ""; // Aktuálnì vybavená zbraò
}
