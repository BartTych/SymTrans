using System.Collections.Generic;
using System;
namespace Symulation
{
    // section config to opis w czasie
    // na wejsciach i wyjsciach 
    // dla wszystkich permutacji
    // plus dlugosc okresu danej permutacji
    public class Section_config_DC
    {
        private readonly CityDataStorage city;
        private CityDataStorage CityMapCopy;
        private readonly List<int> list_of_I_nodes;
        private readonly List<int> list_of_O_nodes;
        private List<Permutation> permutations;
        private readonly List<Permutation> priority_Perm;
        private readonly List<Permutation> start_Perm;
        private readonly List<Permutation> end_Perm;
        private List<Intersection> list_of_intersections;
        private List<Entrance> list_of_entrances;
        private List<Exit> list_of_exits;
        private List<int> list_of_tracks;
        private double period;

        public Section_config_DC(CityDataStorage city, List<int> list_of_I_nodes, List<int> list_of_O_nodes, List<Permutation> permutations,List<Permutation> priority_perm, List<Permutation> start_perm, List<Permutation> end_perm, List<int> list_of_tracks,double DC_length)
        {
            list_of_intersections = new List<Intersection>();
            list_of_entrances = new List<Entrance>();
            list_of_exits = new List<Exit>();
            this.list_of_tracks = list_of_tracks;
            this.city = city.DeepCopyCity();
            CityMapCopy = city.DeepCopyCity();
            this.list_of_I_nodes = list_of_I_nodes;
            this.list_of_O_nodes = list_of_O_nodes;
            this.permutations = permutations;
            priority_Perm = priority_perm;
            start_Perm = start_perm;
            end_Perm = end_perm;

            define_components_of_section_schematics(permutations);
            define_traverses_length(permutations, DC_length);
            
            //create matrix of permutations
            
            //perform config for all permutations
                //translation of matrix to list
                //var(list_of_pairs, list_of_secondary_pairs) = calculate_config(permutations);
            //list = list_of_pairs;
            //secondary_list = list_of_secondary_pairs;
            //calculate time
            //add missing empty spots
        }

        public double get_period()
        {
            return period;
        }

        public List<double[]> get_config_for_exit(int node_number)
        {
            var exit = get_exit_attached_to_node(node_number);

            return exit.get_config();
        }

        public List<double[]> get_config_for_entrance(int node_number)
        {
            var entrance = get_entrance_attached_to_node(node_number);
            return entrance.get_config();

        }

        public bool calculate_config(List<Permutation> permutations, bool print, int section_number)
        {
            double least_common_speed;
            double lost_time;
            double time_distance_from_acceleration;
            double separation;
            
            Exit exit;
            pair_of_permutations pair;
            List<pair_of_permutations> list_of_pairs = null;
            List<pair_of_permutations> list_of_secondary_pairs = null;

            //calculate time at exit for each permutation
            for(int i = 0; i < permutations.Count; i++)
            {
                calculate_speed_at_exit(permutations[i]);
                calculate_speed_at_entrance(permutations[i]);
                permutations[i].calculate_time_of_travel_thru_exit();
            }

            // dla kazdego exit mam liste permutacji z ich dlugosciami traverse 
            // sortuje ta liste wegdlug dlugosci traverse (najdluzszy traverse na koncu)
            // tu bedzie metoda ustawiania kolejnosci
            // narazie sa po kolei
            // teraz dodam tutaj metode ustawiania kolejnosci 
            // tak zeby start byl przed permutacja przejazdowa
            // zeby moc to zrobic potrzebne jest mi info co jest czym 
            // 
            // i tak zeby stop byl przez permutacja przejazdowa (ale to bedzie zalezec od korekcji ustawienia w synchronizacji na D) 

            // jeszcze metoda korygujaca d
            
            for (int i = 0; i < list_of_exits.Count; i++)
            {
                exit = list_of_exits[i];

                move_start_per_in_front_of_priority_if_both_present(ref exit);
                


                //place start 

                // teraz niema zadnego sortowania wiec jest zachowana kolejnosc po numeracji permutacji
                // wystarczy ze tutaj dadam metode ktora sprawdzi czy w danym exit jest perm przejazdowa i perm start
                // jesli sa to umieszcze perm start przed perm przejazdowa 
                // oznaczenia sa gotowe, zostalo napisanie metody sortujacej
                // sort:
                // sprawdzam czy jest start i priority w tej sekcji (jesli tak to jada dalej)
                // jesli w liscie permutacji exit sa jednoczesnie priority i start 
                // to przemieszczam start przed priority

                // exit.sort_permutations_by_current_length_of_traverse();  //najdluzszy traverse na koncu
                // testowo to wylaczam zeby zobaczyc czy moja metoda dziala tez ja pierwszy jedzie trawers krotszy
                // Console.WriteLine("exit attached to node number: {0}", exit.get_number_of_attached_noude());
                // n.print_permutations_and_lengths();

                // jesli liczba permutacji jest rowna 1
                // definiuje poprostu traverse na wyjsciu
                // do zrobienia ale jak ustale format ustawienia na wyjsciu

                // jesli liczba permutacji jest wiksza od 1 
                if (exit.get_list_of_permutations().Count > 1)
                {// to powinna byc nowa metoda
                    list_of_pairs = create_list_of_pairs(exit.get_list_of_permutations());
                    calculate_least_common_speed_for_list_of_pairs(list_of_pairs);
                    calculate_exit_speed_for_list_of_pairs(list_of_pairs);
                    
                    for (int j = 0; j < list_of_pairs.Count; j++)
                    {
                        pair = list_of_pairs[j];
                        //licze dystans od przyspieszania na koncu wynikacajy z least common speed 
                        //licze czas stracony przez kazda pare przez wlaczanie sie do ruchu perwszej permutacji
                        separation = calculate_required_separation_between_permutations_at_the_end_of_common_route(list_of_pairs[j]);
                        pair.required_separation_at_the_end = separation;
                    }
                }
                // jesli liczba permutacji jest wieksza od 2
                if (exit.get_list_of_permutations().Count > 2)
                {//to powinna byc nowa metoda
                    list_of_secondary_pairs = create_list_of_secondary_pairs(exit.get_list_of_permutations());
                    calculate_least_common_speed_for_list_of_pairs(list_of_secondary_pairs);
                    calculate_exit_speed_for_list_of_pairs(list_of_secondary_pairs);

                    for (int j = 0; j < list_of_secondary_pairs.Count; j++)
                    {
                        pair = list_of_secondary_pairs[j];
                        separation = calculate_required_separation_between_permutations_at_the_end_of_common_route(list_of_secondary_pairs[j]);
                        pair.required_separation_at_the_end = separation;
                    }
                }

                // sprawdzam wszystkie pary dodatkowe 
                // co oznacza ze zwiekszam odsepy w parach podstawowych jesli to konieczne
                if (exit.get_list_of_permutations().Count > 2)
                    check_all_secondary_pairs(list_of_secondary_pairs, list_of_pairs);

                // dodatkowo i jesli to jest stop to sprawdzam czy odstep pomiadzy parami jest wiekszy niz wymagany
                // jesli nie, to go zwiekszam
                // to sprawdzenie dziala dla exit ale nie dla start co jest problemem , 
                // jak dodac , zwiekszenie odleglosci pomiedzy startami ?
                // to wyglada jak wydluzenie okresu calej sekcji zeby zmienic odleglosc pomiedzy traversami na wejsciu
                //is_exit_a_station()
                bool exit_a_station = is_exit_a_station(exit);
                if (exit.get_list_of_permutations().Count > 1 && exit_a_station)
                {
                    check_all_pairs_for_min_required_arrival_separation(list_of_pairs, exit);
                  
                }



                if (exit.get_list_of_permutations().Count > 1)
                    exit.add_list_of_primary_pairs(list_of_pairs);
                if (exit.get_list_of_permutations().Count > 2)
                    exit.add_list_of_secondary_pairs(list_of_secondary_pairs);

                
                //if (list_of_pairs != null)
                //{
                //    Console.WriteLine("pary permutacji :");
                //    foreach (pair_of_permutations m in list_of_pairs)
                //    {
                //        Console.WriteLine("para {0}_{1}:", m.first_permutation.Get_start_noude(), m.second_permutation.Get_start_noude());
                //        Console.WriteLine("separation: {0}", m.required_separation_at_the_end);
                //    }
                //}
                //if (list_of_secondary_pairs != null)
                //{
                //    Console.WriteLine("drugorzedne pary permutacji :");
                //    foreach (pair_of_permutations m in list_of_secondary_pairs)
                //    {
                //        Console.WriteLine("para {0}_{1}:", m.first_permutation.Get_start_noude(), m.second_permutation.Get_start_noude());
                //        Console.WriteLine("separation: {0}", m.required_separation_at_the_end);
                //    }

                //}
            }


            //config for exits
            for (int i = 0; i < list_of_exits.Count; i++)
            {
                extract_config_for_exit_from_list_of_pairs(list_of_exits[i]);
            }


            if (is_there_D_intersection_in_section())
            {
                // tu jest jakis blad dla sekcji jak robie 
                // rozgalezienie ktore konczy sie stopem, to one niesa zsynchronizowane prawidlowo
                // z przejazdami na innym kierunku.
                // musze to zdebugowac zeby moc zwiekszyc wydajnosc 

                // pytanie kiedy moge zmieniac kolejnosc, w wersji z start, stop plus (priority), ktora mam teraz
                // przy obecnym algorytmie , zmiana teraz jest ok bo wplywa to tylko na komponenty "upstream" 


                // sprawdzam kolejnosc
                // w konfiguracji D intersection 
                // sprawdzenie jest ok, ale wymaga zmian w 
                // b
                var inter = get_D_intersection();
                move_end_per_in_front_of_priority_if_both_present(ref inter);

                // tu jest burdel i sam tego nierozumiem 
                // a z logicznego punktu widzenia to banal jest 

                // w tym momencie mam zrobione config dla obu exit
                // sa one policzone totalnie niezaleznie wiec teraz moge dowonle przesuwac
                // czas startu odu exit tak zeby spotkaly sie na skrzyzowanie D tak jak chce

                //to powinno byc w srodku metody synchronizacji


                //pytanie jaka jest konwencja oznaczen kolejnosci w parach, niewiem co jest porabane i trudne do ogarniecia 
                //konwencja listy perm na wejsci jest tereaz taka ze ostatnia perm na liscie jedzie pierwsza 
                //powoduje to tez jakis blad z konwencja par 



                synchronize_exits();

            }


            calculate_config_for_every_entrance_and_intersection(permutations);

            period = calculate_section_period();

            //tu brakuje zmiany dlugosci traverse w config dla punktow sekcji
            //bez tego to wprowadza w blad
            if (priority_Perm.Count > 0)
                elongate_priority_permutation_config();


            // przesuniecie w czasie calosci config tak zeby zero bylo w najnizszym punkcie
            // dla wszystkich objectow przechowujacych config
            move_config_to_have_earliest_point_at_zero();



            //Console.WriteLine(" ");
            //Console.WriteLine("Section number: {0}", section_number);
            //Console.WriteLine(" ");
            //if (print)
            //    print_all_schematic_points_config(permutations);
            //Console.WriteLine("okres sekcji: {0}", period);

            var wynik_testu = check_all_schematic_components(permutations,print);
            return wynik_testu;
            
            


            // zrobie tutaj serie sanity check 
                // czy kolejnosc sie zgadza permutacji na kolejnych objektach 
                    // kopiuje liste permutacji z sekcji 
                        //robie liste wszystkich mozliwych kombinacji par po sobie [jako int]
                    // i dla kazdego obiektu sprawdzam po kolei czy dana para z config jest w liscie mozliwych par kombinacji 
                // czy czas jest rosnacy dla kazdej kolejnej permutacji na kazdym obiekcie schematu
                    //ide po kolei po config i sprawdzam czy odstep jest >=0
                // czy czas jest rosnacy dla kazdego config we wszystkich obiektach
                    //sprawdzam czy czas konca jest wiekszy niz czas poczatku 
                    //dla kazdego elementu config
                // czy odlegolsci czasowe sa zero dla najwolniejszego tracku sekcji
                // czy odlegolosci czasowe sa zero dla divergence ?
                    //czy to jest objekt divergence lub node najwolniejszego tracku w sekcji
                            //czy odleglosc w czasie jest 0 dla kolejnych komponentach config

                // ide po wszystkich objektach schematu
                    //puszczam zestaw testow

            // powinienem zrobic jakis test tej metody zeby ja przeczolgac mega z tysiacami konfiguracji i zobaczyc czy to dziala
            // i spelnia wszystkie sanity check
            //parametry na wejsciu
                //kolejnosc permutacji
                //wielkosc kazdej permutacji


            // calculate config for enters
                // implementacja
                // ide po kolei przez wszystkie permutacje sekcji
                    // licze czas przejazdu dla aktualnej dlugosci transferu
                    // odczytuje start i end time dla exit danej permutacji
                    // odpalam enter gdzie zaczyna swoja radosna przygode dana permutacja 
                        // wrzucam config
                            //sprawdzam czy jest wiecej niz jedna perm juz zdefiniowana jesli tak
                                //sortuje konfig wedlug start time :0 i gotowe 

            // chce tutaj dodac metoda ktora policzy config dla wszystkich skzyzowan co pozwoli mi zdebugowac dzialanie metody
                //
                // ide po kolei przez wszystkie permutacje sekcji
                    //licze profil przejazdu dla danej permutacji i aktualnej dlugosci transferu przez cala sekcje
                        //dla kazdego intersection po drodze
                            //dodaje config bazujac na profilu i odleglosci do skzyzowania i czasie start i end z config dla danej permutacji
                                

            // definicja powinna byc jakims zbiorem dla calej sekcji gdzie sa definicje kazdego wejscia i wyjscia
            // to tez troche zalezy od algorytmu ktory pozniej bedzie z tego kozystac i wyznaczac czasy przejazdu dalej.

            // definicja na kazdym wejsciu wyjsciu
            // wrzucam te definicje do obiektow exit i enter 

            // node, exit
            // permutacja, dlugosc, czas start, czas koniec
            // 0, 2.3 , 3
            // 4.5, 6.7, 2

            // najpierw licze exit (2 jesli sa)
            // synchronizuje je wzgledem siebie jesli sa 2

            // przeliczam to na czasy wjazdu
            // wrzucam to wszystko do jednej 
        }

