using System;
using System.Collections.Generic;

namespace Symulation
{
    public class Repository_of_windows
    {
        private List<Window> _list_of_city_windows;

        public Repository_of_windows()
        {
            _list_of_city_windows = new List<Window>();
        }
        public List<Window> get_list_of_windows()
        {
            return _list_of_city_windows;
        }

        public void add_window(Window window)
        {
            _list_of_city_windows.Add(window);
        }
        public int get_number_of_next_free_window()
        {
            return _list_of_city_windows.Count;
        }

        

    }

    public abstract class Window
    {
        public abstract int number { get; set; }
        public abstract bool window_is_deactivated { get; set; } //deactivation is used for sorting sections where number of input and outputs is limited 
        
        //public abstract int permutation_which_start_at_this_window { get; set; }
        //public abstract int permutation_which_exit_at_window { get; set; }

        public abstract double start_time { get; set; }
        public abstract double end_time { get; set; }
        public abstract double speed { get; set; } //[m/s]
        public abstract int max_number_of_pods { get; set; }



        //public abstract int number_of_traverse_with_exit_at_this_window { get; set; }
        //public abstract Traverse traverse_with_exit_at_this_window { get; set; }
        //public abstract int number_of_traverse_with_entrance_at_this_window { get; set; }
        //public abstract Traverse traverse_with_entrance_at_this_window { get; set; }

        public abstract void calculate_max_pod_number();
        public abstract bool is_there_space_available();
        public abstract int return_number_of_rides();

        public abstract Type what_is_the_type();

        public abstract void add_ride_to_window(Ride ride);

        public abstract void add_ride_to_window_at_index(Ride ride, int index);

        public abstract List<Ride> get_list_of_rides();
        
        public abstract int get_index_of_ride(Ride ride);

        public abstract int get_index_of_ride(int ride);

        public abstract int get_number_of_ride_at_index(int index);

        public abstract double get_time_of_ride_at_win(int ride);

        public abstract double get_position_of_ride_at_win(int ride);

        public abstract double get_position_of_ride_in_csys_of_traverse_which_starts_at_win(int ride);

        public abstract double get_position_of_ride_in_csys_of_traverse_which_ends_at_win(int ride);
    }


    public class interaction_window:Window
    {
        public override int number { get; set; }

        public override bool window_is_deactivated { get; set; }
        public int permutation_which_start_at_this_window { get; set; }
        public int permutation_which_exit_at_window { get; set; }
        public override double start_time { get; set; }
        public override double end_time { get; set; }
        public override double speed { get; set; } //[m/s]
        public override int max_number_of_pods { get; set; }



        public double position_of_window_in_traverse_end { get; set; }
        public int number_of_traverse_with_exit_at_this_window { get; set; }
        public Traverse traverse_with_exit_at_this_window { get; set; }

        public double position_of_window_in_traverse_start { get; set; }
        public int number_of_traverse_with_entrance_at_this_window { get; set; }
        public Traverse traverse_with_entrance_at_this_window  { get; set; }


        //list of rides thru that window
        //order is changed when new ride is added 
        private List<Ride> list_of_rides_thru_window = new List<Ride>();
        private List<ride_thru_window> rides_order = new List<ride_thru_window>();



        public interaction_window deep_copy_of_window(int new_number)
        {
            interaction_window window = new interaction_window();
            window.number = new_number;
            window.permutation_which_start_at_this_window=permutation_which_start_at_this_window;
            window.permutation_which_exit_at_window=permutation_which_exit_at_window;
            window.start_time = start_time;
            window.end_time = end_time;
            window.number_of_traverse_with_exit_at_this_window = number_of_traverse_with_exit_at_this_window;
            window.number_of_traverse_with_entrance_at_this_window = number_of_traverse_with_entrance_at_this_window;
            window.traverse_with_entrance_at_this_window = traverse_with_entrance_at_this_window;
            window.traverse_with_exit_at_this_window = traverse_with_exit_at_this_window;


            return window;
        }
        public start_window deep_copy_of_window_for_start(int new_number)
        {
            start_window window = new start_window();
            window.number = new_number;
            window.permutation_which_start_at_this_window = permutation_which_exit_at_window; 
            
            window.start_time = start_time;
            window.end_time = end_time;
            window.traverse_with_entrance_at_this_window   = traverse_with_exit_at_this_window;
            window.number_of_traverse_with_entrance_at_this_window = number_of_traverse_with_exit_at_this_window;

            return window;
        }
        public end_window deep_copy_of_window_for_stop(int new_number)
        {
            end_window window = new end_window();
            window.number = new_number;
            window.permutation_which_exit_at_window = permutation_which_start_at_this_window;
            window.start_time = start_time;
            window.end_time = end_time;
            window.traverse_with_exit_at_this_window = traverse_with_entrance_at_this_window ;
            window.number_of_traverse_with_exit_at_this_window = number_of_traverse_with_entrance_at_this_window;

            return window;
        }

