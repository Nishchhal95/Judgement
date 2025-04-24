using UnityEngine;
using UnityEngine.UI;

public abstract class GameWindow : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    protected virtual void OnEnable()
    {
        closeButton.onClick.AddListener(Hide);
    }

    protected virtual void OnDisable()
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
