using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> cardImages;
    [SerializeField] private List<CardData> deckCards;
    [SerializeField] private Sprite backCardSprite;
    
    public void LoadCards()
    {
        deckCards = new List<CardData>();
        foreach (Sprite cardImage in cardImages)
        {
            // Naming is like "card_clubs_02"
            string[] cardImageNameParts = cardImage.name.Split('_');
            if (cardImageNameParts.Length < 3)
            {
                Debug.LogError("Invalid Naming convention for card " + cardImage.name);
                continue;
            }

            Suit suit; 
            string suitName = cardImageNameParts[1];
            string cardValueString = cardImageNameParts[2];
            switch (suitName.ToUpperInvariant())
            {
                case "BACK":
                    backCardSprite = cardImage;
                    continue;
                case "SPADES":
                    suit = Suit.Spades;
                    break;
                case "HEARTS":
                    suit = Suit.Hearts;
                    break;
                case "DIAMONDS":
                    suit = Suit.Diamonds;
                    break;
                case "CLUBS":
                    suit = Suit.Clubs;
                    break;
                default:
                    Debug.LogError("Invalid card Suit " + suitName.ToUpperInvariant());
                    continue;
            }

            if (!int.TryParse(cardValueString, out int value))
            {
                switch (cardValueString.ToUpperInvariant())
                {
                    case "J":
                        value = 11;
                        break;
                    case "Q":
                        value = 12;
                        break;
                    case "K":
                        value = 13;
                        break;
                    case "A":
                        value = 14;
                        break;
                    default:
                        Debug.LogError("Invalid card Value " + cardValueString);
                        continue;
                }
            }
            
            deckCards.Add(new CardData(suit, value, cardImage));
        }
    }

    public void ShuffleCards()
    {
        for (int i = deckCards.Count - 1; i >= 0; --i)
        {
            int j = Random.Range(0, i + 1);
            (deckCards[i], deckCards[j]) = (deckCards[j], deckCards[i]);
        }
    }

    public CardData GetCardFromDeck()
    {
        CardData cardData = deckCards[0];
        deckCards.RemoveAt(0);
        return cardData;
    }

    public Sprite GetBackCardSprite()
    {
        return backCardSprite;
    }
}