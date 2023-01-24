using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ThisHero : MonoBehaviour
{

    public List<Hero> thisHero = new List<Hero>();

    public int id;
    public string cardName;
    public int hp;
    public string cardDescription;
    public string minionType;

    public HeroPower heroPower;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI minionText;

    public Sprite thisSprite;
    public Image spriteImage;

    public Sprite heroPowerSprite;
    public Image heroPowerImage;

    public Image frame;

    public void SetDataByHeroId(int id)
    {
        thisHero[0] = HeroDatabase.heroList[id];
        UpdateData();
    }
    public void UpdateHeroHp(int health)
    {
        hp += health;

        hpText.text = "" + hp;
    }

    void UpdateData()
    {
        id = thisHero[0].id;
        cardName = thisHero[0].cardName;
        hp = thisHero[0].hp;
        heroPower = HeroPowerDatabase.powerList[thisHero[0].heroPowerNumber];
        cardDescription = heroPower.name + ": " + heroPower.description;
        minionType = thisHero[0].minionType;

        thisSprite = thisHero[0].thisImage;
        heroPowerSprite = heroPower.image;

        nameText.text = "" + cardName;
        hpText.text = "" + hp;
        descriptionText.text = "" + cardDescription;
        minionText.text = "" + minionType;

        spriteImage.sprite = thisSprite;
        heroPowerImage.sprite = heroPowerSprite;

        if (thisHero[0].color == "Red")
        {
            frame.GetComponent<Image>().color = new Color(255, 0, 0, 255);
        }
        else if (thisHero[0].color == "Blue")
        {
            frame.GetComponent<Image>().color = new Color(0, 0, 255, 255);
        }
        else if (thisHero[0].color == "Gray")
        {
            frame.GetComponent<Image>().color = new Color(126, 126, 126, 255);
        }
    }

}
