using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCharactersTest : MonoBehaviour
{
    [SerializeField] Character character1;
    [SerializeField] Character character2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) character1.DealDamage(new Util.Damage() { blunt = 10});
        if (Input.GetKeyDown(KeyCode.Space)) character2.DealDamage(new Util.Damage() { poison = 10});

    }
}
