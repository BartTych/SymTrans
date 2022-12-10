using System;
using System.Collections.Generic;
using System.Linq;

namespace Symulation
{


    public abstract class BasicStepForGroup : ModOfCityMap
    {
        //private ModOfCityMap Element;

        //tutaj beda pola potrzebne do zdefiniowania modyfikacji
        //jak jed node poczatkowy
        //moze powinienem tu uzycz jakiejs opcji ktorej jeszcze nieznam 
        //albo stworzyc rozne obiekty modyfikacji juz z odpowiednimi polami 
        // troche to brzmi jak abstrakcyjna albo virtualna klasa ta modyfikacja 


        public BasicStepForGroup(CityDataStorage cityDataStorage):base(cityDataStorage)
        {
            //Element = new ModOfProfile(cityDataStorage);
        }

        public abstract (int error_code, int[][] new_route) ApplyMod(int star_node,double distance ,int[][] route);



    }

    public class DelayAcceleration : BasicStepForGroup   //version for city sections 
    {
        private readonly CityDataStorage _cityDataStorage;

        public DelayAcceleration(CityDataStorage cityDataStorage) :base(cityDataStorage) 
        {
            this._cityDataStorage = cityDataStorage;
        }

        // start track, pierwszy track zmodyfikowanej sekcji 
        // distance how acceleration is moved
        // route pomiadzy noudami permutacji 

        
        public override (int error_code,int[][] new_route) ApplyMod(int start_track, double distance, int[][] permutation_route) 
        {
            var start_speed_limit = check_mod_start_speed(start_track, permutation_route);

            // modyfikuje tutaj miasto ale w wersji skopiownej do danego Ride// jesli to test przytworzeniu sekcji to uzywam jakiejs kopii testowej wrzuconej do trawersu testowego 
            // wiec powinno tu byc odniesienie do miasta przez pointer     

            // odpalam mod od nouda do przodu i dostaje liste trackow po drodze , afekektowny track ,odleglosc w glab affectowanego tracku i error code 
            var (affected_tracks, distance_into_last_affected_track, error_code) = SearchForLocationOfOthereSideOfMod_AlongTraffic(start_track, distance);
            // tu jest blad taki ze moge przegapic moment wyjazdu poza permutacje , powinienem napisac inna metode ktora bedzie to robic lepiej ale narazie szkoda mi czasu 
            // bo to niewiele wnosi, nawet teraz.
            var last_affected_track = affected_tracks.Last();
            //sprawdzam te czy dana modyfikacja nieweszla w rejon skrzyzowania co tez niejest ok i jesli tak to zwracam error (dzieje sie to automatycznie przy wyliczaniu pozycji konca moda) 
            if (error_code == 1)
            {
                return (error_code, permutation_route);
            }
            //sprawdzam czy dana modyfikacja niewyszla poza permutacje: jesli tak to generuje okreslony error 
            if( !CheckIfModIsInsidePermutation(permutation_route, last_affected_track))
            {
                error_code = 2;
                return (error_code, permutation_route);
            } 
            
            var (first_slowers_track, is_there_a_slower_track) = CheckForTrackSlowerThanStartPoint(affected_tracks, start_speed_limit);
            // sprawdzam czy niema podrodze jakiegos miejsca gdzie predkosc jest mniejsza niz ta z poczatku modyfikacji i jesli tak zwracan numer tracku
            if (is_there_a_slower_track)
            {
                // jesli jest takie miejsce po drodze to modyfikuje predkosc dla wszystkich trackow wczesniej (skrajny przypadek to pierwszy track tak ma co oznacza brak modyfikacji)

                // to chyba powinna byc nowa kategoria modu a moze nie
                // chodzi o to zeby byl ladny opis tego co zostalo zrobione z miastem tak zeby latwo dalo sie to odtworzyc jak beda wprowadzane kolejne modyfikacji
                // efektywnie tutaj zmiana jest predkosc na liscie trackow 
                // kolejne modyfikacji beda robine tek ze miasto jest sprwadzane do wersji bazowej i wszystkiemody sa nanoszone jeszcze raz
                // inaczej byl by to kosmos zeby to usuwac
                trim_list_of_affected_tracks(affected_tracks, first_slowers_track);
                //zmiana predkosci dla wszystkich trackow ktore pozostaly na liscie 
                ChangeSpeedOfTrack(affected_tracks, start_speed_limit);
                return (error_code, permutation_route);
            }
            else
            {
                //jesli niema takiego miejsca
                //wprowadzam mod tracku, nowy modue itd
                var(new_node, new_track, is_new_track_created) = AddNewNodeToTrackAlongTraffic(last_affected_track, distance_into_last_affected_track);
                // w opisie modu dodane do trawersu:
                //metoda replace last track
                if (is_new_track_created)
                {
                    replace_last_affected_track_after_mod(affected_tracks, new_track);
                    permutation_route = mod_of_route_by_delay_acceleration(permutation_route, new_node, new_track, last_affected_track);

                }
                else
                {
                    remove_last_affected_track_from_list(affected_tracks);
                }
                // modyfikuje predkosci wszystkich trackow po drodze do predkosci poczatkowej plus nowy track utworzony w ramach modu: efektywnie pewna lista
                //System.Console.WriteLine("affected tracks by acceleration mod");
                //foreach (int n in affected_tracks)
                //{
                //    System.Console.WriteLine(n);
                //}
                ChangeSpeedOfTrack(affected_tracks, start_speed_limit);

                return (error_code, permutation_route);
            }



            // dodaje modyfikacje do spisu modyfikacji danego trawersu// moze to byc tez traverse testowy


            // modyfikacja route jesli byl dodany nowy noude

        }
        private void remove_last_affected_track_from_list(List<int> affected_tracks)
        {
            var last_track = affected_tracks.Last();
            affected_tracks.Remove(last_track);
        }
        private int[][] mod_of_route_by_delay_acceleration(int [][] route, int new_node, int new_track, int last_affected_track)
        {

            var index = find_index_of_affected_track_in_route(route, last_affected_track);
            var start_node_of_affected_track = route[index][0];
            var finish_node_of_affected_track = route[index][1];

            var fist_new_step = new int[3] {start_node_of_affected_track,new_node,new_track};
            var second_new_step = new int[3] {new_node,finish_node_of_affected_track,last_affected_track};

            var new_route = create_new_route(route,index,fist_new_step,second_new_step);
            return new_route;
        }
        private int[][] create_new_route(int[][] route, int index, int[] first_new_step, int[] second_new_step)
        {
            var new_route = new int[route.Length + 1][];

            for (int i=0; i<index; i++)
            {
                new_route[i] = (int[])route[i].Clone();
            }

            new_route[index] = first_new_step;
            new_route[index + 1] = second_new_step;

            for(int i = index + 1; i < route.Length; i++)
            {
                new_route[i + 1] = (int[])route[i].Clone();
            }
            return new_route;
        }
        private int find_index_of_affected_track_in_route(int [][] route,int affected_track)
        {
            var index = -1;
            for(int i = 0; i < route.GetLength(0); i++)
            {
                if (route[i][2] == affected_track)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private bool CheckIfModIsInsidePermutation(int [][] permutation_route, int affected_track)
        {
            for (int i = 0; i < permutation_route.Length; i++)
            {
                if (permutation_route[i][2] == affected_track)
                    return true;
            }
            return false;
        }
        private (int track_number, bool IsThereSlowersSection) CheckForTrackSlowerThanStartPoint(List<int> Track , double start_speed_limit)
        {

            double track_speed_limit;
            foreach(int n in Track)
            {
                track_speed_limit = _cityDataStorage.GetSpeedLimitForTrack_m_s(n);
                if (track_speed_limit <= start_speed_limit)
                {
                    return (n, true);
                }
            }
            return (-1, false);
        }
        private void trim_list_of_affected_tracks(List<int> Affected_tracks ,int slower_track)
        {
            var index = Affected_tracks.IndexOf(slower_track);
            var length_of_section_after_first_slower = Affected_tracks.Count-index;

            Affected_tracks.RemoveRange(index, length_of_section_after_first_slower);

        }
        private double check_mod_start_speed(int start_track, int [][] permutation_route)
        {
            int index_of_track_before_start_track = -1;
            for(int i = 0; i < permutation_route.GetLength(0); i++)
            {
                if (permutation_route[i][2] == start_track)
                    index_of_track_before_start_track = i - 1;
            }
            var number_of_track_before_start = permutation_route[index_of_track_before_start_track][2];
            return _cityDataStorage.GetSpeedLimitForTrack_m_s(number_of_track_before_start);

        }
        private void replace_last_affected_track_after_mod(List<int> affected_tracks, int new_track)
        {
            int last_track = affected_tracks.Last();
            affected_tracks.Remove(last_track);
            affected_tracks.Add(new_track);
        }

    }
    public class AccelereateBraking : BasicStepForGroup
    {
        private readonly CityDataStorage _cityDataStorage;

        public AccelereateBraking(CityDataStorage cityDataStorage) : base(cityDataStorage)
        {
            this._cityDataStorage = cityDataStorage;
        }

        public override (int error_code, int[][] new_route) ApplyMod(int start_track, double distance, int[][] permutation_route)
        {
            var start_speed_limit = check_mod_start_speed(start_track, permutation_route);//nierozumiem jak to jest predkosc

            // modyfikuje tutaj miasto ale w wersji skopiownej do danego Ride// jesli to test przytworzeniu sekcji to uzywam jakiejs kopii testowej wrzuconej do trawersu testowego 
            // wiec powinno tu byc odniesienie do miasta przez pointer     

            // odpalam mod od nouda do przodu i dostaje liste trackow po drodze , afekektowny track ,odleglosc w glab affectowanego tracku i error code 
            var (affected_tracks, distance_into_last_affected_track, error_code) = SearchForLocationOfOthereSideOfMod_WrongWay(start_track, distance);

            
            var last_affected_track = affected_tracks.Last();
            //sprawdzam te czy dana modyfikacja nieweszla w rejon skrzyzowania co tez niejest ok i jesli tak to zwracam error (dzieje sie to automatycznie przy wyliczaniu pozycji konca moda) 
            if (error_code == -1)
            {
                return (error_code,permutation_route);
            }
            //sprawdzam czy dana modyfikacja niewyszla poza permutacje: jesli tak to generuje okreslony error 
            if (!CheckIfModIsInsidePermutation(permutation_route, last_affected_track))
            {
                error_code = -2;
                return (error_code, permutation_route);
            }


            // tutaj skonczylem prace


            var (first_slowers_track, is_there_a_slower_track) = CheckForTrackSlowerThanStartPoint(affected_tracks, start_speed_limit);
            // sprawdzam czy niema podrodze jakiegos miejsca gdzie predkosc jest mniejsza niz ta z poczatku modyfikacji i jesli tak zwracan numer tracku
            if (is_there_a_slower_track)
            {
                // jesli jest takie miejsce po drodze to modyfikuje predkosc dla wszystkich trackow wczesniej (skrajny przypadek to pierwszy track tak ma co oznacza brak modyfikacji)


                // to chyba powinna byc nowa kategoria modu a moze nie
                // chodzi o to zeby byl ladny opis tego co zostalo zrobione z miastem tak zeby latwo dalo sie to odtworzyc jak beda wprowadzane kolejne modyfikacji
                // efektywnie tutaj zmiana jest predkosc na liscie trackow 
                // kolejne modyfikacji beda robine tek ze miasto jest sprwadzane do wersji bazowej i wszystkiemody sa nanoszone jeszcze raz
                // inaczej byl by to kosmos zeby to usuwac
                trim_list_of_affected_tracks(affected_tracks, first_slowers_track);
                //zmiana predkosci dla wszystkich trackow ktore pozostaly na liscie 
                ChangeSpeedOfTrack(affected_tracks, start_speed_limit);
                return (error_code, permutation_route);
            }
            else
            {
                //jesli niema takiego miejsca
                //wprowadzam mod ostatniego tracku, nowy modue itd
                var (new_node, new_track, is_new_track_created) = AddNewNodeToTrackTheWrongWay(last_affected_track, distance_into_last_affected_track);
                // w opisie modu dodane do trawersu:
                //metoda replace last track
                //replace_last_affected_track_after_mod(affected_tracks, new_track);
                // modyfikuje predkosci wszystkich trackow po drodze do predkosci poczatkowej plus nowy track utworzony w ramach modu: efektywnie pewna lista
                if (is_new_track_created)
                {
                    permutation_route = mod_of_route_by_accelerated_braking(permutation_route, new_node, new_track, last_affected_track);

                }
                else
                {
                    remove_last_track_from_list(affected_tracks);
                }

                //System.Console.WriteLine("affected tracks by braking mod");
                //foreach (int n in affected_tracks)
                //{
                //    System.Console.WriteLine(n);
                //}
                ChangeSpeedOfTrack(affected_tracks, start_speed_limit);
                return (error_code, permutation_route);
            }

            // dodaje modyfikacje do spisu modyfikacji danego trawersu// moze to byc tez traverse testowy
            // modyfikuje route jesli jest nowy noude
        }

        private void remove_last_track_from_list(List<int> affected_tracks)
        {
            int last_track =affected_tracks.Last();
            affected_tracks.Remove(last_track);
        }
        private int[][] mod_of_route_by_accelerated_braking(int[][] route, int new_node, int new_track, int last_affected_track)
        {

            var index = find_index_of_affected_track_in_route(route, last_affected_track);
            var start_node_of_affected_track = route[index][0];
            var finish_node_of_affected_track = route[index][1];

            var fist_new_step = new int[3] { start_node_of_affected_track, new_node, new_track };
            var second_new_step = new int[3] { new_node, finish_node_of_affected_track, last_affected_track };

            var new_route = create_new_route(route, index, fist_new_step, second_new_step);
            return new_route;
        }

        private int[][] create_new_route(int[][] route, int index, int[] first_new_step, int[] second_new_step)
        {
            var new_route = new int[route.Length + 1][];

            for (int i = 0; i < index; i++)
            {
                new_route[i] = (int[])route[i].Clone();
            }

            new_route[index] = first_new_step;
            new_route[index + 1] = second_new_step;

            for (int i = index + 1; i < route.Length; i++)
            {
                new_route[i + 1] = (int[])route[i].Clone();
            }
            return new_route;
        }

        private int find_index_of_affected_track_in_route(int[][] route, int affected_track)
        {
            var index = -1;
            for (int i = 0; i < route.GetLength(0); i++)
            {
                if (route[i][2] == affected_track)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private bool CheckIfModIsInsidePermutation(int[][] permutation_route, int affected_track)
        {
            for (int i = 0; i < permutation_route.Length; i++)
            {
                if (permutation_route[i][2] == affected_track)
                    return true;
            }
            return false;
        }

        private (int track_number, bool IsThereSlowersSection) CheckForTrackSlowerThanStartPoint(List<int> affected_tracks, double start_speed_limit)
        {

            double track_speed_limit;
            foreach (int n in affected_tracks)
            {
                track_speed_limit = _cityDataStorage.GetSpeedLimitForTrack_m_s(n);
                if (track_speed_limit <= start_speed_limit)
                {
                    return (n, true);
                }
            }
            return (-1, false);
        }

        private void trim_list_of_affected_tracks(List<int> Affected_tracks, int slower_track)
        {
            var index = Affected_tracks.IndexOf(slower_track);
            var length_of_section_after_first_slower = Affected_tracks.Count - index;

            Affected_tracks.RemoveRange(index, length_of_section_after_first_slower);

        }

        private double check_mod_start_speed(int start_track, int[][] permutation_route)
        {
            int index_of_track_after_start_track = -1;
            for (int i = 0; i < permutation_route.GetLength(0); i++)
            {
                if (permutation_route[i][2] == start_track)
                    index_of_track_after_start_track = i + 1;
            }
            var number_of_track_after_start = permutation_route[index_of_track_after_start_track][2];
            return _cityDataStorage.GetSpeedLimitForTrack_m_s(number_of_track_after_start);

        }

        private void replace_last_affected_track_after_mod(List<int> affected_tracks, int new_track)
        {
            int last_track = affected_tracks.Last();
            affected_tracks.Remove(last_track);
            affected_tracks.Add(new_track);
        }




    }

    public class ChangeSpeedFromPointToPoint: ModOfCityMap
    {
        private CityDataStorage cityDataStorage;

        public ChangeSpeedFromPointToPoint(CityDataStorage cityDataStorage):base(cityDataStorage)
        {
            this.cityDataStorage = cityDataStorage;
        }


        public void change_speed_between_nodes(int[][] permutation_route, ref int[][] ride_route, double distance_start, double distance_stop, double speed)
        {
            var start_track = permutation_route[0][2];

            // jest tutaj blad bo jak dodam pierwsza modyfikacje to track poczatku sie zmienia
            // wiec musze go zmienic jesli nowy node jest dodany


            var (affected_tracks, distance_into_last_affected_track, error_code) = SearchForLocationOfOthereSideOfMod_AlongTraffic(start_track, distance_start);
            var last_affected_track = affected_tracks.Last();
            var (first_node, new_track, is_new_track_created) = AddNewNodeToTrackAlongTraffic(last_affected_track, distance_into_last_affected_track);

            if (is_new_track_created)
            {
                ride_route = mod_of_route_by_ne_node(ride_route, first_node, new_track, last_affected_track);
                start_track = new_track;
            }



            var (affected_tracks2, distance_into_last_affected_track2, error_code2) = SearchForLocationOfOthereSideOfMod_AlongTraffic(start_track, distance_stop);
            var last_affected_track2 = affected_tracks2.Last();
            var (second_node, new_track2, is_new_track_created2) = AddNewNodeToTrackAlongTraffic(last_affected_track2, distance_into_last_affected_track2);

            if (is_new_track_created2)
                ride_route = mod_of_route_by_ne_node(ride_route, second_node, new_track2, last_affected_track2);


            //to jest ryzykowne bo jak sie okaze ze niezrobilem nowego noda to bedzie dupa (np jak odleglosc poczatku modu jest identyczna z instniejacym juz noudem)
            //w takiej sytuacji musze odczytac ten noude

            change_speed_between_nodes(ride_route, first_node, second_node, speed);

        }


        private void change_speed_between_nodes(int[][] route,int start_node, int fin_node,double speed)
        {
            bool change_flag = false;
            //go along the route 
            for (int i = 0; i < route.GetLength(0); i++)
            {
                if(route[i][0] == start_node)
                    change_flag = true;
                
                if(change_flag)
                    cityDataStorage.DefineSpeedForTrack_m_s(route[i][2], speed);

                if (route[i][1] == fin_node)
                    break;
            }
        }

        private int[][] mod_of_route_by_ne_node(int[][] route, int new_node, int new_track, int last_affected_track)
        {

            var index = find_index_of_affected_track_in_route(route, last_affected_track);
            var start_node_of_affected_track = route[index][0];
            var finish_node_of_affected_track = route[index][1];

            var fist_new_step = new int[3] { start_node_of_affected_track, new_node, new_track };
            var second_new_step = new int[3] { new_node, finish_node_of_affected_track, last_affected_track };

            var new_route = create_new_route(route, index, fist_new_step, second_new_step);
            return new_route;
        }
        private int find_index_of_affected_track_in_route(int[][] route, int affected_track)
        {
            var index = -1;
            for (int i = 0; i < route.GetLength(0); i++)
            {
                if (route[i][2] == affected_track)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        private int[][] create_new_route(int[][] route, int index, int[] first_new_step, int[] second_new_step)
        {
            var new_route = new int[route.Length + 1][];

            for (int i = 0; i < index; i++)
            {
                new_route[i] = (int[])route[i].Clone();
            }

            new_route[index] = first_new_step;
            new_route[index + 1] = second_new_step;

            for (int i = index + 1; i < route.Length; i++)
            {
                new_route[i + 1] = (int[])route[i].Clone();
            }
            return new_route;
        }
    }

    

    //public class Move_point_at_station: BasicModOfProfile
    //{
    //    private CityDataStorage city;

    //    public Move_point_at_station(CityDataStorage city) : base(city)
    //    {
    //        this.city = city;
    //    }
    //    public override (int error_code, int[][] new_route) ApplyMod(int start_node, double distance, int[][] route)
    //    {
    //        // szukam trackow przypietych do punktu startowego
    //        var list_of_tracks = city.GetListOfAllTracksConnectedToNode(start_node);
    //        // zostawiam ten ktory jest tez w route
    //        remove_track_not_in_route(ref list_of_tracks, route);
    //        // skracam track o distance
    //        city.Shorten_track_length(list_of_tracks[0], distance);
    //        // route jest w zasadzie takie samo jak bylo 
    //        // ale zmieniaja sie dane w city do ktorego jest przypisane to route

    //        return (5, new int[5][]);
    //    }

    //    private void remove_track_not_in_route(ref List<int> list_of_tracks, int[][] route)
    //    {
    //        int i = 0;
    //        while (i < list_of_tracks.Count)
    //        {
    //            if (is_track_in_route(list_of_tracks[i], route))
    //            {
    //                i++;
    //                continue;
    //            }
    //            else
    //                list_of_tracks.RemoveAt(i);

    //        }
    //    }
    //    private bool is_track_in_route(int track, int[][] route)
    //    {
    //        for (int i = 0; i < route.GetLength(0); i++)
    //        {
    //            if (route[i][2] == track)
    //                return true;
    //        }
    //        return false;
    //    }
    //}
    //public class Move_start_point : BasicModOfProfile
    //{
    //    private CityDataStorage city;

    //    public Move_start_point(CityDataStorage city): base (city)
    //    {
    //        this.city = city;
    //    }
    //    public override (int error_code, int[][] new_route) ApplyMod(int start_node, double distance, int[][] route)
    //    {
    //        // szukam trackow przypietych do punktu startowego
    //        var list_of_tracks = city.GetListOfAllTracksConnectedToNode(start_node);
    //        // zostawiam ten ktory jest tez w route
    //        remove_track_not_in_route(ref list_of_tracks, route);
    //        // skracam track o distance
    //        city.Shorten_track_length(list_of_tracks[0], distance);
    //        // route jest w zasadzie takie samo jak bylo 
    //        // ale zmieniaja sie dane w city do ktorego jest przypisane to route

    //        return (5, new int[5][]);
    //    }

    //    private void remove_track_not_in_route(ref List<int> list_of_tracks, int[][] route)
    //    {
    //        int i = 0;
    //        while(i<list_of_tracks.Count)
    //        {
    //            if (is_track_in_route(list_of_tracks[i], route))
    //            {
    //                i++;
    //                continue;
    //            }
    //            else
    //                list_of_tracks.RemoveAt(i);

    //        }
    //    }
    //    private bool is_track_in_route(int track ,int [][] route)
    //    {
    //        for(int i = 0; i < route.GetLength(0); i++)
    //        {
    //            if(route[i][2] == track)
    //                return true;
    //        }
    //        return false;
    //    }

    //}
    //public class Move_end_point : BasicModOfProfile
    //{
    //    private CityDataStorage city;

    //    public Move_end_point(CityDataStorage city) : base(city)
    //    {
    //        this.city = city;
    //    }
    //    public override (int error_code, int[][] new_route) ApplyMod(int start_node, double distance, int[][] route)
    //    {
    //        // szukam trackow przypietych do punktu startowego
    //        var list_of_tracks = city.GetListOfAllTracksConnectedToNode(start_node);
    //        // zostawiam ten ktory jest tez w route
    //        remove_track_not_in_route(ref list_of_tracks, route);
    //        // skracam track o distance
    //        city.Shorten_track_length(list_of_tracks[0], distance);
    //        // route jest w zasadzie takie samo jak bylo 
    //        // ale zmieniaja sie dane w city do ktorego jest przypisane to route

    //        return (5, new int[5][]);
    //    }

    //    private void remove_track_not_in_route(ref List<int> list_of_tracks, int[][] route)
    //    {
    //        int i = 0;
    //        while (i < list_of_tracks.Count)
    //        {
    //            if (is_track_in_route(list_of_tracks[i], route))
    //            {
    //                i++;
    //                continue;
    //            }
    //            else
    //                list_of_tracks.RemoveAt(i);

    //        }
    //    }
    //    private bool is_track_in_route(int track, int[][] route)
    //    {
    //        for (int i = 0; i < route.GetLength(0); i++)
    //        {
    //            if (route[i][2] == track)
    //                return true;
    //        }
    //        return false;
    //    }
    //}

    public class station_mod:ModOfCityMap  
    {
        private CityDataStorage city;

        public station_mod(CityDataStorage city):base(city)
        {
            this.city = city;

        }

        public int start_mod(int start_track, double distance, double speed_limit)
        {
            
            var (affected_tracks, distance_into_last_affected_track, error_code) = SearchForLocationOfOthereSideOfMod_AlongTraffic(start_track, distance);


            var last_affected_track = affected_tracks.Last();
            //sprawdzam te czy dana modyfikacja nieweszla w rejon skrzyzowania co tez niejest ok i jesli tak to zwracam error (dzieje sie to automatycznie przy wyliczaniu pozycji konca moda) 
            //raczej przesada tutaj ale bazuje na istniejacym kodzie wiec to juz jest zrobione
            if (error_code == 1)
            {
                return error_code;
            }

            //teoretycznie powinienem sprawdzic czy niewychodze poza permutacje ale to narazie jest troche szalenstwo bo z zalozenia odleglosc konca bedzie rowna jakies 0.5 m
            //teoretycznie powinienem tez sprawdzic czy niema wolniejszego tracku po drodze niz predkos ktora zadalem, ale to tez jest przesada narazie 

                var (new_node, new_track, is_new_track_created) = AddNewNodeToTrackAlongTraffic(last_affected_track, distance_into_last_affected_track);
                // w opisie modu dodane do trawersu:
                //metoda replace last track
                if (is_new_track_created)
                {
                    replace_last_affected_track_after_mod(affected_tracks, new_track);
                    //to jest konieczne tylko w przypadku przyspieszania bo track ostatniego tracku sie zmienia
                }
                else
                {
                    //brak nowego tracku oznacza ze distance into affected track jest zero
                    remove_last_affected_track_from_list(affected_tracks);
                }
                

                ChangeSpeedOfTrack(affected_tracks, speed_limit);

                return error_code;
        }
        public void stop_mod(int start_track, double distance, double speed_limit)
        {
            var (affected_tracks, distance_into_last_affected_track, error_code) = SearchForLocationOfOthereSideOfMod_WrongWay(start_track, distance);
            var last_affected_track = affected_tracks.Last();
            var (new_node, new_track, is_new_track_created) = AddNewNodeToTrackTheWrongWay(last_affected_track, distance_into_last_affected_track);

            if (is_new_track_created)
            {
                //przy tej modyfikacji idacej pod prad nawet po dodaniu nowego tracku ostatni modyfikowany track pozostaje taki sam
                //wiec list trackow ze zmieniona predkosci niewymaga modyfikacji 
            }
            else
                remove_last_affected_track_from_list(affected_tracks);
                //to jest szczegolny przypadek kiedy dystans into affected track jest 0, co oznacza ze last track niepodlega zmianom

            ChangeSpeedOfTrack(affected_tracks, speed_limit);

        }


        private void replace_last_affected_track_after_mod(List<int> affected_tracks, int new_track)
        {
                int last_track = affected_tracks.Last();
                affected_tracks.Remove(last_track);
                affected_tracks.Add(new_track);
        }
        private void remove_last_affected_track_from_list(List<int> affected_tracks)
        {
            var last_track = affected_tracks.Last();
            affected_tracks.Remove(last_track);
        }
    }
    public class DC_section_traverse_mod: ModOfCityMap
    {
        private CityDataStorage city;

        public DC_section_traverse_mod(CityDataStorage city):base(city)
        {
            this.city = city;
        }


        public (int error,int[][] route) add_mod_of_first_pod_in_a_traverse(double length_of_traverse, ref int[][] route)
        {
            // first pod has dalayed acceleration and zero change at braking , so effectively only acceleration mods

            int node;
            var delay_acceleration_profile_mod = new DelayAcceleration(city);
            int track;
            int error_code = 0;
            var queries = new RouteQueries(route, city);
            var steps = queries.List_Of_Route_Steps_BetweenNodes(route);


            for (int i = 0; i < steps.GetLength(0); i++)
            {
                bool is_this_acceleration_point = queries.Is_this_acceleration_step(steps, i);

                // if that is acceleration point
                if (is_this_acceleration_point)
                {
                    // get first track number
                    track = find_track_number_connected_to_node_along_traffic(route, (int)steps[i][0]);
                    // add acceleration mod with length of section
                    (error_code, route) = delay_acceleration_profile_mod.ApplyMod(track, length_of_traverse, route);


                    //if there is an error stop and return error number
                    if (error_code != 0)
                    {
                        return (error_code,route);
                    }
                    continue;
                }

                bool is_this_brake_point = queries.Is_this_brake_step(steps, i);
                if (is_this_brake_point)
                {
                    continue;
                }
            }
            if (error_code != 0)//blad w generowaniu kodu bledu
                return (-17,route);
            return (0,route);
        }


        public (int error, int[][] route) add_mod_of_last_pod_in_a_traverse(double length_of_traverse, ref int[][] route)
        {
            //zero delay at acceleration point and max speed up at braking point. effectively no acceleration mods, only brake mods.

            var AddAcceleratedBraking = new AccelereateBraking(city);
            int track;
            int node;
            int error_code = 0;
            var queries = new RouteQueries(route, city);
            var steps = queries.List_Of_Route_Steps_BetweenNodes(route);
            
            for (int i = 0; i < steps.GetLength(0); i++)
            {
                bool is_this_acceleration_point = queries.Is_this_acceleration_step(steps, i);
                // if that is acceleration point
                if (is_this_acceleration_point)
                {
                    continue;
                }
                bool is_this_brake_point = queries.Is_this_brake_step(steps, i);
                if (is_this_brake_point)
                {
                    // Find first track for brake mod. First one before node 
                    track = find_track_number_connected_to_node_the_wrong_way(route, (int)steps[i][0]);
                    // add acceleration mod with length of section
                    (error_code, route) = AddAcceleratedBraking.ApplyMod(track, length_of_traverse, route);
                    if (error_code != 0)
                    {
                        return (error_code, route);
                    }
                    continue;
                }
            }
            if (error_code != 0)//blad w generowaniu kodu bledu
                return (-17, route);
            return (0, route);
        }

        /// <summary>
        /// Distance is measured in length from start of traverse, so 0 is right at the edge of beginning. At front.
        /// </summary>
        /// <param name="length_of_traverse"></param>
        /// <param name="distance_from_start_of_traverse"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public (int error, int[][] route) add_mod_for_arbitrary_pod_in_traverse(Permutation permutation, double length_of_traverse, double distance_from_start_of_traverse, ref int[][] route)
        {
            int node;
            var delay_acceleration_profile_mod = new DelayAcceleration(city);
            var AddAcceleratedBraking = new AccelereateBraking(city);
            int track;
            int error_code = 0;
            var queries = new RouteQueries(route, city);
            var steps = queries.List_Of_Route_Steps_BetweenNodes(route);

            steps = trim_steps_to_present_in_permutation(steps, permutation);

            // brakuje mi tu sprawdzenie ktore step sa w danej sekcji
            // jakie powinny byc zasady takiego filtrowania
            // noude charakterystyczny takiego stepu musi byc w permutacji ale
            // dla akceleracji poza ostatnim noudem 
            // dla hamowania poza pierwsszym noudem 
            // czyli algorytm bedzie dzialac tak:

            // kopiuje route permutacji
            // przygotowuje liste noudow dla akceleracji
            // przygotowuje liste noudow dla hamowania


            // ide po kolei po stepach
                //dla stepu akcel sprawdzam czy jest na liscie akcel
                //dla stepu brake sprawdzam czy jest na liscie brake
            

            for (int i = 0; i < steps.GetLength(0); i++)
            {
                bool is_this_acceleration_point = queries.Is_this_acceleration_step(steps, i);

                // if that is acceleration point
                if (is_this_acceleration_point)
                {
                    // get first track number
                    track = find_track_number_connected_to_node_along_traffic(route, (int)steps[i][0]);
                    // add acceleration mod with length of section
                    (error_code, route) = delay_acceleration_profile_mod.ApplyMod(track, length_of_traverse - distance_from_start_of_traverse, route);


                    //if there is an error stop and return error number
                    if (error_code != 0)
                    {
                        return (error_code, route);
                    }
                    continue;
                }

                bool is_this_brake_point = queries.Is_this_brake_step(steps, i);
                if (is_this_brake_point)
                {
                    // Find first track for brake mod. First one before node 
                    track = find_track_number_connected_to_node_the_wrong_way(route, (int)steps[i][0]);
                    // add acceleration mod with length of section
                    (error_code, route) = AddAcceleratedBraking.ApplyMod(track, distance_from_start_of_traverse, route);
                    if (error_code != 0)
                    {
                        return (error_code, route);
                    }
                    continue;
                }
            }
            if (error_code != 0)//blad w generowaniu kodu bledu
                return (-17, route);
            return (0, route);
        }

        private double [][] trim_steps_to_present_in_permutation(double [][] steps, Permutation permutation)
        {
            var route_of_permutation = permutation.Get_route();
            List<int> list_of_accepted_indexes = new List<int>(); 

            List<int> list_of_acceleration_nodes = new List<int>();
            List<int> list_of_brake_nodes = new List<int>();

            for(int i = 0; i < route_of_permutation.GetLength(0); i++)
            {
                list_of_acceleration_nodes.Add(route_of_permutation[i][0]);
                list_of_brake_nodes.Add(route_of_permutation[i][1]);
            }

            for(int i = 0; i < steps.GetLength(0); i++)
            {
                //acceleration
                if (steps[i][3] == 1)
                {
                    if(list_of_acceleration_nodes.Contains((int)steps[i][0]))
                        list_of_accepted_indexes.Add(i);
                }
                //brake
                if(steps[i][3] == -1)
                {
                    if (list_of_brake_nodes.Contains((int)steps[i][0]))
                        list_of_accepted_indexes.Add(i);
                }
            }

            double[][] result = new double[list_of_accepted_indexes.Count][];

            for(int i = 0; i < list_of_accepted_indexes.Count; i++)
            {
                result[i] = steps[list_of_accepted_indexes[i]];
            }

            return result;
        } 



        // ta metoda bazuje na odcinku route dla danej permutacji
        // jesli be robic mod calego przejazdu ride 
        // modyfikacje beda dla route permutacji i copii miasta dla danego ride
        // wiec caly objeckt miasta jest zmodyfikowany przez wszystkie modyfikacje dla pojedynczych permutacji 
        // na koniec jak juz mam wszystkie fragmenty route dla kazdej permutacji, moge ja latwo skleic w jeden
        // caly odcinek route dle ride ktory bedzie podstawa do policzenia profilu calego przejazdu
        // metoda szukania profilu bazuje na objekcie city i route przez to miasto
        // w objekcie ride brakuje w takim razie zbioru route dla wszystkich permutacji przez ktore jedzie ride.
        // beda one modyfikowane, na bierzaca podczas uwzgledniania modow
        // bedzie tez metoda przywracania city i route dla wszystkich permutacji do stanu wyjsciowego
        // jesli zmieniona zostanie chociaz jedna modyfikacja, wymaga to przywrocenia calego miasta do stanu wyjsciowego i
        // przeliczenia wszystkich permutacji jeszcze raz zeby uzyskac baze dla nowego profilu
        // zrobienie metody ktora modyfikuje 

    }

    public class Sort_section_traverse_mod: ModOfCityMap
    {
        private CityDataStorage city;

        public Sort_section_traverse_mod(CityDataStorage city):base(city)
        {
            this.city = city;
        }

        /// <summary>
        /// Method of applying mod for sorting section, requires calculating where in treaverse start pod is , are where pod is at traverse end. What is missing is mod of route
        /// looks like it is allready modified but have to be terurned of made ref.
        /// </summary>
        /// <param name="permutation"></param>
        /// <param name="length_of_traverse"></param>
        /// <param name="required_translation"></param>
        /// <param name="start_distance_from_begining_of_permutation"></param>
        /// <param name="end_distance_from_start_of_permutation"></param>
        /// <param name="route"></param>
        public void add_mod_for_arbitrary_pod_in_traverse(Permutation permutation, double length_of_traverse,  double start_distance_from_begining_of_permutation, double end_distance_from_start_of_permutation,double distance_in_traverse_at_start, double distance_in_traverse_at_end , ref int[][] route)
        {
            var per_route = permutation.Get_route();
            var speed_mod = new ChangeSpeedFromPointToPoint(city);
            //licze dlugosc sekcji gdzie moze nastepowac sortowanie
            //bazuje to na rote // proste zliczenie dlugsci trackow po drodze 

            double braking_distance;
            double braking_time;
            //licze wymagane przesuniecie w traversie 
            var Ls = permutation.get_permutation_length();
            var Lt = length_of_traverse;
            var V = permutation.get_entrance_speed();
            double Pp = 1; //wspolczynnik dlugosci sekcji na ktorej nastepuje zmiana pasa 
            double a = 3;

            // var x = required_translation;//tu bylo takie przeliczenie krore jest przeszytwniniem bo x wynika z pozycji 
            // tu jest jakis dziki blad bo raz licze od poczatku traversu
            // a raz od poczatku permutacji co moze rozwalac cala zabawe 

            // tutaj napewno jest zle bo chce wyliczyc shift w ukladzie wspolrzednych 
            // traversu , a nie przemieszczenie w ukladzie drogi permutacji

            // pytanie jak dziala dodawanie noudow 
            // dodawanie noudow dziala w ukladzie wspolrzednych permutacji
            // czyli mam problem do rozwiazania, jak policzyc shift w traverse.

            var x =  distance_in_traverse_at_start - distance_in_traverse_at_end;

            var Tt = (Ls - Lt - 2 * V * Pp) / V;

            double del1;
            
            if(x >= 0)
            {
                del1 = (-Tt + Math.Pow(Math.Pow(Tt, 2) - 4 * x / a, 0.5)) / (-2 / a);
                speed_mod.change_speed_between_nodes(per_route, ref route, start_distance_from_begining_of_permutation, end_distance_from_start_of_permutation, V + del1);
            }
            else
            {
                x = -x;
                del1 = (-Tt + Math.Pow(Math.Pow(Tt, 2) - 4 * x / a, 0.5)) / (-2 / a);
                braking_time = del1 / a;
                braking_distance = V * braking_time - 0.5 * a * Math.Pow(braking_time, 2);




                speed_mod.change_speed_between_nodes(per_route, ref route, start_distance_from_begining_of_permutation + braking_distance, end_distance_from_start_of_permutation-braking_distance, V - del1);
            }

                


            double calculate_total_length_of_permutation(int[][] _route)
            {
                double length = 0;

                for (int i = 0; i < _route.GetLength(0); i++)
                {
                    length = +CityMap.GetLenghtOfTrack(_route[i][2]);
                }
                return length;
            }
        }

    }

    
}
