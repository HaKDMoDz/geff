using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TheGrid.Common;
using TheGrid.Model;
using TheGrid.Model.Menu;

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
            this.keyNewMap = new KeyManager(Keys.M);

            this.keyNewMap.KeyReleased += new KeyManager.KeyReleasedHandler(keyNewMap_KeyReleased);

            this.mouseLeftButton = new MouseManager(MouseButtons.LeftButton);
            this.mouseMiddleButton = new MouseManager(MouseButtons.RightButton);
            this.mouseRightButton = new MouseManager(MouseButtons.RightButton);

            this.mouseRightButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseRightButton_MouseFirstPressed);
            this.mouseRightButton.MousePressed += new MouseManager.MousePressedHandler(mouseRightButton_MousePressed);
            this.mouseRightButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseRightButton_MouseReleased);

            this.mouseLeftButton.MouseReleased += new MouseManager.MouseReleasedHandler(mouseLeftButton_MouseReleased);
        }

        void mouseLeftButton_MouseReleased(MouseButtons mouseButton, MouseState mouseState, GameTime gameTime, Point distance)
        {
            //if (selectedCell != null && Context.SelectedCell == selectedCell && Context.CurrentMenu != null && Context.CurrentMenu.State == MenuState.Opened)

            if (Context.CurrentMenu != null && Context.CurrentMenu.Items.Exists(item => item.MouseOver))
            {
                Context.CurrentMenu.MouseClick(GameEngine, gameTime, mouseState);
            }
            else
            {
                Cell selectedCell = GetSelectedCell(mouseState);

                Context.SelectedCell = selectedCell;

                if (Context.SelectedCell != null)
                {
                    //--- Ferme le précédent menu
                    if (Context.CurrentMenu != null)
                    {
                        Context.CurrentMenu.Close(gameTime);
                        Context.NextMenu = GameEngine.GamePlay.CreateMenu(Context.SelectedCell);
                    }
                    else
                    {
                        //---> Ouvre le nouveau menu
                        Context.CurrentMenu = GameEngine.GamePlay.CreateMenu(Context.SelectedCell);
                        Context.CurrentMenu.Open(gameTime);
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
            if (Context.CurrentMenu != null && Tools.Distance(Point.Zero, distance) < 5f)
            {
                Context.NextMenu = Context.CurrentMenu.ParentMenu;
                Context.CurrentMenu.Close(gameTime);
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
            if (Context.CurrentMenu != null)
            {
                Context.CurrentMenu.MouseOver(GameEngine, gameTime, mouseState);
            }
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

            Matrix mtx = Matrix.CreateScale(- GameEngine.Render.CameraPosition.Z) *Matrix.CreateTranslation(GameEngine.Render.CameraPosition.X, GameEngine.Render.CameraPosition.Y, GameEngine.Render.CameraPosition.Z);

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

        public Cell GetSelectedCell3D(MouseState mouseState)
        {
            Vector3 nearsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 0f);
            Vector3 farsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = GameEngine.GraphicsDevice.Viewport.Unproject(nearsource,
                GameEngine.Render.Projection, GameEngine.Render.View, world);

            Vector3 farPoint = GameEngine.GraphicsDevice.Viewport.Unproject(farsource,
                GameEngine.Render.Projection, GameEngine.Render.View, world);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);

            Cell selectedCell = null;
            float minimalDistance = float.MaxValue;

            foreach (Cell cell in Context.Map.Cells)
            {
                BoundingSphere s = new BoundingSphere(new Vector3(cell.Location.X, cell.Location.Y, cell.Height), Context.Map.R);
                float? distance = pickRay.Intersects(s);

                if (distance.HasValue && distance.Value < minimalDistance)
                {
                    selectedCell = cell;
                    minimalDistance = distance.Value;
                }
            }

            return selectedCell;
        }
    }
}
