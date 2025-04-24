using System;

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
            OnLocalPlayerNameChanged?.Invoke();
        }
    }
    
    public static int IconIndex
    {
        get => iconIndex;
        set
        {
            iconIndex = value;
            OnLocalPlayerIconChanged?.Invoke();
        }
    }
}
