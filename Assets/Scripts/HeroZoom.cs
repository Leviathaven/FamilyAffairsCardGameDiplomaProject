using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HeroZoom : NetworkBehaviour
{
    public GameObject Canvas;
    public GameObject ZoomHero;

    private GameObject zoomHero;

    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        if (!hasAuthority) return;

        zoomHero = Instantiate(ZoomHero, new Vector2(Input.mousePosition.x + 250, Input.mousePosition.y + 250), Quaternion.identity);
        zoomHero.transform.SetParent(Canvas.transform, true);
        zoomHero.transform.localScale *= 2;
        int id = gameObject.GetComponent<ThisHero>().id;
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        zoomHero.GetComponent<ThisHero>().SetDataByHeroId(id);
    }

    public void OnHoverExit()
    {
        if (!hasAuthority) return;
        Destroy(zoomHero);
    }
}