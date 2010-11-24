using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using NewFlowar.Common;
using NewFlowar.Model;
using NewFlowar.Model.Minion;
using NewFlowar.Logic.AI;

namespace NewFlowar.Logic.Controller
{
    public class ControllerLogic
    {
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
        private KeyManager keyHeightMapMode;
        private KeyManager keyDetachedHexa;
        private KeyManager keyGoToFlag;

        private MouseManager mouseLeftButton;
        private MouseManager mouseMiddleButton;
        private MouseManager mouseRightButton;

        private float prevAngle;
        private float prevRayon;
        private float prevRightStickLength;
        #endregion

        private GameEngine gameEngine { get; set; }

        public ControllerLogic(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;

            this.keyLeft = new KeyManager(leftKey);
            this.keyNewMap = new KeyManager(Keys.M);
            this.keyHeightMapMode = new KeyManager(Keys.C);
            this.keyDetachedHexa = new KeyManager(Keys.D);
            this.keyGoToFlag = new KeyManager(Keys.F);

            this.keyNewMap.KeyReleased += new KeyManager.KeyReleasedHandler(keyNewMap_KeyReleased);
            this.keyHeightMapMode.KeyReleased += new KeyManager.KeyReleasedHandler(keyHeightMapMode_KeyReleased);
            this.keyDetachedHexa.KeyReleased += new KeyManager.KeyReleasedHandler(keyDetachedHexa_KeyReleased);
            this.keyGoToFlag.KeyReleased += new KeyManager.KeyReleasedHandler(keyGoToFlag_KeyReleased);

            this.mouseLeftButton = new MouseManager(MouseButtons.LeftButton);
            this.mouseMiddleButton = new MouseManager(MouseButtons.RightButton);
            this.mouseRightButton = new MouseManager(MouseButtons.RightButton);

            this.mouseLeftButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseLeftButton_MouseReleased);
            this.mouseMiddleButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseMiddleButton_MouseFirstPressed);
            this.mouseMiddleButton.MousePressed += new MouseManager.MousePressedHandler(mouseMiddleButton_MousePressed);
        }

        void keyGoToFlag_KeyReleased(Keys key, GameTime gameTime)
        {
            Context.GoToFlagActivated = !Context.GoToFlagActivated;
        }

        void keyDetachedHexa_KeyReleased(Keys key, GameTime gameTime)
        {
            Context.DetachedHexaActivated = !Context.DetachedHexaActivated;
        }

        void keyHeightMapMode_KeyReleased(Keys key, GameTime gameTime)
        {
            if (Context.ContextType == ContextType.DefineSphereHeightMap)
            {
                Context.ContextType = ContextType.None;
            }
            else
            {
                Context.ContextType = ContextType.DefineSphereHeightMap;
                gameEngine.Window.Title = "Radius : " + Context.HeightMapRadius.ToString();
            }
        }

        void keyNewMap_KeyReleased(Keys key, GameTime gameTime)
        {
            this.gameEngine.GamePlay.Map.CreateGrid();
            this.gameEngine.Render.CreateVertex();
        }

        void mouseLeftButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (gameEngine.Window.ClientBounds.Contains(new Point(mouseState.X + gameEngine.Window.ClientBounds.Left, mouseState.Y + gameEngine.Window.ClientBounds.Top)))
            {
                Context.SelectedCell = gameEngine.Render.GetSelectedCell(mouseState);

                if (Context.SelectedCell != null && Context.GoToFlagActivated)
                {
                    foreach (Player player in Context.Players)
                    {
                        foreach (MinionBase minion in player.Minions)
                        {
                            gameEngine.GamePlay.CalcMinionNewPath(minion, Context.SelectedCell);
                        }
                    }
                }
                else
                {
                    if (Context.SelectedMinion == null && Context.SelectedCell != null)
                    {
                        MinionBase selectedMinion = Context.CurrentPlayer.Minions.Find(minion => minion.CurrentCell == Context.SelectedCell);
                        Context.SelectedMinion = selectedMinion;
                    }
                    else if (Context.SelectedMinion != null && Context.SelectedCell != null)
                    {
                        gameEngine.GamePlay.CalcMinionNewPath(Context.SelectedMinion, Context.SelectedCell);

                        Context.SelectedCell = null;
                    }

                    if (Context.SelectedMinion != null && Context.SelectedCell == null)
                    {
                        Context.SelectedMinion = null;
                    }
                }
            }
        }

        void mouseMiddleButton_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            Vector2 cameraDirection = Tools.GetVector2(this.gameEngine.Render.CameraTarget) - Tools.GetVector2(this.gameEngine.Render.CameraPosition);

            prevRayon = cameraDirection.Length();
            prevAngle = Tools.GetAngle(Vector2.UnitX, cameraDirection);
        }

        void mouseMiddleButton_MousePressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            float rayon = prevRayon + (float)distance.Y / 7f;

            float angle = prevAngle + (float)distance.X / 200f;

            this.gameEngine.Render.CameraTarget = Tools.GetVector3WithoutZ(this.gameEngine.Render.CameraPosition) + new Vector3((float)Math.Cos(angle) * rayon, (float)Math.Sin(angle) * rayon, 0f);
        }

        public void UpdateBegin(GameTime gameTime)
        {
            //--- Update Mouse & Keyboard state
            mouseState = Mouse.GetState();
            keyBoardState = Keyboard.GetState();
            gamePadState = GamePad.GetState((PlayerIndex)Context.CurrentPlayer.PlayerId);
            //---

            //--- Relative mouse position
            //mousePosition = new Vector2(((float)mouseState.X - (float)this.gameEngine.GraphicsDevice.Viewport.Width / 2f) * (-this.gameEngine.Render.Zoom / 750f) + this.gameEngine.Render.VecTranslation.X, ((float)mouseState.Y - (float)this.gameEngine.GraphicsDevice.Viewport.Height / 2f) * (-this.gameEngine.Render.Zoom / 750f) + this.gameEngine.Render.VecTranslation.Y);
            mousePositionPoint = new Point(mouseState.X, mouseState.Y);
            //---

            //--- Mouse wheel
            int curMouseWheel = mouseState.ScrollWheelValue;

            if (Context.SelectedCell != null)
            {
                if (prevMouseWheel != curMouseWheel)
                {

                    if (Context.ContextType == ContextType.DefineSphereHeightMap)
                    {
                        Context.HeightMapRadius += (float)(prevMouseWheel - curMouseWheel) / 250f;
                        gameEngine.Window.Title = "Radius : " + Context.HeightMapRadius.ToString();
                    }
                    else
                    {
                        Context.SelectedCell.Height += (float)(prevMouseWheel - curMouseWheel) / 250f;
                        gameEngine.Window.Title = "Hauteur : " + Context.SelectedCell.Height.ToString();
                    }

                    gameEngine.GamePlay.Map.ElevateCell(Context.SelectedCell, Context.HeightMapRadius);
                    gameEngine.Render.CreateVertex();

                    //else
                    //{
                    //    Context.SelectedCell.Height += (float)(prevMouseWheel - curMouseWheel) / 50f;
                    //    this.gameEngine.GamePlay.Map.CalcHeightPoint(Context.SelectedCell);
                    //    gameEngine.Render.CreateVertex();
                    //    //gameEngine.Window.Title = DateTime.Now.Ticks.ToString();
                    //}
                }
            }
            //--- Zoom
            else
            {
                float estimatedZoom = gameEngine.Render.CameraPosition.Z + (prevMouseWheel - curMouseWheel) / 50f;

                if (estimatedZoom > 20)
                {
                    gameEngine.Render.CameraPosition.Z = estimatedZoom;
                    gameEngine.Render.updateViewScreen = true;
                }
            }

            if (prevMouseWheel != curMouseWheel)
            {
                prevMouseWheel = curMouseWheel;
            }
            //---

            if (keyBoardState.IsKeyDown(Keys.PageUp))
            {
                gameEngine.Render.CameraPosition.Z += 0.5f;
                gameEngine.Render.updateViewScreen = true;
            }
            if (keyBoardState.IsKeyDown(Keys.PageDown))
            {
                gameEngine.Render.CameraPosition.Z -= 0.5f;
                gameEngine.Render.updateViewScreen = true;
            }

            if (gameEngine.Render.CameraPosition.Z > 150f)
                gameEngine.Render.CameraPosition.Z = 150f;
            //---

            //--- Keyboard
            float deltaTranslation = gameEngine.Render.CameraPosition.Z / 200f;
            Vector2 vecTempTranslation = Vector2.Zero;

            if (deltaTranslation >= 0.5f)
                deltaTranslation = 0.3f;

            if (keyBoardState.IsKeyDown(Keys.LeftShift))
                deltaTranslation *= 2;

            Vector2 cameraDirection = Tools.GetVector2(this.gameEngine.Render.CameraTarget) - Tools.GetVector2(this.gameEngine.Render.CameraPosition);
            cameraDirection.Normalize();

            if (keyBoardState.IsKeyDown(upKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * cameraDirection;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(downKey))
            {
                vecTempTranslation -= deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * cameraDirection;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(rightKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * Tools.GetPerpendicularVector(cameraDirection);
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(leftKey))
            {
                vecTempTranslation -= deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * Tools.GetPerpendicularVector(cameraDirection);
                gameEngine.Render.updateViewScreen = true;
            }
            //---

            //--- Gamepad

            if (gamePadState.IsConnected)
            {
                Vector3 forward = Tools.GetVector3(cameraDirection);
                Vector3 right = Vector3.Cross(forward, Vector3.Backward);

                float rotationSpeed = deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * 0.01f;

                Quaternion qyaw = Quaternion.CreateFromAxisAngle(Vector3.Backward, -(float)gamePadState.ThumbSticks.Right.X*rotationSpeed);
                qyaw.Normalize();
                Quaternion qtilt = Quaternion.CreateFromAxisAngle(right, (float)gamePadState.ThumbSticks.Right.Y * rotationSpeed);
                qtilt.Normalize();
                Quaternion qroll = Quaternion.CreateFromAxisAngle(forward, 0f);
                qroll.Normalize();
                Quaternion yawpitch = qyaw * qtilt;// *qroll;
                yawpitch.Normalize();

                gameEngine.Render.CameraTarget = gameEngine.Render.CameraPosition + Vector3.Transform(forward, yawpitch)*10f;

                float angleCamera = Tools.GetAngle(Vector2.UnitY, cameraDirection);
                Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.Backward, angleCamera);

                vecTempTranslation += Vector2.Transform(gamePadState.ThumbSticks.Left, q) * deltaTranslation * gameTime.ElapsedGameTime.Milliseconds;
                gameEngine.Render.updateViewScreen = true;
            }
            //---

            //--- Mouse
            mouseLeftButton.Update(mouseState, gameTime);
            mouseMiddleButton.Update(mouseState, gameTime);
            mouseRightButton.Update(mouseState, gameTime);
            //---

            //---
            keyHeightMapMode.Update(keyBoardState, gameTime);
            keyHeightMapMode.Update(keyBoardState, gameTime);
            keyDetachedHexa.Update(keyBoardState, gameTime);
            keyGoToFlag.Update(keyBoardState, gameTime);
            //---

            if (gameEngine.Render.updateViewScreen)
            {
                gameEngine.Render.CameraPosition += Tools.GetVector3(vecTempTranslation);
                gameEngine.Render.CameraTarget += Tools.GetVector3(vecTempTranslation);

            }

            //--- ScreenShot
            if (keyBoardState.IsKeyDown(Keys.N))
            {
                if (!isSKeyPressed)
                    isSKeyPressed = true;
            }
            else if (isSKeyPressed)
            {
                gameEngine.Render.doScreenShot = true;
                isSKeyPressed = false;
            }
            //---

            if (keyBoardState.IsKeyDown(Keys.Space))
            {
                this.gameEngine.Exit();
            }
        }

        public void UpdateEnd(GameTime gameTime)
        {
        }
    }
}
