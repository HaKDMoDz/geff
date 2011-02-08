using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using TheGrid.Model;
using TheGrid.Model.UI.Menu;

namespace TheGrid.Logic.Controller
{
    public class ControllerLogic
    {
        #region Constants
        private const float ZOOM_IN_MAX = -30f;
        private const float ZOOM_OUT_MAX = -1f;
        #endregion

        #region Keyboard and Mouse
        private Keys upKey = Keys.Z;
        private Keys downKey = Keys.S;
        private Keys leftKey = Keys.Q;
        private Keys rightKey = Keys.D;

        private int prevMouseWheel = 0;

        public MouseState mouseState;
        public KeyboardState keyBoardState = Keyboard.GetState();
        public GamePadState gamePadState;

        public Vector2 mousePosition;
        public Point mousePositionPoint;

        private bool isPKeyPressed = false;
        private bool isSKeyPressed = false;

        private KeyManager keyLeft;
        private KeyManager keyRight;
        private KeyManager keyUp;
        private KeyManager keyDown;
        private KeyManager keyNewMap;
        private KeyManager keySaveMap;
        private KeyManager keyEvaluateMusicianPartition;
        private KeyManager keyPlayPauseMusician;
        private KeyManager keyStopMusician;

        private KeyManager keyForward;
        private KeyManager keyBackward;
        private KeyManager keySpeedUp;
        private KeyManager keySpeedDown;

        private MouseManager mouseLeftButton;
        private MouseManager mouseMiddleButton;
        private MouseManager mouseRightButton;

        private float prevRightStickLength;
        private Vector3 prevCameraPosition = Vector3.Zero;
        #endregion

        private GameEngine GameEngine { get; set; }

        public ControllerLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;

            this.keyLeft = new KeyManager(leftKey);
            this.keyNewMap = new KeyManager(Keys.N);
            this.keySaveMap = new KeyManager(Keys.S);
            this.keyEvaluateMusicianPartition = new KeyManager(Keys.C);
            this.keyPlayPauseMusician = new KeyManager(Keys.P);
            this.keyStopMusician = new KeyManager(Keys.Enter);
            this.keyForward = new KeyManager(Keys.Right);
            this.keyBackward = new KeyManager(Keys.Left);
            this.keySpeedUp = new KeyManager(Keys.Down);
            this.keySpeedDown = new KeyManager(Keys.Up);

            this.keyNewMap.KeyReleased += new KeyManager.KeyReleasedHandler(keyNewMap_KeyReleased);
            this.keySaveMap.KeyReleased += new KeyManager.KeyReleasedHandler(keySaveMap_KeyReleased);
            this.keyEvaluateMusicianPartition.KeyReleased += new KeyManager.KeyReleasedHandler(keyEvaluateMusicianPartition_KeyReleased);
            this.keyPlayPauseMusician.KeyReleased += new KeyManager.KeyReleasedHandler(keyPlayPauseMusician_KeyReleased);
            this.keyStopMusician.KeyReleased += new KeyManager.KeyReleasedHandler(keyStopMusician_KeyReleased);

            this.keyForward.KeyPressed += new KeyManager.KeyPressedHandler(keyForward_KeyPressed);
            this.keyForward.KeyReleased += new KeyManager.KeyReleasedHandler(keyForward_KeyReleased);
            this.keyBackward.KeyPressed += new KeyManager.KeyPressedHandler(keyBackward_KeyPressed);
            this.keyBackward.KeyReleased += new KeyManager.KeyReleasedHandler(keyBackward_KeyReleased);
            this.keySpeedUp.KeyPressed += new KeyManager.KeyPressedHandler(keySpeedUp_KeyPressed);
            this.keySpeedDown.KeyPressed += new KeyManager.KeyPressedHandler(keySpeedDown_KeyPressed);

            this.mouseLeftButton = new MouseManager(MouseButtons.LeftButton);
            this.mouseMiddleButton = new MouseManager(MouseButtons.RightButton);
            this.mouseRightButton = new MouseManager(MouseButtons.RightButton);

