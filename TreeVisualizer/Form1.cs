using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeVisualizer
{
    public partial class Form1 : Form
    {
        Pen myPen = new Pen(Color.Black, 3);
        Graphics g = null;
        BTree A = new BTree();

        public Form1()
        {
            InitializeComponent();
            A.Insert(2);
            A.Insert(15);
            A.Insert(5);
            A.Insert(3);
            A.Insert(16);
            A.Insert(25);
            A.Insert(9);
            A.Insert(12);
            A.Insert(1);
            A.Insert(-1);
            A.Insert(3);
            A.Insert(12);
            A.Insert(1);
            A.Insert(-1);
            A.Insert(3);
            A.Insert(12);
            A.Insert(1);
            A.Insert(-1);
            A.Insert(3);
            A.Insert(1);
            A.Insert(-1);
       
            
            
            A.Update(this.pnScreen,null);
            A.Travesal();
        }

        private void pnScreen_Paint(object sender, PaintEventArgs e)
        {
            g = this.pnScreen.CreateGraphics();
            A.Draw(g, myPen, A.Root);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            A = new BTree();
            pnScreen.Refresh();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                int keyToAdd = Convert.ToInt32(txtbInsert.Text);
                A.Insert(keyToAdd);
                A.Update(pnScreen,null);
                pnScreen.Refresh();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int keyToSearch = Convert.ToInt32(txtbSearch.Text);
                Block blockFound = A.Search(A.Root, keyToSearch);
                if (blockFound != null)
                {
                    blockFound.Draw(g, new Pen(new SolidBrush(Color.LightSkyBlue)), keyToSearch);
                    MessageBox.Show("Found!!!");
                    Thread.Sleep(3000);
                    //tu dong goi ham Paint va ve lai nhu ban dau truoc khi search
                    pnScreen.Refresh();
                }
                else
                {
                    MessageBox.Show("Not Found!!!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            A.Update(pnScreen, null);
            pnScreen.Refresh();
        }
    }
}
