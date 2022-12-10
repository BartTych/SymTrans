using System;
using System.Collections.Generic;

namespace Symulation
{
    public class repository_of_boundaries
    {
        //private RepositoryOfTraverses traverses_repository;
        //private Repository_of_windows windows;
        private Boundaries_of_city nodes_boundaries_data;


        public repository_of_boundaries(RepositoryOfTraverses repository_of_traverses ,RepositoryOfSections sections ,Repository_of_windows windows)
        {
            //this.traverses_repository = repository_of_traverses;
            //this.windows = windows;
            nodes_boundaries_data = new Boundaries_of_city(windows);

            instantiate_data_list_for_each_node_IO_in_all_sections(sections);
        }

        private void instantiate_data_list_for_each_node_IO_in_all_sections(RepositoryOfSections sections)
        {
            var list = sections.get_list_of_sections();
            List<int> list_of_I_nodes;
            List<int> list_of_O_nodes;
            
            for(int i = 0; i < sections.get_number_of_sections(); i++)
            {
                list_of_I_nodes = list[i].get_list_of_I_nodes();
                list_of_O_nodes = list[i].get_list_of_O_nodes();

                for(int j = 0; j < list_of_I_nodes.Count; j++)
                {
                    //check if there is a definition for that node allredy
                    if(!nodes_boundaries_data.is_data_entry_for_node_alredy_created(list_of_I_nodes[j]))
                    {
                        nodes_boundaries_data.instantiate_data_for_node(list_of_I_nodes[j]);
                    }
                    //if not add one
                }

                // tak naprawde to kazdy exit node jest tez entrance noudem , ale zostawie to tutaj
                // bo to moze nie byc prawda w przyszlosci jak cos zmienie 
                for (int j = 0; j < list_of_O_nodes.Count; j++)
                {
                    //check if there is a definition for that node allredy
                    if (!nodes_boundaries_data.is_data_entry_for_node_alredy_created(list_of_O_nodes[j]))
                    {
                        nodes_boundaries_data.instantiate_data_for_node(list_of_O_nodes[j]);
                    }
                    //if not add one                
                }

            }
        }

        public void calculate_interactions_between_traverses_for_node(int node_number, double calculation_end)
        {
            nodes_boundaries_data.calculate_interactions_for_node(node_number, calculation_end);
        }

        public void add_next_exit_config_for_node(int node, List<double[]> config, double base_time,Traverse traverse)
        {
            for (int i=0; i<config.Count; i++)
            {
                if(traverse.Permutation == config[i][0])
                {
                    nodes_boundaries_data.add_next_exit_config_for_node(node, config[i], base_time, traverse);
                    traverse.exit_start_time = config[i][2] + base_time;
                    traverse.exit_end_time = config[i][3] + base_time;
                }
            }

        }

