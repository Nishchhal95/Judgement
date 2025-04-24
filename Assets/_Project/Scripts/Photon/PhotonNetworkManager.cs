using System;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    public static Action OnLocalPlayerJoinedRoom;
    public static Action<Player> OnPlayerJoinedRoom;
    public static Action<Player> OnPlayerLeaveRoom;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
    //Callbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        OnLocalPlayerJoinedRoom?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        OnPlayerJoinedRoom?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        OnPlayerLeaveRoom?.Invoke(otherPlayer);
    }
}
