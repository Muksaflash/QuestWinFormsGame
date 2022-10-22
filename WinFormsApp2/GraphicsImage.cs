using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    public class GraphicsImage
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public Image image;

        public GraphicsImage(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            image = new Bitmap("rocket.png");
            this.width = width;
            this.height = height;
        }
    }
}
