using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.Midi;

namespace NewScore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            OpenMidiFile(@"D:\GDD\Log\Geff\NewScore\beethoven-pour-elise.mid");
            //OpenMidiFile(@"D:\GDD\Log\Geff\NewScore\Debussy - Clair de lune.mid");
        }

        public void OpenMidiFile(string fileName)
        {
            //NAudio.Midi.MidiFile midiFile = new MidiFile(@"D:\Libraries\Musics\Midi\beethoven-pour-elise.mid");
            //flourish
            //town
            MidiFile midiFile = new MidiFile(fileName);
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

            Music music = new Music();
            music.ListChanel = new List<Channel>();
            Dictionary<int, int> dicChannel = new Dictionary<int, int>();


            if (midiFile != null)
            {
                part += String.Format(" [ DeltaTicksPerQuarterNote : {0} ] ", midiFile.DeltaTicksPerQuarterNote);

                for (int i = 0; i < midiFile.Tracks; i++)
                {
                    part += "\r\n";
                    foreach (MidiEvent midiEvent in midiFile.Events[i])
                    {
                        NoteOnEvent noteEvent = midiEvent as NoteOnEvent;

                        if (midiEvent is TrackSequenceNumberEvent)
                        {
                            int b = 0;
                        }


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

                            Channel currentChannel = null;
                            if (dicChannel.ContainsKey(noteEvent.Channel))
                            {
                                currentChannel = music.ListChanel[dicChannel[noteEvent.Channel]];
                            }
                            else
                            {
                                currentChannel = new Channel();
                                currentChannel.ListMeasure = new List<Measure>();
                                currentChannel.ListMeasure.Add(new Measure());
                                currentChannel.ListMeasure[0].ListNode = new List<Note>();

                                music.ListChanel.Add(currentChannel);
                                dicChannel.Add(noteEvent.Channel, music.ListChanel.Count - 1);
                            }

                            Note newNote = new Note();
                            newNote.NoteLength = noteEvent.NoteLength;
                            newNote.NoteName = noteEvent.NoteName;
                            currentChannel.ListMeasure[0].ListNode.Add(newNote);
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
