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
    void Start()
    {
        Draw1();
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
                Defense[i].GetComponent<Card>().CardAttack(Attack[i].GetComponent<Card>()._CardDataList[0].CardStatsData[1]); //방어카드에 공격카드의 공격력 인자를 보내는 것이다
            }
            playerDataInformation.PlayerBattle = false; // 배틀 페이즈를 종류한다
        }
    }

    public void end() //모든 페이즈가 종류됨을 알린다
    {
        if(playerDataInformation.PlayerTurn == true) // 자신의 턴일떄
            {
                    playerDataInformation.PlayerTurn = false; //내 턴을 넘긴다
            }
        else//내 턴이 아닐경우
            {
                    playerDataInformation.PlayerTurn = true; // 턴을 받는다
                    playerDataInformation.PlayerBattle = false; // 베틀페이즈는 아직 실향하지 않으므로 종류 시켜준다.
            }
    }
}
