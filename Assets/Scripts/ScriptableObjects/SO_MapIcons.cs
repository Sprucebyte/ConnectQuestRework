using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapIconSprites", menuName = "Scriptable Objects/Map Icon Sprites")]
public class SO_MapIcons: ScriptableObject
{
    [SerializeField] public Sprite fight;
    [SerializeField] public Sprite boss;
    [SerializeField] public Sprite shop;
    [SerializeField] public Sprite chest;
    [SerializeField] public Sprite random;
}
