using System.Collections.Generic;
namespace Symulation
{
    public class Entrance:SchematicComponent
    {
        List<double[]> config;
        int attached_to_city_node;

        private List<Permutation> list_of_permutations;


        public Entrance(int city_noude)
        {
            attached_to_city_node = city_noude;
            config = new List<double[]>();
            list_of_permutations=new List<Permutation>();
        }

        public override List<double[]> get_config()
        {
            return config;
        }
        public override void shift_config_time(double shift_time)
        {
            for (int i = 0; i < config.Count; i++)
            {
                config[i][2] += shift_time;
                config[i][3] += shift_time;
            }
        }
        public double[] get_config_for_permutation(int permutation)
        {
            for(int i = 0; i < list_of_permutations.Count; i++)
            {
                if (config[i][0] == permutation)
                {
                    return config[i];
                }
            }
            return null;
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
        public void if_two_permutations_sort_config()
        {
            if (config.Count == 2)
            {
                config.Sort((x, y) => x[2].CompareTo(y[2]));
            }
        }
        public override void add_config_section(double[] config_section)
        {
            config.Add(config_section);
        }
        public override void add_permutation_to_list(Permutation permutation)
        {
            list_of_permutations.Add(permutation);
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
            return attached_to_city_node;
        }
        public int get_attached_node()
        {
            return attached_to_city_node;
        }
        public void sort_permutations_by_number()
        {
            if (list_of_permutations.Count > 1)
            {
                list_of_permutations.Sort((x, y) => x.get_number_of_permutation().CompareTo(y.get_number_of_permutation()));
                //list_of_permutations.Reverse();
            }
        }
    }
    
}
