using System.Collections.Generic;
using System.Linq;


namespace Symulation
{
    public class RepositoryOfPermutations
    {
        private List<Permutation> _list_of_permutations;
        private readonly CityDataStorage city;

        public RepositoryOfPermutations(CityDataStorage city )
        {
            _list_of_permutations = new List<Permutation>();
            this.city = city;
        }

        public void add_permutation_to_list_of_permutations(Permutation perm)
        {
            _list_of_permutations.Add(perm);

        }
        
        public int return_number_of_next_free_perm_number()
        {
            if (_list_of_permutations.Count == 0)
                return 0;
            else
            {
                var last_permutation_in_list = _list_of_permutations.Last();
                return last_permutation_in_list.get_number_of_permutation() + 1; //add 1 to last number in list, so first free on  

            }
        }
        
        public List<Permutation> get_list_of_permutation()
        {
            return _list_of_permutations;
        }


        public Permutation Get_permutation(int number)
        {
            return _list_of_permutations[number];
        }

        public int return_number_of_permutations()
        {
            return _list_of_permutations.Count;
        }


        public List<Permutation> return_non_end_start_permutations_going_thru_node(int node)
        {
            List<Permutation> list_of_perm = new List<Permutation>();

            for(int i = 0; i < _list_of_permutations.Count; i++)
            {
                if( _list_of_permutations[i].is_node_part_of_this_permutation(node))
                    list_of_perm.Add(_list_of_permutations[i]);
            }

            list_of_perm = remove_start_end_permutations_from_list(list_of_perm);

            return list_of_perm;
        }

        public List<Permutation> remove_start_end_permutations_from_list(List<Permutation> permutations)
        {
            for (int i = permutations.Count - 1; i >= 0; i--)
                if (is_permutation_start_or_end_permutation(permutations[i]))
                    permutations.RemoveAt(i);
            return permutations;
        }

        public List<Permutation> return_non_end_start_permutations_connected_to_permutation_along_the_way(Permutation permutation)
        {
            int end_node = permutation.get_end_node();
            
            return return_non_start_end_permutations_connected_to_node_along_the_way(end_node);
        }


        public List<Permutation> return_non_start_end_permutations_connected_to_node_along_the_way(int node)
        {
            List<Permutation> permutations = new List<Permutation>();

            for(int i = 0; i < _list_of_permutations.Count; i++)
            {
                if (_list_of_permutations[i].get_start_node() == node)
                    permutations.Add(_list_of_permutations[i]);
            }

            permutations = remove_start_end_permutations_from_list(permutations);

            return permutations;
        }

        public List<Permutation> return_non_start_end_permutations_connected_to_permutation_the_wrong_way(Permutation permutation)
        {
            int start_node = permutation.get_start_node();

            return return_non_start_end_permutations_connected_to_node_the_wrong_way(start_node);
        }

        public List<Permutation> return_non_start_end_permutations_connected_to_node_the_wrong_way(int node)
        {
            List<Permutation> permutations = new List<Permutation>();

            for (int i = 0; i < _list_of_permutations.Count; i++)
            {
                if (_list_of_permutations[i].get_end_node() == node)
                    permutations.Add(_list_of_permutations[i]);
            }

            permutations = remove_start_end_permutations_from_list(permutations);

            return permutations;
        }

        public bool is_permutation_start_or_end_permutation(Permutation permutation)
        {
            // sprawdzenie czy start lub end node jest noudem ktoregos przystanku
            // wymaga odwolania do miasta
            int start_node = permutation.get_start_node();
            int end_node = permutation.get_end_node();

            if (city.is_node_a_station_node(start_node) || city.is_node_a_station_node(end_node))
            {
                return true;
            }
            else
                return false;
        }
    }

    public class PerpositoryOfStationDep
    {

    }

    public class StationDep
    {
        public int station_number { get; set; }
        public int node_of_station { get; set; }
        public Station station_in_city_def { get; set; }


        public bool is_there_pod_available(double time)
        {
            return true;
        }



        // list of pods on station
            // 2
            // avilable
            // taken
            // or one list  
        
        // list of pods en route to station
            //free, generated by prediction
            //rides, people get of at the end
        // two types of rides
            //pasanger 
            //transfer ?



    }

}
