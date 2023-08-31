using System.Collections.Generic;

namespace Symulation
{
    public class ModOfCityMap
    {
        public CityDataStorage CityMap;

        public ModOfCityMap(CityDataStorage cityMap)
        {
            CityMap = cityMap;
        }

        public void ChangeSpeedOfTrack(int Track, double speed)
        {
            CityMap.DefineSpeedForTrack_m_s(Track, speed);
        }

        public void ChangeSpeedOfTrack(List<int> Lista, double speed)
        {
            foreach(int n in Lista)
            {
                ChangeSpeedOfTrack(n, speed);
            }
        }

        //tutaj bedzie zmiana w zaleznosci od tego jaka jest geometra danego traku
        //jesli to jest luk, to bedzie troche inaczej ale niejest to duza roznica
        //mam tutaj blad w liczeniu dlugosci po modyfikacji przy kacie 225 wyslo -140 zamiast 555 a przy 45 wyszlo 39 zamiast 555
        public (int NewNodeNumber, int NewTrackNumber ,bool is_new_track_created) AddNewNodeToTrackAlongTraffic(int AffectedTrack, double DistanceIntoAffectedTrack)
        {
            //reorganise city
            //
            bool is_new_track_created = true;
            int EndNode = 0;

            if (DistanceIntoAffectedTrack == 0)
            {
                EndNode = CityMap.GetDirectionalityNodeForTrack(AffectedTrack);
                is_new_track_created = false;
                return (EndNode, -1, is_new_track_created);
            }
            // ta metoda totalnie wymaga uproszczenia, dziala ale jest nieczytelna wiec tak niemoze byc dalej
            // narazie dziala tylko dla geometrii straight, dla curved wymaga przerobienia i funkcji warukowej na poczatku 
            else
            {
                //Read directional node
                var DirectionalNode = CityMap.GetListOfAllTracksInCity()[AffectedTrack].GetDirectionalNode();
                // disconnect directional node from affected track
                CityMap.DisconnectNodeFromTrack(AffectedTrack, DirectionalNode);

                // remove definition of directionality
                CityMap.RemoveDirectionDefinitionForAffectedTrack(AffectedTrack);

                


                //Create new node, next available number
                        //potrzebne jest wyliczenie jakie wspolrzedna bedzie miec ten noude
                var (new_node_x, new_node_y) = CityMap.calculate_position_of_point_at_x_from_start_of_track(AffectedTrack,DistanceIntoAffectedTrack);

                var NewNodeNumber = CityMap.AddNodeToCityMap();
                CityMap.define_position_of_node(NewNodeNumber, new_node_x, new_node_y);




                // change length of affected track
                var AffectedTrackLength = CityMap.GetListOfAllTracksInCity()[AffectedTrack].Length;
                var NewLengthOfTrack = AffectedTrackLength - DistanceIntoAffectedTrack;
                        //zmiana , potrzebna jest redefinicja geometrii tego tracku, i nowa dlugosc zostanie wyliczona juz na jej bazie
                        //def geometri oznacza construction length i construction angle
                //moge to zrobic przez podanie odleglosci x od nouda kierunkowego
                
                CityMap.redefine_track_geometry_by_moving_start_point_by_x(AffectedTrack, DistanceIntoAffectedTrack);

                



                //System.Console.WriteLine();
                //Connect new Node To shortened Track
                CityMap.AttachNodeToTrack(NewNodeNumber, AffectedTrack);

                // create new track of needed length and get it`s number
                        // zmiana potrzebna jest definicja geometrii nowego tracku

                //tu brakuje mi kata nowego tracku ktory w tym przypadku jest kopia istniejacego tracku
                var (con_length, con_angle) = CityMap.get_construction_geomertu_data_for_track(AffectedTrack);
                var NewTrackNumber = CityMap.AddStraightTrackElementToCityMap_With_Next_Number(DistanceIntoAffectedTrack, con_angle);


                // attach directional node of the old affected track to new track
                CityMap.AttachNodeToTrack(DirectionalNode, NewTrackNumber);

                //attach new node to new track
                CityMap.AttachNodeToTrack(NewNodeNumber, NewTrackNumber);

                // define directionality of the old modified track
                CityMap.DefineDirectionForTrack(AffectedTrack, NewNodeNumber);
                // define directionality of the new track
                CityMap.DefineDirectionForTrack(NewTrackNumber, DirectionalNode);

                var speed_of_affected_track = CityMap.GetSpeedLimitForTrack_m_s(AffectedTrack);
                CityMap.DefineSpeedForTrack_m_s(NewTrackNumber,speed_of_affected_track);
                
                //what is missing is adding ref nodes to tracks, only numbers where attached 


                return (NewNodeNumber, NewTrackNumber, is_new_track_created);
            }

        }

        
        public (int NewnodeNumber, int NewTrackNumber , bool is_new_track_added) AddNewNodeToTrackTheWrongWay(int AffectedTrack, double DistanceIntoAffectedTrack)
        {
            //reorganise city
            //
            bool is_new_track_created = true;
            int EndNode = 0;

            if (DistanceIntoAffectedTrack == 0)
            {
                EndNode = CityMap.GetDirectionalityNodeForTrack(AffectedTrack);
                EndNode = CityMap.GetNodeNumberOnTheOthereSideOfTrack( AffectedTrack,EndNode);
                is_new_track_created = false;
                return (EndNode, -1, is_new_track_created);
            }

            //ta metoda totalnie wymaga uproszczenia, dziala ale jest nieczytelna wiec tak niemoze byc dalej
            else
            {
                //Read directional node
                
                //Create new node, next available number
                //potrzebne jest wyliczenie jakie wspolrzedna bedzie miec ten noude
                var (new_node_x, new_node_y) = CityMap.calculate_position_of_point_at_x_from_end_of_track(AffectedTrack, DistanceIntoAffectedTrack);

                var NewNodeNumber = CityMap.AddNodeToCityMap();
                CityMap.define_position_of_node(NewNodeNumber, new_node_x, new_node_y);

                
                
                
                

                var AffectedTrackLength = CityMap.GetListOfAllTracksInCity()[AffectedTrack].Length;
                //zmiana , potrzebna jest redefinicja geometrii tego tracku, i nowa dlugosc zostanie wyliczona juz na jej bazie
                //def geometri oznacza construction length i construction angle
                //moge to zrobic przez podanie odleglosci x od nouda kierunkowego

                CityMap.redefine_track_geometry_by_moving_start_point_by_x(AffectedTrack, AffectedTrackLength - DistanceIntoAffectedTrack);

                var DirectionalNode = CityMap.GetListOfAllTracksInCity()[AffectedTrack].GetDirectionalNode();

                // disconnect directional node from affected track
                CityMap.DisconnectNodeFromTrack(AffectedTrack, DirectionalNode);

                // remove definition of directionality
                CityMap.RemoveDirectionDefinitionForAffectedTrack(AffectedTrack);



                //System.Console.WriteLine();
                //Connect new Node To shortened Track
                //zmiana potrzenbe wspolrzedne tego noda
                CityMap.AttachNodeToTrack(NewNodeNumber, AffectedTrack);

                // create new track of needed length and get it`s number
                        //potrzebna definicja kata
                var NewTrackLength = AffectedTrackLength - DistanceIntoAffectedTrack;
                var (con_length , con_angle) = CityMap.get_construction_geomertu_data_for_track(AffectedTrack);
                var NewTrackNumber = CityMap.AddStraightTrackElementToCityMap_With_Next_Number(NewTrackLength, con_angle);


                // attach directional node of the old affected track to new track
                CityMap.AttachNodeToTrack(DirectionalNode, NewTrackNumber);
                //attach new node to new track
                CityMap.AttachNodeToTrack(NewNodeNumber, NewTrackNumber);


                // define directionality of the old modified track
                CityMap.DefineDirectionForTrack(AffectedTrack, NewNodeNumber);
                // define directionality of the new track
                CityMap.DefineDirectionForTrack(NewTrackNumber, DirectionalNode);

                var speed_of_affected_track = CityMap.GetSpeedLimitForTrack_m_s(AffectedTrack);
                CityMap.DefineSpeedForTrack_m_s(NewTrackNumber, speed_of_affected_track);

                return (NewNodeNumber, NewTrackNumber, is_new_track_created);
            }
        }
        

