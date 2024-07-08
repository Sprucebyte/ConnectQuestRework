using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    List<Character> characters = new List<Character>();
    Character currentCharacter;
    int currentCharacterValue;
    public void NextCharacter()
    {
        if (currentCharacterValue < characters.Count)
        {
            currentCharacterValue++;
        }
        else currentCharacterValue = 0;
        currentCharacter = characters[currentCharacterValue];


        currentCharacter.DealDamage(new Util.Damage() { 
            fire = 2,
            blunt = 5,
            slashing = 1,
        });


    }


    public int GetTeamSize() { return characters.Count;}
    public List<Character> GetCharacters() { return characters; }
    public Character GetCurrentCharacter() { return currentCharacter; }
    public int GetCurrentCharacterValue()  { return currentCharacterValue; }


}
