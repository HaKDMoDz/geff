using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.Midi;
using System.Drawing.Drawing2D;

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
            Measure curMeasure = null;
            bool hasNote = false;
            if (midiFile != null)
            {
                part += String.Format(" [ DeltaTicksPerQuarterNote : {0} ] ", midiFile.DeltaTicksPerQuarterNote);

                for (int i = 0; i < midiFile.Tracks; i++)
                {
                    Channel currentChannel = null;
                    currentChannel = new Channel();
                    hasNote = false;

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
                        else if (noteEvent != null && noteEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity>0)
                        {
                            int noteLength = 1;
                            try
                            {
                                noteLength = noteEvent.NoteLength;
                            }
                            catch { }

                            part += String.Format(" [ AbsoluteTime {0} #  DeltaTime {1} # NoteName {2} # NoteNumber {3} # NoteLength {4}  # Channel {5}]  ", noteEvent.AbsoluteTime, noteEvent.DeltaTime, noteEvent.NoteName, noteEvent.NoteNumber, noteLength, noteEvent.Channel);

                            float typeNote = (float)noteLength / (float)midiFile.DeltaTicksPerQuarterNote;

                            string typeNoteString = String.Empty;

                            if (dicNoteType.ContainsKey(typeNote))
                                typeNoteString = dicNoteType[typeNote];

                            part += String.Format(" OFF :: {0} :: {1}", typeNote, typeNoteString);

                            listNote.Add(noteEvent);

                            float measureLenght = (float)lastTimeSignature.Numerator * 4f / (float)lastTimeSignature.No32ndNotesInQuarterNote * (float)midiFile.DeltaTicksPerQuarterNote;

                           


                            Note newNote = new Note();
                            newNote.NoteLength = noteEvent.NoteLength;
                            newNote.NoteName = noteEvent.NoteName;
                            newNote.NoteNumber = noteEvent.NoteNumber;
                            newNote.AbsoluteTime = noteEvent.AbsoluteTime;

                            curMeasure = currentChannel.FindMeasure(noteEvent.AbsoluteTime);

                            if (curMeasure == null)
                            {
                                int nbPrevMeasure = (int)(noteEvent.AbsoluteTime / measureLenght);

                                curMeasure = new Measure(nbPrevMeasure*measureLenght, measureLenght);
                                currentChannel.ListMeasure.Add(curMeasure);
                            }

                            curMeasure.ListNode.Add(newNote);
                            hasNote = true;
                        }
                        else
                        {
                            part += String.Format(" [ {0} ] ", midiEvent.ToString());
                        }
                    }

                    if (hasNote)
                    {
                        music.ListChanel.Add(currentChannel);
                    }
                }
            }

            int a = 0;
        }

        float interlineSize = 24f;
        float currentY = 40;
        float currentX = 30f;

        public void DrawScore(Music music)
        {
            Image bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics g = Graphics.FromImage(bmp);

            pictureBox1.Image = bmp;

            string[] noteNames = new string[] { "E", "F", "G", "A", "B", "C", "D", "E", "F", "G", "A"};
            g.Clear(Color.White);

            interlineSize = 24f;
            currentY = 40;
            currentX = 30f;

            //currentY -= vScrollBar1.Value * interlineSize * music.ListChanel.Count;

            int nbPart = (int)Math.Round((double)((g.VisibleClipBounds.Height - currentY) / (7f * interlineSize * (float)music.ListChanel.Count + 5f * interlineSize)), MidpointRounding.AwayFromZero);

            float firstPartY = currentY;
            float noteNameY = currentY;

            for (int i = 0; i < nbPart; i++)
            {
                firstPartY = currentY;

                for (int j = 0; j < music.ListChanel.Count; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        if (k < 5)
                        {
                            g.DrawLine(Pens.DarkGray, new Point((int)currentX, (int)currentY), new Point((int)(g.VisibleClipBounds.Width - currentX), (int)currentY));
                        }

                        currentY += interlineSize;
                    }

                    noteNameY = currentY;
                    for (int k = 0; k < noteNames.Length; k++)
                    {
                        g.DrawString(noteNames[k], this.Font, Brushes.Black, new PointF(8, (int)(noteNameY - (k+4) * interlineSize / 2 - interlineSize / 4)));
                    }
                }

                g.DrawLine(Pens.DarkGray, new Point((int)currentX, (int)firstPartY), new Point((int)currentX, (int)(currentY - 3f * interlineSize)));
                g.DrawLine(Pens.DarkGray, new Point((int)(g.VisibleClipBounds.Width - currentX), (int)firstPartY), new Point((int)(g.VisibleClipBounds.Width - currentX), (int)(currentY - 3f * interlineSize)));

                currentY += 3f * interlineSize;
            }


            //---

            SolidBrush brushWhite = new SolidBrush(Color.SkyBlue);
            SolidBrush brushBlack = new SolidBrush(Color.SteelBlue);

            for (int i = 0; i < music.ListChanel.Count; i++)
			{
                currentY=40 + i * 6f * interlineSize;
                foreach (Measure measure in music.ListChanel[i].ListMeasure)
                {
                    Point pointTopMeasure = new Point((int)(((g.VisibleClipBounds.Width - 2*currentX) / 4) / measure.MeasureLength * measure.MeasureStart + currentX), (int)currentY);
                    g.DrawLine(Pens.DarkOrange, pointTopMeasure, new Point(pointTopMeasure.X, pointTopMeasure.Y+ (int)(4f*interlineSize)));

                    foreach (Note note in measure.ListNode)
                    {
                        int noteIndex = note.NoteNumber % 12-4;
                        if (noteIndex < 0)
                            noteIndex += 12;

                        float noteX = ((g.VisibleClipBounds.Width - 2 * currentX) / 4) / measure.MeasureLength * (float)note.AbsoluteTime;
                        float noteY = interlineSize / 2f * (float)(7-noteState[noteIndex, 0]);

                        if (note.NoteNumber > 75)
                            noteY -= 3.5f * interlineSize;

                        noteY += interlineSize / 4f;

                        Rectangle rec = new Rectangle((int)(currentX + noteX + 2), (int)(currentY + noteY + 1), (int)(((g.VisibleClipBounds.Width - 2 * currentX) / 4) / measure.MeasureLength * (float)note.NoteLength) - 2, (int)(interlineSize / 2f - 2));

                        g.DrawRectangle(Pens.Black, rec);

                        if (noteState[noteIndex, 1] == 1)
                            g.FillRectangle(brushBlack, rec);
                        else
                            g.FillRectangle(brushWhite, rec);
                    }
                }
            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawScore(music);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //int max = music.ListChanel.Max(c=>c.ListMeasure.Count);

            //int nbPart = (int)Math.Round((double)((pictureBox1.Height - currentY) / (7f * interlineSize * (float)music.ListChanel.Count + 5f * interlineSize)), MidpointRounding.AwayFromZero);

            //vScrollBar1.Maximum = max / 4 * music.ListChanel.Count / nbPart;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            DrawScore(music);
        }
    }
}
