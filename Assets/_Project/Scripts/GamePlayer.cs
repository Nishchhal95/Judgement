using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayer : MonoBehaviour
{
    [SerializeField] private Image playerIcon;
    [SerializeField] private TMP_Text playerNameTextField;
    [field: SerializeField] public bool IsLocal { get; private set; }
    [field: SerializeField] public Transform CardContainer { get; private set; }
    
    private Player photonPlayer;
    private List<Card> cards = new();

    public void SetPlayerIcon(Sprite icon)
    {
        playerIcon.sprite = icon;
    }

    public void SetPlayerName(string playerName)
    {
        playerNameTextField.SetText(playerName);
    }
    
    public void SetPhotonPlayer(Player player)
    {
        photonPlayer = player;
        IsLocal = player.IsLocal;
    }

    public void AddCard(Card card)
    {
        card.OnClicked += OnCardClicked;
        cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        card.OnClicked -= OnCardClicked;
        cards.Remove(card);
    }

    private void OnCardClicked(Card clickedCard)
    {
        foreach (Card card in cards)
        {
            if (card == clickedCard)
            {
                card.ToggleSelection();
                continue;
            }
            
            card.Deselect();
        }
    }
}
