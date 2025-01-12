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

    static class S
    {
        public static GraphicsDeviceManager graphics;
        public static ContentManager content;

        public static Camera camera;

        public static Map CityMap;

        public enum GameState { ON, USER_TANK_WON, COMPUTER_TANK_WON };
        public enum CitySpot { EMPTY, BUILDING, MY_TANK, COMPUTER_TANK, BOTH };

        public static float GForce = 9.81f * 0.005f;

        public static int cityWidth = 15;
        public static int cityHeight = 15;
        public static int CameraHeight = 200;
        public static int CameraDistance = 500;
        public static int NumOfBuildings = 10;
                    
        public static float BarrelDistance = 160;
        public static float BulletSpeed = 75;
        public static float BulletHeight = 90;
        public static float TankForwardSpeed = 15;
        public static float TankBackwardSpeed = 10;
        public static float TankRotationAngle = 0.02f;
        public static float TankShootingRotationAngle = 0.02f;

        public static List<Drawable> Objects = new List<Drawable>();



    }


}
