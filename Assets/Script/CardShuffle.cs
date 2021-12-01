using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShuffle : MonoBehaviour
{
    public Card CardDeck; //덱

    public GameObject CardHand;//패

    public GameObject CardPrefab;

    public void Start()
    {
        for(int i = 0; i < 5; i++) //덱에서 카드 5장을 랜덤하개 추가
        CardDraw();
    }

    public void CardDraw() //플레이어의 턴마다 패에 카드를 추가
    {
        if(CardHand.transform.childCount < 8) //플레이어 패에 카드가 8장 이하일때만
        {

            GameObject Card;
        CardPrefab.GetComponent<Card>()._CardDataList.Clear(); //프리팹의 카드 정보를 초기화
        int CardRandomCount = Random.Range(0, CardDeck._CardDataList.Count); //덱에서 랜덤하게 한개의 카드를 선택
        CardPrefab.GetComponent<Card>()._CardDataList.Add(CardDeck._CardDataList[CardRandomCount]); //프리팹에 카드 정보 추가

        CardPrefab.GetComponent<Card>()._CardDataList[0].CardStatsData = CardDeck._CardDataList[CardRandomCount].cardDataInformation.CardStatsDatas;

       

        Card = Instantiate(CardPrefab, CardHand.transform); //패 오브젝트를 부모로 지정하여 프리팹을 생성
        CardDeck._CardDataList.RemoveAt(CardRandomCount);//덱에서 패로간 카드를 삭제
        }
    }
}
