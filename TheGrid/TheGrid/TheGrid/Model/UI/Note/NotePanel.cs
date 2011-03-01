using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Logic.UI;
using TheGrid.Model.UI.Menu;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using TheGrid.Logic.Controller;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.UI.Note
{
    public class NotePanel : UIComponent
    {
        public ComponentState State { get; set; }
        public float leftPartWidth = 0f;
        public Sample Sample { get; set; }

        public NotePanel(UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, creationTime)
        {
            Alive = true;
            Visible = true;

            leftPartWidth = 0.3f * UI.GameEngine.Render.ScreenWidth;
            Rec = new Rectangle(0, (int)(0.4f * UI.GameEngine.Render.ScreenHeight), (int)UI.GameEngine.Render.ScreenWidth, (int)(0.6f * UI.GameEngine.Render.ScreenHeight));

            Keyboard keyboard = new Keyboard(this, UI, GetNewTimeSpan());
            ListUIChildren = new List<UIComponent>();
            ListUIChildren.Add(keyboard);

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
        }

        void keyClose_KeyReleased(Keys key, GameTime gameTime)
        {
            this.Alive = false;
        }

        public override void Update(GameTime gametime)
        {
            //if (Enter != null && Leave != null && !UI.GameEngine.Controller.IsMouseOffScreen())
            //{
            //    int countItemSelected = Items.Count(i => i.MouseOver);

            //    if (prevCountItemSelected == 0 && countItemSelected > 0)
            //        Enter(gametime);
            //    if (prevCountItemSelected > 0 && countItemSelected == 0)
            //        Leave(gametime);

            //    prevCountItemSelected = countItemSelected;
            //}

            //double elapsedTimeMenu = gametime.TotalGameTime.Subtract(LastStateChanged).TotalMilliseconds;

            //if (State == ComponentState.Opening && elapsedTimeMenu > 0)
            //    PercentVisibility = elapsedTimeMenu / MENU_ANIMATION_DURATION;

            //if (State == ComponentState.Closing)
            //    PercentVisibility = 1 - elapsedTimeMenu / MENU_ANIMATION_DURATION;

            //if (PercentVisibility > 1)
            //    PercentVisibility = 1;
            //if (PercentVisibility < 0)
            //    PercentVisibility = 0;

            //if (State == ComponentState.Opening || State == ComponentState.Closing || State == ComponentState.Opened)
            //    CreateVertex();

            //if (PercentVisibility == 1 && State == ComponentState.Opening)
            //    State = ComponentState.Opened;
            //if (PercentVisibility == 0 && State == ComponentState.Closing)
            //{
            //    State = ComponentState.Closed;

            //    if (!IsTurnMode)
            //        Visible = false;
            //}

            //if (Alive && Visible)
            //{
            //    MouseOver(gametime);
            //}

            base.Update(gametime);
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Rec, VisualStyle.BackColorDark);

            base.Draw(gameTime);
        }
    }
}
