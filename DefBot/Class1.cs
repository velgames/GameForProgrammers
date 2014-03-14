/*

 * default DLL with declars of methods and standart types for including to user project whithout defined bodyes.
    (Simple bot)
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DefBot
{
    public abstract class Bot
    {
        public virtual void move()
        {
        }

        public void goTo(int coordinate_x, int coordinate_y)
        {
            return;
        }
        
    }
}
