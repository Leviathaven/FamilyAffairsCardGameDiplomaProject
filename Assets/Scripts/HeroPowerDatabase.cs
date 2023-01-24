using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPowerDatabase : MonoBehaviour
{
    public static List<HeroPower> powerList = new List<HeroPower>();

    void Awake()
    {
        powerList.Add(new HeroPower(0, "Temptation", "Random enemy card card attacks other random enemy card", 2, Resources.Load<Sprite>("RosaliaHeroPower")));
        powerList.Add(new HeroPower(1, "Gunned!", "Deal 3 damage to enemy card or enemy hero randomly", 2, Resources.Load<Sprite>("BuckleyHeroPower")));
        powerList.Add(new HeroPower(2, "Berzerk", "Deal 1 damage 3 times to one of enemy's cards and 1 time to one of player's card", 2, Resources.Load<Sprite>("JackieHeroPower")));
    }
}
