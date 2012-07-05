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
        Music music;
        int[,] noteState = new int[12, 2];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            noteState[0, 0] = 0;
            noteState[0, 1] = 0;

            noteState[1, 0] = 1;
            noteState[1, 1] = 0;

            noteState[2, 0] = 1;
            noteState[2, 1] = 1;

            noteState[3, 0] = 2;
            noteState[3, 1] = 0;

            noteState[4, 0] = 2;
            noteState[4, 1] = 1;

            noteState[5, 0] = 3;
            noteState[5, 1] = 0;

            noteState[6, 0] = 3;
            noteState[6, 1] = 1;

            noteState[7, 0] = 4;
            noteState[7, 1] = 0;

            noteState[8, 0] = 5;
            noteState[8, 1] = 0;

            noteState[9, 0] = 5;
            noteState[9, 1] = 1;

            noteState[10, 0] = 6;
            noteState[10, 1] = 0;

            noteState[11, 0] = 6;
            noteState[11, 1] = 1;


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

            music = new Music();
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

                            float measureLenght = (float)lastTimeSignature.Numerator * 4f / (float)lastTimeSignature.Denominator * (float)midiFile.DeltaTicksPerQuarterNote;

                            Note newNote = new Note();
                            newNote.NoteLength = noteEvent.NoteLength;
                            newNote.NoteName = noteEvent.NoteName;
                            newNote.NoteNumber = noteEvent.NoteNumber;
                            newNote.AbsoluteTime = noteEvent.AbsoluteTime;

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

        public void DrawScore(Music music, Graphics g)
        {
            g.Clear(Color.White);

            float interlineSize = 7f;
            float currentY = 20;

            int nbPart = (int)Math.Round((double)((g.VisibleClipBounds.Height - currentY) / (7f * interlineSize * (float)music.ListChanel.Count + 5f * interlineSize)), MidpointRounding.AwayFromZero);


            float firstPartY = currentY;

            for (int i = 0; i < nbPart; i++)
            {
                firstPartY = currentY;

                for (int j = 0; j < music.ListChanel.Count; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {

                        if (k < 5)
                        {
                            g.DrawLine(Pens.DarkGray, new Point(5, (int)currentY), new Point((int)g.VisibleClipBounds.Width - 5, (int)currentY));
                        }

                        currentY += interlineSize;

                    }
                }

                g.DrawLine(Pens.DarkGray, new Point(5, (int)firstPartY), new Point(5, (int)(currentY - 3f * interlineSize)));
                g.DrawLine(Pens.DarkGray, new Point((int)g.VisibleClipBounds.Width - 5, (int)firstPartY), new Point((int)g.VisibleClipBounds.Width - 5, (int)(currentY - 3f * interlineSize)));

                currentY += 3f * interlineSize;
            }


            //---

            
            //foreach (Channel channel in music.ListChanel)
            for (int i = 0; i < music.ListChanel.Count; i++)
			{
                currentY=20 + i * 7f * interlineSize;
                float currentX = 5f;
                foreach (Measure measure in music.ListChanel[i].ListMeasure)
                {
                    foreach (Note note in measure.ListNode)
                    {
                        int noteIndex = note.NoteNumber % 12-4;
                        if (noteIndex < 0)
                            noteIndex += 12;

                        float noteY = interlineSize / 2f * (6-noteState[noteIndex, 0]);
                        Rectangle rec= new Rectangle((int)currentX, (int)(currentY + noteY), 10, (int)interlineSize - 2);

                        if (noteState[noteIndex, 1] == 0)
                            g.DrawRectangle(Pens.Black, rec);
                        else
                            g.FillRectangle(Brushes.Black, rec);

                        currentX+=10f;
                    }
                }
            }

        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = pictureBox1.CreateGraphics();
            DrawScore(music, graphics);
        }
    }
}
