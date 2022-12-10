using System;
using System.Collections.Generic;

namespace Symulation
{
    public class managment_of_traverse_interactions
    {
        private CityDataStorage city;
        private RepositoryOfTraverses traverse_repository;
        private Repository_of_windows windows_repository;
        private RepositoryOfSections repository_of_sections;
        private RepositoryOfPermutations permutations;
        private repository_of_boundaries boundaries_repository;
        private CalculationData calculation_data;

        public managment_of_traverse_interactions(RepositoryOfSections repository_of_sections, RepositoryOfPermutations permutations, CityDataStorage city)
        {
            this.city = city;
            //create traverse_repository
            traverse_repository = new RepositoryOfTraverses();
            windows_repository = new Repository_of_windows();
            //created sections interaction repository(traverse_repository)
            this.repository_of_sections = repository_of_sections;
            this.permutations = permutations;
            boundaries_repository = new repository_of_boundaries(traverse_repository, repository_of_sections, windows_repository);

            calculation_data = new CalculationData(repository_of_sections.get_number_of_sections(), permutations.return_number_of_permutations(), repository_of_sections);

        }

        /// <summary>
        /// Calculate traverses and boundaries for those traverses, start where last calculation ended and goes forward for param "time_forward"
        /// It is first part calculations needed for traffic planing.
        /// </summary>
        /// <param name="time_forward"></param>
        public void create_traverses_and_calculate_boundaries_of_traverses(double time_forward)
        {
            double period;
            int repetitions;
            double current_config_calculation_base;
            Traverse traverse;
            int traverse_start_node;
            int traverse_end_node;
            var list_of_sections = repository_of_sections.get_list_of_sections();
            List<Permutation> list_of_section_permutations;
            List<double[]> config;

            for (int i = 0; i < list_of_sections.Count; i++)
            {
                period = list_of_sections[i].get_period_of_section();
                repetitions = calculate_number_of_config_repetitions(time_forward, period);
                list_of_section_permutations = list_of_sections[i].Get_permutations();
                list_of_section_permutations.Reverse();
                while (repetitions > 0)
                {
                    //ta baza jest wspolna dla wszystkich noudow sekcji w danej repetycji
                    current_config_calculation_base = calculation_data.get_last_calculation_time_for_section(i);

                    //dodaje w petli nowe traversy dla kazdej permutacji sekcji
                    //for each permutation

                    for (int j = 0; j < list_of_section_permutations.Count; j++)
                    {
                        //create new traverse
                        if (list_of_sections[i].what_is_the_type_of_this_section() == typeof(DC_section))
                            traverse = new DC_Traverse();
                        else
                            traverse = new Sort_Traverse();

                        traverse.Number = traverse_repository.get_next_free_number_of_travers();
                        traverse.Permutation = list_of_section_permutations[j].get_number_of_permutation();
                        (traverse.entrance_node, traverse.exit_node) = list_of_section_permutations[j].Get_start_and_end_nodes();

                        //tu jest problem dla traversu 
                        //bo dlugosc dla jednej z permutacji jest wydluzana przy liczeniu config , i to niejest zmienione 
                        //w miejsc gdzie sa przechowywane dane o permutacjach
                        //zmiany wymaga repository of sections gdzie dla kazdej sekcji jest lista permutacji z dlugoscia traversu
                        traverse.Size = list_of_section_permutations[j].get_current_length_of_traverse();

                        list_of_section_permutations[j].add_traverse_to_list(traverse);

                        traverse_start_node = list_of_section_permutations[j].Get_start_noude();
                        config = list_of_sections[i].get_config_for_entrance(traverse_start_node);
                        boundaries_repository.add_next_entrance_config_for_node(traverse_start_node, config, current_config_calculation_base, traverse);

                        traverse_end_node = list_of_section_permutations[j].Get_exit_noude();

                        config = list_of_sections[i].get_config_for_exit(traverse_end_node);
                        boundaries_repository.add_next_exit_config_for_node(traverse_end_node, config, current_config_calculation_base, traverse);

                        traverse_repository.add_new_traverse_to_repository(traverse);

                    }

                    calculation_data.add_new_time_for_section(list_of_sections[i].Get_number_of_section(), period + current_config_calculation_base);
                    repetitions--;
                }
                list_of_section_permutations.Reverse();
            }

            // sort all boundaries foe each node
            // it is necessary since now boundaries are created in order of permutation numbering
            boundaries_repository.sort_boundaries_for_all_nodes();

            int calculate_number_of_config_repetitions(double timeForward, double _period)
            {
                return (int)Math.Ceiling(timeForward / _period);
            }
        }

