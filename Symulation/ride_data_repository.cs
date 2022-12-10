using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Symulation
{
    public class ride_data_repository
    {
        private List<Ride> list_of_rides;
        private readonly CityDataStorage city;

        public ride_data_repository( CityDataStorage city)
        {
            list_of_rides = new List<Ride>();
            this.city = city;
        }

        public int return_number_of_rides()
        {
            return list_of_rides.Count;
        }

        public (double first_start, double last_end) return_time_span_of_rides()
        {
            //for each ride read start and end time

            var (first_start, last_end) = list_of_rides[0].return_start_and_end_time();
            


            double start;
            double end;

            for(int i = 0; i < list_of_rides.Count; i++)
            {
                (start, end) = list_of_rides[i].return_start_and_end_time();

                if(start < first_start)
                    first_start = start;
                if(end > last_end)
                    last_end = end;
            }

            return (first_start, last_end);
        }

        public void calculate_position_data_for_all_rides(double time_delta)
        {
            if (list_of_rides.Count == 0)
            {
                Console.WriteLine("no rides calculated");

            }

            else
            {
                var (start, end) = return_time_span_of_rides();

                var time = start;
                double delta = time_delta;

                float pos_x;
                float pos_y;
                float ang;

                while (time < end)
                {
                    for (int i = 0; i < list_of_rides.Count; i++)
                    {
                        if (list_of_rides[i].is_this_global_time_within_ride_time(time))
                        {
                            (pos_x, pos_y, ang) = list_of_rides[i].calculate_position_and_angle_of_pod_at_time(time);
                            list_of_rides[i].add_row_to_position_data((float)time, pos_x, pos_y, ang, true);
                        }
                        else
                        {
                            list_of_rides[i].add_row_to_position_data((float)time, 0, 0, 0, false);
                        }
                    }

                    time += delta;
                }


            }
        }

        public void clear_data_of_all_rides()
        {
            for(int i = 0; i < list_of_rides.Count; i++)
            {
                list_of_rides[i].clear_position_data();
            }
        }

        public void calculate_distance_check_for_all_rides()
        {
            for(int i = 0; i < list_of_rides.Count; i++)
            {
                list_of_rides[i].calculate_distance_into_route_check();
            }
        }

        public void save_rides_data_in_file()
        {
            if (list_of_rides.Count == 0)
            {
                Console.WriteLine("no rides to gen data");
            }
            else
            {

            var builder = new StringBuilder();

            List<string> list_of_position_data = new List<string>();


            list_of_rides[0].gen_position_data_description();
            var (start_time, end_time, time_delta, number_of_data_points) = list_of_rides[0].return_position_data_description();
            int ilosc_kapsol = return_number_of_rides();
            float[] pos_data;
            bool active;

            // twoze header:
            // czas startu
            // czas konca
            // delta T
            // ilosc kapsol
            // time, pos_x, pos_y, ang (pos_x, pos_y, ang) ...

            builder.Append("czas startu: ,")
                   .Append(start_time)
                   .AppendLine()
                   .Append("czas konca: ,")
                   .Append(end_time)
                   .AppendLine()
                   .Append("delta_T: ,")
                   .Append(time_delta)
                   .AppendLine()
                   .Append("ilosc ride: ,")
                   .Append(ilosc_kapsol)
                   .AppendLine()
                   .Append("ilosc ponktow pozycji: ,")
                   .Append(number_of_data_points);
                   //.AppendLine();


            list_of_position_data.Add(builder.ToString());
            //File.WriteAllText(@"d:/dane_pozycji.txt", builder.ToString());

            builder.Clear();
            builder.Append("time");
            
            for(int i = 0; i < list_of_rides.Count; i++)
            {
                builder.Append(",pos_x")
                    .Append(",pos_y")
                    .Append(",ang")
                    .Append(",active");
            }

            list_of_position_data.Add(builder.ToString());
            //File.AppendAllText(@"d:/dane_pozycji.txt", builder.ToString());

            for(int i = 0; i < number_of_data_points; i++)
            {
                builder.Clear();
                //builder.AppendLine();
                builder.Append(start_time + time_delta * i);

                for(int j = 0; j< list_of_rides.Count; j++)
                {
                    (pos_data, active) = list_of_rides[j].return_position_data__and_activation_at_index(i);
                    builder.Append("," + pos_data[1].ToString())
                        .Append("," + pos_data[2].ToString())
                        .Append("," + pos_data[3].ToString())
                        .Append("," + active);

                }
                list_of_position_data.Add(builder.ToString());
                //File.AppendAllText(@"d:/dane_pozycji.txt", builder.ToString());

            }

            //File.WriteAllLines(@"dane_pozycji.txt", list_of_position_data);
                File.WriteAllLines(@"/Users/bart/data_visual_studio/pods_position_data.txt", list_of_position_data);
            }

        }

        public void calculate_direction_statistics()
        {
            List<(int, int)> directions = new List<(int, int)>();
            List<int> number_of_rides = new List<int>();

            int start;
            int end;
            int number;

            //number_of_stations
            int number_of_stations = city.NumberOfStations();

            for(int i = 0; i < number_of_stations; i++)
            {
                for(int j = 0; j < number_of_stations; j++)
                {
                    if (i != j)
                    {
                        directions.Add((i, j));
                        number_of_rides.Add(0);
                    }

                }
            }


            for(int i = 0; i < list_of_rides.Count; i++)
            {
                start = list_of_rides[i].start_station;
                end = list_of_rides[i].end_station;

                if (directions.Contains((start, end)))
                {
                    number = number_of_rides[directions.IndexOf((start, end))];
                    number++;
                    number_of_rides[directions.IndexOf((start, end))] = number;
                }
                else
                {
                    directions.Add((start, end));
                    number_of_rides.Add(1);
                }
                   
            }

            



            System.Console.WriteLine("statystyka przejazdow");
            for (int i = 0; i < directions.Count; i++)
            {
                System.Console.WriteLine("kierunek :{0} liczba przejazdow :{1}", directions[i], number_of_rides[i]);

            }

        }

        public void calculate_load_in_time_statistics(double time_interval, double start_time)
        {

            list_of_rides.Sort((x, y) => x.return_start_time().CompareTo(y.return_start_time()));
            System.Console.WriteLine("statystyka w czasie");
            System.Console.WriteLine("interval : {0}", time_interval);

            List<double> List_of_interwals = new List<double>();
            List_of_interwals.Add(time_interval + start_time);
            List<int> density = new List<int>();
            density.Add(0);

            for(int i = 0; i < list_of_rides.Count; i++)
            {
                if (list_of_rides[i].return_start_time() < List_of_interwals[List_of_interwals.Count - 1])
                {
                    density[density.Count - 1]++;
                }
                else
                {
                    List_of_interwals.Add((List_of_interwals[List_of_interwals.Count-1]) + time_interval);
                    density.Add(0);
                    i--;
                }



            }

            for(int i = 0; i < List_of_interwals.Count; i++)
            {
                System.Console.WriteLine("time interval : {0}  ilosc kapsol {1}", List_of_interwals[i], density[i]);
            }

        }



        public (List<Ride> rides_with_discontinuity, int number_of_rides_wth_discontinuity) check_rides_for_discontinuinty_of_activation()
        {
            List<Ride> list_of_rides_with_activation_discontinuity = new List<Ride>();
            // ride should be activated only once in symulation and that disapire
            // so there should be one rising edge event
            // if there is more, something went wrong

            bool previous_state_of_activation;
            bool current_state_of_activation;

            float start_time, end_time, delta_T;
            int number_of_position_points;

            int number_of_rising_edges;

            for (int i = 0; i < list_of_rides.Count; i++)
            {
                list_of_rides[i].gen_position_data_description();
                (start_time, end_time, delta_T, number_of_position_points) = list_of_rides[i].return_position_data_description();

                number_of_rising_edges = 0;

                for(int j = 1; j < number_of_position_points; j++)
                {
                    current_state_of_activation = list_of_rides[i].return_activation_state_at_index_of_position_data(j);
                    previous_state_of_activation = list_of_rides[i].return_activation_state_at_index_of_position_data(j-1);

                    if(current_state_of_activation==true && previous_state_of_activation==false)
                        number_of_rising_edges++;
                }

                if (number_of_rising_edges > 1)
                    list_of_rides_with_activation_discontinuity.Add(list_of_rides[i]);
            }

            return (list_of_rides_with_activation_discontinuity, list_of_rides_with_activation_discontinuity.Count);
        }

        public (List<Ride> rides_with_discontinuity, int number_of_rides_with_discontinuity ,

            List<float> time_of_start, List<float> time_of_end,
            List<int> section_of_discontiniuity_start, List<int> section_of_discontiniuity_end,
            List<double> distance_into_section_discontinuity_start, List<double> distance_into_section_discontnuity_end,
            List<int> track_start, List<int> track_end,
            List<double> dist_into_track_start, List<double> dist_into_track_end)

            check_rides_for_discontiniuity_of_position(float max_discontinuity)
        {
            List<Ride> list_of_rides_with_discontinuity = new List<Ride>();
            
            List<float> time_of_discontiniuity_end = new List<float>();
            List<float> time_of_discontiniuity_start = new List<float>();
            
            List<int> section_of_discontiniuity_start = new List<int>();
            List<int> section_of_discontiniuity_end = new List<int>();
            
            List<double> distance_into_section_discontinuity_start = new List<double>();
            List<double> distance_into_section_discontnuity_end = new List<double>();
            
            List<int> track_start = new List<int>();
            List<int> track_end = new List<int>();

            List<double> dist_into_track_start = new List<double>();
            List<double> dist_into_track_end = new List<double>();



            bool previous_state_of_activation;
            bool current_state_of_activation;


            float start_time, end_time, delta_T;
            int number_of_position_points;

            int index_of_activation=0;
            int index_of_deactivation=1;

            double start_dist;
            double end_dist;

            float[] pos;
            float[] prev_pos;

            float translation;

            int section_start_index;
            int section_end_index;

            double dist_into_section_start;
            double dist_into_section_end;

            int _track_start;
            int _track_end;

            double _dist_into_track_start;
            double _dist_into_track_end;


            for (int i = 0; i < list_of_rides.Count; i++)
            {
                list_of_rides[i].gen_position_data_description();
                (start_time, end_time, delta_T, number_of_position_points) = list_of_rides[i].return_position_data_description();


                for (int j = 1; j < number_of_position_points; j++)
                {
                    current_state_of_activation = list_of_rides[i].return_activation_state_at_index_of_position_data(j);
                    previous_state_of_activation = list_of_rides[i].return_activation_state_at_index_of_position_data(j - 1);

                    if (current_state_of_activation == true && previous_state_of_activation == false)
                        index_of_activation = j;

                    if (current_state_of_activation == false && previous_state_of_activation == true)
                        index_of_deactivation = j;
                }

                for(int k = index_of_activation + 2; k < index_of_deactivation; k++)
                {

                    pos = list_of_rides[i].return_positon_data_at_index(k);
                    prev_pos = list_of_rides[i].return_positon_data_at_index(k-1);

                    translation = (float)Math.Pow( Math.Pow(pos[1] - prev_pos[1], 2) + Math.Pow(pos[2] - prev_pos[2], 2), 0.5);

                    if (translation > max_discontinuity)
                    {
                        list_of_rides_with_discontinuity.Add(list_of_rides[i]);
                        time_of_discontiniuity_end.Add((float)list_of_rides[i].debug_time_convert_to_ride_time(pos[0]));
                        time_of_discontiniuity_start.Add((float)list_of_rides[i].debug_time_convert_to_ride_time(prev_pos[0]));


                        start_dist = list_of_rides[i].get_distance_at_time_from_profile(list_of_rides[i].debug_time_convert_to_ride_time(pos[0]));
                        end_dist = list_of_rides[i].get_distance_at_time_from_profile(list_of_rides[i].debug_time_convert_to_ride_time(prev_pos[0]));


                        (section_start_index, dist_into_section_start) = list_of_rides[i].get_index_of_section_and_distance_into_affected_section_at_distance(start_dist);
                        (section_end_index, dist_into_section_end) = list_of_rides[i].get_index_of_section_and_distance_into_affected_section_at_distance(end_dist);

                        section_of_discontiniuity_start.Add(section_start_index);
                        section_of_discontiniuity_end.Add(section_end_index);

                        distance_into_section_discontinuity_start.Add(dist_into_section_start);
                        distance_into_section_discontnuity_end.Add(dist_into_section_end);

                        (_track_start, _dist_into_track_start) = list_of_rides[i].get_number_of_track_and_distance_into_it_of_modified_route_at_distance(start_dist);
                        (_track_end, _dist_into_track_end) = list_of_rides[i].get_number_of_track_and_distance_into_it_of_modified_route_at_distance(end_dist);

                        track_start.Add(_track_start);
                        track_end.Add(_track_end);

                        dist_into_track_start.Add(_dist_into_track_start);
                        dist_into_track_end.Add(_dist_into_track_end);



                        //dobrze bedzie tu dadac jeszcze liczenie sekcji, tracku gdzie to sie stalo it
                        //zeby moc zidentyfikowac co tam sie odpierdala 
                        break;
                    }
                }
            }

            return (list_of_rides_with_discontinuity, list_of_rides_with_discontinuity.Count,
                time_of_discontiniuity_start, time_of_discontiniuity_end,
                section_of_discontiniuity_start,section_of_discontiniuity_end,
                distance_into_section_discontinuity_start,distance_into_section_discontnuity_end,
                track_start,track_end,
                dist_into_track_start,dist_into_track_end);

        }


        /// <summary>
        /// Filter by deleting all rides not meeting criteria of driving thru one of the required traverse list.
        /// </summary>
        /// <param name="list_of_traverse"></param>
        public void filter_rides_by_number_of_traverse(List<int> list_of_traverse)
        {
            // ide po wszystkich ride i sprawdzam czy maja w sobie przynajmniej jeden travers z listy
            // jesli nie to usuwam dany ride z pamieci
            List<int> list_of_rides_to_be_removed = new List<int>();

            for(int i = 0; i < list_of_rides.Count; i++)
            {
                if (! list_of_rides[i].is_this_ride_going_thru_one_of_travers_in_list(list_of_traverse))
                    list_of_rides_to_be_removed.Add(i);
            }

            // reverse so after remowing ride form list in next step , indexes are still valid
            // before reverse, order of rides to remove maches that of ride list
            list_of_rides_to_be_removed.Reverse();

            for(int i = 0; i < list_of_rides_to_be_removed.Count; i++)
                list_of_rides.RemoveAt(list_of_rides_to_be_removed[i]);
        }


        public void apply_mods_to_all_rides()
        {
            for (int i = 0; i < list_of_rides.Count; i++)
            {
                list_of_rides[i].restet_ride_city_and_route_to_base_state();
                list_of_rides[i].aplly_all_mods();
            }
        
        }

        

        public void update_profile_for_all_rides()
        {
            //licze profil dla kazdego ride z uzyciem zmodyfikowanego miasta i zmodyfikowanego ride
            //zapisac juz wynik w ride 

            for(int i = 0; i < list_of_rides.Count; i++)
            {

                list_of_rides[i].calculate_profile();


                
            }

        }

        public void debugg_sort_section_time_travel()
        {
            uint number_of_rides_with_error = 0;

            for (int i = 0; i < list_of_rides.Count; i++)
            {
                if (!list_of_rides[i].debugg_sort_section_travel_time())
                    number_of_rides_with_error++;
            }

            Console.WriteLine("number of rides with error in sort mod {0}", number_of_rides_with_error);
        }

        public void debugg_DC_section_time_travel()
        {
            uint number_of_rides_with_error = 0;

            for (int i = 0; i < list_of_rides.Count; i++)
            {
                if(!list_of_rides[i].debug_DC_section_ride_time())
                    number_of_rides_with_error++;
            }

            Console.WriteLine("number of rides with error in DC mod {0}", number_of_rides_with_error);

        }

        public int get_number_of_of_next_free_ride_number()
        {
            return list_of_rides.Count;
        }

        public void add_new_ride(Ride ride)
        {
            list_of_rides.Add(ride);
        }
        
    }
}
