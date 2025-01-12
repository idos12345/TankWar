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
    class BotKeys : BaseKeys
    {
        private int getOut = 0;
        private List<Point> TankPath;
        private tank CompTank;
        private tank PlayerTank;
        private bool isForword, isBackward, isTurnLeft, isTurnRight, isRotLeft, isRotRight, isShoot;
        private bool IsStuckForward = true;
        public BotKeys()
        {
            isForword = false;
            isBackward = false;
            isTurnLeft = false;
            isTurnRight = false;
            isRotLeft = false;
            isRotRight = false;
            isShoot = false;
        }

        public void RegisterCompTank(tank tank)
        {
            this.CompTank = tank;
        }
        public void RegisterPlayerTank(tank tank)
        {
            this.PlayerTank = tank;
        }

        public void Act()
        {
            // The function gets the tank path calculate distance and angle from the other tank and sct accordingly

            TankPath = ShortPathAlgorithmcs.GetPath();
            double shootingAngle = GetAccurateAngle();
            double ShootingAngelDiff = MathHelper.ToDegrees(CompTank.YRot + CompTank.Shootingdir) % 360 - shootingAngle;

            isForword = false;
            isBackward = false;
            isTurnLeft = false;
            isTurnRight = false;
            isRotLeft = false;
            isRotRight = false;
            isShoot = false;

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                
            }

            if (TankPath != null)
            {
                Point TankCurrentPoint = new Point((int)CompTank.position.X / 800, (int)CompTank.position.Z / 800);

                Point TankDest = TankPath[1];
                Point PlayerTankPoint = TankPath[TankPath.Count - 1];
                Point dir = new Point(TankDest.X - TankCurrentPoint.X, TankDest.Y - TankCurrentPoint.Y);
                int angle = GetDirAngle(dir);

               

                float angelDiff = MathHelper.ToDegrees(CompTank.YRot) % 360 - angle;

                if (angelDiff > 180)
                {
                    angelDiff -= 360;
                }
                else if (angelDiff < -180)
                {
                    angelDiff += 360;
                }

                if (CompTank.IsTankStuck && getOut <= 0)
                {
                    getOut = 140;
                }

                if (getOut > 0)
                {
                    getOut--;
                    if (IsStuckForward)
                    {
                        isBackward = true;
                    }
                    else
                    {
                        isForword = true;
                    }
                    
                }
                else if (Math.Abs(angelDiff) < 7)
                {
                    isForword = true;
                    IsStuckForward = true;
                }
                else if (angelDiff >= 7 && angelDiff < 67.5)
                {

                    isForword = true;
                    IsStuckForward = true;
                    isTurnRight = true;
                }
                else if (angelDiff <= -7 && angelDiff > -67.5)
                {
                    isForword = true;
                    IsStuckForward = true;
                    isTurnLeft = true;
                }
                else if (angelDiff >= 67.5 )
                {
                    isBackward = true;
                    IsStuckForward = false;
                    isTurnLeft = true;
                }
                else if (angelDiff <= -67.5)
                {
                    isBackward = true;
                    IsStuckForward = false;
                    isTurnRight = true;
                }

            }

            // Check if shooting is possible
            if (Vector3.Distance(CompTank.position, PlayerTank.position) <= 3000)
            {

                if (Math.Abs(ShootingAngelDiff) <= 3)
                {
                    isShoot = true;
                }
                else if (ShootingAngelDiff > 3)
                {
                    isRotRight = true;
                }
                else if (ShootingAngelDiff < -3)
                {
                    isRotLeft = true;
                }



            }
        }


        public override bool IsForword()
        {
            return isForword;
        }
        public override bool IsBackward()
        {
            return isBackward;
        }
        public override bool IsTurnLeft()
        {
            return isTurnLeft;
        }
        public override bool IsTurnRight()
        {
            return isTurnRight;
        }
        public override bool IsRotLeft()
        {
            return isRotLeft;
        }
        public override bool IsRotRight()
        {
            return isRotRight;
        }
        public override bool IsShoot()
        {
            return isShoot;
        }

        private int GetDirAngle(Point dir)
        {
            int angle = 0;
            if (dir.X == 1)
            {
                if (dir.Y == 0)
                {
                    angle = 90;
                }
                if (dir.Y == 1)
                {
                    angle = 45;
                }
                if (dir.Y == -1)
                {
                    angle = 135;
                }
            }
            if (dir.X == 0)
            {
                if (dir.Y == 1)
                {
                    angle = 0;
                }
                if (dir.Y == -1)
                {
                    angle = 180;
                }
            }
            if (dir.X == -1)
            {
                if (dir.Y == 0)
                {
                    angle = 270;
                }
                if (dir.Y == 1)
                {
                    angle = 315;
                }
                if (dir.Y == -1)
                {
                    angle = 225;
                }
            }

            return angle;

        }

        private double GetAccurateAngle()
        {
            double X = (double)CompTank.position.X - (double)PlayerTank.position.X;
            double Y = (double)CompTank.position.Z - (double)PlayerTank.position.Z;

            double num = Math.Atan(X / Y);

            if (Y < 0)
            {
                return MathHelper.ToDegrees((float)num);
            }

            return MathHelper.ToDegrees((float)num) + 180;


        }
    }
}
