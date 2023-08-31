using System.Collections.Generic;

namespace Symulation
{

    public class RepositoryOfSynchronization
    {
        private CityDataStorage city;
        private readonly RepositoryOfPermutations repositoryOfPermutations;
        private readonly RepositoryOfSections repositoryOfSections;
        private List<Synchronization> list_of_synchronizations;
        

        public RepositoryOfSynchronization(CityDataStorage city, RepositoryOfPermutations repositoryOfPermutations, RepositoryOfSections repositoryOfSections)
        {
            this.city = city;
            this.repositoryOfPermutations = repositoryOfPermutations;
            this.repositoryOfSections = repositoryOfSections;
            list_of_synchronizations = new List<Synchronization>();
            
        }


        public void perform_synchronization_for_all_sections(double shift)
        {
            for(int i = 0; i < list_of_synchronizations.Count; i++)
            {
                synchronize_DC_sections_in_synch_circle(list_of_synchronizations[i], shift);
            }

        }


        public void synchronize_DC_sections_in_synch_circle(Synchronization synchronization, double shift)
        {
            var List_of_sections = synchronization.get_list_of_sections();
            var List_of_permutations = synchronization.get_list_of_permutations();
            // dla listy synchronizacji 
            // sprawdzam czy ktorakolwiek sekja jest zsynchronizowana
            if (is_there_at_least_one_section_with_defined_synchro(List_of_sections))
            {
                //ide dalej bo istnieje punkt zaczenienia 
            }
            else
            {
                // jesli nie to definiuje synchronization pierwszej sekcji DC na liscie jako zero i ide dalej 
                List_of_sections[return_index_of_first_DC_section_from_list_of_sections(List_of_sections)].define_synch_for_secton(0);
            }

            while (there_is_at_least_one_DC_section_without_defined_synchro(List_of_sections))
            {

                for(int i = 0; i < List_of_sections.Count; i++)
                {
                    if(is_section_DC_section(List_of_sections[i]) && List_of_sections[i].is_synchronization_def_for_section())
                    {
                        // check section before
                        
                        
                        int index_b = return_index_of_first_DC_before_index(List_of_sections, i);

                        if(index_b != -1)//there is DC section before

                        {
                            if (!List_of_sections[index_b].is_synchronization_def_for_section())
                            {
                                // def synchro of index_b section in relation to i section  

                                // synchro start of exit traverse of index_b with start of entrance traverce of i section
                                // index_b
                                // read permutation number which goes thru index_b
                                
                                Permutation perm_before = List_of_permutations[index_b];
                                int exit_node = List_of_permutations[index_b].get_end_node();
                                var config_before = List_of_sections[index_b].get_config_for_exit(exit_node);


                                // read start time of that perm in config
                                var start_time_first = read_start_time_for_permutation_in_config(config_before, List_of_permutations[index_b].get_number_of_permutation());


                                // index i
                                // read permutation number which goes thru i

                                Permutation perm_after = List_of_permutations[i];
                                var start_node = List_of_permutations[i].get_start_node();
                                var config_after = List_of_sections[i].get_config_for_exit(start_node);

                                var start_time_second = read_start_time_for_permutation_in_config(config_after, List_of_permutations[i].get_number_of_permutation());

                                // read synchro time for i section 
                                var synchro_second = List_of_sections[i].get_synchro_time_for_section();


                                // calculate travel time from end of index_b to start of permutation i
                                var search = new SearchForRoutesMain(city);
                                int[][][] routes = search.SeekRouteBetweenNodes(exit_node, start_node);
                                var route = routes[0];

                                var profile_calculator = new FastestProfile(route, city);
                                var (error_code, profile) = profile_calculator.ProfileBetweenNodes();//number is acceleration of pod



                                // travel time can be calculated without size of traverse 
                                var travel_time = read_travel_time_between_nodes_from_profile(profile);


                                // calculate synchro for index_b section 
                                double synchro_for_start_section = (start_time_second + synchro_second - travel_time) - start_time_first;

                                List_of_sections[index_b].define_synch_for_secton(synchro_for_start_section + shift);
                                

                            }
                        }

                        int index_a = return_index_of_first_DC_after_index(List_of_sections, i);
                        
                        if(index_a != -1)//there is DC section after
                        {
                            if (!List_of_sections[index_a].is_synchronization_def_for_section())
                            {

                                //def synchro of index_a section in relation to i section  
                                int start_node = List_of_permutations[index_a].get_start_node();
                                var config_second = List_of_sections[index_a].get_config_for_entrance(start_node);


                                // read start time of that perm in config
                                var start_time_second = read_start_time_for_permutation_in_config(config_second, List_of_permutations[index_a].get_number_of_permutation());


                                // index i
                                // read permutation number which goes thru i

                                var end_node = List_of_permutations[i].get_end_node();
                                var config_first = List_of_sections[i].get_config_for_exit(end_node);

                                var start_time_first = read_start_time_for_permutation_in_config(config_first, List_of_permutations[i].get_number_of_permutation());

                                // read synchro time for i section 
                                var synchro_first = List_of_sections[i].get_synchro_time_for_section();


                                // calculate travel time from end of index_b to start of permutation i
                                var search = new SearchForRoutesMain(city);
                                int[][][] routes = search.SeekRouteBetweenNodes(end_node, start_node);
                                var route = routes[0];

                                var profile_calculator = new FastestProfile(route, city);
                                var (error_code, profile) = profile_calculator.ProfileBetweenNodes();//number is acceleration of pod



                                // travel time can be calculated without size of traverse 
                                var travel_time = read_travel_time_between_nodes_from_profile(profile);


                                // calculate synchro for index_b section 
                                double synchro_for_start_section = (start_time_first + synchro_first + travel_time) - start_time_second;

                                List_of_sections[index_a].define_synch_for_secton(synchro_for_start_section + shift);


                            }
                        }
                    }
                }
            }


            //for(int i = 0; i < List_of_sections.Count; i++)
            //{
            //    System.Console.WriteLine("");
            //    if (List_of_sections[i].what_is_the_type_of_this_section() == typeof(DC_section))
            //    {
            //        System.Console.WriteLine("synchronizacja sekcji:");
            //        System.Console.WriteLine("Secja : {0}  czas synchronizacji {1}",List_of_sections[i].Get_number_of_section() , List_of_sections[i].get_synchro_time_for_section());
                    
            //    }
            //    System.Console.WriteLine(" ");
            //}

            // sprawdzam czy dana sekcja DC ma synchro 
                //jesli tak
                    //to sprawdzam czy sekja dalej ma synchro
                        //jesli nie,to definiuje jej synchro

                   //sprawdzam czy sekcje przed ma synchro (jesli taka jest)
                        //jesli nie to definiuje synchro
                //jesli nie
                    //to ide dalej
        }

