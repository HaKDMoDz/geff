using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public Transform transCard;
    public List<Player> ListPlayer;
    public static GameState GameState = GameState.PickCardInHand;
    public static Player Player;

    void Start()
    {
        CreatePlayers();
        StartGame();

        //int nbCard = 10;
        //float leftLimit = -1.9f;
        //float rightLimit = 1.9f;
        //ListCard = new List<Card>();

        //for (int i = 0; i <= nbCard; i++)
        //{
        //    Vector3 vec = new Vector3(leftLimit + (float)i * (rightLimit - leftLimit) / (float)(nbCard), 0.92f+ (float)i * 0.001f, -8.17f);
        //    Transform transNewCard = (Transform)GameObject.Instantiate(transCard, vec, Quaternion.Euler(325,180f,180f));
        //    Card card = transNewCard.gameObject.GetComponent<Card>();
        //    ListCard.Add(card);
        //}
    }

    private void CreatePlayers()
    {
        ListPlayer = new List<Player>();

        Player player1 = new Player(PlayerType.Human);
        player1.CreateCards();
        ListPlayer.Add(player1);
        Player = player1;

        Player player2 = new Player(PlayerType.AI);
        player2.CreateCards();
        ListPlayer.Add(player2);
    }

    private void StartGame()
    {
        foreach (Player player in ListPlayer)
        {
            player.PickCards(6, false);
            player.PickCards(4, true);
        }

        Game.GameState = global::GameState.PrepareCardBeforPicking;
    }

    void Update()
    {
        if (Game.GameState == global::GameState.PrepareCardBeforPicking)
        {
            Game.Player.SortPileInHand(true);
            Game.GameState = global::GameState.PickCardInHand;
        }
    }
}

public enum GameState
{
    PrepareCardBeforPicking,
    PickCardInHand,
    CardPickedInHand,
    WaitingTurnValidation
}
