using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheGrid.Model;
using Microsoft.Xna.Framework.Input;
using TheGrid.Common;

namespace TheGrid.Logic.Render
{
    public class RenderLogic
    {
        public SpriteBatch SpriteBatch;

        public Map Map
        {
            get
            {
                return this.GameEngine.GamePlay.Map;
            }
        }

        public GameEngine GameEngine { get; set; }

        BasicEffect effect;

        VertexBuffer vBuffer;

        Matrix View;
        Matrix Projection;
        Matrix World;

        public Vector3 CameraPosition = new Vector3(200f, 100f, 50f);
        public Vector3 CameraTarget = new Vector3(200f, 100f, 0f);
        public Vector3 CameraUp = -Vector3.Up;

        public bool updateViewScreen = false;
        public bool doScreenShot = false;
        Microsoft.Xna.Framework.Graphics.Model meshHexa;

        Dictionary<String, Microsoft.Xna.Framework.Graphics.Model> meshModels;

        public RenderLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
        }

        public void InitRender()
        {
            CreateCamera();
            CreateShader();

            CreateVertex();
            Initilize3DModel();

            SpriteBatch = new SpriteBatch(GameEngine.GraphicsDevice);
        }

        private void Initilize3DModel()
        {
            meshModels = new Dictionary<string, Microsoft.Xna.Framework.Graphics.Model>();

            //meshModels.Add("Hexa", GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\Hexa"));
            meshHexa = GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\Hexa");

            /*
            //meshModels.Add("FlowPhant", GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\FlowPhant"));
            meshModels.Add("FlowInspector", GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\FlowInspector"));
            //meshModels.Add("FlowRobot1", GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\FlowRobot1"));
            meshModels.Add("FlowRobot2", GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\FlowRobot2"));

            foreach (Player player in Context.Players)
            {
                foreach (MinionBase minion in player.Minions)
                {
                    minion.InitAnimationPlayer(meshModels[minion.ModelName]);
                }
            }
             * */
        }

