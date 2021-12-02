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
    public GameObject playerCardHand;
    // �÷��̾� ���� 

    public GameObject enemyArea;
    // ��� ī�� ��� ����
    public GameObject enemyCardHand;
    // ��� ����

    public Sprite Card_BgImg;
    // ī�� �޸� �̹���

    private int draw_cnt;

    public int max_draw_cnt;

    [SerializeField]
    private List<CardData> cardList = new List<CardData>();

    private List<int> deckList = new List<int>();

    private List<int> deckList_1 = new List<int>();
    private List<int> deckList_2 = new List<int>();

    #endregion


    private void Start()
    {
        draw_cnt = 0;
        max_draw_cnt = 0;
    }
    

    [Server]
    public override void OnStartServer()    // ����
    {
        base.OnStartServer();

        Debug.Log("ī�� �ε�");

        deckList_1.Add(0);
        deckList_1.Add(1);
        deckList_1.Add(0);
        deckList_1.Add(1);
        deckList_1.Add(0);
        deckList_1.Add(1);

        deckList_2.Add(0);
        deckList_2.Add(0);
        deckList_2.Add(0);
        deckList_2.Add(0);
        deckList_2.Add(1);
        deckList_2.Add(1);
        deckList_2.Add(1);
        deckList_2.Add(1);
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

        //CardManager.Instance.LoadDeck();
    }

    [Command]
    public void CmdLoadDeck()
    {
        Debug.Log("�÷��̾� ��ȣ : ");
        Debug.Log("ī�� �� �ż� : " + deckList.Count);
        Debug.Log("�÷��̾�Ŵ��� ���� : " + max_draw_cnt);
        Debug.Log("ī��Ŵ��� ���� : " + CardManager.Instance.max_deck_cnt);
    }

    [Command]
    public void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }

    [Command]
    public void CmdDrawCard() //�÷��̾��� �ϸ��� �п� ī�带 �߰�
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
    public void CmdDropCard(GameObject card, string field)
    {
        Debug.Log("command");
        RpcShowCard(card, field);
    }


    [ClientRpc]
    private void RpcDrawCard(GameObject card)
    {
        if (draw_cnt > max_draw_cnt)
        {
            Debug.Log("XXXXX");
        }
        else
        {
            card.GetComponent<Card>()._CardDataList.Add(cardList[deckList[draw_cnt]]);
            Debug.Log("���� ���� ī�� �� " + draw_cnt);
            Debug.Log("�ִ� �̱� �� " + max_draw_cnt);
            if (hasAuthority)
            // �÷��̾�
            {
                card.transform.GetChild(0).GetComponent<Image>().sprite = cardList[deckList[draw_cnt]].CardImage;
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
            draw_cnt++;
        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject card, string type)
    {
        Debug.Log("RPC");

        if (type == "Played")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(playerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(enemyArea.transform, false);
            }
        }
        /*
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
        */
    }
}
