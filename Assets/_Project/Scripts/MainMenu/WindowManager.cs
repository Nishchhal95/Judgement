using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private GameWindow[] gameWindows;
    
    private Dictionary<string, GameWindow> idToGameWindowMap = new Dictionary<string, GameWindow>();
    private Stack<GameWindow> gameWindowsStack = new Stack<GameWindow>();

    private Queue<Action> windowActions = new Queue<Action>();

    private void Awake()
    {
        foreach (GameWindow gameWindow in gameWindows)
        {
            gameWindow.Inject(this);
            idToGameWindowMap.Add(gameWindow.GetWindowId(), gameWindow);
        }

        while (windowActions.Count > 0)
        {
            windowActions.Dequeue()?.Invoke();
        }
    }

    public void ShowWindow(string windowId)
    {
        if (idToGameWindowMap.Count == 0)
        {
            windowActions.Enqueue(() => ShowWindowInternal(windowId));
            return;
        }

        ShowWindowInternal(windowId);
    }

    private void ShowWindowInternal(string windowId)
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

    public void HideCurrentWindow()
    {
        if (gameWindowsStack.Count > 0)
        {
            gameWindowsStack.Pop().Hide();
        }
    }
}
