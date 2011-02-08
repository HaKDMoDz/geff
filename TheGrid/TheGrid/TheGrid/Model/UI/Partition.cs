using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TheGrid.Logic.UI;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Common;
using Microsoft.Xna.Framework.Input;

namespace TheGrid.Model.UI
{
    public class Partition : UIComponent
    {
        private Ribbon ribbon;

        public Partition(Ribbon ribbon, UILogic uiLogic, TimeSpan creationTime)
            : base(uiLogic, creationTime)
        {
            this.ribbon = ribbon;
            Visible = true;
            Alive = true;

            Rec = new Rectangle(Ribbon.MARGE, Ribbon.MARGE, (int)(0.7f * Render.ScreenWidth), ribbon.Rec.Height - Ribbon.MARGE * 2);
        }

        public void Init(TimeSpan creationTime)
        {
            ListUIChildren = new List<UIComponent>();

            float channelHeight = (float)Rec.Height / ((float)Context.Map.Channels.Count - 1);
            float channelWidth = Rec.Width - 100 - Ribbon.MARGE;

            for (int i = 1; i < Context.Map.Channels.Count; i++)
            {
                float fi = (float)(i - 1);

                float channelX = Rec.X + Ribbon.MARGE + 100;
                float channelY = (float)Rec.Y + channelHeight * fi;

                ClickableImage imgMuteChannel = new ClickableImage(UI, creationTime, "MuteChannel" + i.ToString(), Render.texMuteChannel, Render.texMuteChannel, new Vector2(Rec.X + Ribbon.MARGE * 2 + Render.FontMenu.MeasureString("0").X, channelY - Ribbon.MARGE + Render.texSoloChannel.Height / 2));
                ClickableImage imgSoloChannel = new ClickableImage(UI, creationTime, "SoloChannel" + i.ToString(), Render.texSoloChannel, Render.texSoloChannel, new Vector2(Rec.X + Ribbon.MARGE * 3 + Render.FontMenu.MeasureString("0").X + Render.texSoloChannel.Width, channelY - Ribbon.MARGE + Render.texMuteChannel.Height / 2));

                imgMuteChannel.Tag = i;
                imgSoloChannel.Tag = i;

                imgMuteChannel.ClickImage += new ClickableImage.ClickImageHandler(imgMuteChannel_ClickImage);
                imgSoloChannel.ClickImage += new ClickableImage.ClickImageHandler(imgSoloChannel_ClickImage);

                ListUIChildren.Add(imgMuteChannel);
                ListUIChildren.Add(imgSoloChannel);

                for (int j = 0; j < Context.Map.Channels[i].ListMusician.Count; j++)
                {
                    float fj = (float)j;

                }
            }
        }

        private void UpdateButtonState()
        {
            for (int i = 1; i < Context.Map.Channels.Count; i++)
            {
                ClickableImage imgMuteChannel = (ClickableImage)ListUIChildren.Find(ui => ui is ClickableImage && ((ClickableImage)ui).Name == "MuteChannel" + i.ToString());
                ClickableImage imgSoloChannel = (ClickableImage)ListUIChildren.Find(ui => ui is ClickableImage && ((ClickableImage)ui).Name == "SoloChannel" + i.ToString());

                TypePlaying typePlaying = Context.Map.Channels[i].TypePlaying;

                if (typePlaying == TypePlaying.Muted)
                {
                    imgMuteChannel.Color = Color.Blue;
                    imgSoloChannel.Color = Color.White;
                }
                else if (typePlaying == TypePlaying.Solo)
                {
                    imgMuteChannel.Color = Color.White;
                    imgSoloChannel.Color = Color.Blue;
                }
                else if (typePlaying == TypePlaying.Playing)
                {
                    imgMuteChannel.Color = Color.White;
                    imgSoloChannel.Color = Color.White;
                }
            }
        }

        void imgMuteChannel_ClickImage(ClickableImage image, MouseState mouseState, GameTime gameTime)
        {
            GamePlay.MuteChannel((int)image.Tag);
            UpdateButtonState();
        }

        void imgSoloChannel_ClickImage(ClickableImage image, MouseState mouseState, GameTime gameTime)
        {
            GamePlay.SoloChannel((int)image.Tag);
            UpdateButtonState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float timelineDuration = 1000f * 10;

            Rectangle rec = new Rectangle(Rec.X, Rec.Y, Rec.Width, (int)((float)Rec.Height * 1.2f));

            Render.SpriteBatch.Draw(Render.texEmptyGradient, rec, new Color(0.2f, 0.2f, 0.2f));

            float channelHeight = (float)Rec.Height / ((float)Context.Map.Channels.Count - 1);
            float channelWidth = Rec.Width - 100 - Ribbon.MARGE;

            for (int i = 1; i < Context.Map.Channels.Count; i++)
            {
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

                //--- Channel
                Render.SpriteBatch.Draw(Render.texEmptyGradient, recChannel, Context.Map.Channels[i].Color);
                //---

                if (Context.Map.Channels[i].ListMusician == null || Context.Map.Channels[i].ListMusician.Count == 0)
                    continue;

                float heightPerMusician = channelHeight / (float)Context.Map.Channels[i].ListMusician.Count;
                for (int j = 0; j < Context.Map.Channels[i].ListMusician.Count; j++)
                {
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

            base.Draw(gameTime);
        }
    }
}
