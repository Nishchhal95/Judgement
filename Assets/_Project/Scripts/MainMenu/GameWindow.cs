using UnityEngine;
using UnityEngine.UI;

public abstract class GameWindow : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(Hide);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public abstract string GetWindowId();
}
