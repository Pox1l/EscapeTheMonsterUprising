using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int money = 1000; // Hr��ovy pen�ze
    public List<string> ownedWeapons = new List<string>(); // Seznam vlastn�n�ch zbran�
    public string equippedWeapon = ""; // Aktu�ln� vybaven� zbra�
}
