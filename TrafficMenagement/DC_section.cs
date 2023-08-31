using System.Collections.Generic;
using System;
namespace Symulation
{
    // tu jest definicja sekcji
    // liczenie ilosci permutacji
    //liczenie route dla permutacji
    //...
    // liczenie max dlugosci traversow


    //popelnilem blad piszac ta metode, pisalem ja za dlugo i jest tu duzo chaosu
    //dzila ale niepotrzebnie pisalem metody ktore sa podobne do tego co juz mam napisane w innych miejscach
    public class DC_section : CitySection
    {
        private readonly int _number_Of_Section;
        private CityDataStorage CityMap;
        private CityDataStorage CityMapCopy;
        private List<int> _list_of_I_nodes;
        private List<int> _list_of_O_nodes;
        private RepositoryOfPermutations _repository;  //reference to city repositiry of permutations
        private List<Permutation> _list_of_permutations;    // list of permutations in given section
        private List<Permutation> _list_of_priority_permutations;
        private List<Permutation> _list_of_start_permutations;
        private List<Permutation> _list_of_end_permutations;

        private List<int> _list_of_tracks_in_section;
        private Section_config_DC_velocity section_config;
        private List<Traverse> _list_of_traverses;      
        private bool is_synchronization_defined;
        private double synchronization_time;


        public DC_section(int number_of_section, CityDataStorage cityData, List<int> list_of_I_nodes, List<int> list_of_O_nodes,List<(int,int)> list_of_priority_permutation_directions, RepositoryOfPermutations repository, double DC_length) //haracteristic node is any node inside section
        {
            _number_Of_Section = number_of_section;
            CityMap = cityData;
            _list_of_I_nodes = list_of_I_nodes;
            _list_of_O_nodes = list_of_O_nodes;
            _repository = repository;

            _list_of_permutations = new List<Permutation>();
            _list_of_priority_permutations = new List<Permutation>();
            _list_of_start_permutations = new List<Permutation>();
            _list_of_end_permutations = new List<Permutation>();
            _list_of_tracks_in_section = new List<int>();

            is_synchronization_defined = false;
            
            // mod city around stations 
            // to jest miejsce gdzie locacje wokol stacji sa modyfikowane
            // a wyglada na to ze podczas generowania interakcji ta operacja jest powtarzana
            // ta operacja jest powtarzana dla kazdej sekcji co jest szalone, trzeba zadbac zeby nic zlego sie przez to niedzialo
            // lub wykonywac ta operacje tylko na stacja danej sekcji
            // to wymaga posprzatania

            prepare_list_of_permutations();
            prepare_list_of_priority_permutations(list_of_priority_permutation_directions);
            prepare_list_of_start_permutation();
            prepare_list_of_end_permutations();
            calculate_entrance_and_exit_speed_for_each_permutations(_list_of_permutations);

            calculate_max_size_of_traverse_for_each_permutation_of_section();
            mark_all_track_in_permutations();
            // czy to jest potrzebne
            mark_all_tracks_in_section_and_add_to_list();
            //matrix_of_permutations = create_matrix_of_permutations(_list_of_permutations.Count);


            CityMapCopy = CityMap.DeepCopyCity(); //important to have that here at the end, because some modifications are made to city before 
            _list_of_permutations.Reverse();

            //section config for each posible permutation
            section_config = new Section_config_DC_velocity(cityData, list_of_I_nodes, list_of_O_nodes, _list_of_permutations,_list_of_priority_permutations,_list_of_start_permutations,_list_of_end_permutations, _list_of_tracks_in_section , DC_length);
            //Console.WriteLine("  ");
            //Console.WriteLine("Section number: {0}", _number_Of_Section);

            // tutaj dzieje sie magia skladania razem roznych permutacji, przejazdu. 
            // teraz np jest tak ze jak robie skladanie to no koniec jedna z permutacji ktora nazywam priorytetowa wydluzam 
            // o tyle ile sie da ta priorytetowa permutacje
            // to jest zalazek tego co chcialbym zrobic docelowo 
            // czyli rozne sposoby optymalizacji zamiast tego skupiarnia sie na realnosci symulacji i przyspieszania
            section_config.calculate_config(_list_of_permutations, true ,number_of_section);
            //perform_check_for_all_posible_combinations(matrix_of_permutations, _list_of_permutations,CityMap, _list_of_I_nodes, _list_of_O_nodes);
        }

        // raczej powinienem dodac metode ktora robi spis wszystkich trackow w danej sekscji
        // potem dla kazdego tracka w sekcji ta informacja zostanie dodana kazdego tracku w jakiej sekcji sie znajduje
        // to pozwoli sprawdzac przez jakie sekcje przejerzdza dana trasa przejazdu
        // co jeszcze trzeba sprawdzic, przez jakie permutacje przejerzdza dana trasa przejazdu . jak to zrobic najlepiej ?  
        // jak ustale przez jakie sekcje jade. to dla kazdej sekcji 
        // jade wzdloz drogi i spisuje sekcje i permutacje dla kazdego tracku - w pewnym sensie juz to mam bo opis drogi zawiera wszystkie tracki
        // i kazdy track ma sekcje i permutacje 


        // ta metoda ulatwi definiowanie sekcji wiec warto to zrobic, narazie dla oszczednosci czasu tego nierobie bo to jest zadziwiajaca skomplikowane
        // oznacza to ze narazie musze definiowac oddzielnie liste wejsc i wyjsc

        //public override double[] get_config_for_node_number(int node_number)
        //{
        //     return section_config.get_config_for_node(node_number);
        //}
        private void calculate_entrance_and_exit_speed_for_each_permutations(List<Permutation> permutations)
        {
            foreach(Permutation permutation in permutations)
            {
                calculate_speed_at_entrance(permutation);
                calculate_speed_at_exit(permutation);
            }
        }
        private void calculate_speed_at_exit(Permutation permutation)
            {
                var route = permutation.Get_route();
                var index_of_last_track = route.GetLength(0) - 1;
                int last_track = route[index_of_last_track][2];

                permutation.set_exit_speed(CityMap.GetSpeedLimitForTrack_m_s(last_track));
            }

