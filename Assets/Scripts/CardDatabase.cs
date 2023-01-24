using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> mafiaCardList = new List<Card>();
    public static List<Card> policeCardList = new List<Card>();
    public static List<Card> peopleCardList = new List<Card>();
    public static List<Card> additionalCardList = new List<Card>();

    void Awake()
    {
        mafiaCardList.Add(new Card(0, "Errand Boy", "Mafia", 1, 2, 2, "", Resources.Load<Sprite>("ErrandBoy"), "Red"));
        mafiaCardList.Add(new Card(1, "Doberman", "Mafia", 1, 2, 2, "Calls into battle a dog gangster with half of its characteristics", Resources.Load<Sprite>("Doberman"), "Red"));
        mafiaCardList.Add(new Card(2, "Lone Wolf", "Mafia", 2, 3, 3, "All your played cards have +1 attack except this one", Resources.Load<Sprite>("LoneWolf"), "Red"));
        mafiaCardList.Add(new Card(3, "Gun For Hire", "Mafia", 2, 4, 5, "Shoots enemies from a distance", Resources.Load<Sprite>("GunForHire"), "Red"));
        policeCardList.Add(new Card(0, "Trainee", "Police", 1, 2, 3, "Shoots his enemies (no damage taken while attacking)", Resources.Load<Sprite>("Trainee"), "Blue"));
        policeCardList.Add(new Card(1, "Detective", "Police", 1, 2, 6, "", Resources.Load<Sprite>("Detective"), "Blue"));
        policeCardList.Add(new Card(2, "Chief Of Police", "Police", 2, 3, 6, "Reduces enemy cards' attack by 1", Resources.Load<Sprite>("ChiefOfPolice"), "Blue"));
        policeCardList.Add(new Card(3, "Squad", "Police", 2, 6, 5, "", Resources.Load<Sprite>("Squad"), "Blue"));
        peopleCardList.Add(new Card(0, "Peasant", "The People", 1, 2, 2, "Strikes with a pitchfork from a distance", Resources.Load<Sprite>("Peasant"), "Gray"));
        peopleCardList.Add(new Card(1, "Bypasser", "The People", 1, 3, 3, "", Resources.Load<Sprite>("Bypasser"), "Gray"));
        peopleCardList.Add(new Card(2, "The Cook", "The People", 2, 3, 4, "All your played cards get +2 health", Resources.Load<Sprite>("TheCook"), "Gray"));
        peopleCardList.Add(new Card(3, "Blacksmith", "The People", 2, 6, 6, "", Resources.Load<Sprite>("Blacksmith"), "Gray"));
        additionalCardList.Add(new Card(0, "Dog Gangster", "Mafia", 0, 1, 1, "", Resources.Load<Sprite>("DogGangster"), "Red"));
    }
}
