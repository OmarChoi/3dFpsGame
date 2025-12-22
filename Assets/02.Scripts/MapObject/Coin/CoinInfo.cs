using UnityEngine;

[CreateAssetMenu(fileName = "MapObjects", menuName = "Game/MapObjects/Coin")]
public class CoinInfo : ScriptableObject
{
    public ECoinType Type;
    public GameObject Prefab;

    public int Value;
    public float Weight;
}
