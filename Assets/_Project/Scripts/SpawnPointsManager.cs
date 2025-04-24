using System;
using UnityEngine;

public class SpawnPointsManager : MonoBehaviour
{
    [SerializeField] private PlayerToSpawnPoints[] playerToSpawnPointsArray;

    public Transform[] GetSpawnPointsByPlayerCount(int playerCount)
    {
        foreach (var playerSpawnPoints in playerToSpawnPointsArray)
        {
            if (playerSpawnPoints.PlayerCount == playerCount)
            {
                return playerSpawnPoints.SpawnPoints;
            }
        }

        return null;
    }
}

[Serializable]
public class PlayerToSpawnPoints
{
    [field: SerializeField] public int PlayerCount { get; set; }
    [field: SerializeField] public Transform[] SpawnPoints { get; set; }
}
