using System;
using System.Collections.Generic;

namespace Symulation
{
    public class Symulation_control
    {
        private CityDataStorage city;

        private RepositoryOfSections RepositoryOfSections;
        private ride_data_repository ride_repositiory;
        //private MenagementOfCitySections MenagementOfSections;
        private LibraryOfSectionsForCity libray_of_sections;
        private ride_request_generation ride_request;
        private RepositoryOfPermutations list_of_permutations;
        private RepositoryOfSynchronization repository_of_synchronization;
        private managment_of_traverse_interactions menagment_of_interactions;
        private SearchForRoutesMain search;
        private DetailsOfRoute route_detail;
        private double sort_traverse_length;
        private double DC_length;
        private readonly double shift;
        private symulation_data sym_data;

        public Symulation_control(CityDataStorage city, List<map_module> list_of_modules, double sort_traverse_length, double shift, double DC_length)
        {
            this.city = city;

            ride_repositiory = new ride_data_repository(city);

            //MenagementOfSections = new MenagementOfCitySections(city);
            libray_of_sections = new LibraryOfSectionsForCity(city);

           // (RepositoryOfSections, list_of_permutations, repository_of_synchronization) = libray_of_sections.sekcje_rozgalezienie_DC(sort_traverse_length, shift);
            (RepositoryOfSections, list_of_permutations, repository_of_synchronization) = libray_of_sections.Translate_city_modules_to_sections(list_of_modules, sort_traverse_length,DC_length);

            menagment_of_interactions = new managment_of_traverse_interactions(RepositoryOfSections, list_of_permutations, city);

            search = new SearchForRoutesMain(city);
            route_detail = new DetailsOfRoute(city, RepositoryOfSections);
            this.sort_traverse_length = sort_traverse_length;
            this.DC_length = DC_length;
            this.shift = shift;
        }

        // tu jest wyzwanie takie ze niewiem gdzie jest liczony config calosci
        /// <summary>
        /// Currently I turned of branching of search threads. Because the is a problem with deep copy method during that process.
        /// Turning it on, to see if it works.
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="finish_time"></param>
        /// <param name="probability"></param>
        /// <param name="speed"></param>
        public ride_data_repository run_symulation_v1(double start_time, double finish_time, double probability, double speed)
        {
            var time1 = DateTime.Now;
            // gdzies jest blad powodujacy ze niektore okna sort sekcji niemaja traversu wejsciowego i wyjsciowego 
            // blad jest prawdopodobnie popelniany na poziomie tworzenia interakcji
            // prawdopodobnie na poziomie tworzenia rownoleglych okien dla sekcji DC , przy tworzeniu nowych okien , niedodalem trawersow
            sym_data = new symulation_data();

            double time = start_time;
            double delta = 0.01;
            List<request> ride_request_list;
            //double probability = 0.003;

            ride_request = new ride_request_generation(city, probability);

            // creations of station managment objects. 
            int licznik_przejazdow = 0;
            while (true)
            {
                // cycle

                // add time
                time = time + delta;
                // check and add interactions (missing deactivation of some of sort section exit windows)
                // there is stored time of end of last calculation if exceded, calculate more
                // currently simplified version, which works only once go requested period of time
                //tu generuje wszystkie requesty 
                if (sym_data.is_generation_of_new_interations_necessary(time))
                {
                    //generating interactions
                    // config is calculated earlier at the level of city sections creation 
                    generate_interactions(46000, 2, 0.5); // seconds of interactions generated[m], length of pod, distance between pods[m]
                }


                //[0]// 1h+		this	{Symulation.Symulation_control}	Symulation.Symulation_control
                // tutaj generuje zapytania o przejazd
                // narzie dzieje sie to kompletnie przypadkowo 
                ride_request_list = ride_request.ride_request_cycle(time);
                
                // w tym cyklu bedzie zapytanie o nowy przejezd 
                // na podstawie losawania w poprzednim kroku
                if(ride_request_list.Count != 0)
                {
                    //Console.WriteLine("ride request: start {0}, finish {1}", ride_request_list[0].Start_station_number, ride_request_list[0].End_station_number);

                    // tutaj powinna byc bardziej rozbudowana metoda pozwalajaca na szukanie nietylko samego przejazdu
                    // ale tez kapsuly do tego przejazdu, miejsca w stacji startowej, miejsca w stacji koncowej, itd.
                    // narazie jest uproszczona
                    // to raczej powinny byc dodatkowe metody ktore razem twoza jedna calosc coraz bardziej rozbudowana
                    // ta metoda jest zupelnie ok , inne funkcjonalnosci powinny obslugiwac reszte np czy kapsuly sa dostepne


                    var (search_effect, was_search_succesfull) = ride_search(ride_request_list[0], finish_time);


                    // ten fragment tez bedzie czescia bardziej rozbudowanej metody szukania przejazdu z wyszukiawniem 
                    // dostepnej kapsoly, itd
                    if (was_search_succesfull)
                    {
                        //add_ride_without_ordering_and_modifications(search_effect);
                        add_ride_with_ordering_and_mod_data_change(search_effect);

                        licznik_przejazdow ++;
                    }
                    


                }

                // stop time cycles
                if (time > finish_time)
                    break;
            }


            //Console.WriteLine((time2 - time1).TotalSeconds);
            
            Console.WriteLine("czas symulacji [s]: {0} ilosc przejazdow: {1}  probability: {2} shift: {3} sort traverse: {4} DC traverse {5}", finish_time - start_time, licznik_przejazdow, probability, shift, sort_traverse_length , DC_length);

            return ride_repositiory;
              
        }

