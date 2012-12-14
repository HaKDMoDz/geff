﻿using System;
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
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Paper
{
    public partial class Form1 : Form
    {
        Graphics g;
        Graphics gBmp;
        Bitmap bmp;
        Pen penDotFar;
        Pen penDotNear;

        Scene scene = new Scene();

        ComponentBase nearestCuboid = null;
        ComponentBase _curComponent = null;

        int valMax = 0;
        int prevVScrollValue = 0;
        int prevHScrollValue = 0;

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

        int prevWidth = 0;
        int prevHeight = 0;
        Point prevLocation;
        Point initialPointMouse;
        Point initialPointMouseWithoutDelta;

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
            btnZoneFoldingH.Tag = Tools.ZoneFoldingH;
            btnZoneFoldingV.Tag = Tools.ZoneFoldingV;

            valMax = vScrollBar.Maximum - Common.depthUnity * 22 * 2;

            vScrollBar.Value = valMax;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (pic.Width == 0 || pic.Height == 0)
                return;

            g = pic.CreateGraphics();
            bmp = new Bitmap(pic.Width, pic.Height);
            gBmp = Graphics.FromImage(bmp);
            Common.lineMidScreen = new Line();
            Common.lineMidScreen.P1 = new Point(0, pic.Height * 75 / 100);
            Common.lineMidScreen.P2 = new Point(pic.Width, pic.Height * 75 / 100);
            Common.ScreenSize = new Size(pic.Width, pic.Height);

            penDotFar = new Pen(Color.White, 2f);
            penDotFar.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            penDotNear = new Pen(Color.White, 2f);
            penDotNear.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            DrawScene();
        }

        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            initialPointMouseWithoutDelta = e.Location;
            initialPointMouse = e.Location;
            initialPointMouse.Offset(-Common.Delta.X, -Common.Delta.Y);
            prevVScrollValue = vScrollBar.Value;
            prevHScrollValue = hScrollBar.Value;


            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //--- Sélection des éléments pour le déplacement et le redimensionnement
                foreach (ComponentBase component in scene.listComponent)
                {
                    if (component.ModeSelection == ModeSelection.NearMove)
                    {
                        component.ModeSelection = ModeSelection.SelectedMove;
                        curComponent = component;
                        prevLocation = component.Location;
                    }
                    else if (component.ModeSelection == ModeSelection.NearResizeWidth)
                    {
                        component.ModeSelection = ModeSelection.SelectedResizeWidth;
                        curComponent = component;
                        prevWidth = ((IResizableWidth)component).Width;
                    }
                    else if (component.ModeSelection == ModeSelection.NearResizeHeight)
                    {
                        component.ModeSelection = ModeSelection.SelectedResizeHeight;
                        curComponent = component;
                        prevHeight = ((IResizableHeight)component).Height;
                    }
                }
                //---



                //---> Création du pliage
                if (Common.CurrentTool == Tools.Folding && curComponent == null)
                {
                    curComponent = new Folding(initialPointMouse.X, initialPointMouse.Y, 50, 1);
                }

                //---> Création de la zone de pliage H
                if (Common.CurrentTool == Tools.ZoneFoldingH && curComponent == null)
                {
                    curComponent = new ZoneFoldingH(initialPointMouse.X, initialPointMouse.Y, 50);
                }

                //---> Création de la zone de pliage V
                if (Common.CurrentTool == Tools.ZoneFoldingV && curComponent == null)
                {
                    curComponent = new ZoneFoldingV(initialPointMouse.X, initialPointMouse.Y, 50);
                }

                //---> Création de la zone de déplacement H
                if (Common.CurrentTool == Tools.ZoneMovingH && curComponent == null)
                {
                    curComponent = new ZoneMovingH(initialPointMouse.X, initialPointMouse.Y, 100, 50);
                }

                //---> Création de la zone de déplacement V
                if (Common.CurrentTool == Tools.ZoneMovingV && curComponent == null)
                {
                    curComponent = new ZoneMovingV(initialPointMouse.X, initialPointMouse.Y, 50, 100);
                }

                //---> Création de la platforme
                if (Common.CurrentTool == Tools.Platform && curComponent == null)
                {
                    curComponent = new Platform(initialPointMouse.X, initialPointMouse.Y, 100, 50);
                }

                if (Common.CurrentTool == Tools.SensorButton && curComponent == null)
                {
                    curComponent = new Sensor(initialPointMouse.X, initialPointMouse.Y, SensorType.Button);
                }

                if (Common.CurrentTool == Tools.SensorCamera && curComponent == null)
                {
                    curComponent = new Sensor(initialPointMouse.X, initialPointMouse.Y, SensorType.Camera);
                }

                if (Common.CurrentTool == Tools.SensorNearness && curComponent == null)
                {
                    curComponent = new Sensor(initialPointMouse.X, initialPointMouse.Y, SensorType.Nearness);
                }

                if (Common.CurrentTool == Tools.SensorRemoteControl && curComponent == null)
                {
                    curComponent = new Sensor(initialPointMouse.X, initialPointMouse.Y, SensorType.RemoteControl);
                }

                //---> Stockage du nouvel élément
                if (Common.CurrentTool != Tools.None && curComponent != null && curComponent.ModeSelection == ModeSelection.None)
                {
                    prevLocation = curComponent.Location;
                    curComponent.ModeSelection = ModeSelection.SelectedMove;
                    curComponent.ColorIndex = Common.CurrentColorIndex;
                    scene.listComponent.Add(curComponent);
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
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                for (int i = 0; i < scene.listComponent.Count; i++)
                {
                    if (scene.listComponent[i].ModeSelection == ModeSelection.NearMove)
                    {
                        this.scene.listComponent.RemoveAt(i);
                        i--;
                    }
                }

                CalcCuboidIntersections();
                DrawScene();
            }
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            Point pointMouse = new Point(e.X, e.Y);


            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                hScrollBar.Value = Math.Min(hScrollBar.Maximum, Math.Max(hScrollBar.Minimum, prevHScrollValue + (initialPointMouseWithoutDelta.X - pointMouse.X)));
                vScrollBar.Value = Math.Min(vScrollBar.Maximum, Math.Max(vScrollBar.Minimum, prevVScrollValue + (initialPointMouseWithoutDelta.Y - pointMouse.Y)));

                this.Text = (prevHScrollValue + pointMouse.X - initialPointMouse.X).ToString();

                Common.Delta = new Point(-hScrollBar.Value, valMax - vScrollBar.Value);
                DrawScene();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left && curComponent != null)
            {
                pointMouse.Offset(-Common.Delta.X, -Common.Delta.Y);

                if (curComponent.ModeSelection == ModeSelection.SelectedMove)
                {
                    curComponent.Location = new Point(prevLocation.X + pointMouse.X - initialPointMouse.X,
                                                      prevLocation.Y + pointMouse.Y - initialPointMouse.Y);
                }
                else if (curComponent.ModeSelection == ModeSelection.SelectedResizeWidth)
                {
                    if (curResizeableWidthComponent != null)
                    {
                        curResizeableWidthComponent.Width = prevWidth + pointMouse.X - initialPointMouse.X;

                        if (curResizeableWidthComponent.Width <= 0)
                            curResizeableWidthComponent.Width = 1;
                    }
                }
                else if (curComponent.ModeSelection == ModeSelection.SelectedResizeHeight)
                {
                    if (curResizeableHeightComponent != null)
                    {
                        curResizeableHeightComponent.Height = prevHeight + pointMouse.Y - initialPointMouse.Y;

                        //(e.Y - Common.lineMidScreen.P1.Y) / Common.depthUnity;

                        if (curResizeableHeightComponent.Height <= 0)
                            curResizeableHeightComponent.Height = 1;
                    }
                }

                SortCuboid();

                CalcCuboidIntersections();

                DrawScene();
            }

            //--- Calcul des poignées de redimensionnement
            if (e.Button == System.Windows.Forms.MouseButtons.None)
            {
                int distanceNearestCuboid = int.MaxValue;

                ModeSelection nearestModeSelection = ModeSelection.None;

                foreach (ComponentBase component in scene.listComponent)
                {
                    if (component.ModeSelection != ModeSelection.SelectedMove)
                        component.ModeSelection = ModeSelection.None;

                    //--- Distance move
                    if (component.RectangleSelection.Contains(pointMouse))
                    {
                        nearestCuboid = component;
                        nearestModeSelection = ModeSelection.NearMove;
                    }
                    //---

                    //--- Distance resize
                    IResizableWidth resizeableWComponent = component as IResizableWidth;
                    IResizableHeight resizeableHComponent = component as IResizableHeight;

                    if (resizeableWComponent != null)
                    {
                        float distance = float.MaxValue / 2f;

                        //--- Calcul la distance du point à chaque segment limite de redimensionnement
                        foreach (Line lineResizeLimit in resizeableWComponent.LineResizableWidth)
                        {
                            distance = Utils.DistancePaAB(pointMouse, lineResizeLimit.P1, lineResizeLimit.P2);
                        }

                        if (distance < 10 && distance < distanceNearestCuboid)
                        {
                            nearestCuboid = component;
                            nearestModeSelection = ModeSelection.NearResizeWidth;
                        }
                    }

                    if (resizeableHComponent != null)
                    {
                        float distance = float.MaxValue / 2f;

                        //--- Calcul la distance du point à chaque segment limite de redimensionnement
                        foreach (Line lineResizeLimit in resizeableHComponent.LineResizableHeight)
                        {
                            distance = Utils.DistancePaAB(pointMouse, lineResizeLimit.P1, lineResizeLimit.P2);
                        }

                        if (distance < 10 && distance < distanceNearestCuboid)
                        {
                            nearestCuboid = component;
                            nearestModeSelection = ModeSelection.NearResizeHeight;
                        }
                    }
                    //---
                }

                if (nearestCuboid != null)
                    nearestCuboid.ModeSelection = nearestModeSelection;

                if (nearestModeSelection != ModeSelection.None)
                {
                    SortCuboid();

                    DrawScene();
                }

                if (nearestModeSelection == ModeSelection.None)
                    pic.Cursor = Cursors.Default;
                else if (nearestModeSelection == ModeSelection.NearResizeWidth)
                    pic.Cursor = Cursors.SizeWE;
                else if (nearestModeSelection == ModeSelection.NearResizeHeight)
                    pic.Cursor = Cursors.SizeNS;
                else if (nearestModeSelection == ModeSelection.NearMove)
                    pic.Cursor = Cursors.SizeAll;
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
                scene.listComponent.Sort(new CuboidComparerWithSelection());
            else
                scene.listComponent.Sort(new CuboidComparer());
        }

        private void CalcCuboidIntersections()
        {
            SortCuboid(false);

            foreach (ComponentBase component in scene.listComponent)
            {
                Folding folding = component as Folding;

                if (folding != null && folding.Height > 1)
                {
                    //Rectangle folding.RecTop = new Rectangle(folding.Location.X, folding.Location.Y - folding.Depth * Common.depthUnity, folding.Width, folding.Depth * Common.depthUnity);
                    folding.ListCutting = new List<Rectangle>();

                    foreach (ComponentBase innerfolding in scene.listComponent)
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

        private Color GetColorFromIndex(int index)
        {
            if (index == 1)
                return Color.FromArgb(129, 242, 48);
            if (index == 2)
                return Color.FromArgb(42, 211, 211);
            if (index == 3)
                return Color.FromArgb(255, 143, 51);
            if (index == 4)
                return Color.FromArgb(245, 49, 101);

            else return Color.White;
        }

        private float[][] GetScalingColorFromIndex(int index)
        {
            Color color = GetColorFromIndex(index);

            return new float[][] { 
                       new float[] {(float)color.R/255f,  0,  0,  0, 0},
                       new float[] {0,  (float)color.G/255f,  0,  0, 0},
                       new float[] {0,  0,  (float)color.B/255f,  0, 0},
                       new float[] {0,  0,  0,  1f, 0},
                       new float[] {0,  0,  0,  0f, 0},};
        }

        private void DrawScene()
        {
            gBmp.Clear(Color.FromArgb(50, 50, 50));

            SolidBrush brush = null;

            for (int i = 0; i < 22; i++)
            {
                brush = new SolidBrush(Color.FromArgb(35 + 10 * i, 35 + 10 * i, 35 + 10 * i));
                gBmp.FillRectangle(brush, Common.lineMidScreen.P1.X, Common.lineMidScreen.P1.Y + 2 * i * Common.depthUnity + Common.Delta.Y, Common.lineMidScreen.P2.X, 2 * Common.depthUnity);
            }


            //SolidBrush brush = new SolidBrush(Color.FromArgb(65, 65, 65));
            //gBmp.FillRectangle(brush, Common.lineMidScreen.P1.X, Common.lineMidScreen.P1.Y, Common.lineMidScreen.P2.X, 2 * Common.depthUnity);

            //brush = new SolidBrush(Color.FromArgb(75, 75, 75));
            //gBmp.FillRectangle(brush, Common.lineMidScreen.P1.X, Common.lineMidScreen.P1.Y + 2 * Common.depthUnity, Common.lineMidScreen.P2.X, 2 * Common.depthUnity);

            //brush = new SolidBrush(Color.FromArgb(85, 85, 85));
            //gBmp.FillRectangle(brush, Common.lineMidScreen.P1.X, Common.lineMidScreen.P1.Y + 4 * Common.depthUnity, Common.lineMidScreen.P2.X, 2 * Common.depthUnity);

            //gBmp.DrawLine(penDotFar, Common.lineMidScreen.P1, Common.lineMidScreen.P2);

            Pen pen = Pens.Black;

            foreach (ComponentBase component in scene.listComponent)
            {
                Folding folding = component as Folding;
                ZoneFoldingV zoneFoldingV = component as ZoneFoldingV;
                ZoneFoldingH zoneFoldingH = component as ZoneFoldingH;
                ZoneMovingV zoneMovingV = component as ZoneMovingV;
                ZoneMovingH zoneMovingH = component as ZoneMovingH;
                Platform platform = component as Platform;
                Sensor sensor = component as Sensor;

                pen = new Pen(GetColorFromIndex(component.ColorIndex), 5f);

                #region Folding
                if (folding != null)
                {


                    brush = new SolidBrush(Color.FromArgb(65, 65, 65));
                    int HeightFactor = folding.Height > 1 ? 2 : 1;
                    gBmp.FillRectangle(brush, folding.Location.X + Common.Delta.X, folding.Location.Y - folding.Height * Common.depthUnity + +Common.Delta.Y, folding.Width, HeightFactor * Common.depthUnity);

                    if (folding.Height > 2)
                    {
                        brush = new SolidBrush(Color.FromArgb(75, 75, 75));
                        HeightFactor = folding.Height > 3 ? 2 : 1;
                        gBmp.FillRectangle(brush, folding.Location.X + Common.Delta.X, folding.Location.Y + (2 - folding.Height) * Common.depthUnity + +Common.Delta.Y, folding.Width, HeightFactor * Common.depthUnity);
                    }

                    if (folding.Height > 4)
                    {
                        brush = new SolidBrush(Color.FromArgb(85, 85, 85));
                        HeightFactor = folding.Height > 5 ? 2 : 1;
                        gBmp.FillRectangle(brush, folding.Location.X + Common.Delta.X, folding.Location.Y + (4 - folding.Height) * Common.depthUnity + +Common.Delta.Y, folding.Width, HeightFactor * Common.depthUnity);
                    }

                    brush.Color = Color.FromArgb(brush.Color.R + 20, brush.Color.G + 20, brush.Color.B + 20);

                    gBmp.FillRectangle(brush, folding.Location.X + Common.Delta.X, folding.Location.Y + Common.Delta.Y, folding.Width, 1 * Common.depthUnity * folding.Height + Common.lineMidScreen.P1.Y - folding.Location.Y);

                    gBmp.DrawLine(pen, folding.Location.X + Common.Delta.X, folding.Location.Y - Common.depthUnity * folding.Height + Common.Delta.Y, folding.Location.X + Common.Delta.X, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height + Common.Delta.Y);
                    gBmp.DrawLine(pen, folding.Location.X + folding.Width + Common.Delta.X, folding.Location.Y - Common.depthUnity * folding.Height + Common.Delta.Y, folding.Location.X + folding.Width + Common.Delta.X, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height + Common.Delta.Y);

                    //gBmp.DrawLine(penDotNear, folding.Location.X, folding.Location.Y, folding.Location.X + folding.Width, folding.Location.Y);

                    //gBmp.DrawLine(penDotFar, folding.Location.X, folding.Location.Y - Common.depthUnity * folding.Height, folding.Location.X + folding.Width, folding.Location.Y - Common.depthUnity * folding.Height);
                    //gBmp.DrawLine(penDotFar, folding.Location.X, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height, folding.Location.X + folding.Width, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height);


                    foreach (Rectangle recCutting in folding.ListCutting)
                    {
                        gBmp.FillRectangle(Brushes.White, recCutting);

                        gBmp.DrawLine(pen, recCutting.Left + Common.Delta.X, recCutting.Top + Common.Delta.Y, recCutting.X + Common.Delta.X, recCutting.Bottom + Common.Delta.Y);
                        gBmp.DrawLine(pen, recCutting.Right + Common.Delta.X, recCutting.Top + Common.Delta.Y, recCutting.Right + Common.Delta.X, recCutting.Bottom + Common.Delta.Y);

                        Line lineFolding = new Line();
                        if (folding.Location.X < recCutting.X)
                            lineFolding.P1 = new Point(recCutting.X + Common.Delta.X, recCutting.Bottom + Common.Delta.Y);
                        else
                            lineFolding.P1 = new Point(folding.Location.X + Common.Delta.X, recCutting.Bottom + Common.Delta.Y);

                        if (folding.Location.X + folding.Width < recCutting.Right)
                            lineFolding.P2 = new Point(folding.Location.X + folding.Width + Common.Delta.X, recCutting.Bottom + Common.Delta.Y);
                        else
                            lineFolding.P2 = new Point(recCutting.Right + Common.Delta.X, recCutting.Bottom + Common.Delta.Y);

                        gBmp.DrawLine(penDotFar, lineFolding.P1, lineFolding.P2);
                    }

                    //if (folding.ModeSelection != ModeSelection.None)
                    //{
                    //    gBmp.DrawImage(Resources.Move, folding.Location.X - Resources.Move.Width / 2, folding.Location.Y - Resources.Move.Height / 2);
                    //    gBmp.DrawImage(Resources.Circle, folding.Location.X - Resources.Circle.Width / 2, folding.Location.Y - Resources.Circle.Height / 2);

                    //    gBmp.DrawImage(Resources.Resize, folding.Location.X + folding.Width - Resources.Resize.Width / 2, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height - Resources.Resize.Height / 2);
                    //    gBmp.DrawImage(Resources.Circle, folding.Location.X + folding.Width - Resources.Circle.Width / 2, Common.lineMidScreen.P1.Y + Common.depthUnity * folding.Height - Resources.Circle.Height / 2);
                    //}
                    //else
                    {
                        //gBmp.DrawRectangle(Pens.Blue, folding.Location.X - 1, folding.Location.Y - 1, 2, 2);
                    }
                }
                #endregion

                #region ZoneFolding
                if (zoneFoldingV != null)
                {
                    DrawImageInRectangle(gBmp, Resources.Grid, zoneFoldingV.Rectangle, component.ColorIndex, true, false);

                    gBmp.DrawLine(pen, zoneFoldingV.Location.X + Common.Delta.X, 0, zoneFoldingV.Location.X + Common.Delta.X, Common.ScreenSize.Height);
                    gBmp.DrawLine(pen, zoneFoldingV.Location.X + zoneFoldingV.Width + Common.Delta.X, 0, zoneFoldingV.Location.X + zoneFoldingV.Width + Common.Delta.X, Common.ScreenSize.Height);
                }
                else if (zoneFoldingH != null)
                {
                    DrawImageInRectangle(gBmp, Resources.Grid, zoneFoldingH.Rectangle, component.ColorIndex, false, true);

                    gBmp.DrawLine(pen, 0, zoneFoldingH.Location.Y + Common.Delta.Y, Common.ScreenSize.Width, zoneFoldingH.Location.Y + Common.Delta.Y);
                    gBmp.DrawLine(pen, 0, zoneFoldingH.Location.Y + zoneFoldingH.Height + Common.Delta.Y, Common.ScreenSize.Width, zoneFoldingH.Location.Y + zoneFoldingH.Height + Common.Delta.Y);
                }
                #endregion

                #region ZoneMoving
                if (zoneMovingV != null)
                {
                    DrawImageInRectangle(gBmp, Resources.GridV, zoneMovingV.RectangleSelection, component.ColorIndex, true, true);

                    gBmp.DrawRectangle(pen, zoneMovingV.RectangleSelection);
                }
                else if (zoneMovingH != null)
                {
                    DrawImageInRectangle(gBmp, Resources.GridH, zoneMovingH.RectangleSelection, component.ColorIndex, true, true);

                    gBmp.DrawRectangle(pen, zoneMovingH.RectangleSelection);
                }
                #endregion

                #region Platform
                if (platform != null)
                {
                    gBmp.DrawLine(pen, platform.Location.X + Common.Delta.X, platform.Location.Y + Common.Delta.Y, platform.Location.X + Common.Delta.X, platform.Location.Y + platform.Height + Common.Delta.Y);
                    gBmp.DrawLine(pen, platform.Location.X - 2 + Common.Delta.X, platform.Location.Y + platform.Height + Common.Delta.Y, platform.Location.X + platform.Width + 3 + Common.Delta.X, platform.Location.Y + platform.Height + Common.Delta.Y);
                    gBmp.DrawLine(pen, platform.Location.X + platform.Width + Common.Delta.X, platform.Location.Y + Common.Delta.Y, platform.Location.X + platform.Width + Common.Delta.X, platform.Location.Y + platform.Height + Common.Delta.Y);
                }
                #endregion

                #region Sensor
                if (sensor != null)
                {
                    Image sensorImage = null;

                    switch (sensor.SensorType)
                    {
                        case SensorType.Button:
                            sensorImage = Resources.Icon_SensorButton;
                            break;
                        case SensorType.Camera:
                            sensorImage = Resources.Icon_SensorCamera;
                            break;
                        case SensorType.Nearness:
                            sensorImage = Resources.Icon_SensorNearness;
                            break;
                        case SensorType.RemoteControl:
                            sensorImage = Resources.Icon_SensorRemoteControl;
                            break;
                        default:
                            break;
                    }

                    DrawImageInRectangle(gBmp, sensorImage, sensor.RectangleSelection, component.ColorIndex, true, true);
                }
                #endregion

                if (component.ModeSelection != ModeSelection.None)
                {
                    pen.Color = Color.White;
                    pen.DashStyle = DashStyle.Dot;
                    pen.Width = 7f;

                    gBmp.DrawRectangle(pen, component.RectangleSelection);
                }
            }

            g.DrawImage(bmp, 0, 0);
        }

        private void DrawImageInRectangle(Graphics g, Image image, Rectangle rectangle, int indexColor, bool useScrollingH, bool useScrollingV)
        {
            //int deltaX = -hScrollBar.Value;
            //int deltaY = 8000 - vScrollBar.Value;

            //if (!useScrollingH)
            //    deltaX = 0;

            //if (!useScrollingV)
            //    deltaY = 0;

            float[][] colorMatrixElements = GetScalingColorFromIndex(indexColor);

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

            for (int i = 0; i < (int)Math.Ceiling((double)rectangle.Width / (double)image.Width); i++)
            {
                for (int j = 0; j < (int)Math.Ceiling((double)rectangle.Height / (double)image.Height); j++)
                {
                    Rectangle rec = new Rectangle(rectangle.X + i * image.Width, rectangle.Y + j * image.Height, Math.Min(image.Width, rectangle.Width), Math.Min(image.Height, rectangle.Height));

                    if ((i + 1) * rec.Width > rectangle.Width)
                        rec.Width -= (i + 1) * rec.Width - rectangle.Width;

                    if ((j + 1) * rec.Height > rectangle.Height)
                        rec.Height -= (j + 1) * rec.Height - rectangle.Height;

                    g.DrawImage(image, rec, 0f, 0f, rec.Width, rec.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //---> Suppression du cuboid courant
            if (e.KeyCode == Keys.Delete && curComponent != null)
            {
                this.scene.listComponent.Remove(curComponent);
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
            btnZoneFoldingV.Checked = false;
            btnZoneFoldingH.Checked = false;
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
            scene.listComponent = new List<ComponentBase>();
            DrawScene();
        }

        private void btnOuvrir_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Papier (*.ppr)|*.ppr";

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Scene), new Type[] 
                    { typeof(ComponentBase), typeof(Folding), typeof(Link), typeof(Platform), typeof(Sensor), typeof(ZoneFoldingH), typeof(ZoneFoldingV), typeof(ZoneMovingH) , typeof(ZoneMovingV)
                    });

                    XmlReader reader = new XmlTextReader(dlg.FileName);

                    scene = (Scene)serializer.Deserialize(reader);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Papier (*.ppr)|*.ppr";

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Scene), new Type[] 
                    { typeof(ComponentBase), typeof(Folding), typeof(Link), typeof(Platform), typeof(Sensor), typeof(ZoneFoldingH), typeof(ZoneFoldingV), typeof(ZoneMovingH) , typeof(ZoneMovingV)
                    });

                    XmlWriter writer = new XmlTextWriter(dlg.FileName, Encoding.UTF8);
                    serializer.Serialize(writer, scene);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Common.CurrentColorIndex = toolStripComboBox1.SelectedIndex + 1;

            if (nearestCuboid != null)
            {
                nearestCuboid.ColorIndex = Common.CurrentColorIndex;
                DrawScene();
            }
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Common.Delta = new Point(Common.Delta.X, valMax - vScrollBar.Value);
            DrawScene();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Common.Delta = new Point(-hScrollBar.Value, Common.Delta.Y);
            DrawScene();
        }
    }
}
