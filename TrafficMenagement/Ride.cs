using System;
using System.Collections.Generic;

namespace Symulation
{
    public class Ride
    {
        
        int RideNumber;
        Pod AssignedPod;

        public int start_station { get; set; }
        public int end_station { get; set; }


        CityDataStorage BaseCity;
        CityDataStorage ModifiedCity;

        double start_time_of_begining_first_traverse_in_global_time; //def at creation 
        
        //to zmienia sie jak zmianiam pozycje ride w traversie startowym 
        double shift_at_start_because_of_position_in_traverse; //updated with mod changes, add time proportional to position in traverse and speed at start

        double start_time_of_exit_last_traverse_in_global_time; //def at creation

        //to zmienia sie jak zmieniam pozycje ride w traversie koncowym
        double shift_at_end_bacause_of_position_in_traverse; //updated with mod changes, add time proportional to position in traverse and speed at end


        


        int[][] BaseRoute;
        int[][] ModifiedRoute;
        private List<mod_data_for_section> list_of_mods;
        //List<SectionMods> _ListOfRideSections;

        List<CitySection> list_of_sections;
        List<Permutation> list_of_permutations;

        List<Traverse> list_of_traverse;

        List<double[]> ride_profile;

        ride_position_data position_data;

        // list of mod data
            // type of mod (enum)
            // affected traverse (niewiem czy to jest potrzebne)
            // data needed for mod
                // entrance position in traverse
                // exit position in traverse

        // pytanie co jest tak naprawde potrzebne do modyfikacji i jak bedzie sie to zmieniac jak beda inne mody
        // moze powinienm zrobic abstrakcyjny mod data, i w zaleznosci rodzaj mudu dodawac potrzebne info
        // to raczej niejest potrzebne , wystarczy ze rozbuduje mod data, i bede je dodawac i wyciagac selektywnie 

        // to bedzie lista modow ktore powinny byc wprowadzone zeby moc 
        // przejechac w zaplanowany sposob przez miasto
        // to list danych do przeprowadzenia tych modow 

        // dopiero jak zostana wprpwadzone w zycie to efektem bedzie profil przejazdu i nowy zmieniony przebieg drogi (nowe dodane noudy i tracki) 
        // wprowadzenie modow , bedzie polegalo na policzeniu ich wszystkich po kolei 


        // potrzebna bedzie metoda tlumaczaca pozycje w nowym zmodyfikowanym przejezdzie na pozycje w niezmodyfikowany miescie , na ktorym bedz wyswiwtnale przejazdy
        // dla danego czasu bedzie liczona pozycja w profilu ride i potem tlumaczona na pozycje w miescie niezmodyfikowanym
        // wystarczy policzyc pozycje od zera w zmodyfikowanym przejezdzie, i na podstawie tego policzyc pozycje w oryginalnym miescie


        List<Window> list_of_entrance_windows;
        // pozycja w kazdym oknie entrance

        List<Window> list_of_exit_windows;
        // pozycja w kazdym oknie exit

        // pozycja w oknie plus pozycja startu okna pozwola wyznaczyc pozycje w traverse 


        // brakuje spisu modydyfikacji wystepujacych we wszystkich sekcjach po kolei 
        // potrzebna jest informacja o polozeniu w oknie -> polozenie w traversie -> to jest podstawa modyfikacji
        // modyfikacja - DC - wystarczy informacja jednego okna bo wejscie i wyjscie jest symetryczne  
        // modyfikacja - sort sekcja - potrzebne jest pozycja na wejsciu traversu i na wyjsciu

        // moze powinienem dodac do kazdego okna pozycje start x okna w traversie, to pozwoli szybko liczyc pozycje ride pod w traversie 

       // chyba brakuje mi dodania jeszcze wprowadzenie w zycie modyfikacji 
       // one sa teraz zapisane na listach ale niesa wprowadzane w zycie 
       // jedno przejscie ktore wprowadza wszystkie mody po kolei 
       // chce jeszcze dodac metode ktora liczy pozycje 


