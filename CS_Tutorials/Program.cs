using System;

namespace CS_Tutorials
{
  class Program
    {
        struct RectangleViewer
        {
            public int ReactangleArea(Rectangle rec)
            {
                return rec.Area();
            }
        }
        //static void Main(string[] args)
        //{
        //    Rectangle rec = new Rectangle(2,1);
        //    RectangleViewer rectangleViewer = new RectangleViewer();
        //    //rec.l = 2;
        //    //rec.h = 1;
        //    Console.WriteLine("The area is {0}, the length is {1}, the height is {2}", rectangleViewer.ReactangleArea(rec), rec.l, rec.h);
        //}
    }
}
