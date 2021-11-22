using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardManager : NetworkBehaviour
{
    #region Sigleton
    private static CardManager instance;
    public static CardManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CardManager>();
            return instance;
        }
    }
    #endregion

    private PlayerManager playerManager;

    public void CardDraw()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // 현재 실행중인 클라이언트의 ID 값 받아오기
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID 값에 해당하는 PlayerManager 할당
        playerManager.cmdDrawCard();
    }

    /*
    public void CardDrop(GameObject card, string field)
    {

    }
    */
}
