using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * =====    Mirror 사용   =====
 *  [Server] / [Client] => 서버 / 클라이언트용 함수
 *  [Command] => 클라이언트 -> 서버 함수 호출용 키워드
 *  [ClientRPC] / [TargetRPC] => 서버 -> 클라이언트 함수 호출용 키워드
 */

public class PlayerManager : NetworkBehaviour
{
    #region Variables
    public GameObject cardHand;
    // 손패 앞면 데이터
    public GameObject cardPlay;
    // 필드 카드 데이터
    public GameObject playerArea;
    // 플레이어 카드 사용 공간
    public GameObject playerCardHand;
    // 플레이어 손패 

    public GameObject enemyArea;
    // 상대 카드 사용 공간
    public GameObject enemyCardHand;
    // 상대 손패

    public Sprite Card_BgImg;
    // 카드 뒷면 이미지

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
    public override void OnStartServer()    // 서버
    {
        base.OnStartServer();

        Debug.Log("카드 로딩");

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
    public override void OnStartClient()    // 클라이언트
    {
        base.OnStartClient();

        // 오브젝트 할당
        playerArea = GameObject.Find("PlayerArea");
        playerCardHand = GameObject.Find("PlayerHand");

        enemyArea = GameObject.Find("EnemyArea");
        enemyCardHand = GameObject.Find("EnemyHand");

        //CardManager.Instance.LoadDeck();
    }

    [Command]
    public void CmdLoadDeck()
    {
        Debug.Log("플레이어 번호 : ");
        Debug.Log("카드 총 매수 : " + deckList.Count);
        Debug.Log("플레이어매니저 변수 : " + max_draw_cnt);
        Debug.Log("카드매니저 변수 : " + CardManager.Instance.max_deck_cnt);
    }

    [Command]
    public void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
    }

    [Command]
    public void CmdDrawCard() //플레이어의 턴마다 패에 카드를 추가
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
            Debug.Log("현재 뽑은 카드 수 " + draw_cnt);
            Debug.Log("최대 뽑기 수 " + max_draw_cnt);
            if (hasAuthority)
            // 플레이어
            {
                card.transform.GetChild(0).GetComponent<Image>().sprite = cardList[deckList[draw_cnt]].CardImage;
                card.transform.SetParent(playerCardHand.transform, false);
            }
            else
            // 상대
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
        // 플레이어
        {
            playerDropZone.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
            playerDropZone.GetComponent<Card>()._CardDataList.Add(card.GetComponent<Card>()._CardDataList[0]);
            playerDropZone.GetComponent<Image>().sprite = card.GetComponent<Card>()._CardDataList[0].CardImage;
            //Debug.Log("선택 카드 시리얼 넘버 " + card.GetComponent<Card>()._CardDataList[0].SerialNum);
            //Debug.Log("드랍존 카드 시리얼 넘버 " + playerDropZone.GetComponent<Card>()._CardDataList[0].SerialNum);
        }
        else
        // 상대
        {
            enemyDropZone.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
            enemyDropZone.GetComponent<Card>()._CardDataList.Add(card.GetComponent<Card>()._CardDataList[0]);
            enemyDropZone.GetComponent<Image>().sprite = card.GetComponent<Card>()._CardDataList[0].CardImage;
        }
        */
    }
}
