using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITokenActivationRequirement
{
    bool IsMet(Board board, int row, int column);
}


[Serializable]
public class ActivationRequirementGroup
{
    public List<TokenActivationRequirement> ActivationRequirements;
}

