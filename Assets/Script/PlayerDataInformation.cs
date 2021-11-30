using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Player")]

public class PlayerDataInformation : ScriptableObject
{ 
    public int[] PlayerStatsData = new int[1];
        //0 체력
        //1 코스트
        //2 쉴드

    public bool PlayerTurn = false;

    public bool PlayerBattle = false;
    public enum PlayerCamp
    {
        악마,
        인간
    }

    public PlayerCamp playerCamp;

    public Sprite PlayerImage;
}