        public override bool is_there_space_available()
        {
            if (window_is_deactivated)
                return false;
            else if (max_number_of_pods > list_of_rides_thru_window.Count)
                return true;
            else
                return false;
        }

        public override void calculate_max_pod_number()
        {
            double length = (end_time - start_time) * speed;
            float length_of_pod = 2;
            float min_distance_between_pods = 0.5f;
            max_number_of_pods = (int)Math.Floor((length - min_distance_between_pods) / (length_of_pod + min_distance_between_pods));

        }

        public override int return_number_of_rides()
        {
            return list_of_rides_thru_window.Count;
        }

        public override void add_ride_to_window(Ride ride)
        {
            list_of_rides_thru_window.Add(ride);
        }

        public override void add_ride_to_window_at_index(Ride ride, int index)
        {
            list_of_rides_thru_window.Insert(index, ride);
        }

        public override List<Ride> get_list_of_rides()
        {
            return list_of_rides_thru_window;
        }

        public override int get_index_of_ride(Ride ride)
        {
            for(int i = 0; i < list_of_rides_thru_window.Count; i++)
            {
                if(list_of_rides_thru_window[i] == ride)
                    return i;
            }
            return -1;

        }

        public override int get_index_of_ride(int ride)
        {
            for (int i = 0; i < list_of_rides_thru_window.Count; i++)
            {
                if (list_of_rides_thru_window[i].get_number_of_ride() == ride)
                    return i;
            }
            return -1;
        }

        public override int get_number_of_ride_at_index(int index)
        {
            return list_of_rides_thru_window[index].get_number_of_ride();
        }

        public override double get_time_of_ride_at_win(int ride)
        {
            //int index = get_index_of_ride(ride);

            double position = get_position_of_ride_at_win(ride);

            // speed -> win speed

            // win start time

            return start_time + position / speed;
        }

        public override double get_position_of_ride_at_win(int ride)
        {
            int index = get_index_of_ride(ride);

            // przeliczenia indexu na pozycje
            // strefa 0.5 m wolnego z przodu
            double dist = 0.5;


            dist = dist + 1 + index * 2.5; // 1m to polowa dlugosci kapsuly, 2.5 to (polowa kapsuly 1 +odleglosc miedzy kapsulami 0.5 + polowa kolejnej kapsuly)

            return dist;
        }


        public override double get_position_of_ride_in_csys_of_traverse_which_starts_at_win(int ride)
        {
            return position_of_window_in_traverse_start + get_position_of_ride_at_win(ride);
        }



        public override double get_position_of_ride_in_csys_of_traverse_which_ends_at_win(int ride)
        {
            return position_of_window_in_traverse_end + get_position_of_ride_at_win(ride);
        }




        public override Type what_is_the_type()
        {
            return typeof(interaction_window);
        }
    }

    public class start_window:Window
    {
        public override int number { get; set; }

        public override bool window_is_deactivated { get; set; }
        public int permutation_which_start_at_this_window { get; set; }
        //public override int permutation_which_exit_at_window { get; set; }
        public override double start_time { get; set; }
        public override double end_time { get; set; }
        public override double speed { get; set; } //[m/s]
        public override int max_number_of_pods { get; set; }

        //public override int number_of_traverse_with_exit_at_this_window { get; set; }
        //public override Traverse traverse_with_exit_at_this_window { get; set; }
        public double position_of_window_begining_in_traverse_with_start_at_this_window { get; set; }

        public int number_of_traverse_with_entrance_at_this_window { get; set; }
        public Traverse traverse_with_entrance_at_this_window { get; set; }


        //list of rides thru that window
        private List<Ride> list_of_rides_thru_window = new List<Ride>();
        private List<ride_thru_window> rides_order = new List<ride_thru_window>();

        //public interaction_window deep_copy_of_window(int new_number)
        //{
        //    interaction_window window = new interaction_window();
        //    window.number = new_number;
        //    window.permutation_which_start_at_this_window = permutation_which_start_at_this_window;
        //    window.permutation_which_exit_at_window = permutation_which_exit_at_window;
        //    window.start_time = start_time;
        //    window.end_time = end_time;
        //    window.number_of_traverse_with_exit_at_this_window = number_of_traverse_with_exit_at_this_window;
        //    window.number_of_traverse_with_entrance_at_this_window = number_of_traverse_with_entrance_at_this_window;

        //    return window;
        //}
        //public interaction_window deep_copy_of_window_for_start(int new_number)
        //{
        //    interaction_window window = new interaction_window();
        //    window.number = new_number;
        //    window.permutation_which_start_at_this_window = permutation_which_exit_at_window;

