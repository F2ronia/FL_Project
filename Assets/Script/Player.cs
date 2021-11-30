using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public PlayerDataInformation playerDataInformation;

    public void Damage(int damage)
    {
        playerDataInformation.PlayerStatsData[0] -= damage;
        print("데미지:" + damage);
    }
}
