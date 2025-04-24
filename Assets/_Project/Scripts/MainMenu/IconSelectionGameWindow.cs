using UnityEngine;
using UnityEngine.UI;

public class IconSelectionGameWindow : GameWindow
{
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private Transform content;
    [SerializeField] private Button buttonPrefab;

    private void Start()
    {
        for (int i = 0; i < gameInfo.iconSprites.Length; i++)
        {
            Button button = Instantiate(buttonPrefab, content);
            button.image.sprite = gameInfo.iconSprites[i];
            int x = i;
            button.onClick.AddListener(delegate { OnIconButtonPressed(x);});
        }
    }

    private void OnIconButtonPressed(int index)
    {
        LocalPlayerInfo.IconIndex = index;
        Hide();
    }

    public override string GetWindowId()
    {
        return "ICON_SELECTION_WINDOW";
    }
}