        //moge uzyc tych metod do liczenia pozycji w drodze 
        public (List<int> AffectedTracks, double DistanceIntoAffectedTrack, int error_code) SearchForLocationOfOthereSideOfMod_AlongTraffic(int StartTrack, double Length)
        {

            int currentNode;
            int CurrentTrackNumber = StartTrack;
            int FinalTrackNumber;
            var AffectedTracks = new List<int>();
            int error_code=0;

            double LengthOfCurrentTrack;
            double RemaningLength = Length;


            double LenghtIntoTrackWhereEndNodeIs;

            while (true)
            {
                LengthOfCurrentTrack = GetLengthOfTrack(CurrentTrackNumber);

                if (RemaningLength < LengthOfCurrentTrack)
                {
                    (FinalTrackNumber, LenghtIntoTrackWhereEndNodeIs) = PrepareDataAfterAffectedTrackIsFound(CurrentTrackNumber, RemaningLength);
                    break;
                    
                }
                else
                {
                    AffectedTracks.Add(CurrentTrackNumber);
                    (CurrentTrackNumber, RemaningLength, error_code) = GoToNextTrack_AlongTraffic(CurrentTrackNumber, RemaningLength, LengthOfCurrentTrack);
                }

                if (error_code != 0)
                {
                    return (AffectedTracks, -1, error_code); // in case of error, procedure is stopped  (error =1 cross road in range of search )
                                                                  
                }
            }

            // co z kierunkowoscia ?
            //-podawac jako parametr na wyjsciu 
            // czy odrazu policzone jako odleglosc od poczatku wzgldem jednego wybranego kierunku 
            // tak zeby byla tylko jedna metoda do dodawania noudow
            // opcja z przeliczeniem odrazu, mi sie podoba
            // jednak bedzie inaczej, podaje odleglosc bez przliczania bo itak beda 2 metody dodawania noda do tracku
            //  wynika to z tego ze potrzebna jest informacja jaki jest numer tracku affektowany przez dana modyfikacja
            // i ten track bedzie inny w zaleznosci od tego czy to jest liczenie w przod czy w tyl

            AffectedTracks.Add(FinalTrackNumber);
            return (AffectedTracks, LenghtIntoTrackWhereEndNodeIs, error_code);
        }
        public (List<int> AffectedTracks, double DistanceIntoAffectedTrack, int error_code) SearchForLocationOfOthereSideOfMod_WrongWay(int StartTrack, double Length)
        {

            int currentNode;
            int CurrentTrackNumber = StartTrack;
            int FinalTrackNumber;
            var AffectedTracks = new List<int>();
            int error_code = 0;

            double LengthOfCurrentTrack;
            double RemaningLength = Length;

            double LenghtIntoTrackWhereEndNodeIs;

            while (true)
            {
                LengthOfCurrentTrack = GetLengthOfTrack(CurrentTrackNumber);

                if (RemaningLength < LengthOfCurrentTrack)
                {
                    (FinalTrackNumber, LenghtIntoTrackWhereEndNodeIs) = PrepareDataAfterAffectedTrackIsFound(CurrentTrackNumber, RemaningLength);
                    break;
                }
                else
                {
                    AffectedTracks.Add(CurrentTrackNumber);
                    (CurrentTrackNumber, RemaningLength, error_code) = GoToNextTrack_WrongWay(CurrentTrackNumber, RemaningLength, LengthOfCurrentTrack);
                }
                if(error_code != 0)
                {
                    return (AffectedTracks, -1, error_code); // if there is an error, procedure is stoped (error = -1 , cross road in range of search)
                }

            }

            // co z kierunkowoscia ?
            //-podawac jako parametr na wyjsciu 
            // czy odrazu policzone jako odleglosc od poczatku wzgldem jednego wybranego kierunku 
            // tak zeby byla tylko jedna metoda do dodawania noudow
            // opcja z przeliczeniem odrazu, mi sie podoba
            // jednak bedzie inaczej, podaje odleglosc bez przliczania bo itak beda 2 metody dodawania noda do tracku
            //  wynika to z tego ze potrzebna jest informacja jaki jest numer tracku affektowany przez dana modyfikacja
            // i ten track bedzie inny w zaleznosci od tego czy to jest liczenie w przod czy w tyl

            AffectedTracks.Add(FinalTrackNumber);
            return (AffectedTracks, LenghtIntoTrackWhereEndNodeIs, error_code);


        }
        
