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
    class Round
    {
        public tank UserTank { get; set; }
        public tank ComputerTank { get; set; }
        private Bullet UserBullet;
        private Bullet ComputerBullet;
        private UserKeys playerKeys;
        private BotKeys computerKeys;

        public Round(UserKeys playerKeys, BotKeys computerKeys)
        {
            this.playerKeys = playerKeys;
            this.computerKeys = computerKeys;

            Vector3 PlayerTankPos = new Vector3(400, 0, 400);
            Vector3 ComputerTankPos = new Vector3(14 * 800 + 400, 0, 14 * 800 + 400);

            UserBullet = new Bullet("Bullet", false, false, new Vector3(0, 0, 0), 5, 0);
            ComputerBullet = new Bullet("Bullet", false, false, new Vector3(0, 0, 0), 5, 0);

            UserTank = new tank("m1a1a", true, true, PlayerTankPos, 0.5f, playerKeys, UserBullet, true);
            ComputerTank = new tank("m1a1a2", true, true, ComputerTankPos, 0.5f, computerKeys, ComputerBullet, false);
            ComputerTank.Rot180Deg();

            UserBullet.RegisterTanks(UserTank, ComputerTank);
            ComputerBullet.RegisterTanks(ComputerTank, UserTank);

            computerKeys.RegisterPlayerTank(UserTank);
            computerKeys.RegisterCompTank(ComputerTank);

            ClearStaticLists();
            S.CityMap.BuildCity();

            S.Objects.Add(UserTank);
            S.Objects.Add(ComputerTank);

        }

        private void ClearStaticLists()
        {
            S.Objects.Clear();
        }

        public S.GameState CheckGameState()
        {
            // This Function Checks if someone won the game

            if (ComputerTank.IsTankDied == true || UserTank.IsTankDied == true)
            {
                ComputerTank.ResetTank();
                UserTank.ResetTank();

                if (ComputerTank.IsTankDied == true)
                {

                    return S.GameState.USER_TANK_WON;
                }
                if (UserTank.IsTankDied == true)
                {

                    return S.GameState.COMPUTER_TANK_WON;
                }
            }

            return S.GameState.ON;
        }
    }
}
