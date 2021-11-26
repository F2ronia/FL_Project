using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardTarget_Net : NetworkBehaviour
{
    #region Variables
    public GameObject Card_1; //ī�带 ���� ������Ʈ
    public GameObject Card_2; //�ʵ��� ī������ ���� ����
    public GameObject CardHand; //Player�� ���� ������ ���� ����
    public Canvas m_canvas;
    private GraphicRaycaster m_gr;

    public PlayerDataInformation playerDataInformation; //�÷��̾� ������ ��ũ���ͺ�

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

            if (Input.GetMouseButtonUp(0))//���콺 Ŭ���� ������
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
                {//�÷��̾� ��Ʋ ������ �϶�

                    case "CardZone1":
                        if (results[0].gameObject.GetComponent<Card>()._CardDataList[0].cardType == CardType.MonsterCard)
                        {
                            if (Card_1 == null)
                            {
                                Card_1 = results[0].gameObject;//Card�� �о�� ������Ʈ�� �ִ´�
                                CardWhetherenabled(0);
                            }
                        }
                        break;

                    case "CardZone2":
                        if (results[0].gameObject.GetComponent<Card>()._CardDataList[0].cardType == CardType.MonsterCard)
                        {
                            Card_2 = results[0].gameObject; //����ĳ��Ʈ�� ������ ������Ʈ�� CardZone������ �ִ´�
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
                            defense.Add(Card_2);//��Ʋ ��ũ��Ʈ�� ���� ��� ������Ʈ �߰�
                            CardWhetherenabled(2);

                            battle.GetComponent<Battle>().Defense = defense; //��Ʋ ��ũ��Ʈ�� ���� �޴� ī���� ����Ʈ�� ������(������ ��꿡 Ȱ��)
                            battle.GetComponent<Battle>().Attack = attack; //��Ʋ ��ũ��Ʈ�� ���� �ϴ� Ű���� ����Ʈ�� ������(������ ��꿡 Ȱ��)
                        }
                        break;
                }

            }

            else if (playerDataInformation.PlayerBattle == false)
            //�÷��̾� ��Ʋ �������� �ƴҶ�
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

        else //����ĳ��Ʈ�� ��Ƽ� ���� ������Ʈ�� ���� ��� 
        {
            if (playerDataInformation.PlayerTurn == true) //Card ������ ������� 
            {

                if (Card_1 != null)
                {
                    Card_1.SetActive(true); //�п� ī�带 Ȱ��ȭ ���ش�
                }

                Card_2 = null;//CardZone�� ����
                Card_1 = null;//Card�� ����
                this.GetComponent<Image>().enabled = false;
                print("ī������ ī�带 �÷��ּ���");
            }
        }
    }


    void CardWhetherenabled(int Whetherenabled)
    {
        switch (Whetherenabled)
        {
            case 0:
                GetComponent<Image>().enabled = true;
                GetComponent<Card>()._CardDataList.Add(Card_1.GetComponent<Card>()._CardDataList[0]);//��ũ��Ʈ�� ������ ������Ʈ�� Card�� ī�������� �ִ´�
                GetComponent<Image>().sprite = Card_1.GetComponent<Card>()._CardDataList[0].CardImage;//Card�� �̹����� ������ ������ ������Ʈ �̹����� �����Ѵ�
                break;

            case 1:
                StartCoroutine(DelayDestory());
                break;
            case 2:
                GetComponent<Image>().enabled = true;
                GetComponent<Card>()._CardDataList.Add(Card_2.GetComponent<Card>()._CardDataList[0]);//��ũ��Ʈ�� ������ ������Ʈ�� Card�� ī�������� �ִ´�
                GetComponent<Image>().sprite = Card_2.GetComponent<Card>()._CardDataList[0].CardImage;//Card�� �̹����� ������ ������ ������Ʈ �̹����� �����Ѵ�

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
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // ���� �������� Ŭ���̾�Ʈ�� ID �� �޾ƿ���
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID ���� �ش��ϴ� PlayerManager �Ҵ�
        playerManager.CmdDropCard(Card_1, Card_2.ToString().Substring(0,9));
        GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(delayTime);
        Destroy(Card_1, 0f);
        Card_1 = null;
        Card_2 = null;
    }
}