        public (ride_search_thread , bool was_search_succesfull) ride_search(request request, double end_time)
        {
            //list of exit windows creates error
            //first window is duplicated

            // missing check if window is active
            // there will be internal info in window, that it is inactive 
            // method checking if there is space available, returns no in such case
            int start_station = request.Start_station_number;
            int end_station = request.End_station_number;
            double request_time = request.request_time;

            // ok to jest sama droga pomiedzy wezlami
            // on zawiera tylko numery wezlow po drodze i numery trackow 
            int[][][] routes = search.SeekRouteBetweenStations(start_station, end_station);
            var route = routes[0];
            List<ride_search_thread> threads_list = new List<ride_search_thread>();
            

            // tworze (jesli zrobie tabele to bedzie tylko wczytywanie)
            // liste sekcji i permutacji ktore bede mijac po drodze
            var (list_of_sections, list_of_permutations) = route_detail.prepare_description_of_route(route);

            // twoze liste trawersow
            // czyli jest tak ze search odbywa sie w przestrzeni okien itp pomiedzy roznymi sekcjami
            // liczba oznacza liczbe traversow, wiec tak naprawde moze byc wiecej okien 
            create_ride_search_threads(request_time, list_of_permutations[0], route, 2); 
            // algorytm szukania przejazdu
            int licznik = 0;
            while (threads_list.Count > 0)
            {
                if(check_ride_search_thread(threads_list[0]) == false)
                {
                    threads_list.RemoveAt(0);
                }
                else
                {
                    //Console.WriteLine("znaleziono przejazd: {0}",licznik);
                    
                    return (threads_list[0], true);
                    //threads_list.RemoveAt(0);
                    //licznik++;
                    //break;
                }
            }
            return (new ride_search_thread(), false);


            bool check_ride_search_thread(ride_search_thread thread)
            {
                //odczytuje index postepu danego watku
                
                Traverse current_traverse;
                Window current_traverse_entrance_window;
                Window current_traverse_exit_window;
                Permutation next_permutation;
                List<interaction_window> list_of_windows_with_access;

                

                while (true)
                {
                    //index oznaczajacy sekcje po drodze kapsoly 
                    int progress_index = thread.index_of_pregress;
                    Type type_of_section = thread.get_type_of_section_for_index(progress_index);

                    // sprawdzam co to za sekcja:

                    // jesli sekcja DC
                    // sprwadzanie polega na sprawdzeniu ile jest jeszcze wolnego miejsca w oknie (czy min 1 miejsce jeszcze jest) ktore jest rozwazane jako przejazd
                    // sprawdzam czy dane okno laczy sie z permutacja ktora jest nastepna na liscie plan przejazdu
                    // jesli tak up date watku , przechodze do nastepnej sekcji lub koncze jesli to byla ostatnia sekcja 
                    if (typeof(DC_section) == type_of_section)
                    {
                        current_traverse_entrance_window = thread.get_entrance_window_for_index(progress_index);
                        if (current_traverse_entrance_window.is_there_space_available())
                        {
                            current_traverse = thread.get_traverse_by_index(progress_index);
                            //tu sie wywala algorytm 
                            current_traverse_exit_window = get_exit_window_paraller_to_entrance_window(current_traverse_entrance_window, current_traverse);

                            // exit window for current section (use of data within windows or traverse)
                            // tu jest cos zle ze jest dodawane okno exit 2 razy to samo
                            // proble jest nie tylko przy prierwszym przejsciu ale tez jak odpalam odgalezienie watku
                            // wtedy efekt jest taki ze ze mam dwa dwa takie same okna na starcie w liscie exit window
                            // w liscie antrance window jest ok, i ilosc entrance zgadza sie z iloscia sekcji a exit nie
                            // moze niepowinienem dodawac exit windows podczas tworzenia watku szukania
                            thread.add_exit_window(current_traverse_exit_window);

                            if (progress_index == 0)
                            {
                                if (current_traverse.begining_start_time > end_time)
                                    return false;
                            }



                            if (thread.is_this_last_section(progress_index))
                            {
                                // koniec szukania
                                // ten watek jest zwracany jako ten ktory 
                                // zakonczyl sie sukcesem
                                return true;
                            }
                            else
                            {
                                // entrance window for next section (to samo co exit window , poprzedniego okna)
                                thread.add_entrance_window(current_traverse_exit_window);
                                // next section traverse (na bazie exit window)
                                var exit_win = (interaction_window)current_traverse_exit_window;
                                thread.add_traverse(exit_win.traverse_with_entrance_at_this_window);

                                //powiekszam index
                                thread.add_one_to_progress();

                                //przechodze do nastepnej petli 
                                continue;
                            }
                        }
                        else
                        {
                            //usuwam ten watek szukania bo niema juz miejsca na kolejna kapsule
                            return false;
                        }

                    }

                    else if (typeof(Sort_section) == type_of_section)
                    {
                        current_traverse_entrance_window = thread.get_entrance_window_for_index(progress_index);

                        //to jest permutacja do ktorej dany przejazd chce sie dostac po przejechaniu przez sort sekcje
                        //tego komponentu niema w DC bo tam poprostu 
                        next_permutation = thread.get_next_permutation_on_route(progress_index);
                        current_traverse = thread.get_traverse_by_index(progress_index);

                        // robie liste okien do ktorych dany traverse mozne wyjechac na wyjsciu z sekcji
                        // i dojechac do kolejnej permutacji in route
                        // bez uwzgledniania aktywnosci danych okien (to jest sprawdzane w kolenym kroku)
                        list_of_windows_with_access = get_list_windows_capable_of_accesing_permutation(current_traverse, next_permutation);
                        if (list_of_windows_with_access.Count == 0)
                        {
                            //usuwam ten watek bo nieda sie dojachac do permutacji docelowej
                            return false;
                        }
                        // da sie dojechac na jeden sposob 
                        else if (list_of_windows_with_access.Count == 1)
                        {
                            current_traverse_exit_window = list_of_windows_with_access[0];
                            if (current_traverse_entrance_window.is_there_space_available() && current_traverse_exit_window.is_there_space_available())
                            {
                                thread.add_exit_window(current_traverse_exit_window);

                                //uzupelniam watek i przechodze do nastepnego kroku
                                // entrance window for next section (to samo co exit window , poprzedniego okna)
                                thread.add_entrance_window(current_traverse_exit_window);
                                // next section traverse (na bazie exit window)
                                var exit_win = (interaction_window)current_traverse_exit_window;
                                thread.add_traverse(exit_win.traverse_with_entrance_at_this_window);

                                //powiekszam index
                                thread.add_one_to_progress();

                                //przechodze do nastepnej petli 
                                continue;
                            }
                            else
                                return false;
                        }

                        // da sie dojechac do permutacji docelowej na wiecej niz jeden sposob 
                        else if (list_of_windows_with_access.Count > 1)
                        {
                            // twoze nowy watek dla drugiego i kolejnych okien (narzie niejest to mozliwe zeby bylo wiecej niz dwa, ale jak zrobie sekcje sortujace na 3 to juz bedzie inaczej)
                            // w ramach tworzenia uzupelniam watki danymi kolejnych okien 
                            // dodaje nowe watki naraz po obecnym watku na liscie watkow w szukaniu
                            // tu jest blad w ustalaniu pozucji nowego watku wzgledem innych watkow serach
                            // dodam nowa metode 

                            for (int i = 1; i < list_of_windows_with_access.Count; i++)
                            {
                                var exit_window = list_of_windows_with_access[i];
                                if (current_traverse_entrance_window.is_there_space_available() && exit_window.is_there_space_available())
                                {

                                    //co powinienem zrobic zeby moc dodac nowe okno jako alternatywe w watku
                                    // musze sprawdzic czy to okno ma jeszcze miejsce, to jest ok
                                    // chce dodac dane do thread tak zeby zapisac to ze juz jestem na kolejnym etapie sprawdzania

                                    add_new_thread_branch(thread, i, exit_window);

                                }
                                else
                                    continue;

                            }

                            // uzupelniam obecny watek danymi z pierwszego okna
                            current_traverse_exit_window = list_of_windows_with_access[0];
                            if (current_traverse_entrance_window.is_there_space_available() && current_traverse_exit_window.is_there_space_available())
                            {

                                thread.add_exit_window(list_of_windows_with_access[0]);
                                // entrance window for next section (to samo co exit window , poprzedniego okna)
                                thread.add_entrance_window(list_of_windows_with_access[0]);
                                // next section traverse (na bazie exit window)
                                
                                Window win1 = list_of_windows_with_access[0];
                                thread.add_traverse(list_of_windows_with_access[0].traverse_with_entrance_at_this_window);

                                //powiekszam index
                                thread.add_one_to_progress();

                                //przechodze do nastepnej petli
                                continue;
                            }
                            else
                                return false;

                        }

                    }

                }

                
            }


            void add_new_thread_branch(ride_search_thread _Thread, int index_on_list, interaction_window new_exit_window)
            {
                
                var new_thread = _Thread.deep_copy();

                //missing section where current thread is chenged by adding info about current traverse
                
                new_thread.add_exit_window(new_exit_window);
                
                // entrance window for next section (to samo co exit window , poprzedniego okna)
                new_thread.add_entrance_window(new_exit_window);
                // next section traverse (na bazie exit window)
                
                
                new_thread.add_traverse(new_exit_window.traverse_with_entrance_at_this_window);

                //powiekszam index
                new_thread.add_one_to_progress();

                // tu mi wywalalo blad bo jest za duzy index
                // o co chodzi
                // mam jeden obiekt na liscie
                // a sytem probuje wstawic kolejny na miejscu 2 . czemu ? to niema sensu przecierz
                // dlaczego ja tak wlasciwie to wstawiam na okreslonym miejscu ?
                // chodzi o to zeby dany thread byl w kolejnosci czasu startu wzgledem innych tredow ?
                // bo jesli tak to powinienm to zrobic w inny sposob
                // to jak mam to teraz wyglada jak by dzialalo przez przypadek wczesniej bo niebylo duzo opcji na wyjsciu
                threads_list.Add(new_thread);
                //threads_list.Insert(index_on_list, new_thread);

                threads_list.Sort((p, q) => p.get_start_time().CompareTo(q.get_start_time()));
            }

            Window get_exit_window_paraller_to_entrance_window(Window entrance_window, Traverse _traverse)
            {
                var list_of_entrance_windows = _traverse.get_entrance_windows_list();
                var list_of_exit_windows = _traverse.get_exit_windows_list();
                int index;

                for(int i = 0; i < list_of_entrance_windows.Count; i++)
                {
                    if (list_of_entrance_windows[i] == entrance_window)
                        return list_of_exit_windows[i];
                }
                return null;
            }
            List<interaction_window> get_list_windows_capable_of_accesing_permutation(Traverse current_traverse, Permutation next_permutation)
            {
                var list_of_exit_windows = current_traverse.get_exit_windows_list();
                List<interaction_window> list_of_windows_capable_of_accesing_permutation = new List<interaction_window>();
                interaction_window exit_window;
                for (int i = 0; i < list_of_exit_windows.Count; i++)
                {
                    exit_window = (interaction_window)list_of_exit_windows[i];
                    if(exit_window.permutation_which_start_at_this_window == next_permutation.get_number_of_permutation())
                    {
                        list_of_windows_capable_of_accesing_permutation.Add(exit_window);
                    }
                }

                return list_of_windows_capable_of_accesing_permutation;
            }


            void create_ride_search_threads(double _request_time, Permutation permutation,int[][] thread_route, int number_of_search_threads)
            {
                List<Window> entrance_windows;
                List<Window> exit_windows;
                double min_margin = 5; // 5[s] 
                var list_of_traverses = permutation.get_list_of_traverses();
                var index_of_first_traverse_after_request_time = index_of_first_traverse_after_time(_request_time, list_of_traverses);
                ride_search_thread thread;


                for (int i = index_of_first_traverse_after_request_time; i < index_of_first_traverse_after_request_time + number_of_search_threads; i++)
                {
                    entrance_windows = list_of_traverses[i].get_entrance_windows_list();
                    exit_windows = list_of_traverses[i].get_exit_windows_list();
                    for(int j = 0; j < entrance_windows.Count; j++)
                    {
                        thread = new ride_search_thread();

                        thread.start_station = start_station;
                        thread.start_node = city.get_node_attached_to_station(start_station);
                        thread.end_station = end_station;
                        thread.end_node = city.get_node_attached_to_station(end_station);

                        thread.add_traverse(list_of_traverses[i]);
                        thread.add_start_time(list_of_traverses[i].begining_start_time);
                        thread.add_entrance_window(entrance_windows[j]);
                        //exit win jest dodawane juz w algorytmie sprawdzania przejazdu
                        //thread.add_exit_window(exit_windows[j]);
                        thread.add_permutation_list(list_of_permutations);
                        thread.add_section_list(list_of_sections);

                        thread.add_route(thread_route);

                        threads_list.Add(thread);
                    }
                    
                }


            }
            int index_of_first_traverse_after_time(double time, List<Traverse> traverses)
            {
                for(int i = 0; i < traverses.Count; i++)
                {
                    if(traverses[i].begining_start_time>time)
                        return i;
                }
                return -1;

            }
            
        }


