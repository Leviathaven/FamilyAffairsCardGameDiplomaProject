using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartGame : NetworkBehaviour
{
    public NetworkManager NetworkManager;

    private void Awake()
    {
        NetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        NetworkManager.networkAddress = IpSaverScript.ip;
        //NetworkManager.StartClient();
    }
}
