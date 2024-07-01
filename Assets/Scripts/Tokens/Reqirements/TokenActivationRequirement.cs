using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//[CreateAssetMenu(fileName = "TokenActivationRequirement", menuName = "Scriptable Objects/Token Activation Requirement")]
public class TokenActivationRequirement : ScriptableObject
{

    public virtual bool IsMet(Board board, Board.Slot slot, out List<Board.Slot> slots)
    {
        slots = new List<Board.Slot>();
        return false;
    }

}