        /// <summary>
        /// uproszczona metoda dodawania interakcji
        /// </summary>
        /// <param name="end_time"></param>
        /// <param name="pod_length"></param>
        /// <param name="min_distance"></param>
        public void generate_interactions(double end_time, double pod_length, double min_distance)
        {
            //Console.WriteLine("faza 1");
            menagment_of_interactions.create_traverses_and_calculate_boundaries_of_traverses(end_time);

            //Console.WriteLine("faza 2");
            menagment_of_interactions.calculate_interactions_between_boundaries_of_traverses(end_time - 3000);

            //Console.WriteLine("faza 3");
            menagment_of_interactions.split_windows_of_interactions_for_DC_section_to_be_parallel(end_time - 4000);

            //Console.WriteLine("faza 4");
            //tutaj sa generowane okna dla start and stop, bez modyfikacji sieci
            menagment_of_interactions.generate_windows_for_start_and_stop(end_time - 6000);

            //Console.WriteLine("faza 5");
            menagment_of_interactions.deactivate_excess_exit_for_sort_sections(end_time - 8000);

            //Console.WriteLine("faza 6");
            menagment_of_interactions.calculate_max_pod_number_for_windows_and_start_distance_in_traverse(end_time - 10000, pod_length, min_distance);

            sym_data.last_end_time_for_calculated_interactions = end_time;
        }

