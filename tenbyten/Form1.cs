using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tenbyten
{
    public partial class Tenbyten : Form
    {

        int offs_w = 1;
        int offs_h = 1;

        List<List<int>> mask = new List<List<int>>()
        {
            new List<int>() { 0, 1, 0 },
            new List<int>() { 0, 1, 1 },
            new List<int>() { 0, 0, 0 },
        };

        int cur_sco;
        int score;

        Mask.MaskSet masks = new Mask.MaskSet();

        Random random = new Random();

        List<Point> valid_point;
        Point cur_point = new Point();
        int s_row;
        int s_col;
        int e_row;
        int e_col;
        int w_offs;
        int h_offs;

        List<List<int>> occupency = new List<List<int>>();

        public Tenbyten()
        {
            InitializeComponent();

            init_main_display();
            init_tmp_display();
            set_random_mask();
            this.valid_point = find_valid_position(this.mask);
            Console.WriteLine(valid_point.ToString());
            this.score = 0;
        }

        private void init_main_display()
        {
            int size_grid = main_display.RowCount;
            for (int i = 0; i < size_grid; i++)
            {
                List<int> temp = new List<int>();
                for (int j = 0; j < size_grid; j++)
                {
                    temp.Add(0);
                }
                this.occupency.Add(temp);
            }
            foreach (Control control in main_display.Controls)
            {
                control.Text = "";
            }
        }

        private void init_tmp_display()
        {
            int w = tmp_display0.ColumnCount;
            int h = tmp_display0.RowCount;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Label new_label = new Label();
                    new_label.Name = "Label" + i.ToString() + j.ToString();
                    tmp_display0.Controls.Add(new_label, i, j);
                }
            }
        }

        private void set_random_mask()
        {
            int c_mask = this.masks.mask_list.Count;
            int r_num = random.Next(c_mask);
            this.mask = this.masks.mask_list[r_num];
            this.cur_sco = this.masks.score_list[r_num];
        }

        private void draw_tmp_display()
        {
            // iterate through the current mask
            int w = this.mask.Count;
            int h = this.mask[0].Count;
            Label tmp_label;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    tmp_label = tmp_display0.GetControlFromPosition(j, i) as Label;
                    if (this.mask[i][j] == 1)
                    {
                        tmp_label.BackColor = Color.Green;
                    }
                    else
                    {
                        tmp_label.BackColor = Color.White;
                    }
                }
            }
        }

        private void click_main(object sender, EventArgs e)
        {
            Label clickedlabel = sender as Label;
            string name = clickedlabel.Name;
            
            //TableLayoutPanel main_panel = clickedlabel.GetContainerControl() as TableLayoutPanel;
            Control top = clickedlabel.TopLevelControl;
            int row = main_display.GetPositionFromControl(clickedlabel).Row;
            int col = main_display.GetPositionFromControl(clickedlabel).Column;
            int main_h = main_display.ColumnCount;
            int main_w = main_display.RowCount;
            debug.Text = mask.Count.ToString() + " " + mask[0].Count.ToString();// row.ToString() + ", " + col.ToString();

            this.cur_point.X = col;
            this.cur_point.Y = row;
            if (this.valid_point.Contains(this.cur_point))
            {
                draw_valid_alloc(this.cur_point, Color.White, true);
                this.score = this.score + this.cur_sco;
                debug.Text = "Score: " + this.score.ToString();
                check_row_col();
                set_random_mask();
                draw_tmp_display();
                this.valid_point = find_valid_position(this.mask);
                if (this.valid_point.Count == 0)
                {
                    debug.Text = debug.Text + " \n no valid allocation anymore!";
                    Console.WriteLine("no valid allocation anymore!");
                    return;
                }
                return;
            }
            else
            {
                return;
            }
        }

        private void check_row_col()
        {
            // check the occupency grid if there is any fully-occupied rows or columns
            int n_row = this.occupency.Count;
            int n_col = this.occupency[0].Count;

            List<int> full_row = new List<int>();
            List<int> full_col = new List<int>();

            // check through the row
            int sum;
            for (int i = 0; i < n_row; i++)
            {
                sum = 0;
                for (int j = 0; j < n_col; j++)
                {
                    sum = sum + this.occupency[i][j];
                }
                if (sum >= n_col)
                {
                    full_row.Add(i);
                }
            }

            // check through the columns
            for (int i = 0; i < n_col; i++)
            {
                sum = 0;
                for(int j = 0; j < n_row; j++)
                {
                    sum = sum + this.occupency[j][i];
                }
                if (sum >= n_row)
                {
                    full_col.Add(i);
                }
            }

            // remove the occupied columns and rows from the occupency grid and paint back
            if ((full_col.Count == 0) && (full_row.Count == 0))
            {
                return;
            }

            // remove fulled rows
            Label tmp_label;
            foreach (int r in full_row)
            {
                for (int i = 0; i < n_col; i++)
                {
                    // occupency
                    this.occupency[r][i] = 0;

                    // label
                    tmp_label = main_display.GetControlFromPosition(i, r) as Label;
                    tmp_label.BackColor = Color.CornflowerBlue;
                }
            }

            // remove fulled columns
            foreach (int c in full_col)
            {
                for (int i = 0; i < n_row; i++)
                {
                    this.occupency[i][c] = 0;
                    tmp_label = main_display.GetControlFromPosition(c, i) as Label;
                    tmp_label.BackColor = Color.CornflowerBlue;
                }
            }
        }

        private void draw_valid_alloc(Point p, Color color, bool write_occp)
        {
            int x = p.X;
            int y = p.Y;
            int x_occ;
            int y_occ;
            for (int i = this.s_row; i <= this.e_row; i++)
            {
                for (int j = this.s_col; j <= this.e_col; j++)
                {
                    x_occ = x - this.w_offs + j;
                    y_occ = y - this.h_offs + i;
                    if (this.mask[i][j] > 0)
                    {
                        Label tmp = main_display.GetControlFromPosition(x_occ, y_occ) as Label;
                        tmp.BackColor = color;
                        if (write_occp)
                        {
                            this.occupency[y_occ][x_occ] = 1;
                        }
                    }                   
                }
            }      

        }

        private List<Point> find_valid_position(List<List<int>> mask)
        {
            /* find valid position to allocate blocks and return as list */
            List<Point> valid_position = new List<Point>();

            // check the width and height of mask are both odd number
            int h_mask = mask.Count;
            int w_mask = mask[0].Count;
            if ((h_mask % 2 == 0) || (w_mask % 2 == 0))
            {
                Console.WriteLine("mask size error;");
                return null;
            }
            int h_offs = (h_mask - 1) / 2;
            int w_offs = (w_mask - 1) / 2;
            this.h_offs = h_offs;
            this.w_offs = w_offs;
            int h_occ = this.occupency.Count;
            int w_occ = this.occupency[0].Count;

            // find the minimum occupied rectangle in mask
            int sum = 0;
            // start of row
            int s_row = 0;
            for (int i = 0; i < mask[0].Count; i++)
            {
                sum = sum + mask[s_row][i];
            }
            while(sum == 0)
            {
                s_row = s_row + 1;
                for (int i = 0; i < mask[0].Count; i++)
                {
                    sum = sum + mask[s_row][i];
                }
            }

            // start of column
            sum = 0;
            int s_col = 0;
            for (int i = 0; i < mask.Count; i++)
            {
                sum = sum + mask[i][s_col];
            }
            while(sum == 0)
            {
                s_col = s_col + 1;
                for (int i = 0; i < mask.Count; i++)
                {
                    sum = sum + mask[i][s_col];
                }
            }

            // end of row
            sum = 0;
            int e_row = mask.Count - 1;
            for (int i = 0; i < mask[0].Count; i++)
            {
                sum = sum + mask[e_row][i];
            }
            while(sum == 0)
            {
                e_row = e_row - 1;
                for (int i = 1; i < mask[0].Count; i++)
                {
                    sum = sum + mask[e_row][i];
                }
            }

            // end of column
            sum = 0;
            int e_col = mask[0].Count - 1;
            for (int i = 0; i < mask.Count; i++)
            {
                sum = sum + mask[i][e_col];
            }
            while(sum == 0)
            {
                e_col = e_col - 1;
                for (int i = 0; i < mask.Count; i++)
                {
                    sum = sum + mask[i][e_col];
                }
            }

            // put in global variables
            this.s_col = s_col;
            this.s_row = s_row;
            this.e_col = e_col;
            this.e_row = e_row;

            // iterate throught each position and add valid position to list
            bool pass_flag = false;
            int x_occ;
            int y_occ;
            for (int m = 0; m < h_occ; m++)
            {
                for (int n = 0; n < w_occ; n++)
                {
                    pass_flag = false;
                    for (int i = s_row; i <= e_row; i++)
                    {
                        if (pass_flag)
                        {
                            break;
                        }
                        for (int j = s_col; j <= e_col; j++)
                        {
                            x_occ = n - w_offs + j;
                            y_occ = m - h_offs + i;

                            if ((x_occ < 0) || (y_occ < 0))
                            {
                                pass_flag = true;
                                break;
                            }

                            if ((x_occ > w_occ - 1) || (y_occ > h_occ - 1))
                            {
                                pass_flag = true;
                                break;
                            }

                            if ((this.occupency[y_occ][x_occ] > 0) && (mask[i][j] > 0))
                            {
                                pass_flag = true;
                                break;
                            }
                        }
                    }
                    if (!pass_flag)
                    {
                        Point tmp_p = new Point();
                        tmp_p.X = n;
                        tmp_p.Y = m;
                        valid_position.Add(tmp_p);
                    }
                }
            }
            
            return valid_position;
        }

        private void display_occupency()
        {
            for(int i = 0; i < this.occupency.Count; i++)
            {
                List<int> row = this.occupency[i];
                for (int j = 0; j < row.Count; j++)
                {
                    Console.Write(row[j].ToString());
                }
                Console.WriteLine("");
            }
        }

        private void draw_tmp_mask(object sender, EventArgs e)
        {
            mod_tmp_mask(sender, Color.Green);
            return;
        }

        private void rm_tmp_mask(object sender, EventArgs e)
        {
            mod_tmp_mask(sender, Color.CornflowerBlue);
            return;
        }

        public void mod_tmp_mask(object sender, Color color)
        {
            // find the current row and column

            Label clickedlabel = sender as Label;

            int row = main_display.GetPositionFromControl(clickedlabel).Row;
            int col = main_display.GetPositionFromControl(clickedlabel).Column;

            // check if current position is in the valid position

            Point tmp_p = new Point();
            tmp_p.Y = row;
            tmp_p.X = col;
            if (!this.valid_point.Contains(tmp_p))
            {
                return;
            }

            // draw a temporary mask

            draw_valid_alloc(tmp_p, color, false);
        }
    }
}
