using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace NewFlowar.Logic.Controller
{
    public class ControllerLogic
    {
        #region Keyboard and Mouse
        private Keys upKey = Keys.Z;
        private Keys downKey = Keys.S;
        private Keys leftKey = Keys.Q;
        private Keys rightKey = Keys.D;

        private ButtonState prevLeftButtonState = ButtonState.Released;
        private ButtonState prevRightButtonState = ButtonState.Released;
        private int prevMouseWheel = 0;

        public MouseState mouseState;
        public KeyboardState keyBoardState = Keyboard.GetState();

        public Vector2 mousePosition;
        public Point mousePositionPoint;

        private bool isPlusKeyPressed = false;
        private bool isMinusKeyPressed = false;

        private bool isPKeyPressed = false;
        private bool isSKeyPressed = false;

        #endregion

        public bool IsMouseLeftButtonPressedAndReleased
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Released && prevLeftButtonState == ButtonState.Pressed;
            }
        }

        public bool IsMouseLeftButtonPressed
        {
            get
            {
                return mouseState.LeftButton == ButtonState.Pressed;
            }
        }

        public bool IsMouseRightButtonPressedAndReleased
        {
            get
            {
                return mouseState.RightButton == ButtonState.Released && prevRightButtonState == ButtonState.Pressed;
            }
        }

        public bool IsMouseRightButtonPressed
        {
            get
            {
                return mouseState.RightButton == ButtonState.Pressed;
            }
        }

        private GameEngine gameEngine { get; set; }

        public ControllerLogic(GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        public void UpdateBegin(GameTime gameTime)
        {
            //--- Update Mouse & Keyboard state
            mouseState = Mouse.GetState();
            keyBoardState = Keyboard.GetState();
            //---

            //--- Relative mouse position
            mousePosition = new Vector2(((float)mouseState.X - (float)this.gameEngine.GraphicsDevice.Viewport.Width / 2f) * (-this.gameEngine.Render.Zoom / 750f) + this.gameEngine.Render.VecTranslation.X, ((float)mouseState.Y - (float)this.gameEngine.GraphicsDevice.Viewport.Height / 2f) * (-this.gameEngine.Render.Zoom / 750f) + this.gameEngine.Render.VecTranslation.Y);
            mousePositionPoint = new Point(mouseState.X, mouseState.Y);
            //---

            //--- Zoom
            int curMouseWheel = mouseState.ScrollWheelValue;
            float estimatedZoom = gameEngine.Render.Zoom + (prevMouseWheel-curMouseWheel) / 100f;

            if (estimatedZoom >20)
                gameEngine.Render.Zoom = estimatedZoom;

            if (prevMouseWheel != curMouseWheel)
            {
                prevMouseWheel = curMouseWheel;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(Keys.PageUp))
            {
                gameEngine.Render.Zoom += 0.5f;
                gameEngine.Render.updateViewScreen = true;
            }
            if (keyBoardState.IsKeyDown(Keys.PageDown))
            {
                gameEngine.Render.Zoom -= 0.5f;
                gameEngine.Render.updateViewScreen = true;
            }

            if (gameEngine.Render.Zoom > 150f)
                gameEngine.Render.Zoom = 150f;
            //---

            //--- Keyboard
            float deltaTranslation = -2f / gameEngine.Render.Zoom;
            Vector2 vecTempTranslation = gameEngine.Render.VecTranslation;

            if (deltaTranslation >= 0.5f)
                deltaTranslation = 0.1f;

            if (keyBoardState.IsKeyDown(Keys.LeftShift))
                deltaTranslation *= 2;

            if (keyBoardState.IsKeyDown(upKey))
            {
                vecTempTranslation.Y -= deltaTranslation * gameTime.ElapsedGameTime.Milliseconds;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(downKey))
            {
                vecTempTranslation.Y += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(rightKey))
            {
                vecTempTranslation.X -= deltaTranslation * gameTime.ElapsedGameTime.Milliseconds;
                gameEngine.Render.updateViewScreen = true;
            }

            if (keyBoardState.IsKeyDown(leftKey))
            {
                vecTempTranslation.X += deltaTranslation * gameTime.ElapsedGameTime.Milliseconds;
                gameEngine.Render.updateViewScreen = true;
            }
            //---

            if (gameEngine.Render.updateViewScreen)
            {
                gameEngine.Render.VecTranslation = vecTempTranslation;
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

            //--- Draw physics objects
            //if (keyBoardState.IsKeyDown(Keys.P))
            //{
            //    if (!isPKeyPressed)
            //        isPKeyPressed = true;
            //}
            //else if (isPKeyPressed)
            //{
            //    gameEngine.Render.drawPhysicObjects = !gameEngine.Render.drawPhysicObjects;
            //    isPKeyPressed = false;
            //}
            //---

            //--- Add blob
            //if (keyBoardState.IsKeyDown(Keys.Add))
            //{
            //    if (!isPlusKeyPressed)
            //        isPlusKeyPressed = true;
            //}
            //else if (isPlusKeyPressed)
            //{
            //    if (gameEngine.GameLogic.currentBlobGroup != null)
            //        gameEngine.GameLogic.AddBlob(gameEngine.GameLogic.currentBlobGroup, 1f, Color.Red, 1f);
            //    //currentBlobGroup.AddBlob(1f, Color.Red, 1f, currentBlobGroup.GroupNumber);

            //    isPlusKeyPressed = false;
            //}
            //---

            //--- Subtract blob
            //if (keyBoardState.IsKeyDown(Keys.Subtract))
            //{
            //    if (!isMinusKeyPressed)
            //        isMinusKeyPressed = true;
            //}
            //else if (isMinusKeyPressed)
            //{
            //    if (gameEngine.GameLogic.currentBlobGroup != null)
            //        gameEngine.GameLogic.currentBlobGroup.DeleteLastBlob();

            //    isMinusKeyPressed = false;
            //}
            //---

            //--- Selection of BlobGroup
            //if (IsMouseLeftButtonPressedAndReleased && !gameEngine.Render.UIManager.IsMouseCaught)
            //{
            //    gameEngine.GameLogic.SelectGroupBlob(mousePosition);
            //}
            ////---

            ////--- Move blob
            //if (IsMouseRightButtonPressedAndReleased && gameEngine.GameLogic.currentBlobGroup != null && !gameEngine.Render.UIManager.IsMouseCaught)
            //{
            //    gameEngine.GameLogic.MoveBlobGroup(gameEngine.GameLogic.currentBlobGroup, mousePosition);
            //}
            //---
        }

        public void UpdateEnd(GameTime gameTime)
        {
            prevLeftButtonState = mouseState.LeftButton;
            prevRightButtonState = mouseState.RightButton;
        }
    }
}
