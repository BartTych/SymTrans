using System.Collections.Generic;
namespace Symulation
{
    public class Intersection:SchematicComponent
    {
        private List<double[]> config;
        private readonly int attached_Node;
        private readonly int type;  //1 convergence, 2 divergence

        private List<Permutation> list_of_permutations;

        public Intersection(int attached_node, int type)
        {
            attached_Node = attached_node;
            this.type = type;
            list_of_permutations = new List<Permutation>();
            config = new List<double[]>();
        }

        public override void add_config_section(double[] section_of_config)
        {
            config.Add(section_of_config);
        }
        public override List<double[]> get_config()
        {
            return config;
        }
        public override void sort_config_if_more_than_one_section()
        {
            {
                if (config.Count > 1)
                {
                    config.Sort((x, y) => x[2].CompareTo(y[2]));
                }
            }
        }
        public override void add_permutation_to_list(Permutation permutation)
        {
            list_of_permutations.Add(permutation);
        }
        public override void clear_list_of_permutations()
        {
            list_of_permutations.Clear();
        }
        public override List<Permutation> get_list_of_permutations()
        {
            return list_of_permutations;
        }
        public override int get_number_of_attached_noude()
        {
            return attached_Node;
        }
        public override bool is_permutation_on_list(Permutation permutation)
        {
            var is_permutation_on_list = false;
            for (int i = 0; i < list_of_permutations.Count; i++)
            {
                if (list_of_permutations[i] == permutation)
                {
                    is_permutation_on_list = true;
                }
            }
            return is_permutation_on_list;
        }
        public override void shift_config_time(double shift_time)
        {
            for (int i = 0; i < config.Count; i++)
            {
                config[i][2] += shift_time;
                config[i][3] += shift_time;
            }
        }
        public int get_type()
        {
            return type;
        }
    }
    
}
