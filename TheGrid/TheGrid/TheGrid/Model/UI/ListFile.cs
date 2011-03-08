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
    public class ListFile : ListBase
    {
        public string Directory;

        public ListFile(UILogic uiLogic, TimeSpan creationTime, string directory, Rectangle rec, SpriteFont font)
            : base(uiLogic, creationTime, rec, font, false)
        {
            this.Modal = true;
            this.Alive = true;
            this.Visible = true;
            this.Directory = directory;
            this.ListUIChildren = new List<UIComponent>();

            LoadFile();

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
        }

        private void LoadFile()
        {
            //--- Charge la liste des fichiers
            String[] files = System.IO.Directory.GetFiles(Directory);

            foreach (string file in files)
            {
                string newFile = Path.GetFileNameWithoutExtension(file);

                ClickableText txtItem = AddItem(newFile, file);

                txtItem.ClickText += new ClickableText.ClickTextHandler(txtItem_ClickText);
            }

            UpdateScrollbar();
            UpdateScrollValue();
            //---
        }

        void keyClose_KeyReleased(Keys key, GameTime gameTime)
        {
            this.Alive = false;
        }

        void txtItem_ClickText(ClickableText clickableText, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            GamePlay.LoadMap(Path.GetFileNameWithoutExtension((string)clickableText.Tag));

            this.Alive = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmpty, Render.GraphicsDevice.Viewport.Bounds, VisualStyle.BackColorModalScreen);

            Render.SpriteBatch.Draw(Render.texEmpty, Rec, VisualStyle.BackColorLight);

            base.Draw(gameTime);
        }
    }
}