        /// <summary>
        /// Calculation of windows of interaction between traverses, forward in time. It is a second stage of data generation for traffic planing algorythm.
        /// It can be done only after traverses with boundaries are instantiated.
        /// </summary>
        /// <param name="time_forward"></param>
        public void calculate_interactions_between_boundaries_of_traverses(double time_forward)
        {
            double prevoious_iteration_end_time = calculation_data.get_previous_interaction_end_time();
            double current_iteration__end_time = prevoious_iteration_end_time + time_forward;
            List<int> List_of_nodes_with_boundaries = boundaries_repository.get_list_of_nodes_with_boundaries();
            int node;

            for (int i = 0; i < List_of_nodes_with_boundaries.Count; i++)
            {
                node = List_of_nodes_with_boundaries[i];
                if (city.is_node_a_station_node(node))
                {
                    continue;
                }

                boundaries_repository.calculate_interactions_between_traverses_for_node(node, current_iteration__end_time);
            }
            calculation_data.add_next_time_to_interaction_data(current_iteration__end_time);



        }

        /// <summary>
        /// Spliting windows of DC_section so config of windows at traverse start is the same as it is at the exit.
        /// It is neede to avoid nonlinear behaviore of traffic planning algorithm. It is a third stage of data generation for traffic planing algorithm. 
        /// </summary>
        /// <param name="time_forward"></param>
        public void split_windows_of_interactions_for_DC_section_to_be_parallel(double time_forward)
        {

            var list_of_sections = repository_of_sections.get_list_of_sections();
            List<Permutation> list_of_permutations;
            List<Traverse> list_of_traverses_for_permutation;
            Traverse traverse;
            List<split> list_of_split_at_end;
            List<split> list_of_split_at_start;
            int permutation_number;
            int start_traverse_index;
            double limiting_time;

            for (int i = 0; i < list_of_sections.Count; i++)
            {
                // ide po wszystkich sekcjach
                if (list_of_sections[i].what_is_the_type_of_this_section() == typeof(DC_section))
                // jesli to sekcja DC opoalam algorytm
                {
                    list_of_permutations = list_of_sections[i].Get_permutations();
                    // ide po permutacjach danej sekcji
                    for (int j = 0; j < list_of_permutations.Count; j++)
                    {
                        permutation_number = list_of_permutations[j].get_number_of_permutation();
                        // ustalam dla jakich trawersow z permutacji sekcji bedzie dziala dana metoda
                        start_traverse_index = calculation_data.get_last_index_of_calc_for_permutation(permutation_number);
                        limiting_time = calculation_data.get_previous_limiting_time_for_permutation(permutation_number) + time_forward;
                        list_of_traverses_for_permutation = list_of_permutations[j].get_list_of_traverses();
                        // mam limitujacy czas gdzies powinienem zapisac info jaki byl index ostatniego policzonego traversu i od tego miejsca zaczynam
                        // plus licze czas limitujacy
                        // i potem poprostu przy kazdym kolejnym sprawdzam czy przekroczylem limit 
                        for (int k = start_traverse_index; k < list_of_traverses_for_permutation.Count; k++)
                        {
                            traverse = list_of_traverses_for_permutation[k];

                            list_of_split_at_start = calculate_list_of_splits(traverse.get_entrance_windows_list(), list_of_permutations[j], traverse);
                            list_of_split_at_end = calculate_list_of_splits(traverse.get_exit_windows_list(), list_of_permutations[j], traverse);

                            modify_entrance_windows_by_all_exit_splits(list_of_split_at_end);
                            modify_exit_windows_by_all_entrance_splits(list_of_split_at_start);


                            // chyba brakuje kompletnie tego elementu
                            // lacze ze soba okna wejscia i wyjscia traversu (czyli daje odnosniki wewnatrz)
                            // to jest potrzebne 
                            // wystarczy to zrobic przez kolejnosc okien na wejsciu i wyjsciu


                            if (traverse.exit_end_time > limiting_time)
                            {
                                calculation_data.change_permutation_calc_index(list_of_permutations[j].get_number_of_permutation(), k + 1);
                                break;
                            }

                        }
                    
                        calculation_data.set_previous_limiting_time(list_of_permutations[j].get_number_of_permutation(), limiting_time);

                        

                    }
                }
            }

            List<split> calculate_list_of_splits(List<Window> list_of_windows, Permutation permutation, Traverse _traverse) // musze jeszcze dodac sort okien pod wzgledem czasu startu
            {
                List<split> list_of_splits = new List<split>();
                split new_split;

                for (int i = 0; i < list_of_windows.Count - 1; i++)
                {
                    new_split = new split();
                    new_split.permutation_number = permutation.get_number_of_permutation();
                    new_split.permutation = permutation;
                    new_split.traverse_number = _traverse.Number;
                    new_split.traverse = _traverse;
                    new_split.start_window_number = list_of_windows[i].number;
                    new_split.end_window_start_time = list_of_windows[i + 1].number;
                    new_split.start_window_end_time = list_of_windows[i].end_time;
                    new_split.end_window_start_time = list_of_windows[i + 1].start_time;

                    list_of_splits.Add(new_split);
                }
                return list_of_splits;
            }


            void modify_entrance_windows_by_all_exit_splits(List<split> list_of_splits)
            {
                for(int n = 0; n < list_of_splits.Count; n++)
                {
                    var(cross_1, window_1, time_1, side_1) = check_crossing_of_first_part_of_split_with_entrance(list_of_splits[n]);
                    var(cross_2, window_2, time_2, side_2) = check_crossing_of_second_part_of_split_with_entrance(list_of_splits[n]);

                    if(cross_1 && cross_2)
                    {
                        if (window_1 == window_2)
                        {
                            cut_window(window_1, time_1, time_2);
                        }
                        else
                        {
                            trim_window(window_1, time_1, side_1);
                            trim_window(window_2, time_2, side_2);
                        }
                    }
                    else if (cross_1 || cross_2)
                    {
                        if (cross_1)
                        {
                            trim_window(window_1, time_1, side_1);
                        }
                        else
                        {
                            trim_window(window_2, time_2, side_2);
                        }
                    }
                    else
                    {
                        //do nothing
                    }
                }
            }
            void modify_exit_windows_by_all_entrance_splits(List<split> list_of_splits)
            {
                for (int n = 0; n < list_of_splits.Count; n++)
                {
                    var (cross_1, window_1, time_1, side_1) = check_crossing_of_first_part_of_split_with_exit(list_of_splits[n]);
                    var (cross_2, window_2, time_2, side_2) = check_crossing_of_second_part_of_split_with_exit(list_of_splits[n]);


                    if (cross_1 && cross_2)
                    {
                        if (window_1 == window_2)
                        {
                            cut_window(window_1, time_1, time_2);
                        }
                        else
                        {
                            trim_window(window_1, time_1, side_1);
                            trim_window(window_2, time_2, side_2);
                        }
                    }
                    else if (cross_1 || cross_2)
                    {
                        if (cross_1)
                        {
                            trim_window(window_1, time_1, side_1);
                        }
                        else
                        {
                            trim_window(window_2, time_2, side_2);
                        }
                    }
                    else
                    {
                        //do nothing
                    }
                }
            }

            void cut_window(interaction_window window, double start_time, double end_time)
            {

                int new_window_number = windows_repository.get_number_of_next_free_window();
                var new_win = window.deep_copy_of_window(new_window_number);
                var affected_entrance_traverse = window.traverse_with_exit_at_this_window;
                var affected_exit_traverse = window.traverse_with_entrance_at_this_window ;

                trim_window(window, end_time, side.Down);
                trim_window(new_win, start_time, side.Up);

                windows_repository.add_window(new_win);

                affected_entrance_traverse.add_exit_window(new_win);
                affected_entrance_traverse.sort_list_of_windows();

                affected_exit_traverse.add_entrance_window(new_win);
                affected_exit_traverse.sort_list_of_windows();

            }

            

            void trim_window(interaction_window window, double time, side side)
            {
                if (side == side.Up)
                    window.end_time = time;
                else 
                    window.start_time = time;


            }

            (bool is_there_crossing, interaction_window window, double time_of_crossing, side side) check_crossing_of_first_part_of_split_with_entrance(split exit_split)
            {
                var trav = exit_split.traverse;
                var time_of_first_part_of_spilt = exit_split.start_window_end_time;

                var time_teleported_to_entrance = teleport_exit_time_to_begining(trav.begining_start_time, trav.begining_end_time, trav.exit_start_time, trav.exit_end_time, time_of_first_part_of_spilt);

                var(is_crossing, window) = check_crosing_with_list_of_windows(time_teleported_to_entrance,trav.get_entrance_windows_list());

                return (is_crossing, window, time_teleported_to_entrance, side.Up);
            }

            (bool is_there_crossing, interaction_window window, double time_of_crossing, side side) check_crossing_of_second_part_of_split_with_entrance(split exit_split)
            {
                var trav = exit_split.traverse;
                var time_of_second_part_of_spilt = exit_split.end_window_start_time ;

                var time_teleported_to_entrance = teleport_exit_time_to_begining(trav.begining_start_time, trav.begining_end_time, trav.exit_start_time, trav.exit_end_time, time_of_second_part_of_spilt);

                var (is_crossing, window) = check_crosing_with_list_of_windows(time_teleported_to_entrance, trav.get_entrance_windows_list());
                return (is_crossing, window, time_teleported_to_entrance, side.Down);

            }

            (bool is_there_crossing, interaction_window window, double time_of_crossing, side side) check_crossing_of_first_part_of_split_with_exit(split entrance_split)
            {
                var trav = entrance_split.traverse;
                var time_of_first_part_of_spilt = entrance_split.start_window_end_time;

                var time_teleported_to_exit = teleport_begining_time_to_exit(trav.begining_start_time, trav.begining_end_time, trav.exit_start_time, trav.exit_end_time, time_of_first_part_of_spilt);

                var (is_crossing, window) = check_crosing_with_list_of_windows(time_teleported_to_exit, trav.get_exit_windows_list());
                return (is_crossing, window, time_teleported_to_exit, side.Up);
            }

            (bool is_there_crossing, interaction_window window, double time_of_crossing, side side) check_crossing_of_second_part_of_split_with_exit(split entrance_split)
            {
                var trav = entrance_split.traverse;
                var time_of_second_part_of_spilt = entrance_split.end_window_start_time;

                var time_teleported_to_exit = teleport_begining_time_to_exit(trav.begining_start_time, trav.begining_end_time, trav.exit_start_time, trav.exit_end_time, time_of_second_part_of_spilt);

                var (is_crossing, window) = check_crosing_with_list_of_windows(time_teleported_to_exit, trav.get_exit_windows_list());
                return (is_crossing, window, time_teleported_to_exit, side.Down);
            }

            double teleport_exit_time_to_begining(double traverse_begining_start, double traverse_begining_end, double traverse_exit_start, double traverse_exit_end, double time_to_teleport)
            {
                double exit_percentage = (time_to_teleport - traverse_exit_start) / (traverse_exit_end - traverse_exit_start);

                double new_time = traverse_begining_start + exit_percentage * (traverse_begining_end - traverse_begining_start);
                return new_time;
            }
            double teleport_begining_time_to_exit(double traverse_begining_start, double traverse_begining_end, double traverse_exit_start, double traverse_exit_end, double time_to_teleport)
            {
                double begining_percentage = (time_to_teleport - traverse_begining_start) / (traverse_begining_end - traverse_begining_start);

                double new_time = traverse_exit_start + begining_percentage * (traverse_exit_end - traverse_exit_start);
                return new_time;
            }

            (bool is_there_a_crossing, interaction_window window) check_crosing_with_list_of_windows(double time , List<Window> list_of_windows)
            {
                for (int l = 0; l < list_of_windows.Count; l++)
                {
                    if(is_there_crossing_with_window(time , list_of_windows[l]))
                    {
                        return (true, (interaction_window)list_of_windows[l]);
                    }


                }
                return (false, new interaction_window());
            }

            bool is_there_crossing_with_window(double time, Window window)
            {
                if (time > window.start_time && time < window.end_time)  //for now , i assume no point crossing is a problem . it looks like it, have to think about it a but more. 
                    return true;
                else
                    return false;
            }

        }

