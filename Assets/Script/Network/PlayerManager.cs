using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * =====    Mirror ���   =====
 *  [Server] / [Client] => ���� / Ŭ���̾�Ʈ�� �Լ�
 *  [Command] => Ŭ���̾�Ʈ -> ���� �Լ� ȣ��� Ű����
 *  [ClientRPC] / [TargetRPC] => ���� -> Ŭ���̾�Ʈ �Լ� ȣ��� Ű����
 */

public class PlayerManager : NetworkBehaviour
{
    #region Variables
    public GameObject cardHand;
    // ���� �ո� ������
    public GameObject cardPlay;
    // �ʵ� ī�� ������
    public GameObject playerArea;
    // �÷��̾� ī�� ��� ����
    private GameObject playerDropZone;
    // �÷��̾� ī�� ��ȯ ����
    public GameObject playerCardHand;
    // �÷��̾� ���� 

    public GameObject enemyArea;
    // ��� ī�� ��� ����
    private GameObject enemyDropZone;
    // ��� ī�� ��ȯ ����
    public GameObject enemyCardHand;
    // ��� ����

    public Sprite Card_BgImg;
    // ī�� �޸� �̹���

    [SerializeField]
    private List<CardData> cardList = new List<CardData>();

    private List<int> deckList = new List<int>();
    #endregion

    [Server]
    public override void OnStartServer()    // ����
    {
        base.OnStartServer();

        Debug.Log("ī�� �ε�");
        Debug.Log(cardList);
    }

    [Client]
    public override void OnStartClient()    // Ŭ���̾�Ʈ
    {
        base.OnStartClient();


        // ������Ʈ �Ҵ�
        playerArea = GameObject.Find("PlayerArea");
        playerCardHand = GameObject.Find("PlayerHand");

        enemyArea = GameObject.Find("EnemyArea");
        enemyCardHand = GameObject.Find("EnemyHand");

        deckList.Add(0);
        deckList.Add(1);
        deckList.Add(0);
        deckList.Add(1);
        deckList.Add(0);
        deckList.Add(1);
    }


    [Command]
    public void cmdDrawCard() //�÷��̾��� �ϸ��� �п� ī�带 �߰�
    {
        if (playerCardHand.transform.childCount < 8)
        {
            GameObject drawCard = Instantiate(cardHand, Vector2.zero, Quaternion.identity);
            drawCard.GetComponent<Card>()._CardDataList.Clear();

            NetworkServer.Spawn(drawCard, connectionToClient);
            RpcDrawCard(drawCard);
        }
    }

    [Command]
    public void cmdDropCard(GameObject card, string field)
    {
        Debug.Log("command");
        RpcShowCard(card, field);
    }


    [ClientRpc]
    private void RpcDrawCard(GameObject card)
    {
        int randomNum = Random.Range(0, deckList.Count);
        Debug.Log(randomNum);
        Debug.Log(deckList[randomNum]);

        card.GetComponent<Card>()._CardDataList.Add(cardList[deckList[randomNum]]);

        if (hasAuthority)
        // �÷��̾�
        {
            card.transform.GetChild(0).GetComponent<Image>().sprite = cardList[deckList[randomNum]].CardImage;
            card.transform.SetParent(playerCardHand.transform, false);
        }
        else
        // ���
        {
            int count = card.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                if (i == 1)
                {
                    card.transform.GetChild(i).GetComponent<Image>().sprite = Card_BgImg;
                    continue;
                }

                card.transform.GetChild(i).gameObject.SetActive(false);
            }
            card.transform.SetParent(enemyCardHand.transform, false);
        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject card, string field)
    {
        Debug.Log("RPC");
        for (int i = 0; i < 3; i++)
        {
            if (playerArea.transform.GetChild(i).ToString().Substring(0, 9).Equals(field)
                && enemyArea.transform.GetChild(i).ToString().Substring(0, 9).Equals(field))
            {
                playerDropZone = playerArea.transform.GetChild(i).gameObject;
                enemyDropZone = enemyArea.transform.GetChild(i).gameObject;
                break;
            }
        }
        if (hasAuthority)
        // �÷��̾�
        {
            playerDropZone.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
            playerDropZone.GetComponent<Card>()._CardDataList.Add(card.GetComponent<Card>()._CardDataList[0]);
            playerDropZone.GetComponent<Image>().sprite = card.GetComponent<Card>()._CardDataList[0].CardImage;
            //Debug.Log("���� ī�� �ø��� �ѹ� " + card.GetComponent<Card>()._CardDataList[0].SerialNum);
            //Debug.Log("����� ī�� �ø��� �ѹ� " + playerDropZone.GetComponent<Card>()._CardDataList[0].SerialNum);
        }
        else
        // ���
        {
            enemyDropZone.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
            enemyDropZone.GetComponent<Card>()._CardDataList.Add(card.GetComponent<Card>()._CardDataList[0]);
            enemyDropZone.GetComponent<Image>().sprite = card.GetComponent<Card>()._CardDataList[0].CardImage;
        }
    }
}
