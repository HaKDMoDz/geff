using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Common;

namespace TheGrid.Model.UI
{
    public class Partition : UIComponent
    {
        private Ribbon ribbon;

        public Partition(Ribbon ribbon, UILogic uiLogic)
            : base(uiLogic)
        {
            this.ribbon = ribbon;
            Visible = true;
            Alive = true;

            Rec = new Rectangle(Ribbon.MARGE, Ribbon.MARGE, (int)(0.7f * Render.ScreenWidth), ribbon.Rec.Height - Ribbon.MARGE * 2);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            float timelineDuration = 1000f * 10;

            Render.SpriteBatch.Draw(Render.texEmptyGradient, Rec, new Color(0.2f, 0.2f, 0.2f));

            float channelHeight = (float)Rec.Height / ((float)Context.Map.Channels.Count - 1);
            float channelWidth = Rec.Width - 100 - Ribbon.MARGE;

            for (int i = 0; i < Context.Map.Channels.Count; i++)
            {
                if (i == 0) continue;

                float fi = (float)(i - 1);

                float channelX = Rec.X + Ribbon.MARGE + 100;
                float channelY = (float)Rec.Y + channelHeight * fi;

                Rectangle recChannel = new Rectangle(
                    (int)channelX,
                    (int)channelY,
                    (int)channelWidth,
                    (int)channelHeight);

                //--- Nombre de musiciens en cours
                Render.SpriteBatch.DrawString(Render.FontMenu, Context.Map.Channels[i].ListMusician.Count(m => m.IsPlaying).ToString(), new Vector2(Rec.X + Ribbon.MARGE, channelY - Ribbon.MARGE), Color.White);
                //---

                //--- Mute / Solo par channel
                //TODO : afficher par des clickable image
                Render.SpriteBatch.Draw(Render.texSoloChannel, new Vector2(Rec.X + Ribbon.MARGE * 2 + Render.FontMenu.MeasureString("0").X, channelY - Ribbon.MARGE + Render.texSoloChannel.Height / 2), Color.White);
                Render.SpriteBatch.Draw(Render.texMuteChannel, new Vector2(Rec.X + Ribbon.MARGE * 3 + Render.FontMenu.MeasureString("0").X + Render.texSoloChannel.Width, channelY - Ribbon.MARGE + Render.texMuteChannel.Height / 2), Color.White);
                //---

                Render.SpriteBatch.Draw(Render.texEmptyGradient, recChannel, Context.Map.Channels[i].Color);

                if (Context.Map.Channels[i].ListMusician == null || Context.Map.Channels[i].ListMusician.Count == 0)
                    continue;

                float heightPerMusician = channelHeight / (float)Context.Map.Channels[i].ListMusician.Count;
                for (int j = 0; j < Context.Map.Channels[i].ListMusician.Count; j++)
                {
                    //--- Mute / Solo par musicien
                    //---

                    float fj = (float)j;

                    for (int k = 0; k < Context.Map.Channels[i].ListMusician[j].Partition.Count; k++)
                    {
                        //if (Context.Map.Channels[i].ListMusician[j].Partition[k].Instrument != null)
                        if (Context.Map.Channels[i].ListMusician[j].Partition[k].Value.Clip != null)
                        {
                            double totalMs = Context.Map.Channels[i].ListMusician[j].Partition[k].Time.Subtract(Context.Time).TotalMilliseconds;

                            if (totalMs > -500f && totalMs < timelineDuration)
                            {
                                float sizeClip = 1f;

                                if (totalMs < 0f)
                                    sizeClip = 1f + (float)totalMs / 500f;

                                Render.SpriteBatch.Draw(Render.texEmpty, new Rectangle(
                                    (int)(channelX + ((1f - sizeClip) * 500f + totalMs) / timelineDuration * channelWidth),
                                    (int)(channelY + heightPerMusician * fj),
                                    (int)(sizeClip * 500f / timelineDuration * channelWidth),
                                    (int)(heightPerMusician)), Color.Black);
                            }
                        }
                    }
                }
            }
        }
    }
}
