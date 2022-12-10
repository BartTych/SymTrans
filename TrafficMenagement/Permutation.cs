using System.Collections.Generic;

namespace Symulation
{
    public class Permutation
    {
        private readonly int number_of_permutation;
        private readonly int start_Node;
        private readonly int end_Node;
        private readonly int section_Number;

        private double max_length_of_traverse;
        private double current_length_of_traverse;
        private double min_speed_along_permutation;   // [m/s]
        private double exit_speed_of_permutation;
        private double entrance_speed_of_permutation;
        private double time_of_transfer_thru_exit;
        private double length_of_permutation;
        private List<Traverse> _list_of_traverses;
        public int error_limiting_traverse_length;

        //private readonly CityDataStorage city;

        private List<SchematicComponent> list_of_schematic_components_along_permutation;
        private int[][] _route;

        public Permutation(int number, int start_node, int end_node, int section_number)
        {
            number_of_permutation = number;
            start_Node = start_node;
            end_Node = end_node;
            section_Number = section_number;
            //city = city;
            list_of_schematic_components_along_permutation = new List<SchematicComponent>();
            _list_of_traverses = new List<Traverse>();
        }
        public Permutation get_deep_copy_without_schematic_components()
        {
            var permutation = new Permutation(number_of_permutation, start_Node, end_Node, section_Number);
            permutation.max_length_of_traverse = max_length_of_traverse;
            permutation.current_length_of_traverse = current_length_of_traverse;
            permutation.min_speed_along_permutation = min_speed_along_permutation;
            permutation.exit_speed_of_permutation = exit_speed_of_permutation;
            permutation.time_of_transfer_thru_exit = time_of_transfer_thru_exit;
            permutation.error_limiting_traverse_length = error_limiting_traverse_length;
            permutation._route = Get_deep_copy_of_route();
            return permutation;
        }
        public List<Traverse> get_list_of_traverses()
        {
            return _list_of_traverses;
        }

        public void add_traverse_to_list(Traverse traverse)
        {
            _list_of_traverses.Add(traverse);
        }
        public void set_exit_speed(double speed)
        {
            exit_speed_of_permutation = speed;
        }
        public double get_exit_speed()
        {
            return exit_speed_of_permutation;
        }
        public void set_entrance_speed(double speed)
        {
            entrance_speed_of_permutation = speed;
        }
        public double get_entrance_speed()
        {
            return entrance_speed_of_permutation;
        }
        public void calculate_time_of_travel_thru_exit()
        {
            time_of_transfer_thru_exit = current_length_of_traverse / exit_speed_of_permutation;
        }
        public double get_time_of_travel_thru_exit()
        {
            return time_of_transfer_thru_exit;
        }
        public void Set_route(int [][] route)
        {
            _route = route;
        }

        public void set_permutation_length(double length)
        {
            length_of_permutation = length;
        }

        public double get_permutation_length()
        {
            return length_of_permutation;
        }

        

        public bool is_node_a_start_node(int node)
        {
            if (start_Node == node)
                return true;
            else
                return false;
        }
        public bool is_node_an_end_node(int node)
        {
            if(end_Node == node)
                return true ;
            else
                return false;
        }


        public bool is_node_part_of_this_permutation(int node)
        {
            bool result = false;

            for(int i = 0; i < _route.GetLength(0); i++)
            {
                if(_route[i][0]==node || _route[i][1] == node)
                {
                    result = true;
                    break;
                }
            }
            return result;


        }

        public int get_start_node()
        {
            return start_Node;
        }

        public int get_end_node()
        {
            return end_Node;
        }

        public bool is_track_present_in_permutation(int track)
        {
            for(int i=0; i < _route.GetLength(0); i++)
            {
                if (_route[i][2] == track)
                    return true;
            }

            return false;
        }

        public int[][] Get_route()
        {
            return _route;
        }
        public int [][] Get_deep_copy_of_route()
        {
            var new_route = new int[_route.GetLength(0)][];
            for (int i = 0; i < _route.GetLength(0); i++)
            {
                new_route[i] = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    new_route[i][j] = _route[i][j];
                }
            }

            return new_route;
        }
        public double get_min_speed_along_permutation()
        {
            return min_speed_along_permutation;
        }
        
        public void set_min_speed_along_permutation(double min_speed)
        {
            min_speed_along_permutation = min_speed;
        }
        public int get_number_of_permutation()
        {
            return number_of_permutation;
        }

        public int get_section_number()
        {
            return section_Number;
        }
        public void set_max_length_of_traverse(double length)
        {
            max_length_of_traverse = length;
        }
        public void set_current_length_of_traverse(double length)
        {
            current_length_of_traverse = length;
        }
        public double get_max_length_of_traverse()
        {
            return max_length_of_traverse;
        }
        public double get_current_length_of_traverse()
        {
            return current_length_of_traverse;
        }
        public (int,int) Get_start_and_end_nodes()
        {
            return (start_Node, end_Node);
        }
        public int Get_start_noude()
        {
            return start_Node;
        }
        public int Get_exit_noude()
        {
            return end_Node;
        }
        public int get_error_limiting_traverse_length()
        {
            return error_limiting_traverse_length;
        }
        public void set_error_limiting_traverse_lenght(int error)
        {
            error_limiting_traverse_length=error;
        }
        public void add_component_to_list(SchematicComponent schematicComponent)
        {
            list_of_schematic_components_along_permutation.Add(schematicComponent);
        }
        public List<SchematicComponent> get_list_of_schematic_components()
        {
            return list_of_schematic_components_along_permutation;
        }
        public SchematicComponent get_last_component_on_list()
        {
            return list_of_schematic_components_along_permutation[list_of_schematic_components_along_permutation.Count - 1];
        }
        public SchematicComponent get_first_component_on_list()
        {
            return list_of_schematic_components_along_permutation[0];
        }
    }

}