        private void calculate_speed_at_entrance(Permutation permutation)
        {
            var route = permutation.Get_route();
            int first_track = route[0][2];
            permutation.set_entrance_speed(CityMap.GetSpeedLimitForTrack_m_s(first_track));
        }

        public override int get_max_number_of_active_exit_windows()
        {
            throw new NotImplementedException();
        }

        public override bool is_synchronization_def_for_section()
        {
            return is_synchronization_defined;
        }

        public override void define_synch_for_secton(double synch_time)
        {
            is_synchronization_defined = true;
            synchronization_time = synch_time;
        }

        public override double get_synchro_time_for_section()
        {
            return synchronization_time;
        }

        public override Type what_is_the_type_of_this_section()
        {
            return typeof(DC_section);
        }

        public override double get_period_of_section()
        {
            return section_config.get_period();
        }
        public override List<int> get_list_of_I_nodes()
        {
            return _list_of_I_nodes;
        }

        public override List<int> get_list_of_O_nodes()
        {
            return _list_of_O_nodes;
        }

        public override List<double[]> get_config_for_entrance(int node_number)
        {
            return section_config.get_config_for_entrance(node_number);
        }

        public override List<double[]> get_config_for_exit(int node_number)
        {
            return section_config.get_config_for_exit(node_number);
        }


        /// <summary>
        /// length of mod is defined inside
        /// speed of mod is defined inside
        /// </summary>
        private void mod_city_around_stations()
        {

            double length_of_mod = 5;
            double speed_of_mod_section = 1.4;// [m/s]

            //for each station
            var list_of_staions = CityMap.GetListOfAllStationsInCity();
            int node;
            List<int> list_of_attached_tracks;
            int directionality_node;
            var station_mod = new station_mod(CityMap);

            for (int i = 0; i < CityMap.NumberOfStations(); i++)
            {
                node = list_of_staions[i].NumberOfAttachedNode;
                list_of_attached_tracks = CityMap.GetListOfAllTracksConnectedToNode(node);

                for (int j = 0; j < list_of_attached_tracks.Count; j++)
                {
                    directionality_node = CityMap.GetDirectionalityNodeForTrack(list_of_attached_tracks[j]);
                    //add mod from side of station node (defined length of new track, and defined speed of modyfied section)

                    if (directionality_node == node)
                    {
                        // delay acceleration for track where directionality node == to station node
                        station_mod.start_mod(list_of_attached_tracks[j], length_of_mod, speed_of_mod_section);
                    }
                    else
                    {
                        //tu jest generowany jakis blad w odnosnikach nouda od ktorego zaczyna sie modyfikowany track (gdzie dodawany jest nowy noude)
                        // accelerate braking for track where directionality node != to station node
                        station_mod.stop_mod(list_of_attached_tracks[j], length_of_mod, speed_of_mod_section);
                    }
                }
            }
        }


        /// <summary>
        /// this method work olny for DC section with one D intersection
        /// </summary>
        public void prepare_list_of_permutations()
        {
            // repeat for each input 
            for (int i = 0; i < _list_of_I_nodes.Count; i++)
            {
                var current_In_node = _list_of_I_nodes[i];
                var current_node_number = _list_of_I_nodes[i];
                int error_code = 0;
                int type_of_node = 0;

            // go forward
            move_to_next_node:
                (current_node_number, error_code) = CityMap.Get_number_of_node_along_traffic(current_node_number);
                // check type of node
                type_of_node = check_type_of_node_connection(current_node_number);
                // switch(depending on node type decide what to do next)
                switch (type_of_node)
                {
                    case 1: //section out node (exit node)
                        add_permutation_to_section_list_of_permutations_and_repository(_number_Of_Section, current_In_node, current_node_number);
                        break;

                    case 2: //connector node (two tracks connected)
                        goto move_to_next_node;

                    case 3: //node is divergence intersection (1 in track, 2 out tracks). it means that both exit will be reached from given in node
                        add_permutation_to_section_list_of_permutations_and_repository(_number_Of_Section, current_In_node, _list_of_O_nodes[0]);
                        add_permutation_to_section_list_of_permutations_and_repository(_number_Of_Section, current_In_node, _list_of_O_nodes[1]);
                        break;

                    case 4: //node is convergence intersection (2 in tracks, 1 out track), it is posible to establish where to go next along the way of traffic 
                        goto move_to_next_node;
                }
            }
        }

        private void prepare_list_of_priority_permutations(List<(int,int)> list_of_priority_directions)
        {
            if (list_of_priority_directions.Count != 0)
            {
                var (priority_start_node, priority_end_node) = list_of_priority_directions[0];


                for (int i = 0; i < _list_of_permutations.Count; i++)
                {
                    var(perm_start_node,perm_end_node) = _list_of_permutations[i].Get_start_and_end_nodes();

                    if (priority_start_node == perm_start_node && priority_end_node == perm_end_node)
                    {
                        _list_of_priority_permutations.Add(_list_of_permutations[i]);
                    }
                }
            }
        }

        private void prepare_list_of_end_permutations()
        {
            for (int i = 0; i < _list_of_permutations.Count; i++)
            {
                var (perm_start_node, perm_end_node) = _list_of_permutations[i].Get_start_and_end_nodes();

                if (CityMap.is_node_a_station_node(perm_end_node))
                {
                    _list_of_end_permutations.Add(_list_of_permutations[i]);
                }
            }
        }

