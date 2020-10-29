using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper_ypz
{
    public unsafe partial class Form1 : Form
    {

        private int n_horizontal;
        private int n_vertical;
        private int offset_left;
        private int offset_right;
        private int offset_top;
        private int n_mines;
        private int grid_length;
        public int offset_bottom;
        private int grid_length_eps;

        private string status;

        private bool[] mines_grid;
        private int[] traverseMatrix;

        private Font font_numbr;


        public unsafe Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.status = "on-going";

            this.font_numbr = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);

            offset_left = 5;                                   //初始化偏移量
            offset_right = 5;

            offset_top = menuBar.Size.Height + label_level.Size.Height + radioButton_easy.Size.Height + 5 ;
            offset_bottom = 5;
            
            //MessageBox.Show(String.Format("{0}", offset_top));

            n_mines = Properties.Settings.Default.n_mines;      //初始化，从Settings读取地雷数量
            n_horizontal = Properties.Settings.Default.n_horizontal;    //初始化，从Settings读取行数
            n_vertical = Properties.Settings.Default.n_vertical;  //初始化，从Settings读取列数

            grid_length_eps = 3;
            grid_length = Properties.Settings.Default.grid_length;
                    
            UpdateWindowSize(n_horizontal, n_vertical);  //自适应窗口大小

            // 初始化扫雷区
            this.mines_grid = InitializeGrid(n_vertical, n_horizontal, n_mines);
            this.traverseMatrix = InitializeTraverseMatrix(n_vertical, n_horizontal);

  

            int sum = 0;
            string _msg = "The mines are under ";
            for(int i = 0; i< n_horizontal* n_vertical; i++)
            {
                if(mines_grid[i] == true)
                {
                    _msg += i.ToString() + " ";
                    sum += 1;
                }
            }
            MessageBox.Show(_msg);

        }


        private bool[] InitializeGrid(int n_vertical, int n_horizontal, int n_mines)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/unsafe-code-pointers/pointer-types
            bool[] mines_grid = new bool[n_horizontal * n_vertical];

            for (int i = 0; i < n_mines; i++)
            {
                mines_grid[i] = false;
            }

            Random rnd = new Random();

            int _i = 0;
            while (_i < n_mines)
            {
                int mine_idx = rnd.Next(n_vertical * n_horizontal);
                if (mines_grid[mine_idx] == false)
                {
                    mines_grid[mine_idx] = true;
                    _i++;
                }
            }
            return mines_grid;
        }

        private int[] InitializeTraverseMatrix(int n_vertical, int n_horizontal)
        {
            int[] traverseMatrix = new int[n_vertical * n_horizontal];
            for (int i = 0; i < n_vertical * n_horizontal; i++)
            {
                traverseMatrix[i] = -1;
            }

            return traverseMatrix;
        }

        private void InitializeGrid_v2(bool[] mines_grid, int n_vertical, int n_horizontal, int n_mines)
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/unsafe-code-pointers/pointer-types
            mines_grid = new bool[n_horizontal * n_vertical];

            for (int i = 0; i < n_mines; i++)
            {
                mines_grid[i] = false;
            }

            Random rnd = new Random();

            int _i = 0;
            while (_i < n_mines)
            {
                int mine_idx = rnd.Next(n_vertical * n_horizontal);
                if (mines_grid[mine_idx] == false)
                {
                    mines_grid[mine_idx] = true;
                    _i++;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mine Sweeper Version 20H2. Microsoft 版權所有.");
            // 增加 title

            // 讓該彈出窗口放在程序正中間位置。
        }






        private void radioButton_easy_CheckedChanged(object sender, EventArgs e)
        {
            n_horizontal = 10;    //初始化，从Settings读取行数
            n_vertical = 10;  //初始化，从Settings读取列数
            n_mines = 15;

            this.status = "on-going";
            this.mines_grid = InitializeGrid(n_vertical, n_horizontal, n_mines);
            this.traverseMatrix = InitializeTraverseMatrix(n_vertical, n_horizontal);

            UpdateWindowSize(n_horizontal, n_vertical);
            this.Refresh();
        }

        private void radioButton_easy_Click(object sender, EventArgs e)
        {
            radioButton_easy_CheckedChanged(sender, e);
        }


        private void radioButton_median_CheckedChanged(object sender, EventArgs e)
        {
            // Set the mode to median.
            n_horizontal = 25;   
            n_vertical = 25;  
            n_mines = 80;

            this.status = "on-going";
            this.mines_grid = InitializeGrid(n_vertical, n_horizontal, n_mines);
            this.traverseMatrix = InitializeTraverseMatrix(n_vertical, n_horizontal);

            // Load a new GUI.
            UpdateWindowSize(n_horizontal, n_vertical);
            this.Refresh();
        }
        private void radioButton_median_Click(object sender, EventArgs e)
        {
            radioButton_median_CheckedChanged(sender, e);
        }

        private void radioButton_hard_CheckedChanged(object sender, EventArgs e)
        {
            // Set the mode to hard.
            n_horizontal = 30;    //初始化，从Settings读取行数
            n_vertical = 30;  //初始化，从Settings读取列数
            n_mines = 99;
       
            this.status = "on-going";
            this.mines_grid = InitializeGrid(n_vertical, n_horizontal, n_mines);
            this.traverseMatrix = InitializeTraverseMatrix(n_vertical, n_horizontal);

            // Load a new GUI.
            UpdateWindowSize(n_horizontal, n_vertical);
            this.Refresh();
        }

        private void radioButton_hard_Click(object sender, EventArgs e)
        {
            radioButton_hard_CheckedChanged(sender, e);
        }







        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Draw text on a rectangle.
            // https://docs.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-draw-wrapped-text-in-a-rectangle?view=netframeworkdesktop-4.8

            // color map
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.media.brushes?view=netcore-3.1

            Graphics g = e.Graphics;                                

            if(this.status == "on-going")
            {
                for (int i_v = 0; i_v < n_vertical; i_v++)
                {
                    for (int i_h = 0; i_h < n_horizontal; i_h++)
                    {
                        int index = n_horizontal * i_v + i_h;
                        Rectangle rec = new Rectangle(offset_left + grid_length * i_h, offset_top + grid_length * i_v, grid_length - grid_length_eps, grid_length - grid_length_eps);
                        if (this.traverseMatrix[index] >= 0) // has been traversed
                        {

                            
                            g.FillRectangle(Brushes.LightGray, rec);

                            string number_str = traverseMatrix[index].ToString();
                            g.DrawString(number_str, font_numbr, Brushes.Black, rec);
                            
                        }
                        else
                        {
                            g.FillRectangle(Brushes.DarkGray, rec);
                        }
                         
                    }
                }
            }else if(this.status == "failed")
            {
                for (int i_v = 0; i_v < n_vertical; i_v++)
                {
                    for (int i_h = 0; i_h < n_horizontal; i_h++)
                    {
                        int index = n_horizontal * i_v + i_h;
                        Rectangle rec = new Rectangle(offset_left + grid_length * i_h, offset_top + grid_length * i_v, grid_length - grid_length_eps, grid_length - grid_length_eps);
                        if (this.mines_grid[index] == true) 
                        {
                            g.FillRectangle(Brushes.Red, rec);

                            string number_str = "x";
                            g.DrawString(number_str, font_numbr, Brushes.Black, rec);
                            
                        }
                        else
                        {
                            g.FillRectangle(Brushes.DarkGray, rec);
                            if (this.traverseMatrix[index] >= 0)
                            {
                                string number_str = this.traverseMatrix[index].ToString();
                                g.DrawString(number_str, font_numbr, Brushes.Black, rec);
                            }
                        }

                    }
                }

            }


        }


        private void UpdateWindowSize(int n_hori, int n_vert)
        {
            this.Width = n_hori * grid_length +  offset_left + offset_right - grid_length_eps + (this.Size.Width - this.ClientSize.Width);
            this.Height = n_vert * grid_length + offset_top + offset_bottom - grid_length_eps + (this.Size.Height - this.ClientSize.Height); // + offset_top
            //int height_update = SystemInformation.CaptionHeight +10; // + offset_top
            //this.Width = width_update + (this.Size.Width - this.ClientSize.Width);
            //this.Height = high_update + 40 + (this.Size.Height - this.ClientSize.Height);
            Refresh();

        }      


       
        private void ClickGrid_xy(int i_h, int i_v)
        {
            int index = n_horizontal * i_v + i_h;

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Compute i_h and i_v

            if (this.status == "failed") 
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {

                bool cursor_in_grid = e.X >= offset_left && e.Y >= offset_top && e.X < this.ClientSize.Width - offset_right && e.Y < this.ClientSize.Height - offset_bottom;

                if (cursor_in_grid)
                {
                    int i_h = (e.X - offset_left) / grid_length;
                    int i_v = (e.Y - offset_top) / grid_length;
                    //string s1 = String.Format("ih = {0}, iv = {1}.", i_h, i_v);



                    //string s2 = String.Format("e.X = {0}, e.Y = {1}.", e.X, e.Y);
                    //MessageBox.Show(s2);

                    // Compute index
                    int index = n_horizontal * i_v + i_h;

                    if (index >= 0 && index < n_horizontal * n_vertical)
                    {
                        if (mines_grid[index] == true)
                        {
                            // Click a mine                  
                            this.status = "failed";
                            this.Refresh();
                            MessageBox.Show("You lose!");


                        }
                        else
                        {
                            // Click a blank, expand
                            ExploreGrid(i_v, i_h, n_vertical, n_horizontal, mines_grid, traverseMatrix);

                            string s1 = traverseMatrix[index].ToString() + " mines surrounded.";
                            //MessageBox.Show(s1);
                            this.Refresh();
                        }
                    }
                }
            }else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {

            }

        }


        private void ExploreGrid(int i_v, int i_h, int n_vertical, int n_horizontal, bool[] mines_grid, int[] traverseMatrix)
        {
            //std::cout << "in ExploreGrid...\n";

            //bool touch_border = (ix == 0) || (ix == nx - 1) || (iy == 0) || (iy == ny - 1);
            bool outside_border = (i_v < 0) || (i_v > n_vertical - 1) || (i_h < 0) || (i_h > n_horizontal - 1);

            int index = n_horizontal * i_v + i_h;

            if (outside_border == true)
            {
                return;
            }

            if (traverseMatrix[index] >= 0) // which means has been traversed.
            {
                return;
            }

            traverseMatrix[index] = 0;


            int number_of_mines_surrounded = CalculateMinesAround(i_v, i_h, n_vertical, n_horizontal, mines_grid, traverseMatrix);
            if (number_of_mines_surrounded > 0)
            {
                // 旁边有雷，或者触及到了边缘
                //std::cout << "結束遞歸\n";
                return; // 結束遞歸
            }
            else
            {
                ExploreGrid(i_v - 1, i_h - 1, n_vertical, n_horizontal, mines_grid, traverseMatrix);
                ExploreGrid(i_v - 1, i_h + 0, n_vertical, n_horizontal, mines_grid, traverseMatrix);
                ExploreGrid(i_v - 1, i_h + 1, n_vertical, n_horizontal, mines_grid, traverseMatrix);

                ExploreGrid(i_v, i_h - 1, n_vertical, n_horizontal, mines_grid, traverseMatrix);
                ExploreGrid(i_v, i_h + 1, n_vertical, n_horizontal, mines_grid, traverseMatrix);

                ExploreGrid(i_v + 1, i_h - 1, n_vertical, n_horizontal, mines_grid, traverseMatrix);
                ExploreGrid(i_v + 1, i_h + 0, n_vertical, n_horizontal, mines_grid, traverseMatrix);
                ExploreGrid(i_v + 1, i_h + 1, n_vertical, n_horizontal, mines_grid, traverseMatrix);
            }
        }

        private int CalculateMinesAround(int i_v, int i_h, int n_vertical, int n_horizontal, bool[] mines_grid, int[] traverseMatrix)
        {
            int index = n_horizontal * i_v + i_h;

            if(mines_grid[index] == true)
            {
                MessageBox.Show("ERROR!!!");
            }

            traverseMatrix[index] = 0;

            for (int i = i_v - 1; i <= i_v + 1; i++)
            {
                for (int j = i_h - 1; j <= i_h + 1; j++)
                {
                    int _idx_ = i * n_horizontal + j;

                    //if (_idx_ > 0 && _idx_ < n_vertical * n_horizontal)
                    if(i >=0 && i < n_vertical && j >= 0 && j < n_horizontal)
                    {
                        if (mines_grid[_idx_] == true)
                        {
                            //std::cout << "index = " << index << std::endl;
                            traverseMatrix[index]++;
                        }

                    }
                    else
                    {
                        continue;
                    }
                }

            }

            return traverseMatrix[index];

        }








        //private void ShowAllMines(MouseEventArgs e)
        //{
        //    Graphics g = e.Graphics;                                //绘制句柄
        //    for (int i = 0; i < n_vertical; i++)
        //    {
        //        for (int j = 0; j < n_horizontal; j++)
        //        {
        //            int index = n_horizontal * i_v + i_h;

        //            if (mines_grid[index] == true)
        //            {
        //                g.FillRectangle(Brushes.DarkGray, new Rectangle(offset_left + grid_length * i, offset_top + grid_length * j, grid_length - grid_length_eps, grid_length - grid_length_eps));
        //            }
        //            else {
        //                g.FillRectangle(Brushes.LightGray, new Rectangle(offset_left + grid_length * i, offset_top + grid_length * j, grid_length - grid_length_eps, grid_length - grid_length_eps)); 
        //            }

        //        }
        //    }
        //}
    }
}
