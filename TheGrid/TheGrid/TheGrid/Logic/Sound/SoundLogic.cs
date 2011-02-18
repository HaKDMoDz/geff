using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using TheGrid.Common;
using JSNet;
using Microsoft.Xna.Framework;
using TheGrid.Model.Effect;
using System.Reflection;

namespace TheGrid.Logic.Sound
{
    public class SoundLogic
    {
        private IWavePlayer waveOutDevice;
        private WaveMixerStream32 mixer;
        private WaveStream[] reader;
        private EffectStream[] offsetStream;
        private WaveChannel32[] channelSteam;
        private Dictionary<String, List<int>> dicSample;
        private Dictionary<String, List<List<Effect>>> dicEffect;
        private int CountInstancePerSample = 6;

        public GameEngine GameEngine { get; set; }

        public SoundLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
        }

        ~SoundLogic()
        {
            waveOutDevice.Dispose();
        }

        public void Init()
        {
            if (waveOutDevice != null)
                waveOutDevice.Dispose();

            mixer = new WaveMixerStream32();
            mixer.AutoStop = true;

            waveOutDevice = new NAudio.Wave.DirectSoundOut();
            //waveOutDevice = new NAudio.Wave.AsioOut();

            waveOutDevice.Init(mixer);
            waveOutDevice.Play();

            int countSample = 0;
            Context.Map.Channels.ForEach(c => c.ListSample.ForEach(s => countSample++));
            countSample *= CountInstancePerSample;

            reader = new WaveStream[countSample];
            offsetStream = new EffectStream[countSample];
            channelSteam = new WaveChannel32[countSample];
            dicSample = new Dictionary<String, List<int>>();
            dicEffect = new Dictionary<String, List<List<Effect>>>();

            LoadSample();

            mixer.Position = long.MaxValue;
            mixer.AutoStop = false;
        }

        public void PlaySample(Sample sample)
        {
            for (int i = 0; i < CountInstancePerSample; i++)
            {
                WaveChannel32 channel = channelSteam[dicSample[sample.Name][i]];

                if (channel.CurrentTime == TimeSpan.Zero || channel.CurrentTime >= channel.TotalTime)
                {
                    channel.Position = 0;
                    return;
                }
            }
        }

        private void LoadSample()
        {
            int index = 0;
            foreach (Channel channel in Context.Map.Channels)
            {
                foreach (Sample sample in channel.ListSample)
                {
                    List<int> listIndexSample = new List<int>();
                    dicSample.Add(sample.Name, listIndexSample);

                    for (int i = 0; i < CountInstancePerSample; i++)
                    {
                        WaveStream outStream = null;

                        outStream = new WaveFileReader(sample.FileName);

                        outStream = WaveFormatConversionStream.CreatePcmStream(outStream);
                        outStream = new BlockAlignReductionStream(outStream);

                        reader[index] = outStream;

                        offsetStream[index] = new EffectStream(CreateEffectChain(sample), reader[index]);
                        channelSteam[index] = new WaveChannel32(offsetStream[index]);

                        channelSteam[index].Position = channelSteam[index].Length;
                        mixer.AddInputStream(channelSteam[index]);

                        listIndexSample.Add(index);

                        index++;
                    }
                }
            }
        }

        public void Update(ChannelEffect channelEffect, float[] values, GameTime gameTime)
        {
            foreach (Sample sample in channelEffect.Channel.ListSample)
            {
                for (int i = 0; i < CountInstancePerSample; i++)
                {
                    Effect effect = dicEffect[sample.Name][i].Find(e => e.Name == channelEffect.Name);

                    if (values[0] == float.MinValue)
                    {
                        effect.Enabled = false;
                    }
                    else
                    {
                        effect.Enabled = true;

                        for (int j = 0; j < values.Length; j++)
                        {
                            effect.Sliders[j].Value = values[j];
                        }

                        effect.Slider();
                    }
                }
            }
        }

        private EffectChain CreateEffectChain(Sample sample)
        {
            EffectChain effectChain = new EffectChain();
            List<Effect> listEffect = new List<Effect>();

            List<List<Effect>> listListEffect;

            if (dicEffect.ContainsKey(sample.Name))
            {
                listListEffect = dicEffect[sample.Name];
            }
            else
            {
                listListEffect = new List<List<Effect>>();
                dicEffect.Add(sample.Name, listListEffect);
            }

            listListEffect.Add(listEffect);

            AddEffect(effectChain, listEffect, typeof(Volume));
            AddEffect(effectChain, listEffect, typeof(Chorus));
            AddEffect(effectChain, listEffect, typeof(JSNet.Delay));
            AddEffect(effectChain, listEffect, typeof(Flanger));
            AddEffect(effectChain, listEffect, typeof(Tremolo));
            AddEffect(effectChain, listEffect, typeof(SuperPitch));

            return effectChain;
        }

        private Effect AddEffect(EffectChain effectChain, List<Effect> listEffect, Type typeOfEffect)
        {
            Effect effect = (Effect)Activator.CreateInstance(typeOfEffect);
            effectChain.Add(effect);
            listEffect.Add(effect);
            effect.SampleRate = 44100f;
            effect.Enabled = false;

            foreach (Slider slider in effect.Sliders)
            {
                slider.Value = slider.Default;
            }

            effect.Slider();

            return effect;
        }

        public IList<Slider> GetEffectParameters(string effectName)
        {
            string typeName = "JSNet." + effectName;

            Assembly assembly = System.AppDomain.CurrentDomain.GetAssemblies().ToList().Find(a => a.GetName().Name == "JSNet");
            Type type = assembly.GetType(typeName);

            Effect effect = (Effect)Activator.CreateInstance(type);

            return effect.Sliders;
        }
    }
}
