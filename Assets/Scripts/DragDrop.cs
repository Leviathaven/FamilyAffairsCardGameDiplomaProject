using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    public GameObject Canvas;
    public PlayerManager PlayerManager;

    private bool isDragging = false;
    private bool isDraggable = false;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private bool isOverDropZone;
    

    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");

        if (hasAuthority)
        {
            isDraggable = true;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "DropZonePlayer")
        {
            isOverDropZone = true;
            dropZone = collision.gameObject;
            return;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "DropZonePlayer")
        {
            isOverDropZone = false;
            dropZone = null;
            return;
        }
    }

    public void StartDrag()
    {
        if (!isDraggable)
            return;
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        if (!isDraggable)
            return;
        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
            PlayerManager.PlayCard(gameObject, PlayerManager.thisIndex, gameObject.GetComponent<ThisCard>().cost);
            isDraggable = false;
            return;
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
            return;
        }
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }
}