        private void elongate_priority_permutation_config()
        {

            Exit exit = get_exit_for_permutation(priority_Perm[0]);
            Entrance entrance = get_entrance_for_permutation(priority_Perm[0]);

            int number_of_perm = priority_Perm[0].get_number_of_permutation();

            double span_of_entrance_config = calculate_span_of_config(entrance.get_config());
            double span_of_exit_config = calculate_span_of_config(exit.get_config());

            double current_length_of_priority = priority_Perm[0].get_current_length_of_traverse();
            double max_length = priority_Perm[0].get_max_length_of_traverse();

            double entrance_speed = priority_Perm[0].get_entrance_speed();
            double exit_speed = priority_Perm[0].get_exit_speed();

            double max_enlargement_at_start_time = (period - span_of_entrance_config) * priority_Perm[0].get_entrance_speed();
            double max_enlargment_at_end_time = (period - span_of_exit_config) * priority_Perm[0].get_exit_speed();
            double max_enlargment_by_size_limits = max_length - current_length_of_priority;

            List<double> sort_list = new List<double>();
            sort_list.Add(max_enlargement_at_start_time);
            sort_list.Add(max_enlargment_at_end_time);
            sort_list.Add(max_enlargment_by_size_limits);
            sort_list.Sort();

            double enlargment = sort_list[0];

            // tu jest zmiana ale najwyrazniej niejset to ta sama zmienna referencyjna
            
            priority_Perm[0].set_current_length_of_traverse(current_length_of_priority + enlargment);
            //change_permutation_traverse_lenght(number_of_perm, current_length_of_priority + enlargment);

            double star_time_shift = enlargment / entrance_speed;
            double end_time_shift = enlargment / exit_speed;

            // poszerzenie w czasie permutacji prioretytowej 
            // licze szerokosc w czasie dla config entrance , config exit
            // licze ile jeszcze zostalo przestrzeni na wydluzenie ne wejsciu
            // licze ile jeszcze zostalo przestrzeni na wydluzenie na wyjsciu
            // sprwadzam jakia jest max dlugosc tej permutacji
            // wydluzam o tyle ile sie da . ograniczeniem jest albo, miejsce w start, miejsce w exit albo max dlugosc w permutacji
            // przesuwam czas konca slotu priority na starcie o wyliczony shift
            // przesuwam czas konca slotu priority na koncu o wyliczony shift
            // troche niebazpieczne jest to ze config wpisany w intersections (to jest jakas pozostalosc po procesie tworzenia config) jest niezmieniony
            // ale to niema znaczenia narazie


            var config =  exit.get_config();

            config[1][3] += star_time_shift;

            config = entrance.get_config();

            config[1][3] += end_time_shift;

        }

        private void change_permutation_traverse_lenght(int perm_number, double length)
        {
            for(int i = 0; i < permutations.Count; i++)
            {
                if (permutations[i].get_number_of_permutation() == perm_number)
                    permutations[i].set_current_length_of_traverse(length);
            }

        }

        private Entrance get_entrance_for_permutation(Permutation permutation)
        {

            for (int i = 0; i < list_of_entrances.Count; i++)
            {
                if (list_of_entrances[i].is_permutation_on_list(permutation))
                {
                    return list_of_entrances[i];
                }
            }
            return null;
        }

        private double calculate_span_of_config(List<double []> config)
        {
            int numer_of_segments = config.Count;
            return config[numer_of_segments - 1][3] - config[0][2];

        }

        private void move_start_per_in_front_of_priority_if_both_present( ref Exit exit)
        {
            if(start_Perm.Count > 0 && priority_Perm.Count > 0)
            {
                var exit_perm = exit.get_list_of_permutations();

                //narazie moze byc tylko jeden start i jedna sekcja priority
                if(exit_perm.Contains(start_Perm[0]) && exit_perm.Contains(priority_Perm[0]))
                {
                    //umieszczam start przed priority
                    int index_of_priority = exit_perm.IndexOf(priority_Perm[0]);
                    int index_of_start = exit_perm.IndexOf(start_Perm[0]);
                    if (index_of_start < index_of_priority)
                        exit_perm.Reverse();
                }
            }
        }

