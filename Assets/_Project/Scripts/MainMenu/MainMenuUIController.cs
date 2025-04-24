using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private Button playerIconButton;
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private WindowManager windowManager;
    
    private void OnEnable()
    {
        playerNameInputField.onValueChanged.AddListener(OnPlayerInputFieldValueChanged);
        
        playerIconButton.onClick.AddListener(OnPlayerIconButtonPressed);
        createGameButton.onClick.AddListener(OnCreateGamePressed);
        joinGameButton.onClick.AddListener(OnJoinGamePressed);

        LocalPlayerInfo.OnLocalPlayerIconChanged += ChangeLocalPlayerIcon;

        SetDefaults();
    }

    private void OnDisable()
    {
        playerNameInputField.onValueChanged.RemoveListener(OnPlayerInputFieldValueChanged);
        
        playerIconButton.onClick.RemoveListener(OnPlayerIconButtonPressed);
        createGameButton.onClick.RemoveListener(OnCreateGamePressed);
        joinGameButton.onClick.RemoveListener(OnJoinGamePressed);
        
        LocalPlayerInfo.OnLocalPlayerIconChanged -= ChangeLocalPlayerIcon;
    }
    
    private void SetDefaults()
    {
        LocalPlayerInfo.Name = "Player";
        playerNameInputField.SetTextWithoutNotify("Player");

        LocalPlayerInfo.IconIndex = 0;
        ChangeLocalPlayerIcon();
        
        UpdateGameButtonsState();
    }

    private void OnPlayerInputFieldValueChanged(string newValue)
    {
        if (string.IsNullOrEmpty(newValue) || string.IsNullOrWhiteSpace(newValue))
        {
            LocalPlayerInfo.Name = "";
            UpdateGameButtonsState();
            return;
        }

        LocalPlayerInfo.Name = newValue;
        UpdateGameButtonsState();
    }

    private void OnPlayerIconButtonPressed()
    {
        windowManager.ShowWindow("ICON_SELECTION_WINDOW");
    }

    private void OnCreateGamePressed()
    {
        // Open Game Creation
    }

    private void OnJoinGamePressed()
    {
        // Open Game Join
    }

    private void ChangeLocalPlayerIcon()
    {
        playerIconButton.image.sprite = gameInfo.iconSprites[LocalPlayerInfo.IconIndex];
    }

    private void UpdateGameButtonsState()
    {
        createGameButton.interactable = !string.IsNullOrEmpty(LocalPlayerInfo.Name) && LocalPlayerInfo.IconIndex != -1;
        joinGameButton.interactable = !string.IsNullOrEmpty(LocalPlayerInfo.Name) && LocalPlayerInfo.IconIndex != -1;
    }
}