        /// <summary>
        /// Previous part is generating windows only for interacting traverses so there is no windows for start and stop. That part of algorythm generates those windows based on interactions
        /// at start/stop traverse on the othere side. if there is still no windows generated by previous part of algorithm.
        /// </summary>
        /// <param name="time_forward"></param>
        public void generate_windows_for_start_and_stop(double time_forward)  //wszystkie metody powinny byc przerobione na wersje limiting time i pamietc gdzie skonczyly poprzednio
            //narazie to totalnie niema sensu i bede tracic przez to czas , zostawiam to jak jest i ide dalej
            //jak juz calosc bedzie dzialac to poproawie caly ten zestaw metod.
        {

            var list_of_permutations = permutations.get_list_of_permutation();
            int index_of_traverse_to_calculate;
            double limiting_time = 0 + time_forward;
            // ide po wszystkich permutacjach miasta
            // sprawdzam czy to jest start lub stop
            // jesli to jest start to kopiuje dane okna z konca na poczatek (tworze nowe okna)
            // jesli to jest stop to kopiuje dane okna z startu na koniec (tworze nowe onka)

            for (int i = 0; i < list_of_permutations.Count; i++)
            {
                if (is_this_a_start_permutation(list_of_permutations[i]))
                {
                    index_of_traverse_to_calculate = calculation_data.get_start_stop_data_for_permutation(list_of_permutations[i].get_number_of_permutation()).Index_of_traverse_to_calculate_next;

                    generate_windows_for_start(list_of_permutations[i], 0,limiting_time);

                }
                if (is_this_a_stop_permutation(list_of_permutations[i]))
                {

                    generate_windows_for_stop(list_of_permutations[i], 0, limiting_time);


                }
            }

            void generate_windows_for_start(Permutation permutation, int start_index, double _limiting_time)
            {
                var _traverses = permutation.get_list_of_traverses();
                List<Window> list_of_exit_windows;
                interaction_window exit_window;
                int new_window_number;
                start_window new_window;
                double start_time;
                double end_time;    
                // go thru all traverse
                for (int i = start_index; i < _traverses.Count; i++)
                {
                    list_of_exit_windows = _traverses[i].get_exit_windows_list();
                    for (int j = 0; j < list_of_exit_windows.Count; j++)
                    {
                        new_window_number = windows_repository.get_number_of_next_free_window();
                        exit_window = (interaction_window)list_of_exit_windows[j];
                        new_window = exit_window.deep_copy_of_window_for_start(new_window_number);
                        start_time = teleport_exit_time_to_begining(_traverses[i].begining_start_time, _traverses[i].begining_end_time, _traverses[i].exit_start_time, _traverses[i].exit_end_time, list_of_exit_windows[j].start_time);
                        end_time = teleport_exit_time_to_begining(_traverses[i].begining_start_time, _traverses[i].begining_end_time, _traverses[i].exit_start_time, _traverses[i].exit_end_time, list_of_exit_windows[j].end_time);
                        new_window.start_time = start_time;
                        new_window.end_time = end_time;
                        _traverses[i].add_entrance_window(new_window);
                        windows_repository.add_window(new_window);

                    }
                    if (_traverses[i].begining_start_time > _limiting_time)
                    {
                        break;
                    }
                    // check time of traverse start if it is > limit time
                    // update traverse index i calculation data
                    // and brake ;




                }


            }
            void generate_windows_for_stop(Permutation permutation, int start_index, double _limiting_time)
            {
                var _traverses = permutation.get_list_of_traverses();
                List<Window> list_of_entrance_windows;
                interaction_window entrance_window;
                int new_window_number;
                end_window new_window;
                double start_time;
                double end_time;
                // go thru all traverse
                for (int i = start_index; i < _traverses.Count; i++)
                {
                    list_of_entrance_windows = _traverses[i].get_entrance_windows_list();
                    for (int j = 0; j < list_of_entrance_windows.Count; j++)
                    {
                        new_window_number = windows_repository.get_number_of_next_free_window();
                        entrance_window = (interaction_window)list_of_entrance_windows[j];
                        new_window = entrance_window.deep_copy_of_window_for_stop(new_window_number);
                        start_time = teleport_begining_time_to_exit(_traverses[i].begining_start_time, _traverses[i].begining_end_time, _traverses[i].exit_start_time, _traverses[i].exit_end_time, list_of_entrance_windows[j].start_time);
                        end_time = teleport_begining_time_to_exit(_traverses[i].begining_start_time, _traverses[i].begining_end_time, _traverses[i].exit_start_time, _traverses[i].exit_end_time, list_of_entrance_windows[j].end_time);
                        new_window.start_time = start_time;
                        new_window.end_time = end_time;
                        _traverses[i].add_exit_window(new_window);
                        windows_repository.add_window(new_window);

                    }
                    if (_traverses[i].begining_start_time > _limiting_time)
                    {
                        break;
                    }
                    // check time of traverse start if it is > limit time
                    // update traverse index i calculation data
                    // and brake ;




                }

            }
            
            double teleport_exit_time_to_begining(double traverse_begining_start, double traverse_begining_end, double traverse_exit_start, double traverse_exit_end, double time_to_teleport)
            {
                double exit_percentage = (time_to_teleport - traverse_exit_start) / (traverse_exit_end - traverse_exit_start);

                double new_time = traverse_begining_start + exit_percentage * (traverse_begining_end - traverse_begining_start);
                return new_time;
            }
            double teleport_begining_time_to_exit(double traverse_begining_start, double traverse_begining_end, double traverse_exit_start, double traverse_exit_end, double time_to_teleport)
            {
                double begining_percentage = (time_to_teleport - traverse_begining_start) / (traverse_begining_end - traverse_begining_start);

                double new_time = traverse_exit_start + begining_percentage * (traverse_exit_end - traverse_exit_start);
                return new_time;
            }
            
            bool is_this_a_start_permutation(Permutation permutation)
            {

                var start_node = permutation.Get_start_noude();
                return city.is_node_a_station_node(start_node);
            }
            bool is_this_a_stop_permutation(Permutation permutation)
            {
                var end_node = permutation.Get_exit_noude();
                return city.is_node_a_station_node(end_node);
            }
        }
        
