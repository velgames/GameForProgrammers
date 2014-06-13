using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GameWorld.GameObjects;
using GameWorld.GameTypes;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using Tao.Lua;

namespace GameWorldPlayer
{
    class mainclass
    {
        static GameState world;
        static StreamReader logReader;
        static int tickCount;
        static int tickCurrent;
        static int playersCount;
        static int worldWidthSize;
        static int worldHeightSize;


        static void Display()
        {
            
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            
            logStateReader();
            drawPlayers();
            GameText.DrawText("TICK : " + tickCurrent.ToString(), 20, 20, 20, 10, 255, 255, 255);

            Glut.glutSwapBuffers();
            System.Threading.Thread.Sleep(20);
        }

        static void logStateReader()
        {
            if (tickCurrent < tickCount)
            {
                world = new GameState(logReader.ReadLine(), playersCount);
                tickCurrent++;
            }
        }

        static bool logHeadReader()
        {
            string buf = "";
            while (!logReader.EndOfStream)
            {
                buf = logReader.ReadLine();
                if (buf == "[START CONFIG]")
                {
                    return loadGameConfig();
                }

            }
            return false;
        }

        static bool loadGameConfig()
        {
            try{
                while (!logReader.EndOfStream)
                {
                    string str = logReader.ReadLine();
                    if (str == "[END]")
                    {
                        while (!logReader.EndOfStream)
                        {
                            str = logReader.ReadLine();
                            if (str == "[GAME STATES]") return true;
                        }
                    }
                    string[] strings = str.Split(' ');
                    switch (strings[0])
                    {
                        case "tickCount":
                            tickCount = Convert.ToInt32(strings[1]);
                            break;
                        case "playersCount":
                            playersCount = Convert.ToInt32(strings[1]);
                            break;
                        case "worldWidthSize":
                            worldWidthSize = Convert.ToInt32(strings[1]);
                            break;
                        case "worldHeightSize":
                            worldHeightSize = Convert.ToInt32(strings[1]);
                            break;
                        case "#":
                            // commentaries
                            break;
                    }
                }
                
            }
            catch(Exception ex)
                {
                    return false;
                }

            return true;
        }
        static void drawPlayers()
        {
            for (int i = 0; i < world.players.Count; i++)
            {
                int n = 40; // tangle count

                Gl.glBegin(Gl.GL_POLYGON);
                for (int j = 0; j < n; j++)
                {
                    Gl.glVertex2d(world.players[i].radius * Math.Cos(2 * Math.PI * j / n) + world.players[i].position.x, world.players[i].radius * Math.Sin(2 * Math.PI * j / n) + world.players[i].position.y);
                }
                Gl.glEnd();
                GameText.DrawText(world.players[i].name.ToUpper(), world.players[i].position.x - world.players[i].radius, world.players[i].position.y + world.players[i].radius, 10, 5, 100, 255, 100);
            }
            
        }

        static bool init()
        {
            logReader = new StreamReader("gameLog.txt");
            loadGameConfig();
            tickCurrent = 0;
            return true;
        }

        static void closer()
        {
            logReader.Close();
        }

        static void WinInit()
        {
            Gl.glClearColor(0, 0, 0, 0);
            Gl.glColor3f(0, 0, 255);
            Gl.glPointSize(2);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluOrtho2D(0, 1024, 0, 768);
            Gl.glLineWidth(2);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }

        static void Main()
        {
            init();
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);
            Glut.glutInitWindowSize(1024, 768);
            Glut.glutCreateWindow("GameForProgrammers OpenGL Player v0.1");
            Glut.glutDisplayFunc(Display);
            //Glut.glutKeyboardFunc();
            Glut.glutIdleFunc(Display);
            WinInit();
            //Glut.glutTimerFunc(1000, FPS_Counter, 2);
            Glut.glutMainLoop();
            

            int playerRadius = 10;
            int playerscount = 4;

            
            int a = 0;
        }
    }
}