        private void prepare_list_of_start_permutation()
        {
            for (int i = 0; i < _list_of_permutations.Count; i++)
            {
                var (perm_start_node, perm_end_node) = _list_of_permutations[i].Get_start_and_end_nodes();

                if (CityMap.is_node_a_station_node(perm_start_node))
                {
                    _list_of_start_permutations.Add(_list_of_permutations[i]);
                }
            }
        }



        //ta metoda powinna byc na poziomie city
        private int check_type_of_node_connection(int node_number)
        {
            if (is_node_out_node(node_number))
                return 1;

            else if (is_node_a_connector_node(node_number))
                return 2;

            else if (is_node_a_divergence(node_number))
                return 3;

            else if (is_node_a_convergence(node_number))
                return 4;
            else
                return -1; //error code
        }
        private int add_permutation_to_section_list_of_permutations_and_repository(int number_of_city_section, int in_node, int out_node)
        {
            // add permutation to list of permutations in city perository of permutations
            var next_permutation_number = _repository.return_number_of_next_free_perm_number();
            // add permutation to list of permutations in section

            var route = calculate_route_for_permutation(in_node, out_node);
            var length = calculate_length_of_route(route);
            var min_speed = calculate_min_speed_along_permutation(route);
            bool is_route_ok = is_whole_route_inside_section(route, out_node);// need to stop if that heppens
            if (is_route_ok)
            {
                var new_permutation = new Permutation(next_permutation_number, in_node, out_node, number_of_city_section);
                new_permutation.Set_route(route);
                new_permutation.set_permutation_length(length);
                new_permutation.set_min_speed_along_permutation(min_speed);
                _list_of_permutations.Add(new_permutation);
                _repository.add_permutation_to_list_of_permutations(new_permutation);
                return 0; // error code
            }
            else
            {
                //add permutation to lists so it is easy to debug
                var new_permutation = new Permutation(next_permutation_number, in_node, out_node, number_of_city_section);
                _list_of_permutations.Add(new_permutation);
                _repository.add_permutation_to_list_of_permutations(new_permutation);
                return 1; // error code
            }
        }
        private double calculate_length_of_route(int [][] route)
        {
            double length = 0;
            for(int i = 0; i < route.GetLength(0); i++)
            {
                length += CityMap.GetLenghtOfTrack(route[i][2]);
            }

            return length;
        }

        private double calculate_min_speed_along_permutation(int[][] route)  // moge to ulepszyc bo tak naprawde to powinno byc liczone za skrzyzowaniem D. Brak tego uwzglednienia obniza sprawnosc.
                                                                             // narazie niechce tego robic bo chce dojsc jak najszybciej do dzialajacego systemu.
        {
            double min_speed=100;
            for (int i=0; i < route.GetLength(0); i++)
            {
                if (i == 0)
                {
                    min_speed = CityMap.GetSpeedLimitForTrack_m_s(route[i][2]);
                    continue;
                }
                if(min_speed > CityMap.GetSpeedLimitForTrack_m_s(route[i][2]))
                {
                    min_speed = CityMap.GetSpeedLimitForTrack_m_s(route[i][2]);
                }
            }
            return min_speed;
        }
        private void mark_all_track_in_permutations()
        {
            for (int i = 0; i < _list_of_permutations.Count; i++)
            {
                var route = _list_of_permutations[i].Get_route();
                for (int j = 0; j < route.GetLength(0); j++)
                {
                    var track_number = route[j][2];
                    var permutation_number = _list_of_permutations[i].get_number_of_permutation();
                    CityMap.add_permutation_to_track(track_number, permutation_number);

                }
            }
        }

        private void mark_all_tracks_in_section_and_add_to_list()
        {
            for (int i = 0; i < _list_of_permutations.Count; i++)
            {
                var route = _list_of_permutations[i].Get_route();
                for (int j = 0; j < route.GetLength(0); j++)
                {
                    var track_number = route[j][2];
                    if (!CityMap.is_section_for_track_defined(track_number))
                        CityMap.add_section_to_track(track_number, _number_Of_Section);

                    if(!_list_of_tracks_in_section.Contains(track_number))
                        _list_of_tracks_in_section.Add(track_number);
                }
            }
        }
        private bool is_node_out_node(int node_number)
        {
            return _list_of_O_nodes.Contains(node_number);
        }
        private bool is_node_a_connector_node(int number)
        {
            var list = CityMap.GetListOfAllTracksConnectedToNode(number);

            if (list.Count == 2)
                return true;
            else
                return false;
        }
        private bool is_node_a_divergence(int number)
        {
            var list = CityMap.GetListOfAllTracksConnectedToNode(number);

            if (list.Count != 3)
                return false;
            else
            {
                var number_of_out_tracks_from_intersection_node = count_number_of_out_track(number);

                if (number_of_out_tracks_from_intersection_node == 2)
                    return true;
                else
                    return false;

            }
        }
        private bool is_node_a_convergence(int number)
        {
            var list = CityMap.GetListOfAllTracksConnectedToNode(number);

            if (list.Count != 3)
                return false;
            else
            {
                var number_of_out_tracks_from_intersection_node = count_number_of_out_track(number);

                if (number_of_out_tracks_from_intersection_node == 1)
                    return true;
                else
                    return false;

            }
        }
        private int count_number_of_out_track(int node_number)
        {
            var number_of_out_tracks_from_intersection_node = 0;
            var list = CityMap.GetListOfAllTracksConnectedToNode(node_number);
            foreach (int n in list)
            {
                if (CityMap.GetDirectionalityNodeForTrack(n) == node_number)
                {
                    number_of_out_tracks_from_intersection_node += 1;
                }
            }
            return number_of_out_tracks_from_intersection_node;
        }
        private int[][] calculate_route_for_permutation(int start_node, int end_node)// ta metoda bazuje na szukaniu drogi najkrotszej, wiec istnieje ryzyko teoretycznie ze wyszukam zle polaczenie. sprawdzam to i jeli bedzie taki problem
                                                                                     // to napisze nowa metode
        {
            var search = new SearchForRoutesMain(CityMap);
            var route = search.SeekRouteBetweenNodes(start_node, end_node)[0];
            return route;
        }
        private bool is_whole_route_inside_section(int[][] route, int end_Node)
        {
            //have to check if othere out node is in route. if yes that means that route goes out of section and that back egain.
            //othere out node number 
            var list = new List<int>();
            foreach(int n in _list_of_O_nodes)
            {
                list.Add(n);
            }
            list.Remove(end_Node);
            if (list.Count == 0)
            {
                return true; //means that there is only one exit and there is no way for the route to create shorter route that the correct one. 
            }
            else
            {
                var second_exit = list[0];
                for (int i = 0; i < route.GetLength(0); i++)
                {
                    if (route[i][1] == second_exit)
                    {
                        return false; //there is othere out node along the way, so route is not fully inside section
                    }
                }
                return true;
            }


        }
        
