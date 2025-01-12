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
    class UserKeys : BaseKeys
    {
        private Keys forword, backward, TurnLeft, TurnRight, RotLeft, RotRight, shoot;
        public UserKeys(Keys forword, Keys backward, Keys TurnLeft, Keys TurnRight, Keys RotLeft, Keys RotRight, Keys shoot)
        {
            this.forword = forword;
            this.backward = backward;
            this.TurnLeft = TurnLeft;
            this.TurnRight = TurnRight;
            this.RotLeft = RotLeft;
            this.RotRight = RotRight;
            this.shoot = shoot;
        }
        public override bool IsForword()
        {
            return Keyboard.GetState().IsKeyDown(forword);
        }
        public override bool IsBackward()
        {
            return Keyboard.GetState().IsKeyDown(backward);
        }
        public override bool IsTurnLeft()
        {
            return Keyboard.GetState().IsKeyDown(TurnLeft);
        }
        public override bool IsTurnRight()
        {
            return Keyboard.GetState().IsKeyDown(TurnRight);
        }
        public override bool IsRotLeft()
        {
            return Keyboard.GetState().IsKeyDown(RotLeft);
        }
        public override bool IsRotRight()
        {
            return Keyboard.GetState().IsKeyDown(RotRight);
        }
        public override bool IsShoot()
        {
            return Keyboard.GetState().IsKeyDown(shoot);
        }
    }
}
