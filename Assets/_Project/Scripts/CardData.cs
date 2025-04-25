using System;
using UnityEngine;

[Serializable]
public class CardData
{
    public CardData(Suit suit, int value, Sprite cardSprite)
    {
        Suit = suit;
        Value = value;
        CardSprite = cardSprite;
    }

    [field: SerializeField] public Suit Suit { get; set; }
    [field: SerializeField] public int Value { get; set; }
    [field: SerializeField] public Sprite CardSprite { get; set; }
}

public enum Suit
{
    Spades = 0,
    Hearts = 1,
    Diamonds = 2,
    Clubs = 3,
    No_Trump = 4,
}