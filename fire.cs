using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankWar
{
    class Fire
    {
        Random rnd;
        ParticleSystem fire;
        Vector3 offset;
        Bullet bullet;
        public tank TankGotShot { get; set; }
        int FireTimer;

        public Fire(Bullet bullet)
        {
            rnd = new Random();
            fire = new ParticleSystem(S.graphics.GraphicsDevice, S.content, S.content.Load<Texture2D>("fire"), 1000, new Vector2(0.5f), 1, Vector3.Zero, 0.5f);
            this.offset = new Vector3(MathHelper.ToRadians(10.0f));
            this.bullet = bullet;
            FireTimer = 200;

        }

        public void updateBuildingGotShot()
        {
            FireTimer--;
            if (FireTimer <= 0)
            {
                Game1.event_draw -= draw;
                Game1.event_update -= updateBuildingGotShot;
                FireTimer = 200;
            }
            Vector3 offset = new Vector3(MathHelper.ToRadians(10.0f));
            Vector3 randAngle = Vector3.Up + randVec3(-offset, offset);
            // Generate a position between (-400, 0, -400) and (400, 0, 400)  
            Vector3 randPosition = randVec3(bullet.position + new Vector3(-10), bullet.position + new Vector3(-10));
            // Generate a speed between 600 and 900  
            float randSpeed = (float)rnd.NextDouble() * 300 + 600;
            fire.AddParticle(randPosition, randAngle, randSpeed);
            fire.Update();

        }

        public void updateTankGotShot()
        {
            FireTimer--;
            if (FireTimer <= 0)
            {
                Game1.event_draw -= draw;
                Game1.event_update -= updateTankGotShot;
                FireTimer = 200;
            }
            Vector3 offset = new Vector3(MathHelper.ToRadians(10.0f));
            Vector3 randAngle = Vector3.Up + randVec3(-offset, offset);
            // Generate a position between (-400, 0, -400) and (400, 0, 400)  
            Vector3 randPosition = randVec3(TankGotShot.position + new Vector3(-10), TankGotShot.position + new Vector3(-10));
            // Generate a speed between 600 and 900  
            float randSpeed = (float)rnd.NextDouble() * 300 + 600;
            fire.AddParticle(randPosition, randAngle, randSpeed);
            fire.Update();

        }

        public void draw()
        {
            fire.Draw(S.camera.view, S.camera.projection, S.camera.GetUp(), S.camera.GetRight());
        }

        private Vector3 randVec3(Vector3 min, Vector3 max)
        {
            return new Vector3(min.X + (float)rnd.NextDouble() *
              (max.X - min.X), min.Y + (float)rnd.NextDouble() *
              (max.Y - min.Y), min.Z + (float)rnd.NextDouble() *
              (max.Z - min.Z));
        }


    }
}