        public override List<int> Get_list_of_out_nodes()
        {
            return _list_of_O_nodes;
        }
        public override List<int> Get_list_of_in_nodes()
        {
            return _list_of_I_nodes;
        }
        public override List<Permutation> Get_permutations()
        {
            return _list_of_permutations;
        }
        public override int Get_number_of_section()
        {
            return _number_Of_Section;
        }

        public override List<int> get_list_of_tracks_in_secton()
        {
            return _list_of_tracks_in_section;
        }


        /// <summary>
        /// tutaj powinna byc jakas zbiorcza metoda ktora dla wszystkich permutacji
        /// sprawdza jaka jest max dlugosc traversu i zapisuja ta wartosc w danych danej permutacji
        /// sprawdzanie moze powinno byc skwantowane dla calkowitej ilosci kapsol ?
        /// jesli tak niebedzie to co to wlasciwie oznacza ? czy tylko straty czy tez potencjalnie jakis zysk na polaczeniach pomiedzy 
        /// sekcjami , narazie zrobie tak ze bede wyznaczac max wartosc analogowe tak zeby moc pozniej sprwadzic jak jest z wydajnoscia
        /// w zaleznosci od dlugosci bez zadnych ograniczen dodatkowych, jesli to niejest wydajny pomysly to zeby miec analogowe dlugosci to 
        /// powinno byc widoczne w wynikach moich symulacji
        /// ta metoda powinna sie odwolywac do jakiejs metody wewnatrz danego obiekty sekcja, o nazwie check permutation
        ///
        /// problemem tej metody jest to ze moze byc tak ze nawet jedna kapsula nieda rady przejechac wtedy sym powinna zostac przerwana i wyswitlony
        /// powinien byc error ktory pozwoli zrozumiec co poszlo nietak i to naprawic 
        ///
        ///  method checking if traverse of length x can go thru permutation without errors
        ///  posible errors codes:
        /// - no error:                                                                                      code  0
        /// - length of traverse is larger than permutation itself.                                          code  5
        ///         //can occur only when there is no spped change along permutation
        /// - acceleration goes into intersection, and that is not allowed by now                            code  4
        ///     generested by profile check
        /// - no time to accelerate at the end to achive exit speed: error genareted by profile search       code  3
        /// - no space to start accelerating before end: error generated by mod adding method                code  2
        ///         //traverse is longer then distance from last acceleration to end of permutation
        /// - acceleration mod goes into intersection: error generested by mod adding method                 code  1
        /// - brake mod goes into intersection: error generested by mod adding method                        code -1
        /// - no space to start braking at the begining: error generested by mod adding method               code -2
        ///         //traverse is longer than distance from start to first brake point
        /// - no time to brake at the begining to follow speed limits: error genareted by profile search     code -3
        /// - braking goes into intersection, that is not alloed by now.                                     code -4
        ///
        /// wlasciwie ta metoda jest ok tylko dla DC_section moze w takim razie powinna byc w srodku
        /// sekcji DC tak zeby mozna bylo dawac inny check w zaleznosci od sekcji i jej rodzaju ?
        /// dokladnie tak powinno byc, dla roznych rodzajaw sekcji check bedzie dzialac inacze czyli check powinien byc
        /// zdefiniowany w srodku samej sekcji
        ///
        /// sprawdzenie przeciecia profilu przy przyspieszaniu i hamowaniu ze skrzyzowaniem niejst takie oczywiste bo 
        /// przy hamowaniu sa juz pousuwane sekcje drogi, czyli musze to zrobic inaczej
        /// napisze metode w ktorej sprawdzam jakie nody sa w strefie przyspieszania i hamowania i sprawdzam czy sa skrzyzowaniami.
        /// to powinno byc dos latwe do zrobienia, w opisie profilu mam noude poczatkowy i dlugosc hamowania/ przyspieszania
        /// wiec poprostu jade wzdluz i spisuje noudy az wyczerpie dlugosc sekcji hamowania/ przyspieszania.
        /// route dla danej permutacji bedzie nawigacja
        /// </summary>
        public override void print__traverse_length_of_all_permutations_of_section()
        {
            Console.WriteLine("length of max traverses length of all permutations for section {0}",_number_Of_Section);
            foreach(Permutation k in _list_of_permutations)
            {
                Console.WriteLine("permutation {0} max length {1}, error number : {2}",k.get_number_of_permutation(),k.get_max_length_of_traverse(),k.get_error_limiting_traverse_length());
                Console.WriteLine("limiting factor:");
                switch (k.get_error_limiting_traverse_length())
                {
                    
                    case 1:
                        Console.WriteLine("acceleration mod goes into intersection");
                        break;
                    case 2:
                        Console.WriteLine("acceleration mod goes outside of permutation");
                        break;
                    case 3:
                        Console.WriteLine("not enougth space to accelerate to exit speed");
                        break;
                    case 4:
                        Console.WriteLine("intersection during acceleration");
                        break;
                    case 5:
                        Console.WriteLine("traverse is longer than permutation itself");
                        break;

                    case -1:
                        Console.WriteLine("brake mod goes into intersection");
                        break;
                    case -2:
                        Console.WriteLine("braking mod goes outside of permutation");
                        break;
                    case -3:
                        Console.WriteLine("no space to brake at the begining of permutation");
                        break;
                    case -4:
                        Console.WriteLine("intersection during braking");
                        break;


                }
            }
        }