        //    window.start_time = start_time;
        //    window.end_time = end_time;
        //    window.traverse_with_entrance_at_this_window = traverse_with_exit_at_this_window;
        //    window.number_of_traverse_with_entrance_at_this_window = number_of_traverse_with_exit_at_this_window;

        //    return window;
        //}
        //public interaction_window deep_copy_of_window_for_stop(int new_number)
        //{
        //    interaction_window window = new interaction_window();
        //    window.number = new_number;
        //    window.permutation_which_exit_at_window = permutation_which_start_at_this_window;
        //    window.start_time = start_time;
        //    window.end_time = end_time;
        //    window.traverse_with_exit_at_this_window = traverse_with_entrance_at_this_window;
        //    window.number_of_traverse_with_exit_at_this_window = number_of_traverse_with_entrance_at_this_window;

        //    return window;
        //}

        public override bool is_there_space_available()
        {
            if (window_is_deactivated)
                return false;
            else if (max_number_of_pods > list_of_rides_thru_window.Count)
                return true;
            else
                return false;
        }

        public override void calculate_max_pod_number()
        {
            double length = (end_time - start_time) * speed;
            float length_of_pod = 2;
            float min_distance_between_pods = 0.5f;
            max_number_of_pods = (int)Math.Floor((length - min_distance_between_pods) / (length_of_pod + min_distance_between_pods));

        }

        public override int return_number_of_rides()
        {
            return list_of_rides_thru_window.Count;
        }

        public override void add_ride_to_window(Ride ride)
        {
            list_of_rides_thru_window.Add(ride);
        }

        public override void add_ride_to_window_at_index(Ride ride, int index)
        {
            list_of_rides_thru_window.Insert(index, ride);
        }

        public override List<Ride> get_list_of_rides()
        {
            return list_of_rides_thru_window;
        }
        public override int get_index_of_ride(Ride ride)
        {
            for (int i = 0; i < list_of_rides_thru_window.Count; i++)
            {
                if (list_of_rides_thru_window[i] == ride)
                    return i;
            }
            return -1;

        }

        public override int get_index_of_ride(int ride)
        {
            for (int i = 0; i < list_of_rides_thru_window.Count; i++)
            {
                if (list_of_rides_thru_window[i].get_number_of_ride() == ride)
                    return i;
            }
            return -1;
        }

        public override int get_number_of_ride_at_index(int index)
        {
            return list_of_rides_thru_window[index].get_number_of_ride();
        }

        public override double get_time_of_ride_at_win(int ride)
        {
            //int index = get_index_of_ride(ride);

            double position = get_position_of_ride_at_win(ride);

            // speed -> win speed

            // win start time

            return start_time + position / speed;
        }

        public override double get_position_of_ride_at_win(int ride)
        {
            int index = get_index_of_ride(ride);

            // przeliczenia indexu na pozycje
            // strefa 0.5 m wolnego z przodu
            double dist = 0.5;


            dist = dist + 1 + index * 2.5; // 1m to polowa dlugosci kapsuly, 2.5 to (polowa kapsuly 1 +odleglosc miedzy kapsulami 0.5 + polowa kolejnej kapsuly)

            return dist;
        }

        public override double get_position_of_ride_in_csys_of_traverse_which_starts_at_win(int ride)
        {
            return position_of_window_begining_in_traverse_with_start_at_this_window + get_position_of_ride_at_win(ride);
        }



        public override double get_position_of_ride_in_csys_of_traverse_which_ends_at_win(int ride)
        {
            throw new NotImplementedException();
        }


        public override Type what_is_the_type()
        {
            return typeof(start_window);
        }
    }

    public class end_window:Window
    {
        public override int number { get; set; }

        public override bool window_is_deactivated { get; set; }
        //public override int permutation_which_start_at_this_window { get; set; }
        public int permutation_which_exit_at_window { get; set; }
        public override double start_time { get; set; }
        public override double end_time { get; set; }
        public override double speed { get; set; } //[m/s]
        public override int max_number_of_pods { get; set; }


        public double position_of_window_begining_in_traverse_with_ends_at_this_window { get; set; }
        public int number_of_traverse_with_exit_at_this_window { get; set; }
        public Traverse traverse_with_exit_at_this_window { get; set; }
        //public override int number_of_traverse_with_entrance_at_this_window { get; set; }
        //public override Traverse traverse_with_entrance_at_this_window { get; set; }


        //list of rides thru that window
        private List<Ride> list_of_rides_thru_window = new List<Ride>();
        private List<ride_thru_window> rides_order = new List<ride_thru_window>();

