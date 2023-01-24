using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Card
{
    public int id;
    public string cardName;
    public int cost;
    public int power;
    public int hp;
    public string cardDescription;
    public string minionType;
    public string color;
    public Sprite thisImage;

    public Card()
    {

    }

    public Card(int Id, string CardName, string MinionType, int Cost, int Power, int HP, string CardDescription, Sprite Image, string Color)
    {
        id = Id;
        cardName = CardName;
        minionType = MinionType;
        cost = Cost;
        power = Power;
        hp = HP;
        cardDescription = CardDescription;
        thisImage = Image;
        color = Color;        
    }
}
