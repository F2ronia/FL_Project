using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    MonsterCard,
    InstallationTypeMagicCard,
    ConstructionTypeMagicCard,
    BuffCard,
    DebuffCard,
    ResuscitationCard
}

[System.Serializable]
public class CardData
{
    public string CardName;
    public int[] CardStatsData = new int[5];
    //0 = 체력
    //1 = 공격력
    //2 = 코스트
    //3 = 카드 희귀도
    //4 = 카드 확률

    public Sprite CardImage;

    public bool Hp;

    public CardDataInformation cardDataInformation;

    public PlayerDataInformation playerDataInformation;

    public CardType cardType;

    public int SerialNum;
    //카드 구분용 넘버
}




public class Card : MonoBehaviour
{
    public List<CardData> _CardDataList;

    public Text HP;
    public Text Attack;
    public Text Coast;

    public Text effect;



    void Start()
    {
        //_CardDataList[0]._CardDataList.Add(넣을 자료); //추가 / 추가하는 순서에 따라 순서가 지정된다
        //_CardDataList[0]._CardDataList.RemoveAt(0); //삭제
        //_CardDataList = new List<CardData>(new CardData[20]); //리스트 크기 초기화
        //_CardDataList.Add(new _CardDataList());
        for (int i = 0; i < _CardDataList.Count; i++)
        {
            _CardDataList[i].CardImage = _CardDataList[i].cardDataInformation.CardImage;
            _CardDataList[i].CardStatsData = _CardDataList[i].cardDataInformation.CardStatsData;
            _CardDataList[i].CardName = _CardDataList[i].cardDataInformation.CardName;
            _CardDataList[i].playerDataInformation = _CardDataList[i].cardDataInformation.playerDataInformation;
        }

    }

    public void CardAttack(int attack) // 공격력을 받은 것이다
    {
        _CardDataList[0].CardStatsData[0] = _CardDataList[0].CardStatsData[0] - attack;
        print("데미지:" + attack);
        print("현재 체력:" + _CardDataList[0].CardStatsData[0]);
    }
}
