using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;


public class DragDrop : NetworkBehaviour
{
    #region Variables
    private GameObject mainCanvas;  // 메인 캔버스
    private GameObject playerArea;  // 플레이어 카드 사용 공간

    private GameObject startParent;
    private Vector2 startPos;

    private GameObject dropZone;
    private bool isOverDropZone;

    private bool isDragging = false;
    #endregion

    private void Start()
    {
        mainCanvas = GameObject.Find("UI");
        playerArea = GameObject.Find("PlayerArea");
        startParent = GameObject.Find("PlayerHand");
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
        isDragging = true;
        //startParent = transform.parent.gameObject;
        startPos = transform.position;
    }

    public void EndDrag()
    {
        isDragging = false;

        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
        }
        else 
        {
            transform.position = startPos;
            Debug.Log(startParent);
            transform.SetParent(startParent.transform, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("On");
        isOverDropZone = true;
        dropZone = col.gameObject;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("Off");
        isOverDropZone = false;
        dropZone = null;
    }
}