using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISZ
{
    class Program
    {
        static Game game;

        static void Main(string[] args)
        {
            List<Game> gameWonByOp = new List<Game>();

            int winFriend = 0;
            int winOp = 0;

            //Parallel.For(0,100, i =>
            for(int i =0; i < 1000; i++)
            {
                game = new Game();
                game.Init();

                while (!game.IsFinished)
                {
                    game.NextTurn();
                    //game.PrintScore();
                }

                if (game.friendPlayer.Score > game.oppositePlayer.Score)
                    winFriend++;
                else if (game.friendPlayer.Score < game.oppositePlayer.Score)
                {
                    winOp++;
                    gameWonByOp.Add(game);
                }
            };

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

        public override string ToString()
        {
            return String.Format("ID : {0} ; Score : {1}", IdPlayerCard, Score);
        }
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
        public Choice Choice { get; set; }
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

        public void DisposeCards()
        {
            ListIndexPickedCard = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                ListIndexPickedCard.Add(i);
            }
        }
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
        public Player friendPlayer;
        public Player oppositePlayer;
        public List<Choice[]> Choices;

        private Board[] Boards;

        private Random rnd = new Random();

        public void Init()
        {
            rnd = new Random();

            friendPlayer = new Player();
            oppositePlayer = new Player();
            Choices = new List<Choice[]>();

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


            //---Test valeur par défaut
            //Boards[0].Scores[0] = 3;
            //Boards[1].Scores[0] = 1;
            //Boards[2].Scores[0] = 3;
            //Boards[3].Scores[0] = 1;
            //Boards[4].Scores[0] = 3;
            //---

            InitCards(ref friendPlayer);
            InitCards(ref oppositePlayer);
        }

        public void NextTurn()
        {
            turn++;

            friendPlayer.DisposeCards();
            oppositePlayer.DisposeCards();

            if (turn == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    friendPlayer.PickCard();
                    oppositePlayer.PickCard();
                }

                //--- Test, valeur par défaut pour l'opposant
                //oppositePlayer.ListIndexAvailableCard.Add(11); // -1
                //oppositePlayer.ListIndexAvailableCard.Add(15); // +1
                //oppositePlayer.ListIndexAvailableCard.Add(16); // +2
                //oppositePlayer.ListIndexAvailableCard.Add(17); // +3
                //---
            }
            else
            {
                friendPlayer.PickCard();
                oppositePlayer.PickCard();
            }

            OppositePlayerTurn();
            FriendPlayerTurn();

            RevealCards();

            EvalScore();

            Choices.Add(new Choice[]{oppositePlayer.Choice, friendPlayer.Choice});
        }

        private void EvalScore()
        {
            if (turn % 3 == 0)
            {
                for (int i = 0; i < Boards.Length; i++)
                {
                    if (Boards[i].PawnsFriendPlayer > 0)
                        friendPlayer.Score += Boards[i].Scores[(turn - 1) / 3];
                    else if (Boards[i].PawnsFriendPlayer < 0)
                        oppositePlayer.Score += Boards[i].Scores[(turn - 1) / 3];
                }
            }

            if (friendPlayer.Score >= 9 && oppositePlayer.Score < 9)
                Winner = friendPlayer;
            if (oppositePlayer.Score >= 9 && friendPlayer.Score < 9)
                Winner = oppositePlayer;
        }

        private void RevealCards()
        {
            //--- Bouge les pions
            for (int i = 0; i < Boards.Length; i++)
            {
                //---> Si le joueur amis a un meilleur choix pour ce plateau
                if (friendPlayer.Choice.ChoiceBoards[i].Score > 0)
                {
                    SwapPawns(friendPlayer, i);
                }
                else if (friendPlayer.Choice.ChoiceBoards[i].Score < 0)
                {
                    SwapPawns(oppositePlayer, i);
                }
            }
        }

        private void SwapPawns(Player player, int i)
        {
            int sign = 1;
            Player otherPlayer = oppositePlayer;
            if (player == oppositePlayer)
            {
                sign = -1;
                otherPlayer = friendPlayer;
            }

            //---> Si le plateau était remporté par l'autre joueur
            if (sign * Boards[i].PawnsFriendPlayer < 0)
            {
                //---> Si l'autre joueur garde l'avantage
                if (sign * (Boards[i].PawnsFriendPlayer + friendPlayer.Choice.ChoiceBoards[i].Score) <= 0)
                {
                    otherPlayer.FreePawns += sign * friendPlayer.Choice.ChoiceBoards[i].Score;
                }
                else
                {
                    otherPlayer.FreePawns += Math.Abs(Boards[i].PawnsFriendPlayer);
                }
            }

            //---> Assez de pions en réserve
            if (player.FreePawns - sign * friendPlayer.Choice.ChoiceBoards[i].Score >= 0)
            {
                player.FreePawns -= sign * friendPlayer.Choice.ChoiceBoards[i].Score;
            }
            else
            {
                int rest = sign * friendPlayer.Choice.ChoiceBoards[i].Score - player.FreePawns;
                player.FreePawns = 0;

                int orientation = -1;

                Dictionary<int, int> dicSens = new Dictionary<int, int>();
                dicSens.Add(-1, 0);
                dicSens.Add(1, 0);
                if (i > 0)
                    dicSens[-1] = -1;
                if (i < 4)
                    dicSens[1] = 1;
                while (rest > 0)
                {
                    orientation *= -1;
                    int sens = dicSens[orientation];


                    if (sens != 0 && sign * Boards[i + sens].PawnsFriendPlayer > 1)
                    {
                        Boards[i + sens].PawnsFriendPlayer -= sign;
                        rest--;
                    }
                    else if (sens != 0)
                    {
                        sens += orientation;

                        dicSens[orientation] = sens;

                        if (i + sens < 0 || i + sens > 4)
                            dicSens[orientation] = 0;
                    }

                    if (dicSens[-1] == 0 && dicSens[1] == 0)
                        break;
                }
            }

            Boards[i].PawnsFriendPlayer += friendPlayer.Choice.ChoiceBoards[i].Score;
        }

        public void PrintScore()
        {
            if (friendPlayer.Choice.Score == 0)
                return;

            Console.WriteLine(new String('_', 80));
            Console.WriteLine("Score : {0}", friendPlayer.Choice.Score);
            Console.WriteLine("Tour : {0}", turn);

            Console.WriteLine("Réserve Op : {0}", oppositePlayer.FreePawns);
            Console.WriteLine("Réserve IA : {0}", friendPlayer.FreePawns);

            Console.WriteLine("Score Op : {0}", oppositePlayer.Score);
            Console.WriteLine("Score IA : {0}", friendPlayer.Score);

            for (int i = 0; i < Boards.Length; i++)
            {
                Card friendPlayerCard = friendPlayer.ListCard[friendPlayer.Choice.ChoiceBoards[i].IdPlayerCard];
                Card oppositePlayerCard = oppositePlayer.ListCard[oppositePlayer.Choice.ChoiceBoards[i].IdPlayerCard];

                string winnerName = "";

                if (Boards[i].PawnsFriendPlayer < 0)
                    winnerName = "Op";
                else if (Boards[i].PawnsFriendPlayer > 0)
                    winnerName = "IA";

                Console.WriteLine("P {0} ({5}): Op= {1,2}  IA= {2,2}  Gagne= {3,2}  Score= {6,2}  fE= {4,3}  Pions= {7,2}", i + 1, oppositePlayerCard.ToString(), friendPlayerCard.ToString(), winnerName, Boards[i].PawnsFriendPlayer, Boards[i].Scores[0], friendPlayer.Choice.ChoiceBoards[i].Score, Boards[i].PawnsFriendPlayer);
            }

            Console.ReadKey();
        }

        private void OppositePlayerTurn()
        {
            oppositePlayer.Choice = new Choice();

            for (int i = 0; i < 5; i++)
            {
                oppositePlayer.Choice.ChoiceBoards[i] = new ChoiceBoard();
                oppositePlayer.Choice.ChoiceBoards[i].IdBoard = i;

                //--- En commentaire pour le test avec des valeurs par défaut
                oppositePlayer.Choice.ChoiceBoards[i].IdPlayerCard = oppositePlayer.ListIndexPickedCard[rnd.Next(0, oppositePlayer.ListIndexPickedCard.Count)];
                oppositePlayer.ListIndexPickedCard.Remove(oppositePlayer.Choice.ChoiceBoards[i].IdPlayerCard);
                //---
            }

            //--- Test, valeur par défaut
            //oppositePlayer.Choice.ChoiceBoards[0].IdPlayerCard = 13;
            //oppositePlayer.Choice.ChoiceBoards[1].IdPlayerCard = 4;
            //oppositePlayer.Choice.ChoiceBoards[2].IdPlayerCard = 10;
            //oppositePlayer.Choice.ChoiceBoards[3].IdPlayerCard = 2;
            //oppositePlayer.Choice.ChoiceBoards[4].IdPlayerCard = 1;

            //oppositePlayer.ListIndexPickedCard.Remove(12);
            //oppositePlayer.ListIndexPickedCard.Remove(4);
            //oppositePlayer.ListIndexPickedCard.Remove(10);
            //oppositePlayer.ListIndexPickedCard.Remove(2);
            //oppositePlayer.ListIndexPickedCard.Remove(1);
            //---
        }

        private void FriendPlayerTurn()
        {
            friendPlayer.Choice = null;

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
                Card oppositePlayerCard = oppositePlayer.ListCard[oppositePlayer.Choice.ChoiceBoards[i].IdPlayerCard];
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

                choiceFriendPlayer.ChoiceBoards[i].Score = value;

                choiceFriendPlayer.Score += Boards[i].PawnsFriendPlayer + (choiceFriendPlayer.ChoiceBoards[i].Score + Math.Sign(choiceFriendPlayer.ChoiceBoards[i].Score)) * Boards[i].Scores[(turn - 1) / 3] * 4;
            }
        }


        private void PlayerTurnRecursively(Choice choice, int numBoard)
        {
            if (numBoard >= Boards.Length)
            {
                CalcScore(choice);

                if (friendPlayer.Choice == null || choice.Score >= friendPlayer.Choice.Score)
                    friendPlayer.Choice = choice;

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
