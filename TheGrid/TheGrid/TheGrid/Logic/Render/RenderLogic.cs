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
        public SpriteFont FontMenu { get; set; }
        public GameEngine GameEngine { get; set; }

        BasicEffect effect;

        VertexBuffer vBuffer;

        public Matrix View;
        public Matrix Projection;
        public Matrix World;

        public Vector3 CameraPosition = new Vector3(200f, 100f, 50f);
        public Vector3 CameraTarget = new Vector3(200f, 100f, 0f);
        public Vector3 CameraUp = -Vector3.Up;

        public bool updateViewScreen = false;
        public bool doScreenShot = false;
        MeshHexa meshHexa = null;
        private Texture2D texEmpty = null;
        private Texture2D texHexa = null;
        private Texture2D texHexa2 = null;

        Dictionary<String, Microsoft.Xna.Framework.Graphics.Model> meshModels;

        public RenderLogic(GameEngine gameEngine)
        {
            this.GameEngine = gameEngine;
        }

        public void InitRender()
        {
            CreateCamera();
            CreateShader();

            Initilize3DModel();

            SpriteBatch = new SpriteBatch(GameEngine.GraphicsDevice);
            FontMenu = GameEngine.Content.Load<SpriteFont>(@"Font\FontMenu");
            texEmpty = GameEngine.Content.Load<Texture2D>(@"Texture\HexaEmpty");
            texHexa = GameEngine.Content.Load<Texture2D>(@"Texture\ImgHexa");
            texHexa2 = GameEngine.Content.Load<Texture2D>(@"Texture\ImgHexa2");
        }

        private void Initilize3DModel()
        {
            meshModels = new Dictionary<string, Microsoft.Xna.Framework.Graphics.Model>();

            meshHexa = new MeshHexa(GameEngine.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(@"3DModel\Hexa"));
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
            effect.VertexColorEnabled = true;
            effect.LightingEnabled = false;
            effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.3f);
            //effect.DirectionalLight0 = new DirectionalLight(

            effect.TextureEnabled = false;
            effect.SpecularColor = new Vector3(0.3f, 0.5f, 0.3f);
            effect.SpecularPower = 1f;

            //effect.SpecularColor = new Vector3(1, 1, 1);
            //effect.SpecularPower = 1f;
            //effect.Texture = GameEngine.Content.Load<Texture2D>(@"Texture\Hexa0");

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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            GameEngine.GraphicsDevice.Clear(new Color(15, 15, 15));

            UpdateCamera();
            UpdateShader(gameTime);

            //--- Affiche la map
            foreach (Cell cell in Context.Map.Cells)
            {
                DrawCell(cell, gameTime);
            }
            //---
  
            //--- Affiche le menu
            if (Context.CurrentMenu != null)
                Context.CurrentMenu.Draw(GameEngine, effect, gameTime);
            //---
      }

        private void DrawCell(Cell cell, GameTime gameTime)
        {
            //GameEngine.GraphicsDevice.

            //VertexPositionColorTexture f;
            
            //vBuffer.SetData<VertexPositionNormalTexture>(vertex.ToArray());

            //meshHexa.Body.MeshParts[0].VertexBuffer.GetData<

            if (Context.CurrentMenu != null && Context.CurrentMenu.ParentCell == cell)
            {
                cell.MatrixLocation = Matrix.CreateTranslation(cell.Location.X, cell.Location.Y, 10f * (float)Context.CurrentMenu.PercentVisibility);
            }
            else
            {
                cell.MatrixLocation = Matrix.CreateTranslation(cell.Location.X, cell.Location.Y, 0f);
            }

            Matrix localWorld = World * Matrix.CreateRotationZ(MathHelper.Pi) * cell.MatrixLocation;

            //--- Affficher la cellule si elle est dans la portion visible de l'écran
            BoundingFrustum frustum = new BoundingFrustum(View * Projection);

            BoundingSphere boundingSphere = meshHexa.Body.BoundingSphere.Transform(localWorld);

            if (frustum.Contains(boundingSphere) == ContainmentType.Disjoint)
                return;
            //---

            float angle = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;
            Vector3 lightDirection = new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), -1f);

            //--- Hexa
            Color channelColor = Color.Gray;
            if (cell.Channel != null)
            {
                channelColor = cell.Channel.Color;
            }
            else if (cell.Clip != null)
            {
                channelColor = Color.White;
            }

            foreach (Effect effect in meshHexa.Body.Effects)
            {
                BasicEffect basicEffect = effect as BasicEffect;

                basicEffect.View = View;
                basicEffect.Projection = Projection;
                basicEffect.World = localWorld;

                basicEffect.EnableDefaultLighting();
                basicEffect.DirectionalLight0.Direction = lightDirection;

                basicEffect.DiffuseColor = channelColor.ToVector3();
            }

            meshHexa.Body.Draw();

            foreach (Effect effect in meshHexa.Icon.Effects)
            {
                BasicEffect basicEffect = effect as BasicEffect;

                basicEffect.View = View;
                basicEffect.Projection = Projection;
                basicEffect.World = localWorld;

                basicEffect.EnableDefaultLighting();
                basicEffect.DirectionalLight0.Direction = lightDirection;

                basicEffect.DiffuseColor = channelColor.ToVector3();
            }

            meshHexa.Icon.Draw();
            //---

            //--- Directions
            for (int i = 0; i < 6; i++)
            {
                foreach (Effect effect in meshHexa.Direction[i].Effects)
                {
                    BasicEffect basicEffect = effect as BasicEffect;

                    basicEffect.View = View;
                    basicEffect.Projection = Projection;
                    basicEffect.World = localWorld;

                    basicEffect.EnableDefaultLighting();
                    basicEffect.DirectionalLight0.Direction = lightDirection;
                    basicEffect.DiffuseColor = channelColor.ToVector3();

                    if (cell.Clip == null || !cell.Clip.Directions[i])
                    {
                        basicEffect.Texture = texEmpty;
                    }
                    else if(cell.Clip.Directions[i])
                    {
                        basicEffect.Texture = texHexa;
                    }
                }

                meshHexa.Direction[i].Draw();
            }
            //---

            //--- Repeater
            for (int i = 0; i < 6; i++)
            {
                foreach (Effect effect in meshHexa.Repeater[i].Effects)
                {
                    BasicEffect basicEffect = effect as BasicEffect;

                    basicEffect.View = View;
                    basicEffect.Projection = Projection;
                    basicEffect.World = localWorld;

                    basicEffect.EnableDefaultLighting();
                    basicEffect.DirectionalLight0.Direction = lightDirection;
                    basicEffect.DiffuseColor = channelColor.ToVector3();

                    if (cell.Clip == null || !cell.Clip.Repeater.HasValue || i > cell.Clip.Repeater.Value)
                    {
                        basicEffect.Texture = texEmpty;
                    }
                    else if (cell.Clip.Repeater.HasValue && i <= cell.Clip.Repeater.Value)
                    {
                        basicEffect.Texture = texHexa;
                    }
                }

                meshHexa.Repeater[i].Draw();
            }
            //---

            //--- Speed
            for (int i = 0; i < 4; i++)
            {
                foreach (Effect effect in meshHexa.Speed[i].Effects)
                {
                    BasicEffect basicEffect = effect as BasicEffect;

                    basicEffect.View = View;
                    basicEffect.Projection = Projection;
                    basicEffect.World = localWorld;

                    basicEffect.EnableDefaultLighting();
                    basicEffect.DirectionalLight0.Direction = lightDirection;
                    basicEffect.DiffuseColor = channelColor.ToVector3();

                    // else if(cell.Clip.Directions[i])
                    //{
                    //    basicEffect.Texture = texHexa;
                    //}
                    //if (cell.Clip.Speed.Value > 4 && cell.Clip.Speed.Value-4>i)
                    //{
                    //    basicEffect
                    //}

                    if (
                        cell.Clip != null && 
                        cell.Clip.Speed.HasValue && 
                        cell.Clip.Speed.Value > 0 && 
                        i < cell.Clip.Speed.Value)
                    {
                        basicEffect.Texture = texHexa;
                    }
                    else if (
                        cell.Clip != null &&
                        cell.Clip.Speed.HasValue &&
                        cell.Clip.Speed.Value < 0 &&
                        i < Math.Abs(cell.Clip.Speed.Value))
                    {
                        basicEffect.Texture = texHexa2;
                    }
                    else
                    {
                        basicEffect.Texture = texEmpty;
                    }

                }

                meshHexa.Speed[i].Draw();
            }
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
    }
}
