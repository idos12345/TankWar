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
    class Camera
    {
        public Vector3 campos { get; set; }
        public Vector3 oldCamPos { get; set; }
        public Vector3 newCamPos { get; set; }
        public Matrix view { get; set; }
        public Vector3 lookAt { get; set; }
        public Matrix projection { get; set; }

        public Camera(Vector3 campos, Vector3 lookAt, Matrix projection)
        {
            this.projection = projection;
            this.campos = campos;
            this.lookAt = lookAt;
            this.view = Matrix.CreateLookAt(campos, lookAt, Vector3.Up);
            
            Game1.event_update += ApplyView;
        }

        public void ApplyView()
        {
            LerpCamera();
            this.view = Matrix.CreateLookAt(campos, lookAt, Vector3.Up); ;
        }

        public void UpdateCamera(Vector3 position, float rot)
        {
            oldCamPos = S.camera.campos;
            newCamPos = new Vector3(position.X - S.CameraDistance * (float)Math.Sin(rot), S.CameraHeight, position.Z - S.CameraDistance * (float)Math.Cos(rot));
            S.camera.lookAt = position;
        }

        private void LerpCamera()
        {
            S.camera.campos = Vector3.Lerp(oldCamPos, newCamPos, 0.1f);
            oldCamPos = S.camera.campos;
        }

        public Vector3 GetRight()
        {
       
                return Vector3.Transform(lookAt - campos, Matrix.CreateRotationY(0.5F));
            
            
        }
        public Vector3 GetUp() 
        {
            Vector3 a = lookAt - campos;

            a = Vector3.Transform(a, Matrix.CreateRotationZ(0.25f));
            a = Vector3.Transform(a, Matrix.CreateRotationX(0.25f));
         
            return a;
            
        }

    }
}
