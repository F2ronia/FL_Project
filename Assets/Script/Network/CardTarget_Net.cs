using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardTarget_Net: MonoBehaviour
{
    public GameObject MouseDrag; //ī�带 ���� ������Ʈ
    public GameObject EndMouseDrag;//�ʵ��� ī������ ���� ����
    public GameObject CardHand; //Player�� ���� ������ ���� ����
    public Canvas m_canvas;
    public GraphicRaycaster m_gr;

    public PlayerDataInformation playerDataInformation; //�÷��̾� ������ ��ũ���ͺ�

    public Battle battle;

    PointerEventData m_ped;

    public GameObject CardWarning; //���(exī�带 �߸��� ���� �� ���) 

    public CardType PlayerCardType;

    public CardEffect cardEffect;


    void Update()
    {
        if (Input.GetMouseButtonUp(0) && MouseDrag != null)//���콺 Ŭ���� ������
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
        if (results.Count != 0)
        {
            GameObject CardGameObjects = results[0].gameObject;
            CardBattleTrue(CardGameObjects);
        }
        else if (MouseDrag != null)
        {
            MouseDrag.SetActive(true);
            CardCancel();
        }
    }

    public void CardBattleTrue(GameObject CardGameObject)
    {
        CardEffect cardEffect = CardGameObject.GetComponent<CardEffect>();
        bool PlayerBattlePage = playerDataInformation.PlayerBattle;

        if (PlayerBattlePage == false && MouseDrag == null && playerDataInformation.PlayerStatsData[1] >= CardGameObject.GetComponent<Card>()._CardDataList[0].CardStatsData[2] || MouseDrag != null || PlayerBattlePage == true)
        {//��Ʋ���� �ƴϰ�, Ŭ���� ī���� �ڽ�Ʈ�� ������ �ڽ�Ʈ���� ���� ��� / Ŭ���� ī�尡 �������(Ŭ���� ī�尡 ���� ��쿡 �ڽ�Ʈ�� Ȯ���ϴ� ���� �������� ����) / ��Ʋ���� ���(�ڽ�Ʈ�� Ȯ���ϴ� ���� �������� ����)
            switch (CardGameObject.tag)
            {
                case "PlayerCardZone":

                    if (PlayerBattlePage == false && MouseDrag != null)
                    {
                        //CardSummon(CardGameObject);
                    }
                    else
                    {
                        print("���õ� ī�尡 �����ϴ�.");
                    }
                    break;
                    
                case "PlayerCard":
                    if (PlayerBattlePage == false && MouseDrag == null && CardGameObject.transform.parent.tag == "PlayerCardHand" || PlayerBattlePage == true && MouseDrag == null && CardGameObject.transform.parent.tag == "PlayerCardZone")
                    {//��Ʋ���� �ƴϰ� Ŭ���� ������Ʈ�� �θ��� �±װ� "PlayerCardHand" �Ǵ� ��Ʋ ���̰� ī������ �ִ� ���͸� Ŭ���� ���(Ŭ���� ������Ʈ�� �����ϱ� ����)
                        CardObjectSave(CardGameObject, PlayerBattlePage);
                    }
                    else if (PlayerBattlePage == false && MouseDrag != null && CardGameObject.transform.parent.tag == "EnemyCardZone" && MouseDrag.GetComponent<Card>()._CardDataList[0].cardType != CardType.MonsterCard)
                    {//��Ʋ���� �ƴϰ� Ŭ���� ������Ʈ�� �ְ�, �θ� ������Ʈ�� �±װ� "EnemyCardZone"�� ��� �Ǵ� ���� ī���� ���(����� ���� �ߵ��� ����)
                        UseMagicCard(CardGameObject);
                        print(CardGameObject.transform.parent.tag);
                    }
                    else if (PlayerBattlePage == false && MouseDrag != null && CardGameObject.transform.parent.tag == "PlayerCardZone" && MouseDrag.GetComponent<Card>()._CardDataList[0].cardType != CardType.MonsterCard)
                    {//��Ʋ���� �ƴϰ� Ŭ���� �θ� ������Ʈ�� �±װ� "PlayerCardZone"�̰� ���� ī���� ����(���� ���� �ߵ��� ����)
                        UseMagicCard(CardGameObject);
                    }
                    else if (PlayerBattlePage == true && MouseDrag != null && CardGameObject.transform.parent.tag == "EnemyCardZone")
                    {//��Ʋ���̰� Ŭ���� ������Ʈ�� �θ� ������Ʈ�� �±װ� "EnemyCardZone"�� ��� (���� ����)
                        CardAttack(CardGameObject);
                    }
                    else
                    {
                        MouseDrag.SetActive(true);
                        CardCancel();
                    }
                    break;
                    
                case "EnemyCardZone":
                    if (PlayerBattlePage == false && MouseDrag != null && CardGameObject.GetComponent<Card>()._CardDataList[0].cardType != CardType.ResuscitationCard)
                    {//��Ʋ���� �ƴϰ� �һ� ������ �ƴѰ�� (���ʹ� ��Ʈ���� �ߵ��� ���Ͽ�)
                        UseMagicCard(CardGameObject);
                    }
                    else
                    {
                        print("���õ� ī�尡 �����ϴ�.");
                    }
                    break;

                case "Enemy":
                    if (PlayerBattlePage == true && MouseDrag != null)
                    {//�� �÷��̾� ���� ������ ���Ͽ�
                        battle.EnemyBattleDoubleCheck(MouseDrag.gameObject, CardGameObject);
                        print("a");
                        CardCancel();
                    }
                    break;
                case "Cemetry":
                    if (PlayerBattlePage == false && MouseDrag == null)
                    {//�һ� ������ �ߵ�
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

    void CardObjectSave(GameObject CardGameObject, bool PlayerBattlePage) //Ŭ���� ī���� ������ ����
    {
        MouseDrag = CardGameObject;
        if (PlayerBattlePage == false)
        {
            MouseDrag.SetActive(false);
        }
    }
    void UseMagicCard(GameObject CardGameObject)
    {
        EndMouseDrag = CardGameObject;
        PlayerCardType = MouseDrag.GetComponent<Card>()._CardDataList[0].cardType;
        if (MouseDrag != null && PlayerCardType != CardType.MonsterCard) //���� ������
        {
            cardEffect = EndMouseDrag.GetComponent<CardEffect>(); //ȿ���� ������ ī�带 ����
            cardEffect.CardE(MouseDrag, EndMouseDrag); //ȿ�� �ߵ��� ���� ����ī��� �ߵ��� ����� ���ڸ� ����
            CardCancel();
        }
    }
    void CardSummon(GameObject CardGameObject) //��ȯ
    {
        PlayerCardType = MouseDrag.GetComponent<Card>()._CardDataList[0].cardType;
        if (MouseDrag != null && PlayerCardType == CardType.MonsterCard) //��ȯ ī�尡 ���� ��츦 ����ؼ�
        {
            EndMouseDrag = Instantiate(MouseDrag, CardGameObject.transform); //ī�带 ��ȯ
            EndMouseDrag.SetActive(true);//������ ī�带 Ȱ��ȭ
            playerDataInformation.PlayerStatsData[1] -= MouseDrag.GetComponent<Card>()._CardDataList[0].CardStatsData[2]; //�ڽ�Ʈ ����
            CardCancel();
        }
    }
    void CardAttack(GameObject CardGameObject) //���� 
    {
        EndMouseDrag = CardGameObject;
        if (EndMouseDrag != null)
        {
            battle.BattleDoubleCheck(MouseDrag.gameObject, EndMouseDrag.gameObject); //�����ϴ� ī��� ����ϴ� ī���� ������ ����
            CardCancel();
        }
    }
}