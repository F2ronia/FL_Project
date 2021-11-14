using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

/*
 * =====    Mirror ���   =====
 *  [Server] / [Client] => ���� / Ŭ���̾�Ʈ�� �Լ�
 *  [Command] => Ŭ���̾�Ʈ -> ���� �Լ� ȣ��� Ű����
 *  [ClientRPC] / [TargetRPC] => ���� -> Ŭ���̾�Ʈ �Լ� ȣ��� Ű����
 */

public class PlayerManager : NetworkBehaviour
{
    public GameObject card;
    // ī�� ������

    public GameObject playerArea;
    // �÷��̾� ī�� ��� ����
    public GameObject playerCardHand;
    // �÷��̾� ���� 

    public GameObject enemyArea;
    // ��� ī�� ��� ����
    public GameObject enemyCardHand;
    // ��� ����


    [SerializeField]
    private List<CardData> cardList = new List<CardData>();

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
    }

    [Command]
    public void cmdDrawCard() //�÷��̾��� �ϸ��� �п� ī�带 �߰�
    {
        if (playerCardHand.transform.childCount < 8)
        {
            /*
            CardPrefab.GetComponent<Card>()._CardDataList.Clear(); //�������� ī�� ������ �ʱ�ȭ
            int CardRandomCount = Random.Range(0, CardDeck._CardDataList.Count); //������ �����ϰ� �Ѱ��� ī�带 ����
            CardPrefab.GetComponent<Card>()._CardDataList.Add(CardDeck._CardDataList[CardRandomCount]); //�����տ� ī�� ���� �߰�
            CardPrefab.GetComponent<Image>().sprite = CardDeck._CardDataList[CardRandomCount].CardImage; //�����տ� ī�� �̹��� ���� �߰�
            Instantiate(CardPrefab, CardHand.transform); //�� ������Ʈ�� �θ�� �����Ͽ� �������� ����
            CardDeck._CardDataList.RemoveAt(CardRandomCount);//������ �зΰ� ī�带 ����
            */
            Debug.Log("ī�� ��ο� ����");
            GameObject drawCard = Instantiate(card, Vector2.zero, Quaternion.identity);
            drawCard.GetComponent<Card>()._CardDataList.Clear();
            int randomNum = Random.Range(0, 2);
            Debug.Log(randomNum);
            drawCard.GetComponent<Card>()._CardDataList.Add(cardList[randomNum]);
            drawCard.GetComponent<Image>().sprite = cardList[randomNum].CardImage;


            NetworkServer.Spawn(drawCard, connectionToClient);
            Debug.Log("��Ʈ��ũ�� ����");
            RpcShowCard(drawCard, "Dealt");
            Debug.Log("������ ������Ʈ ó��");

        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if (hasAuthority)
            // �÷��̾�
            {
                card.transform.SetParent(playerCardHand.transform, false);
            }
            else
            // ���
            {
                card.transform.SetParent(enemyCardHand.transform, false);
            }
        }
        else if (type == "Played")
        {
            //
        }
    }
}