        /// <summary>
        /// deactivate sort section eit window if ther are more than 2. only 2 longest are active 
        /// </summary>
        /// <param name="time_forward"></param>
        public void deactivate_excess_exit_for_sort_sections(double time_forward)
        {
            var list_of_sections = repository_of_sections.get_list_of_sections();
            List<Permutation> list_of_permutations;
            List<Traverse> list_of_traverses;
            List<Window> list_of_exit_windows;
            int max_number_of_active_exit_windows_for_given_section;


            for (int i = 0; i < list_of_sections.Count; i++)
            {
                // ide po wszystkich sekcjach
                if (list_of_sections[i].what_is_the_type_of_this_section() == typeof(Sort_section))
                {

                    max_number_of_active_exit_windows_for_given_section = list_of_sections[i].get_max_number_of_active_exit_windows();
                    list_of_permutations = list_of_sections[i].Get_permutations();
                    // there is only one permutation thru sort section

                    list_of_traverses = list_of_permutations[0].get_list_of_traverses();
                    for(int j = 0; j < list_of_traverses.Count; j++)
                    {
                        if (list_of_traverses[j].exit_end_time > time_forward)
                            continue;

                        list_of_exit_windows = list_of_traverses[j].get_exit_windows_list();

                        if (list_of_exit_windows.Count > max_number_of_active_exit_windows_for_given_section)
                        {
                            // deactywacja wszystkich okien poza 2 najdluzszymi
                            deactivate_windows_apart_from_x_longest_ones(list_of_exit_windows, max_number_of_active_exit_windows_for_given_section);
                            // moge tutaj dodac inne wersje usuwania okien 

                        }

                        // chce tutaj dodac inne wersje zostawiania aktywnych okien
                        // 3 i wiecej zalezy od ukladu jaki zrobie 


                    }
                }
            }


            //void deactivate_windows_apart_from_two_longest(List<Window> list_of_win)
            //{
            //    //sort win by length
            //    list_of_exit_windows.Sort((x, y) => (x.end_time - x.start_time).CompareTo(y.end_time - y.start_time));


            //    //deactivate ind 2 and so forth
            //    for(int i = 2; i < list_of_exit_windows.Count; i++)
            //    {
            //        list_of_exit_windows[i].window_is_deactivated = true;
            //    }

            //}

            void deactivate_windows_apart_from_x_longest_ones(List<Window> list_of_win, int numer_of_active_windows)
            {
                //sort win by length
                list_of_exit_windows.Sort((x, y) => (x.end_time - x.start_time).CompareTo(y.end_time - y.start_time));


                //deactivate ind 2 and so forth
                for (int i = numer_of_active_windows; i < list_of_exit_windows.Count; i++)
                {
                    list_of_exit_windows[i].window_is_deactivated = true;
                }

            }

        }

