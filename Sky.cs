using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankWar
{
    class SkyBox
    {
        // The basicEffect class controls how a 3D object is rendered by affecting it lighting, culling and camera control
        private BasicEffect effect;
        private Model skybox;
        private Matrix[] boneTransforms_sky;

        // Textures for cube and sky box
        private Texture2D[] sky_textures = new Texture2D[6];

        // Various objects need to be passed into this class to creat the skybox  in Game1.cs
        public SkyBox()
        {
            LoadSkyBox();
        }

        public void LoadSkyBox()
        {
            // Craete SkyBox by the X file skybox2

            effect = new BasicEffect(S.graphics.GraphicsDevice);
            effect.TextureEnabled = true;
            skybox = S.content.Load<Model>("SkyBox/skybox2");
            boneTransforms_sky = new Matrix[skybox.Bones.Count];
            skybox.CopyAbsoluteBoneTransformsTo(boneTransforms_sky);
        }

        public void DrawSkyBox(GameTime gameTime, Matrix view, Matrix projection)
        {
            skybox.CopyAbsoluteBoneTransformsTo(boneTransforms_sky);
            foreach (ModelMesh mesh in skybox.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    // Change made to the defualt lighting!
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = boneTransforms_sky[mesh.ParentBone.Index]*Matrix.CreateScale(450)*Matrix.CreateTranslation(new Vector3(S.cityWidth/2*800,-2000,S.cityHeight/2*800));
                }
                mesh.Draw();
            }
        }
    }
}