using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Paper.Properties;
using Paper.Model;

namespace Paper
{
    public partial class Form1 : Form
    {
        Graphics g;
        Graphics gBmp;
        Bitmap bmp;
        Pen penDotFar;
        Pen penDotNear;

        List<Rec> listRec = new List<Rec>();
        List<ComponentBase> listComponent = new List<ComponentBase>();

        //---
        ComponentBase _curComponent = null;

        ComponentBase curComponent
        {
            get
            {
                return _curComponent;
            }
            set
            {
                _curComponent = value;

                curResizeableWidthComponent = _curComponent as IResizableWidth;
                curResizeableHeightComponent = _curComponent as IResizableHeight;
            }
        }

        IResizableWidth curResizeableWidthComponent = null;
        IResizableHeight curResizeableHeightComponent = null;
        //---

        public Form1()
        {
            InitializeComponent();

            Form1_Resize(null, null);

            InitForm();
        }

        private void InitForm()
        {
            btnFolding.Tag = Tools.Folding;
            btnLink.Tag = Tools.Link;
            btnMovingZoneH.Tag = Tools.ZoneMovingH;
            btnMovingzoneV.Tag = Tools.ZoneMovingV;
            btnPlatform.Tag = Tools.Platform;
            btnSensorButton.Tag = Tools.SensorButton;
            btnSensorCamera.Tag = Tools.SensorCamera;
            btnSensorNearness.Tag = Tools.SensorNearness;
            btnSensorRemoteControl.Tag = Tools.SensorRemoteControl;
            btnZoneCuttingH.Tag = Tools.ZoneCuttingH;
            btnzoneCuttingV.Tag = Tools.ZoneMovingV;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            g = pic.CreateGraphics();
            bmp = new Bitmap(pic.Width, pic.Height);
            gBmp = Graphics.FromImage(bmp);
            Common.lineMidScreen = new Line();
            Common.lineMidScreen.P1 = new Point(0, pic.Height * 75 / 100);
            Common.lineMidScreen.P2 = new Point(pic.Width, pic.Height * 75 / 100);

            penDotFar = new Pen(Color.Green, 1);
            penDotFar.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            penDotNear = new Pen(Color.Blue, 1);
            penDotNear.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            DrawScene();
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //--- Sélection des cuboids pour le déplacement et le redimensionnement
                foreach (Folding cuboid in listComponent)
                {
                    if (cuboid.ModeSelection == ModeSelection.NearMove)
                    {
                        cuboid.ModeSelection = ModeSelection.SelectedMove;
                        curComponent = cuboid;
                    }

                    if (cuboid.ModeSelection == ModeSelection.NearResize)
                    {
                        cuboid.ModeSelection = ModeSelection.SelectedResize;
                        curComponent = cuboid;
                    }
                }
                //---

                //---> Création du cuboid
                if (Common.CurrentTool == Tools.Folding && curComponent == null)
                {
                    curComponent = new Folding(e.X, e.Y, 50, 1);
                }

                if (Common.CurrentTool == Tools.ZoneCuttingH && curComponent == null)
                {
                    curComponent = new ZoneCutting(e.X, e.Y, ZoneCuttingType.Horizontal);
                }

                if (Common.CurrentTool == Tools.zonecuttingV && curComponent == null)
                {
                    curComponent = new ZoneCutting(e.X, e.Y, ZoneCuttingType.Vertical);
                }

                if (Common.CurrentTool != Tools.None)
                {
                    curComponent.ModeSelection = ModeSelection.SelectedMove;
                    listComponent.Add(curComponent);
                    SortCuboid();
                }

                DrawScene();
            }
        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        {
            //---> Plus de cuboid actif lors du relâchement du bouton
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                curComponent = null;
            }

            //---> Suppression du cuboid proche
            if (Common.CurrentTool == Tools.Folding && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                for (int i = 0; i < listComponent.Count; i++)
                {
                    if (listComponent[i].ModeSelection == ModeSelection.NearMove)
                    {
                        this.listComponent.RemoveAt(i);
                        i--;
                    }
                }

                CalcCuboidIntersections();
                DrawScene();
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && curComponent != null)
            {
                if (curComponent.ModeSelection == ModeSelection.SelectedMove)
                    curComponent.Location = new Point(e.X, e.Y);

                if (curComponent.ModeSelection == ModeSelection.SelectedResize)
                {
                    if (curResizeableWidthComponent != null)
                        curResizeableWidthComponent.Width = e.X - curComponent.Location.X;

                    if (curResizeableHeightComponent != null)
                    {
                        curResizeableHeightComponent.Height = (e.Y - Common.lineMidScreen.P1.Y) / Common.depthUnity;

                        if (curResizeableHeightComponent.Height == 0)
                            curResizeableHeightComponent.Height = 1;
                    }

                    SortCuboid();
                }

                CalcCuboidIntersections();

                DrawScene();
            }

