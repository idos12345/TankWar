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
    abstract class BaseKeys
    {
        public abstract bool IsForword();
        public abstract bool IsBackward();
        public abstract bool IsTurnLeft();
        public abstract bool IsTurnRight();
        public abstract bool IsRotLeft();
        public abstract bool IsRotRight();
        public abstract bool IsShoot();
    }


}