        public override void calculate_max_size_of_traverse_for_each_permutation_of_section()
        {
            
            var error = 0;
            int perm_number;
            double max_length_coarse = 0;

            for (int k = 0; k < _list_of_permutations.Count; k++)
            {
                error = 0;
                perm_number = _list_of_permutations[k].get_number_of_permutation();
                for (int i = 1; i < 4000; i++)
                {
                    
                    //Console.WriteLine("perm number {0}",perm_number);
                    //Console.WriteLine(i);
                    error = CheckPermutation_DC(perm_number, ((double)i)/1);
                    if (error != 0)
                    {
                        break;
                    }
                    max_length_coarse = ((double)i)/1;
                }
                _list_of_permutations[k].set_max_length_of_traverse(max_length_coarse);
                _list_of_permutations[k].set_error_limiting_traverse_lenght(error);
            }
        }
        public int CheckPermutation_DC(int permutation_number, double length_of_traverse)
        {
            // mam tutaj problem bo zrobilem zamet i metoda niejest jasna, niespodziewalem sie ze to bedzie tak skomlikowane.
            // powinienem szrobic metody sprawdzajace konkretny rodzaj bledu tak zeby to bylo czytelne a nie tak jak jest teraz ze mam sprawdzanie jednoczesnie kilku rodzajow bledow.
            // ale przedudowanie tej netody zajmie czas, narzie niebede tego robic.
            double acceleration = 3; //[m/s]
            var permutation = _repository.Get_permutation(permutation_number);
            int track;
            int error_code;
            List<double[]> profile;

            // question can i check if lengths are correct in one go (accelerations and braking ?)
            // check have to be done twice. onece for first pod in length and one for last pod.
            restore_city_copy_to_base_state();
            var route_deep_copy = permutation.Get_deep_copy_of_route();
            var traverse_mod = new DC_section_traverse_mod(CityMapCopy);
            


                
            // first check 
            // is length shorter than permutation.  It is needed for situation where there is no step along the way and that is the only limitation.
            // It is the least limiting test of all.
            if(permuation_length_is_larger_than_traverse_length(route_deep_copy, length_of_traverse)==false)
            {
                error_code = 5;
                return error_code;

            }

            // for both braking and accererating mods there have to be check  
            // for intersection (no change of speed is allowed on intersection, at least for now)
            // and for end of section (no change of speed is allowed on connection points between sections)

            //var queries = new RouteQueries(route_deep_copy, CityMapCopy);
            //var steps = queries.List_Of_Route_Steps_BetweenNodes(route_deep_copy);
                                                                                   // tu jest problem bo jesli wszedzie jest ta sama predkosc to niema zadnych krokow
                                                                                   // co powoduje ze modyfikacja niejest w stanie wykryc skrzyzowania po drodze, bo modyfikacja niejest liczona
                                                                                   // trzeba dodac alternatywna metode sprawdzania skrzyzowania po drodze, kiedy steps jest pusta lista
                                                                                   // steps discription
                                                                                   //List[k][0] = Route[0][0];                                       //node 
                                                                                   //List[k][1] = 0;                                                 //odleglosc
                                                                                   //List[k][2] = 0;                                                 //predkosc charakterystyczna dla danego stepu (poczatkowa dla przyspieszenia, koncowa dla hamowania)
                                                                                   //List[k][3] = 1;                                                 //rodzaj stepu (-1 hamowanie ,1 przyspieszenie)



            // check for first pod //max delay at acceleration point and no speed up at braking point. efectively no brake mods, only acceleration mods
            
                (error_code,route_deep_copy) = traverse_mod.add_mod_of_first_pod_in_a_traverse(length_of_traverse, ref route_deep_copy);
                if (error_code != 0) return error_code;

            
            {
                // metoda sprawdzajaca przekroczenie skrzyzowania przez sama dlugosc traversu
                // pytanie czy to wogule jest konieczne bo moze tak moze zostac
                // to tez bydzi ptanie czy tak naprawde blad niejest gdzie indziej
                // np w samyli liczeniu czasu dla synchronizacji

            }
            // if there was no error till that point it means that no error was genareted during modification process of city of map
            // so no intersection in range of modyfication


            // create profile for modified permutation to check for errors //czy tutaj tez jest sprwadzane czy jest przeciecie z skrzyzowaniem ?
            // that is a test, can pod accelerate at the end to final speed
            // that is a test, can pod brake at the begining speed
            var Profile = new FastestProfile(route_deep_copy, CityMapCopy);
            (error_code, profile) = Profile.ProfileBetweenNodes();    
            if (error_code != 0) return error_code;

            //test , are all accelerations free off intersection in profile ?   sadzac po tym jak sie zachowuja wyniki ten test zawodzi i przepuszacza
            error_code = check_for_intersection_during_all_acceleration_points(route_deep_copy, profile);
            if (error_code != 0) return error_code;

            //test , are all accelerations free off intersection in profile ?
            error_code = check_for_intersection_during_all_braking_points(route_deep_copy, profile);
            if (error_code != 0) return error_code;


            // if there was no error till that point it means that there is no profile genereation error after acceleration mods
            // efectively no error created for first pod

            // restoration of city and route state to base
            // needed so only brake mods are applied during next for last pad in traverse
            restore_city_copy_to_base_state();
            route_deep_copy = permutation.Get_deep_copy_of_route();
            traverse_mod = new DC_section_traverse_mod(CityMapCopy);

            // check for last pod //zero delay at acceleration point and max speed up at braking point. effectively no acceleration mods, only brake mods.

            (error_code, route_deep_copy) = traverse_mod.add_mod_of_first_pod_in_a_traverse(length_of_traverse, ref route_deep_copy);
            if (error_code != 0) return error_code;

            // create profile for modified permutation to check for errors //czy tutaj tez jest sprwadzane czy jest przeciecie z skrzyzowaniem ?
            // that is a test, can pod accelerate at the end to final speed
            // that is a test, can pod brake at the begining speed
            Profile = new FastestProfile(route_deep_copy, CityMapCopy);
            (error_code, profile) = Profile.ProfileBetweenNodes();
            if (error_code != 0)
                return error_code;


            //test ,are all accelerations free off intersection in profile
            error_code = check_for_intersection_during_all_acceleration_points(route_deep_copy, profile);
            if (error_code != 0) return error_code;

            //test ,are all accelerations free off intersection in profile
            error_code = check_for_intersection_during_all_braking_points(route_deep_copy, profile);  //tu jest gdzies blad bo prawdopodobnie nastepuje odwolanie do oryginalnego miasta a nie do kopi gdzie sa nowe dodane tracki i noudy
            if (error_code != 0) return error_code;



            // if there was no error till that point it means that traverse with length x defined in argument is ok 
            if (error_code == 0)
                return 0;
            else
                return -10;//error in error generation process :)

            // pytnie brzmi czy te testy powinny byc tam zawsze czy powinnu byc robione tylko raz na samym poczatku i tak naprawde ta metoda powinna zwracac 
            // max dlugosc traversu tak zeby to jeszcze dzialalo , 
            // ok uzyje tej metody do iteracyjnego zprawdzenia jaka jest max dlugosc traversu. to bedzie duzo wolniejsze ale jest robione tylko raz na poczatku sym wiec ok
            // mogl bym napisac metody analityczne sprawdzenia ale to mi zalmie duzo czasu co jest bez sensu, szczegolnie ze nieprzydadza sie nigdzie indziej
            // sprawdze tez dzialanie metod modyfikacji przy okazji
        }
        private int check_for_intersection_during_all_acceleration_points(int [][] permutation_route, List<double[]> profile)
        {
            var acceleration_points = find_all_acceleration_points(profile);
            int error_code = 0;
            if (acceleration_points.Count == 0) //no acceleration points 
            {
                return 0;
            }
            else
            {
                for (int i = 0; i < acceleration_points.Count; i++)
                {
                     error_code = check_if_there_is_intersection_during_acceleration(acceleration_points[i], permutation_route);
                    if(error_code != 0) return error_code;
                }
            }

            if(error_code==0) return 0;

            return -17;//error in error generation 
        }
        private int check_for_intersection_during_all_braking_points(int [][] permutation_route, List<double[]> profile)
        {
            var braking_points = find_all_brake_points(profile);
            int error_code = 0;
            if (braking_points.Count == 0) //no brake points 
            {
                return 0;
            }
            else
            {
                for (int i = 0; i < braking_points.Count; i++)
                {
                    error_code = check_if_there_is_intersection_during_braking(braking_points[i], permutation_route);
                    if (error_code != 0) return error_code;
                }
            }

            if (error_code == 0) return 0;

            return -17;//error in error generation
        }
        private int check_if_there_is_intersection_during_acceleration(acceleration_point acceleration_data, int[][] permutation_route)
        {
            var length = acceleration_data.get_odleglosc();
            var start_noude = acceleration_data.get_poczatkowy_noude();
            var first_track_number = return_first_track_of_acceleration(start_noude, permutation_route);

            var error_code = CheckLengthOfAccelerationForIntersections(first_track_number, length);
            return error_code;
        }
        private int check_if_there_is_intersection_during_braking(brake_point braking_data, int[][] permutation_route)
        {
            var length = braking_data.get_odleglosc();
            var start_noude = braking_data.get_poczatkowy_noude();
            var first_track_number = return_first_track_of_braking(start_noude, permutation_route);

            var error_code = CheckLengthOfBrakingForIntersection(first_track_number, length);
            return error_code;
        }
        private int return_first_track_of_acceleration(int start_node, int[][] permutation_route)
        {
            for (int i = 0; i < permutation_route.GetLength(0); i++)
            {
                if (permutation_route[i][0] == start_node)
                {
                    return permutation_route[i][2];
                }
            }

            return -1; //start node is not present in permutation route (last node of permutation route is not checked)
        }
        private int return_first_track_of_braking(int start_node, int[][] permutation_route)
        {
            for (int i = 0; i < permutation_route.GetLength(0); i++)
            {
                if (permutation_route[i][0] == start_node)
                {
                    return permutation_route[i-1][2];
                }
            }

            return -1; //start node is not present in permutation route (last node of permutation route is not checked)
        }
        private int CheckLengthOfAccelerationForIntersections(int StartTrack, double Length)
        {

            int CurrentTrackNumber = StartTrack;
            int error_code = 0;

            double LengthOfCurrentTrack;
            double RemaningLength = Length;


            double LenghtIntoTrackWhereEndNodeIs;

            while (true)
            {
                LengthOfCurrentTrack = GetLengthOfTrackFromCityCopy(CurrentTrackNumber);

                if (RemaningLength < LengthOfCurrentTrack)
                {
                    //(FinalTrackNumber, LenghtIntoTrackWhereEndNodeIs) = PrepareDataAfterAffectedTrackIsFound(CurrentTrackNumber, RemaningLength);
                    return error_code;
                }
                else
                {
                    (CurrentTrackNumber, RemaningLength, error_code) = GoToNextTrack_AlongTraffic(CurrentTrackNumber, RemaningLength, LengthOfCurrentTrack);
                }

                if (error_code != 0)
                {
                    return error_code; // in case of error, procedure is stopped  (error =1 cross road in range of search )
                }
            }

        }
        public int CheckLengthOfBrakingForIntersection(int StartTrack, double Length)
        {

            int CurrentTrackNumber = StartTrack;
            int error_code = 0;

            double LengthOfCurrentTrack;
            double RemaningLength = Length;

            double LenghtIntoTrackWhereEndNodeIs;

            while (true)
            {
                LengthOfCurrentTrack = GetLengthOfTrackFromCityCopy(CurrentTrackNumber);

                if (RemaningLength < LengthOfCurrentTrack)
                {
                    return error_code;
                }
                else
                {
                    (CurrentTrackNumber, RemaningLength, error_code) = GoToNextTrack_WrongWay(CurrentTrackNumber, RemaningLength, LengthOfCurrentTrack);
                }
                if (error_code != 0)
                {
                    return error_code; // if there is an error, procedure is stoped (error = -1 , cross road in range of search)
                }

            }

        }
        private double GetLengthOfTrackFromCityCopy(int Number)
        {
            return CityMapCopy.GetLenghtOfTrack(Number);
        }
        private (int track_number, double remaning_length, int error) GoToNextTrack_AlongTraffic(int current_track_number, double remaning_length, double lenght_of_current_track)
        {
            //tu chce miec sprawdzenie czy to niejest skrzyzowanie, jesli tak to wyrzucam blad

            var (next_track, error) = CityMapCopy.get_number_of_next_track_along_traffic_direction(current_track_number);

            return (next_track, remaning_length - lenght_of_current_track, error);
        }
        private (int track_number, double remaning_length, int error) GoToNextTrack_WrongWay(int current_track_number, double remaning_length, double lenght_of_current_track)
        {
            var (next_track, error) = CityMapCopy.get_number_of_next_track_the_wrong_way(current_track_number);

            return (next_track, remaning_length - lenght_of_current_track, error);

        }
        // to wyglada jak jakas kaszana , mocno nawiazujaca do base of profile
        // powinienem to zastapic
        private List<acceleration_point> find_all_acceleration_points(List<double[]> profile)
        {
            var acceleration_points = new List<acceleration_point>();
            double odleglosc = 0;
            int noude_poczatkowy = 0;
            for (int i = 0; i < profile.Count; i++)
            {
                if (profile[i][4] == 1)// oznczenie przyspieszenia
                {
                    odleglosc = profile[i][1] - profile[i][0];
                    noude_poczatkowy = (int)profile[i][5];
                    acceleration_points.Add(new acceleration_point(odleglosc, noude_poczatkowy));
                }
            }

            return acceleration_points;

        }
        
