using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheGrid.Model;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using IrrKlang;
using TheGrid.Common;
using JSNet;
using Microsoft.Xna.Framework;
using TheGrid.Model.Effect;
using System.Reflection;

namespace TheGrid.Logic.Sound
{
    public class SoundLogic
    {
        ISoundEngine _soundEngine;
        public void PlaySample2(Sample sample)
        {
            ISound sound = _soundEngine.Play2D(sample.FileName);
        }

        public GameEngine GameEngine { get; set; }

        public SoundLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;


            //_soundEngine = new ISoundEngine();
        }

        private IWavePlayer waveOutDevice;
        private WaveMixerStream32 mixer;
        WaveStream[] reader = new WaveStream[4];
        EffectStream[] offsetStream = new EffectStream[4];
        WaveChannel32[] channelSteam = new WaveChannel32[4];
        Dictionary<String, int> dicSample;
        Dictionary<String, List<Effect>> dicEffect;

        public void Init()
        {
            mixer = new WaveMixerStream32();
            mixer.AutoStop = false;

            waveOutDevice = new NAudio.Wave.DirectSoundOut();

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

                    offsetStream[index] = new EffectStream(CreateEffectChain(sample), new WaveOffsetStream(reader[index]));
                    channelSteam[index] = new WaveChannel32(offsetStream[index]);


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

                for (int i = 0; i < values.Length; i++)
                {
                    effect.Sliders[i].Value = values[i];
                }

                effect.Slider();
            }
        }

        private EffectChain CreateEffectChain(Sample sample)
        {
            EffectChain effectChain = new EffectChain();
            List<Effect> listEffect = new List<Effect>();

            dicEffect.Add(sample.Name, listEffect);

            //Volume volume = new Volume();
            //effectChain.Add(new JSNet.Volume());
            //listEffect.Add(volume);
            //volume.Sliders[0].Value = 0f;
            //volume.Sliders[1].Value = 0f;
            //volume.Enabled = true;
            //volume.SampleRate = 44100f;
            //volume.Init();
            //volume.Slider();

            Chorus chorus = new Chorus();
            effectChain.Add(chorus);
            listEffect.Add(chorus);
            chorus.Enabled = false;

            JSNet.Delay delay = new JSNet.Delay();
            effectChain.Add(delay);
            listEffect.Add(delay);
            delay.Enabled = false;

            Flanger flanger = new Flanger();
            effectChain.Add(flanger);
            listEffect.Add(flanger);
            flanger.Enabled = false;

            Tremolo tremolo = new Tremolo();
            effectChain.Add(tremolo);
            listEffect.Add(tremolo);
            tremolo.Enabled = false;

            SuperPitch superPtich = new SuperPitch();
            effectChain.Add(superPtich);
            listEffect.Add(superPtich);
            superPtich.Enabled = true;
            superPtich.Sliders[2].Value = 1f;
            superPtich.Slider();


            return effectChain;
        }

        public IList<Slider> GetEffectParameters(string effectName)
        {
            string typeName = "JSNet." + effectName;
            Type type = Type.GetType(typeName);

            Assembly[] appAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in appAssemblies)
            {
                foreach (Type assemblyType in assembly.GetTypes())
                {
                    if (assemblyType.ToString().Equals(typeName))
                        type = assemblyType;
                }
            }


            
            Effect effect = (Effect)Activator.CreateInstance(type);

            return effect.Sliders;
        }
    }
}
