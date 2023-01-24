using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]

public class HeroPower 
{
    public int id;
    public string description;
    public string name;
    public int cost;

    public Sprite image;

    public HeroPower()
    {

    }

    public HeroPower(int Id, string HeroPowerName, string HeroPowerDescription, int Cost, Sprite Image)
    {
        id = Id;
        description = HeroPowerDescription;
        name = HeroPowerName;
        cost = Cost;
        image = Image;
    }
}
