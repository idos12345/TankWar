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
    class tank : Drawable
    {

        public float Shootingdir { get; set; }
        public BaseKeys TankKeys { get; set; }
        public Bullet bullet { get; set; }
        private bool Isshot = false;
        private int shootingTimer = 0;
        private bool IsPlayerTank;
        public int health { get; set; }
        public bool IsTankStuck { get; set; }
        public bool IsTankDied { get; set; }
        private SoundEffect ShotSound;

        public tank(string ModelName, bool registerDrawEvent
            , bool registerUpdateEvent, Vector3 position, float scale, BaseKeys TankKeys, Bullet bullet, bool IsPlayerTank)
            : base(ModelName, registerDrawEvent, registerUpdateEvent, position, scale)
        {
            IsTankDied = false;
            Shootingdir = 0;
            this.TankKeys = TankKeys;
            this.bullet = bullet;
            this.IsPlayerTank = IsPlayerTank;
            health = 100;
            ShotSound = S.content.Load<SoundEffect>("SoundEffects/105mm_cannon_single_round");

            if (TankKeys is BotKeys)
            {
                Game1.event_update += ((BotKeys)TankKeys).Act;
            }
        }

        public override void update()
        {
   
            IsTankStuck = false;
            Vector3 oldPos = position;

            // Check if the keys been clicked by the player or the computer and act in accordance

            if (TankKeys.IsRotRight())
            {
                Shootingdir -= S.TankShootingRotationAngle;
                model.Bones[14].Transform *= Matrix.CreateRotationY(-S.TankShootingRotationAngle);
            }

            if (TankKeys.IsRotLeft())
            {
                Shootingdir += S.TankShootingRotationAngle;
                model.Bones[14].Transform *= Matrix.CreateRotationY(S.TankShootingRotationAngle);
            }

            if (TankKeys.IsForword())
            {

                position = new Vector3(position.X + S.TankForwardSpeed * (float)Math.Sin(YRot), position.Y, position.Z + S.TankForwardSpeed * (float)Math.Cos(YRot));
            }

            if (TankKeys.IsBackward())
            {
                position = new Vector3(position.X - S.TankBackwardSpeed * (float)Math.Sin(YRot), position.Y, position.Z - S.TankBackwardSpeed * (float)Math.Cos(YRot));
            }

            if (TankKeys.IsTurnLeft() && TankKeys.IsForword())
            {
                YRot += S.TankRotationAngle;
            }

            if (TankKeys.IsTurnLeft() && TankKeys.IsBackward())
            {
                YRot -= S.TankRotationAngle;
            }

            if (TankKeys.IsTurnRight() && TankKeys.IsForword())
            {
                YRot -= S.TankRotationAngle;
            }

            if (TankKeys.IsTurnRight() && TankKeys.IsBackward())
            {
                YRot += S.TankRotationAngle;
            }


            if (TankKeys.IsShoot() && Isshot == false)
            {
                ShotSound.Play();
                bullet.shoot(position, YRot + Shootingdir);
 
                // Start Shooting Timer 
                Isshot = true;
            }

           // inc Shooting timer if there was a shot recently
            if (Isshot == true)
            {
                shootingTimer++;
                if (shootingTimer >= 100)
                {
                    Isshot = false;
                    shootingTimer = 0;
                }
            }

            // Check for Collistions
            for (int i = 0; i < S.Objects.Count; i++)
            {
                if (S.Objects[i].model != model)
                {
                    if (Vector3.Distance(S.Objects[i].position,position) <= 1000 &&this.BoundingSphere.Intersects(S.Objects[i].BoundingSphere))
                    {
                        position = oldPos;
                        IsTankStuck = true;
                    }
                }
            }

            if (!IsInBounds())
            {
                position = oldPos;
                IsTankStuck = true;
            }

            UpdateCityMap();

            if (IsPlayerTank)
            {

                S.camera.UpdateCamera(position, YRot + Shootingdir);
            }


        }


        public void gotShot()
        {
            health -= 10;

            if (health <= 0)
            {
                TankDied();
            }
        }

        public void UpdateCityMap()
        {
            // Update CityMap acccording to the current tank position

            S.CitySpot tankInd;
            if (IsPlayerTank)
            {
                tankInd = S.CitySpot.MY_TANK;
            }
            else
            {
                tankInd = S.CitySpot.COMPUTER_TANK;
            }

            for (int i = 0; i < S.cityWidth; i++)
            {
                for (int j = 0; j < S.cityHeight; j++)
                {
                    if (S.CityMap[i, j] == tankInd)
                    {
                        S.CityMap[i, j] = S.CitySpot.EMPTY;
                    }
                    if (S.CityMap[i, j] == S.CitySpot.BOTH)
                    {
                        if (IsPlayerTank)
                        {
                            S.CityMap[i, j] = S.CitySpot.COMPUTER_TANK;
                        }
                        else
                        {
                            S.CityMap[i, j] = S.CitySpot.MY_TANK;
                        }

                    }
                }
            }

            if (IsPlayerTank)
            {
                if (S.CityMap[(int)position.X / 800, (int)position.Z / 800] == S.CitySpot.COMPUTER_TANK)
                {
                    S.CityMap[(int)position.X / 800, (int)position.Z / 800] = S.CitySpot.BOTH;
                }
                else
                {
                    S.CityMap[(int)position.X / 800, (int)position.Z / 800] = tankInd;
                }
            }
            else
            {
                if (S.CityMap[(int)position.X / 800, (int)position.Z / 800] == S.CitySpot.MY_TANK)
                {
                    S.CityMap[(int)position.X / 800, (int)position.Z / 800] = S.CitySpot.BOTH;
                }
                else
                {
                    S.CityMap[(int)position.X / 800, (int)position.Z / 800] = tankInd;
                }
            }


        }

        public bool IsInBounds()
        {
            // Check if the tank positions are in the city

            return position.X >= 0 && position.Z >= 0 && (int)position.X / 800 <= 14 && (int)position.Z / 800 <= 14;

        }

        public void TankDied()
        {
            IsTankDied = true;

            // Color the tank in black
          /*  foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.DiffuseColor = Color.Black.ToVector3();

                    effect.AmbientLightColor = Color.Gray.ToVector3();
                }

            }*/

            Game1.event_update -= update;
        }

        public void ResetTank()
        {
            // reset the tank to it's default settings 

            model.Bones[14].Transform *= Matrix.CreateRotationY(-Shootingdir);
          
        }


    }
}