        private void move_end_per_in_front_of_priority_if_both_present(ref Intersection intersection)
        {
            if (end_Perm.Count > 0 && priority_Perm.Count > 0)
            {
                var inter_perm = intersection.get_list_of_permutations();

                //narazie moze byc tylko jeden start i jedna sekcja priority
                if (inter_perm.Contains(end_Perm[0]) && inter_perm.Contains(priority_Perm[0]))
                {
                    //umieszczam start przed priority
                    int index_of_priority = inter_perm.IndexOf(priority_Perm[0]);
                    int index_of_end = inter_perm.IndexOf(end_Perm[0]);
                    if (index_of_end < index_of_priority)
                        inter_perm.Reverse();
                }


            }
        }

        private void check_all_pairs_for_min_required_arrival_separation(List<pair_of_permutations> pairs, Exit exit)
        {
            double min_separation_at_arrival = city.get_station_arrival_separation_by_node(exit.get_attached_node_number());
            double difference;

            //min departure separation for station connected to exit
            for(int i = 0; i < pairs.Count; i++)
            {
                if(pairs[i].required_separation_at_the_end < min_separation_at_arrival)
                {
                    difference = min_separation_at_arrival - pairs[i].required_separation_at_the_end;
                    pairs[i].additional_separation_because_of_station_min_arrival_separation = difference;
                    pairs[i].required_separation_at_the_end = min_separation_at_arrival;
                }
            }
        }

        private bool is_exit_a_station(Exit exit)
        {
            return city.is_node_a_station_node(exit.get_attached_node_number());
        }

        private double calculate_section_period()
        {
            //licze rozpietosc czasowa wszystkich wejsc i wyjsc 
            //liczba max jest okresem jalej sekcji
            double period=0;

            for(int i = 0; i < list_of_entrances.Count; i++)
            {
                if (calculate_time_span_of_Schematic_component_config(list_of_entrances[i]) > period)
                {
                    period = calculate_time_span_of_Schematic_component_config(list_of_entrances[i]);
                }
            }

            for(int i = 0; i < list_of_exits.Count; i++)
            {
                if(calculate_time_span_of_Schematic_component_config(list_of_exits[i]) > period)
                {
                    period = calculate_time_span_of_Schematic_component_config(list_of_exits[i]);
                }
            }
            return period + 0.002; //adding 0.002 [s] to make sure there is no overlap
        }

        private double calculate_time_span_of_Schematic_component_config(SchematicComponent component)
        {
            var config = component.get_config();

            return config[config.Count - 1][3] - config[0][2];


        }
        private void move_config_to_have_earliest_point_at_zero()
        {
            //ide po kolei po wszystkich objektach entrance i sposuje czas poczatku pierwszego traersu jadacego przez dany wjazd
            double extreme_time=0;
            List<double[]> temp_config;
            for (int i = 0; i < list_of_entrances.Count; i++)
            {
                if (i == 0)
                {
                    temp_config = list_of_entrances[i].get_config();
                    extreme_time = temp_config[0][2];
                    continue;
                }
                temp_config = list_of_entrances[i].get_config();
                if (temp_config[0][2] < extreme_time)
                {
                    extreme_time = temp_config[0][2];
                }

            }


            //shift config in all schematic components 
            for(int i = 0; i < list_of_entrances.Count; i++)
            {
                list_of_entrances[i].shift_config_time(-extreme_time);

            }

            for(int i=0; i< list_of_exits.Count; i++)
            {
                list_of_exits[i].shift_config_time(-extreme_time);
            }

            for(int i=0; i < list_of_intersections.Count; i++)
            {
                list_of_intersections[i].shift_config_time(-extreme_time);
            }

        }
        private bool check_all_schematic_components(List<Permutation> permutations, bool print)
        {
            bool wynik;
            foreach(Intersection n in list_of_intersections)
            {
                //Console.WriteLine("node: {0}",n.get_number_of_attached_noude());
                wynik = sanity_check(permutations, n);
                if(!wynik)
                    return false;
            }

            foreach (Exit n in list_of_exits)
            {
                //Console.WriteLine("node: {0}", n.get_number_of_attached_noude());
                wynik = sanity_check(permutations, n);
                if (!wynik)
                    return false;
            }
            foreach (Entrance n in list_of_entrances)
            {
                //Console.WriteLine("node: {0}", n.get_number_of_attached_noude());
                wynik = sanity_check(permutations, n);
                if (!wynik)
                    return false;

            }
            return true;
        }

        private bool sanity_check(List<Permutation> permutations, SchematicComponent component)
        {
            var node_number = component.get_number_of_attached_noude();
            var (D_node, is_there_D_in_section) = return_D_info();
            var list_of_slowest_nodes = get_list_of_nodes_for_slowest_section(list_of_tracks);
            var list_of_valid_pairs = prepare_list_of_all_valid_pairs_of_permutations(permutations);
            var config = component.get_config();

            var result = perform_all_checks(config, node_number, D_node, is_there_D_in_section, list_of_slowest_nodes, list_of_valid_pairs);
            //Console.WriteLine("test result: {0}", result);
            return result;
        }

        private bool perform_all_checks(List<double[]> config, int node,int D_node, bool is_there_D, List<int> list_of_slow_nodes, List<(int, int)> valid_pairs)
        {
            //zmieniam kolejnosc a ten fragment sprawdzal czy konejnosc jest zgodna z numeracja permutacji
            //var bool_1 = check_if_all_permutations_are_in_correct_order(config, valid_pairs);
            var bool_1 = true;
            var bool_2 = check_if_time_goes_up_between_all_sections_of_config(config);
            var bool_3 = check_if_time_goes_up_for_each_section_of_config(config);
            //var bool_4 = check_if_time_difference_is_zero_for_slowest_nodes_schematic_comp(list_of_slow_nodes, node, config);//conot be used bacause it is not allways true
            //var bool_5 = check_if_zero_time_difference_for_D(node, is_there_D, D_node, config);//conot be used bacause it is not allways true

            if (bool_1 && bool_2 && bool_3)
            {
                return true;
            }
            else
                return false;
        }


        private bool check_if_all_permutations_are_in_correct_order(List<double[]> config, List<(int,int)> valid_pairs)
        {
            (int, int) pair;
            if (config.Count == 1)
            {
                return true;
            }
            else
            {
                for(int i = 0; i < config.Count - 1; i++)
                {
                    pair = ((int)config[i][0], (int)config[i + 1][0]);
                    if (!valid_pairs.Contains(pair))
                    {
                        // ok ale dlaczego istniej jakas prawidlowa kolejnosc
                        // kiedy sa tworzone pary do liczenia config ?
                        Console.WriteLine("incorrect_order");
                        return false;
                    }
                }

            }
            return true;
        }
        private bool check_if_time_goes_up_between_all_sections_of_config(List<double[]> config)
        {
            if (config.Count == 1)
            {
                return true;
            }
            else
            {
                double difference;
                for (int i = 0; i < config.Count - 1; i++)
                {
                    difference =  config[i + 1][2] - config[i][3];
                    if (difference < -0.0000001)
                    {
                        Console.WriteLine("time do not go up between sections of config");
                        return false;
                    }
                }


            }
            return true;
        }
        private bool check_if_time_goes_up_for_each_section_of_config(List<double[]> config)
        {
            for(int i=0;i<config.Count; i++)
            {
                if (config[i][3] < config[i][2])
                {
                    Console.WriteLine("time do not go up in each section of config");
                    return false;
                }
            }

            return true;
        }
        
        private List<(int,int)> prepare_list_of_all_valid_pairs_of_permutations(List<Permutation> permutations)
        {
            int number_of_permutation = permutations.Count;
            
            List<(int, int)> list_of_valid_pairs = new List<(int, int)>();

            for (int i = number_of_permutation-1; i >=0; i--)
            {
                for(int j = i-1; j >= 0; j--)
                {
                    list_of_valid_pairs.Add((permutations[i].get_number_of_permutation(), permutations[j].get_number_of_permutation()));
                }
            }
            return list_of_valid_pairs;
        }

        private (int node,bool is_there_D_in_section) return_D_info()
        {
            var is_there_D = is_there_D_intersection_in_section();
            
            int node = 0;

            if (is_there_D)
            {
                foreach (Intersection n in list_of_intersections)
                {
                    if (n.get_type() == 2)
                    {
                        node = n.get_number_of_attached_noude();   
                    }
                }
            }
            return(node,is_there_D);
        }
        private List<int> get_list_of_nodes_for_slowest_section(List<int> list_of_tracks)
        {
            var slowest = get_slowest_track_speed(list_of_tracks);

            var list_of_slowest_tracks = get_list_of_all_tracks_with_given_speed(list_of_tracks, slowest);

            var list_of_nodes = get_list_of_nodes_attached_to_list_of_slowest_tracks(list_of_slowest_tracks);

            return list_of_nodes;
        }

        private double get_slowest_track_speed(List<int> list_of_tracks)
        {
            double slowest = 0;
            for(int i = 0; i < list_of_tracks.Count; i++)
            {
                if(city.GetSpeedLimitForTrack_m_s(list_of_tracks[i])<slowest || slowest == 0)
                {
                    slowest = city.GetSpeedLimitForTrack_m_s(list_of_tracks[i]);
                }
            }
            return slowest;
        }

