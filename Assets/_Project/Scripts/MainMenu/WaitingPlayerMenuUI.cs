using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPlayerMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameTextField;
    [SerializeField] private Image playerIcon;

    public void Initialize(string playerName, Sprite playerSprite)
    {
        playerNameTextField.SetText(playerName);
        playerIcon.sprite = playerSprite;
    }
}
