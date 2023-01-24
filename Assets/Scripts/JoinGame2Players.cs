using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinGame2Players : MonoBehaviour
{
    public GameObject IpAddressField;

    public void GameJoin(string scene)
    {
        IpAddressField = GameObject.Find("IPAddressField");
        IpSaverScript.ip = IpAddressField.GetComponent<TMP_InputField>().text;
        SceneManager.LoadScene(scene);
    }
}