        public Ride(int Number, CityDataStorage city, ride_search_thread thread)
        {
            RideNumber = Number;

            BaseCity = city;
            ModifiedCity = BaseCity.DeepCopyCity();

            BaseRoute = thread.get_thread_route();
            ModifiedRoute = thread.get_thread_route();

            list_of_sections = thread.get_city_sections_list();
            list_of_permutations = thread.get_premutations_list();
            list_of_traverse = thread.get_list_of_traverses();
            list_of_entrance_windows = thread.get_entrance_windows();
            list_of_exit_windows = thread.get_exit_windows();
            list_of_mods = new List<mod_data_for_section>();
            for(int i = 0; i < list_of_sections.Count; i++)
                list_of_mods.Add(new mod_data_for_section());

            // time of start in global time of simulation
            start_time_of_begining_first_traverse_in_global_time = list_of_traverse[0].begining_start_time;
            start_time_of_exit_last_traverse_in_global_time = list_of_traverse[list_of_traverse.Count - 1].exit_start_time;

            position_data = new ride_position_data();

            start_station = thread.start_station;
            end_station = thread.end_station;
        }

        public int get_number_of_ride()
        {
            return RideNumber;
        }

        /// <summary>
        /// return time in global sym time
        /// </summary>
        /// <returns></returns>
        public (double, double) return_start_and_end_time()
        {
            return (start_time_of_begining_first_traverse_in_global_time + shift_at_start_because_of_position_in_traverse, start_time_of_exit_last_traverse_in_global_time + shift_at_end_bacause_of_position_in_traverse);
        }

        public double return_start_time()
        {
            return start_time_of_begining_first_traverse_in_global_time + shift_at_start_because_of_position_in_traverse;
        }

        /// <summary>
        /// Add mod for section,there can be only one for section. It is defined by position in traverse and length in traverse.
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="section_number"></param>
        public void add_mod_for_section(mod_data_for_section mod, int section_number)
        {
            int index = return_index_of_section_in_ride_route();

            list_of_mods[index] = mod;

            int return_index_of_section_in_ride_route()
            {
                for(int i = 0; i < list_of_sections.Count; i++)
                {
                    if (list_of_sections[i].Get_number_of_section() == section_number)
                        return i;
                }

                return -1; //error section number is not part of ride route

            }
        }

        public mod_data_for_section get_deep_copy_of_mod(int section_number)
        {
            int index = return_index_of_section_in_ride_route();

            int return_index_of_section_in_ride_route()
            {
                for (int i = 0; i < list_of_sections.Count; i++)
                {
                    if (list_of_sections[i].Get_number_of_section() == section_number)
                        return i;
                }
                return -1; //error section number is not part of ride route
            }

            var section_mod_copy = new mod_data_for_section();

            section_mod_copy.mod_type = list_of_mods[index].mod_type;
            section_mod_copy.dist_from_start_at_entrance_in_traverse = list_of_mods[index].dist_from_start_at_entrance_in_traverse;
            section_mod_copy.distance_from_start_at_exit_in_traverse = list_of_mods[index].distance_from_start_at_exit_in_traverse;
            section_mod_copy.dist_from_start_of_mod_beginning_in_permutation = list_of_mods[index].dist_from_start_of_mod_beginning_in_permutation;
            section_mod_copy.dist_from_start_of_mod_end_in_permutation = list_of_mods[index].dist_from_start_of_mod_end_in_permutation;
            
            return section_mod_copy;
        }
        public List<CitySection> get_list_of_sections()
        {
            return list_of_sections;
        }

        public List<Permutation> get_list_of_permutations()
        {
            return list_of_permutations;
        }

        public List<Traverse> get_list_of_trraverses()
        {
            return list_of_traverse;
        }

        public List<Window> get_entrance_windows()
        {
            return list_of_entrance_windows;
        }

        public Window get_entrance_window_at_section_index(int index)
        {
            return list_of_entrance_windows[index];
        }

        public int get_index_of_section_for_entrance_window(Window win)
        {
            for(int i = 0; i < list_of_entrance_windows.Count; i++)
            {
                if(list_of_entrance_windows[i] == win)
                {
                    return i;
                }
            }
            return -1;
        }

        public List<Window> get_exit_windows()
        {
            return list_of_exit_windows;
        }

        public Window get_exit_window_at_section_index(int index)
        {
            return list_of_exit_windows[index];
        }

        public Window get_entrance_window_for_section_number(int section_number)
        {
            int index = -1;
            //sprawdzam index ktory ma sekcja w danym ride

            for (int i=0; i < list_of_sections.Count; i++)
            {
                if (list_of_sections[i].Get_number_of_section() == section_number)
                {
                    index = i;
                    break;
                }
            }
            //dle tego indexu zwracam entrance win 
            return list_of_entrance_windows[index];

        }