        private double GetLengthOfTrack(int Number)
        {
            return CityMap.GetLenghtOfTrack(Number);
        }
        private (int track_number, double remaning_length, int error) GoToNextTrack_AlongTraffic(int current_track_number, double remaning_length, double lenght_of_current_track)
        {
            //tu chce miec sprawdzenie czy to niejest skrzyzowanie, jesli tak to wyrzucam blad

            var (next_track, error) = CityMap.get_number_of_next_track_along_traffic_direction(current_track_number);

            return (next_track, remaning_length - lenght_of_current_track, error);
        }
        private (int track_number, double remaning_length, int error) GoToNextTrack_WrongWay(int current_track_number, double remaning_length, double lenght_of_current_track)
        {
            var (next_track, error) = CityMap.get_number_of_next_track_the_wrong_way(current_track_number);

            return (next_track, remaning_length - lenght_of_current_track, error);

        }
        private (int FinalTrack, double LengthIntoAffectedTrack) PrepareDataAfterAffectedTrackIsFound(int CurrentTrack, double RamainingLength)
            {
                var FinalTrackNumber = CurrentTrack;
                var LenghtIntoTrackWhereEndNodeIs = RamainingLength;

                return (FinalTrackNumber, LenghtIntoTrackWhereEndNodeIs);
            }


        public int find_track_number_connected_to_node_along_traffic(int[][] route, int node_number)
        {
            for (int i = 0; i < route.GetLength(0); i++)
            {
                if (route[i][0] == node_number)
                {
                    return route[i][2];
                }
            }

            return -1;
        }
        public int find_track_number_connected_to_node_the_wrong_way(int[][] route, int node_number)
        {
            for (int i = 0; i < route.GetLength(0); i++)
            {
                if (route[i][1] == node_number)
                {
                    return route[i][2];
                }
            }

            return -1;
        }





    }

}