        private List<int> get_list_of_all_tracks_with_given_speed(List<int> list_of_tracks, double speed)
        {
            var list = new List<int>();

            for(int i = 0; i < list_of_tracks.Count; i++)
            {
                if (city.GetSpeedLimitForTrack_m_s(list_of_tracks[i]) == speed)
                {
                    list.Add(list_of_tracks[i]);
                }
            }
            return list;
        }

        private List<int> get_list_of_nodes_attached_to_list_of_slowest_tracks(List<int> list_of_tracks)
        {//metoda dziala tak ze wyrzuca tylko noudy skrajne sekcji najwolniejszych plus nody skrzyzowan wewnatrz cale otoczone ta predkoscia

            List<int> lista_noudow = new List<int>();
            List<int> tepm_lista;

            for(int i = 0; i < list_of_tracks.Count; i++)
            {
                tepm_lista = city.get_numbers_of_attached_nodes_for_track(list_of_tracks[i]);

                for(int j = 0; j < tepm_lista.Count; j++)
                {
                    if (lista_noudow.Contains(tepm_lista[j]))
                    {
                        lista_noudow.Remove(tepm_lista[j]);
                    }
                    else
                        lista_noudow.Add(tepm_lista[j]);
                }


            }

            return lista_noudow;
        }

        private void print_all_schematic_points_config(List<Permutation> permutations)
        {
            for(int i = 0; i < list_of_exits.Count; i++)
            {
                Console.WriteLine(" ");

                Console.WriteLine("exit   numer nouda:{0}", list_of_exits[i].get_attached_node_number());
                var config = list_of_exits[i].get_config();
                for(int j=0; j < config.Count; j++)
                {
                    Console.WriteLine("{0}  {1}  {2}  {3}  {4}", config[j][0], config[j][1], config[j][2].ToString("N2"), config[j][3].ToString("N2"), get_start_node_of_permutation((int)config[j][0], permutations));
                }
            }
            
            for (int i = 0; i < list_of_entrances.Count; i++)
            {
                Console.WriteLine(" ");
                Console.WriteLine("entrance   numer nouda:{0}", list_of_entrances[i].get_attached_node());
                var config = list_of_entrances[i].get_config();
                for (int j = 0; j < config.Count; j++)
                {
                    Console.WriteLine("{0}  {1}  {2}  {3}  {4}", config[j][0], config[j][1], config[j][2].ToString("N2"), config[j][3].ToString("N2"), get_start_node_of_permutation((int)config[j][0], permutations));
                }
            }

            for (int i = 0; i < list_of_intersections.Count; i++)
            {
                Console.WriteLine(" ");
                Console.WriteLine("intersection   numer nouda:{0}", list_of_intersections[i].get_number_of_attached_noude());
                var config = list_of_intersections[i].get_config();
                for (int j = 0; j < config.Count; j++)
                {
                    Console.WriteLine("{0}  {1}  {2}  {3}  {4}", config[j][0], config[j][1], config[j][2].ToString("N2"), config[j][3].ToString("N2"), get_start_node_of_permutation((int)config[j][0], permutations));
                }
            }
        }
        private int get_start_node_of_permutation(int permutation, List<Permutation> permutations)
        {
            for (int i = 0; i < permutations.Count; i++)
            {
                if (permutations[i].get_number_of_permutation() == permutation)
                {
                    return permutations[i].Get_start_noude();
                }
            }
            return -1;
        }
        private void calculate_config_for_every_entrance_and_intersection(List<Permutation> permutations)
        {
            
            for (int i = 0; i < permutations.Count; i++)
            {
                // moge to przerobic na dwa sposoby
                // oddzielnie liczyc profil dla pierwszej i ostatniej kapsuly w zaspole 
                
                int error;
                var length = permutations[i].get_current_length_of_traverse();

                var first_pod_route = permutations[i].Get_deep_copy_of_route();
                var last_pod_route = permutations[i].Get_deep_copy_of_route();

                var first_pod_city_copy = city.DeepCopyCity();
                var last_pod_city_copy = city.DeepCopyCity();

                var traverse_first_pod_mod = new DC_section_traverse_mod(first_pod_city_copy);
                var traverse_last_pod_mod = new DC_section_traverse_mod(last_pod_city_copy);


                // mod of first pod robi blad przz wydluzenie przejazdu o dlugosc traversu
                // co w praktyce oznacza ze pewnie przesowa ostatni noude
                traverse_first_pod_mod.add_mod_of_first_pod_in_a_traverse(length, ref first_pod_route);
                traverse_last_pod_mod.add_mod_of_last_pod_in_a_traverse(length, ref last_pod_route);

                var First_permutation_profile_generator = new FastestProfile(first_pod_route, first_pod_city_copy);
                var (error1, first_pod_profile) = First_permutation_profile_generator.ProfileBetweenNodes(3);
                var Last_permutation_profile_generator = new FastestProfile(last_pod_route, last_pod_city_copy);
                var (error2, last_pod_profile) = Last_permutation_profile_generator.ProfileBetweenNodes(3);

                var list_of_schematic_components = permutations[i].get_list_of_schematic_components();


                var first_pod_travel_time = get_time_at_end_of_profile(first_pod_profile);
                var last_pod_travel_time = get_time_at_end_of_profile(last_pod_profile);

                var exit = (Exit)permutations[i].get_last_component_on_list();
                var config_of_permutation_at_exit = exit.get_deep_copy_of_config_for_permutation(permutations[i].get_number_of_permutation());
                //ta metoda do przerobienia tak zeby przesowala config roznie dla startu i konca danego zespolu
                var queries = new RouteQueries(first_pod_route, first_pod_city_copy);
                
                double length_of_route = first_pod_profile[first_pod_profile.Count - 1][1];
                //double length_of_route_last_pod = last_pod_profile[last_pod_profile.Count - 1][1];
                
                
                for (int j = 0; j < list_of_schematic_components.Count-1; j++)
                {
                    var current_component = list_of_schematic_components[j];
                    var node = current_component.get_number_of_attached_noude();
                    var distance_from_start_to_node = queries.Distance_To_Node_On_Route(node);

                    var aa = get_time_at_distance_from_profile(first_pod_profile, length_of_route);
                    var bb = get_time_at_distance_from_profile(first_pod_profile, distance_from_start_to_node);

                    var cc = get_time_at_distance_from_profile(last_pod_profile, length_of_route);
                    var dd = get_time_at_distance_from_profile(last_pod_profile, distance_from_start_to_node);

                    var first_pod_travel_time_from_node_to_end = aa - bb;
                    var last_pod_travel_time_from_node_to_end = cc - dd;

                    var first_time = config_of_permutation_at_exit[2] - first_pod_travel_time_from_node_to_end;
                    var last_time = config_of_permutation_at_exit[3] - last_pod_travel_time_from_node_to_end;

                    current_component.add_config_section(new double[] { permutations[i].get_number_of_permutation(), length, first_time, last_time});
                    current_component.sort_config_if_more_than_one_section();
                }
            }
        }


        private void synchronize_exits()
        {
            double min_numerical_separation = 0.001;

            // na wyjsciu dostaje czas pomiedzy poczatkiem pierwszej permutacji w parze na skrzyzowaniu D
            // a czasem na koncu drugiej permutacji w parze na koncu jej permutacji
            
            // to pozwala przesunac pierwsza permutacje wraz z calym config exit danej permutacji o okreslony czas
            // to oznacza ze kolejnosc jest losowa a tak niepowinno byc 
            // powinienem ustalic kolejnosc na bazie jakiejs zasady ktora ustale


            //brak ustalenia na jakiej odlegodci powinno byc sprawdzenie przesuniecia pomiedzy traversami
            //teraz jest recznie co jest bledem 

            var (synch_time, D_pair) = calculate_synchronization_time_for_D_pair(); //potrzebuje info numery permutacji do synchronizacji, i ktore exit jest pierwsze
            //Console.WriteLine("synch time: {0}", synch_time);


            var delayed_exit = get_exit_for_permutation(D_pair.first_permutation);
            var leading_exit = get_exit_for_permutation(D_pair.second_permutation);

            var end_time_for_delayed_acceleration = calculate_end_time_for_delayed_permutation(synch_time, leading_exit, D_pair.second_permutation.get_number_of_permutation()) - min_numerical_separation;

            //ta metoda jest poprawiona po znalezieniu bledu z roznymi czasami przejazdu w zespole
            delayed_exit.move_config_to_have_end_time_of_permutation_as_given(end_time_for_delayed_acceleration,D_pair.first_permutation.get_number_of_permutation());
        }