        public Window get_exit_window_for_section_number(int section_number)
        {
            int index = -1;
            //sprawdzam index ktory ma sekcja w danym ride

            for (int i = 0; i < list_of_sections.Count; i++)
            {
                if (list_of_sections[i].Get_number_of_section() == section_number)
                {
                    index = i;
                    break;
                }
            }

            return list_of_exit_windows[index];
        }

        public int get_index_of_section_for_exit_window(Window win)
        {
            for (int i = 0; i < list_of_entrance_windows.Count; i++)
            {
                if (list_of_entrance_windows[i] == win)
                {
                    return i;
                }
            }
            return -1;
        }

        public Window get_start_window()
        {
            return list_of_entrance_windows[0];
        }
        

        public void ClearModsForSection(int section_number)
        {



        }


        /// <summary>
        /// Apply mod per definition in mod_def_list for same index. is aplied to copy of city and route. Only one mod can be applied
        /// to section, if new mod is to be applied after changes, reset copy of city and route to base state and that apply mod.
        /// </summary>
        /// <param name="index_sekcji"></param>
        public void apply_mod_in_section( int index_sekcji)
        {
            // dotychczas podczas obliczen symulaci, kolejnosci itp wrzucalem info jak zrobic mody
            // ale one niebyly realizowane. Wiec teraz je wprowadzam 

            // wykozystuje dane z listy mod data 
            // wprowadzam mod dla danej sekcji
            // co oznacza zmodyfikowanie miasta i route
            // chce je wprowadzac po kolei, mimo ze teraz to jeszcze niema zanaczenia, 
            // jak beda modyfikacje sie zmieniac na zywo w czasie symulacji to bede dzialac tak ze zmieniam jeden mod i resetuje kopie miasta dla ride
            // i wprowadzam je wszystkie od nowa, inaczej potrzebny by byl skomplikowany algorytm usuwania wprowadzonych zmian dla sekcji
            // co jest wlasciwie nierealne, bo moze zmienic sie cala numeracja miasta po jednym 
            // nowym modzie jesli zmieni sie inosc nowych noudow.

            
            if(list_of_mods[index_sekcji].mod_type == mod_type.DC_section)
            {
                //instanciate object , capable of making that mod
                var DC_mod = new DC_section_traverse_mod(ModifiedCity);

                var length_of_traverse = list_of_traverse[index_sekcji].Size;
                var distance_from_start_of_traverse = list_of_mods[index_sekcji].dist_from_start_at_entrance_in_traverse;
                var distance_from_end_of_traverse = list_of_mods[index_sekcji].distance_from_start_at_exit_in_traverse;
                var permutation = list_of_permutations[index_sekcji];

                DC_mod.add_mod_for_arbitrary_pod_in_traverse(permutation, length_of_traverse, distance_from_start_of_traverse, ref ModifiedRoute);

                // tutaj licze jakie musi miec opznienie w starcie kapsola zeby jechala zsynchronizowana z reszta kapsol z traverse.
                if (index_sekcji == 0)
                {
                    shift_at_start_because_of_position_in_traverse = (distance_from_start_of_traverse) / list_of_permutations[0].get_entrance_speed();
                }

                if(index_sekcji == list_of_mods.Count - 1)
                {
                    shift_at_end_bacause_of_position_in_traverse = distance_from_end_of_traverse / list_of_permutations[list_of_permutations.Count -1].get_exit_speed();
                }
            }

            
            if(list_of_mods[index_sekcji].mod_type == mod_type.sort_section)
            {
                //instanciate object , capable of making that mod
                var sort_mod = new Sort_section_traverse_mod(ModifiedCity);

                var length_of_traverse = list_of_traverse[index_sekcji].Size;
                var distance_from_start_of_traverse_at_begining = list_of_mods[index_sekcji].dist_from_start_at_entrance_in_traverse;
                var distance_form_start_of_traverse_at_end = list_of_mods[index_sekcji].distance_from_start_at_exit_in_traverse;
                var dist_form_start_of_permutation_at_beginning = list_of_mods[index_sekcji].dist_from_start_of_mod_beginning_in_permutation;
                var dist_from_start_of_permutation_at_end = list_of_mods[index_sekcji].dist_from_start_of_mod_end_in_permutation;
                var permutation = list_of_permutations[index_sekcji];

                sort_mod.add_mod_for_arbitrary_pod_in_traverse(permutation, length_of_traverse,dist_form_start_of_permutation_at_beginning,dist_from_start_of_permutation_at_end ,distance_from_start_of_traverse_at_begining, distance_form_start_of_traverse_at_end, ref ModifiedRoute);

            }


        }