        //public interaction_window deep_copy_of_window(int new_number)
        //{
        //    interaction_window window = new interaction_window();
        //    window.number = new_number;
        //    window.permutation_which_start_at_this_window = permutation_which_start_at_this_window;
        //    window.permutation_which_exit_at_window = permutation_which_exit_at_window;
        //    window.start_time = start_time;
        //    window.end_time = end_time;
        //    window.number_of_traverse_with_exit_at_this_window = number_of_traverse_with_exit_at_this_window;
        //    window.number_of_traverse_with_entrance_at_this_window = number_of_traverse_with_entrance_at_this_window;

        //    return window;
        //}
        //public interaction_window deep_copy_of_window_for_start(int new_number)
        //{
        //    interaction_window window = new interaction_window();
        //    window.number = new_number;
        //    window.permutation_which_start_at_this_window = permutation_which_exit_at_window;

        //    window.start_time = start_time;
        //    window.end_time = end_time;
        //    window.traverse_with_entrance_at_this_window = traverse_with_exit_at_this_window;
        //    window.number_of_traverse_with_entrance_at_this_window = number_of_traverse_with_exit_at_this_window;

        //    return window;
        //}
        //public interaction_window deep_copy_of_window_for_stop(int new_number)
        //{
        //    interaction_window window = new interaction_window();
        //    window.number = new_number;
        //    window.permutation_which_exit_at_window = permutation_which_start_at_this_window;
        //    window.start_time = start_time;
        //    window.end_time = end_time;
        //    window.traverse_with_exit_at_this_window = traverse_with_entrance_at_this_window;
        //    window.number_of_traverse_with_exit_at_this_window = number_of_traverse_with_entrance_at_this_window;

        //    return window;
        //}

        public override bool is_there_space_available()
        {
            if (window_is_deactivated)
                return false;
            else if (max_number_of_pods > list_of_rides_thru_window.Count)
                return true;
            else
                return false;
        }

        public override void calculate_max_pod_number()
        {
            double length = (end_time - start_time) * speed;
            float length_of_pod = 2;
            float min_distance_between_pods = 0.5f;
            max_number_of_pods = (int)Math.Floor((length - min_distance_between_pods) / (length_of_pod + min_distance_between_pods));

        }

        public override int return_number_of_rides()
        {
            return list_of_rides_thru_window.Count;
        }

        public override void add_ride_to_window(Ride ride)
        {
            list_of_rides_thru_window.Add(ride);
        }

        public override void add_ride_to_window_at_index(Ride ride, int index)
        {
            list_of_rides_thru_window.Insert(index, ride);
        }

        public override List<Ride> get_list_of_rides()
        {
            return list_of_rides_thru_window;
        }

        public override int get_index_of_ride(Ride ride)
        {
            for (int i = 0; i < list_of_rides_thru_window.Count; i++)
            {
                if (list_of_rides_thru_window[i] == ride)
                    return i;
            }
            return -1;

        }

        public override int get_index_of_ride(int ride)
        {
            for (int i = 0; i < list_of_rides_thru_window.Count; i++)
            {
                if (list_of_rides_thru_window[i].get_number_of_ride() == ride)
                    return i;
            }
            return -1;
        }

        public override int get_number_of_ride_at_index(int index)
        {
            return list_of_rides_thru_window[index].get_number_of_ride();
        }

        public override double get_time_of_ride_at_win(int ride)
        {
            //int index = get_index_of_ride(ride);

            double position = get_position_of_ride_at_win(ride);

            // speed -> win speed

            // win start time

            return start_time + position / speed;
        }

        public override double get_position_of_ride_at_win(int ride)
        {
            int index = get_index_of_ride(ride);

            // przeliczenia indexu na pozycje
            // strefa 0.5 m wolnego z przodu
            double dist = 0.5;


            dist = dist + 1 + index * 2.5; // 1m to polowa dlugosci kapsuly, 2.5 to (polowa kapsuly 1 +odleglosc miedzy kapsulami 0.5 + polowa kolejnej kapsuly)

            return dist;
        }

        public override double get_position_of_ride_in_csys_of_traverse_which_starts_at_win(int ride)
        {
            throw new NotImplementedException();
        }

        public override double get_position_of_ride_in_csys_of_traverse_which_ends_at_win(int ride)
        {
            return position_of_window_begining_in_traverse_with_ends_at_this_window + get_position_of_ride_at_win(ride);
        }

        public override Type what_is_the_type()
        {
            return typeof(end_window);
        }
    }

    public struct ride_thru_window
    {
        public type_of_ride_thru_windows type_of_ride { get; set; }
        public int ride_number { get; set; }

        public int in_window_structural_number { get; set; }
        public int index_in_window { get; set; }

        public int this_window_structural_number { get; set; }
        public int index_this_window { get; set; }

        public int out_window_structurel_number { get; set; }
        public int index_out_window { get; set; }
    }

    public enum type_of_ride_thru_windows
    {
        start,
        end,
        intersection
    }


}
