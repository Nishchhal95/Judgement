using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WatiingPlayersGameWindow : GameWindow
{

    [SerializeField] private WaitingPlayerMenuUI waitingPlayerMenuUIPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private GameInfo gameInfo;

    private Dictionary<Player, WaitingPlayerMenuUI> playerToPlayerUIMap = new Dictionary<Player, WaitingPlayerMenuUI>();

    protected override void OnEnable()
    {
        base.OnEnable();
        
        PhotonNetworkManager.OnLocalPlayerJoinedRoom += OnLocalPlayerJoinedRoom;
        PhotonNetworkManager.OnPlayerJoinedRoom += OnPlayerJoinedRoom;
        PhotonNetworkManager.OnPlayerLeaveRoom += OnPlayerLeftRoom;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        PhotonNetworkManager.OnLocalPlayerJoinedRoom -= OnLocalPlayerJoinedRoom;
        PhotonNetworkManager.OnPlayerJoinedRoom -= OnPlayerJoinedRoom;
        PhotonNetworkManager.OnPlayerLeaveRoom -= OnPlayerLeftRoom;
    }
    
    private void OnLocalPlayerJoinedRoom()
    {
        //Show Local Player First
        WaitingPlayerMenuUI playerUI = Instantiate(waitingPlayerMenuUIPrefab, content);
        playerUI.Initialize(PhotonNetwork.LocalPlayer.NickName, gameInfo.iconSprites[LocalPlayerInfo.IconIndex]);
        playerToPlayerUIMap.Add(PhotonNetwork.LocalPlayer, playerUI);
        
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                continue;
            }
            
            if (player.CustomProperties.TryGetValue("icon", out object iconIndexObj))
            {
                int iconIndex = (int)iconIndexObj;
                WaitingPlayerMenuUI remotePlayerUI = Instantiate(waitingPlayerMenuUIPrefab, content);
                remotePlayerUI.Initialize(player.NickName, gameInfo.iconSprites[iconIndex]);
                playerToPlayerUIMap.Add(player, remotePlayerUI);
            }
        }
    }

    private void OnPlayerJoinedRoom(Player player)
    {
        if (!player.CustomProperties.TryGetValue("icon", out object iconIndexObj))
        {
            return;
        }
        int iconIndex = (int)iconIndexObj;
        WaitingPlayerMenuUI remotePlayerUI = Instantiate(waitingPlayerMenuUIPrefab, content);
        remotePlayerUI.Initialize(player.NickName, gameInfo.iconSprites[iconIndex]);
        playerToPlayerUIMap.Add(player, remotePlayerUI);
    }

    private void OnPlayerLeftRoom(Player player)
    {
        if (!playerToPlayerUIMap.TryGetValue(player, out WaitingPlayerMenuUI waitingPlayerMenuUI))
        {
            return;
        }
        
        Destroy(waitingPlayerMenuUI.gameObject);
        playerToPlayerUIMap.Remove(player);
    }

    public override string GetWindowId()
    {
        return "WAITING_ROOM_WINDOW";
    }
}