        public void generation_of_request()
        {


            //calculate probability that that request is generated 

            //calculate what request is generated 



        }

        public void search_for_ride()
        {
            //first wersion can be simple and dont care if there is pod available at start and space available at the end



        }

        public Ride add_ride_without_ordering_and_modifications(ride_search_thread sucessful_ride_search)
        {
            int number = ride_repositiory.get_number_of_of_next_free_ride_number();
            var ride = new Ride(number,city,sucessful_ride_search);
            
            ride_repositiory.add_new_ride(ride);

            add_ride_to_all_windows_along_the_way(ride);

            return ride;
            // add ride to all windows along the way

            // just adding to windows and ride repository
            // types of rides (people transfer, empty transfer)

            // changing order of pods afred adding new ride

            // defining mods of new ride
            // changing mods of affected rides 


            // new mods are needed . general location in DC traverse
            // sort section mod
            // both mods are done, but require testing 

            void add_ride_to_all_windows_along_the_way(Ride _ride)
            {
                var entrance_windows = _ride.get_entrance_windows();
                var exit_windows = _ride.get_exit_windows();

                for(int i = 0; i < entrance_windows.Count; i++)
                {
                    entrance_windows[i].add_ride_to_window(_ride);
                }

                exit_windows[exit_windows.Count - 1].add_ride_to_window(_ride);

            }
        }


        // jest blad liczenia indexsow jak wydluzam traversy sekcji dc
        // ten blad zmienia swoje polozenie, w losowy sposob
        // to znaczy ze zalezy od jakiegos konkretnego rozlozenia zapytan
        // na poczatek dowale ilosc zapytan - nic to niezmienia
        // zawsze jak jest blad to w oknie wyjsciowym sort sekcji jest zero na liczniku
        // moze algorytm niedodaje przejazdow do okna wyjsciowego sort sekcji, ale raczej nie bo w tym momencie sie wywala algorytm
        // albo liczenie kolejnosci na oknie wyjsciowym sort sekcji niedaje rady
        // powtarza sie to ze blad jest juz na pierwszej sort sekcji po drodze