        public void add_next_entrance_config_for_node(int node, List<double[]> config, double base_time, Traverse traverse)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if(traverse.Permutation == config[i][0]) 
                {
                    nodes_boundaries_data.add_next_entrance_config_for_node(node, config[i], base_time, traverse);
                    traverse.begining_start_time = config[i][2] + base_time;
                    traverse.begining_end_time = config[i][3] + base_time;
                }

            }

        }

        public void add_begining_start_time_to_traverse(List<double[]> config, Traverse traverse, double current_base_time)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if (traverse.Permutation == config[i][0])
                    traverse.begining_start_time = config[i][2] + current_base_time;
            }
        }


        public void add_begining_end_time_to_traverse(List<double[]> config, Traverse traverse, double current_base_time)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if (traverse.Permutation == config[i][0])
                    traverse.begining_end_time = config[i][3]+current_base_time;
            }
        }

        public void add_exit_start_time_to_traverse(List<double[]> config, Traverse traverse, double current_base_time)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if (traverse.Permutation == config[i][0])
                    traverse.exit_start_time = config[i][2] + current_base_time;
            }
        }

        public void add_exit_end_time_to_traverse(List<double[]> config, Traverse traverse, double current_base_time)
        {
            for (int i = 0; i < config.Count; i++)
            {
                if (traverse.Permutation == config[i][0])
                    traverse.exit_end_time = config[i][3] + current_base_time;
            }
        }

        public List<boundaries_at_node> get_boundaries_of_city()
        {
            return nodes_boundaries_data.get_city_list_of_boundaries();
        }

        public List<int> get_list_of_nodes_with_boundaries()
        {
            return nodes_boundaries_data.get_list_of_all_nodes_with_boundaries();
        }

        public void sort_boundaries_for_all_nodes()
        {
            nodes_boundaries_data.sort_all_boundaries();
        }

    }

    public class Boundaries_of_city
    {
        private List<boundaries_at_node> _list_of_boundaries;
        // tuatj powinienem zrobic liste traversow dla nodow startowych  
        // tak zeby moc szybciej wyszukiwac przejazdy

        private Repository_of_windows windows;

        public Boundaries_of_city(Repository_of_windows windows)
        {
            _list_of_boundaries = new List<boundaries_at_node>();
            this.windows = windows;
        }


        public void calculate_interactions_for_node(int node_number, double calculation_end_time)
        {
            // konwencja oznaczen
            // wspolzedne sa ustawione tak ze left side jedzie do nuda, prawa strona odjerzdza


            bool calculation_time_exceded=false;
            int index_of_left_side;
            int index_of_right_side;
            var boundary = return_boundaries_for_node(node_number);
            Traverse right_side_traverse;
            Traverse left_side_traverse;
            double right_side_start;
            double right_side_end;
            double left_side_start;
            double left_side_end;
            double representative_start;
            double representative_end;
            interaction_window window;

            index_of_left_side = boundary.exit_index;
            index_of_right_side = boundary.entrance_index;
            
            while (!calculation_time_exceded)
            {

                (right_side_start, right_side_end) = boundary.return_start_and_end_time_for_entrance_boundary_with_index(index_of_right_side);
                (left_side_start, left_side_end) = boundary.return_start_and_end_time_for_exit_boundary_with_index(index_of_left_side);

                right_side_traverse = boundary.get_traverse_for_entrance_boundary_with_index(index_of_right_side);
                left_side_traverse = boundary.get_traverse_for_exit_boundary_with_index(index_of_left_side);

                //musze peliej zrozumiec co to oznaczalo
                (representative_start, representative_end) = calculate_representative_times_for_overlap(right_side_start, right_side_end, left_side_start, left_side_end);


                if (is_there_overlap(representative_start, representative_end))
                {
                    
                    
                    window = create_new_window(boundary, index_of_right_side, index_of_left_side, representative_start, representative_end, windows, right_side_traverse, left_side_traverse);
                    windows.add_window(window);

                    add_window_to_right_and_left_side_traverse(boundary, window, index_of_left_side, index_of_right_side);


                }

                move_indexes_forward(ref index_of_right_side, ref index_of_left_side, right_side_end, left_side_end);

                if (is_end_time_exceeded(calculation_end_time, right_side_start, left_side_start))
                {
                    calculation_time_exceded = true;
                    boundary.exit_index = index_of_left_side;
                    boundary.entrance_index = index_of_right_side;
                }
            }
           


        }

        public void sort_all_boundaries()
        {
            for(int i = 0; i < _list_of_boundaries.Count; i++)
            {
                _list_of_boundaries[i].sort_all_boundaries();
            }
        }

        private void add_window_to_right_and_left_side_traverse( boundaries_at_node boundary,interaction_window window, int left_side_index, int right_side_index)
        {
            boundary.add_window_to_left_boudary_traverse_with_index(left_side_index, window);
            boundary.add_window_to_right_boundary_traverse_with_index(right_side_index, window);


        }
        private bool is_end_time_exceeded(double calculation_time, double entrance_start_time, double exit_start_time)
        {
            if (entrance_start_time > calculation_time && exit_start_time > calculation_time)
                return true;
            else
                return false;
        }
        private void move_indexes_forward(ref int entrance_index, ref int exit_index, double entrance_end,double exit_end)
        {

            var exit_has_to_be_changed = false;
            var entrance_has_to_be_changed = false;

            if (entrance_end <= exit_end)
                entrance_has_to_be_changed = true;
            else
                exit_has_to_be_changed = true;

            if (entrance_has_to_be_changed)
                entrance_index++;
            if (exit_has_to_be_changed)
                exit_index++;
        }
        private (double representative_start, double representative_end) calculate_representative_times_for_overlap(double entrance_start, double entrance_end, double exit_start, double exit_end)
        {
            double representative_start;
            double representative_end;

            if (entrance_start >= exit_start)
                representative_start = entrance_start;
            else
                representative_start = exit_start;

            if (entrance_end <= exit_end)
                representative_end = entrance_end;
            else
                representative_end = exit_end;

            return (representative_start, representative_end);
        }
        private bool is_there_overlap(double representative_start, double representative_end)
        {
            if (representative_start < representative_end)
                return true;
            else
                return false;
        }
        private interaction_window create_new_window(boundaries_at_node boundary, int entrance_index, int exit_index, double representative_start, double representative_end, Repository_of_windows windows, Traverse entrance_traverse, Traverse exit_traverse)
        {

            var window = new interaction_window();

            window.number = windows.get_number_of_next_free_window();
            window.permutation_which_exit_at_window = boundary.get_permutation_number_of_exit_boundary_with_index(exit_index);
            window.permutation_which_start_at_this_window = boundary.get_permutation_number_of_entrance_boundary_with_index(entrance_index);
            window.start_time = representative_start;
            window.end_time = representative_end;
            window.number_of_traverse_with_exit_at_this_window = boundary.get_traverse_number_of_exit_boundry_with_index(exit_index);
            window.traverse_with_exit_at_this_window = exit_traverse;
            window.number_of_traverse_with_entrance_at_this_window = boundary.get_traverse_number_of_entrance_with_index(entrance_index);
            window.traverse_with_entrance_at_this_window  = entrance_traverse;

            return window;
        }
        public bool is_data_entry_for_node_alredy_created(int node_number)
        {
            for (int i = 0; i < _list_of_boundaries.Count; i++)
            {
                if (_list_of_boundaries[i].node_number == node_number)
                {
                    return true;
                }
            }
            return false;
        }
        public void instantiate_data_for_node(int node_number)
        {
            _list_of_boundaries.Add(new boundaries_at_node(node_number));
        }
        public void add_next_entrance_config_for_node(int node, double[] config, double base_time, Traverse traverse)
        {
            var boundary = return_boundaries_for_node(node);
            boundary.add_entrance_boundary(config,base_time, traverse);
        }
        public void add_next_exit_config_for_node(int node, double[] config,double base_time, Traverse traverse)
        {
            var boundary = return_boundaries_for_node(node);
            boundary.add_exit_boundary(config,base_time,traverse);
        }
        public List<int> get_list_of_all_nodes_with_boundaries()
        {
            var list = new List<int>();
            for (int i = 0; i < _list_of_boundaries.Count; i++)
            {
                list.Add(_list_of_boundaries[i].node_number);
            }

            return list;

        }
        private boundaries_at_node return_boundaries_for_node(int node_number)
        {
            for (int i = 0; i < _list_of_boundaries.Count; i++)
            {
                if(_list_of_boundaries[i].node_number==node_number)
                    return _list_of_boundaries[i];
            }
            return null;
        }
        public List<boundaries_at_node> get_city_list_of_boundaries()
        {
            return _list_of_boundaries;
        }

    }
    public class boundaries_at_node
    {
        public int node_number { get; set; }
        public int entrance_index { get; set; }
        public int exit_index { get; set; }

        private List<boundary_of_traverse> node_left_boundaries;
        private List<boundary_of_traverse> node_right_boundaries;



        public boundaries_at_node(int node_number)
        {
            this.node_number = node_number;
            node_left_boundaries = new List<boundary_of_traverse>();
            node_right_boundaries = new List<boundary_of_traverse>();
            entrance_index = 0;
            exit_index = 0;
        }


        public (double start, double end) return_start_and_end_time_for_exit_boundary_with_index(int index)
        {
            return (node_left_boundaries[index].start_time, node_left_boundaries[index].end_time);
        }
        public (double start, double end) return_start_and_end_time_for_entrance_boundary_with_index(int index)
        {
            return (node_right_boundaries[index].start_time, node_right_boundaries[index].end_time);
        }

        public int get_permutation_number_of_exit_boundary_with_index(int index)
        {
            return node_left_boundaries[index].permutation_number;
        }
        public int get_permutation_number_of_entrance_boundary_with_index(int index)
        {
            return node_right_boundaries[index].permutation_number;
        }

        public int get_traverse_number_of_exit_boundry_with_index(int index)
        {
            return node_left_boundaries[index].traverse_number;
        }

        public Traverse get_traverse_for_exit_boundary_with_index(int index)
        {
            return node_left_boundaries[index].traverse;
        }
        public int get_traverse_number_of_entrance_with_index(int index)
        {
            return node_right_boundaries[index].traverse_number;
        }
        public Traverse get_traverse_for_entrance_boundary_with_index(int index)
        {
            return node_right_boundaries[index].traverse;
        }
        public void add_window_to_left_boudary_traverse_with_index(int index, interaction_window window)
        {
            node_left_boundaries[index].traverse.add_exit_window(window);
        }
        public void add_window_to_right_boundary_traverse_with_index(int index, interaction_window window)
        {
            node_right_boundaries[index].traverse.add_entrance_window(window);
        }

        public void add_entrance_boundary(double[] config, double base_time, Traverse traverse)
        {
            var boundary = new boundary_of_traverse();
            boundary.nude_number = node_number;
            boundary.permutation_number = (int)config[0];
            boundary.traverse_number = traverse.Number;
            boundary.traverse = traverse;
            boundary.length = config[1];
            boundary.start_time = base_time + config[2];
            boundary.end_time = base_time + config[3];
            //boundary.traverse_number
            //jeszcze brakuje mi nadania numeru trawersu;
            node_right_boundaries.Add(boundary);
        }
        public void add_exit_boundary(double[] config, double base_time, Traverse traverse)
        {
            var boundary = new boundary_of_traverse();

            boundary.nude_number = node_number;
            boundary.permutation_number = (int)config[0];
            boundary.traverse_number = traverse.Number;
            boundary.traverse = traverse;
            boundary.length = config[1];
            boundary.start_time = base_time + config[2];
            boundary.end_time = base_time + config[3];
            //boundary.traverse_number
            //jeszcze brakuje mi nadania numeru trawersu;
            node_left_boundaries.Add(boundary);
        }

        public void sort_all_boundaries()
        {
            node_left_boundaries.Sort((x, y) => x.start_time.CompareTo(y.start_time));
            node_right_boundaries.Sort((x,y) => x.start_time.CompareTo(y.start_time));
        }  
    }
    public struct boundary_of_traverse
    {
        public int nude_number { get; set; }
        public int permutation_number { get; set; }
        public int traverse_number { get; set; }
        public Traverse traverse { get; set; }
        public double length { get; set; }
        public double start_time { get; set; }
        public double end_time { get; set; }

    }
    

}
