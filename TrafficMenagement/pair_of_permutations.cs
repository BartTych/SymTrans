namespace Symulation
{
    public class pair_of_permutations
    {
        public Permutation first_permutation { get; set; }
        public Permutation second_permutation { get; set; }

        public double least_comon_speed { get; set; }
        public double speed_at_exit { get; set; }
        //public double Time_distance_because_of_acceleration_from_least_common_speed { get; set; }
        //public double Time_distence_because_of_first_perm_intersection_loss { get; set; }
        public double additional_separation_because_of_secondary_pair { get; set; }
        public double additional_separation_because_of_station_min_arrival_separation { get; set; }
        //public double Cumulative_time { get; set; }
        public double required_separation_at_the_end { get; set; }
        
        public void add_additinal_required_time_of_separation(double additional_time)
        {
            required_separation_at_the_end += additional_time;
        }

    }
    
}