        /// <summary>
        /// Method div into 2 sections, adding ride with ordering and def of mods 
        /// </summary>
        /// <param name="sucessful_ride_search"></param>
        public void add_ride_with_ordering_and_mod_data_change(ride_search_thread sucessful_ride_search)
        {
            var number = ride_repositiory.get_number_of_of_next_free_ride_number();
            int affected_ride_number;
            var ride_to_be_added = new Ride(number, city, sucessful_ride_search);
            int current_section_number;
            //moze to powinno byc w metodzie add, new ride, inaczej to jest troche zamet
            ride_repositiory.add_new_ride(ride_to_be_added);

            var list_of_sections_of_new_ride = ride_to_be_added.get_list_of_sections();
            var list_of_traverses_of_new_ride = ride_to_be_added.get_list_of_trraverses();
            var list_of_indices_of_new_ride_in_windows = go_along_route_and_establish_location_of_new_ride(ride_to_be_added);
            var list_of_permutations_of_new_ride = ride_to_be_added.get_list_of_permutations();


            Window permutation_entrance_window;
            int entrance_win_insert_index_of_new_ride;
            int number_of_rides_in_entrance_win;
            List<Ride> list_of_rides_thru_entrance_window;
            double dist_of_sorting_start_in_permutation=0;
            double dist_of_DC_start_in_permutation=0;


            Window permutation_exit_window;
            int exit_win_insert_index_of_new_ride;
            int number_of_rides_in_exit_win;
            List<Ride> list_of_rides_thru_exit_window;
            double dist_of_sorting_end_in_permutatiom=0;
            double dist_of_DC_end_in_permutation=0;

            mod_data_for_section mod;

            //add new ride with ordering of rides in windows
            //important to have that first before mods
            //calculation of mods require to have updated list of rides thru windows
            
            add_new_ride_to_windows_along_the_way(list_of_indices_of_new_ride_in_windows, sucessful_ride_search, ride_to_be_added);

            // mods
            // for new ride, i go along the route, and define mod for new ride and affected rides. Affected rides mod are overitten.
            // for each section at a time.
            // process is different for DC section and sort section
            
            for (int i = 0; i < list_of_sections_of_new_ride.Count; i++)
            {

                if (list_of_sections_of_new_ride[i].what_is_the_type_of_this_section() == typeof(DC_section))
                {
                    permutation_entrance_window = ride_to_be_added.get_entrance_window_at_section_index(i);
                    entrance_win_insert_index_of_new_ride = list_of_indices_of_new_ride_in_windows[i].index_of_new_ride;
                    number_of_rides_in_entrance_win = permutation_entrance_window.return_number_of_rides();
                    list_of_rides_thru_entrance_window = permutation_entrance_window.get_list_of_rides();

                    // teraz po kolei zmieniam mod data wszystkich new ride i wszystkich kolejnych dla okna entrance
                    // (dla DC to wystarczy zeby miec pewnosc ze to sa wszystkie ride jakie wymagaja zmiany, w oknie wejsciowym i wyjsciowym jest taki sam uklad)
                    for (int j = entrance_win_insert_index_of_new_ride; j < number_of_rides_in_entrance_win; j++)
                    {
                        // info do modu ride, mod ride jest we wspolrzednych traverse
                        // wyliczenie bazuje na indexsie kolejnosci w oknie plus polozenie okna w traversie
                        // tu musze wyciagnac numer ride ktory ma wprowadzane zmiany
                        affected_ride_number = permutation_entrance_window.get_number_of_ride_at_index(j);

                        if (permutation_entrance_window.what_is_the_type() == typeof(start_window))
                        {
                            current_section_number = list_of_sections_of_new_ride[i].Get_number_of_section();
                            var win = (start_window)permutation_entrance_window;

                            // tutaj odwoluje sie w kolko do jednej permutacji
                            // a powinienem odwolywac sie do kolejnych na liscie jadacych przez dane okno
                            
                            dist_of_DC_start_in_permutation = win.get_position_of_ride_in_csys_of_traverse_which_starts_at_win(affected_ride_number);

                            dist_of_DC_end_in_permutation = dist_of_DC_start_in_permutation;


                            mod = new mod_data_for_section();
                            mod.dist_from_start_at_entrance_in_traverse = dist_of_DC_start_in_permutation;
                            mod.distance_from_start_at_exit_in_traverse = dist_of_DC_end_in_permutation;//for DC section start and end distance is the same 
                            mod.mod_type = mod_type.DC_section;

                            list_of_rides_thru_entrance_window[j].add_mod_for_section(mod, current_section_number);
                        }

                        else if (permutation_entrance_window.what_is_the_type() == typeof(interaction_window))
                        {
                            current_section_number = list_of_sections_of_new_ride[i].Get_number_of_section();
                            var win = (interaction_window)permutation_entrance_window;
                            
                            dist_of_DC_start_in_permutation = win.get_position_of_ride_in_csys_of_traverse_which_starts_at_win(affected_ride_number);
                            dist_of_DC_end_in_permutation = dist_of_DC_start_in_permutation;

                            mod = new mod_data_for_section();
                            mod.dist_from_start_at_entrance_in_traverse = dist_of_DC_start_in_permutation;
                            mod.distance_from_start_at_exit_in_traverse = dist_of_DC_end_in_permutation; //for DC section start and end distance is the same 
                            mod.mod_type = mod_type.DC_section;

                            list_of_rides_thru_entrance_window[j].add_mod_for_section(mod, current_section_number);
                        }

                        // mody sa definiowane no podstawie poczatku DC, dlatego niema opcji exit windows 

                        // dla DC sekcji wystarczy info z okna startowego , bo pozycja na starcie i na koncu jest taka sama
                    }
                }

                // tutaj niema bledow, mody ride ktore maja zmiany maja prawidlowe zmiany
                // jest ten sam blad co dla DC, odnosze sie zawsze do numeru nowego ride wiec o dziwo inne Ride niemaja wprowadzanych zmian 
                // ciekawe jak to wogule dzialalo i czemu niewykrylem tego bledu
                else if (list_of_sections_of_new_ride[i].what_is_the_type_of_this_section() == typeof(Sort_section))
                {
                    permutation_entrance_window = ride_to_be_added.get_entrance_window_at_section_index(i);
                    permutation_exit_window = ride_to_be_added.get_exit_window_at_section_index(i);

                    entrance_win_insert_index_of_new_ride = list_of_indices_of_new_ride_in_windows[i].index_of_new_ride;
                    exit_win_insert_index_of_new_ride = list_of_indices_of_new_ride_in_windows[i + 1].index_of_new_ride;

                    number_of_rides_in_entrance_win = permutation_entrance_window.return_number_of_rides();
                    number_of_rides_in_exit_win = permutation_exit_window.return_number_of_rides();

                    list_of_rides_thru_entrance_window = permutation_entrance_window.get_list_of_rides();
                    list_of_rides_thru_exit_window = permutation_exit_window.get_list_of_rides();

                    double length_of_current_permutation = list_of_permutations_of_new_ride[i].get_permutation_length();
                    double length_of_current_traverse = list_of_traverses_of_new_ride[i].Size;
                    var V = list_of_permutations_of_new_ride[i].get_entrance_speed();
                    var change_of_lane_distance = V * 1; //number is coefficient 

                    // teraz po kolei zmieniam mod data new ride i wszystkich kolejnych dla okna entrance
                    for (int j = entrance_win_insert_index_of_new_ride; j < number_of_rides_in_entrance_win; j++)
                    {
                        affected_ride_number = permutation_entrance_window.get_number_of_ride_at_index(j);
                        //int index_of_affected_ride_in_entrance_win = 
                        int index_of_affected_ride_in_exit_win = get_index_of_ride(list_of_rides_thru_entrance_window[j], list_of_rides_thru_exit_window);
                        current_section_number = list_of_sections_of_new_ride[i].Get_number_of_section();
                        
                        
                        var common_entrance_win = (interaction_window)permutation_entrance_window;
                        var new_ride_exit_win = (interaction_window)permutation_exit_window;

                        int index_at_start = common_entrance_win.get_index_of_ride(affected_ride_number);
                        int index_at_end = new_ride_exit_win.get_index_of_ride(affected_ride_number);

                        dist_of_sorting_start_in_permutation = length_of_current_traverse - common_entrance_win.get_position_of_ride_in_csys_of_traverse_which_starts_at_win(affected_ride_number) + change_of_lane_distance;
                        if(j == entrance_win_insert_index_of_new_ride)
                            dist_of_sorting_end_in_permutatiom = length_of_current_permutation -  new_ride_exit_win.get_position_of_ride_in_csys_of_traverse_which_ends_at_win(affected_ride_number) - change_of_lane_distance;
                        
                        if(j==entrance_win_insert_index_of_new_ride)
                            mod = new mod_data_for_section();
                        else
                            mod = list_of_rides_thru_entrance_window[j].get_deep_copy_of_mod(current_section_number);
                        //copy existing mod for window 


                        mod.dist_from_start_of_mod_beginning_in_permutation = dist_of_sorting_start_in_permutation;
                        
                        if(j == entrance_win_insert_index_of_new_ride)
                            mod.dist_from_start_of_mod_end_in_permutation = dist_of_sorting_end_in_permutatiom;
                        
                        mod.dist_from_start_at_entrance_in_traverse = common_entrance_win.get_position_of_ride_in_csys_of_traverse_which_starts_at_win(affected_ride_number);
                        
                        if(j == entrance_win_insert_index_of_new_ride)
                            mod.distance_from_start_at_exit_in_traverse = new_ride_exit_win.get_position_of_ride_in_csys_of_traverse_which_ends_at_win(affected_ride_number);

                        mod.mod_type = mod_type.sort_section;

                        list_of_rides_thru_entrance_window[j].add_mod_for_section(mod, current_section_number);


                    }

                    // teraz po kolei zmieniam mod data wszystkich new ride i wszystkich kolejnych dla okna exit
                    for (int j = exit_win_insert_index_of_new_ride; j < number_of_rides_in_exit_win; j++)
                    {
                        affected_ride_number = permutation_exit_window.get_number_of_ride_at_index(j);
                        //int index_of_affected_ride_in_entrance_win = get_index_of_ride(list_of_rides_thru_exit_window[j], list_of_rides_thru_entrance_window);
                        current_section_number = list_of_sections_of_new_ride[i].Get_number_of_section();

                        //tu jest blad bo niektore ride z okna exit moga miec inne okno entrance niz new ride algorytm sie wywala 
                        var new_ride_entrance_win = (interaction_window)permutation_entrance_window;
                        var common_exit_win = (interaction_window)permutation_exit_window;
                        
                        if(j == exit_win_insert_index_of_new_ride)
                            dist_of_sorting_start_in_permutation = length_of_current_traverse - new_ride_entrance_win.get_position_of_ride_in_csys_of_traverse_which_starts_at_win(affected_ride_number) + change_of_lane_distance;
                        
                        dist_of_sorting_end_in_permutatiom = length_of_current_permutation - common_exit_win.get_position_of_ride_in_csys_of_traverse_which_ends_at_win(affected_ride_number) - change_of_lane_distance;

                        if(j==exit_win_insert_index_of_new_ride)
                            mod = new mod_data_for_section();
                        else
                            mod = list_of_rides_thru_exit_window[j].get_deep_copy_of_mod(current_section_number);
                        
                        if(j == exit_win_insert_index_of_new_ride)
                            mod.dist_from_start_of_mod_beginning_in_permutation = dist_of_sorting_start_in_permutation;
                        
                        mod.dist_from_start_of_mod_end_in_permutation = dist_of_sorting_end_in_permutatiom;
                        
                        if(j == exit_win_insert_index_of_new_ride)
                            mod.dist_from_start_at_entrance_in_traverse = new_ride_entrance_win.get_position_of_ride_in_csys_of_traverse_which_starts_at_win(affected_ride_number);
                        
                        mod.distance_from_start_at_exit_in_traverse = common_exit_win.get_position_of_ride_in_csys_of_traverse_which_ends_at_win(affected_ride_number);

                        mod.mod_type = mod_type.sort_section;

                        list_of_rides_thru_exit_window[j].add_mod_for_section(mod, current_section_number);
                    }
                }
            }

            int get_index_of_ride(Ride _ride, List<Ride> _list_of_rides)
            {
                for(int i = 0; i < _list_of_rides.Count; i++)
                {
                    if (_list_of_rides[i] == _ride)
                        return i;
                }
                return -1;
            }

            void add_new_ride_to_windows_along_the_way(List<loc_of_new_ride> list_of_locations, ride_search_thread thred, Ride ride)
            {
                var list_of_windows_along_the_way = thred.get_all_windows_along_the_way();
                //var okno_z_indexem_2 = list_of_windows_along_the_way[2].number;

                for(int i = 0; i < list_of_windows_along_the_way.Count; i++)
                {
                    list_of_windows_along_the_way[i].add_ride_to_window_at_index(ride, list_of_locations[i].index_of_new_ride);
                }
            }
            
            //czsami search zwraca zawyzony index przejazdu przez sekcje sort
            //jest blad w exit window list , pierwszy komponent ma blad i powtarza sie z 2
            //wiec prawdopodobnie pierwsze okno ma blad 

            List<loc_of_new_ride> go_along_route_and_establish_location_of_new_ride(Ride new_ride)
            {
                List<loc_of_new_ride> list_of_indices_in_windows_along_route_new_ride = new List<loc_of_new_ride>();
                loc_of_new_ride loc = new loc_of_new_ride();
                CitySection section;
                list_of_sections_of_new_ride = new_ride.get_list_of_sections();
                list_of_traverses_of_new_ride = new_ride.get_list_of_trraverses();
                var list_of_exit_windows = new_ride.get_exit_windows();


                //ogarniam pierwsze okno drogi

                int index_in_window = return_location_number_at_start_window(new_ride.get_start_window());
                //loc.ride_number = new_ride.get_number_of_ride();
                loc.window_number = new_ride.get_start_window().number;
                loc.ride_number = new_ride.get_number_of_ride();
                loc.index_of_new_ride = index_in_window;
                list_of_indices_in_windows_along_route_new_ride.Add(loc); 
                //jade po kolei po sekcjach i ogarniam wszystkie okna na wyjsciu z sekcji
                //po kazdym cyklu zapamietuje jaki byl wynik to jest podstawa do dzialania w nastepnym kroku
                // moze wystarczy tylko loc_of_new_ride ?
                for (int i = 0; i < list_of_sections_of_new_ride.Count; i++)
                {
                    section = list_of_sections_of_new_ride[i];
                    //jaka to sekcja 
                    if (section.what_is_the_type_of_this_section() == typeof(DC_section))
                    {
                        //transfer poprzedniego loc na okno wyjscia
                        loc = new loc_of_new_ride();
                        loc.window_number = list_of_exit_windows[i].number;
                        // dla okna wyjsciowego DC jest to kopia indexu z poprzedniego okna czyli okna wejsciowego
                        
                        loc.ride_number = new_ride.get_number_of_ride();
                        loc.index_of_new_ride = list_of_indices_in_windows_along_route_new_ride[i].index_of_new_ride;
                        
                        list_of_indices_in_windows_along_route_new_ride.Add(loc);

                    }
                    else if(section.what_is_the_type_of_this_section() == typeof(Sort_section))
                    {
                        loc = new loc_of_new_ride();
                        loc.window_number = list_of_exit_windows[i].number;
                        loc.ride_number = new_ride.get_number_of_ride();
                        var ilosc_ride_w_oknie = list_of_exit_windows[i].return_number_of_rides();
                        var index_of_new = return_order_index_of_exit_window_for_sort_section(list_of_exit_windows[i], (Sort_section)list_of_sections_of_new_ride[i], list_of_indices_in_windows_along_route_new_ride, i, list_of_traverses_of_new_ride[i], new_ride);
                        var number_of_win = list_of_exit_windows[i].number;

                        loc.index_of_new_ride = index_of_new;

                        list_of_indices_in_windows_along_route_new_ride.Add(loc);
                    }
                }
                return list_of_indices_in_windows_along_route_new_ride;
            }

            
            int return_order_index_of_exit_window_for_sort_section(Window affected_exit_win, Sort_section sort, List<loc_of_new_ride> indices, int i, Traverse traverse, Ride _new_ride)
            {
                int new_ride_index_at_entrance_win = indices[i].index_of_new_ride;
                int index_at_exit = 0;

                var list_of_rides_thru_affected_exit = affected_exit_win.get_list_of_rides();

                Window entrance_window_of_new_ride = _new_ride.get_entrance_window_at_section_index(i);
                Ride ride_beeing_checked;
                int checked_ride_index_at_entrance_win;

                // to realizuje przez 
                int new_ride_index_at_enter_win = traverse.get_order_index_of_entrance_window(entrance_window_of_new_ride);
                int ride_beeing_checked_index_at_entrance_win;
                int number_of_rides_thru_exit_win = affected_exit_win.return_number_of_rides();

                // potrzebna sa 2 informacje, z ktorego okna jechala dana kapsola w oknie wyjsciowym (ta informacja jest w kazdym ride)

                // jesli dlugosc listy w oknie out to zero, to ride jest pierwszy
                if (number_of_rides_thru_exit_win == 0)
                {
                    //jeszcze nic tam niema wiec index 0
                    return 0;
                }

                // jesli sa juz jakies ride w oknie exit, to odpalam algorytm sprawdzania 
                for(int k = 0; k < number_of_rides_thru_exit_win; k++)
                {
                    ride_beeing_checked = list_of_rides_thru_affected_exit[k];

                    // index okna w starcie traversu przez ktore jedzie ride do ktorego teraz sie porownuje
                    
                    // jest error bo jakims cudem dostal sie do sekcji, ride ktory przez ta sekcje niejedzie, wiec wyskakuje blad jak probuje ustlisc
                    // ktora sekcja tego ride jest sekcja w ktorej teraz dzialam.
                    // wiec blad musil sie zdazyc wczesniej , i jego efektem jest trafienie tego ride tutaj do sekcji gdzie nipowinno go byc 
                    // pytanie jak to sie stalo. we wczesniejszym etapie dodawania ride, musial byc sprawdzony i dodany. wow
                    // jak to sie stalo , niebrzmi to latwo teraz.
                    // narazie powtarza sie to ze dzieje sie to w sort numerze sekcji 7 i w srodku siedzi ride o drodze przez sekcje 4,5,6
                    // niektore ride maja za duzo exit win, wiec pewnie jest tak ze to jest przyczyna problemu
                    // jak dodaje ride do wszystkich okien z niego to wychodza wlasnie takie szopki, wiec najpier musze naprawic
                    // problem z iloscia okien


                    // i jest tak w srodku ride o drodze 
                    // widze ze dodany jako przejazd zostal tez ride bez route, to tez niepowinno sie stac
                    // dodatkowo ten ride ma 6 exit windows, i 5 komponentow innych typow , czyli tez cos tutaj niezadzialalo
                    // pierwsze okno jest zdublowane, czyli algorytm tworzenia przejazdow robi takie bledy 

                    ride_beeing_checked_index_at_entrance_win = traverse.get_order_index_of_entrance_window(ride_beeing_checked.get_entrance_window_for_section_number(sort.Get_number_of_section()));

                    // traverse.what_is_the_index_of_window_in_traverse_start(window)
                    // window = okno przez ktore wjerzdza teraz sprawdzany ride do traversu
                    //
                    // to powinna byc metoda ktora pyta ride jakie bylo okna na starcie sekcji o numerze KKK


                    // jesli z wczesniejszego okna w entrance to nowa kapsola jest dalej i dodaje 1 do indexu
                    if(ride_beeing_checked_index_at_entrance_win < new_ride_index_at_enter_win)
                    {
                        index_at_exit++;
                        continue;
                    }

                    //jest z tego samego okna, musze sprawdzic kolejnosc w oknie wejsciowym
                    if(ride_beeing_checked_index_at_entrance_win == new_ride_index_at_enter_win)
                    {
                        checked_ride_index_at_entrance_win = entrance_window_of_new_ride.get_index_of_ride(ride_beeing_checked);
                        //sprawdzany ride jest przed nowym ride wiec index ++
                        if(checked_ride_index_at_entrance_win < new_ride_index_at_entrance_win)
                        {
                            index_at_exit++;
                            continue;
                        }

                        else if(checked_ride_index_at_entrance_win == new_ride_index_at_entrance_win)
                        {
                            // skoro jest ten sam index to to new ride bedzie o jeden przed sprawdzanym ride
                            // index zostaje taki sam, jest to graniczyny przypadek, wszystkie wczesniejsze ride w oknie entrance
                            // sa przed new ride, a ten ride i wszystkie pozniejsze sa juz za.
                            return index_at_exit;
                        }

                        else if(checked_ride_index_at_entrance_win > new_ride_index_at_entrance_win)
                        {
                            // sprawdzany ride rusza po dodawanym ride w oknie entrance wiec niema juz wplywu na index 
                            return index_at_exit;

                        }

                    }

                    //jest z okna pozniej czyli index juz sie niezmienia
                    if(ride_beeing_checked_index_at_entrance_win > new_ride_index_at_enter_win)
                    {
                        return index_at_exit;
                    }
                }
                return index_at_exit;
            }

            int return_location_number_at_start_window(Window win)
            {
                //this is simplifies now, and probably shoul be mowed to new class , so can be reused 
                return win.return_number_of_rides();
            }
        }


