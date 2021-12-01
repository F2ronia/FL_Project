using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelect : MonoBehaviour
{
    public List<CardData> CardList = new List<CardData>(); // 확률에 맞는 카드를 모두 저장
    public List<CardData> RandomCrad = new List<CardData>(); //저장된 카드를 선별하기 위한 임시 저장소
    public GameObject PlayerCradDB;

    void Start()
    {
        //_CardDataList[0]._CardDataList.Add(넣을 자료); //추가 / 추가하는 순서에 따라 순서가 지정된다
        //_CardDataList[0]._CardDataList.RemoveAt(0); //삭제
        //_CardDataList = new List<CardData>(new CardData[20]); //리스트 크기 초기화
        //_CardDataList.Add(new _CardDataList());
        for(int i = 0;i < CardList.Count; i++)
        {
            CardList[i].CardImage = CardList[i].cardDataInformation.CardImage;
            CardList[i].CardStatsData = CardList[i].cardDataInformation.CardStatsDatas;
            CardList[i].CardName = CardList[i].cardDataInformation.CardName;
        }
 
    }

    public void CardRandomSelect()
    {
        if(CardList.Count != 0)
        {
        int CardStatsMaxCount = CardList[0].CardStatsData[4]; //최대치의 값을 리스트의 0번 항목을 선택하여 해줘서 오류를 방지한다
        int CardStatsMinCount = CardList[0].CardStatsData[4]; //최소치의 값을 리스트에 0번 항목을 선택하여 해줘서 오류를 방지한다
       
            for(int i = 0;i < CardList.Count; i++)
            {
            //최대치 확인
                if(CardList[i].CardStatsData[4] >= CardStatsMaxCount)
                {
                    CardStatsMaxCount = CardList[i].CardStatsData[4];
                }

            //최소치 설정
                if(CardList[i].CardStatsData[4] <= CardStatsMinCount)
                {
                    CardStatsMinCount = CardList[i].CardStatsData[4]; 
                }
                if(i == CardList.Count - 1)
                {
                    CardSelect(CardStatsMaxCount, CardStatsMinCount);
                }
            }
        }
    }
    
    void CardSelect(int CardStatsMaxCount, int CardStatsMinCount)
    {
        List<int> CardListNumber = new List<int>(); //카드를 확률에 따라 선정 후 선정된 카드 중 한장을 선택한 뒤 삭제할 때 사용할 카드의 위치 정보
        int CardRandomPercentage = Random.Range(CardStatsMinCount, CardStatsMaxCount);//남은 카드의 최대 확률을 가져와 0~최대 확률 중 숫자를 랜덤하게 선정


        for(int i = 0;i < CardList.Count; i++)//뽑기 안에 들어있는 수 만큼 실행
        {

            if(CardRandomPercentage >= CardList[i].CardStatsData[4]) //랜덤으로 선언된 CardRandomPercentage보다 카드의 확률이 클 경우 
            {
               RandomCrad.Add(CardList[i]); //카드를 제 선정하기 위해 카드 정보를 임시 저장
               CardListNumber.Add(i);//나중에 삭제를 위해 위치 정보 저장
            }

            if(i == CardList.Count - 1 && CardListNumber.Count != 0)//확률에 따라 선별이 끝나고 
            {
                int CountNum = Random.Range(0,RandomCrad.Count); //선별된 카드를 다시 랜덤하게 1장 선택
                PlayerCradDB.GetComponent<Card>()._CardDataList.Add(RandomCrad[CountNum]);//선택된 카드를 플레이어의 카드 정보에 추가
                RandomCrad.Clear(); //카드의 정보를 저장하던 임시 리스트를 초기화
                CardList.RemoveAt(CardListNumber[CountNum]);//재 선정된 카드를 뽑기에서 삭제
            }
        }
    }
}
