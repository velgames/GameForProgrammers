using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameWorld.GameTypes
{
    public class GamePoint
    {
        public int x;
        public int y;

        public GamePoint(int _x,int _y)
        {
            x = _x;
            y = _y;
        }

    }

    public class GameDPoint
    {
        public double x;
        public double y;

        public GameDPoint(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

    }


}
