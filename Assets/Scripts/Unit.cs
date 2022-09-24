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
    public bool shielded = false;

    public bool TakeDamage(int dmg)
    {
        if(shielded == true){
            currentHP -= (dmg/2);
        } else {
            currentHP -= dmg;
        }

        if(currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int heal)
    {
        currentHP += heal;
        if(currentHP > maxHP)
           currentHP = maxHP;
    }

    public void Blocking()
    {
        shielded = true;
    }

    public void NotBlocking()
    {
        shielded = false;
    }
}