        public void aplly_all_mods()
        {
            for(int i = 0; i < list_of_sections.Count; i++)
                apply_mod_in_section(i);
        }

        public void restet_ride_city_and_route_to_base_state()
        {
            ModifiedCity = BaseCity.DeepCopyCity();
            ModifiedRoute = BaseRoute;
        }


        public void calculate_profile()
        {
            var profile_calculator = new FastestProfile(ModifiedRoute, ModifiedCity);

            
            var(error_code, profile) = profile_calculator.ProfileBetweenNodes();

            ride_profile = profile;


        }




        /// <summary>
        /// Returst bool, if ride going thru at least one of traverse in list of traverse.
        /// </summary>
        /// <param name="traverse_list"></param>
        /// <returns></returns>
        public bool is_this_ride_going_thru_one_of_travers_in_list(List<int> traverse_list)
        {
            for(int i = 0; i < traverse_list.Count; i++)
                if (is_this_ride_going_thru_traverse(traverse_list[i]))
                    return true;
            //ride is not going thru any of traverse in the list 
            return false;
        }


        /// <summary>
        /// Returst bool, is rede going thru traverse, param traverse is int
        /// </summary>
        /// <param name="treverse"></param>
        /// <returns></returns>
        public bool is_this_ride_going_thru_traverse(int traverse)
        {
            for(int i = 0; i < list_of_traverse.Count; i++)
                if(list_of_traverse[i].Number == traverse)
                    return true;

            //ride is not going thru traverse 
            return false;
        }

        


