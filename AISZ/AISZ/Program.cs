using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AISZ
{
    class Program
    {
        static Game game;

        static void Main(string[] args)
        {
            game = new Game();
            game.Init();

            while (!game.IsFinished)
            {
                game.NextTurn();
                game.PrintScore();
            }
        }
    }

    public class Board
    {
        public int IdBoard { get; set; }
        public int[] Scores { get; set; }
        public int PawnsFriendPlayer { get; set; }
    }

    public class ChoiceBoard
    {
        public int IdPlayerCard { get; set; }
        public int IdBoard { get; set; }
        public int Score { get; set; }
    }

    public class Choice
    {
        public ChoiceBoard[] ChoiceBoards { get; set; }
        public int Score { get; set; }
        public List<int> IdPlayerCards { get; set; }

        public Choice()
        {
            IdPlayerCards = new List<int>();
            ChoiceBoards = new ChoiceBoard[5];
        }

        public void SetPlayerChoice(Board board, int idIdPlayerCard)
        {
            if (this.ChoiceBoards[board.IdBoard] == null)
                this.ChoiceBoards[board.IdBoard] = new ChoiceBoard();

            this.ChoiceBoards[board.IdBoard].IdBoard = board.IdBoard;
            this.ChoiceBoards[board.IdBoard].IdPlayerCard = IdPlayerCards[idIdPlayerCard];
            this.IdPlayerCards.RemoveAt(idIdPlayerCard);
        }

        public Choice Clone()
        {
            Choice clone = new Choice();

            for (int i = 0; i < this.ChoiceBoards.Length; i++)
            {
                if (this.ChoiceBoards[i] != null)
                {
                    clone.ChoiceBoards[i] = new ChoiceBoard();
                    clone.ChoiceBoards[i].IdBoard = this.ChoiceBoards[i].IdBoard;
                    clone.ChoiceBoards[i].IdPlayerCard = this.ChoiceBoards[i].IdPlayerCard;
                }
            }

            clone.IdPlayerCards.AddRange(this.IdPlayerCards.ToArray());

            return clone;
        }

        public override string ToString()
        {
            string str = string.Empty;

            for (int i = 0; i < ChoiceBoards.Length; i++)
            {
                str = String.Format("{0} [{1}]", str, ChoiceBoards[i].IdPlayerCard);
            }

            return str;
        }
    }

    public class Card
    {
        public CardType CardType { get; set; }
        public int Value { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            string val = "";

            if (CardType == CardType.PlusValue)
                val = "+";

            val += Value.ToString();

            return val;
        }
    }

    public class Player
    {
        public List<Card> ListCard { get; set; }
        public List<int> ListIndexAvailableCard { get; set; }
        public List<int> ListIndexPickedCard { get; set; }
        public int FreePawns { get; set; }
        public int Score { get; set; }

        public Player()
        {
            ListCard = new List<Card>();
            ListIndexPickedCard = new List<int>();
            ListIndexAvailableCard = new List<int>();
            Score = 0;
            FreePawns = 17;
        }

        public void PickCard()
        {
            if (ListIndexAvailableCard.Count > 0)
            {
                Random rnd = new Random();
                int indexPickedCard = ListIndexAvailableCard[rnd.Next(0, ListIndexAvailableCard.Count)];
                ListIndexAvailableCard.Remove(indexPickedCard);
                ListIndexPickedCard.Add(indexPickedCard);
            }
        }

        public void DisposeCards(Choice choice)
        {
            /*
            if (choice != null)
            {
                foreach (ChoiceBoard choiceBoard in choice.ChoiceBoards)
                {
                    Card cardChoosed = ListCard[choiceBoard.IdPlayerCard];

                    if (cardChoosed.CardType != CardType.Value || cardChoosed.Value > 6)
                    {
                        ListIndexAvailableCard.Remove(cardChoosed.Index);
                    }
                }
            }
            */
            ListIndexPickedCard = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                ListIndexPickedCard.Add(i);
            }
        }

        //public void SortPickedCard()
        //{
        //    for (int i = 0; i < ListIndexPickedCard.Count; i++)
        //    {
        //        ListIndexPickedCard[i].Index = i;
        //    }
        //}
    }

    public enum CardType
    {
        Value,
        PlusValue,
        MinusValue
    }

    public class Game
    {
        public Player Winner;
        public bool IsFinished
        {
            get
            {
                return turn == 9 || Winner != null;
            }
        }

        private int turn;
        private Player friendPlayer;
        private Player oppositePlayer;

        private Board[] Boards;
        //private List<Choice> Choices;
        private Choice oppositePlayerChoice;
        private Choice bestFriendPlayerChoice;

        private Random rnd = new Random();

        public void Init()
        {
            rnd = new Random();

            friendPlayer = new Player();
            oppositePlayer = new Player();

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

            InitCards(ref friendPlayer);
            InitCards(ref oppositePlayer);
        }

        public void NextTurn()
        {
            turn++;

            friendPlayer.DisposeCards(bestFriendPlayerChoice);
            oppositePlayer.DisposeCards(oppositePlayerChoice);

            if (turn == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    friendPlayer.PickCard();
                    oppositePlayer.PickCard();
                }
            }
            else
            {
                friendPlayer.PickCard();
                oppositePlayer.PickCard();
            }


            //friendPlayer.SortPickedCard();
            //oppositePlayer.SortPickedCard();

            OppositePlayerTurn();
            FriendPlayerTurn();
        }

        public void PrintScore()
        {
            if (bestFriendPlayerChoice.Score == 0)
                return;

            Console.WriteLine(new String('_', 80));
            Console.WriteLine("Score : {0}", bestFriendPlayerChoice.Score);
            Console.WriteLine("Tour : {0}", turn);

            for (int i = 0; i < Boards.Length; i++)
            {
                Card friendPlayerCard = friendPlayer.ListCard[bestFriendPlayerChoice.ChoiceBoards[i].IdPlayerCard];
                Card oppositePlayerCard = oppositePlayer.ListCard[oppositePlayerChoice.ChoiceBoards[i].IdPlayerCard];

                Boards[i].PawnsFriendPlayer += bestFriendPlayerChoice.ChoiceBoards[i].Score;

                string winnerName = "";

                if (Boards[i].PawnsFriendPlayer < 0)
                    winnerName = "Op";
                else if (Boards[i].PawnsFriendPlayer > 0)
                    winnerName = "IA";


                Console.WriteLine("Planète {0} ({5}): Op = {1} ; IA = {2} ; Gagnants = {3} ; Score = {4}", i + 1, oppositePlayerCard.ToString(), friendPlayerCard.ToString(), winnerName, Boards[i].PawnsFriendPlayer, Boards[i].Scores[0]);
            }

            Console.ReadKey();
        }

        private void OppositePlayerTurn()
        {
            oppositePlayerChoice = new Choice();

            for (int i = 0; i < 5; i++)
            {
                oppositePlayerChoice.ChoiceBoards[i] = new ChoiceBoard();
                oppositePlayerChoice.ChoiceBoards[i].IdBoard = i;
                oppositePlayerChoice.ChoiceBoards[i].IdPlayerCard = oppositePlayer.ListIndexPickedCard[rnd.Next(0, oppositePlayer.ListIndexPickedCard.Count)];
                oppositePlayer.ListIndexPickedCard.Remove(oppositePlayerChoice.ChoiceBoards[i].IdPlayerCard);
            }
        }

        private void FriendPlayerTurn()
        {
            //Choices = new List<Choice>();
            bestFriendPlayerChoice = new Choice();

            Choice choice = new Choice();

            for (int i = 0; i < friendPlayer.ListIndexPickedCard.Count; i++)
            {
                choice.IdPlayerCards.Add(i);
            }

            PlayerTurnRecursively(choice, 0);
        }

        private void CalcScore(Choice choiceFriendPlayer)
        {
            for (int i = 0; i < 5; i++)
            {
                Card friendPlayerCard = friendPlayer.ListCard[choiceFriendPlayer.ChoiceBoards[i].IdPlayerCard];
                Card oppositePlayerCard = oppositePlayer.ListCard[oppositePlayerChoice.ChoiceBoards[i].IdPlayerCard];
                int value = 0;
                int f = friendPlayerCard.Value;
                int o = oppositePlayerCard.Value;

                if (friendPlayerCard.CardType == CardType.Value)
                {
                    if (oppositePlayerCard.CardType == CardType.Value)
                    {
                        value = f - o;
                    }
                    else if (oppositePlayerCard.CardType == CardType.PlusValue)
                    {
                        value = -f - o;

                    }
                    else if (oppositePlayerCard.CardType == CardType.MinusValue)
                    {
                        value = 1;
                    }
                }
                else if (friendPlayerCard.CardType == CardType.PlusValue)
                {
                    if (oppositePlayerCard.CardType == CardType.Value)
                    {
                        value = f + o;
                    }
                    else if (oppositePlayerCard.CardType == CardType.PlusValue)
                    {
                        value = f - o;
                    }
                    else if (oppositePlayerCard.CardType == CardType.MinusValue)
                    {
                        value = f;
                    }
                }
                else if (friendPlayerCard.CardType == CardType.MinusValue)
                {
                    if (oppositePlayerCard.CardType == CardType.Value)
                    {
                        value = -1;
                    }
                    else if (oppositePlayerCard.CardType == CardType.PlusValue)
                    {
                        value = -o;
                    }
                    else if (oppositePlayerCard.CardType == CardType.MinusValue)
                    {
                        value = 0;
                    }
                }

                choiceFriendPlayer.ChoiceBoards[i].Score = Boards[i].PawnsFriendPlayer + value;

                choiceFriendPlayer.ChoiceBoards[i].Score += Math.Sign(choiceFriendPlayer.ChoiceBoards[i].Score) * Boards[i].Scores[0] * 4;

                choiceFriendPlayer.Score += choiceFriendPlayer.ChoiceBoards[i].Score;
            }
        }


        private void PlayerTurnRecursively(Choice choice, int numBoard)
        {
            if (numBoard >= Boards.Length)
            {
                CalcScore(choice);

                if (choice.Score >= bestFriendPlayerChoice.Score)
                    bestFriendPlayerChoice = choice;

                return;
            }

            for (int i = 0; i < choice.IdPlayerCards.Count; i++)
            {
                Choice newChoice = choice.Clone();

                newChoice.SetPlayerChoice(Boards[numBoard], i);

                PlayerTurnRecursively(newChoice, numBoard + 1);
            }
        }

        private void InitCards(ref Player player)
        {
            for (int i = 0; i < 10; i++)
            {
                player.ListCard.Add(new Card() { CardType = CardType.Value, Value = i + 1, Index = i });

                if (i > 5)
                    player.ListIndexAvailableCard.Add(i);
            }

            player.ListCard.Add(new Card() { CardType = CardType.MinusValue, Value = -1, Index = 10 });
            player.ListCard.Add(new Card() { CardType = CardType.MinusValue, Value = -1, Index = 11 });
            player.ListCard.Add(new Card() { CardType = CardType.MinusValue, Value = -1, Index = 12 });
            player.ListCard.Add(new Card() { CardType = CardType.PlusValue, Value = 1, Index = 13 });
            player.ListCard.Add(new Card() { CardType = CardType.PlusValue, Value = 1, Index = 14 });
            player.ListCard.Add(new Card() { CardType = CardType.PlusValue, Value = 1, Index = 15 });
            player.ListCard.Add(new Card() { CardType = CardType.PlusValue, Value = 2, Index = 16 });
            player.ListCard.Add(new Card() { CardType = CardType.PlusValue, Value = 3, Index = 17 });

            player.ListIndexAvailableCard.Add(10);
            player.ListIndexAvailableCard.Add(11);
            player.ListIndexAvailableCard.Add(12);
            player.ListIndexAvailableCard.Add(13);
            player.ListIndexAvailableCard.Add(14);
            player.ListIndexAvailableCard.Add(15);
            player.ListIndexAvailableCard.Add(16);
            player.ListIndexAvailableCard.Add(17);
        }
    }
}
