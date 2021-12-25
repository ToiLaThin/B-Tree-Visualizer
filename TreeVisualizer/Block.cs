using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeVisualizer
{
    class Block
    {
        #region Field && Properties
        //read-only propertis
        const int width = 20;
        public int Width
        {
          get { return width; }  
        }

        const int height = 20;
        public int Height
        {
            get { return height; }
        }

        //toa do 1 diem ben tren trai cua block do
        int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        private SolidBrush br;
        #endregion

        #region Methods
        public Block()
        {
            this.x = 4;
            this.y = 4;
            this.br = new SolidBrush(Color.Red);
        }
        public void Draw(Graphics g,Pen p,int key)
        {
            g.DrawRectangle(p, this.x, this.y, this.Width, this.Height);
            g.DrawString(key.ToString(), SystemFonts.DefaultFont, br, this.x + 4, this.y + 4);
        }
        #endregion

    }
}