        private double calculate_end_time_for_delayed_permutation(double synch_time, Exit exit, int permutation_number)
        {
            double end_time_for_dalayed_permutation;
            var time_of_start_leading_permutation = exit.get_start_time_from_permutation(permutation_number);
            end_time_for_dalayed_permutation = time_of_start_leading_permutation - synch_time;

            return end_time_for_dalayed_permutation;
        }
        private Exit get_exit_for_permutation(Permutation permutation)
        {
            for (int i=0; i < list_of_exits.Count; i++)
            {
                if (list_of_exits[i].is_permutation_on_list(permutation))
                    return list_of_exits[i];
            }
            return null;
        }
        private void extract_config_for_exit_from_list_of_pairs(Exit exit)
        {
            // ta metoda jest ok, uwzglednia roznice czasu przejazdu kapsol w zespole
            // tak naprawde metody liczace skladowe

            double min_numerical_spacing = 0.002;//numerical spacing is added also at d synchronization

            var pairs = exit.get_list_of_primary_pairs();
            if (pairs != null)
            {


                pair_of_permutations pair;
                double time_thru_exit = 0;
                double spacing_time = 0;
                double current_start_time;
                double current_end_time = 0;
                for (int i = pairs.Count - 1; i >= 0; i--)
                {
                    pair = pairs[i];
                    time_thru_exit = pair.second_permutation.get_time_of_travel_thru_exit();
                    if (i == pairs.Count - 1)
                    {
                        current_start_time = 0;
                        current_end_time = time_thru_exit;
                    }
                    else
                    {
                        current_start_time = current_end_time + spacing_time + min_numerical_spacing;
                        current_end_time = current_start_time + time_thru_exit;
                    }

                    exit.add_new_config_line(new double[] { pair.second_permutation.get_number_of_permutation(), pair.second_permutation.get_current_length_of_traverse(), current_start_time, current_end_time });
                    spacing_time = pair.required_separation_at_the_end;


                }

                var last_pair = pairs[0];
                time_thru_exit = last_pair.first_permutation.get_time_of_travel_thru_exit();
                current_start_time = current_end_time + spacing_time;
                current_end_time = current_start_time + time_thru_exit;

                // second permutation of last pair:
                exit.add_new_config_line(new double[] { last_pair.first_permutation.get_number_of_permutation(), last_pair.first_permutation.get_current_length_of_traverse(), current_start_time, current_end_time });
            }
            else
            {
                Permutation permutation = exit.get_list_of_permutations()[0];
                exit.add_new_config_line(new double[] { permutation.get_number_of_permutation(), permutation.get_current_length_of_traverse(), 0, permutation.get_time_of_travel_thru_exit() });
            }
            
        }

        private (double synch_time,pair_of_permutations D_intersection_pair ) calculate_synchronization_time_for_D_pair()
        {
            //dodatni czas synch to taki ktory oznacza ze pierwsza permutacja jest wczesniej niz druga
            var D_intersection_pair = create_pair_for_D_intersection();
            var delay_between_perm_at_intersection_accumulated_from_start_point = D_intersection_pair.required_separation_at_the_end;
            //tutaj dla przejrzystosci mogl bym zrobic oddzielny objekt pair dla D intersection i inna nazwe metody roznicy czasu od wspolnego przyspieszania

            //ta metoda liczy roznice czasu dla ostatniej kapsuly pierwszej permutacji a pierwszej kapsuly drugiej permutacji.
            var time_travel_difference_from_intersection_to_end = calculate_time_travel_difference_from_D_to_end(D_intersection_pair);

            var cumulative_time_difference = time_travel_difference_from_intersection_to_end + delay_between_perm_at_intersection_accumulated_from_start_point;
            return (cumulative_time_difference, D_intersection_pair);
        }

        private double calculate_time_travel_difference_from_D_to_end(pair_of_permutations pair_D)
        {
            int error;
            double distance_of_similar_speed_first_permutation = 0;
            double distence_of_similar_speed_second_permutation = 0;

            var length_of_first_traverse = pair_D.first_permutation.get_current_length_of_traverse();
            var length_of_second_traverse = pair_D.second_permutation.get_current_length_of_traverse();

            var first_city_copy = city.DeepCopyCity();
            var second_city_copy = city.DeepCopyCity();

            var last_common_track_number = calculate_last_common_track_for_D_intersection_permutations(pair_D);

            var traverse_first_pod_mod = new DC_section_traverse_mod(first_city_copy);
            var traverse_second_pod_mod = new DC_section_traverse_mod(second_city_copy);

            int[][] first_route = pair_D.first_permutation.Get_deep_copy_of_route();
            int[][] second_route = pair_D.second_permutation.Get_deep_copy_of_route();

            var distance_from_start_to_intersection_first_permutation = sum_distance_from_route_start_to_track(last_common_track_number, pair_D.first_permutation.Get_route());
            var distence_from_start_to_intersection_second_permutation = sum_distance_from_route_start_to_track(last_common_track_number, pair_D.second_permutation.Get_route());

          

            (error, first_route) = traverse_first_pod_mod.add_mod_of_last_pod_in_a_traverse(length_of_first_traverse, ref first_route);
            (error, second_route) = traverse_second_pod_mod.add_mod_of_first_pod_in_a_traverse(length_of_second_traverse, ref second_route);

            //wprowadzam modyfikacje profilu tak zeby symulacja byla przeprowadzona dla kapsuly jadacej w zespole od dlugosci traversu
            // niema to znaczenia czy to bedzie dla pierwszej kapsuly, czy ostatniej , ale modyfikacje sa konieczne.


            var First_permutation_profile_generator = new FastestProfile(first_route, first_city_copy);
            var (error1, first_profile) = First_permutation_profile_generator.ProfileBetweenNodes(3);
            var Second_permutation_profile_generator = new FastestProfile(second_route, second_city_copy);
            var (error2, second_profile) = Second_permutation_profile_generator.ProfileBetweenNodes(3);

            double first_speed;
            double second_speed;

            var time_at_end_first_permutation = get_time_at_end_of_profile(first_profile);
            var time_at_intersection_first_permutation = get_time_at_distance_from_profile(first_profile, distance_from_start_to_intersection_first_permutation);

            var first_permutation_travel_time = time_at_end_first_permutation - time_at_intersection_first_permutation;

            var time_at_end_second_permutation = get_time_at_end_of_profile(second_profile);
            var time_at_intersection_second_permutation = get_time_at_distance_from_profile(second_profile, distence_from_start_to_intersection_second_permutation);

            var second_permutation_travel_time = time_at_end_second_permutation - time_at_intersection_second_permutation;

            var travel_time_difference = second_permutation_travel_time - first_permutation_travel_time;
            return travel_time_difference;
        }
        private pair_of_permutations create_pair_for_D_intersection()
        {
            var D_intersection = get_D_intersection();
            //D_intersection.sort_permutations_by_current_length_of_traverse();
            var list_of_permutations = D_intersection.get_list_of_permutations();
            pair_of_permutations pair_D = new pair_of_permutations();
            pair_D.first_permutation = list_of_permutations[1];
            pair_D.second_permutation = list_of_permutations[0];

            int last_common_track = calculate_last_common_track_for_D_intersection_permutations(pair_D);
            pair_D.speed_at_exit = city.GetSpeedLimitForTrack_m_s(last_common_track);
            pair_D.least_comon_speed = calculate_least_common_speed_for_pair_of_permutations(pair_D);
            pair_D.required_separation_at_the_end = calculate_required_separation_between_permutations_at_D(pair_D);

            // w tym przypadku jest tylko odstep wynikajacy z przyspieszenia
            return pair_D;
        }
        private bool is_there_D_intersection_in_section()
        {
            foreach(Intersection n in list_of_intersections)
            {
                if (n.get_type() == 2)
                {
                    return true;
                }
            }
            return false;
        }
        private Intersection get_D_intersection()
        {
            foreach (Intersection n in list_of_intersections)
            {
                if (n.get_type() == 2)
                {
                    return n;
                }
            }
            return null;
        }
        private void check_all_secondary_pairs(List<pair_of_permutations> list_of_secondary_pairs, List<pair_of_permutations> list_of_primary_pairs)
        {
            for (int j = 0; j < list_of_secondary_pairs.Count; j++)
            {
                check_secondary_pair(list_of_secondary_pairs[j], list_of_primary_pairs);
            }
        }
        private void check_secondary_pair(pair_of_permutations pair, List<pair_of_permutations> list_of_primary_pairs)
        {
            pair_of_permutations first_pair_of_check;
            pair_of_permutations last_pair_of_check;
            List<pair_of_permutations> list_of_check_pairs = new List<pair_of_permutations>(list_of_primary_pairs);
            double required_pair_separation_time = pair.required_separation_at_the_end;
            double cumulative_time_from_primary_list = 0;
            first_pair_of_check = find_first_pair_of_check(pair, list_of_primary_pairs);
            last_pair_of_check = find_last_pair_of_check(pair, list_of_primary_pairs);

            trim_check_list(first_pair_of_check,last_pair_of_check,list_of_check_pairs);
            cumulative_time_from_primary_list = sum_all_check_time(list_of_check_pairs);

            if (required_pair_separation_time > cumulative_time_from_primary_list)
            {
                var difference = required_pair_separation_time - cumulative_time_from_primary_list;

                //add that difference to to first pair in the list of check
                list_of_check_pairs[0].add_additinal_required_time_of_separation(difference);
                list_of_check_pairs[0].additional_separation_because_of_secondary_pair = difference;

            }
        }
        private double sum_all_check_time(List<pair_of_permutations> list_of_check_pairs)
        {
            double time = 0;
            int index_of_last_pair = list_of_check_pairs.Count - 1;

            for(int i = 0 ; i < list_of_check_pairs.Count ; i++)
            {
                time += list_of_check_pairs[i].required_separation_at_the_end;
                if (i == index_of_last_pair)
                    break;
                else
                    time += list_of_check_pairs[i].second_permutation.get_time_of_travel_thru_exit();
            }
            return time;
        }
        private pair_of_permutations find_first_pair_of_check(pair_of_permutations pair, List<pair_of_permutations> list_of_primary_pairs)
        {
            for(int i = 0; i < list_of_primary_pairs.Count; i++)
            {
                if(pair.first_permutation == list_of_primary_pairs[i].first_permutation)
                {
                    return list_of_primary_pairs[i];

                }
            }
            return null;
        }
        private pair_of_permutations find_last_pair_of_check(pair_of_permutations pair, List<pair_of_permutations> list_of_primary_pairs)
        {
            for (int i = 0; i < list_of_primary_pairs.Count; i++)
            {
                if (pair.second_permutation == list_of_primary_pairs[i].second_permutation)
                {
                    return list_of_primary_pairs[i];

                }
            }
            return null;
        }
        private void trim_check_list(pair_of_permutations first_pair,pair_of_permutations last_pair, List<pair_of_permutations> list_of_check_pairs)
        {
            int length = list_of_check_pairs.Count;
            //remove pairs before first one
            for (int i = 0; i < length; i++)
            {
                if (list_of_check_pairs[0]==first_pair)
                {
                    break;
                }
                else
                    list_of_check_pairs.RemoveAt(0);
            }

            //remove pairs alfer last one
            list_of_check_pairs.Reverse();
            length = list_of_check_pairs.Count;
            for (int i = 0; i < length; i++)
            {
                if (list_of_check_pairs[0]==last_pair)
                {
                    break;
                }
                else
                    list_of_check_pairs.RemoveAt(0);
            }

            list_of_check_pairs.Reverse();
        }
        private void calculate_speed_at_exit(Permutation permutation)
        {
            var route = permutation.Get_route();
            var index_of_last_track = route.GetLength(0) - 1;
            int last_track = route[index_of_last_track][2];

            permutation.set_exit_speed(city.GetSpeedLimitForTrack_m_s(last_track));
        }