        //faza 6
        //dodanie do okien info o max ilosci kapsol i policzenie pozycji okien w traversach
        public void calculate_max_pod_number_for_windows_and_start_distance_in_traverse(double time_forward, double pod_length ,double min_separation)
        {
            var list_of_windows = windows_repository.get_list_of_windows();
            int permutation_number;
            Permutation permutation;
            double length;
            int max_number;
            Traverse traverse_starting_at_window;
            Traverse traverse_ending_at_window;
            double traverse_starting_at_window_begining_start_time;
            double traverse_ending_at_window_end_start_time;
            double window_start_time;
            double traverse_starting_at_windows_start_speed;
            double traverse_ending_at_windows_end_speed;

            for (int i = 0; i < list_of_windows.Count; i++)
            {
                if (list_of_windows[i].what_is_the_type() == typeof(start_window))
                {
                    start_window window = (start_window)list_of_windows[i];
                    permutation_number = window.permutation_which_start_at_this_window;
                    permutation = permutations.Get_permutation(permutation_number);
                    //to wyglada jak blad bo do max win capacity biore traverse length 
                    window.speed = permutation.get_entrance_speed();
                    window.calculate_max_pod_number();

                    // traverse zaczynajacy sie w oknie
                    traverse_starting_at_window = window.traverse_with_entrance_at_this_window;

                    // czas poczatek traversu
                    traverse_starting_at_window_begining_start_time = traverse_starting_at_window.begining_start_time;
                    
                    // czas poczatku okna
                    window_start_time = window.start_time;

                    // predkosc na poczatku traversu
                    traverse_starting_at_windows_start_speed = permutation.get_entrance_speed();

                    // przypisanie dystansu
                    window.position_of_window_begining_in_traverse_with_start_at_this_window = (window_start_time - traverse_starting_at_window_begining_start_time) * traverse_starting_at_windows_start_speed;

                    

                }

                else if(list_of_windows[i].what_is_the_type() == typeof(end_window))
                {
                    end_window window = (end_window)list_of_windows[i];
                    permutation_number = window.permutation_which_exit_at_window;
                    permutation = permutations.Get_permutation(permutation_number);
                    window.speed = permutation.get_exit_speed();
                    window.calculate_max_pod_number();

                    // traverse konczacy sie na oknie
                    traverse_ending_at_window = window.traverse_with_exit_at_this_window;

                    // czas poczatek traversu
                    traverse_ending_at_window_end_start_time = traverse_ending_at_window.begining_start_time;

                    // czas poczatku okna
                    window_start_time = window.start_time;

                    // predkosc na poczatku traversu. predkosc w tym oknie obu trawersow jest rowna
                    traverse_starting_at_windows_start_speed = permutation.get_exit_speed();

                    // przypisanie dystansu
                    window.position_of_window_begining_in_traverse_with_ends_at_this_window = (window_start_time - traverse_ending_at_window_end_start_time) * traverse_starting_at_windows_start_speed;

                }

                else if (list_of_windows[i].what_is_the_type() == typeof(interaction_window))
                {
                    // czy to jest ok ze mam tylko jedna predkosc dla tego wyliczenia
                    // predkosci na wejsciu i wyjsciu moga sie roznic
                    // czy to jest ok ? ze teraz mam tylko jedna ?
                    // chyba nie, bo powinienem sprawdzic obie i przypisac ta ktora jest mniejsza do wyliczenia dopuszczalnej 
                    // ilosci kapsol w oknie
                    // sprawadza sie to do pytnia czy moze byc roznica predkosci na granicy sekcji 
                    // narazie niemoze tak byc wiec tak jak jest z jedna predkosci jest ok
                    // zmiana tego bedzie skomplikowana wiec raczej jeszcze przez dosc dlugi czas niebede tego robic

                    interaction_window window = (interaction_window)list_of_windows[i];
                    permutation_number = window.permutation_which_start_at_this_window;
                    permutation = permutations.Get_permutation(permutation_number);
                    window.speed = permutation.get_entrance_speed();
                    window.calculate_max_pod_number();


                    // traverse zaczynajacy sie w oknie
                    traverse_starting_at_window = window.traverse_with_entrance_at_this_window;
                    traverse_ending_at_window = window.traverse_with_exit_at_this_window;

                    // czas poczatek traversu
                    traverse_starting_at_window_begining_start_time = traverse_starting_at_window.begining_start_time;
                    traverse_ending_at_window_end_start_time = traverse_ending_at_window.exit_start_time;

                    // czas poczatku okna
                    window_start_time = window.start_time;

                    // predkosc na poczatku traversu. predkosc w tym oknie obu trawersow jest rowna
                    traverse_starting_at_windows_start_speed = permutation.get_entrance_speed();

                    // przypisanie dystansu
                    window.position_of_window_in_traverse_start = (window_start_time - traverse_starting_at_window_begining_start_time) * traverse_starting_at_windows_start_speed;
                    window.position_of_window_in_traverse_end = (window_start_time - traverse_ending_at_window_end_start_time) * traverse_starting_at_windows_start_speed;

                    
                }


            }


        }
        

        

