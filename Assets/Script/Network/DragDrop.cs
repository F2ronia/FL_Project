using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

[RequireComponent (typeof (NetworkIdentity))]
[RequireComponent (typeof (NetworkTransform))]

public class DragDrop : NetworkBehaviour
{
    #region Variables
    private GameObject mainCanvas;  // ���� ĵ����
    private GameObject playerArea;  // �÷��̾� ī�� ��� ����
    private GameObject playerHand;  // �÷��̾� ����

    private Vector2 startPos;

    private bool isOverDropZone;

    private bool isDragging = false;

    private bool isDraggable = true;

    private PlayerManager playerManager;
    #endregion

    private void Start()
    {
        mainCanvas = GameObject.Find("UI");
        playerArea = GameObject.Find("PlayerCardZone");
        playerHand = GameObject.Find("PlayerHand");

        /*
        if (!hasAuthority)
        {
            isDraggable = false;
        }
        */
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition;
            transform.SetParent(mainCanvas.transform, true);
        }
    }

    public void StartDrag()
    {
        if (!isDraggable)
            return;

        isDragging = true;
        transform.SetParent(mainCanvas.transform, false);
        playerHand = transform.parent.gameObject;
        startPos = transform.position;
    }

    public void EndDrag()
    {
        if (!isDraggable)
            return;

        isDragging = false;

        if (isOverDropZone)
        {
            transform.SetParent(playerArea.transform, false);
            isDraggable = false;
            // ��Ʈ��ũ ����
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.CmdPlayCard(gameObject);
        }
        else 
        {
            transform.position = startPos;
            transform.SetParent(playerHand.transform, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        isOverDropZone = true;
        playerArea = col.gameObject;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        isOverDropZone = false;
        playerArea = null;
    }
}