        private double read_travel_time_between_nodes_from_profile(List<double[]> profile)
        {

            int index_of_last = profile.Count - 1;

            double start_time = profile[0][7];
            double end_time = profile[index_of_last][8];

            return end_time - start_time;

        }

        private double read_start_time_for_permutation_in_config(List<double[]> config, int permutation_number)
        {
            //config: perm number, length, start time , end time

            for(int i = 0; i < config.Count; i++)
            {
                if(config[i][0] == permutation_number)
                    return config[i][2];
            }

            return -1; //error
        }

        private int return_index_of_first_DC_before_index(List<CitySection> sections, int index)
        {

            if(index == 0)
                return -1;


            for(int i = index - 1; i >= 0; i--)
            {
                if (is_section_DC_section(sections[i]))
                    return i;
            }

            return -1;
        }

        private int return_index_of_first_DC_after_index(List<CitySection> sections, int index)
        {

            if(sections.Count - 1 == index)
                return -1;


            for (int i = index + 1; i < sections.Count; i++)
            {
                if (is_section_DC_section(sections[i]))
                    return i;
            }

            return -1;
        }

        private int return_index_of_first_DC_section_from_list_of_sections(List<CitySection> sections)
        {

            for(int i = 0; i < sections.Count; i++)
            {
                if(sections[i].what_is_the_type_of_this_section() == typeof(DC_section))
                {
                    return i;
                }
            }

            return -1; //exception should be here
        }

