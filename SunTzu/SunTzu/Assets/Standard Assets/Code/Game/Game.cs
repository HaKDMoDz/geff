using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public Transform transCard;
    public List<Player> ListPlayer;
    public static GameState GameState = GameState.PrepareAnimationCardInHandBeforPicking;
    public Player Player;

    private float startTimeAnimationCard = 0f;

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

        Game.GameState = GameState.PrepareAnimationCardInHandBeforPicking;
    }

    void Update()
    {
        if (Game.GameState == global::GameState.PrepareAnimationCardInHandBeforPicking)
        {
            float i = 0f;
            foreach (Card card in Player.ListCardInHand)
            {
                card.transform.position = new Vector3(2.5f, 0.92f + i * 0.001f, -8.17f);
                card.transform.rotation = Quaternion.Euler(325, 180f, 180f);
                i++;
            }

            startTimeAnimationCard = Time.time;
            Game.GameState = global::GameState.AnimateCardInHandBeforPicking;
        }
        else if (Game.GameState == global::GameState.AnimateCardInHandBeforPicking)
        {
            float leftLimit = -1.7f;
            float rightLimit = 2.1f;
            float animationDuration = 1f;
            float currentTime = Time.time;

            for (int i = 0; i < Player.ListCardInHand.Count; i++)
            {
                Card card = Player.ListCardInHand[i];

                float x = Mathf.Lerp(2.5f, leftLimit + (float)i * (rightLimit - leftLimit) / (float)(Player.ListCardInHand.Count), (currentTime - startTimeAnimationCard) / animationDuration);
                card.transform.position = new Vector3(x, card.transform.position.y, card.transform.position.z);
                card.initialLocation = card.transform.position;
            }

            if (currentTime - startTimeAnimationCard >= animationDuration)
                Game.GameState = global::GameState.PickCardInHand;
        }
    }
}

public enum GameState
{
    PrepareAnimationCardInHandBeforPicking,
    AnimateCardInHandBeforPicking,
    PickCardInHand,
    WaitingTurnValidation
}
