using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AIConfig
{
    public float maxEntitySearchDist;
    public float movingCostMult;
    public float fishingCostMult;
    public float moneyGainMult;
    public float moneyLossMult;
    public float itemGainMult;
    public float itemLossMult;

    //public AIConfig(float maxEntitySearchDist = 1000, float movingCostMult = 1, float fishingCostMult = 1, float moneyGainMult = 1, float moneyLossMult = 1, float itemGainMult = 1, float itemLossMult=1)
    //{
    //    this.maxEntitySearchDist = maxEntitySearchDist;
    //    this.movingCostMult = movingCostMult;
    //    this.fishingCostMult = fishingCostMult;
    //    this.moneyGainMult = moneyGainMult;
    //    this.moneyLossMult = moneyLossMult;
    //    this.itemGainMult = itemGainMult;
    //    this.itemLossMult = itemLossMult;
    //}

    public static AIConfig neutral = new AIConfig() { maxEntitySearchDist=1000, movingCostMult = 1, fishingCostMult = 1, moneyGainMult = 1, moneyLossMult = 1, itemGainMult = 1, itemLossMult = 1 };
}
