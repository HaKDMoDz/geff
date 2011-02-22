using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using TheGrid.Model;
using TheGrid.Model.UI.Menu;
using TheGrid.Logic.Render;
using TheGrid.Model.Instrument;

namespace TheGrid.Logic.Controller
{
    public class ControllerLogic
    {
        #region Keyboard and Mouse
        private Keys upKey = Keys.Z;
        private Keys downKey = Keys.S;
        private Keys leftKey = Keys.Q;
        private Keys rightKey = Keys.D;

        public MouseState mouseState;
        public KeyboardState keyBoardState = Keyboard.GetState();
        public GamePadState gamePadState;

        public Vector2 mousePosition;
        public Point mousePositionPoint;

        //private float prevRightStickLength;
        private Vector3 prevCameraPosition = Vector3.Backward;
        #endregion

        private GameEngine GameEngine { get; set; }

        private List<KeyManager> ListKeyManager = new List<KeyManager>();
        private List<MouseManager> ListMouseManager = new List<MouseManager>();

        private KeyManager AddKey(Keys keys)
        {
            KeyManager keyManager = new KeyManager(keys);
            ListKeyManager.Add(keyManager);

            return keyManager;
        }

        private MouseManager AddMouse(MouseButtons mouseButton)
        {
            MouseManager mouseManager = new MouseManager(mouseButton);
            ListMouseManager.Add(mouseManager);

            return mouseManager;
        }

        public ControllerLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;

            KeyManager keyLeft = AddKey(leftKey);
            KeyManager keyRight = AddKey(rightKey);
            KeyManager keyUp = AddKey(upKey);
            KeyManager keyDown = AddKey(downKey);
            KeyManager keyZoomIn = AddKey(Keys.PageDown);
            KeyManager keyZoomOut = AddKey(Keys.PageUp);
            KeyManager keyMenuP = AddKey(Keys.Add);
            KeyManager keyMenuM = AddKey(Keys.Subtract);
            KeyManager keyCloneCell = AddKey(Keys.LeftControl);
            KeyManager keyStopSound = AddKey(Keys.Space);

            //KeyManager keyExit = AddKey(Keys.);
            KeyManager keyPlayPauseMusician = AddKey(Keys.P);
            KeyManager keyStopMusician = AddKey(Keys.Enter);

            KeyManager keyForward = AddKey(Keys.Right);
            KeyManager keyBackward = AddKey(Keys.Left);
            KeyManager keySpeedUp = AddKey(Keys.Down);
            KeyManager keySpeedDown = AddKey(Keys.Up);

            keyLeft.KeyPressed += new KeyManager.KeyPressedHandler(keyLeft_KeyPressed);
            keyRight.KeyPressed += new KeyManager.KeyPressedHandler(keyRight_KeyPressed);
            keyUp.KeyPressed += new KeyManager.KeyPressedHandler(keyUp_KeyPressed);
            keyDown.KeyPressed += new KeyManager.KeyPressedHandler(keyDown_KeyPressed);
            keyZoomIn.KeyPressed += new KeyManager.KeyPressedHandler(keyZoomIn_KeyPressed);
            keyZoomOut.KeyPressed += new KeyManager.KeyPressedHandler(keyZoomOut_KeyPressed);
            keyMenuP.KeyPressed += new KeyManager.KeyPressedHandler(keyMenuP_KeyPressed);
            keyMenuM.KeyPressed += new KeyManager.KeyPressedHandler(keyMenuM_KeyPressed);
            keyCloneCell.KeyReleased += new KeyManager.KeyReleasedHandler(keyCloneCell_KeyReleased);
            keyStopSound.KeyReleased += new KeyManager.KeyReleasedHandler(keyStopSound_KeyReleased);
            //keyExit.KeyReleased += new KeyManager.KeyReleasedHandler(keyExit_KeyReleased);
            keyPlayPauseMusician.KeyReleased += new KeyManager.KeyReleasedHandler(keyPlayPauseMusician_KeyReleased);
            keyStopMusician.KeyReleased += new KeyManager.KeyReleasedHandler(keyStopMusician_KeyReleased);

