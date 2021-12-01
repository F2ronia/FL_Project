using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Cardtarget : MonoBehaviour

{
    public GameObject MouseDrag; //카드를 받을 오브젝트
    public GameObject EndMouseDrag;//필드의 카드존을 받을 변수
    public GameObject CardHand; //Player의 패의 정보를 받을 변수
    public Canvas m_canvas;
    public GraphicRaycaster m_gr;

    public PlayerDataInformation playerDataInformation; //플레이어 데이터 스크립터블

    public Battle battle;

    PointerEventData m_ped;

    public GameObject CardWarning; //경고(ex카드를 잘못된 곳에 둔 경우) 

    public CardType PlayerCardType;

    public CardEffect cardEffect;


    void Update()
    {
        if(Input.GetMouseButtonUp(0) && MouseDrag != null)//마우스 클릭이 끝날떄
        {
            RayCast();
        } 
        if (Input.GetMouseButton(0) && MouseDrag == null)
        {
            RayCast();
        }
    }


    void Start()
    {
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
    }

    void RayCast()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        m_ped.position = Input.mousePosition;
        m_gr.Raycast(m_ped, results);
        if(results.Count != 0)
        {
            GameObject CardGameObjects = results[0].gameObject;
            CardBattleTrue(CardGameObjects);
        }
        else if(MouseDrag != null)
        {
            MouseDrag.SetActive(true);
            CardCancel();
        }
    }      

    public void CardBattleTrue(GameObject CardGameObject)
    {
        CardEffect cardEffect = CardGameObject.GetComponent<CardEffect>();
        bool PlayerBattlePage = playerDataInformation.PlayerBattle;

        if(PlayerBattlePage == false && MouseDrag == null && playerDataInformation.PlayerStatsData[1] >= CardGameObject.GetComponent<Card>()._CardDataList[0].CardStatsData[2] || MouseDrag != null || PlayerBattlePage == true)
        {//베틀중이 아니고, 클릭한 카드의 코스트가 본인의 코스트보다 낮을 경우 / 클릭한 카드가 있을경우(클릭한 카드가 있을 경우에 코스트를 확인하는 것을 방지하지 위해) / 배틀중일 경우(코스트를 확인하는 것을 방지하지 위해)
            switch (CardGameObject.tag)
            {
            case "PlayerCardZone":

            if(PlayerBattlePage == false && MouseDrag != null)
            {
                CardSummon(CardGameObject);
            }
            else
            {
                print("선택된 카드가 없습니다.");
            }
            break;

            case "PlayerCard":
            if(PlayerBattlePage == false && MouseDrag == null && CardGameObject.transform.parent.tag == "PlayerCardHand" || PlayerBattlePage == true && MouseDrag == null && CardGameObject.transform.parent.tag == "PlayerCardZone")
            {//배틀중이 아니고 클릭한 오브젝트의 부모의 태그가 "PlayerCardHand" 또는 배틀 중이고 카드존에 있는 몬스터를 클릭한 경우(클릭한 오브젝트를 저장하기 위해)
                CardObjectSave(CardGameObject, PlayerBattlePage);
            }
            else if(PlayerBattlePage == false && MouseDrag != null && CardGameObject.transform.parent.tag == "EnemyCardZone" && MouseDrag.GetComponent<Card>()._CardDataList[0].cardType != CardType.MonsterCard)
            {//배틀중이 아니고 클릭한 오브젝트가 있고, 부모 오브젝트의 태그가 "EnemyCardZone"인 경우 또는 마법 카드인 경우(디버프 마법 발동을 위해)
                UseMagicCard(CardGameObject);
                print(CardGameObject.transform.parent.tag);
            }
            else if(PlayerBattlePage == false && MouseDrag != null && CardGameObject.transform.parent.tag == "PlayerCardZone" && MouseDrag.GetComponent<Card>()._CardDataList[0].cardType != CardType.MonsterCard)
            {//배틀중이 아니고 클릭한 부모 오브젝트의 태그가 "PlayerCardZone"이고 마법 카드인 경으(버프 마법 발동을 위해)
                UseMagicCard(CardGameObject);
            }
            else if(PlayerBattlePage == true && MouseDrag != null && CardGameObject.transform.parent.tag == "EnemyCardZone")
            {//배틀중이고 클릭한 오브젝트의 부모 오브젝트이 태그가 "EnemyCardZone"인 경우 (몬스터 공격)
                CardAttack(CardGameObject);
            }
            else
            {
                MouseDrag.SetActive(true);
                CardCancel();
            }
            break;

            case "EnemyCardZone":
            if(PlayerBattlePage == false && MouseDrag != null && CardGameObject.GetComponent<Card>()._CardDataList[0].cardType != CardType.ResuscitationCard)
            {//배틀중이 아니고 소생 마법이 아닌경우 (에너미 컨트롤의 발동을 위하여)
                UseMagicCard(CardGameObject);
            }
            else
            {
                print("선택된 카드가 없습니다.");
            }
            break;

            case "Enemy":
            if(PlayerBattlePage == true && MouseDrag != null)
            {//적 플레이어 직접 공격을 위하여
                battle.EnemyBattleDoubleCheck(MouseDrag.gameObject, CardGameObject);
                print("a");
                CardCancel();
            }
            break;
            case "Cemetry":
            if(PlayerBattlePage == false && MouseDrag == null)
            {//소생 마법의 발동
                cardEffect.Resuscitation();
            }
            break;
            }
        }

        
    }

    void CardCancel()
    {
        MouseDrag = null;
        EndMouseDrag = null;
    }

    void CardObjectSave(GameObject CardGameObject, bool PlayerBattlePage) //클릭한 카드의 정보를 저장
    {
         MouseDrag = CardGameObject;
            if(PlayerBattlePage == false)
            {
                MouseDrag.SetActive(false);
            }
    }
    void UseMagicCard(GameObject CardGameObject)
    {
        EndMouseDrag = CardGameObject;
            PlayerCardType = MouseDrag.GetComponent<Card>()._CardDataList[0].cardType;
            if(MouseDrag != null && PlayerCardType != CardType.MonsterCard) //오류 방지용
            {
                cardEffect = EndMouseDrag.GetComponent<CardEffect>(); //효과를 적용할 카드를 지정
                cardEffect.CardE(MouseDrag, EndMouseDrag); //효과 발동을 위해 마법카드와 발동할 대상의 인자를 보냄
                CardCancel();
            }
    }
    void CardSummon(GameObject CardGameObject) //소환
    {
        PlayerCardType = MouseDrag.GetComponent<Card>()._CardDataList[0].cardType; 
            if(MouseDrag != null && PlayerCardType == CardType.MonsterCard) //소환 카드가 없을 경우를 대비해서
            {
                EndMouseDrag = Instantiate(MouseDrag, CardGameObject.transform); //카드를 소환
                EndMouseDrag.SetActive(true);//소한한 카드를 활성화
                playerDataInformation.PlayerStatsData[1] -= MouseDrag.GetComponent<Card>()._CardDataList[0].CardStatsData[2]; //코스트 지불
                CardCancel();
            }
    }
    void CardAttack(GameObject CardGameObject) //공격 
    {
        EndMouseDrag = CardGameObject;
        if(EndMouseDrag != null)
        {
            battle.BattleDoubleCheck( MouseDrag.gameObject, EndMouseDrag.gameObject ); //공격하는 카드와 방어하는 카드의 정보를 보냄
            CardCancel();
        }
    }
}