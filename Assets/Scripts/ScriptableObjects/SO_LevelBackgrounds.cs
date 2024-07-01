using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBackgrounds", menuName = "Scriptable Objects/Level Backgrounds")]
public class SO_LevelBackgrounds : ScriptableObject
{
    [SerializeField] public List<Background> backgrounds;

}