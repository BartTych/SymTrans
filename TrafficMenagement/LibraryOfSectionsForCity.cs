using System.Collections.Generic;

namespace Symulation
{
    public class LibraryOfSectionsForCity
    {
        private CityDataStorage city;
        RepositoryOfPermutations repository_of_permutations;
        private RepositoryOfSections Repository_of_Sections;
        private RepositoryOfSynchronization Repository_of_synchronizations;
        List<CitySection> city_sections;



        public LibraryOfSectionsForCity(CityDataStorage city)
        {
            this.city = city;
            city_sections = new List<CitySection>();
            repository_of_permutations = new RepositoryOfPermutations(city);
            Repository_of_Sections = new RepositoryOfSections();
            Repository_of_synchronizations = new RepositoryOfSynchronization(city, repository_of_permutations, Repository_of_Sections);

        }

        //public (RepositoryOfSections, RepositoryOfPermutations) SectionsAndPermutationsForCzernieVice()
        //{
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;

        //    list_of_I_nodes = new List<int>(){30, 23};
        //    list_of_O_nodes = new List<int>(){36};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() {54};
        //    list_of_O_nodes = new List<int>() {7, 11};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() {6, 16};
        //    list_of_O_nodes = new List<int>() {17};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() {17};
        //    list_of_O_nodes = new List<int>() {24, 30};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() {49, 42};
        //    list_of_O_nodes = new List<int>() {6, 11};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations));

        //    return (Repository_of_Sections, repository_of_permutations);
        //}
        //public (RepositoryOfSections, RepositoryOfPermutations) SectionsAndPermutationsRozgalezienie()
        //{
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;

        //    list_of_I_nodes = new List<int>() { 25, 23 ,19, 16, 12, 11, 9, 29, 31};
        //    list_of_O_nodes = new List<int>() { 34, 5};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations));



        //    return (Repository_of_Sections, repository_of_permutations);
        //}

        //public (RepositoryOfSections, RepositoryOfPermutations) SectionsAndPermutationsRozgalezienie_stacja()
        //{
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;

        //    list_of_I_nodes = new List<int>() { 37, 38, 12, 11, 9,36 };
        //    list_of_O_nodes = new List<int>() { 36, 5 };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations));



        //    return (Repository_of_Sections, repository_of_permutations);
        //}

        //public (RepositoryOfSections, RepositoryOfPermutations, RepositoryOfSynchronization) Sections_first_test(double sort_traverse_length, double shift)
        //{
        //    //dodam tutaj repositiory of synchronization

        //    int default_exit_number_for_sort = 3;
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;
        //    List<(int, int)> list_of_priority_directions;

        //    //change of speed aroud stations
        //    mod_city_around_stations();

        //    //przy dodawaniu sekcji jest liczony config tej sekcji

        //    list_of_I_nodes = new List<int>() {0, 2};
        //    list_of_O_nodes = new List<int>() {2, 4};
        //    list_of_priority_directions = new List<(int, int)> {(0, 4)};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 4 };
        //    list_of_O_nodes = new List<int>() { 7 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length,default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 7, 9 };
        //    list_of_O_nodes = new List<int>() { 9, 11 };
        //    list_of_priority_directions = new List<(int, int)> { (7, 11) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 11 };
        //    list_of_O_nodes = new List<int>() { 12 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length,default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 12, 15 };
        //    list_of_O_nodes = new List<int>() { 17, 15 };
        //    list_of_priority_directions = new List<(int, int)> { (12, 17) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 17 };
        //    list_of_O_nodes = new List<int>() { 18 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length,default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 18, 20 };
        //    list_of_O_nodes = new List<int>() { 20, 22 };
        //    list_of_priority_directions = new List<(int, int)> { (18, 22) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 22 };
        //    list_of_O_nodes = new List<int>() { 0 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length,default_exit_number_for_sort));

            


        //    // repositiory of synch
        //    // add new 
        //    // curent algorithm of chosing synchro circle, smallest number of sections  
        //    Repository_of_synchronizations.add_new_synchronization_by_node(new Synchronization(5));
            
