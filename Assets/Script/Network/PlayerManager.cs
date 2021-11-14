using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

/*
 * =====    Mirror 사용   =====
 *  [Server] / [Client] => 서버 / 클라이언트용 함수
 *  [Command] => 클라이언트 -> 서버 함수 호출용 키워드
 *  [ClientRPC] / [TargetRPC] => 서버 -> 클라이언트 함수 호출용 키워드
 */

public class PlayerManager : NetworkBehaviour
{
    public GameObject card;
    // 카드 데이터

    public GameObject playerArea;
    // 플레이어 카드 사용 공간
    public GameObject playerCardHand;
    // 플레이어 손패 

    public GameObject enemyArea;
    // 상대 카드 사용 공간
    public GameObject enemyCardHand;
    // 상대 손패


    [SerializeField]
    private List<CardData> cardList = new List<CardData>();

    [Server]
    public override void OnStartServer()    // 서버
    {
        base.OnStartServer();

        Debug.Log("카드 로딩");
        Debug.Log(cardList);
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
    }

    [Command]
    public void cmdDrawCard() //플레이어의 턴마다 패에 카드를 추가
    {
        if (playerCardHand.transform.childCount < 8)
        {
            /*
            CardPrefab.GetComponent<Card>()._CardDataList.Clear(); //프리팹의 카드 정보를 초기화
            int CardRandomCount = Random.Range(0, CardDeck._CardDataList.Count); //덱에서 랜덤하게 한개의 카드를 선택
            CardPrefab.GetComponent<Card>()._CardDataList.Add(CardDeck._CardDataList[CardRandomCount]); //프리팹에 카드 정보 추가
            CardPrefab.GetComponent<Image>().sprite = CardDeck._CardDataList[CardRandomCount].CardImage; //프리팹에 카드 이미지 정보 추가
            Instantiate(CardPrefab, CardHand.transform); //패 오브젝트를 부모로 지정하여 프리팹을 생성
            CardDeck._CardDataList.RemoveAt(CardRandomCount);//덱에서 패로간 카드를 삭제
            */
            Debug.Log("카드 드로우 실행");
            GameObject drawCard = Instantiate(card, Vector2.zero, Quaternion.identity);
            drawCard.GetComponent<Card>()._CardDataList.Clear();
            int randomNum = Random.Range(0, 2);
            Debug.Log(randomNum);
            drawCard.GetComponent<Card>()._CardDataList.Add(cardList[randomNum]);
            drawCard.GetComponent<Image>().sprite = cardList[randomNum].CardImage;


            NetworkServer.Spawn(drawCard, connectionToClient);
            Debug.Log("네트워크에 스폰");
            RpcShowCard(drawCard, "Dealt");
            Debug.Log("스폰된 오브젝트 처리");

        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if (hasAuthority)
            // 플레이어
            {
                card.transform.SetParent(playerCardHand.transform, false);
            }
            else
            // 상대
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
