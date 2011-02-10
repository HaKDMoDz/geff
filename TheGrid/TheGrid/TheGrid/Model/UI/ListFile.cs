using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using System.IO;
using TheGrid.Common;

namespace TheGrid.Model.UI
{
    public class ListFile : UIComponent
    {
        public string Directory;

        public ListFile(UILogic uiLogic, TimeSpan creationTime, string directory) : base(uiLogic, creationTime)
        {
            this.Alive = true;
            this.Visible = true;
            this.Directory = directory;
            this.ListUIChildren = new List<UIComponent>();

            Vector2 sizeFileName = Render.FontText.MeasureString(new String(' ',40)) + new Vector2(Ribbon.MARGE*2, Ribbon.MARGE*2);

            Rec = new Rectangle((int)(Render.ScreenWidth/2-sizeFileName.X/2), (int)(0.3f * Render.ScreenHeight), (int)sizeFileName.X, (int)(0.6f*Render.ScreenHeight));

            //--- Charge la liste des fichiers
            String[] files = System.IO.Directory.GetFiles(directory);
            Vector2 vec = new Vector2(Rec.X + Ribbon.MARGE, Rec.Y + Ribbon.MARGE);

            foreach (string file in files)
            {
                string newFile = Path.GetFileNameWithoutExtension(file);
                //ListFile.Add(newFile);

                ClickableText txtFile = new ClickableText(this.UI, creationTime, "FontText", newFile.Substring(0, Math.Min(20, newFile.Length)), vec, Color.White, Color.LightBlue);
                txtFile.Tag = file;
                vec.Y += sizeFileName.Y + Ribbon.MARGE;

                txtFile.ClickText += new ClickableText.ClickTextHandler(txtFile_ClickText);
                ListUIChildren.Add(txtFile);
            }
            //---

            //---
            //---
        }

        void txtFile_ClickText(ClickableText clickableText, Microsoft.Xna.Framework.Input.MouseState mouseState, GameTime gameTime)
        {
            Context.Map = FileSystem.LoadLevel(Path.GetFileNameWithoutExtension((string)clickableText.Tag));
            GamePlay.EvaluateMuscianGrid(TimeSpan.Zero);
            GamePlay.Stop();

            this.Alive = false;
        }

        public override void Draw(GameTime gameTime)
        {
            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, new Color(0.2f, 0.2f, 0.2f, 0.95f));

            base.Draw(gameTime);
        }
    }
}