        //    Repository_of_synchronizations.perform_synchronization_for_all_sections(shift);

        //    return (Repository_of_Sections, repository_of_permutations,Repository_of_synchronizations);
        //}

        //public (RepositoryOfSections, RepositoryOfPermutations, RepositoryOfSynchronization) sekcje_rozgalezienie_DC(double sort_traverse_length, double shift)
        //{
        //    //dodam tutaj repositiory of synchronization

        //    int default_exit_number_for_sort = 3;
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;
        //    List<(int, int)> list_of_priority_directions;

        //    //change of speed aroud stations
        //    mod_city_around_stations();

        //    //przy dodawaniu sekcji jest liczony config tej sekcji

        //    list_of_I_nodes = new List<int>() { 1, 3 };
        //    list_of_O_nodes = new List<int>() { 3, 5 };
        //    list_of_priority_directions = new List<(int, int)> { (1, 5) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 5 };
        //    list_of_O_nodes = new List<int>() { 6 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 0 };
        //    list_of_O_nodes = new List<int>() { 1 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));


        //    list_of_I_nodes = new List<int>() { 9, 12 };
        //    list_of_O_nodes = new List<int>() { 12, 13 };
        //    list_of_priority_directions = new List<(int, int)> { (9, 13) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 8 };
        //    list_of_O_nodes = new List<int>() { 9 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 13 };
        //    list_of_O_nodes = new List<int>() { 14 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));


        //    list_of_I_nodes = new List<int>() { 17, 19 };
        //    list_of_O_nodes = new List<int>() { 19, 21 };
        //    list_of_priority_directions = new List<(int, int)> { (17, 21) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 21 };
        //    list_of_O_nodes = new List<int>() { 22 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 16 };
        //    list_of_O_nodes = new List<int>() { 17 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 6, 22 };
        //    list_of_O_nodes = new List<int>() { 8 };
        //    list_of_priority_directions = new List<(int, int)> {};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 14 };
        //    list_of_O_nodes = new List<int>() { 16, 0 };
        //    list_of_priority_directions = new List<(int, int)> { };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));


        //    // repositiory of synch
        //    // add new 
        //    // curent algorithm of chosing synchro circle, smallest number of sections  
        //    //Repository_of_synchronizations.add_new_synchronization_by_node(new Synchronization(5));

        //    //Repository_of_synchronizations.perform_synchronization_for_all_sections(shift);

        //    return (Repository_of_Sections, repository_of_permutations, Repository_of_synchronizations);
        //}

        public (RepositoryOfSections, RepositoryOfPermutations, RepositoryOfSynchronization) Translate_city_modules_to_sections( List<map_module> list_of_map_modules, double sort_traverse_length, double DC_length)
        {

            int default_exit_number_for_sort = 3;
            List<int> list_of_I_nodes;
            List<int> list_of_O_nodes;
            List<(int, int)> list_of_priority_directions;

            mod_city_around_stations();

            //ide po wszystkich modulach mapy
            //wyciagam najpierw I node list , O node list i liste priority
            //potem twoze i dodaje sekcje
            // tutaj definiujac sekcje dla kazdej z nich licze config 
            // czyli robie tak ze dopasowuje do siebie kazdy z mozliwych kierunkow przejazdu. 
            for(int i = 0; i < list_of_map_modules.Count; i++)
            {
                if(list_of_map_modules[i].type_of_section == type_of_section.DC)
                {
                    // problem ze wszystkie numery musza byc w numeracji samej mapy niskopoziomowej

                    list_of_I_nodes = list_of_map_modules[i].get_I_nodes_numbers();
                    list_of_O_nodes = list_of_map_modules[i].get_O_nodes_numbers();
                    if (list_of_map_modules[i].is_priority_direction_def())
                        list_of_priority_directions = new List<(int, int)> { list_of_map_modules[i].get_priority_dir() };
                    else
                        list_of_priority_directions = new List<(int, int)>();

                    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations, DC_length));
                }

