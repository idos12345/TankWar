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
    class Bullet : Drawable
    {
       private float height;
       private Boolean IsShotYet = false;
       private tank MyTank;
       private tank RivalTank;
       private Smoke smoke;
       private Fire fire;
       private float DownSpeed = 0;

        public Bullet(string ModelName, bool registerDrawEvent
            , bool registerUpdateEvent, Vector3 position, float scale, float dir)
            : base(ModelName, registerDrawEvent, registerUpdateEvent, position, scale)
        {
            this.YRot = dir;
            height = S.BulletHeight;
            smoke = new Smoke(this);
            fire = new Fire(this);
        }

        public void shoot(Vector3 pos, float dir)
        { 
            // The function take care about shooting the bullet

            DownSpeed = 0;
            height = S.BulletHeight;
            this.YRot = dir;
            position = new Vector3(pos.X + S.BarrelDistance * (float)Math.Sin(YRot), height, pos.Z + S.BarrelDistance * (float)Math.Cos(YRot));



            if (IsShotYet == false)
            {
                Game1.event_draw += smoke.draw;
                Game1.event_update += smoke.update;
                Game1.event_draw -= fire.draw;
                Game1.event_update -= fire.updateBuildingGotShot;
                Game1.event_update -= fire.updateTankGotShot;
                Game1.event_update += update;
                Game1.event_draw += draw;
                IsShotYet = true;
            }

        }

        public override void update()
        {
            DownSpeed += S.GForce;
            height -= DownSpeed;
            position = new Vector3(position.X + S.BulletSpeed * (float)Math.Sin(YRot), height, position.Z + S.BulletSpeed * (float)Math.Cos(YRot));

            // If hited the floor stop draw the bullet
            if (height <= 0)
            {
             
                Game1.event_draw -= draw;
                Game1.event_update -= update;
                Game1.event_draw -= smoke.draw;
                Game1.event_update -= smoke.update;
                IsShotYet = false;
            }

            // Check for collision 
            for (int i = 0; i < S.Objects.Count; i++)
            {
                if (Vector3.Distance(position,S.Objects[i].position) <= 1000 &&S.Objects[i] != this && S.Objects[i] != MyTank &&
                    this.BoundingSphere.Intersects(S.Objects[i].BoundingSphere))
                {
                    if (S.Objects[i] == RivalTank)
                    {
                        fire.TankGotShot = RivalTank;
                        Game1.event_update += fire.updateTankGotShot;
                        RivalTank.gotShot();
                    }
                    else
                    {
                        Game1.event_update += fire.updateBuildingGotShot;
                    }
                    Game1.event_draw -= smoke.draw;
                    Game1.event_update -= smoke.update;
                    Game1.event_draw += fire.draw;
                 
                    Game1.event_draw -= draw;
                    Game1.event_update -= update;
                    IsShotYet = false;
                }
            }

        }

        public void RegisterTanks(tank MyTank,tank RivalTank)
        {
            this.MyTank = MyTank;
            this.RivalTank = RivalTank;
        }

    }
}
