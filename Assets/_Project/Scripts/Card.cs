using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public Action<Card> OnClicked;
    
    [SerializeField] private Image cardImage;
    [SerializeField] private CardData cardData;

    private bool selected = false;
    private Sprite backCardSprite;

    public void Initialize(CardData cardData, Sprite backCardSprite)
    {
        this.cardData = cardData;
        this.backCardSprite = backCardSprite;
        FaceUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke(this);
    }

    public void ToggleSelection()
    {
        if (selected)
        {
            Deselect();
        }
        else
        {
            Select();
        }
    }

    public void Select()
    {
        transform.DOLocalMoveY(200, 0.2f);
        transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
        selected = true;
    }

    public void Deselect()
    {
        transform.DOLocalMoveY(0, 0.2f);
        transform.DOScale(Vector3.one, 0.2f);
        selected = false;
    }

    public void FaceUp()
    {
        cardImage.sprite = cardData.CardSprite;
    }
    
    public void FaceDown()
    {
        cardImage.sprite = backCardSprite;
    }
}
