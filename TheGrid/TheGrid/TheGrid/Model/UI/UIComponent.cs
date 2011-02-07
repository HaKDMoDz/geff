using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using TheGrid.Logic.Render;
using System.ComponentModel;
using TheGrid.Logic.Controller;

namespace TheGrid.Model.UI
{
    public abstract class UIComponent
    {
        public UILogic UI { get; set; }
        public RenderLogic Render { get { return UI.GameEngine.Render; } }
        public ControllerLogic Controller { get { return UI.GameEngine.Controller; } }

        public bool Visible { get; set; }
        public bool Alive { get; set; }
        public List<UIComponent> ListUIChildren { get; set; }
        public UIComponent UIDependency { get; set; }

        public virtual Rectangle Rec {get ;set;}

        public virtual void UpdateUIDependency(GameTime gameTime){}

        public virtual void Update(GameTime gameTime)
        {
            if (Alive && Visible && ListUIChildren != null)
            {
                foreach (UIComponent uiComponent in ListUIChildren)
                {
                    uiComponent.Update(gameTime);
                }
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Alive && Visible && ListUIChildren != null)
            {
                foreach (UIComponent uiComponent in ListUIChildren)
                {
                    uiComponent.Draw(gameTime);
                }
            }
        }

        public UIComponent(UILogic uiLogic)
        {
            this.Alive = true;
            this.Visible = false;
            this.UI = uiLogic;
        }
    }
}