                if(list_of_map_modules[i].type_of_section== type_of_section.Sort)
                {
                    list_of_I_nodes = list_of_map_modules[i].get_I_nodes_numbers();
                    list_of_O_nodes = list_of_map_modules[i].get_O_nodes_numbers();
                    //list_of_priority_directions = list_of_map_modules[i].priority_direction;

                    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));


                }
            }
            return (Repository_of_Sections, repository_of_permutations, Repository_of_synchronizations);

        }

        //public (RepositoryOfSections, RepositoryOfPermutations, RepositoryOfSynchronization) Sections_eight_stations(double sort_traverse_length, double shift)
        //{
        //    //dodam tutaj repositiory of synchronization

        //    int default_exit_number_for_sort = 3;
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;
        //    List<(int, int)> list_of_priority_directions;

        //    //change of speed aroud stations
        //    mod_city_around_stations();

        //    //przy dodawaniu sekcji jest liczony config tej sekcji

        //    list_of_I_nodes = new List<int>() { 0, 32 };
        //    list_of_O_nodes = new List<int>() { 32, 3 };
        //    list_of_priority_directions = new List<(int, int)> { (0, 3) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 3 };
        //    list_of_O_nodes = new List<int>() { 4 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 4, 33 };
        //    list_of_O_nodes = new List<int>() { 33, 7 };
        //    list_of_priority_directions = new List<(int, int)> { (4, 7) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 7 };
        //    list_of_O_nodes = new List<int>() { 8 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 8, 34 };
        //    list_of_O_nodes = new List<int>() { 34, 11 };
        //    list_of_priority_directions = new List<(int, int)> { (8, 11) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 11 };
        //    list_of_O_nodes = new List<int>() { 12 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 12, 35 };
        //    list_of_O_nodes = new List<int>() { 35, 15 };
        //    list_of_priority_directions = new List<(int, int)> { (12, 15) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 15 };
        //    list_of_O_nodes = new List<int>() { 16 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 16, 36 };
        //    list_of_O_nodes = new List<int>() { 36, 19 };
        //    list_of_priority_directions = new List<(int, int)> { (16, 19) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 19 };
        //    list_of_O_nodes = new List<int>() { 20 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 20, 37 };
        //    list_of_O_nodes = new List<int>() { 37, 23 };
        //    list_of_priority_directions = new List<(int, int)> { (20, 23) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 23 };
        //    list_of_O_nodes = new List<int>() { 24 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 24, 38 };
        //    list_of_O_nodes = new List<int>() { 38, 27 };
        //    list_of_priority_directions = new List<(int, int)> { (24, 27) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 27 };
        //    list_of_O_nodes = new List<int>() { 28 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 28, 39 };
        //    list_of_O_nodes = new List<int>() { 39, 31 };
        //    list_of_priority_directions = new List<(int, int)> { (28, 31) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 31 };
        //    list_of_O_nodes = new List<int>() { 0 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    // repositiory of synch
        //    // add new 
        //    // curent algorithm of chosing synchro circle, smallest number of sections  
        //    Repository_of_synchronizations.add_new_synchronization_by_node(new Synchronization(40));

        //    Repository_of_synchronizations.perform_synchronization_for_all_sections(shift);

        //    return (Repository_of_Sections, repository_of_permutations, Repository_of_synchronizations);
        //}

        //public (RepositoryOfSections, RepositoryOfPermutations, RepositoryOfSynchronization) sections_two_rings_v_1_0(double sort_traverse_length, double shift)
        //{
        //    //dodam tutaj repositiory of synchronization

        //    int default_exit_number_for_sort = 3;
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;
        //    List<(int, int)> list_of_priority_directions;

        //    //change of speed aroud stations
        //    mod_city_around_stations();

        //    //przy dodawaniu sekcji jest liczony config tej sekcji

        //    list_of_I_nodes = new List<int>() { 2, 5 };
        //    list_of_O_nodes = new List<int>() { 5, 6 };
        //    list_of_priority_directions = new List<(int, int)> { (2, 6) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 6 };
        //    list_of_O_nodes = new List<int>() { 8 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 8 };
        //    list_of_O_nodes = new List<int>() { 10 };
        //    list_of_priority_directions = new List<(int, int)> {};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 10 };
        //    list_of_O_nodes = new List<int>() { 11 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 11, 12 };
        //    list_of_O_nodes = new List<int>() { 12, 15 };
        //    list_of_priority_directions = new List<(int, int)> { (11, 15) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 15 };
        //    list_of_O_nodes = new List<int>() { 16 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 16 };
        //    list_of_O_nodes = new List<int>() { 0 };
        //    list_of_priority_directions = new List<(int, int)> {};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 0 };
        //    list_of_O_nodes = new List<int>() { 2 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    // repositiory of synch
        //    // add new 
        //    // curent algorithm of chosing synchro circle, smallest number of sections  
        //    Repository_of_synchronizations.add_new_synchronization_by_node(new Synchronization(7));

        //    Repository_of_synchronizations.perform_synchronization_for_all_sections(shift);

        //    return (Repository_of_Sections, repository_of_permutations, Repository_of_synchronizations);
        //}

        //public (RepositoryOfSections, RepositoryOfPermutations, RepositoryOfSynchronization) sections_twelve_stations_v_1_0(double sort_traverse_length, double shift)
        //{
        //    //dodam tutaj repositiory of synchronization

        //    int default_exit_number_for_sort = 3;
        //    List<int> list_of_I_nodes;
        //    List<int> list_of_O_nodes;
        //    List<(int, int)> list_of_priority_directions;

        //    //change of speed aroud stations
        //    mod_city_around_stations();

        //    //przy dodawaniu sekcji jest liczony config tej sekcji

        //    list_of_I_nodes = new List<int>() { 0 };
        //    list_of_O_nodes = new List<int>() { 41, 3 };
        //    list_of_priority_directions = new List<(int, int)> { };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 3 };
        //    list_of_O_nodes = new List<int>() { 4 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 4, 33 };
        //    list_of_O_nodes = new List<int>() { 33, 7 };
        //    list_of_priority_directions = new List<(int, int)> { (4,7) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 7 };
        //    list_of_O_nodes = new List<int>() { 8 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 8, 34 };
        //    list_of_O_nodes = new List<int>() { 34, 11 };
        //    list_of_priority_directions = new List<(int, int)> { (8, 11) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 11 };
        //    list_of_O_nodes = new List<int>() { 12 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 12 };
        //    list_of_O_nodes = new List<int>() { 15, 83 };
        //    list_of_priority_directions = new List<(int, int)> { };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 15 };
        //    list_of_O_nodes = new List<int>() { 16 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));


        //    list_of_I_nodes = new List<int>() { 16, 36 };
        //    list_of_O_nodes = new List<int>() { 36, 19 };
        //    list_of_priority_directions = new List<(int, int)> { (16, 19)};
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 19 };
        //    list_of_O_nodes = new List<int>() { 20 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 20, 37 };
        //    list_of_O_nodes = new List<int>() { 37, 23 };
        //    list_of_priority_directions = new List<(int, int)> { (20, 23) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 23 };
        //    list_of_O_nodes = new List<int>() { 24 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 24, 38 };
        //    list_of_O_nodes = new List<int>() { 38, 27 };
        //    list_of_priority_directions = new List<(int, int)> { (24, 27) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 27 };
        //    list_of_O_nodes = new List<int>() { 28 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 28, 39 };
        //    list_of_O_nodes = new List<int>() { 39, 31 };
        //    list_of_priority_directions = new List<(int, int)> { (28, 31) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 31 };
        //    list_of_O_nodes = new List<int>() { 0 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 41 };
        //    list_of_O_nodes = new List<int>() { 43 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 81 };
        //    list_of_O_nodes = new List<int>() { 83 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));




        //    list_of_I_nodes = new List<int>() { 43, 45 };
        //    list_of_O_nodes = new List<int>() { 80 };
        //    list_of_priority_directions = new List<(int, int)> { };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 46 };
        //    list_of_O_nodes = new List<int>() { 45 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 50, 48 };
        //    list_of_O_nodes = new List<int>() { 48, 46 };
        //    list_of_priority_directions = new List<(int, int)> { (50, 46) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 51 };
        //    list_of_O_nodes = new List<int>() { 50 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));


        //    list_of_I_nodes = new List<int>() { 55, 53 };
        //    list_of_O_nodes = new List<int>() { 53, 51 };
        //    list_of_priority_directions = new List<(int, int)> { (55, 51) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 56 };
        //    list_of_O_nodes = new List<int>() { 55 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 60, 58 };
        //    list_of_O_nodes = new List<int>() { 58, 56 };
        //    list_of_priority_directions = new List<(int, int)> { (60, 56) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 61 };
        //    list_of_O_nodes = new List<int>() { 60 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 63 };
        //    list_of_O_nodes = new List<int>() { 61, 81 };
        //    list_of_priority_directions = new List<(int, int)> { };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 80 };
        //    list_of_O_nodes = new List<int>() { 79 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));




        //    list_of_I_nodes = new List<int>() { 79, 77 };
        //    list_of_O_nodes = new List<int>() { 77, 75 };
        //    list_of_priority_directions = new List<(int, int)> { (79, 75) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 75 };
        //    list_of_O_nodes = new List<int>() { 73 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 73, 71 };
        //    list_of_O_nodes = new List<int>() { 71, 69 };
        //    list_of_priority_directions = new List<(int, int)> { (73, 69) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 69 };
        //    list_of_O_nodes = new List<int>() { 67 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));

        //    list_of_I_nodes = new List<int>() { 67, 66 };
        //    list_of_O_nodes = new List<int>() { 66, 85 };
        //    list_of_priority_directions = new List<(int, int)> { (67,85) };
        //    Repository_of_Sections.add_new_section(new DC_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, list_of_priority_directions, repository_of_permutations));

        //    list_of_I_nodes = new List<int>() { 85 };
        //    list_of_O_nodes = new List<int>() { 63 };
        //    Repository_of_Sections.add_new_section(new Sort_section(Repository_of_Sections.get_next_free_section_number(), city, list_of_I_nodes, list_of_O_nodes, repository_of_permutations, sort_traverse_length, default_exit_number_for_sort));


        //    // repositiory of synch
        //    // add new 
        //    // curent algorithm of chosing synchro circle, smallest number of sections  
        //    //Repository_of_synchronizations.add_new_synchronization_by_node(new Synchronization(84));
        //    //Repository_of_synchronizations.add_new_synchronization_by_node(new Synchronization(42));
        //    //Repository_of_synchronizations.add_new_synchronization_by_node(new Synchronization(68));



        //    //Repository_of_synchronizations.perform_synchronization_for_all_sections(shift);

        //    return (Repository_of_Sections, repository_of_permutations, Repository_of_synchronizations);
        //}

        /// <summary>
        /// length of mod is defined inside
        /// speed of mod is defined inside
        /// </summary>
        private void mod_city_around_stations()
        {

            double length_of_mod = 5;
            double speed_of_mod_section = 3;// [m/s]

            //for each station
            var list_of_staions = city.GetListOfAllStationsInCity();
            int node;
            List<int> list_of_attached_tracks;
            int directionality_node;
            var station_mod = new station_mod(city);

            for (int i = 0; i < city.NumberOfStations(); i++)
            {
                node = list_of_staions[i].NumberOfAttachedNode;
                list_of_attached_tracks = city.GetListOfAllTracksConnectedToNode(node);

                for (int j = 0; j < list_of_attached_tracks.Count; j++)
                {
                    directionality_node = city.GetDirectionalityNodeForTrack(list_of_attached_tracks[j]);
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

    }

}
