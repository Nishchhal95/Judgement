using UnityEngine;

public class GameController : MonoBehaviour
{
    private const int PLAYER_COUNT = 8;
    
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GamePlayer localPlayerPrefab;
    [SerializeField] private GamePlayer remotePlayerPrefab;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private SpawnPointsManager spawnPointsManager;
    [SerializeField] private Sprite localPlayerIcon;
    [SerializeField] private Sprite remotePlayerIcon;

    private GamePlayer localPlayer;
    private GamePlayer[] gamePlayers;

    private void Start()
    {
        cardManager.LoadCards();
        cardManager.ShuffleCards();

        SpawnPlayers();
        
        DealCards();
        
        // Set Trump Suit
        
        // Let Players BID in clockwise order
        
        // Start Game
    }

    private void SpawnPlayers()
    {
        gamePlayers = new GamePlayer[PLAYER_COUNT];
        Transform[] spawnPoints = spawnPointsManager.GetSpawnPointsByPlayerCount(PLAYER_COUNT);
        for (int i = 0; i < PLAYER_COUNT; i++)
        {
            if (i == 0)
            {
                localPlayer = Instantiate(localPlayerPrefab, spawnPoints[i]);
                localPlayer.SetPlayerName("LPlayer");
                localPlayer.SetPlayerIcon(localPlayerIcon);
                gamePlayers[i] = localPlayer;
                continue;
            }
            
            GamePlayer remoteGamePlayer = Instantiate(remotePlayerPrefab, spawnPoints[i]);
            remoteGamePlayer.SetPlayerName("RPlayer" + i);
            remoteGamePlayer.SetPlayerIcon(remotePlayerIcon);
            gamePlayers[i] = remoteGamePlayer;
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
                if (i == 0)
                {
                    card.transform.localPosition = new Vector3(66F, 0,0) + new Vector3(j * 50, 0, 0);
                }
                else
                {
                    card.transform.localPosition = new Vector3(j * 10, 0, 0);
                    card.FaceDown();
                }

                gamePlayers[i].AddCard(card);
            }
        }
    }
}