        public List<CitySection> get_city_sections()
        {
            return RepositoryOfSections.get_list_of_sections();
        }

        public CityDataStorage get_city()
        {
            return city;
        }

        public struct loc_of_new_ride
        {
            public int ride_number { get; set; }
            public int window_number { get; set; }
            public int index_of_new_ride { get; set; }

        }

        public void move_pods()//in firs wersion that is not necessary and can be empty
        {

        }

        public void calculate_statistics()
        {

        }

    }


    public class ride_request_generation
    {
        private Random random;
        private double probability;
        private CityDataStorage city;

        public ride_request_generation( CityDataStorage city, double probability_of_request)
        {
            random = new Random();
            probability = probability_of_request;
            this.city = city;
        }

        public List<request> ride_request_cycle(double time)
        {
            var list = new List<request>();
            if (calculate_if_request_is_created_in_this_cycle(probability))
            {
                //list.Add(new request(1, 8, time));//tymczasowo do debugowania
                //list.Add(new request(1, 4, time));
                //list.Add(new request(2, 0, time));
                //list.Add(new request(2, 3, time));
                list.Add(create_request_for_random_stations(time));
                //list.Add(create_request_for_max_load(time));
                return list;
            }
            else
            {
                return list;
            }
        }


        /// <summary>
        /// calculate if ride request is created, based on arg probability [0,1]
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public bool calculate_if_request_is_created_in_this_cycle(double probability)
        {
            if (random.NextDouble() < probability)
                return true;
            else
                return false;
        }

