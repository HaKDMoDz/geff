using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using TheGrid.Model;

namespace TheGrid.Logic.Controller
{
    public class ControllerLogic
    {
        #region Constants
        private const int ZOOM_IN_MAX = 2;
        private const int ZOOM_OUT_MAX = 500;
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

        private MouseManager mouseLeftButton;
        private MouseManager mouseMiddleButton;
        private MouseManager mouseRightButton;

        private float prevRightStickLength;
        private Vector3 prevCameraPosition = Vector3.Zero;
        #endregion

        private GameEngine gameEngine { get; set; }

        public ControllerLogic(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;

            this.keyLeft = new KeyManager(leftKey);
            this.keyNewMap = new KeyManager(Keys.M);

            this.keyNewMap.KeyReleased += new KeyManager.KeyReleasedHandler(keyNewMap_KeyReleased);

            this.mouseLeftButton = new MouseManager(MouseButtons.LeftButton);
            this.mouseMiddleButton = new MouseManager(MouseButtons.RightButton);
            this.mouseRightButton = new MouseManager(MouseButtons.RightButton);

            this.mouseRightButton.MousePressed += new MouseManager.MousePressedHandler(mouseRightButton_MousePressed);
            this.mouseRightButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseRightButton_MouseFirstPressed);
        }

        void mouseRightButton_MouseFirstPressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime)
        {
            prevCameraPosition = gameEngine.Render.CameraPosition;
        }

        void mouseRightButton_MousePressed(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            if (Tools.Distance(Point.Zero, distance) > 5f)
            {
                gameEngine.Render.CameraPosition = new Vector3(prevCameraPosition.X, prevCameraPosition.Y, gameEngine.Render.CameraPosition.Z) + new Vector3(-distance.X, distance.Y, 0f) * gameEngine.Render.CameraPosition.Z / 500f; ;
                gameEngine.Render.CameraTarget = new Vector3(gameEngine.Render.CameraPosition.X, gameEngine.Render.CameraPosition.Y, 0f);
            }
        }

        void keyNewMap_KeyReleased(Keys key, GameTime gameTime)
        {
            this.gameEngine.GamePlay.Map.CreateGrid();
            this.gameEngine.Render.CreateVertex();
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

                }
            }
            //--- Zoom
            else
            {
                float estimatedZoom = gameEngine.Render.CameraPosition.Z + (float)(prevMouseWheel - curMouseWheel) / 50f;

                if (gameEngine.Render.CameraPosition.Z != estimatedZoom && estimatedZoom > ZOOM_IN_MAX && estimatedZoom < ZOOM_OUT_MAX)
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

            if (gameEngine.Render.CameraPosition.Z > ZOOM_OUT_MAX)
                gameEngine.Render.CameraPosition.Z = ZOOM_OUT_MAX;
            //---

            //--- Keyboard
            float deltaTranslation = gameEngine.Render.CameraPosition.Z / 200f;
            Vector2 vecTempTranslation = Vector2.Zero;

            if (deltaTranslation >= 0.5f)
                deltaTranslation = 0.3f;

            if (keyBoardState.IsKeyDown(Keys.LeftShift))
                deltaTranslation *= 2;

            if (keyBoardState.IsKeyDown(upKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * -Vector2.UnitY;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(downKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * Vector2.UnitY;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(rightKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * -Vector2.UnitX;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(leftKey))
            {
                vecTempTranslation += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds * Vector2.UnitX;

                gameEngine.Render.updateViewScreen = true;
            }
            //---

            //--- Gamepad
            if (gamePadState.IsConnected)
            {
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
            }
            //---

            //--- Mouse
            mouseLeftButton.Update(mouseState, gameTime);
            mouseMiddleButton.Update(mouseState, gameTime);
            mouseRightButton.Update(mouseState, gameTime);
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

        private Quaternion spacecraftRotation = Quaternion.Identity;
        private float prevAngleX = 0f;
        private float prevAngleY = 0f;

        public void UpdateEnd(GameTime gameTime)
        {
        }
    }
}
