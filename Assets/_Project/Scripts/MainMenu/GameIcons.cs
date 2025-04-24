using UnityEngine;

[CreateAssetMenu(menuName = "Create GameInfo", fileName = "GameInfo", order = 0)]
public class GameInfo : ScriptableObject
{
    [SerializeField] public Sprite[] iconSprites;
}
