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

            Rec = new Rectangle(Ribbon.MARGE, Ribbon.MARGE, (int)(0.7f * Render.ScreenWidth), ribbon.Rec.Height - Ribbon.MARGE);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            float timelineDuration = 1000f * 10;

            Render.SpriteBatch.Draw(Render.texEmpty, Rec, Color.DarkGray);

            float channelHeight = 0.2f * Render.ScreenHeight / (float)Context.Map.Channels.Count;
            float channelWidth = (0.8f * Render.ScreenWidth - 0.02f * Render.ScreenWidth);

            for (int i = 0; i < Context.Map.Channels.Count; i++)
            {
                float fi = (float)i;

                float channelX = 0.1f * Render.ScreenWidth + 0.01f * Render.ScreenWidth;
                float channelY = 0.05f * Render.ScreenHeight + channelHeight * fi + 0.2f * channelHeight;

                Rectangle recChannel = new Rectangle(
                    (int)channelX,
                    (int)channelY,
                    (int)channelWidth,
                    (int)(channelHeight - 0.2f * channelHeight));

                //--- Nombre de musiciens en cours
                //Render.SpriteBatch.DrawString
                //---

                //--- Mute / Solo par channel
                //---

                Render.SpriteBatch.Draw(Render.texEmpty, recChannel, Context.Map.Channels[i].Color);

                if (Context.Map.Channels[i].ListMusician == null || Context.Map.Channels[i].ListMusician.Count == 0)
                    continue;

                float heightPerMusician = (channelHeight - 0.2f * channelHeight) / (float)Context.Map.Channels[i].ListMusician.Count;
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
