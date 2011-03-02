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

namespace TheGrid.Model.UI
{
    public class ListLibrary : UIComponent
    {
        public ListLibrary(UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, creationTime)
        {
            this.Modal = true;
            this.Alive = true;
            this.Visible = true;
            this.ListUIChildren = new List<UIComponent>();

            Vector2 sizeLibraryName = Render.FontText.MeasureString(new String(' ', 40)) + new Vector2(Ribbon.MARGE * 2, Ribbon.MARGE);

            Rec = new Rectangle((int)(Render.ScreenWidth / 2 - sizeLibraryName.X / 2), (int)(0.3f * Render.ScreenHeight), (int)sizeLibraryName.X, (int)(0.6f * Render.ScreenHeight));

            //--- Charge la liste des librairies
            String[] libraries = System.IO.Directory.GetDirectories(Path.Combine(Directory.GetParent(System.Windows.Forms.Application.ExecutablePath).FullName, @"Sound\Library"));
            Vector2 vec = new Vector2(Rec.X + Ribbon.MARGE, Rec.Y + Ribbon.MARGE);

            foreach (string library in libraries)
            {
                string libraryName = Path.GetFileName(library);

                if (libraryName == ".svn")
                    continue;

                ClickableText txtLibrary = new ClickableText(this.UI, creationTime,Render.FontText, libraryName.Substring(0, Math.Min(20, libraryName.Length)), vec, VisualStyle.ForeColor, VisualStyle.ForeColor, VisualStyle.BackColorLight, VisualStyle.BackForeColorMouseOver, false);
                txtLibrary.Rec = new Rectangle(txtLibrary.Rec.X, txtLibrary.Rec.Y, Rec.Width - 2 * Ribbon.MARGE, txtLibrary.Rec.Height);
                txtLibrary.Tag = libraryName;

                vec.Y += sizeLibraryName.Y;

                txtLibrary.ClickText += new ClickableText.ClickTextHandler(txtLibrary_ClickText);
                ListUIChildren.Add(txtLibrary);
            }
            //---

            //---
            KeyManager keyClose = AddKey(Keys.Escape);
            keyClose.KeyReleased += new KeyManager.KeyReleasedHandler(keyClose_KeyReleased);
            //---
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
