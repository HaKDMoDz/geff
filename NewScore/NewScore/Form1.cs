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
        int[,] noteStateSol = new int[12, 2];
        int[,] noteStateFa = new int[12, 2];
        SolidBrush[,] brushNote;

        Dictionary<string, string> dicNoteName = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            noteStateSol[0, 0] = 0;
            noteStateSol[0, 1] = 0;

            noteStateSol[1, 0] = 1;
            noteStateSol[1, 1] = 0;

            noteStateSol[2, 0] = 1;
            noteStateSol[2, 1] = 1;

            noteStateSol[3, 0] = 2;
            noteStateSol[3, 1] = 0;

            noteStateSol[4, 0] = 2;
            noteStateSol[4, 1] = 1;

            noteStateSol[5, 0] = 3;
            noteStateSol[5, 1] = 0;

            noteStateSol[6, 0] = 3;
            noteStateSol[6, 1] = 1;

            noteStateSol[7, 0] = 4;
            noteStateSol[7, 1] = 0;

            noteStateSol[8, 0] = 5;
            noteStateSol[8, 1] = 0;

            noteStateSol[9, 0] = 5;
            noteStateSol[9, 1] = 1;

            noteStateSol[10, 0] = 6;
            noteStateSol[10, 1] = 0;

            noteStateSol[11, 0] = 6;
            noteStateSol[11, 1] = 1;



            noteStateFa[0, 0] = 0;
            noteStateFa[0, 1] = 0;

            noteStateFa[1, 0] = 0;
            noteStateFa[1, 1] = 1;

            noteStateFa[2, 0] = 1;
            noteStateFa[2, 1] = 0;

            noteStateFa[3, 0] = 1;
            noteStateFa[3, 1] = 1;

            noteStateFa[4, 0] = 2;
            noteStateFa[4, 1] = 0;

            noteStateFa[5, 0] = 3;
            noteStateFa[5, 1] = 0;

            noteStateFa[6, 0] = 3;
            noteStateFa[6, 1] = 1;

            noteStateFa[7, 0] = 4;
            noteStateFa[7, 1] = 0;

            noteStateFa[8, 0] = 4;
            noteStateFa[8, 1] = 1;

            noteStateFa[9, 0] = 5;
            noteStateFa[9, 1] = 0;

            noteStateFa[10, 0] = 6;
            noteStateFa[10, 1] = 0;

            noteStateFa[11, 0] = 6;
            noteStateFa[11, 1] = 1;

            dicNoteName.Add("C", "Do");
            dicNoteName.Add("D", "Ré");
            dicNoteName.Add("E", "Mi");
            dicNoteName.Add("F", "Fa");
            dicNoteName.Add("G", "Sol");
            dicNoteName.Add("A", "La");
            dicNoteName.Add("B", "Si");

            brushNote = new SolidBrush[4, 2];

            brushNote[0, 0] = new SolidBrush(Color.SkyBlue);
            brushNote[0, 1] = new SolidBrush(Color.SteelBlue);

            brushNote[1, 0] = new SolidBrush(Color.FromArgb(173, 235, 135));
            brushNote[1, 1] = new SolidBrush(Color.FromArgb(93, 180, 70));

            brushNote[2, 0] = new SolidBrush(Color.FromArgb(235, 135, 206));
            brushNote[2, 1] = new SolidBrush(Color.FromArgb(180, 70, 130));

            brushNote[3, 0] = new SolidBrush(Color.FromArgb(235, 221, 135));
            brushNote[3, 1] = new SolidBrush(Color.FromArgb(178, 180, 70));

            //OpenMidiFile(@"D:\GDD\Log\Geff\NewScore\beethoven-pour-elise.mid");
            OpenMidiFile(@"D:\GDD\Log\Geff\NewScore\Debussy - Clair de lune.mid");
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
            StringBuilder partition = new StringBuilder();

            if (midiFile != null)
            {
                part += String.Format(" [ DeltaTicksPerQuarterNote : {0} ] ", midiFile.DeltaTicksPerQuarterNote);

                for (int i = 0; i < midiFile.Tracks; i++)
                {
                    Channel currentChannel = null;
                    currentChannel = new Channel();
                    hasNote = false;

                    partition.AppendLine();

                    foreach (MidiEvent midiEvent in midiFile.Events[i])
                    {
                        NoteOnEvent noteEvent = midiEvent as NoteOnEvent;

                        partition.AppendLine();

                        if (midiEvent is TimeSignatureEvent)
                        {
                            lastTimeSignature = midiEvent as TimeSignatureEvent;
                            //part += String.Format(" [ {0} ] ", midiEvent.ToString());
                            partition.AppendLine(String.Format(" [ {0} ] ", midiEvent.ToString()));
                        }
                        else if (noteEvent != null && noteEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity > 0)
                        {
                            int noteLength = 1;
                            //try
                            {
                                noteLength = noteEvent.NoteLength;
                            }
                            //catch { }

                            partition.AppendLine(String.Format(" [ AbsoluteTime {0} #  DeltaTime {1} # NoteName {2} # NoteNumber {3} # NoteLength {4}  # Channel {5}]  ", noteEvent.AbsoluteTime, noteEvent.DeltaTime, noteEvent.NoteName, noteEvent.NoteNumber, noteLength, noteEvent.Channel));

                            float typeNote = (float)noteLength / (float)midiFile.DeltaTicksPerQuarterNote;

                            string typeNoteString = String.Empty;

                            if (dicNoteType.ContainsKey(typeNote))
                                typeNoteString = dicNoteType[typeNote];

                            partition.AppendLine(String.Format(" OFF :: {0} :: {1}", typeNote, typeNoteString));

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

                                curMeasure = new Measure(nbPrevMeasure * measureLenght, measureLenght);
                                currentChannel.AddMeasure(curMeasure);
                            }

                            curMeasure.AddNote(newNote);
                            hasNote = true;
                        }
                        else
                        {
                            partition.AppendLine(String.Format(" [ {0} ] ", midiEvent.ToString()));
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

        float interlineSize = 36f;
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

            currentY = margeY;
            currentX = margeX;

            currentY -= vScrollBar1.Value * interlineSize;

            int nbPart = 1 + (int)Math.Ceiling((double)(((float)g.VisibleClipBounds.Height) / (interlineSize * ((float)music.ListChanel.Count * 6f + 1f))));

            float firstPartY = margeY;
            float noteNameY = margeY;

            double firstMeasure = 1 + vScrollBar1.Value / (music.ListChanel.Count * 6 + 1) * 4;

            for (int i = 0; i <= music.MaxMeasure / 4; i++)
            {
                bool draw = true;
                if (!(i * 4 + 1 >= firstMeasure && i * 4 + 1 <= (firstMeasure + nbPart * 4)))
                    draw = false;

                firstPartY = currentY;

                for (int j = 0; j < music.ListChanel.Count; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        currentY += interlineSize;

                        if (draw && k < 5)
                        {
                            g.DrawLine(Pens.DarkGray, new Point((int)margeX, (int)currentY), new Point((int)(g.VisibleClipBounds.Width - margeX), (int)currentY));
                        }

                    }

                    noteNameY = currentY;
                    for (int k = 0; k < noteNames.Length; k++)
                    {
                        if (draw)
                        {
                            string noteName = dicNoteName[noteNames[k]];
                            g.DrawString(noteName, this.Font, Brushes.Black, new PointF(8, (int)(noteNameY - (k + 2) * interlineSize / 2 - interlineSize / 4)));
                        }
                    }
                }

                if (draw)
                {
                    g.DrawLine(Pens.DarkGray, new Point((int)margeX, (int)(firstPartY + interlineSize)), new Point((int)margeX, (int)(currentY - 1f * interlineSize)));
                    g.DrawLine(Pens.DarkGray, new Point((int)(g.VisibleClipBounds.Width - margeX), (int)(firstPartY + interlineSize)), new Point((int)(g.VisibleClipBounds.Width - margeX), (int)(currentY - 1f * interlineSize)));
                }

                currentY += interlineSize;
            }


            //---


            for (int i = 0; i < Math.Min(4, music.ListChanel.Count); i++)
            {
                currentY = margeY + i * 6f * interlineSize - vScrollBar1.Value * interlineSize;
                Measure prevMeasure = null;

                foreach (Measure measure in music.ListChanel[i].ListMeasure)
                {
                    float nbMeasure = measure.MeasureStart / measure.MeasureLength + 1;

                    if (!(nbMeasure >= firstMeasure && nbMeasure <= (firstMeasure + nbPart * 4)))
                        continue;

                    Point pointTopMeasure = new Point((int)(((g.VisibleClipBounds.Width - 2 * margeX) / 4) * ((nbMeasure - 1) % 4) + margeX),
                        (int)margeY + (int)interlineSize + (int)((int)(i * 6f * interlineSize) +
                        ((music.ListChanel.Count * 6 + 1f) * interlineSize) * (int)((nbMeasure - 1) / 4f)) - (int)(vScrollBar1.Value * interlineSize));

                    g.DrawLine(Pens.DarkOrange, pointTopMeasure, new Point(pointTopMeasure.X, pointTopMeasure.Y + (int)(4f * interlineSize)));

                    if (i == 0)
                        g.DrawString(nbMeasure.ToString(), this.Font, Brushes.Black, new Point(pointTopMeasure.X + 5, pointTopMeasure.Y - (int)interlineSize));

                    int[,] noteState = null;

                    if (measure.BaseKey == 64)
                        noteState = noteStateSol;
                    else
                        noteState = noteStateFa;

                    foreach (Note note in measure.ListNote)
                    {
                        int noteIndex = note.NoteNumber % 12 - (measure.BaseKey == 64 ? 4 : 7);
                        if (noteIndex < 0)
                            noteIndex += 12;

                        float noteX = ((g.VisibleClipBounds.Width - 2 * margeX) / 4) / measure.MeasureLength * (float)note.AbsoluteTime;
                        float noteY = interlineSize + interlineSize / 2f * (float)(7 - noteState[noteIndex, 0]);

                        int delta = note.NoteNumber - measure.BaseKey;
                        //int div = (delta / 12);

                        if (note.NoteNumber == 37)
                        {
                            int a = 0;
                        }

                        float div = 0;
                        if(delta >= 12)
                            div =delta/12;
                        if (delta < 0)
                            div = -delta / 12 + 1;

                        noteY += -(float)Math.Sign(delta) * div * 3.5f * interlineSize;
                        noteY += interlineSize / 4f;

                        if (noteX >= g.VisibleClipBounds.Width - 2f * margeX)
                        {
                            currentY = margeY + i * 6f * interlineSize - (vScrollBar1.Value * interlineSize) + ((music.ListChanel.Count * 6 + 1f) * interlineSize) * (int)(noteX / (g.VisibleClipBounds.Width - 2f * margeX));
                            noteX %= g.VisibleClipBounds.Width - 2f * margeX;
                        }

                        Rectangle rec = new Rectangle((int)(currentX + noteX + 2), (int)(currentY + noteY + 1), (int)(((g.VisibleClipBounds.Width - 2 * currentX) / 4) / measure.MeasureLength * (float)note.NoteLength) - 2, (int)(interlineSize / 2f - 2));

                        string noteName = String.Empty;

                        if (optFrancaise.Checked)
                            noteName = dicNoteName[note.NoteName.Substring(0, 1)];
                        if (optAmericaine.Checked)
                            noteName = note.NoteName;
                        if (optNombre.Checked)
                            noteName = note.NoteNumber.ToString();

                        if (noteState[noteIndex, 1] == 1)
                        {
                            g.FillRectangle(brushNote[i, 1], rec);
                            g.DrawString(noteName, this.Font, Brushes.White, rec.Location);
                        }
                        else
                        {
                            g.FillRectangle(brushNote[i, 0], rec);
                            g.DrawString(noteName, this.Font, Brushes.Black, rec.Location);
                        }
                    }

                    if (prevMeasure == null || measure.BaseKey != prevMeasure.BaseKey || nbMeasure % 4 == 1 || (measure.NumMeasure - 1) / 4 != (prevMeasure.NumMeasure - 1) / 4)
                    {
                        Rectangle recCle = new Rectangle(pointTopMeasure.X - (int)(interlineSize / 4), pointTopMeasure.Y + (int)(((4 - measure.LocationKey) * interlineSize) - interlineSize / 4), (int)interlineSize / 2, (int)interlineSize / 2);
                        SolidBrush b = new SolidBrush(Color.FromArgb(100, 120, 120, 180));
                        g.FillEllipse(b, recCle);

                        string nameKey = measure.NameKey;

                        if (!optAmericaine.Checked)
                            nameKey = dicNoteName[nameKey];

                        g.DrawString(nameKey, this.Font, Brushes.White, recCle.Location.X + recCle.Width / 2 - g.MeasureString(nameKey, this.Font).Width / 2, recCle.Location.Y + recCle.Height / 2 - g.MeasureString(nameKey, this.Font).Height / 2);
                    }

                    prevMeasure = measure;
                }
            }

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawScore(music);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int max = music.ListChanel.Max(c => c.ListMeasure.Count);

            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = (int)((margeY + ((float)music.ListChanel.Count * 6 + 1f) * interlineSize * (float)max / 4f) / interlineSize);
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

        private void trackZom_Scroll(object sender, EventArgs e)
        {
            interlineSize = trackZom.Value;
            Form1_Resize(null, null);
            DrawScore(music);
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            int firstMeasure = 1 + vScrollBar1.Value / (music.ListChanel.Count * 6 + 1) * 4;
            this.Text = firstMeasure.ToString() + " // " + vScrollBar1.Value.ToString();
            DrawScore(music);
        }

        private void optAucune_CheckedChanged(object sender, EventArgs e)
        {
            DrawScore(music);
        }
    }
}
