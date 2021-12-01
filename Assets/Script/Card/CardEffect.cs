using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : MonoBehaviour
{

    public List<GameObject> PlayerCradZoneList;

    public PlayerDataInformation playerDataInformation;

    public GameObject Cemetry = null;
    void Start()
    {
        Cemetry = GameObject.Find("CardCemetry").transform.Find("CemetryButton").gameObject;
        PlayerCradZoneList.Clear();
        GameObject PlayerCradZone = GameObject.Find("PlayerCardZone");
        for(int i = 0; i < PlayerCradZone.transform.childCount; i++)
        {
            PlayerCradZoneList.Add(PlayerCradZone.transform.GetChild(i).gameObject);
        }
    
    }

    public void CardE(GameObject PlayerCard, GameObject EnemyCard) // 효과 발동 전 알맞는 효과에 지정해주는 역할
    {
        var CardType = PlayerCard.GetComponent<Card>()._CardDataList[0].cardType;
        print(this.name);

        switch(CardType)
        {
            case CardType.BuffCard:
            if(EnemyCard.transform.parent.CompareTag("PlayerCardZone"))
            {
                CardBuff(PlayerCard.GetComponent<Card>());
            }
            else
            {
                PlayerCard.SetActive(true);
            }
            break;

            case CardType.DebuffCard:
            if(EnemyCard.transform.parent.CompareTag("EnemyCardZone"))
            {
                CardDebuff(PlayerCard.GetComponent<Card>());
            }
            else
            {
                PlayerCard.SetActive(true);
            }
            break;

            case CardType.EnemyControllerCard:
            if(EnemyCard.transform.parent.CompareTag("EnemyCardZone"))
            {
                CardEnemyController(PlayerCard);
            }
            else
            {
                PlayerCard.SetActive(true);
            }
            break;

            case CardType.ResuscitationCard:
            CemetryCard(PlayerCard);
            break;
        }
    }
    public void CardDebuff(Card PlayerCard) //디버프 카드 효과
    {
         for(int i = 0; i < 2; i++) 
        {
            var playerCard = PlayerCard._CardDataList[0].CardStatsData[i];//디버프 카드의 스탯(마법카드의 스탯으로 감소치 결정)
            GetComponent<Card>()._CardDataList[0].CardStatsData[i]-=playerCard;//마법카드가 적용된 카드에 마법카드의 감소 수치 만큼 감소
            playerDataInformation.PlayerStatsData[1] -= PlayerCard.GetComponent<Card>()._CardDataList[0].CardStatsData[2];//코스트 지불
        }
    }

    public void CardBuff(Card PlayerCard) // 버프 카드 효과
    {
        for(int i = 0; i < 2; i++) 
        {
            var playerCard = PlayerCard._CardDataList[0].CardStatsData[i];
            GetComponent<Card>()._CardDataList[0].CardStatsData[i]+=playerCard;
            print(playerCard);
            playerDataInformation.PlayerStatsData[1] -= PlayerCard.GetComponent<Card>()._CardDataList[0].CardStatsData[2];
        }
    }

    public void CardEnemyController(GameObject PlayerCard)
    {
        int ListCount = 0;
        for(int i = 0; i < PlayerCradZoneList.Count; i++)
        {
            if(PlayerCradZoneList[i].transform.childCount == 0)
            {
                GameObject Card = Instantiate(this.gameObject, PlayerCradZoneList[i].transform);
                Card.SetActive(true);
                Destroy(this.gameObject);
                playerDataInformation.PlayerStatsData[1] -= PlayerCard.GetComponent<Card>()._CardDataList[0].CardStatsData[2];
                break;
            }
            else
            {
                ListCount+=1;
                if(ListCount == 3)
                {
                    PlayerCard.SetActive(true);
                    print("시전자의 카드존은 이미 다 찼습니다");
                }
            }
        }
    }

    public void CemetryCard(GameObject PlayerCard) //플레이어의 카드존이 다 찼는지 확인하고 빈 카드존이 있다면 소생카드에 나와있는 코스트만큼 플레이어의 마나를 깍는 역할
    {
        int ListCount = 0;// 빈칸이 아닐경우 1씩 증가
        for(int i = 0; i < PlayerCradZoneList.Count; i++)// 플레이어의 커드존 수만큼 반복문을 작동
        {
            if(PlayerCradZoneList[i].transform.childCount == 0) //카드존의 자식이 없는 경우 빈칸으로 인식(카드존에 카드가 없는 경우)
            {
                Cemetry.transform.gameObject.SetActive(true);//묘지를 활성화
                playerDataInformation.PlayerStatsData[1] -= PlayerCard.GetComponent<Card>()._CardDataList[0].CardStatsData[2];//코스트 지불
                break;
            }
            else
            {
                ListCount+=1;//빈칸이 아니라 1 증가
                if(ListCount == PlayerCradZoneList.Count)//카운트가 카드존의 수와 동일하면
                {
                    PlayerCard.SetActive(true);//마법카드를 패로 되돌린다(발동 무효화)
                    print("시전자의 카드존은 이미 다 찼습니다");
                }
            }
        }
    }
    public void Resuscitation()
    {GameObject Card;
         for(int i = 0; i < PlayerCradZoneList.Count; i++)//플레이어 카드존만큼 반복문 작동
        {
            if(PlayerCradZoneList[i].transform.childCount == 0) //플레이어의 카드존에 자식이 없는 경우 빈칸으로 인식(카드존에 카드가 없는 경우)
            {
                Card = Instantiate(this.gameObject, PlayerCradZoneList[i].transform);//i에 해당하는 카드존에 카드를 생성
                Card.tag = "PlayerCard";//생성한 카드의 태그를 변경
                Card.SetActive(true);//생성한 카드를 활성화
                Destroy(this);//묘지에 있는 본인을 삭제
                Cemetry.transform.gameObject.SetActive(false);//묘지를 비활성화
                break;
            }
        }
    }


}
