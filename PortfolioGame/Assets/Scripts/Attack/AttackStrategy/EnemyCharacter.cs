using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : EnemyParameters
{
    public EnemyType type;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Health = 100; // Example health value
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Debug.Log($"{Name} has been defeated!");
            Destroy(gameObject);
        }
    }
}
