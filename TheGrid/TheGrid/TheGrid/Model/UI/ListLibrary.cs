using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using System.IO;
using TheGrid.Common;
using TheGrid.Logic.Controller;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TheGrid.Model.UI
{
    public class ListLibrary : ListBase
    {
        public ListLibrary(UILogic uiLogic, TimeSpan creationTime, Rectangle rec, SpriteFont font, bool checkable)
            : base(uiLogic, creationTime, rec, font, checkable)
        {
            this.Modal = true;
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();

            LoadLibraries();

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
        }

        private void LoadLibraries()
        {
            //--- Charge la liste des librairies
            String[] libraries = System.IO.Directory.GetDirectories(Path.Combine(Directory.GetParent(System.Windows.Forms.Application.ExecutablePath).FullName, @"Files\Sound\Library"));

            foreach (string library in libraries)
            {
                string libraryName = Path.GetFileName(library);

                if (libraryName == ".svn")
                    continue;

                ClickableText txtLibrary = AddItem(libraryName, library);

                txtLibrary.ClickText += new ClickableText.ClickTextHandler(txtLibrary_ClickText);
            }
            //---

            UpdateScrollbar();
            UpdateScrollValue();
        }

        void keyClose_KeyReleased(Keys key, GameTime gameTime)
        {
            this.Alive = false;
        }

        void txtLibrary_ClickText(ClickableText clickableText, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            GamePlay.NewMap(clickableText.Text);

            this.Alive = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Render.GraphicsDevice.Viewport.Bounds, VisualStyle.BackColorModalScreen);

            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, VisualStyle.BackColorLight);

            base.Draw(gameTime);
        }
    }
}
