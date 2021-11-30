using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Battle : MonoBehaviour
{

    public PlayerDataInformation playerDataInformation; //플레이어의 스크립터블 오브젝트가 부착되는곳

    public GameObject PlayerDraw; //드로우를 위한 버튼 UI

    public List<GameObject> Attack;//공격을 하는 카드정보
    public List<GameObject> Defense; //공격을 받는 카드정보

    public List<GameObject> EnemyCradZoneList;
    void Start()
    {
        Draw1();
        EnemyCradZoneList.Clear();
        GameObject PlayerCradZone = GameObject.Find("EnemyCardZone");
        for(int i = 0; i < PlayerCradZone.transform.childCount; i++)
        {
            EnemyCradZoneList.Add(PlayerCradZone.transform.GetChild(i).gameObject);
        }
    }

    void Draw1()
    {
        PlayerDraw.SetActive(true); // 덱을 활성화 시킨다(카드 드로우를 위해)
    }

    public void Draw2()
    {
        PlayerDraw.SetActive(false);
    }

    public void Battle1()//공격할 카드를 지정
    {
        for(int i = 0; i < 3; i++)
            {
                if(playerDataInformation.PlayerTurn == true) //자신의 턴일때
                {
                    playerDataInformation.PlayerBattle = true; //배틀 페이즈에 돌입했음을 선언
                }
            }
    }

    public void Battle2()//지정된 카드를 공격할떄
    {
        if(playerDataInformation.PlayerBattle == true)
        {
             for(int i = 0; i < Attack.Count; i++) //자신이 공격한 만큼 공격을 실행
            {
                if(Defense[i].GetComponent<Card>() != null)
                {
                    Defense[i].GetComponent<Card>().CardDamage(Attack[i].GetComponent<Card>()._CardDataList[0].CardStatsData[1]); //방어카드에 공격카드의 공격력 인자를 보내는 것이다
                }
                else
                {
                    Defense[i].GetComponent<Player>().Damage(Attack[i].GetComponent<Card>()._CardDataList[0].CardStatsData[1]); //방어카드에 공격카드의 공격력 인자를 보내는 것이다
                }
                //애니메이션 타격 실행
            }
            playerDataInformation.PlayerBattle = false; // 배틀 페이즈를 종류한다
        }
    }

    public void end() //모든 페이즈가 종류됨을 알린다
    {
        if(playerDataInformation.PlayerTurn == true) // 자신의 턴일떄
            {
                    playerDataInformation.PlayerTurn = false;
                    Attack.Clear();
                    Defense.Clear(); //내 턴을 넘긴다
            }
        else//내 턴이 아닐경우
            {
                    playerDataInformation.PlayerTurn = true; // 턴을 받는다
                    playerDataInformation.PlayerBattle = false; // 베틀페이즈는 아직 실향하지 않으므로 종류 시켜준다.
            }          
    }

    public void BattleDoubleCheck(GameObject AttackObject , GameObject DefenseObject)
    {
        if(AttackObject != null)
        {
             for(int i = 0; i < Attack.Count;i++)
                            {
                                if(Attack[i] == AttackObject && Attack.Count > 0) //i컨애 있는 오브젝트와 공격하는 몬스터 카드가 동일할 경우 리스트에서 삭제
                                {
                                    
                                    Defense.RemoveAt(i);
                                    Attack.RemoveAt(i);
                                }
                            }
                        Attack.Add(AttackObject);
                        Defense.Add(DefenseObject);//배틀 스크립트에 공격 방어 오브젝트 추가
        }   
    }

    public void EnemyBattleDoubleCheck(GameObject AttackObject , GameObject DefenseObject) //상대 플레이어를 직접 공격
    {
        int ListCount = 0;
        for(int i = 0; i < EnemyCradZoneList.Count; i++) //상대 플레이어의 카드존에 카드가 있는지 확인
        {
            if(i < Attack.Count) 
            {
                if(Attack[i] == AttackObject) //i컨애 있는 오브젝트와 직접 공격하는 몬스터 카드가 동일할 경우 리스트에서 삭제
                {
                                    
                Defense.RemoveAt(i);
                Attack.RemoveAt(i);
                }
            }
            
            
            if(EnemyCradZoneList[i].transform.childCount == 0)
            {
                ListCount += 1;
                if(ListCount == 3)
                {
                    Attack.Add(AttackObject);
                    Defense.Add(DefenseObject);//배틀 스크립트에 공격 방어 오브젝트 추가
                }
            }
            else
            {
                print("공격대상 지정실패");
            }

        }
    }
}
