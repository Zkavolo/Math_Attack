using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    static System.Random r = new System.Random();
    static double epsilon = 1e-6;

    public List<TreeNode> children;
    public double nVisits, totValue;
    public double uctValue;
    public double value;

    public UnitStats unitstats;

    public TreeNode(UnitStats unitstats)
    {
        children = new List<TreeNode>();
        nVisits = 0;
        totValue = 0;

        this.unitstats = unitstats;
        // Debug.Log(this.unitstats.userAction);
        // Debug.Log(this.unitstats.pHP);
        // Debug.Log(this.unitstats.eHP);
        // Debug.Log(this.unitstats.Pult);
        // Debug.Log(this.unitstats.Eult);
        // Debug.Log(this.unitstats.Pheal);
        // Debug.Log(this.unitstats.EHeal);
        // Debug.Log(this.unitstats.Pdmg);
        // Debug.Log(this.unitstats.Edmg);
        // Debug.Log(this.unitstats.Pblocking);
        // Debug.Log(this.unitstats.Eblocking);
    }

    public void iterateMCTS()
    {
        //make a new list for visited

        LinkedList<TreeNode> visited = new LinkedList<TreeNode>();
        TreeNode cur = this;

        //Add cur unitstats to visited list

        visited.AddLast(this);

        //1. SELECTION

        while (!cur.isLeaf())
        {
            cur = cur.select();

            visited.AddLast(cur);
        }
        if (cur.unitstats.GameResult == 0)
        {
            //2. EXPANSION
            cur.expand();

            //3. SIMULATION
            int chicount = 0;
            foreach(TreeNode child in cur.children){
            child.value = child.simulate();
            visited.AddLast(child);
            chicount++;
            }

            foreach (TreeNode node in visited)
            {
                //4. BACKPROPAGATION
                // Debug.Log(node.unitstats.enemyAction+" value of node "+node.value);
                node.updateStats(node.value);
            }
        }
    }

    public void expand()
    {
        List<int> childrenActions = listLegalActions();
        //Apply one move for each expansion child
        foreach (int act in childrenActions)
        {
            TreeNode childNode = new TreeNode(new UnitStats(unitstats.userAction, unitstats.enemyAction, unitstats.pHP, unitstats.eHP, unitstats.Pult, unitstats.Eult, unitstats.Pheal, unitstats.EHeal, unitstats.Pdmg, unitstats.Edmg, unitstats.Pblocking, unitstats.Eblocking));
        
            childNode.unitstats.enemyAction = act;
            childNode.unitstats.GameResult = checkGameRes(childNode.unitstats, act);
            children.Add(childNode);
        }
    }

    public List<int> listLegalActions()
    {   
        List<int> LegalActions = new List<int>();
        for(int i = 1; i <= 4; i ++)
            {
                if (i == 1)
                {   
                    if(unitstats.Eult == 3)
                    {
                        LegalActions.Add(i);
                    }
                    else
                    {
                        // Debug.Log("move not legal");
                    }
                }
                else if (i == 4)
                {
                    if(unitstats.EHeal == 0)
                    {
                        // Debug.Log("move not legal");
                    }
                    else
                    {
                        LegalActions.Add(i);
                    }
                }
                else
                {
                    LegalActions.Add(i);
                }
            }
        return LegalActions;
    }

    public TreeNode select()
    {
        TreeNode selected = null;
        double bestValue = Double.MinValue;
        // Debug.Log("BestValue ="+bestValue);
        foreach (TreeNode c in children)
        {
            //UCT value calculation
            double uctValue =
                    c.totValue / (c.nVisits + epsilon) +
                            (Math.Sqrt(2)*(Math.Sqrt(Math.Log(nVisits + 1) / (c.nVisits + epsilon)))) +
                            r.NextDouble() * epsilon;
            c.uctValue = uctValue;
            // Debug.Log(uctValue);
            Debug.Log(c.unitstats.enemyAction+" "+uctValue);
            if (uctValue > bestValue)
            {
                selected = c;
                bestValue = uctValue;
            }
        }
        return selected;
    }

    public bool isLeaf()
    {
        return children.Count == 0;
    }

    //simulate battle until get result
    public double simulate()
    {
        //UnitStats simunitstats = new UnitStats(unitstats.userAction, unitstats.enemyAction, unitstats.pHP, unitstats.eHP, unitstats.Pult, unitstats.Eult, unitstats.Pheal, unitstats.EHeal, unitstats.Pdmg, unitstats.Edmg, unitstats.Pblocking, unitstats.Eblocking);
        // Debug.Log("sim enemy move"+simunitstats.enemyAction);

        int simValue = 0;
        int eCount = 0; 
        int pCount = 0;
        int n = 5;
        bool currentturn = true;
        List<int> tempCond = new List<int>();
        int numberOfwins = 0;
        int numOfSim = 0;
        // UnitStats simunitstats;
        // Debug.Log("temp Game Result "+tempsimunitstats.GameResult);

        for(int i = 0 ; i < 5 ; i++){

            UnitStats simunitstats = new UnitStats(unitstats.userAction, unitstats.enemyAction, unitstats.pHP, unitstats.eHP, unitstats.Pult, unitstats.Eult, unitstats.Pheal, unitstats.EHeal, unitstats.Pdmg, unitstats.Edmg, unitstats.Pblocking, unitstats.Eblocking);
            simunitstats.GameResult = unitstats.GameResult;

            while (simunitstats.GameResult == 0)
            {
                //Check winCond
                if(simunitstats.pHP <= 0)
                {
                    //1 for enemy win
                    simunitstats.GameResult = 1;
                    numberOfwins++;
                    // Debug.Log("enemy win");
                }
                else if (simunitstats.eHP <= 0)
                {
                    //-1 for player win
                    simunitstats.GameResult = -1;
                    // Debug.Log("player win");
                }

                //Player Turn because Enemy Turn has been done from expansion
                if(currentturn){

                simunitstats.Pblocking = false;

                if(simunitstats.Pult < 3 && simunitstats.Pheal == 0){
                   tempCond.Add(1);
                   tempCond.Add(4);
                 } else if(simunitstats.Pult < 3){
                   tempCond.Add(1);
                 } else if(simunitstats.Pheal == 0){
                   tempCond.Add(4);
                 } 
                
                 int simPAct = doRandomMove(n, tempCond);
                 // Debug.Log(currentturn+" "+pCount+" "+simPAct);

                    if(simPAct == 4)
                    {
                        simunitstats.pHP += simunitstats.Pdmg*5;
                        simunitstats.pHP = simunitstats.pHP > 100 ? simunitstats.pHP = 100 : simunitstats.pHP = simunitstats.pHP;
                        simunitstats.Pheal--;
                        // Debug.Log("Player Healed = "+simunitstats.Pheal+" Player HP ="+simunitstats.pHP);
                    }
                    else if(simPAct == 3)
                    {
                        simunitstats.Pblocking = true;
                        // Debug.Log("Player Blocking = "+simunitstats.Pblocking);
                    }
                    else if(simPAct == 1)
                    {
                        simunitstats.eHP = simunitstats.Eblocking ? simunitstats.eHP -= simunitstats.Pdmg : simunitstats.eHP -= (simunitstats.Pdmg*2);
                        simunitstats.Pult = 0;
                        // Debug.Log("Player Ults"+simunitstats.Pult+" Enemy HP = "+simunitstats.eHP);
                    }
                    else if(simPAct == 2)
                    {
                        simunitstats.eHP = simunitstats.Eblocking ? simunitstats.eHP -= (simunitstats.Pdmg/2) : simunitstats.eHP -= simunitstats.Pdmg;
                        simunitstats.Pult = simunitstats.Pult >= 3 ? simunitstats.Pult = 3 : simunitstats.Pult += 1;
                        // Debug.Log("Player attacked "+simunitstats.Pult+" Enemy HP = "+simunitstats.eHP);
                    }
                    else if(simPAct == 0)
                    {
                        // Debug.Log("Player failed to do an action");
                    }
                    currentturn = false;
                    pCount++;
                    tempCond.Clear();
                } 

                //Enemy turn
                else if(currentturn == false) {

                simunitstats.Eblocking = false;

                tempCond.Add(0);

                if(simunitstats.Eult < 3 && simunitstats.EHeal == 0){
                   tempCond.Add(1);
                   tempCond.Add(4);
                 } else if(simunitstats.Eult < 3){
                   tempCond.Add(1);
                 } else if(simunitstats.EHeal == 0){
                   tempCond.Add(4);
                 } 
                
                 int simEAct = doRandomMove(n, tempCond);
                 // Debug.Log(currentturn+" "+eCount+" "+simEAct);

                    if(simEAct == 4)
                    {
                        simunitstats.eHP += simunitstats.Edmg*5;
                        simunitstats.EHeal--;
                        simunitstats.eHP = simunitstats.eHP > 100 ? simunitstats.eHP = 100 : simunitstats.eHP = simunitstats.eHP;
                        // Debug.Log("Enemy Healed = "+simunitstats.EHeal+" Enemy HP ="+simunitstats.eHP);
                    }
                    else if(simEAct == 3)
                    {
                        simunitstats.Eblocking = true;
                        // Debug.Log("Enemy Blocking = "+simunitstats.Eblocking);
                    }
                    else if(simEAct == 1)
                    {
                        simunitstats.pHP = simunitstats.Pblocking ? simunitstats.pHP -= simunitstats.Edmg : simunitstats.pHP -= (simunitstats.Edmg*2);
                        simunitstats.Eult = 0;
                        // Debug.Log("Enemy Ults"+simunitstats.Eult+" Player HP = "+simunitstats.pHP);
                    }
                    else if(simEAct == 2)
                    {
                        simunitstats.pHP = simunitstats.Pblocking ? simunitstats.pHP -= (simunitstats.Edmg/2) : simunitstats.pHP -= simunitstats.Edmg;
                        simunitstats.Eult = simunitstats.Eult >= 3 ? simunitstats.Eult = 3 : simunitstats.Eult += 1;
                        // Debug.Log("Enemy attacked "+simunitstats.Eult+" Player HP = "+simunitstats.pHP);
                    }
                    eCount++;
                    currentturn = true;
                    tempCond.Clear();
                }
            //end of while
            }
            numOfSim++;
            // Debug.Log("current sim count = "+numOfSim+" Game Result = "+simunitstats.GameResult);
            // end of for    
        }

        // Debug.Log(tempsimunitstats.GameResult);
        //return result from sim
        simValue = numberOfwins;
        // Debug.Log("num of simulation = "+numOfSim);
        // Debug.Log("numofwins = "+simValue);
        return simValue;
    }

    public void updateStats(double value)
    {
        nVisits++;
        totValue += value;
    }

    public int checkGameRes(UnitStats unitstats, int curAct)
    {
        int result = 0;

        if(curAct == 1)
        {
            unitstats.pHP = unitstats.Pblocking ? unitstats.pHP -= unitstats.Edmg : unitstats.pHP -= (unitstats.Edmg*2);
            // Debug.Log("enemy Ults "+ unitstats.pHP);
        } 
        else if(curAct == 2)
        {
            unitstats.pHP = unitstats.Pblocking ? unitstats.pHP -= (unitstats.Edmg/2) : unitstats.pHP -= unitstats.Edmg;
            // Debug.Log("enemy Attacks "+ unitstats.pHP);
        }
        else if(curAct == 3)
        {
            unitstats.Eblocking = true;
            // Debug.Log("enemy Blocking "+ unitstats.Eblocking);
        }
        else if(curAct == 4)
        {
            unitstats.eHP += (unitstats.Edmg*5);
            unitstats.eHP = unitstats.eHP > 100 ? unitstats.eHP = 100 : unitstats.eHP = unitstats.eHP;
            // Debug.Log("enemy Healed "+ unitstats.eHP);
        }

        if(unitstats.eHP <= 0)
        {
            result = -1;
        }
        else if(unitstats.pHP <= 0)
        {
            result = 1;
        }
        return result;
    }

    public static int doRandomMove(int n, List<int> x) 
    {
    int result = r.Next(n - x.Count);

    for (int i = 0; i < x.Count; i++) 
    {
        if (result < x[i])
            return result;
        result++;
    }
    return result;
    }
}
