using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private GameWindow[] gameWindows;
    
    private Dictionary<string, GameWindow> idToGameWindowMap = new Dictionary<string, GameWindow>();
    private Stack<GameWindow> gameWindowsStack = new Stack<GameWindow>();

    private void Start()
    {
        foreach (GameWindow gameWindow in gameWindows)
        {
            idToGameWindowMap.Add(gameWindow.GetWindowId(), gameWindow);
        }
    }

    public void ShowWindow(string windowId)
    {
        if(!idToGameWindowMap.TryGetValue(windowId, out GameWindow gameWindow))
        {
            Debug.LogError("Cannot find a Game Window with ID " + windowId);
            return;
        }

        while (gameWindowsStack.Count > 0)
        {
            gameWindowsStack.Pop().Hide();
        }
        
        gameWindow.Show();
        gameWindowsStack.Push(gameWindow);
    }
}
