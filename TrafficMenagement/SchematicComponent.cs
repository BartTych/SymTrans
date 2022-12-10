using System.Collections.Generic;
namespace Symulation
{
    public abstract class SchematicComponent
    {
        public abstract void add_permutation_to_list(Permutation permutation);
        public abstract List<Permutation> get_list_of_permutations();
        public abstract void clear_list_of_permutations();
        public abstract int get_number_of_attached_noude();
        public abstract List<double[]> get_config();
        public abstract void add_config_section(double[] section_of_config);
        public abstract void sort_config_if_more_than_one_section();
        public abstract void shift_config_time(double shift_time);
        public abstract bool is_permutation_on_list(Permutation permutation);
    }
    
}
