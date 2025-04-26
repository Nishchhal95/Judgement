using Photon.Pun;
using Photon.Realtime;
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
        PhotonNetworkManager.OnConnectedToPhotonLobby += ConnectedToPhotonLobby;
        playerNameInputField.onValueChanged.AddListener(OnPlayerInputFieldValueChanged);
        
        playerIconButton.onClick.AddListener(OnPlayerIconButtonPressed);
        createGameButton.onClick.AddListener(OnCreateGamePressed);
        joinGameButton.onClick.AddListener(OnJoinGamePressed);

        LocalPlayerInfo.OnLocalPlayerIconChanged += ChangeLocalPlayerIcon;

        SetDefaults();
    }

    private void OnDisable()
    {
        PhotonNetworkManager.OnConnectedToPhotonLobby -= ConnectedToPhotonLobby;
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
        
        windowManager.ShowWindow("LOADING_WINDOW");
    }
    
    private void ConnectedToPhotonLobby()
    {
        windowManager.HideCurrentWindow();
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
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 8,
            PublishUserId = true
        };

        PhotonNetwork.CreateRoom("DEMO_ROOM", roomOptions);
        windowManager.ShowWindow("WAITING_ROOM_WINDOW");
    }

    private void OnJoinGamePressed()
    {
        // Open Game Join
        PhotonNetwork.JoinRoom("DEMO_ROOM");
        windowManager.ShowWindow("WAITING_ROOM_WINDOW");
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