        private void calculate_speed_at_entrance(Permutation permutation)
        {
            var route = permutation.Get_route();
            int first_track = route[0][2];
            permutation.set_entrance_speed(city.GetSpeedLimitForTrack_m_s(first_track));
        }

        private double calculate_required_separation_between_permutations_at_the_end_of_common_route(pair_of_permutations pair)
        {
            double distance_of_similar_speed_first_permutation = 0;
            double distence_of_similar_speed_second_permutation = 0;

            var length_of_first_traverse = pair.first_permutation.get_current_length_of_traverse();
            var length_of_second_traverse = pair.second_permutation.get_current_length_of_traverse();

            var first_city_copy = city.DeepCopyCity();
            var second_city_copy = city.DeepCopyCity();

            var first_common_track_number = calculate_first_common_track(pair);

            var traverse_first_pod_mod = new DC_section_traverse_mod(first_city_copy);
            var traverse_second_pod_mod = new DC_section_traverse_mod(second_city_copy);

            int[][] first_route = pair.first_permutation.Get_deep_copy_of_route();
            int[][] second_route = pair.second_permutation.Get_deep_copy_of_route();

            var distance_from_start_to_intersection_first_permutation = sum_distance_from_route_start_to_track(first_common_track_number, pair.first_permutation.Get_route());
            var distence_from_start_to_intersection_second_permutation = sum_distance_from_route_start_to_track(first_common_track_number, pair.second_permutation.Get_route());

            

            // sprawdzam czy predkosc wlaczania sie do ruchu pierwszej kapsoly jest nizsza niz po wlaczeniu sie do ruchu
            // nierobie tego bo jesli tak jest to strta czasu wyjdzie ujemna. dodanie tego sprawdzenia na poczatku bylo by fajne ale wymaga pracy i czasu
            // lepiej jak zrobi to za mnie komp

            traverse_first_pod_mod.add_mod_of_first_pod_in_a_traverse(length_of_first_traverse, ref first_route);    
            traverse_second_pod_mod.add_mod_of_last_pod_in_a_traverse(length_of_second_traverse, ref second_route);  


            var First_permutation_profile_generator = new FastestProfile(first_route, first_city_copy);
            var (error1, first_profile) = First_permutation_profile_generator.ProfileBetweenNodes(3);
            var Second_permutation_profile_generator = new FastestProfile(second_route, second_city_copy);
            var (error2, second_profile) = Second_permutation_profile_generator.ProfileBetweenNodes(3);

            double length_of_route_first_traverse = first_profile[first_profile.Count - 1][1];

            // przesuniecie obu profili tak zeby mialy koniec w punkcie zero

            shift_profile_time_to_zero_at_end(first_profile);
            shift_profile_time_to_zero_at_end(second_profile);

            // dla calego odcinka wspolnej drogi licze roznice w czasie
            // ide od poczatku sekcji wspolnej do konca 
            // licze odchylke i sprawdzam czy jest w exstremum
            // max odchylka na minus oznacza konieczne przesuniecie
            double length_of_check = length_of_route_first_traverse - distance_from_start_to_intersection_first_permutation;
            double current_position = 0;
            double deviation=0;
            double temp_deviation;
            double temp_time_of_first_profile;
            double temp_time_of_second_profile;
            double delta = 0.05;
            while (current_position < length_of_check)
            {
                temp_time_of_first_profile = get_time_at_distance_from_profile(first_profile, current_position + distance_from_start_to_intersection_first_permutation);
                temp_time_of_second_profile = get_time_at_distance_from_profile(second_profile, current_position + distence_from_start_to_intersection_second_permutation);

                temp_deviation = temp_time_of_first_profile - temp_time_of_second_profile;
                if(temp_deviation<deviation)
                    deviation=temp_deviation;
                current_position += delta;
                
                // pierwszy profil jest pozniej w danym punkcie jesli jest dodatnia wartosc
                // jak sa wrtosci ujemne to znaczy ze drugi zespol zostal wyprzedzony i doszlo by do kolizji
                // na koniec najwieksza wartosc ujemna jest wymaganym odstepem w czasie pomiedzy zespolami
                // jesli nigdzie niebylo wartosci ujemnych to odstep pomiedzy zespolami moze zostac zero
            }
            return -deviation;
        }
        private double calculate_required_separation_between_permutations_at_D(pair_of_permutations pair)
        {
            double distance_of_similar_speed_first_permutation = 0;
            double distence_of_similar_speed_second_permutation = 0;

            var length_of_first_traverse = pair.first_permutation.get_current_length_of_traverse();
            var length_of_second_traverse = pair.second_permutation.get_current_length_of_traverse();

            var first_city_copy = city.DeepCopyCity();
            var second_city_copy = city.DeepCopyCity();

            var first_common_track_number = calculate_first_common_track(pair);

            var traverse_first_pod_mod = new DC_section_traverse_mod(first_city_copy);
            var traverse_second_pod_mod = new DC_section_traverse_mod(second_city_copy);

            int[][] first_route = pair.first_permutation.Get_deep_copy_of_route();
            int[][] second_route = pair.second_permutation.Get_deep_copy_of_route();

            var distance_from_start_to_intersection_first_permutation = sum_distance_from_route_start_to_track(first_common_track_number, pair.first_permutation.Get_route());
            var distence_from_start_to_intersection_second_permutation = sum_distance_from_route_start_to_track(first_common_track_number, pair.second_permutation.Get_route());



            // sprawdzam czy predkosc wlaczania sie do ruchu pierwszej kapsoly jest nizsza niz po wlaczeniu sie do ruchu
            // nierobie tego bo jesli tak jest to strta czasu wyjdzie ujemna. dodanie tego sprawdzenia na poczatku bylo by fajne ale wymaga pracy i czasu
            // lepiej jak zrobi to za mnie komp

            traverse_first_pod_mod.add_mod_of_first_pod_in_a_traverse(length_of_first_traverse, ref first_route);    // niepamietam czemu length of first traverse in both cases 
            traverse_second_pod_mod.add_mod_of_last_pod_in_a_traverse(length_of_second_traverse, ref second_route);  // wyglada raczej na to ze powinienem liczyc mod pierwszej kapsuly z pierwszego zespolu
                                                                                                                     // i ostatniej kapsuly z drugiego zespolu

            //wprowadzam modyfikacje profilu tak zeby symulacja byla przeprowadzona dla kapsuly jadacej w zespole od dlugosci traversu
            // niema to znaczenia czy to bedzie dla pierwszej kapsuly, czy ostatniej , ale modyfikacje sa konieczne.


            var First_permutation_profile_generator = new FastestProfile(first_route, first_city_copy);
            var (error1, first_profile) = First_permutation_profile_generator.ProfileBetweenNodes(3);
            var Second_permutation_profile_generator = new FastestProfile(second_route, second_city_copy);
            var (error2, second_profile) = Second_permutation_profile_generator.ProfileBetweenNodes(3);

            double length_of_route_first_traverse = first_profile[first_profile.Count - 1][1];

            // przesuniecie obu profili tak zeby mialy koniec w punkcie zero

            //shift_profile_time_to_zero_at_end(first_profile);
            //shift_profile_time_to_zero_at_end(second_profile);

            // dla calego odcinka wspolnej drogi licze roznice w czasie
            // ide od poczatku do konca 
            // licze odchylke i sprawdzam czy jest w exstremum
            // max odchylka na minus oznacza konieczne przesuniecie


            double length_of_check = 100;//length of check 
            double current_position = 0;
            double deviation = 0;
            double temp_deviation;
            double temp_time_of_first_profile;
            double temp_time_of_second_profile;
            double delta = 0.05;


            while (current_position < length_of_check)
            {
                temp_time_of_first_profile = get_time_at_distance_from_profile(first_profile, current_position + distance_from_start_to_intersection_first_permutation);
                temp_time_of_second_profile = get_time_at_distance_from_profile(second_profile, current_position + distence_from_start_to_intersection_second_permutation);

                temp_deviation = temp_time_of_first_profile - temp_time_of_second_profile;
                if (temp_deviation < deviation)
                    deviation = temp_deviation;
                current_position += delta;

                // pierwszy profil jest pozniej w danym punkcie jesli jest dodatnia wartosc
                // jak sa wrtosci ujemne to znaczy ze drugi zespol zostal wyprzedzony i doszlo by do kolizji
                // na koniec najwieksza wartosc ujemna jest wymaganym odstepem w czasie pomiedzy zespolami
                // jesli nigdzie niebylo wartosci ujemnych to odstep pomiedzy zespolami moze zostac zero
            }
            return -deviation;
        }