        struct split
        {
            public int permutation_number { get; set; }
            public Permutation permutation { get; set; }
            public int traverse_number { get; set; }
            public Traverse traverse { get; set; }
            public int start_window_number { get; set; }
            public int end_window_number { get; set; }
            public double start_window_end_time { get; set; }
            public double end_window_start_time { get; set; }


        }
        enum side
        {
            Up,
            Down
        }

    }

    // moze jeszcze brakowac jakis informacji w oknach , ale to juz bedzie latwo dodac, postaram sie napisac ta metode tak ze 
    // bedzie kopiowac cale info w obiektach nawet jesli dodam jeszcze cos nowego 

    public class CalculationData
    {
        private List<double>[] _sections_config_time_repository;
        private List<double> _interaction_data_repository;
        private List<permutation_calc_data> _list_of_permutation_calc_data;
        private List<start_stop_calc_data> _list_of_data_for_start_stop_windows;
        private RepositoryOfSections repositoryOfSections;


        // tutaj beda przechowywane dane o czasie poczatku obliczen w kazdej iteracji [czas globalny]   

        // dla kazdej sekcji
        // tylko jak przechowywac te dane
        // moge zrobic macierz gdzie index bedzie sie zgadzac z numerem listy 

        public CalculationData(int number_of_sections, int number_of_permutarions, RepositoryOfSections repositoryOfSections)
        {
            this.repositoryOfSections = repositoryOfSections;

            instantiate_config_time_repository(number_of_sections);
            _interaction_data_repository = new List<double>();
            _interaction_data_repository.Add(0);
            instantiate_permutation_calc_data_storage(number_of_permutarions);
            instantiate_start_stop_data_storage(number_of_permutarions);
        }
        private void instantiate_config_time_repository(int number_of_sections)
        {
            _sections_config_time_repository = new List<double>[number_of_sections];
            add_zero_at_start_of_each_list();



            void add_zero_at_start_of_each_list()
            {
                for (int i = 0; i < _sections_config_time_repository.Length; i++)
                {
                    _sections_config_time_repository[i] = new List<double>();
                    double current_section_synch_time = repositoryOfSections.get_synch_time_for_section_with_index(i);
                    add_new_time_for_section(i, current_section_synch_time);
                }
            }
        }

