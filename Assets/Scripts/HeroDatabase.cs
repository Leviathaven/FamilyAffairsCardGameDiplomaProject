using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDatabase : MonoBehaviour
{
    public static List<Hero> heroList = new List<Hero>();

    void Awake()
    {  
        heroList.Add(new Hero (0, "Rosalia", "Mafia", 0, 30, Resources.Load<Sprite>("RosaliaArt"), "Red"));
        heroList.Add(new Hero(1, "Captain Buckley", "Police", 1, 30, Resources.Load<Sprite>("BuckleyArt"), "Blue"));
        heroList.Add(new Hero(2, "Jackie", "The People", 2, 30, Resources.Load<Sprite>("JackieArt"), "Gray"));
    }
}
