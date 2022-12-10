using System.Collections.Generic;

namespace Symulation
{
    public class RepositoryOfSections
    {
        private List<CitySection> list_of_sections;

        public RepositoryOfSections()
        {
            list_of_sections = new List<CitySection>();
        }

        public void add_new_section(CitySection city_section)
        {
            list_of_sections.Add(city_section);
        }

        public List<CitySection> get_list_of_sections()
        {
            return list_of_sections;
        }

        public int get_next_free_section_number()
        {
            return list_of_sections.Count;
        }

        public int get_number_of_sections()
        {
            return list_of_sections.Count;
        }

        public List<Permutation> get_list_of_permutations_for_section(int section_number)
        {
            return list_of_sections[section_number].Get_permutations(); 
        }

        public CitySection get_section_with_number(int number)
        {
            return list_of_sections[number];
        }

        public double get_synch_time_for_section_with_index(int index)
        {
            return list_of_sections[index].get_synchro_time_for_section();
        }


    }



}
