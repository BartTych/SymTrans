using System;
using System.Collections.Generic;

namespace Symulation
{
    public class ride_search_thread
    {

        public int start_station { get; set; }
        public int start_node { get; set; }
        public int end_station { get; set;}
        public int end_node { get; set; }

        public int index_of_pregress { get; set; }//index of 
        public int index_of_last_section { get; set; } // index of last section / travese

        private int[][] thread_route;

        private double start_time;


        // sections and permutations are based on route 
        List<CitySection> list_of_sections;
        List<Permutation> list_of_permutations;

        // traverse i dladanej permutacji 
        // i okna danego traversu sa juz przypisane charakterystyczne dla danego watku szukania 
        List<Traverse> list_of_traverse;
        List<Window> list_of_entrance_windows;
        List<Window> list_of_exit_windows;

        public ride_search_thread()
        {
            list_of_sections = new List<CitySection>();
            list_of_permutations = new List<Permutation>();
            list_of_traverse = new List<Traverse>();
            list_of_entrance_windows = new List<Window>();
            list_of_exit_windows = new List<Window>();
        }

        public List<CitySection> get_city_sections_list()
        {
            return list_of_sections;
        }
        public List<Permutation> get_premutations_list()
        {
            return list_of_permutations;
        }
        public List<Traverse> get_list_of_traverses()
        {
            return list_of_traverse;
        }
        public List<Window> get_entrance_windows()
        {
            return list_of_entrance_windows;
        }
        public List<Window> get_exit_windows()
        {
            return list_of_exit_windows;
        }
        
        public List<Window> get_all_windows_along_the_way()
        {
            var list = new List<Window>();

            for (int i = 0; i < list_of_entrance_windows.Count; i++)
                list.Add(list_of_entrance_windows[i]);

            list.Add(list_of_exit_windows[list_of_exit_windows.Count - 1]);

            return list;
        }

        public void add_start_time(double start_time)
        {
            this.start_time = start_time;
        }

        public double get_start_time()
        {
            return start_time;
        }

        public void add_route(int[][] route)
        {
            thread_route = route;
        }

        public int[][] get_thread_route()
        {
            return thread_route;
        }
        public void add_section_list(List<CitySection> route_sections)
        {
            list_of_sections = route_sections;
            index_of_last_section = route_sections.Count-1;
        }
        public void add_permutation_list(List<Permutation> route_permutations)
        {
            list_of_permutations = route_permutations;
        }
        public void add_permutation(Permutation permutation)
        {
            list_of_permutations.Add(permutation);
        }
        public void add_traverse(Traverse traverse)
        {
            list_of_traverse.Add(traverse);
        }
        public void add_exit_window(Window window)
        {
            list_of_exit_windows.Add(window);
        }
        public void add_entrance_window(Window window)
        {
            list_of_entrance_windows.Add(window);
        }
        public bool is_this_last_section(int index)
        {
            if(index_of_last_section == index)
                return true;
            else
                return false;
        }
        public Type get_type_of_section_for_index(int index)
        {
            var sections = list_of_sections[index];

            return sections.GetType();
        }
        public Window get_entrance_window_for_index(int index)
        {
            return list_of_entrance_windows[index];
        }
        public Window get_exit_window_for_index(int index)
        {
            return list_of_exit_windows[index];
        }
        public Permutation get_next_permutation_on_route(int index)
        {
            return list_of_permutations[index + 1];
        }
        public Traverse get_traverse_by_index(int index)
        {
            return list_of_traverse[index];
        }

        public ride_search_thread deep_copy()
        {
            var new_thread = new ride_search_thread();
            new_thread.start_station = start_station;
            new_thread.end_station = end_station;
            new_thread.start_node = start_node;
            new_thread.end_node = end_node;
            new_thread.index_of_last_section = index_of_last_section;
            new_thread.index_of_pregress = index_of_pregress;
            new_thread.start_time = start_time;
            new_thread.list_of_sections = new List<CitySection>();
            
            for(int i=0; i<list_of_sections.Count; i++)
                new_thread.list_of_sections.Add(list_of_sections[i]);
            
            new_thread.list_of_permutations = new List<Permutation>();

            for(int i=0; i<list_of_permutations.Count; i++)
                new_thread.list_of_permutations.Add(list_of_permutations[i]);

            new_thread.list_of_entrance_windows = new List<Window>();

            for (int i = 0; i < list_of_entrance_windows.Count; i++)
                new_thread.list_of_entrance_windows.Add(list_of_entrance_windows[i]);

            new_thread.list_of_exit_windows = new List<Window>();

            for (int i = 0; i < list_of_exit_windows.Count; i++)
                new_thread.list_of_exit_windows.Add(list_of_exit_windows[i]);

            new_thread.list_of_traverse = new List<Traverse>();

            for(int i=0;i<list_of_traverse.Count;i++)
                new_thread.list_of_traverse.Add(list_of_traverse[i]);

            new_thread.add_route(thread_route);


            return new_thread;
        }
        public void add_one_to_progress()
        {
            index_of_pregress++;
        }
        // na przejezdzie przez sort sekcje mozna wybrac rozne okna w trawersie wiec musz jakos zapisywac ktore okna zostaly wybrane
        // sort sekcja , okno wejscia , okno wyjscia
        // DC sekcja, okno wejscia, okno wyjscia
        





    }
}