            keyForward.KeyPressed += new KeyManager.KeyPressedHandler(keyForward_KeyPressed);
            keyForward.KeyReleased += new KeyManager.KeyReleasedHandler(keyForward_KeyReleased);
            keyBackward.KeyPressed += new KeyManager.KeyPressedHandler(keyBackward_KeyPressed);
            keyBackward.KeyReleased += new KeyManager.KeyReleasedHandler(keyBackward_KeyReleased);
            keySpeedUp.KeyPressed += new KeyManager.KeyPressedHandler(keySpeedUp_KeyPressed);
            keySpeedDown.KeyPressed += new KeyManager.KeyPressedHandler(keySpeedDown_KeyPressed);

            MouseManager mouseRightButton = AddMouse(MouseButtons.RightButton);
            MouseManager mouseLeftButton = AddMouse(MouseButtons.LeftButton);
            MouseManager mouseMiddleButton = AddMouse(MouseButtons.MiddleButton);
            MouseManager mouseWheel = AddMouse(MouseButtons.Wheel);

            mouseRightButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseRightButton_MouseFirstPressed);
            mouseRightButton.MousePressed += new MouseManager.MousePressedHandler(mouseRightButton_MousePressed);
            mouseRightButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseRightButton_MouseReleased);

            mouseLeftButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseLeftButton_MouseReleased);

            mouseMiddleButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseMiddleButton_MouseReleased);

            mouseWheel.MouseWheelChanged += new MouseManager.MouseWheelChangedHandler(mouseWheel_MouseWheelChanged);
        }

        #region Évènement clavier
        void keyStopSound_KeyReleased(Keys key, GameTime gameTime)
        {
            GameEngine.Sound.Stop();
        }

        void keyCloneCell_KeyReleased(Keys key, GameTime gameTime)
        {
            Context.CopiedCell = null;
        }

        void keyMenuM_KeyPressed(Keys key, GameTime gameTime)
        {
            Context.MenuSize -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;

            if (Context.Map.SpeedFactor < 0f)
                Context.Map.SpeedFactor = 0f;
        }

        void keyMenuP_KeyPressed(Keys key, GameTime gameTime)
        {
            Context.MenuSize += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
        }

        void keyZoomOut_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.Render.CameraPosition.Z += 0.5f;
            GameEngine.Render.updateViewScreen = true;

            if (GameEngine.Render.CameraPosition.Z > RenderLogic.ZOOM_OUT_MAX)
                GameEngine.Render.CameraPosition.Z = RenderLogic.ZOOM_OUT_MAX;
        }

        void keyZoomIn_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.Render.CameraPosition.Z -= 0.5f;
            GameEngine.Render.updateViewScreen = true;

            if (GameEngine.Render.CameraPosition.Z < RenderLogic.ZOOM_IN_MAX)
                GameEngine.Render.CameraPosition.Z = RenderLogic.ZOOM_IN_MAX;
        }

        void keyDown_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.Render.MoveCamera(Vector2.UnitY, gameTime);
        }

        void keyUp_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.Render.MoveCamera(-Vector2.UnitY, gameTime);
        }

        void keyRight_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.Render.MoveCamera(Vector2.UnitX, gameTime);
        }

        void keyLeft_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.Render.MoveCamera(-Vector2.UnitX, gameTime);
        }

        void keyExit_KeyReleased(Keys key, GameTime gameTime)
        {
            GameEngine.Exit();
        }

        void keyNewMap_KeyReleased(Keys key, GameTime gameTime)
        {
            Context.Map.CreateGrid();
        }

        void keyBackward_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.GamePlay.Backward(gameTime);
        }

        void keyBackward_KeyReleased(Keys key, GameTime gameTime)
        {
            Context.IsNavigatingThroughTime = false;
        }

        void keyForward_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.GamePlay.Forward(gameTime);
        }

        void keyForward_KeyReleased(Keys key, GameTime gameTime)
        {
            Context.IsNavigatingThroughTime = false;
        }

        void keySpeedDown_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.GamePlay.SpeedDown(gameTime);
        }

        void keySpeedUp_KeyPressed(Keys key, GameTime gameTime)
        {
            GameEngine.GamePlay.SpeedUp(gameTime);
        }

        void keyStopMusician_KeyReleased(Keys key, GameTime gameTime)
        {
            if (Context.IsPlaying)
                GameEngine.GamePlay.Stop();
            else
                GameEngine.GamePlay.Play();
        }

        void keyPlayPauseMusician_KeyReleased(Keys key, GameTime gameTime)
        {
            if (Context.IsPlaying)
                GameEngine.GamePlay.Pause();
            else
                GameEngine.GamePlay.Play();
        }
        #endregion

        #region Évènement souris
        void mouseMiddleButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (GameEngine.UI.Ribbon.Rec.Contains(mousePositionPoint))
                return;

            Cell selectedCell = GetSelectedCell(mouseState);

            if (selectedCell != null && selectedCell.Clip != null && selectedCell.Clip.Instrument is InstrumentSample)
            {
                GameEngine.Sound.PlaySample(((InstrumentSample)selectedCell.Clip.Instrument).Sample);
            }
        }

        void mouseLeftButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (GameEngine.UI.Ribbon.Rec.Contains(mousePositionPoint))
                return;

            CircularMenu currentMenu = (CircularMenu)GameEngine.UI.ListUIComponent.Find(ui => ui is CircularMenu);

            Cell selectedCell = GetSelectedCell(mouseState);
            Context.SelectedCell = selectedCell;

            if((keyBoardState.IsKeyDown(Keys.LeftShift) && selectedCell != null))
            {
                if(selectedCell.Clip != null && selectedCell.Clip.Instrument != null)
                {
                    if(selectedCell.Clip.Instrument is InstrumentSample)
                    {
                        GameEngine.UI.OpenListSample(gameTime, selectedCell);
                    }
                    else if(selectedCell.Clip.Instrument is InstrumentEffect)
                    {
                        GameEngine.UI.OpenPannelEffect(gameTime, ((InstrumentEffect)selectedCell.Clip.Instrument).ChannelEffect, selectedCell);
                    }
                    else if(selectedCell.Clip.Instrument is InstrumentNote)
                    {
                    }
                }
            }
            else if (keyBoardState.IsKeyDown(Keys.LeftControl) && selectedCell != null)
            {
                if (Context.CopiedCell == null)
                {
                    Context.CopiedCell = selectedCell;
                }
                else
                {
                    selectedCell.Clip = null;
                    selectedCell.Channel = Context.CopiedCell.Channel;
                    if (Context.CopiedCell.Clip != null)
                    {
                        selectedCell.Clip = new Clip();
                        for (int i = 0; i < 6; i++)
                        {
                            selectedCell.Clip.Directions[i] = Context.CopiedCell.Clip.Directions[i];
                        }
                        selectedCell.Clip.Repeater = Context.CopiedCell.Clip.Repeater;
                        selectedCell.Clip.Speed = Context.CopiedCell.Clip.Speed;
                        selectedCell.Clip.Duration = Context.CopiedCell.Clip.Duration;

                        if (Context.CopiedCell.Clip.Instrument != null && !(Context.CopiedCell.Clip.Instrument is InstrumentStart))
                        {
                            if (Context.CopiedCell.Clip.Instrument is InstrumentSample)
                            {
                                selectedCell.Clip.Instrument = new InstrumentSample(((InstrumentSample)Context.CopiedCell.Clip.Instrument).Sample);
                            }
                            //TODO : cloner les effets
                            //TODO : cloner les notes
                        }
                    }

                    GameEngine.GamePlay.EvaluateMuscianGrid();
                }
            }
            else if (Context.SelectedCell != null)
            {
                //--- Ferme le précédent menu
                if (currentMenu != null)
                {
                    currentMenu.Close(gameTime);

                    //---> Supprime le menu dépendant du menu courant
                    GameEngine.UI.ListUIComponent.RemoveAll(ui => ui is CircularMenu && ui.UIDependency != null && ui.UIDependency == currentMenu);

                    //--- Créé un nouveau menu ayant comme dépendance le menu courant
                    CircularMenu newMenu = GameEngine.UI.CreateMenu(Context.SelectedCell, gameTime.TotalGameTime);
                    newMenu.Alive = true;
                    newMenu.UIDependency = currentMenu;
                    newMenu.State = MenuState.WaitDependency;

                    GameEngine.UI.ListUIComponent.Add(newMenu);
                    //---
                }
                else if (currentMenu == null || currentMenu.State == MenuState.Closing || currentMenu.State == MenuState.Closed)
                {
                    //---> Ouvre le nouveau menu
                    CircularMenu newMenu = GameEngine.UI.CreateMenu(Context.SelectedCell, gameTime.TotalGameTime);
                    newMenu.Alive = true;
                    GameEngine.UI.ListUIComponent.Add(newMenu);

                    newMenu.Open(gameTime);
                }
                //---
            }
        }

        void mouseRightButton_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            if (GameEngine.UI.Ribbon.Rec.Contains(mousePositionPoint))
                return;

            prevCameraPosition = GameEngine.Render.CameraPosition;
        }

        void mouseRightButton_MousePressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (prevCameraPosition != Vector3.Backward && Tools.Distance(Point.Zero, distance) > 5f)
            {
                GameEngine.Render.CameraPosition = new Vector3(prevCameraPosition.X, prevCameraPosition.Y, GameEngine.Render.CameraPosition.Z) + new Vector3(distance.X, distance.Y, 0f) * GameEngine.Render.CameraPosition.Z * RenderLogic.ZOOM_OUT_MAX;
                GameEngine.Render.CameraTarget = new Vector3(GameEngine.Render.CameraPosition.X, GameEngine.Render.CameraPosition.Y, 0f);
            }
        }

        void mouseRightButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            prevCameraPosition = Vector3.Backward;

            CircularMenu currentMenu = (CircularMenu)GameEngine.UI.ListUIComponent.Find(ui => ui is CircularMenu);

            if (currentMenu != null &&
                (currentMenu.State == MenuState.Opened || currentMenu.State == MenuState.Opening) &&
                Tools.Distance(Point.Zero, distance) < 5f)
            {
                if (currentMenu.Items.Count(i => i.MouseOver) > 0 && currentMenu.ParentMenu != null)
                {
                    currentMenu.UIDependency = null;
                    currentMenu.ParentMenu.UIDependency = currentMenu;
                    currentMenu.ParentMenu.Alive = true;
                    currentMenu.ParentMenu.Visible = false;
                    GameEngine.UI.ListUIComponent.Add(currentMenu.ParentMenu);
                }
                currentMenu.Close(gameTime);
            }
        }

        void mouseWheel_MouseWheelChanged(MouseState mouseState, GameTime gameTime, int prevMouseWheel)
        {
            int curMouseWheel = mouseState.ScrollWheelValue;

            float estimatedZoom = GameEngine.Render.CameraPosition.Z - (float)(prevMouseWheel - curMouseWheel) / 200f;

            if (GameEngine.Render.CameraPosition.Z != estimatedZoom && estimatedZoom > RenderLogic.ZOOM_IN_MAX && estimatedZoom < RenderLogic.ZOOM_OUT_MAX)
            {
                GameEngine.Render.CameraPosition.Z = estimatedZoom;
            }

            if (prevMouseWheel != curMouseWheel)
            {
                prevMouseWheel = curMouseWheel;
            }

            if (GameEngine.Render.CameraPosition.Z > RenderLogic.ZOOM_OUT_MAX)
                GameEngine.Render.CameraPosition.Z = RenderLogic.ZOOM_OUT_MAX;
        }
        #endregion

        public void UpdateBegin(GameTime gameTime)
        {
            //--- Update Mouse & Keyboard state
            mouseState = Mouse.GetState();
            keyBoardState = Keyboard.GetState();
            gamePadState = GamePad.GetState((PlayerIndex)Context.CurrentPlayer.PlayerId);
            //---

            //--- Relative mouse position
            mousePositionPoint = new Point(mouseState.X, mouseState.Y);
            mousePosition = new Vector2(mouseState.X, mouseState.Y);
            //---
        }

        public void UpdateEnd(GameTime gameTime)
        {
            //--- Key & Mouse Manager
            if (GameEngine.UI.IsUIModalActive())
            {
                foreach (KeyManager keyManager in ListKeyManager)
                {
                    keyManager.Reset();
                }
            }
            else
            {
                foreach (KeyManager keyManager in ListKeyManager)
                {
                    keyManager.Update(keyBoardState, gameTime);
                }
            }

            if (GameEngine.UI.IsMouseHandled() || GameEngine.UI.IsUIModalActive())
            {
                foreach (MouseManager mouseManager in ListMouseManager)
                {
                    mouseManager.Reset();
                }
            }
            else
            {
                foreach (MouseManager mouseManager in ListMouseManager)
                {
                    mouseManager.Update(mouseState, gameTime);
                }
            }
            //---

            //--- Gamepad
            if (gamePadState.IsConnected)
            {
                #region
                //float rotationSpeed = deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * 0.02f;

                //if (gamePadState.ThumbSticks.Right.Length() > 0f)
                //{
                //    Vector3 forward = Tools.GetVector3(cameraDirection);
                //    Vector3 right = Vector3.Cross(forward, Vector3.Backward);

                //    float upDownRotation = 0.0f;
                //    float leftRightRotation = 0.0f;

                //    //leftRightRotation -= gamePadState.ThumbSticks.Right.X / 50.0f;
                //    //upDownRotation += gamePadState.ThumbSticks.Right.Y / 50.0f;


                //    leftRightRotation -= gamePadState.ThumbSticks.Right.X / 20f; ;
                //    upDownRotation += gamePadState.ThumbSticks.Right.Y / 20f;

                //    Quaternion additionalRotation = Quaternion.CreateFromAxisAngle(Vector3.Backward, leftRightRotation) * Quaternion.CreateFromAxisAngle(right,
                //        upDownRotation);



                //    spacecraftRotation *=  additionalRotation;

                //    //spacecraftRotation.Y = MathHelper.Clamp(spacecraftRotation.Y, -01f, 1f);

                //    gameEngine.Render.CameraTarget = gameEngine.Render.CameraPosition + Vector3.Transform(Vector3.UnitY, spacecraftRotation);

                //    //AddToSpaceCraftPosition(new Vector3(0,0,-1));

                //    /*
                //    Vector3 forward = Tools.GetVector3(cameraDirection);
                //    Vector3 right = Vector3.Cross(forward, Vector3.Backward);

                //    prevAngleX += gamePadState.ThumbSticks.Right.X * rotationSpeed;
                //    prevAngleY += gamePadState.ThumbSticks.Right.Y * rotationSpeed;

                //    //prevAngleY = Microsoft.Xna.Framework.MathHelper.Clamp(prevAngleY, -1f, 1f);

                //    Quaternion qyaw = Quaternion.CreateFromAxisAngle(Vector3.Backward, -(float)prevAngleX);
                //    qyaw.Normalize();
                //    Quaternion qtilt = Quaternion.CreateFromAxisAngle(right, (float)prevAngleY);
                //    qtilt.Normalize();
                //    Quaternion qroll = Quaternion.CreateFromAxisAngle(forward, 0f);
                //    qroll.Normalize();
                //    Quaternion yawpitch = qyaw * qtilt * qroll;
                //    yawpitch.Normalize();

                //    gameEngine.Render.CameraTarget = gameEngine.Render.CameraPosition + Vector3.Transform(Vector3.UnitY, yawpitch);
                //     */
                //}

                //if (gamePadState.ThumbSticks.Left.Length() > 0f)
                //{
                //    float angleCamera = Tools.GetAngle(Vector2.UnitY, cameraDirection);
                //    Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.Backward, angleCamera);

                //    vecTempTranslation += Vector2.Transform(gamePadState.ThumbSticks.Left, q) * rotationSpeed * 30f;
                //    gameEngine.Render.updateViewScreen = true;
                //}
                #endregion
            }
            //---
        }

        public Cell GetSelectedCell(MouseState mouseState)
        {
            uint[] pixelData = new uint[1];

            Vector3 mousePosition = new Vector3((float)mouseState.X - (float)GameEngine.GraphicsDevice.Viewport.Width / 2f, (float)mouseState.Y - (float)GameEngine.GraphicsDevice.Viewport.Height / 2f, 0f);

            Matrix mtx = Matrix.CreateScale(-GameEngine.Render.CameraPosition.Z) * Matrix.CreateTranslation(GameEngine.Render.CameraPosition.X, GameEngine.Render.CameraPosition.Y, GameEngine.Render.CameraPosition.Z);

            mousePosition = Vector3.Transform(mousePosition, mtx);

            foreach (Cell cell in Context.Map.Cells)
            {
                Rectangle rec = new Rectangle((int)(cell.Location.X * GameEngine.Render.HexaWidth), (int)(cell.Location.Y * GameEngine.Render.HexaWidth), (int)GameEngine.Render.HexaWidth, (int)GameEngine.Render.HexaWidth);

                if (rec.Contains((int)mousePosition.X, (int)mousePosition.Y))
                {
                    GameEngine.Render.texHexa2D.GetData<uint>(0, new Rectangle((int)(mousePosition.X - (float)rec.X), (int)(mousePosition.Y - (float)rec.Y), 1, 1), pixelData, 0, 1);

                    if (((pixelData[0] & 0xFF000000) >> 24) > 20)
                        return cell;
                }
            }

            return null;
        }
    }
}
