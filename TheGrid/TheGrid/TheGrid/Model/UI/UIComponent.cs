using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using TheGrid.Logic.Render;
using System.ComponentModel;
using TheGrid.Logic.Controller;
using TheGrid.Logic.GamePlay;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TheGrid.Model.UI
{
    public abstract class UIComponent
    {
        public UILogic UI { get; set; }
        public RenderLogic Render { get { return UI.GameEngine.Render; } }
        public ControllerLogic Controller { get { return UI.GameEngine.Controller; } }
        public GamePlayLogic GamePlay { get { return UI.GameEngine.GamePlay; } }

        public TimeSpan CreationTime { get; set; }
        public bool Visible { get; set; }
        public bool Alive { get; set; }
        public List<UIComponent> ListUIChildren { get; set; }
        public UIComponent UIDependency { get; set; }
        public Object Tag { get; set; }
        public bool Modal { get; set; }
        protected int countChildren;
        public virtual Rectangle Rec { get; set; }
        public bool MouseHandled { get; set; }

        public virtual void UpdateUIDependency(GameTime gameTime) { }

        private List<KeyManager> ListKeyManager = new List<KeyManager>();
        private List<MouseManager> ListMouseManager = new List<MouseManager>();

        protected KeyManager AddKey(Keys keys)
        {
            KeyManager keyManager = new KeyManager(keys);
            ListKeyManager.Add(keyManager);

            return keyManager;
        }

        protected MouseManager AddMouse(MouseButtons mouseButton)
        {
            MouseManager mouseManager = new MouseManager(mouseButton);
            ListMouseManager.Add(mouseManager);

            return mouseManager;
        }

        public virtual void Update(GameTime gameTime)
        {
            MouseHandled = false;

            if (Alive && Visible && ListUIChildren != null)
            {
                ListUIChildren.Sort((x, y) => y.CreationTime.CompareTo(x.CreationTime));

                for (int i = 0; i < ListUIChildren.Count; i++)
                {
                    UIComponent uiComponent = ListUIChildren[i];

                    if (uiComponent.Alive && uiComponent.Visible)
                        uiComponent.Update(gameTime);
                }
            }

            foreach (KeyManager keyManager in ListKeyManager)
            {
                keyManager.Update(Controller.keyBoardState, gameTime);
            }

            foreach (MouseManager mouseManager in ListMouseManager)
            {
                mouseManager.Update(Controller.mouseState, gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Alive && Visible && ListUIChildren != null)
            {
                ListUIChildren.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));

                foreach (UIComponent uiComponent in ListUIChildren)
                {
                    if (uiComponent.Alive && uiComponent.Visible)
                        uiComponent.Draw(gameTime);
                }
            }
        }

        public UIComponent(UILogic uiLogic, TimeSpan creationTime)
        {
            this.CreationTime = creationTime;
            this.Alive = true;
            this.Visible = false;
            this.UI = uiLogic;
        }

        protected TimeSpan GetNewTimeSpan()
        {
            countChildren++;

            return CreationTime.Add(TimeSpan.FromSeconds(countChildren));
        }

        public bool IsUIModalActive()
        {
            bool isUIModalActive = Modal;

            if (Alive && Visible && ListUIChildren != null)
            {
                foreach (UIComponent ui in ListUIChildren)
                {
                    if (ui.Alive && ui.Visible)
                    {
                        isUIModalActive |= ui.IsUIModalActive();
                    }
                }
            }

            return isUIModalActive;
        }

        public bool IsMouseHandled()
        {
            bool isMouseHandled = false;

            if (Alive && Visible)
            {
                isMouseHandled = MouseHandled;

                if (ListUIChildren != null)
                {
                    foreach (UIComponent ui in ListUIChildren)
                    {
                        if (ui.Alive && ui.Visible)
                        {
                            isMouseHandled |= ui.IsMouseHandled();
                        }
                    }
                }
            }

            return isMouseHandled;
        }

        public Texture2D GetIcon(string iconName)
        {
            return UI.GameEngine.Content.Load<Texture2D>(@"Texture\Icon\" + iconName);
        }

        public Texture2D GetImage(string imagePath)
        {
            return UI.GameEngine.Content.Load<Texture2D>(@"Texture\" + imagePath);
        }
    }
}
