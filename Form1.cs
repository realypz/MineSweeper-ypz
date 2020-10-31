// https://github.com/realypz/MineSweeper-ypz

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
        // Constants for the definition of mines grid
        private int n_mines;
        private int n_horizontal;
        private int n_vertical;
        private int fail_at_index;


        // Constants for the grid visualization and text drawing
        private int offset_left;
        private int offset_right;
        private int offset_top;
        private int grid_length;
        private int offset_bottom;
        private int grid_length_eps;
        private Font font_numbr;
        private int offset_text_display_h;
        private int offset_text_display_v;


        // Game state variables
        private bool[] mines_grid; // where 1 means there is a mine.
        private int[] traverseMatrix; // All the values initialized as -1. If the grid cell has been explored, then then the value will be how many mines around this grid.
        private bool[] markMatrix; // where 1 means marked as a mine by the player.
        private string status; // one of "on-going", "failed", "succeed"


        public unsafe Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;


            // Constants for the grid and text drawing
            grid_length_eps = 2;
            grid_length = Properties.Settings.Default.grid_length;

            offset_left = 5;                                   
            offset_right = 5;

            offset_top = menuBar.Size.Height + label_level.Size.Height + radioButton_easy.Size.Height + 5 ;
            offset_bottom = 5;

            offset_text_display_v = 3;
            offset_text_display_h = 4;
            this.font_numbr = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);


            // Constants for the definition of mines grid
            n_mines = Properties.Settings.Default.n_mines;
            n_horizontal = Properties.Settings.Default.n_horizontal;
            n_vertical = Properties.Settings.Default.n_vertical;

            IntializeGame(n_vertical, n_horizontal, n_mines);
        }

        private void InitializeGrid(bool[] mines_grid, bool[] markMatrix, int[] traverseMatrix, int n_all_grids, int n_mines)
        {   // Initialize the grid of mines. Randomly assign the mines over the grids.

            for (int i = 0; i < n_all_grids; i++)
            {
                mines_grid[i] = false;
                markMatrix[i] = false;
                traverseMatrix[i] = -1;
            }

            // Randomly assign the mines under the grids cells.
            for(int i = 0; i< n_mines; i++)
            {
                mines_grid[i] = true;
            }

            Shuffle(mines_grid, n_all_grids);
        }

        private void Shuffle(bool[] arr, int N_all)
        {
            Random rnd = new Random();
            for(int i = N_all - 1; i >= 0; i--)
            {
                int random_index = rnd.Next(i+1);
                bool temp_cell = arr[random_index];
                arr[random_index] = arr[i];
                arr[i] = temp_cell;
            }
        }

        private void IntializeGame(int n_vertical, int n_horizontal, int n_mines)
        {   // A template of the intialization for all three modes.
            this.status = "on-going";
            this.label_status.Text = "on-going";
            this.label_status.ForeColor = System.Drawing.Color.Blue;

            this.fail_at_index = -1;

            this.markMatrix = new bool[n_vertical * n_horizontal];
            this.mines_grid = new bool[n_horizontal * n_vertical];
            this.traverseMatrix = new int[n_vertical * n_horizontal];

            InitializeGrid(this.mines_grid, this.markMatrix, this.traverseMatrix, n_vertical * n_horizontal, n_mines);

            UpdateWindowSize(n_horizontal, n_vertical);
            this.Refresh();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/realypz/MineSweeper-ypz");
            MessageBox.Show("Mine Sweeper Version 20H2. realypz 版權所有.");
        }

        private void radioButton_easy_CheckedChanged(object sender, EventArgs e)
        {   // Set the mode to easy.
            n_horizontal = 12;   
            n_vertical = 10; 
            n_mines = 15;

            IntializeGame(n_vertical, n_horizontal, n_mines);
        }

        private void radioButton_easy_Click(object sender, EventArgs e)
        {
            radioButton_easy_CheckedChanged(sender, e);
        }

        private void radioButton_median_CheckedChanged(object sender, EventArgs e)
        {   // Set the mode to median.
            n_horizontal = 25;   
            n_vertical = 20;  
            n_mines = 70;

            IntializeGame(n_vertical, n_horizontal, n_mines);
        }

        private void radioButton_median_Click(object sender, EventArgs e)
        {
            radioButton_median_CheckedChanged(sender, e);
        }

        private void radioButton_hard_CheckedChanged(object sender, EventArgs e)
        {   // Set the mode to hard.
            n_horizontal = 30;    
            n_vertical = 20;  
            n_mines = 80;

            IntializeGame(n_vertical, n_horizontal, n_mines);
        }

        private void radioButton_hard_Click(object sender, EventArgs e)
        {
            radioButton_hard_CheckedChanged(sender, e);
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {   // All the grids are drawn by this function.
            // this.markMatrix, this.mines_grid, this.traverseMatrix and this.status are the four variables to determine how to fill the colors and text of the grids.

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
                        Rectangle rec_for_text = new Rectangle(offset_left+ offset_text_display_h + grid_length * i_h, offset_top + offset_text_display_v + grid_length * i_v, grid_length - grid_length_eps, grid_length - grid_length_eps);
                        if (this.traverseMatrix[index] >= 0) // has been traversed
                        {
                            g.FillRectangle(Brushes.LightGray, rec);

                            string number_str = this.traverseMatrix[index].ToString();
                            g.DrawString(number_str, font_numbr, Brushes.Black, rec_for_text);
                            
                        }
                        else if (this.markMatrix[index] == true)
                        {
                            g.FillRectangle(Brushes.DarkGray, rec);
                            g.DrawString("?", font_numbr, Brushes.Blue, rec_for_text);
                        }
                        else
                        {
                            g.FillRectangle(Brushes.DarkGray, rec);
                        }
                         
                    }
                }
            }else // this.status == "failed" or "succeed"
            {
                for (int i_v = 0; i_v < n_vertical; i_v++)
                {
                    for (int i_h = 0; i_h < n_horizontal; i_h++)
                    {
                        int index = n_horizontal * i_v + i_h;
                        Rectangle rec = new Rectangle(offset_left + grid_length * i_h, offset_top + grid_length * i_v, grid_length - grid_length_eps, grid_length - grid_length_eps);
                        Rectangle rec_for_text = new Rectangle(offset_left + offset_text_display_h + grid_length * i_h, offset_top + offset_text_display_v + grid_length * i_v, grid_length - grid_length_eps, grid_length - grid_length_eps);

                        if (this.mines_grid[index] == true) 
                        {
                            if(index == this.fail_at_index)    
                                g.FillRectangle(Brushes.DarkRed, rec);

                            else   
                                g.FillRectangle(Brushes.Red, rec);

                            if(this.markMatrix[index] == true)  
                                g.DrawString("?", font_numbr, Brushes.Blue, rec_for_text);

                        }
                        else
                        {
                            if(this.traverseMatrix[index] >= 0)
                                g.FillRectangle(Brushes.LightGray, rec);
                            else
                                g.FillRectangle(Brushes.DarkGray, rec);

                            if (this.traverseMatrix[index] >= 0)
                            {
                                string number_str = this.traverseMatrix[index].ToString();
                                g.DrawString(number_str, font_numbr, Brushes.Black, rec_for_text);
                            }else if (this.markMatrix[index] == true)
                                g.DrawString("?", font_numbr, Brushes.Blue, rec_for_text);
                        }

                    }
                }

            }

        }


        private void UpdateWindowSize(int n_hori, int n_vert)
        {   // When you swith to another level, the entire windows will be redrawn according to the grid size.
            this.Width = n_hori * grid_length +  offset_left + offset_right - grid_length_eps + (this.Size.Width - this.ClientSize.Width);
            this.Height = n_vert * grid_length + offset_top + offset_bottom - grid_length_eps + (this.Size.Height - this.ClientSize.Height); // + offset_top
            this.Refresh();
        }      


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.status != "on-going") 
                return;

            bool cursor_in_grid = e.X >= offset_left && e.Y >= offset_top && e.X < this.ClientSize.Width - offset_right && e.Y < this.ClientSize.Height - offset_bottom;

            if (!cursor_in_grid) return;


            // compute the coordinate (i_h, i_v) of this gird where the mouse clicks.
            int i_h = (e.X - offset_left) / grid_length;
            int i_v = (e.Y - offset_top) / grid_length;

            // Compute index
            int index = n_horizontal * i_v + i_h;

            if (index >= 0 && index < n_horizontal * n_vertical)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left) { 
                    
                    if (this.markMatrix[index] == false && this.traverseMatrix[index] < 0)
                        ExploreGrid(i_v, i_h, n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);                    
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if(this.traverseMatrix[index] < 0)
                        this.markMatrix[index] = !this.markMatrix[index];
                }
                else if(e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    if(this.markMatrix[index] == false && this.traverseMatrix[index] >= 0)
                    {
                        int number_of_mines_surrounded = Calculate_mines_or_marks_Around(i_v, i_h, n_vertical, n_horizontal, this.mines_grid);
                        int number_of_marked_mines_surrounded = Calculate_mines_or_marks_Around(i_v, i_h, n_vertical, n_horizontal, this.markMatrix); 
                            
                        if(number_of_mines_surrounded == number_of_marked_mines_surrounded)
                        {
                            ExploreGrid(i_v -1 , i_h - 1, n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);
                            ExploreGrid(i_v -1 , i_h    , n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);
                            ExploreGrid(i_v -1 , i_h + 1, n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);
                            ExploreGrid(i_v,     i_h - 1, n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);
                            ExploreGrid(i_v,     i_h + 1, n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);
                            ExploreGrid(i_v + 1, i_h - 1, n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);
                            ExploreGrid(i_v + 1, i_h    , n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);
                            ExploreGrid(i_v + 1, i_h + 1, n_vertical, n_horizontal, this.mines_grid, this.traverseMatrix, this.markMatrix);

                        }
                    }
                }
                    
            }

            this.Refresh();

            if (CheckSucceed())
            {
                this.status = "succeed";
                this.label_status.Text = "succeed!";
                this.label_status.ForeColor = System.Drawing.Color.Green;
                
                MessageBox.Show("Succeed");
            }

            this.Refresh();
        }


        private bool CheckSucceed()
        {
            for(int i = 0; i < this.n_vertical * this.n_horizontal; i++)
            {

                bool success_i = (this.mines_grid[i] && this.markMatrix[i]) || (!this.mines_grid[i] && !this.markMatrix[i] && (this.traverseMatrix[i] >= 0));
                if (success_i == false)
                {
                    return false;
                }
            }
            return true;
        }

        private void ExploreGrid(int i_v, int i_h, int n_vertical, int n_horizontal, bool[] mines_grid, int[] traverseMatrix, bool[] markMatrix)
        {
            bool outside_border = (i_v < 0) || (i_v > n_vertical - 1) || (i_h < 0) || (i_h > n_horizontal - 1);

            if (outside_border == true)
                return;

            int index = n_horizontal * i_v + i_h;

            if (markMatrix[index] == true)
                return;

            if (mines_grid[index] == true)
            {
                // Click a mine                  
                this.status = "failed";
                this.fail_at_index = index;

                this.label_status.ForeColor = System.Drawing.Color.Red;
                this.label_status.Text = "failed";

                this.Refresh();
                MessageBox.Show("You lose!");
                return;
            }

            if (traverseMatrix[index] >= 0) // which means has been traversed.
                return;

            traverseMatrix[index] = Calculate_mines_or_marks_Around(i_v, i_h, n_vertical, n_horizontal, mines_grid);

            if (traverseMatrix[index] > 0)
                return;
            else
            {
                ExploreGrid(i_v - 1, i_h - 1, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);
                ExploreGrid(i_v - 1, i_h + 0, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);
                ExploreGrid(i_v - 1, i_h + 1, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);

                ExploreGrid(i_v, i_h - 1, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);
                ExploreGrid(i_v, i_h + 1, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);

                ExploreGrid(i_v + 1, i_h - 1, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);
                ExploreGrid(i_v + 1, i_h + 0, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);
                ExploreGrid(i_v + 1, i_h + 1, n_vertical, n_horizontal, mines_grid, traverseMatrix, markMatrix);
            }
        }

        private int Calculate_mines_or_marks_Around(int i_v, int i_h, int n_vertical, int n_horizontal, bool[] arr2D)
        {
            int index = n_horizontal * i_v + i_h;
            int sum = 0;

            for (int i = i_v - 1; i <= i_v + 1; i++)
            {
                for (int j = i_h - 1; j <= i_h + 1; j++)
                {
                    int _idx_ = i * n_horizontal + j;
                    if(i >=0 && i < n_vertical && j >= 0 && j < n_horizontal && _idx_ != index)
                    {   
                        if (arr2D[_idx_] == true)         sum ++;
                    }
                }
            }
            return sum;
        }
    }
}
