using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using Tao.Lua;

namespace GameWorldPlayer
{
    static class GameText
    {
        static public void DrawText(string text, double x, double y, double h, double w, double red, double green, double blue)
      {
          Gl.glColor3ub(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue));
          for (int i = 0; i < text.Length; i++)
          {
              switch (text[i])
              {
                  case 'A':
                       Gl.glBegin(Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                       Gl.glBegin(Gl.GL_LINES);
                       Gl.glVertex2d(x, y + (h / 2));
                       Gl.glVertex2d(x + w, y + (h / 2));
                       Gl.glEnd();
                      break;

                  case 'B':
                       Gl.glBegin(Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y + h/2+h/7);
                       Gl.glVertex2d(x, y+h/2);
                       Gl.glVertex2d(x + w, y + h / 2 - h / 7);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;



                  case 'C':
                       Gl.glBegin(Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                      break;

                  case 'D':
                       Gl.glBegin(Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + 3 * h / 4);
                       Gl.glVertex2d(x + w, y + h / 4);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;

                  case 'E':
                       Gl.glBegin(Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                       Gl.glBegin(Gl.GL_LINES);
                       Gl.glVertex2d(x, y + (h / 2));
                       Gl.glVertex2d(x + w, y + (h / 2));
                       Gl.glEnd();
                      break;

                  case 'F':
                       Gl.glBegin(Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                       Gl.glBegin(Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + (h / 2));
                       Gl.glVertex2d(x + w, y + (h / 2));
                       Gl.glEnd();
                      break;

                  case 'G':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x + (w / 2), y + (h / 2));
                       Gl.glVertex2d(x + w, y + (h / 2));
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                      break;

                  case 'H':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case 'I':
                       Gl.glBegin( Gl.GL_LINES);
                       Gl.glVertex2d(x + w / 2 - 1, y);
                       Gl.glVertex2d(x + w / 2 - 1, y + h);
                       Gl.glVertex2d(x + w / 2, y);
                       Gl.glVertex2d(x + w / 2, y + h);
                       Gl.glVertex2d(x + w / 2 + 0.5, y);
                       Gl.glVertex2d(x + w / 2 + 0.5, y + h);
                       Gl.glEnd();
                      break;

                  case 'L':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case 'K':
                       Gl.glBegin( Gl.GL_LINES);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);

                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y + h);

                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case 'M':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w / 2, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case 'N':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                      break;

                  case 'O':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;

                  case 'P':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glEnd();
                      break;

                  case 'R':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case 'S':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;

                  case 'T':
                       Gl.glBegin( Gl.GL_LINES);
                       Gl.glVertex2d(x + (w / 2) - 1, y);
                       Gl.glVertex2d(x + (w / 2) - 1, y + h);
                       Gl.glVertex2d(x + (w / 2), y);
                       Gl.glVertex2d(x + (w / 2), y + h);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x, y + h - 0.5);
                       Gl.glVertex2d(x + w, y + h - 0.5);
                       Gl.glEnd();
                      break;

                  case 'U':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                      break;

                  case 'V':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w / 2, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                      break;

                  case 'W':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w / 2, y + h);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                      break;

                  case 'X':
                       Gl.glBegin( Gl.GL_LINES);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glEnd();
                      break;

                  case 'Y':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w / 2, y + 2 * (h / 3));
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w / 2, y + 2 * (h / 3));
                       Gl.glVertex2d(x + w / 2, y);
                       Gl.glEnd();
                      break;

                  case '0':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;


                  case '1':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w / 2, y + h);
                       Gl.glVertex2d(x + w / 2, y);
                       Gl.glEnd();
                      break;

                  case '2':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case '3':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;

                  case '4':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x,y+h);
                       Gl.glVertex2d(x,y+h/2);
                       Gl.glVertex2d(x+w,y+h/2);
                       Gl.glVertex2d(x+w,y+h);
                       Gl.glVertex2d(x+w,y);
                       Gl.glEnd();
                      break;

                  case '5':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;

                  case '6':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x, y + h / 2 + 1);
                       Gl.glEnd();
                      break;

                  case '7':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case '8':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x,y+h/2);
                       Gl.glVertex2d(x,y);
                       Gl.glVertex2d(x+w,y);
                       Gl.glVertex2d(x+w,y+h);
                       Gl.glVertex2d(x,y+h);
                       Gl.glVertex2d(x,y+h/2);
                       Gl.glVertex2d(x+w,y+h/2);
                       Gl.glEnd();
                      break;

                  case '9':
                       Gl.glBegin( Gl.GL_LINE_STRIP);

                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x, y + h / 2);
                       Gl.glVertex2d(x + w, y + h / 2);
                       Gl.glEnd();
                      break;

                  case '(':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x + w, y + h);
                       Gl.glVertex2d(x + w / 2, y + h * 3 / 4);
                       Gl.glVertex2d(x + w / 2, y + h / 4);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case ')':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y + h);
                       Gl.glVertex2d(x + w / 2, y + h * 3 / 4);
                       Gl.glVertex2d(x + w / 2, y + h / 4);
                       Gl.glVertex2d(x, y);
                       Gl.glEnd();
                      break;

                  case '.':
                       Gl.glBegin( Gl.GL_POINTS);
                       Gl.glVertex2d(x + w, y);
                       Gl.glEnd();
                      break;

                  case ':':
                       Gl.glBegin( Gl.GL_POINTS);
                       Gl.glVertex2d(x + w/2, y);
                       Gl.glVertex2d(x + w / 2, y+h);
                       Gl.glEnd();
                      break;

                  case '/':
                       Gl.glBegin( Gl.GL_LINE_STRIP);
                       Gl.glVertex2d(x, y);
                       Gl.glVertex2d(x + w, y+h);
                       Gl.glEnd();
                      break;



              }
              x += w + w;
          }
      }
    }
}
   