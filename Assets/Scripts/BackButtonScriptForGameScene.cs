using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class BackButtonScriptForGameScene : NetworkBehaviour
{

    public GameObject NetworkManager;

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkManager = GameObject.Find("NetworkManager");
    }

    public void BackToMenu(string scene)
    {              
        NetworkManager NetworkManagerScript = NetworkManager.GetComponent<NetworkManager>().GetComponent<NetworkManager>();
        NetworkManagerScript.StopClient();
        Destroy(NetworkManager);
        SceneManager.LoadScene(scene);
    }

}
