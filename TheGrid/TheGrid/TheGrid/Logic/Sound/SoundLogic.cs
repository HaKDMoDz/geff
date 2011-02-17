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
        WaveStream[] reader;
        EffectStream[] offsetStream;
        WaveChannel32[] channelSteam;
        Dictionary<String, int> dicSample;
        Dictionary<String, List<Effect>> dicEffect;

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

            reader = new WaveStream[countSample];
            offsetStream = new EffectStream[countSample];
            channelSteam = new WaveChannel32[countSample];
            dicSample = new Dictionary<String, int>();
            dicEffect = new Dictionary<String, List<Effect>>();

            LoadSample();

            mixer.Position = long.MaxValue;
            mixer.AutoStop = false;
        }

        public void PlaySample(Sample sample)
        {
            channelSteam[dicSample[sample.Name]].Position = 0;
        }

        private void LoadSample()
        {
            int index = 0;
            foreach (Channel channel in Context.Map.Channels)
            {
                foreach (Sample sample in channel.ListSample)
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

                    dicSample.Add(sample.Name, index);

                    index++;
                }
            }
        }

        public void Update(ChannelEffect channelEffect, float[] values, GameTime gameTime)
        {
            foreach (Sample sample in channelEffect.Channel.ListSample)
            {
                Effect effect = dicEffect[sample.Name].Find(e => e.Name == channelEffect.Name);

                if (values[0] == float.MinValue)
                {
                    effect.Enabled = false;
                }
                else
                {
                    effect.Enabled = true;

                    for (int i = 0; i < values.Length; i++)
                    {
                        effect.Sliders[i].Value = values[i];
                    }

                    effect.Slider();
                }
            }
        }

        private EffectChain CreateEffectChain(Sample sample)
        {
            EffectChain effectChain = new EffectChain();
            List<Effect> listEffect = new List<Effect>();

            dicEffect.Add(sample.Name, listEffect);

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
