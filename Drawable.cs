using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankWar
{
    class Drawable
    {
        public Model model { get; set; }
        public Vector3 position { get; set; }
        public float YRot { get; set; }
        public float scale { get; set; }
        public Matrix[] boneTransformations { get; set; }
        private BoundingSphere boundingSphere;

        public Drawable(string ModelName,bool registerDrawEvent, bool registerUpdateEvent, Vector3 position, float scale)
        {
            model = S.content.Load<Model>(ModelName);
            this.position = new Vector3(position.X, position.Y, position.Z);
            this.YRot = 0;
            this.scale = scale;
            boneTransformations = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransformations);
            buildBoundingSphere();

            if (registerDrawEvent)
            {
                Game1.event_draw += draw;
            }
            if (registerUpdateEvent)
            {
                Game1.event_update += update;
            }
        }

        public virtual void draw()
        {
            // Draw A Model

            model.CopyAbsoluteBoneTransformsTo(boneTransformations);


            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransformations[mesh.ParentBone.Index] * Matrix.CreateScale(scale) * Matrix.CreateRotationY(YRot) * Matrix.CreateTranslation(position);
                    effect.View = S.camera.view;
                    effect.Projection = S.camera.projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        public virtual void update()
        {

        }

        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(boneTransformations[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            this.boundingSphere = sphere;
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = Matrix.CreateScale(scale*0.47f)
                * Matrix.CreateTranslation(position);

                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);
                return transformed;
            }
        }

        public void Rot180Deg()
        {
            YRot += MathHelper.ToRadians(180);
        }
    }
}
