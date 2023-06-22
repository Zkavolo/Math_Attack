using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats
{
    [Header("Unit Stats")]
    public int userAction;
    public int enemyAction;
    public int pHP;
    public int eHP;
    public int Pult;
    public int Eult;
    public int Pheal;
    public int EHeal;
    public int Pdmg;
    public int Edmg;
    public bool Pblocking;
    public bool Eblocking;
    public int playout;

    public int GameResult;

    // Stores all game variables that affects gameplay
    public UnitStats (int userAction, int enemyAction, int pHP, int eHP, int Pult, int Eult, int Pheal, int EHeal, int Pdmg, int Edmg, bool Pblocking, bool Eblocking, int playout)
    {
        
        this.userAction = userAction;
        this.enemyAction = enemyAction;
        this.pHP = pHP;
        this.eHP = eHP;
        this.Pult = Pult;
        this.Eult = Eult;
        this.Pheal = Pheal;
        this.EHeal = EHeal;
        this.Pdmg = Pdmg;
        this.Edmg = Edmg;
        this.Pblocking = Pblocking;
        this.Eblocking = Eblocking;
        this.playout = playout;

        GameResult = 0;
    }
}
