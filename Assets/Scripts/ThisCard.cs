using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    
    public int id;
    public string cardName;
    public int power;
    public int hp;
    public string cardDescription;
    public string minionType;
    public int cost;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI minionText;
    public TextMeshProUGUI costText;

    public Sprite thisSprite;
    public Image spriteImage;

    public Image frame;

    public void SetDataByCardId(int id, int type)
    {
        if (type == 0)
        {
            thisCard[0] = CardDatabase.mafiaCardList[id];
        }
        else if (type == 1)
        {
            thisCard[0] = CardDatabase.policeCardList[id];
        }
        else if (type == 2)
        {
            thisCard[0] = CardDatabase.peopleCardList[id];
        }
        else if (type == 3)
        {
            thisCard[0] = CardDatabase.additionalCardList[id];
        }
        else
        {
            Debug.Log("There is no type of that kind");
        }
        UpdateData();
    }

    public void UpdateCardHpAndAttack(int health, int attack)
    {
        hp += health;
        power += attack;
        if (power < 0)
            power = 0;
        powerText.text = "" + power;
        hpText.text = "" + hp;
    }

    public void SetDataByCard(ThisCard data)
    {
        thisCard[0] = data.thisCard[0];
        UpdateData();
    }

    void UpdateData()
    {        
        id = thisCard[0].id;
        cardName = thisCard[0].cardName;
        power = thisCard[0].power;
        hp = thisCard[0].hp;
        cardDescription = thisCard[0].cardDescription;
        minionType = thisCard[0].minionType;
        cost = thisCard[0].cost;
        thisSprite = thisCard[0].thisImage;

        nameText.text = "" + cardName;
        powerText.text = "" + power;
        hpText.text = "" + hp;
        descriptionText.text = "" + cardDescription;
        minionText.text = "" + minionType;
        costText.text = "" + cost;

        spriteImage.sprite = thisSprite;

        if (thisCard[0].color == "Red")
        {
            frame.GetComponent<Image>().color = new Color(255, 0, 0, 255);
        }
        else if (thisCard[0].color == "Blue")
        {
            frame.GetComponent<Image>().color = new Color(0, 0, 255, 255);
        }
        else if (thisCard[0].color == "Gray")
        {
            frame.GetComponent<Image>().color = new Color(126, 126, 126, 255);
        }
    }

}
