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
        Dictionary<string, string> dicNoteName = new Dictionary<string, string>();

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

            dicNoteName.Add("C", "Do");
            dicNoteName.Add("D", "Ré");
            dicNoteName.Add("E", "Mi");
            dicNoteName.Add("F", "Fa");
            dicNoteName.Add("G", "Sol");
            dicNoteName.Add("A", "La");
            dicNoteName.Add("B", "Si");


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
                        music.AddChannel(currentChannel);
                    }
                }
            }

            Form1_Resize(null, null);
        }

        float interlineSize = 28f;
        float currentY = 40;
        float currentX = 30f;


        float margeX = 30f;
        float margeY = 80f;

        public void DrawScore(Music music)
        {
            if (pictureBox1.Width == 0 || pictureBox1.Height == 0)
                return;

            Image bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics g = Graphics.FromImage(bmp);

            pictureBox1.Image = bmp;

            string[] noteNames = new string[] { "E", "F", "G", "A", "B", "C", "D", "E", "F", "G", "A" };



            g.Clear(Color.White);

            interlineSize = 28f;
            currentY = margeY;
            currentX = margeX;

            currentY -= vScrollBar1.Value*interlineSize;

            int nbPart = (int)Math.Round((double)((g.VisibleClipBounds.Height) / (5f * interlineSize * (float)music.ListChanel.Count + 5f * interlineSize)), MidpointRounding.AwayFromZero);

            float firstPartY = margeY;
            float noteNameY = margeY;


            double firstMeasure = 1+4*Math.Round((((float)vScrollBar1.Value * interlineSize - margeY) / (6f * interlineSize * (float)music.ListChanel.Count + 2f * interlineSize)), MidpointRounding.AwayFromZero);

            for (int i = 0; i <= music.MaxMeasure/4; i++)
            {
                bool draw = true;
                if (!(i*4+1 >= (firstMeasure-4) && i*4+1 <= (firstMeasure+ nbPart*4)))
                    draw=false;

                firstPartY = currentY;

                for (int j = 0; j < music.ListChanel.Count; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        if (draw && k < 5)
                        {
                            g.DrawLine(Pens.DarkGray, new Point((int)margeX, (int)currentY), new Point((int)(g.VisibleClipBounds.Width - margeX), (int)currentY));
                        }

                        currentY += interlineSize;
                    }

                    noteNameY = currentY;
                    for (int k = 0; k < noteNames.Length; k++)
                    {
                        if (draw)
                        {
                            string noteName = dicNoteName[noteNames[k]];
                            g.DrawString(noteName, this.Font, Brushes.Black, new PointF(8, (int)(noteNameY - (k + 4) * interlineSize / 2 - interlineSize / 4)));
                        }
                    }
                }

                if (draw)
                {
                    g.DrawLine(Pens.DarkGray, new Point((int)margeX, (int)firstPartY), new Point((int)margeX, (int)(currentY - 2f * interlineSize)));
                    g.DrawLine(Pens.DarkGray, new Point((int)(g.VisibleClipBounds.Width - margeX), (int)firstPartY), new Point((int)(g.VisibleClipBounds.Width - margeX), (int)(currentY - 2f * interlineSize)));
                }

                currentY += 2f * interlineSize;
            }


            //---

            SolidBrush brushWhite = new SolidBrush(Color.SkyBlue);
            SolidBrush brushBlack = new SolidBrush(Color.SteelBlue);

            for (int i = 0; i < music.ListChanel.Count; i++)
			{
                currentY = margeY + i * 6f * interlineSize - vScrollBar1.Value*interlineSize;
                foreach (Measure measure in music.ListChanel[i].ListMeasure)
                {
                    float nbMeasure = measure.MeasureStart / measure.MeasureLength;

                    if (!(nbMeasure >= (firstMeasure-4) && nbMeasure <= (firstMeasure +nbPart*4)))
                        continue;

                    Point pointTopMeasure = new Point((int)(((g.VisibleClipBounds.Width - 2*margeX) / 4) * (nbMeasure%4) + margeX), (int)margeY+
                        (int)((int)(i*6f*interlineSize)+
                        ((music.ListChanel.Count * 6 + 2f) * interlineSize) * (int)(nbMeasure / 4f)) - (int)(vScrollBar1.Value * interlineSize));

                    g.DrawLine(Pens.DarkOrange, pointTopMeasure, new Point(pointTopMeasure.X, pointTopMeasure.Y+ (int)(4f*interlineSize)));
                    
                    if( i == 0)
                        g.DrawString((nbMeasure+1).ToString(), this.Font, Brushes.Black, new Point(pointTopMeasure.X+5, pointTopMeasure.Y-(int)interlineSize));

                    foreach (Note note in measure.ListNode)
                    {
                        int noteIndex = note.NoteNumber % 12-4;
                        if (noteIndex < 0)
                            noteIndex += 12;

                        float noteX = ((g.VisibleClipBounds.Width - 2 * margeX) / 4) / measure.MeasureLength * (float)note.AbsoluteTime;
                        float noteY = interlineSize / 2f * (float)(7-noteState[noteIndex, 0]);
                        if (note.NoteNumber > 75)
                            noteY -= 3.5f * interlineSize;

                        noteY += interlineSize / 4f;

                        if (noteX >= g.VisibleClipBounds.Width - 2f*margeX)
                        {
                            currentY = margeY + i * 6f * interlineSize - (vScrollBar1.Value * interlineSize) + ((music.ListChanel.Count*6+2f) * interlineSize)* (int)(noteX/(g.VisibleClipBounds.Width - 2f * margeX));
                            noteX %= g.VisibleClipBounds.Width -2f*margeX;
                        }

                        Rectangle rec = new Rectangle((int)(currentX + noteX + 2), (int)(currentY + noteY + 1), (int)(((g.VisibleClipBounds.Width - 2 * currentX) / 4) / measure.MeasureLength * (float)note.NoteLength) - 2, (int)(interlineSize / 2f - 2));

                        g.DrawRectangle(Pens.Black, rec);

                        if (noteState[noteIndex, 1] == 1)
                        {
                            string noteName = dicNoteName[note.NoteName.Substring(0,1)];

                            g.FillRectangle(brushBlack, rec);
                            g.DrawString(noteName, this.Font, Brushes.White, rec.Location);

                        }
                        else
                        {
                            string noteName = dicNoteName[note.NoteName.Substring(0, 1)];

                            g.FillRectangle(brushWhite, rec);
                            g.DrawString(noteName, this.Font, Brushes.Black, rec.Location);
                        }
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
            int max = music.ListChanel.Max(c=>c.ListMeasure.Count);

            int nbPart = (int)Math.Round((double)(pictureBox1.Height / (6f * interlineSize * (float)music.ListChanel.Count + 2f * interlineSize)), MidpointRounding.AwayFromZero);

            if (nbPart == 0)
                return;

            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = (int)((margeY + (6f * interlineSize * (float)music.ListChanel.Count + 2f * interlineSize) * (float)max /4f ) / interlineSize);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            double firstMeasure = 1+4*Math.Round((((float)vScrollBar1.Value * interlineSize - margeY) / (((float)music.ListChanel.Count*6f + 2f)* interlineSize)),  MidpointRounding.AwayFromZero);
            this.Text = firstMeasure.ToString();
            DrawScore(music);
        }

        private void btnOuvrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.mid |*.mid";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenMidiFile(dlg.FileName);
            }
        }
    }
}
