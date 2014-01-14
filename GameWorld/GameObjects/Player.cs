using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameWorld.GameTypes;

namespace GameWorld.GameObjects
{
    public class Player
    {
        public GamePoint position;
        public int radius;
        public GamePoint currentSpeed;
        public string name;
        public int ID;

        public Player(GamePoint _position,int _radius,string _name,int _ID)
        {
            currentSpeed = new GamePoint(0, 0);
            position = _position;
            radius = _radius;
            name = _name;
            ID = _ID;
            
        }

        public void move()
        {
            position.x += currentSpeed.x;
            position.y += currentSpeed.y;
        }
    }
}
