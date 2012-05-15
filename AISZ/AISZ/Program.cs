using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISZ
{
    class Program
    {
        static List<Card> listPlayerCard;
        static List<Card> listEnnemyCard;
        static Board[] Boards;
        static List<Choice> Choices;
        static void Main(string[] args)
        {
            Random rnd = new Random();

            InitCards(listPlayerCard);
            InitCards(listEnnemyCard);

            Boards = new Board[5];
            for (int i = 0; i < 5; i++)
            {
                Boards[i] = new Board();
                Boards[i].Scores = new int[3];

                for (int j = 0; j < 3; j++)
                {
                    Boards[i].Scores[j] = rnd.Next(1, 4);
                }
            }
        }

        private static void PlayerTurn()
        {
            Choices = new List<Choice>();


            foreach (Card card in listPlayerCard)
            {
                Choice choice = new Choice();
                choice.ChoiceBoards = new ChoiceBoard[5];
                choice.ChoiceBoards[0] = new ChoiceBoard();
                choice.ChoiceBoards[0].Board = Boards[0];
                choice.ChoiceBoards[0].PlayerCard = card;

                PlayerTurnRecursively(choice, 0);
            }
        }

        private static void PlayerTurnRecursively(Choice choice, int numBoard)
        {


            foreach (Card card in listEnnemyCard)
            {
                Choice newChoice = choice.Clone();

                newChoice.ChoiceBoards[numBoard].EnnemyCard = card;

                Choices.Add(newChoice);


            }
        }

        private static void InitCards(List<Card> listCard)
        {
            listCard = new List<Card>();

            for (int i = 0; i < 11; i++)
            {
                listCard.Add(new Card() { CardType = CardType.Value, Value = i });
            }

            listCard.Add(new Card() { CardType = CardType.MinusValue, Value = -1 });
            listCard.Add(new Card() { CardType = CardType.PlusValue, Value = 1 });
            listCard.Add(new Card() { CardType = CardType.PlusValue, Value = 1 });
        }
    }

    public class Board
    {
        public int[] Scores { get; set; }
    }

    public class ChoiceBoard
    {
        public Card PlayerCard { get; set; }
        public Card EnnemyCard { get; set; }
        public Board Board { get; set; }
    }

    public class Choice
    {
        public ChoiceBoard[] ChoiceBoards { get; set; }

        public Choice Clone()
        {
            Choice clone = new Choice();
            clone.ChoiceBoards = new ChoiceBoard[5];
            
            for (int i = 0; i < 5; i++)
            {
                if (this.ChoiceBoards[i] != null)
                {
                    clone.ChoiceBoards[i] = new ChoiceBoard();
                    clone.ChoiceBoards[i].Board = this.ChoiceBoards[i].Board;
                    clone.ChoiceBoards[i].PlayerCard = this.ChoiceBoards[i].PlayerCard;
                    clone.ChoiceBoards[i].EnnemyCard = this.ChoiceBoards[i].EnnemyCard;
                }
            }

            return clone;
        }
    }

    public class Card
    {
        public CardType CardType { get; set; }
        public int Value { get; set; }
    }

    public enum CardType
    {
        Value,
        PlusValue,
        MinusValue
    }
}
