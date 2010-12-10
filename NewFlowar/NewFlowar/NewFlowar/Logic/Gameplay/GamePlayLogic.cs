using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Model;
using NewFlowar.Common;
using NewFlowar.Model.Minion;
using Microsoft.Xna.Framework;
using NewFlowar.Logic.AI;
using Algorithms;

namespace NewFlowar.Logic.GamePlay
{
    public class GamePlayLogic
    {
        private Random rnd = new Random();

        public Map Map { get; set; }
        public GameEngine GameEngine { get; set; }

        public GamePlayLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
            InitializeMap();
            InitializePlayers();
        }

        private void InitializePlayers()
        {
            Context.Players = new List<Player>();
            Context.Players.Add(new Player() { PlayerName = "Geff" });
            Context.CurrentPlayer = Context.Players[0];

            Random rnd = new Random();
            Context.CurrentPlayer.Minions = new List<MinionBase>();
            for (int i = 0; i < 50; i++)
            {
                int indexCell = rnd.Next(Map.Cells.Count);

                MinionBase minion = null;

                int indexMinion = rnd.Next(2);

                if (indexMinion == 0)
                    minion = new Inspector(Map.Cells[indexCell]);
                //else if (indexMinion == 1)
                //    minion = new Phant(Map.Cells[indexCell]);
                //else if (indexMinion == 1)
                //    minion = new Robot(Map.Cells[indexCell]);
                else if (indexMinion == 1)
                    minion = new Robot2(Map.Cells[indexCell]);

                Context.CurrentPlayer.Minions.Add(minion);
            }
        }

        private void InitializeMap()
        {
            Context.HeightMapRadius = 10f;

            Map = new Map(32,32);
            Map.CreateGrid();

            Map.Cells[Map.Cells.Count / 2].Height = Map.R * 10;
            Map.ElevateCell(Map.Cells[Map.Cells.Count / 2], 80f);

            Map.Cells[Map.Cells.Count / 3].Height = -Map.R * 15;
            Map.ElevateCell(Map.Cells[Map.Cells.Count / 2], 50f);

            Map.Cells[Map.Cells.Count / 3].Height = -Map.R * 15;
            Map.ElevateCell(Map.Cells[Map.Cells.Count / 3], 50f);

            Map.CalcHeightPoint();
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);

            //--- Calcul la position des minions
            for (int i = 0; i < Context.Players.Count; i++)
            {
                for (int j = 0; j < Context.Players[i].Minions.Count; j++)
                {
                    MinionBase minion = Context.Players[i].Minions[j];

                    if (minion.Path.Count > 0)
                    {
                        minion.TraveledLength += minion.Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds * (float)Map.R * 0.001f;

                        float p = minion.TraveledLength / (float)Map.R;

                        int indexPrevCell = (int)p;

                        if (indexPrevCell + 1 <= minion.Path.Count - 1)
                        {
                            Cell prevCell = Map.Cells[minion.Path[indexPrevCell]];
                            Cell nextCell = Map.Cells[minion.Path[indexPrevCell + 1]];
                            Cell nextCell2 = null;

                            if (indexPrevCell + 2 <= minion.Path.Count - 1)
                                nextCell2 = Map.Cells[minion.Path[indexPrevCell + 2]];

                            float percent = p - (float)(indexPrevCell);

                            minion.Location = Vector3.Lerp(Tools.GetVector3(prevCell.Location), Tools.GetVector3(nextCell.Location), percent);

                            //--- Détection de la cellule sur laquelle se trouve le minion
                            float? distance1 = Tools.RayIntersectCell(new Vector3(minion.Location.X, minion.Location.Y, 50f), -Vector3.UnitZ, Map, prevCell);

                            if (distance1.HasValue)
                            {
                                minion.CurrentCell = prevCell;
                                minion.Location = new Vector3(minion.Location.X, minion.Location.Y, 50f - distance1.Value);
                            }
                            else
                            {
                                float? distance2 = Tools.RayIntersectCell(new Vector3(minion.Location.X, minion.Location.Y, 50f), -Vector3.UnitZ, Map, nextCell);

                                if (distance2.HasValue)
                                {
                                    minion.CurrentCell = nextCell;
                                    minion.Location = new Vector3(minion.Location.X, minion.Location.Y, 50f - distance2.Value);
                                }
                            }
                            //---

                            //--- Calcul de l'orientation du minion
                            float angleCellToNextCell = Tools.GetAngle(Vector2.UnitX, nextCell.Location - prevCell.Location);
                            float angleNextCellToNextCell2 = 0f;

                            if (nextCell2 != null)
                            {
                                angleNextCellToNextCell2 = Tools.GetAngle(Vector2.UnitX, nextCell2.Location - nextCell.Location);
                            }
                            else
                            {
                                //percent = 1f;
                            }

                            Quaternion qPrevCell = Quaternion.CreateFromAxisAngle(prevCell.Normal, angleCellToNextCell);
                            Quaternion qNextCell = Quaternion.CreateFromAxisAngle(nextCell.Normal, angleNextCellToNextCell2);

                            Quaternion qRotation = Quaternion.Lerp(qPrevCell, qNextCell, percent);

                            minion.MatrixRotation = Matrix.CreateFromQuaternion(qRotation);
                            //---
                        }
                        else
                        {
                            minion.Path = new List<int>();
                            minion.TraveledLength = 0f;
                            minion.PathLength = 0f;
                        }
                    }
                    else
                    {
                        int indexCell = rnd.Next(Map.Cells.Count);

                        CalcMinionNewPath(minion, Map.Cells[indexCell]);
                    }
                }
            }
            //---
        }

        public void CalcMinionNewPath(MinionBase minion, Cell cell)
        {
            if (false)
            {
                minion.Path = PathFinding.CalcPath(minion.CurrentCell, cell, true, 10f);
                minion.PathLength = (minion.Path.Count - 1) * Map.R;
                minion.TraveledLength = 0f;
            }
            else
            {
                IPathFinder mPathFinder = new PathFinderFast(Map.Matrix);
                //IPathFinder mPathFinder = new PathFinder(Map.Matrix);

                mPathFinder.Formula = HeuristicFormula.Manhattan;
                mPathFinder.Diagonals = false;
                mPathFinder.HeavyDiagonals = false;
                mPathFinder.HeuristicEstimate = 1;
                mPathFinder.PunishChangeDirection = false;
                mPathFinder.TieBreaker = false;
                mPathFinder.SearchLimit = 5000;
                mPathFinder.DebugProgress = true;
                mPathFinder.DebugFoundPath = true;

                List<PathFinderNode> path = mPathFinder.FindPath(new System.Drawing.Point(minion.CurrentCell.Coord.X - 1, minion.CurrentCell.Coord.Y - 1), new System.Drawing.Point(cell.Coord.X - 1, cell.Coord.Y - 1));

                minion.Path = new List<int>();
                if (path != null)
                {
                    path.Reverse();

                    for (int i = 0; i < path.Count; i++)
                    {
                        minion.Path.Add(path[i].X + path[i].Y * Map.Width);
                    }
                }

                minion.PathLength = (minion.Path.Count - 1) * Map.R;
                minion.TraveledLength = 0f;
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            foreach (Player player in Context.Players)
            {
                foreach (MinionBase minion in player.Minions)
                {
                    if(minion.AnimationPlayer != null)
                        minion.AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                }
            }
        }
    }
}
