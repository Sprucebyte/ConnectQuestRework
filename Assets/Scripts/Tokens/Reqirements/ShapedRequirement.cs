using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ShapedRequirement", menuName = "Activation Requirements/Shaped Requirement")]
public class ShapedRequirement : TokenActivationRequirement
{
    [HorizontalLine(2f, EColor.Blue)]
    [SerializeField] public Token centerToken;
    
    [Serializable]
    public class RequiredTokens
    {
        [HorizontalLine(2f, EColor.Green)]
        [SerializeField] public List<Token> validTokens = new List<Token>();
        [SerializeField] public Vector2Int relativePosition = new Vector2Int();
    }
    [HorizontalLine(2f, EColor.Red)]
    public List<RequiredTokens> requiredTokens = new List<RequiredTokens>();
    public override bool IsMet(Board board, Board.Slot slot, out List<Board.Slot> slots)
    {
        slots = new List<Board.Slot>();
        foreach (RequiredTokens requiredToken in requiredTokens) {
            if (slot.x + requiredToken.relativePosition.x >= board.boardSize.x) return false;
            if (slot.y + requiredToken.relativePosition.y >= board.boardSize.y) return false;
            if (slot.x + requiredToken.relativePosition.x < 0) return false;
            if (slot.y + requiredToken.relativePosition.y < 0) return false;
            Board.Slot checkSlot = board.slots[slot.x + requiredToken.relativePosition.x, slot.y + requiredToken.relativePosition.y];
            if (checkSlot.obj == null) return false;
            //if (checkSlot.obj.GetComponent<Token>().id != requiredToken.validTokens[0].id) return false;
            

            bool hasValidToken = false;
            foreach (Token validToken in requiredToken.validTokens) { 
               if (checkSlot.obj.GetComponent<Token>().id == validToken.id) hasValidToken = true;
            }
            if (!hasValidToken) return false;
            slots.Add(checkSlot);
        }
        return true;
    }
}

