﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Player : ICreature
    {
        static int dX, dY = 0;
        public static bool dead = false;

        public CreatureCommand Act(int x, int y)
        {
            switch(Game.KeyPressed)
            {
                case System.Windows.Forms.Keys.Left:
                    dX = -1;
                    break;
                case System.Windows.Forms.Keys.Up:
                    dY = -1;
                    break;
                case System.Windows.Forms.Keys.Right:
                    dX = 1;
                    break;
                case System.Windows.Forms.Keys.Down:
                    dY = 1;
                    break;
                default:
                    Stay();
                    break;
            }
            if (!(x + dX >= 0 && x + dX < Game.MapWidth &&
                y + dY >= 0 && y + dY < Game.MapHeight))
                    Stay();
            if (Game.Map[x + dX, y + dY] != null)
            {
                if (Game.Map[x + dX, y + dY].ToString() == "Digger.Sack")
                    Stay();
            }
            return new CreatureCommand() { DeltaX = dX, DeltaY = dY };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.ToString() == "Digger.Gold")
                Game.Scores += 10;
            if (conflictedObject.ToString() == "Digger.Sack")
                return true;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }

        private static void Stay()
        {
            dX = 0;
            dY = 0;
        }
    }

    public class Sack : ICreature
    {
        private int counter = 0;
        public static bool deadlyForPlayer = false;

        public CreatureCommand Act(int x, int y)
         {
            if (y < Game.MapHeight - 1)
            {
                var map = Game.Map[x, y + 1];
                if (map == null || (counter > 0 && map.ToString() == "Digger.Player"))
                {
                    counter++;
                    return Falling();
                }
            }
            if (counter > 1)
            {
                counter = 0;
                return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
            }
            counter = 0;
            return DoNothing();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }

        private CreatureCommand Falling()
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
        }

        private CreatureCommand DoNothing()
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }
    }

    public class Gold : ICreature 
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0 };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject.ToString() == "Digger.Player") return true;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }
}