        private struct acceleration_point
        {
            private readonly double odleglosc;
            private readonly int noude_Poczatkowy;

            public acceleration_point(double odleglosc, int noude_poczatkowy)
            {
                this.odleglosc = odleglosc;
                noude_Poczatkowy = noude_poczatkowy;
            }
            public double get_odleglosc()
            {
                return odleglosc;
            }
            public int get_poczatkowy_noude()
            {
                return noude_Poczatkowy;
            }
        }
        private List<brake_point> find_all_brake_points(List<double[]> profile)
        {
            var brake_points = new List<brake_point>();
            double odleglosc = 0;
            int noude_poczatkowy = 0;
            for (int i = 0; i < profile.Count; i++)
            {
                if (profile[i][4] == -1)// oznczenie hamowania
                {
                    odleglosc = profile[i][1] - profile[i][0];
                    noude_poczatkowy = (int)profile[i][6];
                    brake_points.Add(new brake_point(odleglosc, noude_poczatkowy));
                }
            }

            return brake_points;

        }
        private struct brake_point
        {
            private readonly double odleglosc;
            private readonly int noude_Poczatkowy;

            public brake_point(double odleglosc, int noude_poczatkowy)
            {
                this.odleglosc = odleglosc;
                noude_Poczatkowy = noude_poczatkowy;
            }