        public request create_request_for_random_stations(double time)
        {
            int number_of_stations = city.NumberOfStations();
            int start_station = random.Next(number_of_stations);
            int end_station;
            while (true)
            {
                end_station = random.Next(number_of_stations);
                if (end_station == start_station)
                    continue;
                else
                    break;
            }
            var request = new request(start_station, end_station,time);
            return request;
        }


        public request create_request_for_max_load(double time)
        {
            int number_of_stations = city.NumberOfStations();
            int start_station = random.Next(10);
            int end_station;
            while (true)
            {
                end_station = random.Next(10,20);
                if (end_station == start_station)
                    continue;
                else
                    break;
            }

            var request = new request(start_station, end_station, time);


            return request;



        }


    }



    public struct request
    {
        public int Start_station_number { get;}
        public int End_station_number { get;}

        public double request_time { get; }

        public request(int start_station, int end_station, double time)
        {
            Start_station_number = start_station;
            End_station_number = end_station;
            request_time = time;
        }
    }

    public class symulation_data
    {
        public double last_end_time_for_calculated_interactions { get; set; }

        public bool is_generation_of_new_interations_necessary(double time)
        {
            if (time > last_end_time_for_calculated_interactions)
                return true;
            else
                return false;

        }


    }

    public class traffic_generation
    {





    }
}
