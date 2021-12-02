using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    MonsterCard,
    BuffCard,
    DebuffCard,
    ResuscitationCard,
    EnemyControllerCard
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

    public CardType cardType;
}

public class Card : MonoBehaviour
{
    public List<CardData> _CardDataList;

    public Text[] CardText;

    public Image[] CardImage;
    public float ObjectXSize;
    public float ObjectNextSize;

    public GameObject Cemetry;

    void Start()
    {
        Cemetry = GameObject.Find("CardCemetry").transform.Find("CemetryButton").gameObject.transform.Find("Viewport").gameObject.transform.Find("Cemetry").gameObject;
    }

    void Update()
    {
        if (_CardDataList.Count == 1 && GetComponent<Card>()._CardDataList[0].CardStatsData[0] <= 0)
        {
            KillCard();
        }
        if (_CardDataList.Count == 1)
        {
            ResetCard();
        }
    }

    public void ResetCard()
    {//오브젝트 소환 후 변화하는 카드의 크기를 맞추기 위해
        _CardDataList[0].cardType = _CardDataList[0].cardDataInformation.cardType; //카드의 타입지정(몬스터 카드인가 마법카드인가를 지정)

        for (int i = 0; i < 3; i++)
        {
            if (_CardDataList[0].cardType == CardType.MonsterCard) //몬스터 카드인 경우 체력, 코스트 비용, 공격력을 넣어주는 역할
            {
                CardText[i].text = _CardDataList[0].CardStatsData[i].ToString();
            }
            else if (_CardDataList[0].cardType != CardType.MonsterCard && i < 2)// 마법 카드인 경우 체력과 공격력의 텍스트와 이미지를 비활성화
            {
                CardImage[i].GetComponent<Image>().enabled = false;
                CardText[i].GetComponent<Text>().enabled = false;
                CardImage[3].GetComponent<Image>().color = Color.blue; //마법 카드인 경우 색깔 지정
            }
        }

        CardImage[2].sprite = _CardDataList[0].cardDataInformation.CardImage; //소환시 카드의 이미지를 넣어주는 역할
        CardText[3].text = _CardDataList[0].cardDataInformation.CardEffact; //카드 효과를 넣어주는 역할



        RectTransform rectTransform;
        rectTransform = GetComponent<RectTransform>();

        ObjectXSize = rectTransform.rect.width; //카드의 width값을 저장
        ObjectNextSize = ObjectXSize / CardImage[3].GetComponent<RectTransform>().rect.width; //저장한 값을 토대로 나누어 기존에 비해 배수를 측정
        if (ObjectNextSize <= 0) //오류 방지용
        {
            ObjectNextSize = 1;
        }
        for (int i = 0; i < CardImage.Length; i++) //이미지를 커진만큼 크기를 키워주는 역할
        {
            CardImage[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CardImage[i].GetComponent<RectTransform>().rect.height * ObjectNextSize);
            CardImage[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CardImage[i].GetComponent<RectTransform>().rect.width * ObjectNextSize);

            if (i < CardImage.Length - 1)//텍스트를 커진만큼 크기를 키워주는 역할
            {
                CardText[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CardText[i].GetComponent<RectTransform>().rect.height * ObjectNextSize);
                CardText[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CardText[i].GetComponent<RectTransform>().rect.width * ObjectNextSize);
            }
        }
    }

    void KillCard()//체력이 0 이하인 카드를 카드존에서 묘지로 보내는 역할
    {
        if (GetComponent<Card>()._CardDataList[0].CardStatsData[0] <= 0)//체력 확인
        {
            GameObject card;
            this.GetComponent<Card>()._CardDataList[0].CardStatsData = this.GetComponent<Card>()._CardDataList[0].cardDataInformation.CardStatsDatas; //묘지로 보내기전 죽기 전으로 초기화
            card = Instantiate(this.gameObject, Cemetry.transform);//묘지로 보냄
            card.tag = "Cemetry";//태그를 변경하여 묘지에서 소생할때 이용
            Destroy(this.gameObject); // 삭제
        }
    }

    public void CardDamage(int damage) //데미지 계산을 위한 함수
    {
        _CardDataList[0].CardStatsData[0] = _CardDataList[0].CardStatsData[0] - damage; //받은 데미지 인자만큼 체력에서 빼주는 역할
        print("데미지:" + damage);
        print("현재 체력:" + _CardDataList[0].CardStatsData[0]);
    }
}
