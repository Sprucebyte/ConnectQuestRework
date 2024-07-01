using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.PlasticSCM.Editor.WebApi.CredentialsResponse;

public class RadiusRequirement : ITokenActivationRequirement
{
    public TokenType requiredType;
    public int radius;

    public bool IsMet(Board board, int row, int column)
    {
        /*
        for (int r = -radius; r <= radius; r++)
        {
            for (int c = -radius; c <= radius; c++)
            {
                if (r == 0 && c == 0) continue; // Skip the center token
                int checkRow = row + r;
                int checkColumn = column + c;
                if (checkRow >= 0 && checkRow < board.rows && checkColumn >= 0 && checkColumn < board.columns)
                {
                    if (board.GetToken(checkRow, checkColumn)?.type == requiredType)
                    {
                        return true;
                    }
                }
            }
        }*/
        return false;
    }
}
