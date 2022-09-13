using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Description")]
    public string unitName;

    [Header("Unit Damage")]
    public int damage;

    [Header("Unit Health Points")]
    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0)
            return true;
        else
            return false;
    }
}
