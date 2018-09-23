using System;
namespace CS_Tutorials
{
    public class Rectangle
    {
        public int l;
        public int h;

        public Rectangle(int l, int h)
        {
            this.l = l;
            this.h = h;
        }

        public int Area() { return (this.h * this.l); }

    }
}
