using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework;
using TheGrid.Logic.Controller;
using TheGrid.Common;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.UI
{
    public class BPMMeter : UIComponent
    {
        private float _prevSpeedFactor = 0f;
        private Ribbon _ribbon;

        public BPMMeter(Ribbon ribbon, UILogic uiLogic, TimeSpan creationTime) : base(uiLogic, ribbon, creationTime)
        {
            Alive = true;
            Visible = true;
            _ribbon = ribbon;

            MouseManager mouseLeftButton = AddMouse(MouseButtons.LeftButton);
            mouseLeftButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseLeftButton_MouseFirstPressed);
            mouseLeftButton.MousePressed += new MouseManager.MousePressedHandler(mouseLeftButton_MousePressed);
            mouseLeftButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseLeftButton_MouseReleased);
        }

        public void Init()
        {
            Rec = new Rectangle(_ribbon.RecMenuBar.Left + 200, _ribbon.RecMenuBar.Top + Ribbon.MARGE, 240, _ribbon.RecMenuBar.Height - Ribbon.MARGE * 2);
        }

        void mouseLeftButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            _prevSpeedFactor = 0f;
        }

        void mouseLeftButton_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            if (Rec.Contains(UI.GameEngine.Controller.mousePositionPoint))
            {
                _prevSpeedFactor = Context.Map.SpeedFactor;
            }
        }

        void mouseLeftButton_MousePressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (_prevSpeedFactor > 0f)
            {
                Context.Map.SpeedFactor = ((float)mouseState.X - (float)Rec.Left) / (float)Rec.Width * 2f;
                _ribbon.Partition.InitSegment();
                UI.GameEngine.GamePlay.EvaluateMuscianGrid(); 
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Rectangle recManualSpeed = new Rectangle(Rec.Left, Rec.Top, (int)((float)Rec.Width * Context.Map.SpeedFactor / 2f), Rec.Height);

            Render.SpriteBatch.Draw(Render.texEmpty, Rec, VisualStyle.BackColorLight);
            Render.SpriteBatch.Draw(Render.texEmpty, recManualSpeed, VisualStyle.BackForeColorMouseOver);
        }
    }
}
