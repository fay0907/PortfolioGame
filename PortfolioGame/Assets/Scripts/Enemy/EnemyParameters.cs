using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameters : MonoBehaviour
{
    public enum EnemyType
    {
        Beast,
        Undead,
        Humanoid,
        Archer
    }
    public int Health;
    public string Name;
}
