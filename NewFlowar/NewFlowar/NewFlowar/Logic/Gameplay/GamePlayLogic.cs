using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewFlowar.Model;
using NewFlowar.Common;
using NewFlowar.Model.Minion;
using Microsoft.Xna.Framework;
using NewFlowar.Logic.AI;

namespace NewFlowar.Logic.GamePlay
{
    public class GamePlayLogic
    {
        public Map Map { get; set; }

        public GamePlayLogic()
        {
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
            for (int i = 0; i < 10; i++)
            {
                int indexCell = rnd.Next(Map.Cells.Count);

                MinionBase minion = null;

                int indexMinion = rnd.Next(2);

                if (indexMinion == 0)
                    minion = new Inspector(Map.Cells[indexCell]);
                else if (indexMinion == 1)
                    minion = new Phant(Map.Cells[indexCell]);

                Context.CurrentPlayer.Minions.Add(minion);
            }
        }

        private void InitializeMap()
        {
            Context.HeightMapRadius = 10f;

            Map = new Map(20, 40);
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
            Random rnd = new Random();

            //--- Calcul la position des minions
            for (int i = 0; i < Context.Players.Count; i++)
            {
                for (int j = 0; j < Context.Players[i].Minions.Count; j++)
                {
                    MinionBase minion = Context.Players[i].Minions[j];
                    minion.CurrentCell.ContainsMinion = false;

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

                            float percent = p - (float)((int)p);

                            minion.Location = Vector3.Lerp(Tools.GetVector3(prevCell.Location), Tools.GetVector3(nextCell.Location), percent);

                            //--- Détection de la cellule sur laquelle se trouve le minion
                            float? distance1 = Tools.RayIntersectCell(new Vector3(minion.Location.X, minion.Location.Y, 50f), new Vector3(0f, 0f, -1f), Map, prevCell);

                            if (distance1.HasValue)
                            {
                                minion.CurrentCell = prevCell;
                                minion.Location = new Vector3(minion.Location.X, minion.Location.Y, 50f - distance1.Value);
                            }
                            else
                            {
                                float? distance2 = Tools.RayIntersectCell(new Vector3(minion.Location.X, minion.Location.Y, 50f), new Vector3(0f, 0f, -1f), Map, nextCell);

                                if (distance2.HasValue)
                                {
                                    minion.CurrentCell = nextCell;
                                    minion.Location = new Vector3(minion.Location.X, minion.Location.Y, 50f - distance2.Value);
                                }
                            }
                            //---

                            //--- Calcul de l'orientation du minion
                            float angleNextCell = Tools.GetAngle(Vector2.UnitX, nextCell.Location - Tools.GetVector2(minion.Location));
                            float angle = 0f;

                            if (nextCell2 != null)
                            {
                                angle = Tools.GetAngle(Vector2.UnitX, nextCell2.Location - Tools.GetVector2(minion.Location));
                            }
                            else
                            {
                                percent = 0f;
                            }

                            minion.Angle = angleNextCell * (1f - percent) + angle * percent;
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
                        if (Map.Cells[indexCell].Height < 10f)
                        {
                            minion.Path = PathFinding.CalcPath(minion.CurrentCell, Map.Cells[indexCell], true, 10f);

                            minion.PathLength = (minion.Path.Count - 1) * Map.R;
                            minion.TraveledLength = 0f;
                        }
                    }

                    //--- Calcul de la position sur l'axe Z du minion (selon le terrain)
                    if (minion.Location.Z == 0)
                    {
                        float? distance = Tools.RayIntersectCell(new Vector3(minion.Location.X, minion.Location.Y, 50f), new Vector3(0f, 0f, -1f), Map, minion.CurrentCell);

                        if (distance.HasValue)
                        {
                            minion.Location = new Vector3(minion.Location.X, minion.Location.Y, 50f - distance.Value);
                        }

                        if (distance.HasValue)
                            minion.Location = new Vector3(minion.Location.X, minion.Location.Y, 50f - distance.Value);
                    }
                    //---

                    minion.CurrentCell.ContainsMinion = true;
                }
            }
            //---
        }
    }
}
