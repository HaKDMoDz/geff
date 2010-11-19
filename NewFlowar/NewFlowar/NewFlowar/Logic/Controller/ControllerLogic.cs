using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using NewFlowar.Common;

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

        public Vector2 mousePosition;
        public Point mousePositionPoint;

        private bool isPKeyPressed = false;
        private bool isSKeyPressed = false;

        private KeyManager keyLeft;
        private KeyManager keyRight;
        private KeyManager keyUp;
        private KeyManager keyDown;

        private MouseManager mouseLeftButton;
        private MouseManager mouseMiddleButton;
        private MouseManager mouseRightButton;
        
        private float prevAngle;
        private float prevRayon;
        #endregion

        private GameEngine gameEngine { get; set; }

        public ControllerLogic(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;

            this.keyLeft = new KeyManager(leftKey);

            this.mouseLeftButton = new MouseManager(MouseButtons.LeftButton);
            this.mouseMiddleButton = new MouseManager(MouseButtons.MiddleButton);
            this.mouseRightButton = new MouseManager(MouseButtons.RightButton);

            this.mouseMiddleButton.MouseFirstPressed += new MouseManager.MouseFirstPressedHandler(mouseMiddleButton_MouseFirstPressed);
            this.mouseMiddleButton.MousePressed += new MouseManager.MousePressedHandler(mouseMiddleButton_MousePressed);
        }

        void mouseMiddleButton_MouseFirstPressed(MouseButtons mouseButton, GameTime gameTime)
        {
            Vector2 cameraDirection = Tools.GetVector2(this.gameEngine.Render.CameraTarget) - Tools.GetVector2(this.gameEngine.Render.CameraPosition);

            prevRayon = cameraDirection.Length();
            prevAngle = Tools.GetAngle(Vector2.UnitX, cameraDirection);
        } 

        void mouseMiddleButton_MousePressed(MouseButtons mouseButton, GameTime gameTime, Point distance)
        {

            float rayon = prevRayon + (float)distance.Y/7f;

            float angle = prevAngle + (float)distance.X / 200f;

            this.gameEngine.Render.CameraTarget = Tools.GetVector3WithoutZ(this.gameEngine.Render.CameraPosition) + new Vector3((float)Math.Cos(angle) * rayon, (float)Math.Sin(angle) * rayon, 0f);
        }

        public void UpdateBegin(GameTime gameTime)
        {
            //--- Update Mouse & Keyboard state
            mouseState = Mouse.GetState();
            keyBoardState = Keyboard.GetState();
            //---

            //--- Relative mouse position
            //mousePosition = new Vector2(((float)mouseState.X - (float)this.gameEngine.GraphicsDevice.Viewport.Width / 2f) * (-this.gameEngine.Render.Zoom / 750f) + this.gameEngine.Render.VecTranslation.X, ((float)mouseState.Y - (float)this.gameEngine.GraphicsDevice.Viewport.Height / 2f) * (-this.gameEngine.Render.Zoom / 750f) + this.gameEngine.Render.VecTranslation.Y);
            mousePositionPoint = new Point(mouseState.X, mouseState.Y);
            //---

            //--- Zoom
            int curMouseWheel = mouseState.ScrollWheelValue;
            float estimatedZoom = gameEngine.Render.CameraPosition.Z + (prevMouseWheel - curMouseWheel) / 50f;

            if (estimatedZoom >20)
                gameEngine.Render.CameraPosition.Z = estimatedZoom;

            if (prevMouseWheel != curMouseWheel)
            {
                prevMouseWheel = curMouseWheel;
                gameEngine.Render.updateViewScreen = true;
            }

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
            float deltaTranslation = 8f / gameEngine.Render.CameraPosition.Z;
            Vector2 vecTempTranslation = Vector2.Zero;

            if (deltaTranslation >= 0.5f)
                deltaTranslation = 0.1f;

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

            if (keyBoardState.IsKeyDown(Keys.M))
            {
                this.gameEngine.GamePlay.Map.CreateGrid();
                this.gameEngine.Render.CreateVertex();
            }

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
