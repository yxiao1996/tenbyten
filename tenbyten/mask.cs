using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tenbyten
{
    class Mask
    {
        public List<List<int>> mask_base = new List<List<int>>()
        {
            new List<int>() { 0, 0, 0, 0, 0 },
            new List<int>() { 0, 0, 0, 0, 0 },
            new List<int>() { 0, 0, 1, 0, 0 },
            new List<int>() { 0, 0, 0, 0, 0 },
            new List<int>() { 0, 0, 0, 0, 0 }
        };

        public Mask(){}

        class small_i_v : Mask
        {
            public List<List<int>> mask_value;
            public int score = 2;
            
            public small_i_v()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[1][2] = 1;
            }
        }

        class small_i_h : Mask
        {
            public List<List<int>> mask_value;
            public int score = 2;

            public small_i_h()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[2][1] = 1;
            }
        }

        class mid_i_h : Mask
        {
            public List<List<int>> mask_value;
            public int score = 3;

            public mid_i_h()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[2][1] = 1;
                mask_value[2][3] = 1;
            }
        }

        class mid_i_v : Mask
        {
            public List<List<int>> mask_value;
            public int score = 3;

            public mid_i_v()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[1][2] = 1;
                mask_value[3][2] = 1;
            }
        }

        class small_L_lt : Mask
        {
            public List<List<int>> mask_value;
            public int score = 3;

            public small_L_lt()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[1][2] = 1;
                mask_value[2][1] = 1;
            }
        }

        public class small_L_rt : Mask
        {
            public List<List<int>> mask_value;
            public int score = 3;

            public small_L_rt()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[1][2] = 1;
                mask_value[2][3] = 1;
            }
        }

        public class small_L_lb : Mask
        {
            public List<List<int>> mask_value;
            public int score = 3;

            public small_L_lb()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[3][2] = 1;
                mask_value[2][1] = 1;
            }
        }

        public class small_L_rb : Mask
        {
            public List<List<int>> mask_value;
            public int score = 3;

            public small_L_rb()
            {
                Mask m = new Mask();
                mask_value = m.mask_base;
                mask_value[3][2] = 1;
                mask_value[2][3] = 1;
            }
        }

        public class MaskSet
        {
            public List<List<int>> mask_small_i_v;
            public List<List<int>> mask_small_i_h;
            public List<List<int>> mask_mid_i_v;
            public List<List<int>> mask_mid_i_h;
            public List<List<int>> mask_small_L_lt;
            public List<List<int>> mask_small_L_rt;
            public List<List<int>> mask_small_L_lb;
            public List<List<int>> mask_small_L_rb;

            // a list to store all the masks
            public List<List<List<int>>> mask_list = new List<List<List<int>>>();
            public List<int> score_list = new List<int>();

            public MaskSet()
            {
                // small I vertical
                Mask.small_i_v i_small_i_v = new small_i_v();
                mask_small_i_v = i_small_i_v.mask_value;
                mask_list.Add(mask_small_i_v);
                score_list.Add(i_small_i_v.score);

                // small I horizontal
                Mask.small_i_h i_small_i_h = new small_i_h();
                mask_small_i_h = i_small_i_h.mask_value;
                mask_list.Add(mask_small_i_h);
                score_list.Add(i_small_i_h.score);

                // mid I vertical
                Mask.mid_i_v i_mid_i_v = new mid_i_v();
                mask_mid_i_v = i_mid_i_v.mask_value;
                mask_list.Add(mask_mid_i_v);
                score_list.Add(i_mid_i_v.score);

                // mid I horizontal
                Mask.mid_i_h i_mid_i_h = new mid_i_h();
                mask_mid_i_h = i_mid_i_h.mask_value;
                mask_list.Add(mask_mid_i_h);
                score_list.Add(i_mid_i_h.score);

                // small L left top
                Mask.small_L_lt i_small_i_lt = new small_L_lt();
                mask_small_L_lt = i_small_i_lt.mask_value;
                mask_list.Add(mask_small_L_lt);
                score_list.Add(i_small_i_lt.score);

                // small L right top
                Mask.small_L_rt i_small_L_rt = new small_L_rt();
                mask_small_L_rt = i_small_L_rt.mask_value;
                mask_list.Add(mask_small_L_rt);
                score_list.Add(i_small_L_rt.score);

                // small L left bottom
                Mask.small_L_lb i_small_i_lb = new small_L_lb();
                mask_small_L_lb = i_small_i_lb.mask_value;
                mask_list.Add(mask_small_L_lb);
                score_list.Add(i_small_i_lb.score);

                // small L right bottom
                Mask.small_L_rb i_small_L_rb = new small_L_rb();
                mask_small_L_rb = i_small_L_rb.mask_value;
                mask_list.Add(mask_small_L_rb);
                score_list.Add(i_small_L_rb.score);
            }
        }
    }

    
}
