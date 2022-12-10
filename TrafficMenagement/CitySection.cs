using System;
using System.Collections.Generic;

namespace Symulation
{
    public abstract class CitySection //to bedzie abstrakcyjna klasa 
    {

        public abstract Type what_is_the_type_of_this_section();
        public abstract List<int> Get_list_of_out_nodes();
        public abstract List<int> Get_list_of_in_nodes();
        public abstract List<Permutation> Get_permutations();
        public abstract int Get_number_of_section();

        public abstract List<int> get_list_of_I_nodes();
        public abstract List<int> get_list_of_O_nodes();
        public abstract List<double[]> get_config_for_exit(int node_number);
        public abstract List<double[]> get_config_for_entrance(int node_number);
        public abstract double get_period_of_section();
        public abstract List<int> get_list_of_tracks_in_secton();
        public abstract void calculate_max_size_of_traverse_for_each_permutation_of_section();
        public abstract void print__traverse_length_of_all_permutations_of_section();
        public abstract bool is_synchronization_def_for_section();
        public abstract void define_synch_for_secton(double synch_time);
        public abstract double get_synchro_time_for_section();
        public abstract int get_max_number_of_active_exit_windows();
    }

}
