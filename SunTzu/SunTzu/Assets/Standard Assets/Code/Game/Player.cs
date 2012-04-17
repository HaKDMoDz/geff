using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Object
{
    public List<Card> ListCardInDrawPile;
    public List<Card> ListCardInHand;
    public List<Card> ListCardInDiscardPile;

    public PlayerType PlayerType = PlayerType.AI;

    public Player(PlayerType playerType)
    {
        this.PlayerType = playerType;
    }

    public void CreateCards()
    {
        ListCardInDrawPile = new List<Card>();
        ListCardInHand = new List<Card>();
        ListCardInDiscardPile = new List<Card>();

        CreateCard(CardType.PawnValue, 1);
        CreateCard(CardType.PawnValue, 2);
        CreateCard(CardType.PawnValue, 3);
        CreateCard(CardType.PawnValue, 4);
        CreateCard(CardType.PawnValue, 5);
        CreateCard(CardType.PawnValue, 6);
        CreateCard(CardType.PawnMinusValue, -1);
        CreateCard(CardType.PawnBonusValue, 1);
        CreateCard(CardType.PawnBonusValue, 2);
        CreateCard(CardType.PawnBonusValue, 3);
        CreateCard(CardType.Sick, 0);
    }

    private void CreateCard(CardType cardType, int cardValue)
    {
		GameObject gameLogic = GameObject.Find("GameLogic");
		Game game = gameLogic.GetComponent<Game>();
		
		Transform transNewCard = (Transform)GameObject.Instantiate(game.transCard);
		
		Card card = transNewCard.GetComponent<Card>();
        card.CardType = cardType;
        card.CardValue = cardValue;
		
		ListCardInDrawPile.Add(card);
    }

    public void PickCards(int nbCardToPick, bool randomly)
    {
        for (int i = 0; i < nbCardToPick; i++)
        {
            int indexCard = i;
            
            if(randomly)
                indexCard = Random.Range(0, ListCardInDrawPile.Count - 1);

            Card pickedCard = ListCardInDrawPile[indexCard];

            ListCardInHand.Add(pickedCard);
            ListCardInDrawPile.Remove(pickedCard);
        }
    }
}

public enum PlayerType
{
    Human,
    AI
}