        //Création de la caméra
        private void CreateCamera()
        {
            UpdateCamera();
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 2f, (float)GameEngine.Graphics.PreferredBackBufferWidth / (float)GameEngine.Graphics.PreferredBackBufferHeight, 0.01f, 1000.0f);
            //Projection = Matrix.CreatePerspective(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, 0.01f, 1000.0f);
            World = Matrix.Identity;
        }

        private void UpdateCamera()
        {
            View = Matrix.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
        }

        private void CreateShader()
        {
            effect = new BasicEffect(GameEngine.GraphicsDevice);
            //effect.World = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalMilliseconds/800);

            //effect.PreferPerPixelLighting = true;
            effect.VertexColorEnabled = false;
            effect.LightingEnabled = true;
            effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.3f);
            //effect.DirectionalLight0 = new DirectionalLight(

            effect.TextureEnabled = true;
            effect.SpecularColor = new Vector3(0.3f, 0.5f, 0.3f);
            effect.SpecularPower = 1f;
            //effect.SpecularColor = new Vector3(1, 1, 1);
            //effect.SpecularPower = 1f;
            effect.Texture = GameEngine.Content.Load<Texture2D>(@"Texture\Hexa0");

            UpdateShader(new GameTime());
        }

        public void UpdateShader(GameTime gameTime)
        {
            effect.View = View;
            effect.Projection = Projection;
            effect.World = World;

            float angle = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;
            effect.DirectionalLight0.Direction = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -1f);
        }

        public void CreateVertex()
        {
            vBuffer = new VertexBuffer(GameEngine.GraphicsDevice, typeof(VertexPositionNormalTexture), Map.Cells.Count * 12, BufferUsage.None);

            List<VertexPositionNormalTexture> vertex = new List<VertexPositionNormalTexture>();

            foreach (Cell cell in Map.Cells)
            {
                AddVertex(3, 2, 1, vertex, cell);
                AddVertex(6, 4, 3, vertex, cell);
                AddVertex(6, 5, 4, vertex, cell);
                AddVertex(6, 3, 1, vertex, cell);
            }

            vBuffer.SetData<VertexPositionNormalTexture>(vertex.ToArray());

            GameEngine.GraphicsDevice.SetVertexBuffer(vBuffer);
            //GameEngine.GraphicsDevice.Textures[0] = GameEngine.Content.Load<Texture2D>(@"Texture\HexaFloor1");
        }

        private void AddVertex(int index1, int index2, int index3, List<VertexPositionNormalTexture> vertex, Cell cell)
        {
            Dictionary<int, Vector2> uv = new Dictionary<int, Vector2>();

            uv.Add(1, new Vector2(0.75f, 0f));
            uv.Add(2, new Vector2(1f, 0.5f));
            uv.Add(3, new Vector2(0.75f, 1f));
            uv.Add(4, new Vector2(0.26f, 1f));
            uv.Add(5, new Vector2(0f, 0.5f));
            uv.Add(6, new Vector2(0.26f, 0f));

            vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index1]], Map.Normals[cell.Points[index1]], uv[index1]));
            vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index2]], Map.Normals[cell.Points[index2]], uv[index2]));
            vertex.Add(new VertexPositionNormalTexture(Map.Points[cell.Points[index3]], Map.Normals[cell.Points[index3]], uv[index3]));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            GameEngine.GraphicsDevice.Clear(new Color(15, 15, 15));

            UpdateCamera();
            //UpdateShader(gameTime);

            //--- Affiche la map
            foreach (Cell cell in Map.Cells)
            {
                DrawCell(cell, gameTime);
            }


            //CreateVertex();
            /*
            GameEngine.GraphicsDevice.SetVertexBuffer(vBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameEngine.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, Map.Cells.Count * 12);
            }
            */
            //---
        }

        private void DrawCell(Cell cell, GameTime gameTime)
        {
            float angle = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;
            Vector3 lightDirection = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -1f);
            Matrix localWorld = World * cell.MatrixLocation;
            ModelMesh mesh = null;

            //--- Hexa
            Color channelColor = Color.White;
            if (cell.Channel != null)
            {
                channelColor = cell.Channel.Color;
            }

            mesh = meshHexa.Meshes["Hexa"];
            foreach (Effect effect in mesh.Effects)
            {
                ((IEffectMatrices)effect).View = View;
                ((IEffectMatrices)effect).Projection = Projection;
                ((IEffectMatrices)effect).World = localWorld;

                ((IEffectLights)effect).EnableDefaultLighting();
                ((IEffectLights)effect).DirectionalLight0.Direction = lightDirection;

                ((BasicEffect)effect).DiffuseColor = channelColor.ToVector3();
            }

            mesh.Draw();
            //---

            //--- Directions
            for (int i = 1; i < 7; i++)
            {
                mesh = meshHexa.Meshes["Direction_" + i.ToString()];

                foreach (Effect effect in mesh.Effects)
                {
                    ((IEffectMatrices)effect).View = View;
                    ((IEffectMatrices)effect).Projection = View;
                    ((IEffectMatrices)effect).World = localWorld * mesh.ParentBone.Transform;

                    ((IEffectLights)effect).EnableDefaultLighting();
                    ((IEffectLights)effect).DirectionalLight0.Direction = lightDirection;

                    if (cell.Clip != null && cell.Clip.Directions[i])
                    {
                        ((BasicEffect)effect).DiffuseColor = Color.Red.ToVector3();
                    }
                    else
                    {
                        ((BasicEffect)effect).DiffuseColor = channelColor.ToVector3();
                    }
                }

                mesh.Draw();
            }
            //---

            //--- Repeater
            for (int i = 1; i < 7; i++)
            {
                mesh = meshHexa.Meshes["Repeater_" + i.ToString()];

                foreach (Effect effect in mesh.Effects)
                {
                    ((IEffectMatrices)effect).View = View;
                    ((IEffectMatrices)effect).Projection = View;
                    ((IEffectMatrices)effect).World = localWorld;

                    ((IEffectLights)effect).EnableDefaultLighting();
                    ((IEffectLights)effect).DirectionalLight0.Direction = lightDirection;

                    if (cell.Clip != null && cell.Clip.Repeater.HasValue && cell.Clip.Repeater >= i)
                    {
                        ((BasicEffect)effect).DiffuseColor = Color.Red.ToVector3();
                    }
                    else
                    {
                        ((BasicEffect)effect).DiffuseColor = channelColor.ToVector3();
                    }
                }

                mesh.Draw();
            }
            //---

            //--- Speed
            //if (cell.Clip.Speed.HasValue)
            //{
            //    if (cell.Clip.Speed.Value > 0)
            //    {
            //        for (int i = 0; i < cell.Clip.Repeater.Value; i++)
            //        {
            //            meshHexa.Meshes["Speed_H_" + i.ToString()].Draw();
            //        }
            //    }

            //    if (cell.Clip.Speed.Value < 0)
            //    {
            //        for (int i = 0; i > cell.Clip.Repeater.Value; i--)
            //        {
            //            meshHexa.Meshes["Speed_L_" + i.ToString()].Draw();
            //        }
            //    }
            //}
            //---
        }

        //private void DrawMinion(GameTime gameTime, MinionBase minion)
        //{
        //    float scaleZ = 1f -(float)Math.Cos(minion.BornTime.Subtract(gameTime.TotalGameTime).TotalMilliseconds * 0.01f) * 0.15f;

        //    DrawModel(gameTime, meshModels[minion.ModelName], Matrix.CreateScale(1f, 1f, scaleZ) * minion.MatrixRotation * Matrix.CreateTranslation(minion.Location), minion.AnimationPlayer);

        //    //* Matrix.CreateRotationZ(minion.Angle)
        //}

        //private void DrawModel(GameTime gameTime, Microsoft.Xna.Framework.Graphics.Model meshModel, Matrix mtxWorld, AnimationPlayer animationPlayer)
        //{
        //    float angle = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;
        //    Vector3 lightDirection = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -1f);

        //    Matrix[] mtxMeshTransform = new Matrix[meshModel.Bones.Count];
        //    meshModel.CopyAbsoluteBoneTransformsTo(mtxMeshTransform);

        //    Matrix[] bones = null;

        //    if (animationPlayer != null)
        //        bones = animationPlayer.GetSkinTransforms();

        //    //bones[0] = Matrix.CreateScale(500f) * World;// *mtxWorld;



        //    // Compute camera matrices.
        //    /*
        //    View = Matrix.CreateTranslation(0, 0, 0) *
        //                  Matrix.CreateRotationY(MathHelper.ToRadians(0f)) *
        //                  Matrix.CreateRotationX(MathHelper.ToRadians(0f)) *
        //                  Matrix.CreateLookAt(new Vector3(0, 0, -100f),
        //                                      new Vector3(0, 0, 0), Vector3.Up);
        //    */
        //    /*
        //    Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
        //                                                            1.6f,
        //                                                            1,
        //                                                            10000);

        //    */

        //    foreach (ModelMesh mesh in meshModel.Meshes)
        //    {


        //        foreach (Effect effect2 in mesh.Effects)
        //        {
        //            ((IEffectMatrices)effect2).View = View;
        //            ((IEffectMatrices)effect2).Projection = Projection;

        //            ((IEffectLights)effect2).EnableDefaultLighting();
        //            ((IEffectLights)effect2).DirectionalLight0.Direction = lightDirection;
        //            //((IEffectLights)effect2).PreferPerPixelLighting = true;
        //            //((IEffectLights)effect2).SpecularColor = new Vector3(0.25f);
        //            //((IEffectLights)effect2).SpecularPower = 16;

        //            if (effect2 is SkinnedEffect)
        //            {
        //                if (animationPlayer != null)
        //                    ((SkinnedEffect)effect2).SetBoneTransforms(bones);

        //                ((IEffectMatrices)effect2).World = World * mtxWorld;
        //            }
        //            else
        //            {
        //                if (((BasicEffect)effect2).Texture != null)
        //                {
        //                    ((BasicEffect)effect2).TextureEnabled = true;
        //                    ((BasicEffect)effect2).VertexColorEnabled = false;
        //                }

        //                ((IEffectMatrices)effect2).World = mtxMeshTransform[mesh.ParentBone.Index] * World * mtxWorld;
        //            }
        //        }

        //        mesh.Draw();
        //    }
        //}

        public Cell GetSelectedCell(MouseState mouseState)
        {
            Vector3 nearsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 0f);
            Vector3 farsource = new Vector3((float)mouseState.X, (float)mouseState.Y, 1f);

            Matrix world = Matrix.CreateTranslation(0, 0, 0);

            Vector3 nearPoint = GameEngine.GraphicsDevice.Viewport.Unproject(nearsource,
                Projection, View, world);

            Vector3 farPoint = GameEngine.GraphicsDevice.Viewport.Unproject(farsource,
                Projection, View, world);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);

            Cell selectedCell = null;
            float minimalDistance = float.MaxValue;

            foreach (Cell cell in Map.Cells)
            {
                BoundingSphere s = new BoundingSphere(new Vector3(cell.Location.X, cell.Location.Y, cell.Height), Map.R);
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
