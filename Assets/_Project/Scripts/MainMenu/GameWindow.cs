using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class GameWindow : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject root;
    [SerializeField] private GameObject mainWindow;

    private WindowManager windowManager;

    protected virtual void OnEnable()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(windowManager.HideCurrentWindow);
        }
    }

    protected virtual void OnDisable()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(windowManager.HideCurrentWindow);
        }
    }

    public virtual void Show()
    {
        mainWindow.transform.localScale = Vector3.zero;
        root.gameObject.SetActive(true);
        mainWindow.transform.DOScale(Vector3.one, 0.2f);
    }
    
    public virtual void Hide()
    {
        mainWindow.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            root.gameObject.SetActive(false);
        });
    }

    public abstract string GetWindowId();

    public void Inject(WindowManager windowManager)
    {
        this.windowManager = windowManager;
    }
}