        private void instantiate_start_stop_data_storage(int number_of_permutations)
        {
            _list_of_data_for_start_stop_windows = new List<start_stop_calc_data>();
            for (int i = 0; i < number_of_permutations; i++)
            {
                _list_of_data_for_start_stop_windows.Add(new start_stop_calc_data(i,0));
            }
            
        }

        public void add_new_time_for_section(int section_number, double time)
        {
            //pytanie czy chce usuwac stare czasy, czy moga sie do czegos przydac
            //narazie je zostawie
            _sections_config_time_repository[section_number].Add(time);

        }

        public double get_last_calculation_time_for_section(int section_number)
        {
            var number = _sections_config_time_repository[section_number].Count - 1;
            return _sections_config_time_repository[section_number][number];
        }

        public double get_previous_interaction_end_time()
        {
            int last_index = _interaction_data_repository.Count - 1;
            return _interaction_data_repository[last_index];
        }

        public void add_next_time_to_interaction_data(double time)
        {
            _interaction_data_repository.Add(time);
        }

        private void instantiate_permutation_calc_data_storage(int number_of_permutations)
        {
            _list_of_permutation_calc_data = new List<permutation_calc_data>();
            for (int i = 0; i < number_of_permutations; i++)
            {
                var perm_calc_data = new permutation_calc_data();
                perm_calc_data.permutation_number = i;
                perm_calc_data.index_of_last_calculated_traverse = 0;
                perm_calc_data.previous_limiting_time = 0;
                _list_of_permutation_calc_data.Add(perm_calc_data);

            }

        }