        public bool is_there_at_least_one_section_with_defined_synchro(List<CitySection> sections)
        {
            for(int i = 0; i <sections.Count; i++)
            {
                if(sections[i].is_synchronization_def_for_section())
                    return true;
            }

            return false;
        }

        public bool there_is_at_least_one_DC_section_without_defined_synchro(List<CitySection> sections)
        {
            for(int i = 0; i < sections.Count; i++)
            {
                if (sections[i].what_is_the_type_of_this_section() == typeof(DC_section) && !sections[i].is_synchronization_def_for_section())
                    return true;
            }
            return false;
        }

        private bool is_section_DC_section(CitySection section)
        {
            if(section.what_is_the_type_of_this_section() == typeof(DC_section))
                return true;

            return false;
        }

        public void add_new_synchronization_by_node(Synchronization synchronization)
        {
            int def_node = synchronization.get_def_node();

            Permutation start_permutation = return_non_start_permutation_going_thru_node(def_node);


            var permutation_ring_of_synch = return_synch_circle_list_from_start_permutation_by_smalest_number_of_sections_in_circle(start_permutation);
            
            synchronization.add_list_of_permutations(permutation_ring_of_synch);
            synchronization.add_list_of_sections(translate_list_of_permutations_into_list_of_sections(permutation_ring_of_synch));
            list_of_synchronizations.Add(synchronization);


        }

        private List<CitySection> translate_list_of_permutations_into_list_of_sections(List<Permutation> list_of_perm)
        {
            List<CitySection> list_of_city_sections = new List<CitySection>();

            for (int i = 0; i < list_of_perm.Count; i++)
            {
                list_of_city_sections.Add(repositoryOfSections.get_section_with_number(list_of_perm[i].get_section_number()));
            }

            return list_of_city_sections;
        }

        private List<Permutation> return_synch_circle_list_from_start_permutation_by_smalest_number_of_sections_in_circle(Permutation permutation)
        {
            List<List<Permutation>> circle_candidates = new List<List<Permutation>>();
            List<Permutation> List_of_perms_along_the_way = new List<Permutation>();

            circle_candidates.Add(new List<Permutation> { permutation });
            //dla kazdego candydata
            // sprawdz co jest za perm (w kierunku ruchu) 
            //jesli jest wiecej niz jedna perm to kopiuje listy

            while (true)
            {
                for (int i = 0; i < circle_candidates.Count; i++)
                {
                    List_of_perms_along_the_way = return_permutations_along_the_way_for_circle_candidate(circle_candidates[i]);

                    if (List_of_perms_along_the_way.Count == 1)
                    {
                        // sprawdzam czy to jest to samo co na poczatku listy bo jesli tak to jest odpowiedz 
                        if (circle_candidates[i][0] == List_of_perms_along_the_way[0])
                        {
                            //to jest odpowiedz i ja zwracam
                            return circle_candidates[i];
                        }
                        else
                        {
                            circle_candidates[i].Add(List_of_perms_along_the_way[0]);
                        }

                    }

                    if (List_of_perms_along_the_way.Count == 2)
                    {
                        //kopiuje kandydata i dodaje go na kolejny index zsuwajac reszte dalej
                        circle_candidates.Insert(i + 1, copy_list(circle_candidates[i]));

                        if (circle_candidates[i][0] == List_of_perms_along_the_way[0])
                        {
                            //to jest odpowiedz i ja zwracam
                            return circle_candidates[i];
                        }
                        else
                        {
                            circle_candidates[i].Add(List_of_perms_along_the_way[0]);
                        }

                        //zwiekszam index i
                        i++;

                        //powtarzam procedure dla kolejnego 
                        if (circle_candidates[i][0] == List_of_perms_along_the_way[1])
                        {
                            //to jest odpowiedz i ja zwracam
                            return circle_candidates[i];
                        }
                        else
                        {
                            circle_candidates[i].Add(List_of_perms_along_the_way[1]);
                        }


                    }





                }

            }

        }

