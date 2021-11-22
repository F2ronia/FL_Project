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
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // ���� �������� Ŭ���̾�Ʈ�� ID �� �޾ƿ���
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID ���� �ش��ϴ� PlayerManager �Ҵ�
        playerManager.cmdDrawCard();
    }

    /*
    public void CardDrop(GameObject card, string field)
    {

    }
    */
}