        public void change_permutation_calc_index(int permutation, int new_index)
        {
            var calc = _list_of_permutation_calc_data[permutation];
            calc.index_of_last_calculated_traverse = new_index;
            _list_of_permutation_calc_data[permutation] = calc;
        }

        public int get_last_index_of_calc_for_permutation(int permutation_number)
        {
            return _list_of_permutation_calc_data[permutation_number].index_of_last_calculated_traverse;
        }
        public double get_previous_limiting_time_for_permutation(int permutation)
        {
            return _list_of_permutation_calc_data[permutation].previous_limiting_time;
        }

        public void set_previous_limiting_time(int permutation, double time)
        {
            var calc = _list_of_permutation_calc_data[permutation];
            calc.previous_limiting_time = time;
            _list_of_permutation_calc_data[permutation] = calc;
        }

        public start_stop_calc_data get_start_stop_data_for_permutation(int permutation)
        {
            return _list_of_data_for_start_stop_windows[permutation];
        }

    }

    public struct permutation_calc_data
    {
        public int permutation_number { get; set; }
        public double previous_limiting_time { get; set; }
        public int index_of_last_calculated_traverse { get; set; }


    }

    public struct start_stop_calc_data
    {
        public int Permutation_number { get; set; }
        public int Index_of_traverse_to_calculate_next { get; set; }

        public start_stop_calc_data(int number, int index)
        {
            Permutation_number = number;
            Index_of_traverse_to_calculate_next = index;
        }

    }


}


