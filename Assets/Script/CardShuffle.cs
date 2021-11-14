using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardShuffle : NetworkBehaviour
{
    public PlayerManager playerManager;

    public void CardDraw()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;    // 현재 실행중인 클라이언트의 ID 값 받아오기
        playerManager = networkIdentity.GetComponent<PlayerManager>();          // ID 값에 해당하는 PlayerManager 할당
        playerManager.cmdDrawCard();
    }
}