        private List<double[]> shift_profile_time_to_zero_at_end(List<double[]> profile)
        {

            double current_time_at_end = profile[profile.Count - 1][8];
            double shift_time = - current_time_at_end;


            int length = profile.Count;

            for (int i = 0; i < length; i++)
            {
                profile[i][7] += shift_time;
                profile[i][8] += shift_time;
            }
            return profile;
        }
        private double sum_distance_from_route_start_to_track(int track_number, int[][] route)
        {
            double sum = 0;

            for(int i = 0; i < route.GetLength(0); i++)
            {
                if (route[i][2] == track_number)
                    break;
                sum += city.GetLenghtOfTrack(route[i][2]);

            }
            return sum;
        }
        private int calculate_first_common_track(pair_of_permutations pair)
        {

            int first_common_track_number = -1;
            int[][] route_first = pair.first_permutation.Get_route();
            int[][] route_second = pair.second_permutation.Get_route();

            for (int i = 0; i < route_first.GetLength(0); i++)
            {
                if (is_track_present_in_second_route(route_first[i][2], route_second))
                {
                    first_common_track_number = route_first[i][2];
                    break;
                }

            }
            return first_common_track_number;


        }
        private int calculate_last_common_track_for_D_intersection_permutations(pair_of_permutations pair)
        {
            int last_common_track_number = -1;
            int[][] route_first = pair.first_permutation.Get_route();
            int[][] route_second = pair.second_permutation.Get_route();

            for (int i = 0; i < route_first.GetLength(0); i++)
            {
                if (!is_track_present_in_second_route(route_first[i][2], route_second))
                {
                    break;
                }
                last_common_track_number = route_first[i][2];
            }
            return last_common_track_number;
        }
        private void calculate_least_common_speed_for_list_of_pairs(List<pair_of_permutations> pairs_of_permutations)
        {
            pair_of_permutations pair;
            for (int i = 0; i < pairs_of_permutations.Count; i++)
            {
                pair = pairs_of_permutations[i];
                pair.least_comon_speed = calculate_least_common_speed_for_pair_of_permutations(pair);

            }
        }
        private List<pair_of_permutations> create_list_of_secondary_pairs(List<Permutation> permutations)
        {
            List<pair_of_permutations> lista = new List<pair_of_permutations>();
            pair_of_permutations pair;

            for (int i = 0; i < permutations.Count - 2; i++)
            {
                for(int j = 2 + i; j < permutations.Count; j++)
                {
                    pair = new pair_of_permutations();
                    pair.first_permutation = permutations[i];
                    pair.second_permutation = permutations[j];
                    lista.Add(pair);
                }
            }

            return lista;
        }
        private double calculate_least_common_speed_for_pair_of_permutations(pair_of_permutations pair)
        {
            
            var list_of_common_tracks = return_list_of_common_tracks(pair.first_permutation.Get_route(), pair.second_permutation.Get_route());
            var least_common_speed = find_least_common_speed_in_list_of_tracks(list_of_common_tracks);
            return least_common_speed;
        }
        private void calculate_exit_speed_for_list_of_pairs(List<pair_of_permutations> list)
        {
            pair_of_permutations pair;
            for(int i=0; i < list.Count; i++)
            {
                pair=list[i];
                pair.speed_at_exit = calculate_exit_speed_for_pair(pair);
            }

        }
        private double calculate_exit_speed_for_pair(pair_of_permutations pair)
        {
            var route = pair.first_permutation.Get_route();
            var index_of_last_track = route.GetLength(0) - 1;
            int last_track = route[index_of_last_track][2];

            return city.GetSpeedLimitForTrack_m_s(last_track);

        }
        private double find_least_common_speed_in_list_of_tracks(List<int> list_of_tracks)
        {
            double smallest_speed = 0;
            double speed;

            foreach (int n in list_of_tracks)
            {
                speed =city.GetSpeedLimitForTrack_m_s(n);
                if (smallest_speed == 0 || speed < smallest_speed)
                {
                    smallest_speed = speed;
                }

            }

            return smallest_speed;
        }
        private List<int> return_list_of_common_tracks(int[][] first_route, int[][] second_route)
        {
            var lista = new List<int>();

            for (int i = 0; i < first_route.GetLength(0); i++)
            {
                if (is_track_present_in_second_route(first_route[i][2], second_route))
                {
                    lista.Add(first_route[i][2]);
                }
            }
            return lista;
        }

        private bool is_track_present_in_second_route(int track_number, int[][] second_route)
        {
            bool is_track_present = false;

            for (int i = 0; i < second_route.GetLength(0); i++)
            {
                if (track_number == second_route[i][2])
                {
                    is_track_present = true;
                    break;
                }
            }
            return is_track_present;
        }

