using System.Collections.Generic;
using System;
namespace Symulation
{
    public class Exit:SchematicComponent
    {

        private readonly int number_of_attached_node;
        private List<Permutation> list_of_permutations;
        private List<pair_of_permutations> list_of_primary_pairs;
        private List<pair_of_permutations> list_of_secondary_pairs;

        private List<double[]> config;


        public Exit(int number)
        {
            this.number_of_attached_node = number;
            list_of_permutations = new List<Permutation>();
            config = new List<double[]>();
        }

        public override void add_config_section(double[] config_section)
        {
            config.Add(config_section);
        }

        public double[] get_config_for_permutation(int permutation)
        {
            for(int i = 0; i < config.Count; i++)
            {
                if (config[i][0] == permutation)
                    return config[i];
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
        public double[] get_deep_copy_of_config_for_permutation(int permutation)
        {
            double[] copy = new double[4]; 
            for (int i = 0; i < config.Count; i++)
            {
                if (config[i][0] == permutation)
                {
                    for(int j = 0; j < 4; j++)
                        copy[j] = config[i][j];
                }
            }
            return copy;
        }
        public void move_config_to_have_end_time_of_permutation_as_given(double time, int permutation)
        {
            double required_time_shift;
            double current_end_of_permutation = get_end_time_from_permutation(permutation);
            required_time_shift =  time - current_end_of_permutation;
            shift_config_time(required_time_shift);
        }
        public override void shift_config_time(double shift_time)
        {
            for(int i = 0; i < config.Count; i++)
            {
                config[i][2] += shift_time;
                config[i][3] += shift_time;
            }
        }
        public double get_start_time_from_permutation(int per_number)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if(config[i][0] == per_number)
                {
                    return config[i][2];
                }
            }
            return -1000;
        }
        public double get_end_time_from_permutation(int per_number)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if (config[i][0] == per_number)
                {
                    return config[i][3];
                }
            }
            return -1000;
        }
        public void add_list_of_primary_pairs(List<pair_of_permutations> list)
        {
            list_of_primary_pairs = list;
        }
        public void add_list_of_secondary_pairs(List<pair_of_permutations> list)
        {
            list_of_secondary_pairs = list;
        }
        public List<pair_of_permutations> get_list_of_primary_pairs()
        {
            return list_of_primary_pairs;
        }
        public void add_new_config_line(double[] new_line)
        {
            config.Add(new_line);
        }
        public override List<double[]> get_config()
        {
            return config;
        }

        public override void clear_list_of_permutations()
        {
            list_of_permutations.Clear();
        }
        public override bool is_permutation_on_list(Permutation permutation)
        {
            var is_permutation_on_list = false;
            for(int i = 0; i < list_of_permutations.Count; i++)
            {
                if (list_of_permutations[i] == permutation)
                {
                    is_permutation_on_list = true;
                }
            }
            return is_permutation_on_list;
        }

        public override void add_permutation_to_list(Permutation permutation)
        {
            list_of_permutations.Add(permutation);
        }

        public override List<Permutation> get_list_of_permutations()
        {
            return list_of_permutations;
        }

        public override int get_number_of_attached_noude()
        {
            return number_of_attached_node;
        }


        public int get_attached_node_number()
        {
            return number_of_attached_node;
        }

        public void sort_permutations_by_current_length_of_traverse()
        {
            list_of_permutations.Sort((x, y) => x.get_current_length_of_traverse().CompareTo(y.get_current_length_of_traverse()));
            //list_of_permutations.Reverse();
        }
        public void sort_permutations_by_number()
        {
            if (list_of_permutations.Count > 1)
            {
                list_of_permutations.Sort((x, y) => x.get_number_of_permutation().CompareTo(y.get_number_of_permutation()));
                //list_of_permutations.Reverse();
            }
        }

        public void print_permutations_and_lengths()
        {
            
            foreach(Permutation n in list_of_permutations)
            {
                Console.WriteLine("Permutation number :{0}", n.get_number_of_permutation());
                Console.WriteLine("length of travers:{0}", n.get_current_length_of_traverse());
            }
        }



    }
    
}