            public double get_odleglosc()
            {
                return odleglosc;
            }

            public int get_poczatkowy_noude()
            {
                return noude_Poczatkowy;
            }
        } 

        //to jest jeden z testow 
        private bool permuation_length_is_larger_than_traverse_length(int[][] route, double length_of_traverse)
        {
            double total_length_of_permutation = 0;
            for (int i = 0; i < route.GetLength(0); i++)
            {
                total_length_of_permutation += CityMap.GetLenghtOfTrack(route[i][2]);
            }

            if(total_length_of_permutation > length_of_traverse)
                return true;
            else 
                return false;
        }
       
        private void restore_city_copy_to_base_state()
        {
            CityMapCopy = CityMap.DeepCopyCity();
        }

        private int[][] create_matrix_of_permutations(int n)
        {
            int[][] result = new int[factorial(n)][];
            int[] matrix_of_change_frequency = create_matrix_of_frequency(n);
            int[] higher_matrix_of_frequency = create_higher_matrix_of_frequency(n);
            Console.WriteLine("matrix change");
            foreach (int m in matrix_of_change_frequency)
            {
                Console.WriteLine(m);
            }
            List<int> lista = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            lista.Reverse();
            //= Math.DivRem(matrix_of_change_frequency[7], 123, out result);

            Console.WriteLine("permutacje:");

            for (int i = 0; i < factorial(n); i++)
            {
                var new_permutation_list = create_permutation_list(i, lista, matrix_of_change_frequency, higher_matrix_of_frequency);
                result[i] = List_to_matrix(new_permutation_list);

            }
            //foreach (int[] m in result)
            //{
            //    foreach(int k in m)
            //        Console.Write("{0} ", k);
            //    Console.WriteLine("");
            //}
           
            return result;

        }
        private int[] List_to_matrix(List<int> lista)
        {
            int[] maciez = new int[lista.Count];

            for (int i = 0; i < lista.Count; i++)
            {
                maciez[i] = lista[i];
            }
            return maciez;
        }
        private List<int> create_permutation_list(int number, List<int> lista_bazowa, int[] frequency_matrix, int[] higher_frequency_matrix)
        {
            int result;
            int index;
            List<int> new_list = new List<int>();
            for (int j = 0; j < lista_bazowa.Count; j++)
            {
                if (frequency_matrix[j] == 0)
                    index = 0;
                else
                {
                    index = Math.DivRem(number, higher_frequency_matrix[j], out result);
                    index = Math.DivRem(result, frequency_matrix[j], out result);
                }
                new_list = insert_int_at_index(new_list, lista_bazowa[j], index);
            }
            return new_list;
        }
        private List<int> insert_int_at_index(List<int> list, int integer, int index)
        {
            if (list.Count == 0)
            {
                list.Add(integer);
            }
            else
            {
                list.Insert(index, integer);
            }
            return list;
        }
        private int[] create_matrix_of_frequency(int n)
        {
            int[] matrix_of_change_frequency = new int[n];
            for (int i = 0; i < n; i++)
            {
                matrix_of_change_frequency[i] = factorial(i);
            }
            return matrix_of_change_frequency;
        }
        private int[] create_higher_matrix_of_frequency(int n)
        {
            int[] matrix_of_change_frequency = new int[n];
            for (int i = 0; i < n; i++)
            {
                matrix_of_change_frequency[i] = factorial(i + 1);
            }
            return matrix_of_change_frequency;
        }
        private int factorial(int n)
        {
            int wynik = 1;
            if (n == 0)
                return 0;
            else if (n == 1)
                return 1;
            else
            {

                for (int i = 1; i <= n; i++)
                {
                    wynik = wynik * i;
                }
                return wynik;
            }

        }