        private List<pair_of_permutations> create_list_of_pairs(List<Permutation> permutations)
        {
            List<pair_of_permutations> lista = new List<pair_of_permutations>();
            pair_of_permutations new_pair;
            if (permutations.Count == 1)
                return null;
            int number_of_pairs = permutations.Count-1;
            for (int i = 0;i< number_of_pairs; i++)
            {
                new_pair = new pair_of_permutations();
                new_pair.first_permutation = permutations[i];
                new_pair.second_permutation = permutations[i+1];
                lista.Add(new_pair);
            }

            return lista;

        }
        public void define_components_of_section_schematics(List<Permutation> section_permutations)
        {
            //go along route of all permutations and add entrances, intersection and exits


            var route_nodes = new List<int>();
            foreach(Permutation n in section_permutations)
            {
                route_nodes = get_list_of_nodes_along_route(n.Get_route());

                for (int i = 0; i < route_nodes.Count; i++)
                {
                    check_node(route_nodes[i],n);
                }

            }
        }
        private List<int> get_list_of_nodes_along_route(int[][] route)
        {
            var new_list =new List<int>();

            for(int i = 0 ; i < route.GetLength(0) ; i++)
            {
                new_list.Add(route[i][0]);
            }

            new_list.Add(route[route.GetLength(0)-1][1]);

            return new_list;
        }
        private void check_node(int node_number, Permutation permutation)
        {
            bool is_this_a_start_node = permutation.is_node_a_start_node(node_number);
            bool is_this_an_end_node = permutation.is_node_an_end_node(node_number);
            // tu jest problem bo jak dodje schematic component do permutacji
            // to dla stacji dodaje jednoczasnie exit i start
            // a w przypadku stacji powinenem dodawac tylko start jesli dana permutacja sie tam zaczyna
            // i tylko koniec jesli dana permutacja sie tam konczy


            if (is_node_an_entrance(node_number) && is_this_a_start_node)
            {
                Entrance entrance;
                if (!is_there_already_antrance_attached_to_node(node_number))
                {
                    
                    list_of_entrances.Add(new Entrance(node_number));
                }

                add_per_to_entrance(node_number, permutation);

                //sort per required patern
                //for now it will be sort and reverse
                //entrance = get_entrance_attached_to_node(node_number);
                //entrance.sort_permutations_by_number();
            }


            var (is_node_intersection, type) = is_node_an_intersection(node_number);//type 1 convergence, 2 Divergence
            if (is_node_intersection)
            {
                Intersection intersection;
                if (!is_there_intersection_allready_attached_to_node(node_number))
                {
                    list_of_intersections.Add(new Intersection(node_number, type));
                }

                add_per_to_intersection(node_number, permutation);
                //sort per required patern
                //for now it will be sort and reverse
                //intersection = get_intersection_attached_to_node(node_number);
                //intersection.sort_permutations_by_number();
            }

            if (is_node_an_exit(node_number) && is_this_an_end_node)
            {
                Exit exit;
                if (!is_there_exit_allready_attached_to_node(node_number))
                {
                    list_of_exits.Add(new Exit(node_number));
                }

                add_per_to_exit(node_number, permutation);
                //sort per required patern
                //for now it will be sort and reverse
                //exit = get_exit_attached_to_node(node_number);
                //exit.sort_permutations_by_number();
            }

        }
        public void define_traverses_length(List<Permutation> permutations, double length_trav_DC)
        {

            double max_temp_length = length_trav_DC;
            {
                foreach(Permutation m in permutations)
                {
                    //max_temp_length = m.get_max_length_of_traverse();

                    //m.set_current_length_of_traverse(max_temp_length - 1);



                    //switch (m.get_number_of_permutation())
                    //{
                    //    case 0:
                    //        max_temp_length = 17;
                    //        break;
                    //    case 1:
                    //        max_temp_length = 17;
                    //        break;
                    //    case 2:
                    //        max_temp_length = 17;
                    //        break;
                    //    case 3:
                    //        max_temp_length = 80; //sort
                    //        break;
                    //    case 4:
                    //        max_temp_length = 17;
                    //        break;
                    //    case 5:
                    //        max_temp_length = 15;
                    //        break;
                    //    case 6:
                    //        max_temp_length = 15;
                    //        break;
                    //    case 7:
                    //        max_temp_length = 80; //sort
                    //        break;
                    //    case 8:
                    //        max_temp_length = 18;
                    //        break;
                    //    case 9:
                    //        max_temp_length = 13;
                    //        break;
                    //    case 10:
                    //        max_temp_length = 12;
                    //        break;
                    //    case 11:
                    //        max_temp_length = 80;//sort
                    //        break;
                    //    case 12:
                    //        max_temp_length = 15;
                    //        break;
                    //    case 13:
                    //        max_temp_length = 14;
                    //        break;
                    //    case 14:
                    //        max_temp_length = 11;
                    //        break;
                    //    case 15:
                    //        max_temp_length = 80; //sort
                    //        break;

                    //}

                    m.set_current_length_of_traverse(max_temp_length);

                }
            }
        }
        public double get_speed_at_distance_from_profile(List<double[]> profile, double distance_from_start)
        {
            double speed = 0;
            double distance_into_affected_track = 0;
            double distance_from_affected_end_of_track = 0;
            double initial_speed;
            double end_speed;
            double acceleration;
            int index_of_profile = 0;

            //znajduje w ktorym odcinku profilu jest punkt docelowy
            for (int i = 0; i < profile.Count; i++)
            {
                if (profile[i][0] <= distance_from_start && profile[i][1] > distance_from_start)
                {
                    index_of_profile = i;
                    //licze na ile wchodzi dany punkt w odcinek docelowy
                    distance_into_affected_track = distance_from_start - profile[i][0];
                    distance_from_affected_end_of_track = profile[i][1] - distance_from_start;
                    break;
                }
            }

            //sprawdzam w jakim rodzaju odcinka jest punkt docelowy 
            switch (profile[index_of_profile][4])
            {
                //jesli jest to 0 (track) to podaje poprostu predkos danego tracku
                case 0:

                    speed = profile[index_of_profile][2];

                    break;
                //jesli jest to 1 (acceleration) to licze predkosc z wzoru na bazie odleglosci od poczatku przyspieszania
                case 1:

                    initial_speed = profile[index_of_profile][2];
                    acceleration = profile[index_of_profile][3];
                    speed = (-initial_speed + Math.Sqrt(Math.Pow(initial_speed, 2) + 2 * distance_into_affected_track * acceleration)) + initial_speed;

                    break;

                //jesli jest to -1 (brake) to licze predkosc z wzoru na bazie odleglosci (prawdopodobnie od konca tak jak byl liczony)
                case -1:
                    end_speed = profile[index_of_profile][2];
                    acceleration = profile[index_of_profile][3];

                    speed = (-end_speed + Math.Sqrt(Math.Pow(end_speed, 2) + 2 * distance_from_affected_end_of_track * acceleration)) + end_speed;

                    break;
            }
            return speed;

        }
        public double get_time_at_distance_from_profile(List<double[]> profile, double distance_from_start)
        {
            if (distance_from_start == profile[profile.Count - 1][1])
            {
                return profile[profile.Count - 1][8];
            }

            double time = 0;
            double distance_into_affected_track = 0;
            double distance_from_affected_end_of_track = 0;
            double initial_speed;
            double initial_time;
            double time_at_end_of_affected_track;
            double end_speed;
            double acceleration;
            double velocity;
            int index_of_profile = 0;

            //znajduje w ktorym odcinku profilu jest punkt docelowy
            for (int i = 0; i < profile.Count; i++)
            {
                if (profile[i][0] <= distance_from_start && profile[i][1] > distance_from_start)
                {
                    index_of_profile = i;
                    //licze na ile wchodzi dany punkt w odcinek docelowy
                    distance_into_affected_track = distance_from_start - profile[i][0];
                    distance_from_affected_end_of_track = profile[i][1] - distance_from_start;
                    break;
                }
            }

            //sprawdzam w jakim rodzaju odcinka jest punkt docelowy 
            switch (profile[index_of_profile][4])
            {
                //jesli jest to 0 (track) to sumuje czas poczatku sekcji i czas potrzebny na przejechanie odcinka wglab sekcji;
                case 0:
                    velocity = profile[index_of_profile][2];
                    time = profile[index_of_profile][7] + distance_into_affected_track / velocity;

                    break;
                //jesli jest to 1 (acceleration) to licze czas z wzoru na bazie odleglosci od poczatku przyspieszania
                case 1:

                    initial_speed = profile[index_of_profile][2];
                    initial_time = profile[index_of_profile][7];
                    acceleration = profile[index_of_profile][3];
                    time = (-initial_speed + Math.Sqrt(Math.Pow(initial_speed, 2) + 2 * distance_into_affected_track * acceleration)) / acceleration + initial_time;

                    break;

                //jesli jest to -1 (brake) to licze czas z wzoru na bazie odleglosci (hamowanie ma odwrocony kierunek obliczen)
                case -1:
                    end_speed = profile[index_of_profile][2];
                    time_at_end_of_affected_track = profile[index_of_profile][8];
                    acceleration = profile[index_of_profile][3];

                    time = -(-end_speed + Math.Sqrt(Math.Pow(end_speed, 2) + 2 * distance_from_affected_end_of_track * acceleration)) / acceleration + time_at_end_of_affected_track;

                    break;
            }
            return time;

        }
        public double get_time_at_start_of_profile(List<double[]> profile)
        {
            return profile[0][6];
        }
        public double get_time_at_end_of_profile(List<double[]> profile)
        {
            var index_of_end = profile.Count-1;
            return profile[index_of_end][8];// is this a correct index ?
        }
        private SchematicComponent get_schematic_component_for_node (int node_number)
        {
            var (is_node_intersection, type) = is_node_an_intersection(node_number);
            if (is_node_intersection)
                return get_intersection_attached_to_node(node_number);
            else if (is_node_an_entrance(node_number))
            {
                return get_entrance_attached_to_node(node_number);
            }
            else if (is_node_an_exit(node_number))
            {
                return get_exit_attached_to_node(node_number);
            }

            return null;
        }
        private Intersection get_intersection_attached_to_node(int node)
        {
            foreach(Intersection n in list_of_intersections)
            {
                if (n.get_number_of_attached_noude() == node)
                {
                    return n;
                }
            }

            return null;
        }
        private Entrance get_entrance_attached_to_node(int node)
        {
            foreach(Entrance n in list_of_entrances)
            {
                if (n.get_attached_node() == node)
                {
                    return n;
                }
            }
            return null;
        }
        private Exit get_exit_attached_to_node(int node)
        {
            foreach(Exit n in list_of_exits)
            {
                if(n.get_attached_node_number() == node)
                {
                    return n;
                }
            }
            return null;
        }

        private bool is_node_an_entrance(int node_number)
        {
            if (list_of_I_nodes.Contains(node_number))
                return true;
            else
                return false;
        }
        private bool is_there_already_antrance_attached_to_node(int node_number)
        {
            int node;
            foreach (Entrance n in list_of_entrances)
            {
                node = n.get_attached_node();
                if (node == node_number)
                    return true;
            }
            return false;
        }

      
        private bool is_node_an_exit(int node_number)
        {
            if (list_of_O_nodes.Contains(node_number))
                return true;
            else
                return false;
        }
        private bool is_there_exit_allready_attached_to_node(int node_number)
        {
            foreach(Exit n in list_of_exits)
            {
                if (n.get_attached_node_number() == node_number)
                    return true;
            }
            return false;
        }
        
        private void add_per_to_entrance(int node_number, Permutation permutation)
        {
            foreach (Entrance n in list_of_entrances)
            {
                if (n.get_number_of_attached_noude() == node_number)
                {
                    n.add_permutation_to_list(permutation);
                    permutation.add_component_to_list(n);
                }
            }
        }
        private void add_per_to_exit(int node_number, Permutation permutation)
        {
            foreach(Exit n in list_of_exits)
            {
                if (n.get_attached_node_number() == node_number)
                {
                    n.add_permutation_to_list(permutation);
                    permutation.add_component_to_list(n);
                }
            }
        }
        private void add_per_to_intersection(int node_number, Permutation permutation)
        {
            foreach(Intersection n in list_of_intersections)
            {
                if (n.get_number_of_attached_noude() == node_number)
                {
                    n.add_permutation_to_list(permutation);
                    permutation.add_component_to_list(n);
                }
            }
        }
        private (bool, int) is_node_an_intersection(int node_number)
        {
            return city.is_node_an_intersection_plus_type(node_number);
        }
        private bool is_there_intersection_allready_attached_to_node(int node_number)
        {
            foreach(Intersection n in list_of_intersections)
            {
                if (n.get_number_of_attached_noude() == node_number)
                    return true;
            }
            return false;
        }
    }

}