            this.mouseRightButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseRightButton_MouseFirstPressed);
            this.mouseRightButton.MousePressed += new MouseManager.MousePressedHandler(mouseRightButton_MousePressed);
            this.mouseRightButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseRightButton_MouseReleased);

            this.mouseLeftButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseLeftButton_MouseReleased);
        }

        void keySaveMap_KeyReleased(Keys key, GameTime gameTime)
        {
            FileSystem.SaveLevel(Context.Map, "Test-" + Guid.NewGuid().ToString());
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
            if(Context.IsPlaying)
                GameEngine.GamePlay.Pause();
            else
                GameEngine.GamePlay.Play();
        }

        void keyEvaluateMusicianPartition_KeyReleased(Keys key, GameTime gameTime)
        {
            GameEngine.GamePlay.EvaluateMuscianGrid();
        }

        void mouseLeftButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            Menu currentMenu = (Menu)GameEngine.UI.ListUIComponent.Find(ui => ui is Menu);

            if (currentMenu != null && currentMenu.State == MenuState.Opened && currentMenu.Items.Exists(item => item.MouseOver))
            {
                currentMenu.MouseClick(gameTime);
                GameEngine.GamePlay.EvaluateMuscianGrid();
            }
            else
            {
                Cell selectedCell = GetSelectedCell(mouseState);

                Context.SelectedCell = selectedCell;

                if (Context.SelectedCell != null)
                {
                    //--- Ferme le précédent menu
                    if (currentMenu != null)
                    {
                        currentMenu.Close(gameTime);

                        //---> Supprime le menu dépendant du menu courant
                        GameEngine.UI.ListUIComponent.RemoveAll(ui => ui is Menu && ui.UIDependency != null && ui.UIDependency == currentMenu);

                        //--- Créé un nouveau menu ayant comme dépendance le menu courant
                        Menu newMenu = GameEngine.UI.CreateMenu(Context.SelectedCell);
                        newMenu.Alive = true;
                        newMenu.UIDependency = currentMenu;
                        newMenu.State = MenuState.WaitDependency;

                        GameEngine.UI.ListUIComponent.Add(newMenu);
                        //---
                    }
                    else if (currentMenu == null || currentMenu.State == MenuState.Closing || currentMenu.State == MenuState.Closed)
                    {
                        //---> Ouvre le nouveau menu
                        Menu newMenu = GameEngine.UI.CreateMenu(Context.SelectedCell);
                        newMenu.Alive = true;
                        GameEngine.UI.ListUIComponent.Add(newMenu);

                        newMenu.Open(gameTime);
                    }
                    //---
                }
            }
        }

        void mouseRightButton_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            prevCameraPosition = GameEngine.Render.CameraPosition;
        }

        void mouseRightButton_MousePressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (Tools.Distance(Point.Zero, distance) > 5f)
            {
                GameEngine.Render.CameraPosition = new Vector3(prevCameraPosition.X, prevCameraPosition.Y, GameEngine.Render.CameraPosition.Z) + new Vector3(distance.X, distance.Y, 0f) * GameEngine.Render.CameraPosition.Z * ZOOM_OUT_MAX;
                GameEngine.Render.CameraTarget = new Vector3(GameEngine.Render.CameraPosition.X, GameEngine.Render.CameraPosition.Y, 0f);
            }
        }

        void mouseRightButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            Menu currentMenu = (Menu)GameEngine.UI.ListUIComponent.Find(ui => ui is Menu);

            if (currentMenu != null && 
                (currentMenu.State == MenuState.Opened || currentMenu.State == MenuState.Opening) && 
                Tools.Distance(Point.Zero, distance) < 5f)
            {
                if (currentMenu.ParentMenu != null)
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

        void keyNewMap_KeyReleased(Keys key, GameTime gameTime)
        {
            Context.Map.CreateGrid();
        }

        public void UpdateBegin(GameTime gameTime)
        {
            //--- Update Mouse & Keyboard state
            mouseState = Mouse.GetState();
            keyBoardState = Keyboard.GetState();
            gamePadState = GamePad.GetState((PlayerIndex)Context.CurrentPlayer.PlayerId);
            //---

            //--- Relative mouse position
            mousePositionPoint = new Point(mouseState.X, mouseState.Y);
            //---

            //--- Mouse wheel
            int curMouseWheel = mouseState.ScrollWheelValue;

            float estimatedZoom = GameEngine.Render.CameraPosition.Z - (float)(prevMouseWheel - curMouseWheel) / 200f;

            if (GameEngine.Render.CameraPosition.Z != estimatedZoom && estimatedZoom > ZOOM_IN_MAX && estimatedZoom < ZOOM_OUT_MAX)
            {
                GameEngine.Render.CameraPosition.Z = estimatedZoom;
                GameEngine.Render.updateViewScreen = true;
            }

            if (prevMouseWheel != curMouseWheel)
            {
                prevMouseWheel = curMouseWheel;
            }
            //---

            //--- Zoom clavier
            if (keyBoardState.IsKeyDown(Keys.PageUp))
            {
                GameEngine.Render.CameraPosition.Z += 0.5f;
                GameEngine.Render.updateViewScreen = true;
            }
            if (keyBoardState.IsKeyDown(Keys.PageDown))
            {
                GameEngine.Render.CameraPosition.Z -= 0.5f;
                GameEngine.Render.updateViewScreen = true;
            }

            if (GameEngine.Render.CameraPosition.Z > ZOOM_OUT_MAX)
                GameEngine.Render.CameraPosition.Z = ZOOM_OUT_MAX;
            //---

            //--- Keyboard
            float deltaTranslation = GameEngine.Render.CameraPosition.Z / 200f;
            Vector2 vecTempTranslation = Vector2.Zero;

            if (deltaTranslation >= 0.5f)
                deltaTranslation = 0.3f;

            if (keyBoardState.IsKeyDown(Keys.LeftShift))
                deltaTranslation *= 2;

            if (keyBoardState.IsKeyDown(upKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * -Vector2.UnitY;
                GameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(downKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * Vector2.UnitY;
                GameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(rightKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * -Vector2.UnitX;
                GameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(leftKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * Vector2.UnitX;

                GameEngine.Render.updateViewScreen = true;
            }

            keyNewMap.Update(keyBoardState, gameTime);
            keySaveMap.Update(keyBoardState, gameTime);
            keyEvaluateMusicianPartition.Update(keyBoardState, gameTime);
            keyPlayPauseMusician.Update(keyBoardState, gameTime);
            keyStopMusician.Update(keyBoardState, gameTime);
            keyForward.Update(keyBoardState, gameTime);
            keyBackward.Update(keyBoardState, gameTime);
            keySpeedUp.Update(keyBoardState, gameTime);
            keySpeedDown.Update(keyBoardState, gameTime);
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

            //--- Mouse
            mouseLeftButton.Update(mouseState, gameTime);
            mouseMiddleButton.Update(mouseState, gameTime);
            mouseRightButton.Update(mouseState, gameTime);
            //---

            //---
            //Menu currentMenu = (Menu)GameEngine.UI.ListUIComponent.Find(ui => ui is Menu);
            //if (currentMenu != null)
            //{
            //    currentMenu.MouseOver(GameEngine, gameTime, mouseState);
            //}
            //---

            if (GameEngine.Render.updateViewScreen)
            {
                GameEngine.Render.CameraPosition += Tools.GetVector3(vecTempTranslation);
                GameEngine.Render.CameraTarget += Tools.GetVector3(vecTempTranslation);
            }

            //--- ScreenShot
            if (keyBoardState.IsKeyDown(Keys.N))
            {
                if (!isSKeyPressed)
                    isSKeyPressed = true;
            }
            else if (isSKeyPressed)
            {
                GameEngine.Render.doScreenShot = true;
                isSKeyPressed = false;
            }
            //---

            if (keyBoardState.IsKeyDown(Keys.Space))
            {
                this.GameEngine.Exit();
            }
        }

        public void UpdateEnd(GameTime gameTime)
        {
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
