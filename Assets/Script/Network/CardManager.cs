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

    [SyncVar]
    public int max_deck_cnt = 6;

    private PlayerManager playerManager;

    public void CardDraw()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // ���� �������� Ŭ���̾�Ʈ�� ID �� �޾ƿ���
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID ���� �ش��ϴ� PlayerManager �Ҵ�
        playerManager.CmdDrawCard();
    }

    /*
    public void CardDrop(GameObject card, string field)
    {

    }
    */

    public void LoadDeck()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // ���� �������� Ŭ���̾�Ʈ�� ID �� �޾ƿ���
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID ���� �ش��ϴ� PlayerManager �Ҵ�
        playerManager.max_draw_cnt = max_deck_cnt;
        playerManager.CmdLoadDeck();
    }
}