            //--- Calcul des poignées de redimensionnement
            if ((Common.CurrentTool == Tools.None || Common.CurrentTool == Tools.Folding) && e.Button == System.Windows.Forms.MouseButtons.None)
            {
                int distanceNearestCuboid = int.MaxValue;

                ComponentBase nearestCuboid = null;
                ModeSelection nearestModeSelection = ModeSelection.None;

                foreach (ComponentBase component in listComponent)
                {
                    if (component.ModeSelection != ModeSelection.SelectedMove)
                        component.ModeSelection = ModeSelection.None;

                    //--- Distance move
                    int distance = Utils.Distance(new Point(e.X, e.Y), component.Location);

                    if (distance < 10 && distance < distanceNearestCuboid)
                    {
                        nearestCuboid = component;
                        nearestModeSelection = ModeSelection.NearMove;
                    }
                    //---

                    //--- Distance resize
                    IResizableWidth resizeableWComponent = component as IResizableWidth;
                    IResizableHeight resizeableHComponent = component as IResizableHeight;

                    if (resizeableWComponent != null && resizeableHComponent != null)
                    {
                        distance = Utils.Distance(new Point(e.X, e.Y), new Point(component.Location.X + resizeableWComponent.Width, Common.lineMidScreen.P1.Y + Common.depthUnity * resizeableHComponent.Height));

                        if (distance < 10 && distance < distanceNearestCuboid)
                        {
                            nearestCuboid = component;
                            nearestModeSelection = ModeSelection.NearResize;
                        }
                    }
                    //---
                }

                if (nearestCuboid != null)
                    nearestCuboid.ModeSelection = nearestModeSelection;

                SortCuboid();

                DrawScene();
            }
            //---
        }

        private void SortCuboid()
        {
            SortCuboid(true);
        }

        private void SortCuboid(bool handleSelection)
        {
            if (handleSelection)
                listComponent.Sort(new CuboidComparerWithSelection());
            else
                listComponent.Sort(new CuboidComparer());
        }

        private void CalcCuboidIntersections()
        {
            SortCuboid(false);

            foreach (ComponentBase component in listComponent)
            {
                Folding folding = component as Folding;

                if (folding != null && folding.Height > 1)
                {
                    //Rectangle folding.RecTop = new Rectangle(folding.Location.X, folding.Location.Y - folding.Depth * Common.depthUnity, folding.Width, folding.Depth * Common.depthUnity);
                    folding.ListCutting = new List<Rectangle>();

                    foreach (ComponentBase innerfolding in listComponent)
                    {
                        Folding innerFolding = component as Folding;

                        if (innerFolding.Height < folding.Height && innerFolding.Location.Y - innerFolding.Height * Common.depthUnity < folding.RecTop.Y)
                        {
                            Rectangle result = innerFolding.RecFace;

                            result.Intersect(folding.RecTop);

                            if (result.Width * result.Height > 0)
                            {
                                result.X = innerFolding.Location.X;

                                if (innerFolding.RecFace.Y > folding.RecTop.Y)
                                    result.Height = folding.RecTop.Y - innerFolding.RecTop.Y;
                                else
                                    result.Height = innerFolding.Height * Common.depthUnity;

                                result.Width = innerFolding.Width;

                                folding.ListCutting.Add(result);
                            }
                        }
                    }
                }
            }
        }