        private List<Permutation> copy_list(List<Permutation> permutations)
        {
            List<Permutation> new_permutations = new List<Permutation>();

            for(int i = 0; i < permutations.Count; i++)
            {
                new_permutations.Add(permutations[i]);
            }

            return new_permutations;
        }


        private List<Permutation> return_permutations_along_the_way_for_circle_candidate(List<Permutation> circle_candidate)
        {
            Permutation current_last_perm = circle_candidate[circle_candidate.Count -1];

            var new_perm = return_permutations_along_the_way(current_last_perm);

            return new_perm;

        }

        private List<Permutation> return_permutations_along_the_way(Permutation permutation)
        {
            var perms = repositoryOfPermutations.return_non_end_start_permutations_connected_to_permutation_along_the_way(permutation);

            return perms;
        }



        private Permutation return_non_start_permutation_going_thru_node(int node)
        {
            var start_permutations = repositoryOfPermutations.return_non_end_start_permutations_going_thru_node(node);
            Permutation start_permutation;

            if (start_permutations.Count == 0)
            {
                System.Console.WriteLine("error");
                return null;
            }
            else if (start_permutations.Count > 1)
            {
                System.Console.WriteLine("error");
                return null;   
            }
            else
            {
                start_permutation = start_permutations[0];
            }

            return start_permutation;
        }





    }

    public class Synchronization
    {
        private readonly int def_Node;
        private List<Permutation> list_of_perm;
        private List<CitySection> list_of_city_sections;

        public Synchronization(int def_node)
        {
            def_Node = def_node;
            list_of_perm = new List<Permutation>();
            list_of_city_sections = new List<CitySection>();
            //narazie dzialam bez sekcji synchronizacyjnej 
            //dodam ja pozniej lub wepne do ukladu jakis track tego ukladu
        }

        public int get_def_node()
        {
            return def_Node;
        }

        public void add_list_of_permutations(List<Permutation> permutations)
        {
            list_of_perm = permutations;
        }
        public List<Permutation> get_list_of_permutations()
        {
            return list_of_perm;
        }

        public void add_list_of_sections(List<CitySection> sections)
        {
            list_of_city_sections = sections;
        }

        public List<CitySection> get_list_of_sections()
        {
            return list_of_city_sections;
        }

        public void calculate_synch_time_for_sections_in_list()
        {

        }

        // potrzebne metody
        // -ogarniecie pierscienia do synchronizacji
            //- z jakimi permutacjami laczy sie dany node [0]
            //- z jakimi permutacjami laczy sie dana permutacja [1]
            //- ktora sekcja jest bazowa, czyli pierwsza jesli chodzi o kierunek ruchu
                //- tak ktora ma w sobie noude 
            //- czy dana pemutacja jest przed czy za dana pemutacja w kierunku ruchu
                //- metoda do sortowania permutacji w liscie spisu pierscienia 
            //- liczenie roznicy w czasie pomiedzy sekcjami 
        //kazda sekcje bedzie miec oprocz config, jeszcze wartosc startowa dla generowania traversow
            //to jest narazie ok , przy zalozeniu ze jest staly konfig w czasie
            //jesli bedzie sie zmieniac to skomplikuje sytuacje, ale narazie tak upraszczam algorytm  

                
    }

}
