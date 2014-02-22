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
        public GameDPoint acceleration;
        public int radius;
        public GameDPoint currentSpeed;
        public string name;
        public int ID;

        public Player(GamePoint _position,int _radius,string _name,int _ID)
        {
            currentSpeed = new GameDPoint(0, 0);
            acceleration = new GameDPoint(0, 0);
            position = _position;
            radius = _radius;
            name = _name;
            ID = _ID;
            
        }

        public void move()
        {
            currentSpeed.x += acceleration.x;
            currentSpeed.y += acceleration.y;
            position.x += (int) currentSpeed.x;
            position.y += (int) currentSpeed.y;
            acceleration.x = 0;
            acceleration.y = 0;
        }
    }
}
