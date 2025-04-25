using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    private static int PLAYER_COUNT = 8;
    
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GamePlayer localPlayerPrefab;
    [SerializeField] private GamePlayer remotePlayerPrefab;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private SpawnPointsManager spawnPointsManager;
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private BiddingWindow biddingWindow;

    private Player[] roomPlayers;
    private GamePlayer localPlayer;
    // This will always have Local at Index 0 and it is visually different for every player.
    private GamePlayer[] gamePlayers;
    private int currentRound = 0;
    private Suit trumpSuit = Suit.Spades; 
    private int totalTrumpSuits = 5;
    
    private int currentPlayerToBidIndex = 0;
    private Dictionary<int, int> actorIdToBidMap = new Dictionary<int, int>();

    private void Start()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        roomPlayers = PhotonNetwork.PlayerList;
        PLAYER_COUNT = roomPlayers.Length;
        cardManager.LoadCards();

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        int seed = Random.Range(1, 9999);
        photonView.RPC("SetupGameRPC", RpcTarget.All, seed);
    }

    [PunRPC]
    private void SetupGameRPC(int seed)
    {
        cardManager.ShuffleCards(seed);
        SpawnPlayers();
        DealCards();
        trumpSuit = (Suit)(currentRound % totalTrumpSuits);
        StartBidding();
    }

    private void SpawnPlayers()
    { 
        gamePlayers = new GamePlayer[PLAYER_COUNT];
        Transform[] spawnPoints = spawnPointsManager.GetSpawnPointsByPlayerCount(PLAYER_COUNT);
        int localPlayerActorId = PhotonNetwork.LocalPlayer.ActorNumber;
        int localPlayerIndex = localPlayerActorId - 1;
        
        int roomPlayerIndex = localPlayerIndex;
        int localIndex = 0;
        // Spawn Local Player
        Player localPhotonPlayer = roomPlayers[roomPlayerIndex];
        localPlayer = Instantiate(localPlayerPrefab, spawnPoints[localIndex]);
        localPlayer.SetPlayerName(localPhotonPlayer.NickName);
        localPlayer.SetPlayerIcon(GetIconForPlayer(localPhotonPlayer));
        localPlayer.SetPhotonPlayer(localPhotonPlayer);
        gamePlayers[roomPlayerIndex] = localPlayer;

        localIndex++;
        roomPlayerIndex++;
        while (localIndex < PLAYER_COUNT)
        {
            Player remotePhotonPlayer = roomPlayers[roomPlayerIndex % PLAYER_COUNT];
            GamePlayer remoteGamePlayer = Instantiate(remotePlayerPrefab, spawnPoints[localIndex]);
            remoteGamePlayer.SetPlayerName(remotePhotonPlayer.NickName);
            remoteGamePlayer.SetPlayerIcon(GetIconForPlayer(remotePhotonPlayer));
            remoteGamePlayer.SetPhotonPlayer(remotePhotonPlayer);
            gamePlayers[roomPlayerIndex % PLAYER_COUNT] = remoteGamePlayer;
            localIndex++;
            roomPlayerIndex++;
        }
    }

    private void DealCards()
    {
        int cardPerPlayer = 52 / PLAYER_COUNT;

        for (int i = 0; i < PLAYER_COUNT; i++)
        {
            for (int j = 0; j < cardPerPlayer; j++)
            {
                CardData cardData = cardManager.GetCardFromDeck();
                Card card = Instantiate(cardPrefab, gamePlayers[i].CardContainer);
                card.Initialize(cardData, cardManager.GetBackCardSprite());
                if (gamePlayers[i].IsLocal)
                {
                    card.transform.localPosition = new Vector3(66F, 0,0) + new Vector3(j * 50, 0, 0);
                }
                else
                {
                    card.transform.localPosition = new Vector3(j * 10, 0, 0);
                    //card.FaceDown();
                }

                gamePlayers[i].AddCard(card);
            }
        }
    }
    
    private void StartBidding()
    {
        if (roomPlayers[currentPlayerToBidIndex].UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            ShowBiddingUI();
        }
    }
    
    private void ShowBiddingUI()
    {
        int cardPerPlayer = 52 / PLAYER_COUNT;
        int bidToSkip = -1;
        if (currentPlayerToBidIndex == PLAYER_COUNT - 1)
        {
            int totalBid = 0;
            foreach (int bid in actorIdToBidMap.Values)
            {
                totalBid += bid;
            }

            if (totalBid < cardPerPlayer)
            {
                bidToSkip = cardPerPlayer - totalBid;
            }
        }
        biddingWindow.Initialize(cardPerPlayer, OnBidValueSelected, bidToSkip);
    }

    private void OnBidValueSelected(int bidValue)
    {
        photonView.RPC("SetBidRPC", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, bidValue);
    }

    [PunRPC]
    private void SetBidRPC(int actorNumber, int bidValue)
    {
        actorIdToBidMap.Add(actorNumber, bidValue);
        currentPlayerToBidIndex++;
        if (currentPlayerToBidIndex >= PLAYER_COUNT)
        {
            StartGame();
            return;
        }
        
        if (roomPlayers[currentPlayerToBidIndex].UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            ShowBiddingUI();
        }
    }

    private void StartGame()
    {
        Debug.Log("Game Starting");
    }

    private Sprite GetIconForPlayer(Player player)
    {
        if (!player.CustomProperties.TryGetValue("icon", out object iconIndexObj))
        {
            return defaultIcon;
        }
        
        int iconIndex = (int)iconIndexObj;
        return gameInfo.iconSprites[iconIndex];

    }
}
