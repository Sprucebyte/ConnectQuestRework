using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour 
{
    enum States
    {
        waiting,
        attacking,
        dead
    }


    [SerializeField] string displayName = "Unnamed";
    [SerializeField] string description = "...";

    [SerializeField] float health = 100;

    [SerializeField] Util.Damage attackStrengthsMultiplier  = new Util.Damage(1);
    [SerializeField] Util.Damage attackWeaknessesMultiplier = new Util.Damage(1);
    [SerializeField] Util.Damage healthStrengthsMultiplier  = new Util.Damage(1);
    [SerializeField] Util.Damage healthWeaknessesMultiplier = new Util.Damage(1);

    Canvas canvas;
    [SerializeField] GameObject nametagPrefab;
    GameObject nametagObject;
    TMP_Text nametag;

    public Util.Damage GetAttackStrength() { return attackStrengthsMultiplier;  }
    public Util.Damage GetAttackWeakness() { return attackWeaknessesMultiplier; }
    public Util.Damage GetHealthStrength() { return healthStrengthsMultiplier;  }
    public Util.Damage GetHealthWeakness() { return healthWeaknessesMultiplier; }

    public void DealDamage(Util.Damage damage)
    {
        foreach (var field in typeof(Util.Damage).GetFields())
        {
            health -= Mathf.Round((float)field.GetValue(damage) / (float)field.GetValue(healthStrengthsMultiplier) * (float)field.GetValue(healthWeaknessesMultiplier));
        }
    }


    private void Start()
    {
        canvas = FindFirstObjectByType<Canvas>();
        nametagObject = Instantiate(nametagPrefab, canvas.transform);
        nametag = nametagObject.GetComponent<TMP_Text>();
        
        nametag.text = name;

        DealDamage(new Util.Damage()
        {
            fire = 2,
            blunt = 5,
            slashing = 1,
        });
    }

    private void Update()
    {
        //nametagObject.transform.position = transform.position + (transform.up * 40);
    }

}
