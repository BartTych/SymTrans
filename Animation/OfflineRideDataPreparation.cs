using System;
using System.Collections.Generic;
using System.IO;

namespace Symulation
{


    public class OfflineRideDataPreparation
    {
        private ride_data_repository ride_repository;

        public OfflineRideDataPreparation(ride_data_repository ride_repository)
        {
            this.ride_repository = ride_repository;
        }

        public void filter_rides_by_traverse_number(List<int> list_of_traverse)
        {
            // filrty beda w samym ride repository
            // ewentualnie moge wrzucic je w nowa classe
            // jak sie zrobi ich za duzo 

            // tu bedzie tylko odwolanie do funkcji ktora jest w samym ride repository
            ride_repository.filter_rides_by_number_of_traverse(list_of_traverse);

        }

        public void apply_all_mods_in_symulation_rides()
        {
            ride_repository.apply_mods_to_all_rides();
        }

        public void update_profile_for_all_rides()
        {
            ride_repository.update_profile_for_all_rides();
        }

        public void calculate_position_data_and_activation(double time_delta)
        {
            // generate data at each time for each ride and store in matrix (new data form)
            ride_repository.calculate_position_data_for_all_rides( time_delta);
            

        }
        

        public void clear_position_data()
        {
            ride_repository.clear_data_of_all_rides();
        }

        public void ride_pos_check()
        {
            ride_repository.calculate_distance_check_for_all_rides();
        }

        public void sort_sections_time_travel_check()
        {
            ride_repository.debugg_sort_section_time_travel();
        }

        public void DC_section_time_travel_check()
        {
            ride_repository.debugg_DC_section_time_travel();
        }


        public void save_position_data_to_file()
        {
            ride_repository.save_rides_data_in_file();
        }

        public void statistics_of_directions()
        {
            ride_repository.calculate_direction_statistics();

        }

        public void search_for_discontinuity_in_rides_position_data(float max_discontiniuity)
        {

            var (list_of_rides_with_dis, number_of_rides ,
                time_of_start, time_of_end,
                section_start,section_end,
                distance_into_section_start, distance_into_section_end,
                track_start,track_end,
                dis_into_track_start, dis_into_track_end) 
                = ride_repository.check_rides_for_discontiniuity_of_position(max_discontiniuity);
            Console.WriteLine("number of rides with position discontiniuity: {0}", number_of_rides);

        }

        public void search_for_rides_activation_discontinuity()
        {
           var(list_of_rides, number_of_rides)  = ride_repository.check_rides_for_discontinuinty_of_activation();

            Console.WriteLine("number of rides with activation discontinuity: {0} ", number_of_rides);
        }

        public void statistisc_of_time(double interval, double start_time)
        {
            ride_repository.calculate_load_in_time_statistics(interval,start_time);
        }

        public void read_data()
        {
            //wczytuje dane

            string zawartoscPliku = File.ReadAllText(@"d:/dane_pozycji.txt");
            string[] wiersze = zawartoscPliku.Split('\n');


            //czytam header i zapisuje podstawowe dane analizy

            //czas poczatku
            var start_time = double.Parse(wiersze[0].Split(',')[1]);
            var czas_konca = double.Parse(wiersze[1].Split(',')[1]);
            var delta_T = double.Parse(wiersze[2].Split(',')[1]);
            var ilosc_ride = int.Parse(wiersze[3].Split(',')[1]);

            Console.WriteLine("czas poczatku {0}", start_time);
            Console.WriteLine("czas konca {0}", czas_konca);
            Console.WriteLine("delta T {0}", delta_T);
            Console.WriteLine("ilosc kapsol {0}", ilosc_ride);

            //delta_T

            //ilosc ride

            //wrzucam wszystkie dane ride do jegged array 
            //lista czasu
            //lista dla kazdej kapsuly (double[], bool)

            //var wynik = double.Parse(liczby[0]);
            //var wynik_2 = bool.Parse(liczby[4]);

            //jak bede wyswietlac to szumak indexu najblirzszego czasu (robie to algebraicznie)
            //i wyswietlam wszystke aktywne kapsuly dla danych tego indexu
            // z szarpaniem poradze sobie tak ze dowale mniejsz delta t 

        }
    }

   
    
    
}
