using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardFlipper : NetworkBehaviour
{
    public GameObject CardBack;
    public void Flip()
    {        
        CardBack.SetActive(!CardBack.activeSelf);
    }
}