        private void DrawScene()
        {
            gBmp.Clear(Color.White);

            SolidBrush brush = new SolidBrush(Color.FromArgb(91, 177, 255));
            gBmp.FillRectangle(brush, Common.lineMidScreen.P1.X, Common.lineMidScreen.P1.Y, Common.lineMidScreen.P2.X, 2 * Common.depthUnity);

            brush = new SolidBrush(Color.FromArgb(128, 194, 255));
            gBmp.FillRectangle(brush, Common.lineMidScreen.P1.X, Common.lineMidScreen.P1.Y + 2 * Common.depthUnity, Common.lineMidScreen.P2.X, 2 * Common.depthUnity);

            brush = new SolidBrush(Color.FromArgb(181, 219, 255));
            gBmp.FillRectangle(brush, Common.lineMidScreen.P1.X, Common.lineMidScreen.P1.Y + 4 * Common.depthUnity, Common.lineMidScreen.P2.X, 2 * Common.depthUnity);

            gBmp.DrawLine(penDotFar, Common.lineMidScreen.P1, Common.lineMidScreen.P2);

            Pen pen = Pens.Black;

            foreach (ComponentBase component in listComponent)
            {
                Folding folding = component as Folding;
                ZoneCutting zoneCutting = component as ZoneCutting;
                Sensor sensor = component as Sensor;

                #region Folding
                if (folding != null)
                {

                    gBmp.FillRectangle(Brushes.White, folding.Location.X, folding.Location.Y - Common.depthUnity * folding.Height, folding.Width, 2 * Common.depthUnity * folding.Height + Common.lineMidScreen.P1.Y - folding.Location.Y);

                    brush = new SolidBrush(Color.FromArgb(91, 177, 255));
                    int HeightFactor = folding.Height > 1 ? 2 : 1;
                    gBmp.FillRectangle(brush, folding.Location.X, folding.Location.Y - folding.Height * Common.depthUnity, folding.Width, HeightFactor * Common.depthUnity);

                    if (folding.Height > 2)
                    {
                        brush = new SolidBrush(Color.FromArgb(128, 194, 255));
                        HeightFactor = folding.Height > 3 ? 2 : 1;
                        gBmp.FillRectangle(brush, folding.Location.X, folding.Location.Y + (2 - folding.Height) * Common.depthUnity, folding.Width, HeightFactor * Common.depthUnity);
                    }

                    if (folding.Height > 4)
                    {
                        brush = new SolidBrush(Color.FromArgb(181, 219, 255));
                        HeightFactor = folding.Height > 5 ? 2 : 1;
                        gBmp.FillRectangle(brush, folding.Location.X, folding.Location.Y + (4 - folding.Height) * Common.depthUnity, folding.Width, HeightFactor * Common.depthUnity);
                    }

                    gBmp.DrawLine(pen, folding.Location.X, folding.Location.Y - Common.depthUnity * folding.Height, folding.Location.X, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height);
                    gBmp.DrawLine(pen, folding.Location.X + folding.Width, folding.Location.Y - Common.depthUnity * folding.Height, folding.Location.X + folding.Width, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height);

                    gBmp.DrawLine(penDotNear, folding.Location.X, folding.Location.Y, folding.Location.X + folding.Width, folding.Location.Y);

                    gBmp.DrawLine(penDotFar, folding.Location.X, folding.Location.Y - Common.depthUnity * folding.Height, folding.Location.X + folding.Width, folding.Location.Y - Common.depthUnity * folding.Height);
                    gBmp.DrawLine(penDotFar, folding.Location.X, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height, folding.Location.X + folding.Width, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height);


                    foreach (Rectangle recCutting in folding.ListCutting)
                    {
                        gBmp.FillRectangle(Brushes.White, recCutting);

                        gBmp.DrawLine(pen, recCutting.Left, recCutting.Top, recCutting.X, recCutting.Bottom);
                        gBmp.DrawLine(pen, recCutting.Right, recCutting.Top, recCutting.Right, recCutting.Bottom);

                        Line lineFolding = new Line();
                        if (folding.Location.X < recCutting.X)
                            lineFolding.P1 = new Point(recCutting.X, recCutting.Bottom);
                        else
                            lineFolding.P1 = new Point(folding.Location.X, recCutting.Bottom);

                        if (folding.Location.X + folding.Width < recCutting.Right)
                            lineFolding.P2 = new Point(folding.Location.X + folding.Width, recCutting.Bottom);
                        else
                            lineFolding.P2 = new Point(recCutting.Right, recCutting.Bottom);

                        gBmp.DrawLine(penDotFar, lineFolding.P1, lineFolding.P2);
                    }

                    if (folding.ModeSelection != ModeSelection.None)
                    {
                        gBmp.DrawImage(Resources.Move, folding.Location.X - Resources.Move.Width / 2, folding.Location.Y - Resources.Move.Height / 2);
                        gBmp.DrawImage(Resources.Circle, folding.Location.X - Resources.Circle.Width / 2, folding.Location.Y - Resources.Circle.Height / 2);

                        gBmp.DrawImage(Resources.Resize, folding.Location.X + folding.Width - Resources.Resize.Width / 2, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height - Resources.Resize.Height / 2);
                        gBmp.DrawImage(Resources.Circle, folding.Location.X + folding.Width - Resources.Circle.Width / 2, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height - Resources.Circle.Height / 2);
                    }
                    else
                    {
                        gBmp.DrawRectangle(Pens.Blue, folding.Location.X - 1, folding.Location.Y - 1, 2, 2);
                    }
                }
                #endregion

                #region ZoneCutting
                #endregion

                #region Sensor
                #endregion


            }

            g.DrawImage(bmp, 0, 0);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //---> Suppression du cuboid courant
            if (e.KeyCode == Keys.Delete && curComponent != null)
            {
                this.listComponent.Remove(curComponent);
                curComponent = null;
                DrawScene();
            }
        }

        private void UncheckAllButtons()
        {
            btnFolding.Checked = false;
            btnLink.Checked = false;
            btnMovingZoneH.Checked = false;
            btnMovingzoneV.Checked = false;
            btnPlatform.Checked = false;
            btnSensorButton.Checked = false;
            btnSensorCamera.Checked = false;
            btnSensorNearness.Checked = false;
            btnSensorRemoteControl.Checked = false;
            btnZoneCuttingH.Checked = false;
            btnzoneCuttingV.Checked = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ToolStripButton toolStripItem = ((ToolStripButton)sender);
            bool isChecked = toolStripItem.Checked;
            UncheckAllButtons();

            toolStripItem.Checked = isChecked;

            if (isChecked)
            {
                Common.CurrentTool = ((Tools)toolStripItem.Tag);
            }
            else
            {
                Common.CurrentTool = Tools.None;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //---> RAZ de la scène
            listComponent = new List<ComponentBase>();
            DrawScene();
        }

        private void btnOuvrir_Click(object sender, EventArgs e)
        {

        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {

        }
    }
}
