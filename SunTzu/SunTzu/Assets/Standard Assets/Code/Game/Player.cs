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

        CreateCard(CardType.PawnValue, 1, 0);
        CreateCard(CardType.PawnValue, 2, 1);
        CreateCard(CardType.PawnValue, 3, 2);
        CreateCard(CardType.PawnValue, 4, 3);
        CreateCard(CardType.PawnValue, 5, 4);
        CreateCard(CardType.PawnValue, 6, 5);
        CreateCard(CardType.PawnValue, 7, 6);
        CreateCard(CardType.PawnValue, 8, 7);
        CreateCard(CardType.PawnValue, 9, 8);
        CreateCard(CardType.PawnValue, 10, 9);
        CreateCard(CardType.PawnMinusValue, -1, 10);
        CreateCard(CardType.PawnMinusValue, -1, 11);
        CreateCard(CardType.PawnMinusValue, -1, 12);
        CreateCard(CardType.PawnBonusValue, 1, 13);
        CreateCard(CardType.PawnBonusValue, 1, 14);
        CreateCard(CardType.PawnBonusValue, 1, 15);
        CreateCard(CardType.PawnBonusValue, 2, 16);
        CreateCard(CardType.PawnBonusValue, 3, 17);
        CreateCard(CardType.Sick, 4, 18);
        CreateCard(CardType.Sick, 4, 19);
    }

    private void CreateCard(CardType cardType, int cardValue, int index)
    {
        GameObject gameLogic = GameObject.Find("GameLogic");
        Game game = gameLogic.GetComponent<Game>();

        Transform transNewCard = (Transform)GameObject.Instantiate(game.transCard);

        transNewCard.gameObject.renderer.enabled = false;

        Card card = transNewCard.GetComponent<Card>();
        card.CardType = cardType;
        card.CardValue = cardValue;
        card.Index = index;

        ListCardInDrawPile.Add(card);
    }

    public void PickCards(int nbCardToPick, bool randomly, bool showCard)
    {
        for (int i = 0; i < nbCardToPick; i++)
        {
            int indexCard = i;

            if (randomly)
                indexCard = Random.Range(0, ListCardInDrawPile.Count - 1);

            Card pickedCard = ListCardInDrawPile[indexCard];

            pickedCard.renderer.enabled = showCard;

            ListCardInHand.Add(pickedCard);
            ListCardInDrawPile.Remove(pickedCard);
        }
    }

    public void SortPileInHand(bool firstSort)
    {
        if (ListCardInHand == null || ListCardInHand.Count == 0)
            return;

        float cardWidth = ListCardInHand[0].transform.collider.bounds.size.x;
        float space = 0.01f;
        float nbCard = (float)ListCardInHand.Count;
        float large = (nbCard - 1) * (cardWidth + space);


        ListCardInHand.Sort(new ComparerCard());

        for (int i = 0; i < ListCardInHand.Count; i++)
        {
            Card card = ListCardInHand[i];

            if (firstSort)
            {
                card.transform.position = new Vector3(4.2f, 1.5f + (float)i * 0.001f, -0.6f);
                card.transform.rotation = Quaternion.Euler(325, 180f, 180f);
            }

            card.StartLocation = card.transform.position;
            card.Location = new Vector3(-large/2f+((float)i)*(cardWidth+space*2f), 1.5f + (float)i * 0.001f, -0.6f);
            card.LastTimeAnimation = Time.time;
			card.DurationAnimation = 0.7f;
        }
    }
}

public class ComparerCard : Comparer<Card>
{
    public override int Compare(Card x, Card y)
    {
		int val =x.Index.CompareTo(y.Index);
        return val;
    }
}

public enum PlayerType
{
    Human,
    AI
}
