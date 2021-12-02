using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Card")]
public class CardDataInformation : ScriptableObject
{
    public string CardName;
    public int[] CardStatsDatas = new int[6];
    //0 = 체력
    //1 = 공격력
    //2 = 코스트
    //3 = 플레이어 정보
    //4 = 카드 희귀도
    //5 = 카드 종류(1 = 주술) 

    public Sprite CardImage;

    public CardType cardType;

    public string CardEffact;
}
