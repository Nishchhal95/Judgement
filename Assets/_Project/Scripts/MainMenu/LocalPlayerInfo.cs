using System;
using ExitGames.Client.Photon;
using Photon.Pun;

[Serializable]
public static class LocalPlayerInfo
{
    public static Action OnLocalPlayerNameChanged;
    public static Action OnLocalPlayerIconChanged;
    
    private static string name;
    private static int iconIndex;

    public static string Name
    {
        get => name;
        set
        {
            name = value;
            PhotonNetwork.LocalPlayer.NickName = value;
            OnLocalPlayerNameChanged?.Invoke();
        }
    }
    
    public static int IconIndex
    {
        get => iconIndex;
        set
        {
            iconIndex = value;
            Hashtable playerProperties = new Hashtable
            {
                ["icon"] = iconIndex
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
            OnLocalPlayerIconChanged?.Invoke();
        }
    }
}