        //private void perform_check_for_all_posible_combinations(int[][] matrix_combinations, List<Permutation> permutations ,CityDataStorage cityData,List<int> list_of_I_nodes, List<int> list_of_O_nodes)
        //{
        //    bool wynik_testu;
        //    int podliczenie_pozytywne=0;
        //    int podliczenie_negatywne=0;

        //    var repository_of_negative_results = new List<int>();
        //    for (int i=3000000; i<3002000; i++)
        //    {
        //        var new_list_of_permutations = get_list_of_permutations_from_one_of_matrix_positions(i, matrix_combinations, permutations);
        //        var temp_list_of_permutations = new List<Permutation>();
        //        for (int j = 0; j < new_list_of_permutations.Count; j++)
        //        {
        //            temp_list_of_permutations.Add(new_list_of_permutations[j].get_deep_copy_without_schematic_components());
        //        }

        //        var section_config = new Section_config_DC(cityData, list_of_I_nodes, list_of_O_nodes, temp_list_of_permutations, _list_of_tracks_in_section);
        //        wynik_testu = section_config.calculate_config(temp_list_of_permutations, false);

        //        if (wynik_testu)
        //            podliczenie_pozytywne += 1;
        //        else
        //        {
        //            podliczenie_negatywne += 1;
        //            repository_of_negative_results.Add(i);
        //        }
        //        if (Math.IEEERemainder(i, 10) == 0) 
        //            Console.WriteLine("liczba testow: {0}",i);
        //    }

        //    Console.WriteLine("wynik testu");
        //    Console.WriteLine("podliczenie pozytywne: {0}", podliczenie_pozytywne);
        //    Console.WriteLine("podliczenie negatywne: {0}", podliczenie_negatywne);

        //    for (int k = 0; k < repository_of_negative_results.Count; k++)
        //    {
        //        int i = repository_of_negative_results[k];
        //        Console.WriteLine("test o numerze: {0}",i);
        //        var new_list_of_permutations = get_list_of_permutations_from_one_of_matrix_positions(i, matrix_combinations, permutations);
        //        foreach(Permutation n in new_list_of_permutations)
        //        {
        //            Console.WriteLine(n.get_number_of_permutation());

        //        }
        //        //var new_list_of_permutations = get_list_of_permutations_from_one_of_matrix_positions(i, matrix_combinations, permutations);
        //        //var temp_list_of_permutations = new List<Permutation>();
        //        //for (int j = 0; j < new_list_of_permutations.Count; j++)
        //        //{
        //        //    temp_list_of_permutations.Add(new_list_of_permutations[j].get_deep_copy_without_schematic_components());
        //        //}

        //        //var section_config = new SectionConfig(cityData, list_of_I_nodes, list_of_O_nodes, temp_list_of_permutations, _list_of_tracks_in_section);

        //        //wynik_testu = section_config.calculate_config(temp_list_of_permutations, true);
        //    }

        //}

        private List<Permutation> get_list_of_permutations_from_one_of_matrix_positions(int i,int[][] matrix,List<Permutation> permutations)
        {
            List<Permutation> list_of_permutations = new List<Permutation>();
            int[] combination = matrix[i];

            for(int k = 0; k < combination.Length; k++)
            {
                list_of_permutations.Add(get_perm_from_list_by_number(permutations,combination[k]));
            }
            return list_of_permutations;
        }
        private Permutation get_perm_from_list_by_number(List<Permutation> permutations, int number)
        {
            for (int i = 0; i < permutations.Count; i++)
            {
                if (permutations[i].get_number_of_permutation() == number)
                {
                    return permutations[i];
                }
            }
            return null;
        }
    }
    
}