        public bool is_this_global_time_within_ride_time(double time)
        {
            if (time >= start_time_of_begining_first_traverse_in_global_time + shift_at_start_because_of_position_in_traverse && time < start_time_of_exit_last_traverse_in_global_time + shift_at_end_bacause_of_position_in_traverse)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Metod calculates location x,y (cordinates of city) of pod by time in global sim time corrdinate. 
        /// </summary>
        /// <param name="time"></param>
        public (float, float) calculate_position_of_pod_at_time(double time)
        {
            time -= start_time_of_begining_first_traverse_in_global_time + shift_at_start_because_of_position_in_traverse;

            var distance_into_profile = get_distance_at_time_from_profile(time); 
            var (pos_x, pos_y) = get_cordinates_at_distance(distance_into_profile);
            return (pos_x, pos_y);
        }

        public (float, float, float) calculate_position_and_angle_of_pod_at_time(double time)
        {
            time -= start_time_of_begining_first_traverse_in_global_time + shift_at_start_because_of_position_in_traverse;

            var distance_into_profile = get_distance_at_time_from_profile(time);

            var (pos_x, pos_y, ang) = get_cordinates_and_angle_at_distance(distance_into_profile);
            return (pos_x, pos_y, ang);
        }


        public void calculate_distance_into_route_check()
        {
            double delta_time = 0.03;
            
            double time = delta_time;
            int number_of_jumps = 0;
            
            double end_time = start_time_of_exit_last_traverse_in_global_time + shift_at_end_bacause_of_position_in_traverse - start_time_of_begining_first_traverse_in_global_time -shift_at_start_because_of_position_in_traverse;

            double dist_1;
            double dist_2;

            while (time < end_time)
            {
                dist_1 = get_distance_at_time_from_profile(time - delta_time);
                dist_2 = get_distance_at_time_from_profile(time);

                if(Math.Abs(dist_1 - dist_2) > 0.7)
                    number_of_jumps++;

                time += delta_time;
            }

            Console.WriteLine("numer ride {0}", RideNumber);
            Console.WriteLine("number of jumps {0}", number_of_jumps);
        }

        public bool debugg_sort_section_travel_time()
        {
            double time_span_of_sort_teory;
            double time_span_of_sort_profile;

            double sort_profile_start = 0;
            double sort_profile_end = 0;

            int sort_start_node;
            int sort_end_node;

            double x;

            for (int i = 0; i<list_of_sections.Count; i ++)
            {
                if (list_of_sections[i].what_is_the_type_of_this_section() == typeof(Sort_section))
                {


                    time_span_of_sort_teory = (list_of_permutations[i].get_permutation_length() - list_of_mods[i].dist_from_start_at_entrance_in_traverse + list_of_mods[i].distance_from_start_at_exit_in_traverse) /
                        list_of_permutations[i].get_entrance_speed();
                    x = list_of_mods[i].dist_from_start_at_entrance_in_traverse - list_of_mods[i].distance_from_start_at_exit_in_traverse;
                    sort_start_node = list_of_permutations[i].get_start_node();
                    sort_end_node = list_of_permutations[i].get_end_node();


                    for (int j = 0; j < ride_profile.Count; j++)
                    {
                        if (ride_profile[j][5] == sort_start_node)
                        {
                            sort_profile_start = ride_profile[j][7];
                            break;
                        }
                    }

                    for (int j = ride_profile.Count - 1; j >= 0; j--)
                    {
                        if (ride_profile[j][6] == sort_end_node)
                        {
                            sort_profile_end = ride_profile[j][8];
                            break;
                        }
                    }

                    time_span_of_sort_profile = sort_profile_end - sort_profile_start;


                    if (Math.Abs(time_span_of_sort_teory - time_span_of_sort_profile) > 0.0001)
                    {
                        Console.WriteLine("ride number {0}", RideNumber);
                        Console.WriteLine("time theory : {0}  profile : {1} x: {2} diffrence : {3}", time_span_of_sort_teory, time_span_of_sort_profile, x, time_span_of_sort_teory - time_span_of_sort_profile);

                        return false;

                    }
                }
            }

            return true;
        }


        public bool debug_DC_section_ride_time()
        {
            double DC_profile_start = 0;
            double DC_profile_end = 0;

            double DC_time_from_profile=0;

            double DC_window_start = 0;
            double DC_window_end = 0;

            double DC_theory_time = 0;

            int index_at_start;
            Window start_window;
            Window end_window;

            //Console.WriteLine("ride number {0}", RideNumber);

            for (int i = 0; i < list_of_sections.Count; i++)
            {
                if (list_of_sections[i].what_is_the_type_of_this_section() == typeof(DC_section))
                {
                    (int DC_start_node, int DC_end_node) = list_of_permutations[i].Get_start_and_end_nodes();
                    
                    // teoretyczny czas startu
                    // teoretyczny czas konca

                    start_window = list_of_entrance_windows[i];
                    end_window = list_of_exit_windows[i];

                    DC_window_start = start_window.get_time_of_ride_at_win(RideNumber);
                    DC_window_end = end_window.get_time_of_ride_at_win(RideNumber);

                    DC_theory_time = DC_window_end - DC_window_start;


                    // start time at win
                    // position in win
                    // speed at win

                    // teoretyczny czas przejazdu


                    // czas startu wedlug profilu
                    // czas konca wedlug profilu

                    // praktyczny czas przejazdu

                    for (int j = 0; j < ride_profile.Count; j++)
                    {
                        if (ride_profile[j][5] == DC_start_node)
                        {
                            DC_profile_start = ride_profile[j][7];
                            break;
                        }
                    }

                    for (int j = ride_profile.Count - 1; j >= 0; j--)
                    {
                        if (ride_profile[j][6] == DC_end_node)
                        {
                            DC_profile_end = ride_profile[j][8];
                            break;
                        }
                    }

                    DC_time_from_profile = DC_profile_end - DC_profile_start;

                    if (Math.Abs(DC_time_from_profile - DC_theory_time) > 0.0001)
                    {
                        Console.WriteLine("ride number {0}", RideNumber);
                        Console.WriteLine("time theory : {0}  profile : {1} diffrence : {2} index {3} max_index {4} start station {5} end_station {6}", DC_theory_time, DC_time_from_profile, DC_theory_time - DC_time_from_profile, i, list_of_permutations.Count - 1, start_station,end_station);
                        return false;

                    }
                    //Console.WriteLine("time theory : {0}  profile : {1} diffrence : {2}", DC_theory_time, DC_time_from_profile, DC_theory_time - DC_time_from_profile);
                }
            }

            return true;

        }

        public double debug_time_convert_to_ride_time(double time)
        {
            return time -= start_time_of_begining_first_traverse_in_global_time + shift_at_start_because_of_position_in_traverse;
        }

        /// <summary>
        /// time = time form begining of ride
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public double get_distance_at_time_from_profile(double time)
        {
            // sprawdam w ktorej sekcji pedzie kapsula dla danego czasu
            // profil podaje czas dla kazdego tracku, wiec jest to formalnosc 
            // licze time into track, roznica od zadanego czasu do poczatku tracu
            
            //jest problem bo jak mam czas rowny dlugosci przejazdu to wywala mi -1 

            if(time == ride_profile[ride_profile.Count-1][8])
                return ride_profile[ride_profile.Count-1][1];


            //int affected_part_of_profile=-1;
            double time_into_affected_part_of_profile = -1;
            double time_to_end_of_part_of_profile = -1;
            //int charateristic_node_number;
            double start_distance =-1;
            double end_distance = -1;

            for (int i = 0; i < ride_profile.Count; i++)
            {
                if (time < ride_profile[i][8])
                {
                    //affected_part_of_profile = i;
                    time_into_affected_part_of_profile = time - ride_profile[i][7];
                    time_to_end_of_part_of_profile = ride_profile[i][8] - time;

                    start_distance = ride_profile[i][0];
                    end_distance = ride_profile[i][1];

                    //transfer
                    if (ride_profile[i][4] == 0)
                    {
                        //licze ile przejedzie kapsola ze stala predkoscia wzgledem tego co bylo na poczatku
                        return start_distance + ride_profile[i][2] * time_into_affected_part_of_profile;
                    }
                    //acceleration
                    if (ride_profile[i][4] == 1)
                    {
                        //licze ile sie przemiescima kapsola przy przyspieszaniu wzgledem tego co bylo na poczatku
                        return start_distance + ride_profile[i][2] * time_into_affected_part_of_profile + 0.5 * ride_profile[i][3] * Math.Pow(time_into_affected_part_of_profile, 2);
                    }
                    //brake
                    if (ride_profile[i][4] == -1)
                    {
                        //licze ile sie przemiesci kapsola przy hamowaniu wzgledem tego co bylo na koncu hamowania
                        return end_distance - ride_profile[i][2] * time_to_end_of_part_of_profile - 0.5 * ride_profile[i][3] * Math.Pow(time_to_end_of_part_of_profile, 2);

                    }
                }
            }

            return -1;

            // odczytuje node charakterystyczny danego fragmentu
            // zczytuje wspolzedne nodu charakterystycznego      

            // licze przesuniecie w danym czesci profilu
                // sprawdzam rodzaj tracku, przyspieszenie, hamowanie, constant  
                // i na podstawie tego licze delte wzgledem nouda charakterystycznego 
        }

        /// <summary>
        /// distance = distance into route
        /// </summary>
        /// <param name="distance"></param>
        private (float, float) get_cordinates_at_distance(double distance)
        {
            
            // ktora to sekcja 
            var (index, distance_into_affected_section) = get_index_of_section_and_distance_into_affected_section_at_distance(distance);


            // ktory to track modified_route
            var (track, distance_into_track) = get_number_of_track_and_distance_into_it_of_modified_route_at_distance(distance);


            if (list_of_sections[index].what_is_the_type_of_this_section() == typeof(DC_section))
            {
                //odwoluje sie do tracku i podaje wspolrzedne
                var (pos_x, pos_y) = ModifiedCity.calculate_position_of_point_at_x_from_start_of_track(track, distance_into_track); 
                return (pos_x, pos_y);
            }


            if (list_of_sections[index].what_is_the_type_of_this_section() == typeof(Sort_section))
            {

                //odwoluje sie do tracku i wyliczam wspolrzedne
                var (pos_x, pos_y) = ModifiedCity.calculate_position_of_point_at_x_from_start_of_track(track, distance_into_track);


                // zaleznie od miejsca w sort sekcji
                // dodaje odpowiednia wartosc prostopadle do toru
                // musze znac dlugosc sekcji zmiany toru, jest zdefiniowana w obliczeniach sort
                // odczytuje kat w punkcie 
                // w zelaznasci od odleglosci dodaje wartosc, 


                //narzie bez korekcji, jako test 
                return (pos_x, pos_y);
            }
            return (-1, -1);
        }

        private (float, float , float) get_cordinates_and_angle_at_distance(double distance)
        {
            // tu jest czasami blad bo przychodzi zapytanie o dystans ktorego niema wedlug affected sections
            // oznacza to ze bledy numeryczne sie kumuluja i odleglosci distance into section sie niezgadza 

            // ktora to sekcja i dystans
            var (index, distance_into_affected_section) = get_index_of_section_and_distance_into_affected_section_at_distance(distance);


            // ktory to track modified_route
            var (track, distance_into_track) = get_number_of_track_and_distance_into_it_of_modified_route_at_distance(distance);


            if (list_of_sections[index].what_is_the_type_of_this_section() == typeof(DC_section))
            {
                //odwoluje sie do tracku i podaje wspolrzedne
                var (pos_x, pos_y, ang) = ModifiedCity.calculate_position_and_angle_of_point_at_x_from_start_of_track(track, distance_into_track);
                return (pos_x, pos_y, ang);
            }


            if (list_of_sections[index].what_is_the_type_of_this_section() == typeof(Sort_section))
            {
                float translation = 0;
                float max_side_translation = (float)list_of_permutations[index].get_entrance_speed()/3;
                // odwoluje sie do tracku i wyliczam wspolrzedne
                var traverse = list_of_traverse[index];

                var permutation_length = list_of_permutations[index].get_permutation_length();
                var length_of_track_change = list_of_permutations[index].get_entrance_speed() * 1;
                var length_of_traverse = list_of_permutations[index].get_current_length_of_traverse();

                // odczytuje predkosc tej sekcji
                // odczytuje dlugosc tej sekcji
                // wyliczam odlegolosc pierwszej zmiany pasa
                // wyliczam odleglosc drugiej zmiany pasa 



                // sprwadzam czy to jest pierwsze czy drugie okno na wyjsciu  
                // odczytuje traverse tej sekcji

                var exit_win_of_current_section = list_of_exit_windows[index];
                var active_exit_winds = traverse.get_list_af_active_exit_windows();

                //int index_of_exit_win_in_traverse;

                //if (active_exit_winds[0].number == exit_win_of_current_section.number)
                //    index_of_exit_win_in_traverse = 0;
                //else
                //    index_of_exit_win_in_traverse = 1;
                

                //to jest baza podejmowania decyzji jaka scierzka z dostepnych dla sort bedzie jechac dany pod
                //pierwsze okno > prawa strona (niepamietam czy to jest dobra strona)
                //drugie okno > lewa strona
                //trzecie okno > srodek
                var index_of_window = return_index_of_window_in_list_of_active_traverse_exit_windows(index);
                

                float angle_change;

                var (pos_x, pos_y, ang) = ModifiedCity.calculate_position_and_angle_of_point_at_x_from_start_of_track(track, distance_into_track);

                // tu jest blad bo powinno byc wiecej sekcj
                // jeszcze na poczatku i na koncu jest sekcja o dlugosci traversu gdzie niedzieje sie nic
                // bo kapsuly czakaja az 


                // first distance where traverse enters the sort
                if (distance_into_affected_section < length_of_traverse)
                {
                    translation = 0f;
                    angle_change = 0f;
                }

                //first change of sort
                else if (distance_into_affected_section < length_of_track_change + length_of_traverse)
                {
                    translation = (float)(((distance_into_affected_section - length_of_traverse) / length_of_track_change) * max_side_translation);
                    angle_change = (float)(360 * Math.Atan(max_side_translation / length_of_track_change) / (2 * Math.PI));
                }

                //sorting
                else if (distance_into_affected_section <= permutation_length - length_of_track_change - length_of_traverse)
                {
                    translation = max_side_translation;
                    angle_change = 0;
                }
                
                //second change of line, going back to original position
                else if(distance_into_affected_section <= permutation_length - length_of_traverse)
                {
                    translation = (float)(((permutation_length - distance_into_affected_section - length_of_traverse) / length_of_track_change) * max_side_translation);
                    angle_change = -(float)(360 * Math.Atan(max_side_translation / length_of_track_change) / (2 * Math.PI));
                }
                //last section where pod are exiting sort
                else 
                {
                    translation = 0f;
                    angle_change = 0f;
                }


                float translation_x;
                float translation_y;
                float translation_angle = 0;

                if (index_of_window == 0)
                    translation_angle = ang + 90;
                if (index_of_window == 1)
                    translation_angle = ang - 90;

                translation_x = ((float)Math.Cos((translation_angle / 360) * (2 * Math.PI))) * translation;
                translation_y = ((float)Math.Sin((translation_angle / 360) * (2 * Math.PI))) * translation;

                if (index_of_window == 0)
                    return (pos_x + translation_x, pos_y + translation_y, ang + angle_change);
                else if (index_of_window == 1)
                    return (pos_x + translation_x, pos_y + translation_y, ang - angle_change);
                else
                    //dla 3 sciezki kapsola jedzie srodkiem, bez translacji
                    return (pos_x, pos_y, ang);


            }
            return (-1, -1, -1);
        }

        /// <summary>
        /// Ta metoda dziala uniwersalnie, niezaleznie od ilosci grup na jakie sortuje dana sekcja
        /// </summary>
        /// <param name="index_of_ride_section"></param>
        /// <returns></returns>
        public int return_index_of_window_in_list_of_active_traverse_exit_windows(int index_of_ride_section)
        {
            int index = 0;

            
            var traverse = list_of_traverse[index_of_ride_section];
            var exit_win = list_of_exit_windows[index_of_ride_section];


            var list_of_traverse_exit_win = traverse.get_exit_windows_list();


            for (int i = 0; i < list_of_exit_windows.Count; i++)
            {

                if (list_of_traverse_exit_win[i] == exit_win)
                {
                    return index;
                }
                else
                {
                    if (list_of_traverse_exit_win[i].window_is_deactivated)
                        continue;
                    else
                        index++;
                }

            }
            return index;

        }

        public (int, double) get_index_of_section_and_distance_into_affected_section_at_distance(double distance)
        {
            double remaining_distance = distance;

            for(int i= 0; i < list_of_permutations.Count; i++)
            {
                if (remaining_distance < list_of_permutations[i].get_permutation_length())
                    return (i, remaining_distance);
                else
                    remaining_distance -= list_of_permutations[i].get_permutation_length();
            }


            //to stanowi zabezpieczenie przed bledami numerycznymi 
            if (remaining_distance < 0.001)
                return (list_of_permutations.Count-1, list_of_permutations[list_of_permutations.Count - 1].get_permutation_length());


            return (-1, -1);
        }
        
        public (int, double) get_number_of_track_and_distance_into_it_of_modified_route_at_distance(double distance)
        {
            double remaining_dis = distance;


            for(int i = 0; i < ModifiedRoute.GetLength(0); i++)
            {
                if (remaining_dis < ModifiedCity.GetLenghtOfTrack(ModifiedRoute[i][2]))
                    return (ModifiedRoute[i][2], remaining_dis);
                else
                    remaining_dis -= ModifiedCity.GetLenghtOfTrack(ModifiedRoute[i][2]);

            }

            return (-1, -1);
        }
        
        public void add_row_to_position_data(float time, float pos_x, float pos_y, float ang, bool active)
        {
            position_data.add_position(time, pos_x, pos_y, ang, active);
        }

        public void gen_position_data_description()
        {
            position_data.gen_data_description();
        }

        public (float, float, float, int) return_position_data_description()
        {
            return position_data.return_data_description();
        }

        public (float[], bool) return_position_data__and_activation_at_index(int index)
        {
            return position_data.return_position_data_point_at_index(index);
        }

        public bool return_activation_state_at_index_of_position_data(int index)
        {
            return position_data.return_activation_state_at_index_of_position_data(index);
        }

        public float[] return_positon_data_at_index(int index)
        {
            return position_data.return_position_data_at_index(index);
        }

        public void clear_position_data()
        {
            position_data.clear_position_data();
        }


    }





