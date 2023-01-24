using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PickDeck : NetworkBehaviour
{
    public PlayerManager PlayerManager;

    public int numCharacter;

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        List<PlayerManager> pmList = PlayerManager.playersList;
        int count = pmList.Count;
        if (count == 2)
        {            
            PlayerManager.CharacterPick(numCharacter);
        }
        else
        {
            TextMeshProUGUI label = GameObject.Find("TextIfNot2Players").GetComponent<TextMeshProUGUI>();
            label.text = "Wait until the other player connects to pick a character";
        }
    }
}
