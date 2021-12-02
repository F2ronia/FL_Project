using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardManager : MonoBehaviour//NetworkBehaviour
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
    public List<CardDataInformation> cardDataList;

    public GameObject playerHand;

    public GameObject cardPrefab;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
            DrawCard();
    }

    public void DrawCard()
    {
        if (playerHand.transform.childCount < 8)
        {
            int random = Random.Range(0, cardDataList.Count);

            GameObject spawnCard = Instantiate(cardPrefab, Vector2.zero, Quaternion.identity);
            spawnCard.GetComponent<Card>()._CardDataList.Clear();
            spawnCard.transform.GetChild(0).GetComponent<Image>().sprite = cardDataList[random].CardImage;
            spawnCard.transform.SetParent(playerHand.transform, false);
        }
    }


    
    //[SyncVar]
    public int max_deck_cnt = 6;

    private PlayerManager playerManager;

    
    public void CardDraw()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // ���� �������� Ŭ���̾�Ʈ�� ID �� �޾ƿ���
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID ���� �ش��ϴ� PlayerManager �Ҵ�
        playerManager.CmdDrawCard();
    }

    
    public void CardDrop(GameObject card, string field)
    {
        return;
    }
    

    public void LoadDeck()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // ���� �������� Ŭ���̾�Ʈ�� ID �� �޾ƿ���
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID ���� �ش��ϴ� PlayerManager �Ҵ�
        playerManager.max_draw_cnt = max_deck_cnt;
        playerManager.CmdLoadDeck();
    }
}
