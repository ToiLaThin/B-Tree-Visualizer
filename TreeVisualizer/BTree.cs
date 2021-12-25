using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeVisualizer
{
    class BTree
    {
        BTreeNode root;
        public BTreeNode Root
        {
            get { return root; }
            set { root = value; }
        }

        public BTree()
        {
            root = new BTreeNode();
        }

        public void Travesal()
        {
            root.Travesal();
        }

        public void Insert(int val)
        {
            if (root == null)
            {
                root = new BTreeNode();
                root.NKeys = 1;
                root.Keys[0] = val;
            }
            else if (root.NKeys== BTreeNode.ORDER-1)
            {
                //neu day nut tao nut moi lam parent  va split nut hien tai
                BTreeNode temp = null; temp=new BTreeNode(); temp.Childs[0] = root;
                temp.SplitChild(0, temp.Childs[0]);

                //cap nhat nut goc nam tren nut goc cu va khong con la la
                root = temp; root.IsLeaf= false;
                if (val <= temp.Keys[0])
                    root.Childs[0].InsertLeafRe(val);
                else
                    root.Childs[1].InsertLeafRe(val);
            }
            else
                root.InsertLeafRe(val);
        }

        //ham de quy;tham so node la nut cha cua nut can cap nhat LEVEL
        public void Update(Panel pnl,BTreeNode node)
        {
            if (node == null)//Thuc hien lan dau | doi thu tu kiem ktra null truoc thi ko bi loi object reference is not set to null
            {
                root.UpdateLevel(node);
                root.UpdateCordinations(pnl);//moi chi thu update root
                if (root.IsLeaf)
                    return;

                root.Childs[0].UpdateLevel(root);
                //TODO update toa do cua cac child va toa do cua cac keyBlocks
                //chay 1 lan o child 0 chu ko con trong vong for
                //vi ban chat ham UpdateCordination se thuc hien voi tat ca cac child(node.Childs[0] chi la hinh thuc de goi ham nay len)
                root.Childs[0].UpdateCordinations(pnl, root);
                this.Update(pnl, root.Childs[0]);
                for (int i = 1; i <= root.NKeys; i++)//tu 1
                {
                    BTreeNode temp = root.Childs[i];
                    root.Childs[i].UpdateLevel(root);
                    this.Update(pnl,temp);
                }
            }
            else if (node.IsLeaf){ return; }//La leaf thi dung
            else
            {
                node.Childs[0].UpdateLevel(node);
                //TODO update toa do cua cac child va toa do cua cac keyBlocks
                //chay 1 lan o child 0 chu ko con trong vong for
                //vi ban chat ham UpdateCordination se thuc hien voi tat ca cac child(node.Childs[0] chi la hinh thuc de goi ham nay len)
                node.Childs[0].UpdateCordinations(pnl,node);
                this.Update(pnl, node.Childs[0]);
                for (int i = 1; i <= node.NKeys; i++)//tu 1
                {
                    BTreeNode temp= node.Childs[i];
                    node.Childs[i].UpdateLevel(node);
                    this.Update(pnl,temp);
                }
            }
        }

        //node phai thuoc BTree va ta se truyen vao root
        public void Draw(Graphics g,Pen p,BTreeNode node)
        {
            if (node == null || node.NKeys == 0)
                return;
            node.Draw(g, p);
            for (int i = 0; i < node.NKeys; i++)
            {
                if (!node.IsLeaf)
                {
                    this.Draw(g,p,node.Childs[i]);
                    g.DrawLine(new Pen(Color.Black), node.PointLineFrs[i], node.Childs[i].PointLineTo);
                }
            }
            if (!node.IsLeaf)//KTRA DE TRANH LOI OBJECT REF IS NOT SET TO AN INSTANCE VI KO THE DUNG CAC PROPERTIES CUA CLASS NEU CLASS LA NULL
            {
                this.Draw(g, p, node.Childs[node.NKeys]);
                g.DrawLine(new Pen(Color.Black), node.PointLineFrs[node.NKeys], node.Childs[node.NKeys].PointLineTo);
            }
        }

        public Block Search(BTreeNode node,int val)
        {
            //TH la nut leaf
            if (node.IsLeaf)
            {
                for (int i = 0; i < node.NKeys; i++)
                {
                    if (val == node.Keys[i])
                        return node.KeyBlocks[i];
                    else if (val < node.Keys[i])//vi la leaf nen ko con child de search trong child nen ko tim thay
                        return null;
                }
                //da kiem tra het key ma ko tim thay
                return null;
            }
            //TH la nut internal
            for (int i = 0; i < node.NKeys; i++)
            {
                if (val == node.Keys[i])
                    return node.KeyBlocks[i];

                //di xuong child[i] de  search vi: node->arrKey[i-1]< val <node->arrKey[i]
                else if (val < node.Keys[i])
                    return Search(node.Childs[i], val);
            }
            //di vao child cuoi de search
            return Search(node.Childs[node.NKeys], val);
        }
    }


    class BTreeNode
    {   
        #region Field && Properties
        static int order = 5;
        public static int ORDER
        {
            get { return BTreeNode.order; }
            set { BTreeNode.order = value; }
        }

        int nKeys;
        public int NKeys
        {
            get { return nKeys; }
            set { nKeys = value; }
        }

        int[] keys; 
        public int[] Keys
        {
            get { return keys; }
            set { keys = value; }
        }

        bool isLeaf;
        public bool IsLeaf
        {
            get { return isLeaf; }
            set { isLeaf = value; }
        }

        int level;
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }


        BTreeNode[] childs; 
        internal BTreeNode[] Childs
        {
            get { return childs; }
            set { childs = value; }
        }

        Block[] keyBlocks;
        internal Block[] KeyBlocks
        {
            get { return keyBlocks; }
            set { keyBlocks = value; }
        }

        Point[] pointLineFrs;
        public Point[] PointLineFrs
        {
            get { return pointLineFrs; }
            set { pointLineFrs = value; }
        }

        Point pointLineTo;
        public Point PointLineTo
        {
            get { return pointLineTo; }
            set { pointLineTo = value; }
        }

        #endregion

        public BTreeNode()
        {
            this.nKeys = 0; this.level = 0; this.width = 0; this.height = 0;
            this.isLeaf = true;
            this.keys= new int[ORDER - 1];
            this.childs = new BTreeNode[ORDER];
            this.keyBlocks = new Block[ORDER - 1];
            //tranh error object is not set to an reference
            for (int i = 0; i < ORDER - 1; i++)
                this.keyBlocks[i] = new Block();
            this.pointLineFrs = new Point[ORDER];
            this.pointLineTo = new Point();
        }

        public void Travesal()
        {
            if (this == null || this.NKeys == 0)
                return;
            for (int i = 0; i < this.NKeys; i++)
            {
                if (!this.IsLeaf) 
                    this.Childs[i].Travesal();
                Console.WriteLine(this.Keys[i]);
            }
            if (!this.IsLeaf)//KTRA DE TRANH LOI OBJECT REF IS NOT SET TO AN INSTANCE VI KO THE DUNG CAC PROPERTIES CUA CLASS NEU CLASS LA NULL
                this.Childs[this.NKeys].Travesal();
        }
        
        public void SplitChild(int pos,BTreeNode childToSplit)
        {
	        BTreeNode newChild=new BTreeNode();
            newChild.isLeaf = childToSplit.isLeaf; //newChild chua values lon(ben trai)
	        int mid, nleft, nright;
	        //xac dinh index phan tu day len dua vao ORDER
	        if ((ORDER - 1) % 2 == 1){
		        mid = (ORDER - 1) / 2;
		        nright = (ORDER - 1) / 2;
		        nleft = nright;
            }
            else
            {
		        mid = (ORDER - 1) / 2 - 1;
		        nright= (ORDER - 1) / 2;
		        nleft = nright - 1;
            }
		     //copy cac key
		     newChild.nKeys = nright;
		     for (int i = 0; i < newChild.nKeys; i++)
			    newChild.Keys[i] = childToSplit.Keys[mid + 1 + i];//bo phan tu mid=>chay tu mid + 1
		     //copy cac child cua node child
		     for (int i = 0; i <= newChild.nKeys; i++)
			    newChild.Childs[i] = childToSplit.Childs[mid + 1 + i];

		     //cap nhat child
		     childToSplit.nKeys = nleft;

		     //cap nhat parent
		     this.nKeys++;
		     for (int i = this.nKeys; i > pos + 1; i--)
                this.Childs[i] = this.Childs[i - 1];
		     this.Childs[pos + 1] = newChild;//pos la vi tri cua child bi split trong parent

		     //doi cac key cua parent ve phai
		     for (int i = this.nKeys - 1; i > pos; i--)
			    this.Keys[i] = this.Keys[i - 1];
		     //cap nhat key cua parent tai pos
		     //luu y child -> nKeys chinh la mid=(ORDER-1)/2 
		     //index cua child sau khi split la 0->(ORDER-1)/2-1
		     this.Keys[pos] = childToSplit.Keys[mid];
        }

        int GetPosToIns(int val, int[] Keys, int nKeys)
        {
	        int i;
	        for (i = 0; i < nKeys; i++)
		        if (val < Keys[i])
			        return i;
	        return i;//vi tri cuoi mang
        }

        public void InsertLeafRe(int val)
        {
	        int pos = GetPosToIns(val, this.Keys,this.nKeys);
	        if (this.IsLeaf)//neu la leaf thi chac chan chua day
	        {
		        for (int i = this.nKeys; i > pos; i--)
			        this.Keys[i] = this.Keys[i - 1];
		        //chen va doi so key
		        this.Keys[pos] = val;
		        this.nKeys++;
	        }
	        else if (!this.IsLeaf)
	        {
		        if (this.Childs[pos].nKeys == ORDER - 1)
		        {
			        this.SplitChild(pos, this.Childs[pos]);
			        if (val < this.Keys[pos])//cap nhat vi tri pos sau khi co nut moi 
				        pos = pos;
			        else
				        pos = pos + 1;
		        }
		        this.Childs[pos].InsertLeafRe(val);
	        }
        }

        //ve 1 node bao gom nhieu ham ve 1 block
        public void Draw(Graphics g,Pen p)
        {
            for (int i = 0; i < this.nKeys; i++)//neu chua insert phan tu nao ma ve thi vong lap nay ko chay 1 lan nao
            {
                try
                {
                    this.keyBlocks[i].Draw(g, p,keys[i]);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        //cap nhat lai chieu cao cua moi node( ham nay duoc wrap lai trong ham update cua BTree)
        public void UpdateLevel(BTreeNode parent)
        {
            if (parent == null)
                this.level = 0;
            else
                this.level = parent.level + 1;
        }

        //update toa do cac block 1 node duawj vao block dau tien[0]
        //x, y ,width la thong so cua Block[0]
        void ConfigureBlocksCordinations()
        {
            for (int i = 1; i < nKeys; i++)
            {
                this.keyBlocks[i].X = this.keyBlocks[i - 1].X + keyBlocks[0].Width;
                this.keyBlocks[i].Y = this.keyBlocks[i - 1].Y;
            }
        }

        //update toa do cac diem de ve duong dua vao diem dau[0]
        //diem di[0] da co toa do
        void ConfigureLinesFromCordinations()
        {
            int i;
            for (i = 1; i < nKeys; i++)
            {
                this.pointLineFrs[i].X = this.pointLineFrs[i - 1].X + keyBlocks[0].Width;
                this.pointLineFrs[i].Y = this.pointLineFrs[i - 1].Y;
            }
            this.pointLineFrs[i].X = this.pointLineFrs[i - 1].X + keyBlocks[0].Width;
            this.pointLineFrs[i].Y = this.pointLineFrs[i - 1].Y;
        }

        //tinh toan ra chieu dai va rong cua 1 node dua vao nKeys hien hanh
        void ConfigureNodeSize()
        {
            if (this.nKeys == 0)
            {
                this.width = 0;
                this.height = 0;
                return;
            }
            else if (this.nKeys > 0)
            {
                this.width = this.nKeys * this.keyBlocks[0].Width;
                this.height = this.keyBlocks[0].Height;
            }
        }

        //cap nhat lai toa do cac block | kich thuoc cua 1 node
        //chu yeu se tinh toan toa do cua block[0] o tung child/node roi goi ham ConfigureBlockCordinations()
        //co su dung ham ConfigureNodeSize
        public void UpdateCordinations(Panel pnl,BTreeNode parent=null)//optional parameter
        {
            if(this.Level==0)//th la  root
            {
                this.ConfigureNodeSize();
                this.keyBlocks[0].X = Convert.ToInt32(pnl.Width / 2 - this.width / 2);
                this.keyBlocks[0].Y = this.level * 80;
                this.ConfigureBlocksCordinations();

                this.pointLineFrs[0].X = this.keyBlocks[0].X;
                this.pointLineFrs[0].Y = this.keyBlocks[0].Y + this.keyBlocks[0].Height;
                this.ConfigureLinesFromCordinations();
            }
            else//th khac root
            {
                //dinh nghia khoang cach giua cac node con | dung cho viec tinh toa do ve sau 
                //cang xuong sau thi khoang cach nay cang giam de tranh ti le dung phai con node khac
                int ChildsSpace;
                int baseSpace = 250;
                ChildsSpace = baseSpace / (parent.childs[0].level*parent.childs[0].level);
                //nen nho parent.childs[0] la 1 BTreeNode
                //xac dinh toa do block[0] cua cac child cua node parent
                if(parent.level==0)
                    parent.childs[0].keyBlocks[0].X = (parent.keyBlocks[0].X / 2) - parent.childs[0].keyBlocks[0].Width;
                else
                    parent.childs[0].keyBlocks[0].X = parent.keyBlocks[0].X - parent.childs[0].keyBlocks[0].Width - 150 + 150/(2*parent.childs[0].level);//khoang cach cua con dau tien so voi cha
                parent.childs[0].keyBlocks[0].Y = parent.childs[0].level*80;
                parent.childs[0].ConfigureBlocksCordinations();
                //cap nhat cac lineFrom cua child[0] cua parent
                parent.childs[0].pointLineFrs[0].X = parent.childs[0].keyBlocks[0].X;
                parent.childs[0].pointLineFrs[0].Y = parent.childs[0].keyBlocks[0].Y + parent.childs[0].keyBlocks[0].Height;
                parent.childs[0].ConfigureLinesFromCordinations();
                //cap nhat 1 lineto cua child[0] cua parent| cho luon la toa do cua keyBlock[0]
                parent.childs[0].pointLineTo.X = parent.childs[0].keyBlocks[0].X;
                parent.childs[0].pointLineTo.Y = parent.childs[0].keyBlocks[0].Y;



                for (int i = 1; i <= parent.nKeys; i++)//chay tu 1
                {
                    //tinh toan ra width de tinh toa do keyBlock[0] cua tung CHILD cua parent
                    parent.childs[i - 1].ConfigureNodeSize();
                    parent.childs[i].keyBlocks[0].X = parent.childs[i - 1].keyBlocks[0].X + parent.childs[i - 1].width + ChildsSpace;
                    parent.childs[i].keyBlocks[0].Y = parent.childs[i - 1].keyBlocks[0].Y;
                    parent.childs[i].ConfigureBlocksCordinations();


                    //PHAN NAY CO THE BO VAO 1 HAM RIENG 1 THAM SO LA 1 bTREENODE
                    //cap nhat cac lineFrom cua child thu [i] cua parent
                    parent.childs[i].pointLineFrs[0].X = parent.childs[i].keyBlocks[0].X;
                    parent.childs[i].pointLineFrs[0].Y = parent.childs[i].keyBlocks[0].Y + parent.childs[0].keyBlocks[0].Height;
                    parent.childs[i].ConfigureLinesFromCordinations();

                    //cap nhat 1 lineto cua child thu [i] cua parent| cho luon la toa do cua keyBlock[0]
                    parent.childs[i].pointLineTo.X = parent.childs[i].keyBlocks[0].X;
                    parent.childs[i].pointLineTo.Y = parent.childs[i].keyBlocks[0].Y;
                }
            }
        }
    }
}
