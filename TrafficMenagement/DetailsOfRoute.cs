using System.Collections.Generic;

namespace Symulation
{
    public class DetailsOfRoute
    {
        private CityDataStorage city;
        private RepositoryOfSections sections;

        public DetailsOfRoute(CityDataStorage city, RepositoryOfSections sections)
        {
            this.city = city;
            this.sections = sections;
        }

        public (List<CitySection> list_of_sections, List<Permutation> list_of_permutations) prepare_description_of_route (int[][] route)
        {
            List<int> list_of_section_numbers = new List<int> ();
            List<Permutation> list_of_permutations = new List<Permutation> ();
            List<Permutation> temp_list_of_permutations;
            List<CitySection> list_of_sections = new List<CitySection>();

            for (int i = 0; i < route.GetLength(0); i++)
            {
                if (!list_of_section_numbers.Contains(city.get_section_number_of_track(route[i][2])))
                    list_of_section_numbers.Add(city.get_section_number_of_track(route[i][2]));
            }

            for (int i = 0; i < list_of_section_numbers.Count; i++)
            {
                list_of_sections.Add(sections.get_section_with_number(list_of_section_numbers[i]));
                temp_list_of_permutations = sections.get_list_of_permutations_for_section(list_of_section_numbers[i]);
                for(int j = 0; j < temp_list_of_permutations.Count; j++)
                {
                    if (is_permutation_part_of_route(temp_list_of_permutations[j], route))
                        list_of_permutations.Add(temp_list_of_permutations[j]);
                }
            }


            return (list_of_sections, list_of_permutations);

            bool is_permutation_part_of_route(Permutation permutation, int [][] _route)
            {
                int per_start_node = permutation.Get_start_noude();
                int per_end_node = permutation.Get_exit_noude();
                List<int> list_of_route_noudes = new List<int>();
                
                for(int i = 0; i < _route.GetLength(0) ; i++)
                    list_of_route_noudes.Add(_route[i][0]);
                list_of_route_noudes.Add(_route[_route.GetLength(0)-1][1]);


                if (list_of_route_noudes.Contains(per_start_node) && list_of_route_noudes.Contains(per_end_node))
                    return true;
                else
                    return false;


            }

        }




    }

}
