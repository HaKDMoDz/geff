using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using TheGrid.Common;
using Microsoft.Xna.Framework;

namespace TheGrid.Logic.GamePlay
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
        }

        private void InitializeMap()
        {
            Map = new Map(32,32);
            Map.CreateGrid();

            //Map.CalcHeightPoint();
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
        }
    }
}