    public struct mod_data_for_section
    {
        public mod_type mod_type { get; set; }
        public double dist_from_start_at_entrance_in_traverse { get; set; }
        public double distance_from_start_at_exit_in_traverse { get; set; }

        public double dist_from_start_of_mod_beginning_in_permutation { get; set; }
        public double dist_from_start_of_mod_end_in_permutation { get; set; }


    }


    public enum mod_type
    {
        DC_section,
        sort_section
    }

    public class ride_position_data
    {
        List<(float[], bool)> position_data;

        float start_time;
        float end_time;
        float delta_T;
        int number_of_position_points;

       

        public void clear_position_data()
        {
            position_data.Clear();
        }

        public ride_position_data()
        {
            position_data = new List<(float[], bool)>();
        }
            
        public void add_position(float time, float pos_x, float pos_y, float ang, bool active)
        {
            var raw = (new float[] {time, pos_x, pos_y, ang}, active);
            position_data.Add(raw);
        }


        public void gen_data_description()
        {
            start_time = position_data[0].Item1[0];
            end_time = position_data[position_data.Count - 1].Item1[0];

            delta_T = position_data[1].Item1[0] - position_data[0].Item1[0];
            number_of_position_points = position_data.Count;
        }

        public (float,float,float,int) return_data_description()
        {
            return (start_time, end_time, delta_T, number_of_position_points);
        }

        public (float[], bool) return_position_data_point_at_index(int index)
        {
            return position_data[index];
        }

        public bool return_activation_state_at_index_of_position_data( int index)
        {
            return position_data[index].Item2;
        }

        public float[] return_position_data_at_index(int index)
        {
            return position_data[index].Item1;
        }
    }

}
