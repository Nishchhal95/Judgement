using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiddingWindow : MonoBehaviour
{
    [SerializeField] private Button biddingButtonPrefab;
    [SerializeField] private Transform content;

    private List<Button> bidButtons = new();
    private Action<int> onBidButtonClicked;

    public void Initialize(int maxBid, Action<int> onBidButtonClicked, int skipNumber = -1)
    {
        this.onBidButtonClicked = onBidButtonClicked;
        for (int i = 0; i <= maxBid; i++)
        {
            Button button = Instantiate(biddingButtonPrefab, content);
            button.interactable = i != skipNumber;
            button.GetComponentInChildren<TMP_Text>().SetText(i.ToString());
            var x = i;
            button.onClick.AddListener(delegate { OnBidButtonClicked(x);});
            bidButtons.Add(button);
        }
        
        gameObject.SetActive(true);
    }

    private void OnBidButtonClicked(int bidValue)
    {
        onBidButtonClicked?.Invoke(bidValue);
        foreach (Button bidButton in bidButtons)
        {
            bidButton.onClick.RemoveAllListeners();
            Destroy(bidButton.gameObject);
        }
        bidButtons.Clear();
        gameObject.SetActive(false);
    }
}
