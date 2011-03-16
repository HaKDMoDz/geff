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
using VoiceRecorder.Audio;
using System.Diagnostics;
using NAudio.Midi;

namespace TheGrid.Logic.Sound
{
    public class SoundLogic
    {
        public MidiIn midiIn = null;
        private IWavePlayer waveOutDevice;
        private WaveMixerStream32 mixer;
        private WaveStream[] reader;
        private EffectStream[] offsetStream;
        private WaveChannel32[] channelSteam;
        private Dictionary<String, List<int>> dicSample;
        private Dictionary<String, List<List<Effect>>> dicEffect;
        private int CountInstancePerSample = 12;
        private bool[] PlayingNote;

        public delegate void MidiNoteEventHandler(int noteKey, string noteName);
        public event MidiNoteEventHandler MidiNoteEvent;

        public GameEngine GameEngine { get; set; }

        public SoundLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
        }

        ~SoundLogic()
        {
            //waveOutDevice.Stop();
            waveOutDevice.Dispose();
            mixer.Close();

            if (midiIn != null)
            {
                midiIn.Stop();
                midiIn.Dispose();
            }
        }

        public void Init(Map map)
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
                waveOutDevice.Dispose();

                mixer.Close();
            }

            OpenMidiFile();

            mixer = new WaveMixerStream32();
            mixer.AutoStop = true;

            waveOutDevice = new NAudio.Wave.DirectSoundOut();
            //waveOutDevice = new NAudio.Wave.AsioOut();

            waveOutDevice.Init(mixer);

            int countSample = 0;
            map.Channels.ForEach(c => c.ListSample.ForEach(s => countSample++));
            countSample *= CountInstancePerSample;

            reader = new WaveStream[countSample];
            offsetStream = new EffectStream[countSample];
            channelSteam = new WaveChannel32[countSample];
            PlayingNote = new bool[countSample];
            dicSample = new Dictionary<String, List<int>>();
            dicEffect = new Dictionary<String, List<List<Effect>>>();

            LoadSample(map);

            waveOutDevice.Play();
            mixer.Position = long.MaxValue;
            mixer.AutoStop = false;

            CreateMidi();
        }

        private void CreateMidi()
        {
            if (midiIn != null)
            {
                midiIn.Stop();
                midiIn.Close();
                midiIn.Dispose();
            }

            if (MidiIn.NumberOfDevices > 0)
            {
                MidiInCapabilities cap = MidiIn.DeviceInfo(0);
                midiIn = new NAudio.Midi.MidiIn(0);
            }

            if (midiIn != null)
            {
                midiIn.Start();
                midiIn.MessageReceived += new EventHandler<MidiInMessageEventArgs>(midiIn_MessageReceived);
            }
        }

        void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            NoteEvent noteEvent = e.MidiEvent as NoteEvent;

            if (MidiNoteEvent != null && noteEvent != null && noteEvent.CommandCode == MidiCommandCode.NoteOn)
            {
                MidiNoteEvent(noteEvent.NoteNumber - 20, noteEvent.NoteName);
            }
        }

        public void PlayNote(Sample sample, float noteKey)
        {
            int indexChannel = GetFreeSample(sample);

            if (indexChannel != -1)
            {
                PlayingNote[dicSample[sample.Name][indexChannel]] = true;

                Effect effect = dicEffect[sample.Name][indexChannel].Find(e => e.Name == "SuperPitch");

                float octave = (int)((noteKey - sample.NoteKey) / 12f);
                float semitonesF = noteKey - (sample.NoteKey + 12f * octave);
                float semitones = (int)semitonesF;
                float cents = (int)((semitonesF - semitones) * 100f);

                //AddSlider(0, -100, 100, 1, "Pitch adjust (cents)");
                //AddSlider(0, -12, 12, 1, "Pitch adjust (semitones)");
                //AddSlider(0, -12, 12, 1, "Pitch adjust (octaves)");
                //AddSlider(50, 0, 200, 1, "Window size (ms)");
                //AddSlider(20, 0.05f, 50, 0.5f, "Overlap size (ms)");

                effect.Enabled = true;
                effect.Sliders[0].Value = cents;
                effect.Sliders[1].Value = semitones;
                effect.Sliders[2].Value = octave;
                //effect.Sliders[3].Value = 200;
                //effect.Sliders[4].Value = 50f;
                effect.Slider();

                channelSteam[dicSample[sample.Name][indexChannel]].Position = 0;
            }
        }

        public void PlaySample(Sample sample)
        {
            int indexChannel = GetFreeSample(sample);

            if (indexChannel != -1)
            {
                channelSteam[dicSample[sample.Name][indexChannel]].Position = 0;
            }
        }

        private int GetFreeSample(Sample sample)
        {
            if (sample == null)
                return -1;

            for (int i = 0; i < CountInstancePerSample; i++)
            {
                WaveChannel32 channel = channelSteam[dicSample[sample.Name][i]];

                if (!PlayingNote[dicSample[sample.Name][i]] && channel.CurrentTime == TimeSpan.Zero || channel.CurrentTime >= channel.TotalTime)
                {
                    return i;
                }
            }

            return -1;
        }


        private void Test()
        {
            float[] buffer = new float[4096];
            IPitchDetector pitchDetector = new AutoCorrelator(44100);

            for (int midiNoteNumber = 45; midiNoteNumber < 63; midiNoteNumber++)
            {
                float freq = (float)(8.175 * Math.Pow(1.05946309, midiNoteNumber));
                SetFrequency(buffer, freq);
                float detectedPitch = pitchDetector.DetectPitch(buffer, buffer.Length);
                // since the autocorrelator works with a lag, give it two shots at the same buffer
                detectedPitch = pitchDetector.DetectPitch(buffer, buffer.Length);
                Console.WriteLine("Testing for {0:F2}Hz, got {1:F2}Hz", freq, detectedPitch);
                //Assert.AreEqual(detectedPitch, freq, 0.5);
            }
        }

        private void SetFrequency(float[] buffer, float frequency)
        {
            float amplitude = 0.25f;
            for (int n = 0; n < buffer.Length; n++)
            {
                buffer[n] = (float)(amplitude * Math.Sin((2 * Math.PI * n * frequency) / 44100));
            }
        }

        private void LoadSample(Map map)
        {
            try
            {
                int index = 0;
                foreach (Channel channel in map.Channels)
                {
                    foreach (Sample sample in channel.ListSample)
                    {
                        List<int> listIndexSample = new List<int>();
                        dicSample.Add(sample.Name, listIndexSample);

                        for (int i = 0; i < CountInstancePerSample; i++)
                        {
                            WaveStream outStream = new WaveFileReader(sample.FileName);

                            //--- Détection de la fréquence moyenne du son
                            if (i == 0 && sample.Frequency == -1f)
                            {

                                //=======
                                IWaveProvider waveFloat = null;

                                if (outStream.WaveFormat.Channels > 1)
                                {
                                    StereoToMonoProvider16 stereo = new StereoToMonoProvider16(outStream);
                                    stereo.LeftVolume = 1f;
                                    stereo.RightVolume = 1f;

                                    waveFloat = new Wave16ToFloatProvider(stereo);
                                }
                                else
                                {
                                    waveFloat = new Wave16ToFloatProvider(outStream);
                                }

                                IWaveProvider outStream2 = new AutoTuneWaveProvider(waveFloat);
                                IWaveProvider wave16 = new WaveFloatTo16Provider(outStream2);

                                byte[] buffer = new byte[8192];
                                int _bytesRead;
                                do
                                {
                                    _bytesRead =
                                    wave16.Read(buffer, 0, buffer.Length);
                                    //writer.WriteData(buffer, 0, _bytesRead);
                                } while (_bytesRead != 0);//&& writer.Length < waveFileReader.Length);
                                //writer.Close();
                                //=======
                                outStream.Position = 0;

                                //---> Fréquence sonore du sample
                                sample.Frequency = ((AutoTuneWaveProvider)outStream2).Frequency;
                                //---> Note sur un clavier de 88 touches pour la fréqence
                                sample.NoteKey = (12f * (float)Math.Log(sample.Frequency / 55f) + 13f * (float)Math.Log(2f)) / ((float)Math.Log(2f));

                                Debug.WriteLine(sample.Name + " == " + sample.Frequency.ToString() + " == " + sample.NoteKey.ToString());
                            }
                            //---

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

                        sample.Duration = channelSteam[dicSample[sample.Name][0]].TotalTime;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Update(ChannelEffect channelEffect, float[] values, GameTime gameTime)
        {
            foreach (Sample sample in channelEffect.Channel.ListSample)
            {
                for (int i = 0; i < CountInstancePerSample; i++)
                {
                    if (!PlayingNote[dicSample[sample.Name][i]])
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

            //AddEffect(effectChain, listEffect, typeof(Volume));
            //AddEffect(effectChain, listEffect, typeof(Chorus));
            //AddEffect(effectChain, listEffect, typeof(JSNet.Delay));
            //AddEffect(effectChain, listEffect, typeof(Flanger));
            //AddEffect(effectChain, listEffect, typeof(Tremolo));
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

        public void Stop()
        {
            if (channelSteam == null)
                return;

            for (int i = 0; i < channelSteam.Length; i++)
            {
                channelSteam[i].Position = channelSteam[i].Length;
            }
        }

        public void Stop(string sampleName)
        {
            if (channelSteam == null)
                return;

            for (int i = 0; i < CountInstancePerSample; i++)
            {
                WaveChannel32 channel = channelSteam[dicSample[sampleName][i]];
                channel.Position = channel.Length;
            }
        }

        public void OpenMidiFile()
        {
            return;

            //NAudio.Midi.MidiFile midiFile = new MidiFile(@"D:\Libraries\Musics\Midi\beethoven-pour-elise.mid");
            //flourish
            //town
            NAudio.Midi.MidiFile midiFile = new MidiFile(@"D:\GDD\Log\Geff\TheGrid\TheGrid\TheGrid\Files\Sound\Midi\town.mid");
            string part = String.Empty;
            TimeSignatureEvent lastTimeSignature = null;// new TimeSignatureEvent(4,4,24,32,

            List<NoteEvent> listNote = new List<NoteEvent>();

            Dictionary<float, string> dicNoteType = new Dictionary<float, string>();

            //dicNoteType.Add(1.75f * 4f, "Ronde double pointée");
            //dicNoteType.Add(1.5f * 4f, "Ronde pointée");
            dicNoteType.Add(4f, "Ronde");

            dicNoteType.Add(1.75f * 2f, "Blanche double pointée");
            dicNoteType.Add(1.5f * 2f, "Blanche pointée");
            dicNoteType.Add(2f, "Blanche");

            dicNoteType.Add(1.75f * 1f, "Noire double pointée");
            dicNoteType.Add(1.5f * 1f, "Noire pointée");
            dicNoteType.Add(1f, "Noire");

            dicNoteType.Add(1.75f * 0.5f, "Croche double pointée");
            dicNoteType.Add(1.5f * 0.5f, "Croche pointée");
            dicNoteType.Add(0.5f, "Croche");

            dicNoteType.Add(1.75f * 0.25f, "Double croche double pointée");
            dicNoteType.Add(1.5f * 0.25f, "Double croche pointée");
            dicNoteType.Add(0.25f, "Double croche");

            dicNoteType.Add(1.75f * 0.125f, "Triple croche double pointée");
            dicNoteType.Add(1.5f * 0.125f, "Triple croche pointée");
            dicNoteType.Add(0.125f, "Triple croche");

            dicNoteType.Add(1.75f * 0.0625f, "Quadruple croche double pointée");
            dicNoteType.Add(1.5f * 0.0625f, "Quadruple croche pointée");
            dicNoteType.Add(0.0625f, "Quadruple croche");


            if (midiFile != null)
            {
                part += String.Format(" [ DeltaTicksPerQuarterNote : {0} ] ", midiFile.DeltaTicksPerQuarterNote);

                for (int i = 0; i < midiFile.Tracks; i++)
                {
                    part += "\r\n";
                    foreach (MidiEvent midiEvent in midiFile.Events[i])
                    {
                        NoteOnEvent noteEvent = midiEvent as NoteOnEvent;

                        part += "\r\n";
                        if (midiEvent is TimeSignatureEvent)
                        {
                            lastTimeSignature = midiEvent as TimeSignatureEvent;
                            part += String.Format(" [ {0} ] ", midiEvent.ToString());
                        }
                        else if (noteEvent != null && noteEvent.CommandCode == MidiCommandCode.NoteOn)
                        {
                            int noteLength = 1;
                            try
                            {
                                noteLength = noteEvent.NoteLength;
                            }
                            catch { }

                            part += String.Format(" [ AbsoluteTime {0} #  DeltaTime {1} # NoteName {2} # NoteNumber {3} # NoteLength {4} ]  ", noteEvent.AbsoluteTime, noteEvent.DeltaTime, noteEvent.NoteName, noteEvent.NoteNumber, noteLength);

                            float typeNote = (float)noteLength / (float)midiFile.DeltaTicksPerQuarterNote;

                            string typeNoteString = String.Empty;

                            if (dicNoteType.ContainsKey(typeNote))
                                typeNoteString = dicNoteType[typeNote];

                            part += String.Format(" OFF :: {0} :: {1}", typeNote, typeNoteString);

                            listNote.Add(noteEvent);
                        }
                        else
                        {
                            part += String.Format(" [ {0} ] ", midiEvent.ToString());
                        }
                    }
                }
            }

            int a = 0;
        }
    }
}
