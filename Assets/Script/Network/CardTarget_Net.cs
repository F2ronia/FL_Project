using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardTarget_Net : NetworkBehaviour
{
    #region Variables
    public GameObject Card_1; //카드를 받을 오브젝트
    public GameObject Card_2; //필드의 카드존을 받을 변수
    public GameObject CardHand; //Player의 패의 정보를 받을 변수
    public Canvas m_canvas;
    private GraphicRaycaster m_gr;

    public PlayerDataInformation playerDataInformation; //플레이어 데이터 스크립터블

    public GameObject battle;

    public List<GameObject> defense;
    public List<GameObject> attack;
    PointerEventData m_ped;

    private PlayerManager playerManager;

    private float delayTime = 0.5f;

    #endregion

    void Update()
    {
        if (playerDataInformation.PlayerTurn == true)
        {

            if (Input.GetMouseButtonUp(0))//마우스 클릭이 끝날떄
            {
                RayCast();
            }


            if (Input.GetMouseButton(0) && Card_1 == null)
            {
                RayCast();

            }
        }
        this.transform.position = Input.mousePosition;
    }

    void Start()
    {
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
    }

    void RayCast()
    {
        m_ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_gr.Raycast(m_ped, results);

        if (results.Count != 0 && playerDataInformation.PlayerTurn == true)
        {
            if (playerDataInformation.PlayerBattle == true)
            {
                switch (results[0].gameObject.tag)
                {//플레이어 베틀 페이즈 일때

                    case "CardZone1":
                        if (results[0].gameObject.GetComponent<Card>()._CardDataList[0].cardType == CardType.MonsterCard)
                        {
                            if (Card_1 == null)
                            {
                                Card_1 = results[0].gameObject;//Card에 읽어온 오브젝트를 넣는다
                                CardWhetherenabled(0);
                            }
                        }
                        break;

                    case "CardZone2":
                        if (results[0].gameObject.GetComponent<Card>()._CardDataList[0].cardType == CardType.MonsterCard)
                        {
                            Card_2 = results[0].gameObject; //레이캐스트로 가져온 오브젝트를 CardZone변수에 넣는다
                            print("2");

                            for (int i = 0; i < attack.Count; i++)
                            {
                                if (attack[i] == Card_1 && attack.Count < 0)
                                {
                                    defense.RemoveAt(i);
                                    attack.RemoveAt(i);
                                }
                            }
                            attack.Add(Card_1);
                            defense.Add(Card_2);//배틀 스크립트에 공격 방어 오브젝트 추가
                            CardWhetherenabled(2);

                            battle.GetComponent<Battle>().Defense = defense; //배틀 스크립트에 공격 받는 카드의 리스트를 보낸다(데미지 계산에 활용)
                            battle.GetComponent<Battle>().Attack = attack; //배틀 스크립트에 공격 하는 키드의 리스트를 보낸다(데미지 계산에 활용)
                        }
                        break;
                }

            }

            else if (playerDataInformation.PlayerBattle == false)
            //플레이어 배틀 페이지가 아닐때
            {
                switch (results[0].gameObject.tag)
                {
                    case "Card1":
                        if (results[0].gameObject.GetComponent<Card>()._CardDataList[0].cardType == CardType.MonsterCard)
                        {
                            Card_1 = results[0].gameObject;
                            CardWhetherenabled(0);
                            Card_1.SetActive(false);
                        }
                        break;

                    case "CardZone1":
                        if (Card_1 != null)
                        {
                            Card_2 = results[0].gameObject;
                            CardWhetherenabled(1);
                        }
                        break;
                }
            }

        }

        else //래이캐스트를 쏘아서 받은 오브젝트가 없을 경우 
        {
            if (playerDataInformation.PlayerTurn == true) //Card 변수가 있을경우 
            {

                if (Card_1 != null)
                {
                    Card_1.SetActive(true); //패에 카드를 활성화 해준다
                }

                Card_2 = null;//CardZone을 비운다
                Card_1 = null;//Card를 비운다
                this.GetComponent<Image>().enabled = false;
                print("카드존에 카드를 올려주세요");
            }
        }
    }


    void CardWhetherenabled(int Whetherenabled)
    {
        switch (Whetherenabled)
        {
            case 0:
                GetComponent<Image>().enabled = true;
                GetComponent<Card>()._CardDataList.Add(Card_1.GetComponent<Card>()._CardDataList[0]);//스크립트가 장착된 오브젝트에 Card의 카드정보를 넣는다
                GetComponent<Image>().sprite = Card_1.GetComponent<Card>()._CardDataList[0].CardImage;//Card의 이미지를 가져와 장착된 오브젝트 이미지로 변경한다
                break;

            case 1:
                StartCoroutine(DelayDestory());
                break;
            case 2:
                GetComponent<Image>().enabled = true;
                GetComponent<Card>()._CardDataList.Add(Card_2.GetComponent<Card>()._CardDataList[0]);//스크립트가 장착된 오브젝트에 Card의 카드정보를 넣는다
                GetComponent<Image>().sprite = Card_2.GetComponent<Card>()._CardDataList[0].CardImage;//Card의 이미지를 가져와 장착된 오브젝트 이미지로 변경한다

                Card_1 = null;
                Card_2 = null;
                GetComponent<Image>().enabled = false;
                break;

        }
    }

    private IEnumerator DelayDestory()
    {
        Debug.Log("coroutine");
        Debug.Log(Card_1);
        Debug.Log(Card_2);
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // 현재 실행중인 클라이언트의 ID 값 받아오기
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID 값에 해당하는 PlayerManager 할당
        playerManager.CmdDropCard(Card_1, Card_2.ToString().Substring(0,9));
        GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(delayTime);
        Destroy(Card_1, 0f);
        Card_1 = null;
        Card_2 = null;
    }
}