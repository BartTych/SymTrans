using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace Symulation
{

    public class TrackDisplayDataPreparation
    {

        private readonly Symulation_control symulation_control;
        private List<CitySection> _list_of_sections;
        private CityDataStorage city;
        private Track[] _list_of_city_tracks;
        private repository_of_track_dis_data repository_of_track_data;


        //city
        //sekcje
        public TrackDisplayDataPreparation(Symulation_control symulation_control)
        {
            this.symulation_control = symulation_control;
            _list_of_sections = symulation_control.get_city_sections();
            city = symulation_control.get_city();
            _list_of_city_tracks = city.GetListOfAllTracksInCity();
            repository_of_track_data = new repository_of_track_dis_data();
        }
        public void prepare_data_for_track_display()
        {
            // jak to zrobic ?
            // powinienem zminilalizowac liczbe obiektow do renderowania bo inaczej to bedzie strasznie wolno dzialac 
            // jade po kolei po wszystkich sekcjach
            List<int> list_of_tracks = null;

            display_track display;

            float length_of_track;
            float start_x, start_y, start_ang;
            float end_x, end_y, end_ang;
            float middle_x, middle_y, middle_ang;
            
            double length_of_permutation;
            double length_of_traverse;
            double start, start_of_first_change, end_of_first_change, start_of_second_change, end_of_second_change, end;

            int number_of_track;
            double dist_into_affected_track;

            for (int i = 0; i < _list_of_sections.Count; i++)
            {


                // to jest opcja uproszczona , mozna jeszcze zminimalizowac ilosc wyswietlanych sekcji ale narazie to zostawie
                // i zrobie wyswietlanie dla kazdego tracku w sekcji
                // mozna to potencjalnie zmniejszyc przez laczenie trackow ktore maja pomiedzy soba zero kata zmian 

                if(_list_of_sections[i].what_is_the_type_of_this_section() == typeof(DC_section))
                {
                    list_of_tracks = _list_of_sections[i].get_list_of_tracks_in_secton();


                    for (int j = 0; j < list_of_tracks.Count; j++)
                    {
                        length_of_track = (float)city.GetLenghtOfTrack(list_of_tracks[j]);
                        (start_x, start_y, start_ang) = _list_of_city_tracks[list_of_tracks[j]].return_position_and_angle_of_start();
                        (middle_x, middle_y, middle_ang) = _list_of_city_tracks[list_of_tracks[j]].return_position_and_angle_at_center();
                        (end_x, end_y, end_ang) = _list_of_city_tracks[list_of_tracks[j]].return_position_and_angle_of_end();

                        // method is using fields defined abouwe;
                        add_new_track_to_repository();
                    }

                    continue;
                }

                if (_list_of_sections[i].what_is_the_type_of_this_section() == typeof(Sort_section))
                {
                    var permutation = _list_of_sections[i].Get_permutations()[0];  //for sort there is only one permutation



                    length_of_permutation = permutation.get_permutation_length();
                    length_of_traverse = permutation.get_current_length_of_traverse();
                    var speed = permutation.get_entrance_speed();
                    
                    // calculate all characteristic distances
                    start = 0;
                    start_of_first_change = length_of_traverse;
                    end_of_first_change = length_of_traverse + speed * 1;
                    start_of_second_change = length_of_permutation - length_of_traverse -speed * 1;
                    end_of_second_change = length_of_permutation - length_of_traverse;
                    end = length_of_permutation;

                    //odpalam przygotowana metode dla wszystkich kombinacji odcinkow i wszystkich stron 
                    // spoko bylo by zrobic metode dodawania

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(start, start_of_first_change, permutation, 0, 0);
                    add_new_track_to_repository();

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(start_of_first_change, end_of_first_change, permutation, 1, 0);
                    add_new_track_to_repository();

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(start_of_first_change, end_of_first_change, permutation, 1, 1);
                    add_new_track_to_repository();

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(end_of_first_change, start_of_second_change, permutation, 2, 0);
                    add_new_track_to_repository();

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(end_of_first_change, start_of_second_change, permutation, 2, 1);
                    add_new_track_to_repository();

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(start_of_second_change, end_of_second_change, permutation, 3, 0);
                    add_new_track_to_repository();

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(start_of_second_change, end_of_second_change, permutation, 3, 1);
                    add_new_track_to_repository();

                    (length_of_track,
                    start_x, start_y, start_ang,
                    middle_x, middle_y, middle_ang,
                    end_x, end_y, end_ang) = calculate_data_for_sort_section_track_disp(end_of_second_change, end, permutation, 4, 0);
                    add_new_track_to_repository();
                }


                (int, double) get_number_of_track_and_distance_into_it_of_permutation_route_at_distance(double distance, Permutation permutation)
                {
                    double remaining_dis = distance;
                    var permutation_route = permutation.Get_route();

                    for (int k = 0; k < permutation_route.GetLength(0); k++)
                    {
                        if (remaining_dis < city.GetLenghtOfTrack(permutation_route[k][2]))
                            return (permutation_route[k][2], remaining_dis);
                        else
                            remaining_dis -= city.GetLenghtOfTrack(permutation_route[k][2]);

                    }

                    //there can be numerical error so id remaning dis is small it is ok, it is just the end of route 
                    if (remaining_dis < 0.002)
                        return (permutation_route[permutation_route.GetLength(0) - 1][2], city.GetLenghtOfTrack(permutation_route[permutation_route.GetLength(0) - 1][2]));

                    return (-1, -1);
                }

                void add_new_track_to_repository()
                {
                    display = new display_track();
                    display.length_of_track = length_of_track;
                    display.start_pos_x = start_x;
                    display.start_pos_y = start_y;
                    display.start_ang = start_ang;
                    display.midle_pos_x = middle_x;
                    display.midle_pos_y = middle_y;
                    display.midle_ang = middle_ang;
                    display.end_pos_x = end_x;
                    display.end_pos_y = end_y;
                    display.end_ang = end_ang;

                    repository_of_track_data.add_new_track_for_display(display);
                }

                (float, float, float, float, float, float, float, float, float, float) calculate_data_for_sort_section_track_disp(double start_dis, double end_dis, Permutation perm, int type_of_track,int index_of_direction)
                {
                    var speed = perm.get_entrance_speed();
                    double max_translation = speed / 3;
                    double length_of_change = (end_dis - start_dis);
                    length_of_track = (float)length_of_change;

                    (number_of_track, dist_into_affected_track) = get_number_of_track_and_distance_into_it_of_permutation_route_at_distance(start_dis, perm);
                    (start_x, start_y, start_ang) = _list_of_city_tracks[number_of_track].calculate_position__and_tangent_angle_of_point_at_x_from_start(dist_into_affected_track);

                    (number_of_track, dist_into_affected_track) = get_number_of_track_and_distance_into_it_of_permutation_route_at_distance((start_dis + end_dis) / 2, perm);
                    (middle_x, middle_y, middle_ang) = _list_of_city_tracks[number_of_track].calculate_position__and_tangent_angle_of_point_at_x_from_start(dist_into_affected_track);

                    (number_of_track, dist_into_affected_track) = get_number_of_track_and_distance_into_it_of_permutation_route_at_distance(end_dis, perm);
                    (end_x, end_y, end_ang) = _list_of_city_tracks[number_of_track].calculate_position__and_tangent_angle_of_point_at_x_from_start(dist_into_affected_track);

                    //calculation o of translation and angle change
                    float translation_x;
                    float translation_y;
                    float translation_angle = 0;

                    if (index_of_direction == 0)
                        translation_angle = start_ang + 90;
                    if (index_of_direction == 1)
                        translation_angle = start_ang - 90;

                    translation_x = (float)((Math.Cos((translation_angle / 360) * (2 * Math.PI))) * max_translation);
                    translation_y = (float)((Math.Sin((translation_angle / 360) * (2 * Math.PI))) * max_translation);

                    var angle_change = (float)((Math.Atan((max_translation ) / length_of_change)/(Math.PI * 2))*360);

                    if (type_of_track == 0)
                        return (length_of_track, start_x, start_y, start_ang, middle_x, middle_y, middle_ang, end_x, end_y, end_ang);
                    
                    if (type_of_track == 1 && index_of_direction == 0 ) //first change of track , side A
                        return (length_of_track, start_x, start_y, start_ang + angle_change, middle_x + translation_x/2, middle_y + translation_y/2, middle_ang + angle_change, end_x + translation_x, end_y + translation_y, end_ang + angle_change);
                    
                    if (type_of_track == 1 && index_of_direction == 1) // first change of track, side B
                        return (length_of_track, start_x, start_y, start_ang - angle_change, middle_x + translation_x / 2, middle_y + translation_y / 2, middle_ang - angle_change, end_x + translation_x, end_y + translation_y, end_ang - angle_change);

                    if (type_of_track == 2 && index_of_direction == 0) // sort, side A
                        return (length_of_track, start_x + translation_x, start_y + translation_y, start_ang , middle_x + translation_x, middle_y + translation_y, middle_ang, end_x + translation_x, end_y + translation_y, end_ang );
                    
                    if (type_of_track == 2 && index_of_direction == 1) // sort , side B
                        return (length_of_track, start_x + translation_x, start_y + translation_y, start_ang , middle_x + translation_x, middle_y + translation_y, middle_ang, end_x + translation_x, end_y + translation_y, end_ang );

                    if (type_of_track == 3 && index_of_direction == 0) // second change of track, side A
                        return (length_of_track, start_x + translation_x, start_y + translation_y, start_ang - angle_change, middle_x + translation_x/2, middle_y + translation_y/2, middle_ang - angle_change, end_x, end_y, end_ang - angle_change);
                    
                    if (type_of_track == 3 && index_of_direction == 1) // second change of track, side B
                        return (length_of_track, start_x + translation_x, start_y + translation_y, start_ang + angle_change, middle_x + translation_x / 2, middle_y + translation_y / 2, middle_ang + angle_change, end_x, end_y, end_ang + angle_change);

                    if (type_of_track == 4)
                        return (length_of_track, start_x, start_y, start_ang, middle_x, middle_y, middle_ang, end_x, end_y, end_ang);

                    else
                        return (-1, -1, -1, -1, -1, -1, -1, -1, -1, -1);

                }

            }
        }

        /// <summary>
        /// save calculated track position data , to txt file as csv 
        /// </summary>
        public void save_data_to_file()
        {
            repository_of_track_data.save_tracks_data_in_file();
        }

    }
   

    public class repository_of_track_dis_data
    {
        private List<display_track> list_of_tracks_to_disp;

        public repository_of_track_dis_data()
        {
            list_of_tracks_to_disp = new List<display_track>();
        }

        public void add_new_track_for_display(display_track track)
        {
            list_of_tracks_to_disp.Add(track);
        }

        public int get_number_of_tracks()
        {
            return list_of_tracks_to_disp.Count;
        }

        public display_track get_track_to_disp(int number)
        {
            return list_of_tracks_to_disp[number];
        }


        public List<display_track> get_list_of_disp_tracks()
        {
            return list_of_tracks_to_disp;
        }

        public void save_tracks_data_in_file()
        {
            if (list_of_tracks_to_disp.Count == 0)
            {
                Console.WriteLine("no track to disp");
            }
            else
            {

                var builder = new StringBuilder();

                List<string> list_of_position_data = new List<string>();


                int number_of_tracks = get_number_of_tracks();
                float[] pos_data;
                bool active;

                // twoze header:
                // czas startu
                // czas konca
                // delta T
                // ilosc kapsol
                // time, pos_x, pos_y, ang (pos_x, pos_y, ang) ...

                builder.Append("ilosc trackow: ,")
                       .Append(number_of_tracks);
                //.AppendLine();


                list_of_position_data.Add(builder.ToString());
                //File.WriteAllText(@"d:/dane_pozycji.txt", builder.ToString());

                builder.Clear();
                builder.Append("Length of track")
                       .Append(",start_pos_x")
                       .Append(",start_pos_y")
                       .Append(",start_ang")
                       .Append(",midlle_pos_x")
                       .Append(",midlle_pos_y")
                       .Append(",midlle_ang")
                       .Append(",end_pos_x")
                       .Append(",end_pos_y")
                       .Append(",end_ang");

                list_of_position_data.Add(builder.ToString());
                //File.AppendAllText(@"d:/dane_pozycji.txt", builder.ToString());

                for (int i = 0; i < number_of_tracks; i++)
                {
                    builder.Clear();
                    //builder.AppendLine();
                    builder.Append(list_of_tracks_to_disp[i].length_of_track.ToString())
                        .Append("," + list_of_tracks_to_disp[i].start_pos_x.ToString())
                        .Append("," + list_of_tracks_to_disp[i].start_pos_y.ToString())
                        .Append("," + list_of_tracks_to_disp[i].start_ang.ToString())
                        .Append("," + list_of_tracks_to_disp[i].midle_pos_x.ToString())
                        .Append("," + list_of_tracks_to_disp[i].midle_pos_y.ToString())
                        .Append("," + list_of_tracks_to_disp[i].midle_ang.ToString())
                        .Append("," + list_of_tracks_to_disp[i].end_pos_x.ToString())
                        .Append("," + list_of_tracks_to_disp[i].end_pos_y.ToString())
                        .Append("," + list_of_tracks_to_disp[i].end_ang.ToString());

                    list_of_position_data.Add(builder.ToString());
                    //File.AppendAllText(@"d:/dane_pozycji.txt", builder.ToString());

                }

                //File.WriteAllLines(@"tracks_position_data.txt", list_of_position_data);
                File.WriteAllLines(@"/Users/bart/data_visual_studio/tracks_position_data.txt", list_of_position_data);

            }

        }

    }



    public struct display_track
    {
        public float length_of_track { get; set; }

        public float start_pos_x { get; set; }
        public float start_pos_y { get; set; }
        public float start_ang { get; set; }

        public float midle_pos_x { get; set; }
        public float midle_pos_y { get; set; }
        public float midle_ang { get; set; }

        public float end_pos_x { get; set; }
        public float end_pos_y { get; set; }
        public float end_ang { get; set; }

    }
    
}
