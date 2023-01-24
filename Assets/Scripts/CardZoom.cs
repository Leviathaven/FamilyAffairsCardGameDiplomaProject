using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardZoom : NetworkBehaviour
{
    public GameObject Canvas;
    public GameObject ZoomCard;

    private GameObject zoomCard;

    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        if (!hasAuthority) return;
       
        zoomCard = Instantiate(ZoomCard, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 250), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform, true);
        zoomCard.transform.localScale *= 2;
        int id = gameObject.GetComponent<ThisCard>().id;
        zoomCard.GetComponent<ThisCard>().SetDataByCard(gameObject.GetComponent<ThisCard>());
    }

    public void OnHoverExit()
    {
        if (!hasAuthority) return;
        Destroy(zoomCard);
    }
}
