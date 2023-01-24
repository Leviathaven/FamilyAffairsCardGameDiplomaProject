using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Hero 
{
    public int id;
    public string cardName;
    public int hp;
    public int heroPowerNumber;
    public string minionType;
    public string color;

    public Sprite thisImage;

    public Hero()
    {

    }

    public Hero(int Id, string CardName, string MinionType, int HeroPowerNumber, int HP, Sprite Image, string Color)
    {
        id = Id;
        cardName = CardName;
        minionType = MinionType;
        heroPowerNumber = HeroPowerNumber;
        hp = HP;
        thisImage = Image;
        color = Color;
    }
}
