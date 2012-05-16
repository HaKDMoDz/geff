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

            Boards = new Board[5];
            for (int i = 0; i < Boards.Length; i++)
            {
                Boards[i] = new Board();
                Boards[i].IdBoard = i;
                Boards[i].Scores = new int[3];

                for (int j = 0; j < 3; j++)
                {
                    Boards[i].Scores[j] = rnd.Next(1, 4);
                }
            }

            InitCards(ref listPlayerCard);
            InitCards(ref listEnnemyCard);

            PlayerTurn();
        }

        private static void PlayerTurn()
        {
            Choices = new List<Choice>();


            //for (int j = 0; j < listPlayerCard.Count; j++)
            {
                Choice choice = new Choice();
                //Choices.Add(choice);

                choice.ChoiceBoards = new ChoiceBoard[Boards.Length];
                choice.IdPlayerCards = new List<int>();
                choice.IdEnnemyCards = new List<int>();

                for (int i = 0; i < listPlayerCard.Count; i++)
                {
                    choice.IdPlayerCards.Add(i);
                    choice.IdEnnemyCards.Add(i);
                }

                //choice.SetPlayerChoice(Boards[0], j);

                PlayerTurnRecursively(choice, 0);
            }
        }

        private static void PlayerTurnRecursively(Choice choice, int numBoard)
        {
            if (numBoard >= Boards.Length)
            {
                //Console.WriteLine(choice.ToString());
                Choices.Add(choice);

                return;
            }

            for (int j = 0; j < choice.IdPlayerCards.Count; j++)
            {
                /*
                if (j == 1 && numBoard == 0)
                {
                    int a = 0;
                }


                //Console.WriteLine("J : {0} - NumBoard : {1}", j, numBoard);
                Choice newChoice = choice;

                if (choice.IdPlayerCards.Count!=1)
                {
                    newChoice = choice.Clone();
                }

                //if(numBoard>0)
                    newChoice.SetPlayerChoice(Boards[numBoard], j);


                PlayerTurnRecursively(newChoice, numBoard + 1);
                */
                
                for (int i = 0; i < choice.IdEnnemyCards.Count; i++)
                {
                    Choice newChoice = choice.Clone();

                    /*
                    if (j+i>0 || numBoard==0)
                    {
                        newChoice = choice.Clone();
                        //Choices.Add(newChoice);
                    }*/

                    newChoice.SetPlayerChoice(Boards[numBoard], j);
                    newChoice.SetEnnemyChoice(Boards[numBoard], i);


                    PlayerTurnRecursively(newChoice, numBoard + 1);
                }
                 
            }
        }

        private static void InitCards(ref List<Card> listCard)
        {
            listCard = new List<Card>();

            //for (int i = 0; i < Boards.Length; i++)
            //{
            //    listCard.Add(new Card() { CardType = CardType.Value, Value = i });
            //}

            ////listCard.Add(new Card() { CardType = CardType.MinusValue, Value = -1 });
            ////listCard.Add(new Card() { CardType = CardType.PlusValue, Value = 1 });
            ////listCard.Add(new Card() { CardType = CardType.PlusValue, Value = 2 });

            for (int i = 0; i < 5; i++)
            {
                listCard.Add(new Card() { CardType = CardType.Value, Value = i });
            }

            listCard.Add(new Card() { CardType = CardType.MinusValue, Value = -1 });
            listCard.Add(new Card() { CardType = CardType.PlusValue, Value = 1 });
            listCard.Add(new Card() { CardType = CardType.PlusValue, Value = 2 });
        }
    }

    public class Board
    {
        public int IdBoard { get; set; }
        public int[] Scores { get; set; }
    }

    public class ChoiceBoard
    {
        public int IdPlayerCard { get; set; }
        public int IdEnnemyCard { get; set; }
        public int IdBoard { get; set; }
    }

    public class Choice
    {
        public ChoiceBoard[] ChoiceBoards { get; set; }

        public List<int> IdPlayerCards { get; set; }
        public List<int> IdEnnemyCards { get; set; }

        public void SetPlayerChoice(Board board, int idIdPlayerCard)
        {
            if (this.ChoiceBoards[board.IdBoard] == null)
                this.ChoiceBoards[board.IdBoard] = new ChoiceBoard();

            this.ChoiceBoards[board.IdBoard].IdBoard = board.IdBoard;
            this.ChoiceBoards[board.IdBoard].IdPlayerCard = IdPlayerCards[idIdPlayerCard];
            this.IdPlayerCards.RemoveAt(idIdPlayerCard);
        }

        public void SetEnnemyChoice(Board board, int idIdEnnemyCard)
        {
            if (this.ChoiceBoards[board.IdBoard] == null)
                this.ChoiceBoards[board.IdBoard] = new ChoiceBoard();

            this.ChoiceBoards[board.IdBoard].IdBoard = board.IdBoard;
            this.ChoiceBoards[board.IdBoard].IdEnnemyCard = IdEnnemyCards[idIdEnnemyCard];
            this.IdEnnemyCards.RemoveAt(idIdEnnemyCard);
        }

        public Choice Clone()
        {
            Choice clone = new Choice();
            clone.ChoiceBoards = new ChoiceBoard[this.ChoiceBoards.Length];
            clone.IdPlayerCards = new List<int>();
            clone.IdEnnemyCards = new List<int>();

            for (int i = 0; i < this.ChoiceBoards.Length; i++)
            {
                if (this.ChoiceBoards[i] != null)
                {
                    clone.ChoiceBoards[i] = new ChoiceBoard();
                    clone.ChoiceBoards[i].IdBoard = this.ChoiceBoards[i].IdBoard;
                    clone.ChoiceBoards[i].IdPlayerCard = this.ChoiceBoards[i].IdPlayerCard;
                    clone.ChoiceBoards[i].IdEnnemyCard = this.ChoiceBoards[i].IdEnnemyCard;
                }
            }

            clone.IdPlayerCards.AddRange(this.IdPlayerCards.ToArray());
            clone.IdEnnemyCards.AddRange(this.IdEnnemyCards.ToArray());

            return clone;
        }

        public override string ToString()
        {
            string str = string.Empty;

            for (int i = 0; i < ChoiceBoards.Length; i++)
            {
                str = String.Format("{0} [{1} ; {2}]", str, ChoiceBoards[i].IdPlayerCard, ChoiceBoards[i].IdEnnemyCard);
            }

            return str;
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
