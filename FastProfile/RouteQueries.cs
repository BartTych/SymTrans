using System;
using System.Collections.Generic;

namespace Symulation
{
    public class RouteQueries
    {
        int[][] _route;
        double[][] _RouteSummary;
        double[][] _ListOfNegativeSteps;
        Node[] _listOfNodes;
        Track[] _listOfTracks;
        Station[] _listOfStations;

        public RouteQueries(int [][] Route, CityDataStorage City)
        {
            _route = Route;
            _listOfNodes = City.GetListOFAllNodesInCity();
            _listOfTracks = City.GetListOfAllTracksInCity();
            _listOfStations =City.GetListOfAllStationsInCity();
            //_RouteSummary = SummaryOfRoute(Route);
            //_ListOfNegativeSteps = List_Of_Route_Steps(Route);

        }

        public int WhatIsRouteIndexOfTrackOfPositionOnRoute(double Distance)
        {
            int Track=0;
            for (int i = 0; i < _route.GetLength(0); i++)
            {
                if (Distance < _RouteSummary[i][2])
                {
                    break;
                }
                Track++;
            }
            return Track;
        }
        public int WhatIsRouteNumberOfTrackOfPositionOnRoute(double Distance)
        {
            int Track = 0;
            for (int i = 0; i < _route.GetLength(0); i++)
            {
                if (Distance < _RouteSummary[i][2])
                {
                    break;
                }
                Track++;
            }
            return _route[Track][2];
        }
        public bool CheckIfEndOfTrackIsReached(int CurrentTrackIndex, double position)
        {
            if (position >= _RouteSummary[CurrentTrackIndex][2])
            {
                return true;
            }
            else
                return false;
        }
        public bool CheckIfEndOfRouteIsReached(double position)
        {
            if (position >= _RouteSummary[_route.GetLength(0)][2])
            {
                return true;
            }
            else
                return false;
        }
        public double WhatIsSpeedLimit(double x)
        {
            int track = WhatIsRouteNumberOfTrackOfPositionOnRoute(x);

            return _listOfTracks[track].GetSpeedLimit();
        }


        public double[][] SummaryOfRoute(int[][] Route)
        {
            double[][] SummaryOfTracksLength = new double[Route.GetLength(0)+1][];

            for (int i = 0; i <= Route.GetLength(0); i++)
            {
                SummaryOfTracksLength[i] = new double[5];  //5 bo rozbudowuje pozniej to summary o predkosc i przyspieszenie
                if(i== Route.GetLength(0))
                    SummaryOfTracksLength[i][0] = Route[i-1][1];
                else
                    SummaryOfTracksLength[i][0] = Route[i][0];                                  //Node zaczynajacy dany odcinek
                if(i>0)
                SummaryOfTracksLength[i][1] = _listOfTracks[Route[i-1][2]].Length;         //route disstance
            }

            for (int i = 1; i < Route.GetLength(0)+1; i++)
            {
                SummaryOfTracksLength[i][1] = SummaryOfTracksLength[i][1] + SummaryOfTracksLength[i - 1][1];
            }
            return SummaryOfTracksLength;
        }

        public double[][] List_Of_Route_Steps_BetweenStations(int[][] Route)  // start speed is zero, end speed is zero
        {

            int RouteLength = Route.GetLength(0);
            double SpeedOne;
            double SpeedTwo;
            double Distance = 0;
            int k = 0;          //ilcznik stepow

            double[][] List = new double[Route.GetLength(0)][];
            for (int i = 0; i < Route.GetLength(0); i++)
                List[i] = new double[4];

            //poczatkowe przyspieszenie
            List[k][0] = Route[0][0];                                       //node 
            List[k][1] = 0;                                                 //odleglosc
            List[k][2] = 0;                                                 //predkosc charakterystyczna dla danego stepu (poczatkowa dla przyspieszenia, koncowa dla hamowania)
            List[k][3] = 1;                                                 //rodzaj stepu (-1 hamowanie ,1 przyspieszenie)
            k = 1;


            for (int i = 0; i < RouteLength-1; i++)
            {
                SpeedOne = _listOfTracks[Route[i][2]].GetSpeedLimit();
                SpeedTwo = _listOfTracks[Route[i + 1][2]].GetSpeedLimit();


                Distance += _listOfTracks[Route[i][2]].Length;

                if (SpeedTwo < SpeedOne)
                {
                    List[k][0] = Route[i][1];
                    List[k][1] = Distance;
                    List[k][2] = SpeedTwo;
                    List[k][3] = -1;        //negative step
                    k++;
                }

                if (SpeedTwo > SpeedOne)
                {
                    List[k][0] = Route[i][1];
                    List[k][1] = Distance;
                    List[k][2] = SpeedOne;
                    List[k][3] = 1;        //positive step
                    k++;
                }
            }

            int LengthOfList = 1;

            for (int i = 0; i < List.GetLength(0); i++)
            {
                if (List[i][0] == 0 & List[i][1] == 0 & List[i][2] == 0 & List[i][3] == 0)
                {
                    LengthOfList = i;
                    break;
                }
            }

            double[][] FinalList = new double[LengthOfList + 1][];
            for (int i = 0; i < LengthOfList + 1; i++)
                FinalList[i] = new double[4];

            for (int i = 0; i < LengthOfList; i++)
            {
                FinalList[i][0] = List[i][0];
                FinalList[i][1] = List[i][1];
                FinalList[i][2] = List[i][2];
                FinalList[i][3] = List[i][3];
            }

            //dodanie ostatniego kroku
            FinalList[LengthOfList][0] = Route[RouteLength-1][1];
            FinalList[LengthOfList][1] = Distance + _listOfTracks[Route[RouteLength - 1][2]].Length;
            FinalList[LengthOfList][2] = 0;
            FinalList[LengthOfList][3] = -1;

            return FinalList;

        }

        public double[][] List_Of_Route_Steps_BetweenNodes(int[][] Route) //assuming that pod is at speed of first track at the begining and is reaching last track speed at the end. 
        {

            int RouteLength = Route.GetLength(0);
            double SpeedOne;
            double SpeedTwo;
            double Distance = 0;
            int k = 0;          //ilcznik stepow

            double[][] List = new double[Route.GetLength(0)][];
            for (int i = 0; i < Route.GetLength(0); i++)
                List[i] = new double[4];

            //poczatkowe przyspieszenie przy starcie z zatrzymania // to niejest potrzebne w wersji dla node-node 
            //List[k][0] = Route[0][0];                                       //node 
            //List[k][1] = 0;                                                 //odleglosc od poczatku danej drogi do noda charakterystycznego 
            //List[k][2] = 0;                                                 //predkosc charakterystyczna dla danego stepu (poczatkowa dla przyspieszenia, koncowa dla hamowania)
            //List[k][3] = 1;                                                 //rodzaj stepu (-1 hamowanie ,1 przyspieszenie)
            //k = 1;


            for (int i = 0; i < RouteLength - 1; i++)
            {
                SpeedOne = _listOfTracks[Route[i][2]].GetSpeedLimit();
                SpeedTwo = _listOfTracks[Route[i + 1][2]].GetSpeedLimit();


                Distance += _listOfTracks[Route[i][2]].Length;

                if (SpeedTwo < SpeedOne)
                {
                    List[k][0] = Route[i][1];
                    List[k][1] = Distance;
                    List[k][2] = SpeedTwo;
                    List[k][3] = -1;        //negative step
                    k++;
                }

                if (SpeedTwo > SpeedOne)
                {
                    List[k][0] = Route[i][1];
                    List[k][1] = Distance;
                    List[k][2] = SpeedOne;
                    List[k][3] = 1;        //positive step
                    k++;
                }
            }

            int LengthOfList = 1;

            for (int i = 0; i < List.GetLength(0); i++)
            {
                if (List[i][0] == 0 & List[i][1] == 0 & List[i][2] == 0 & List[i][3] == 0)
                {
                    LengthOfList = i;
                    break;
                }
            }
           
            double[][] FinalList = new double[LengthOfList][];
            for (int i = 0; i < LengthOfList; i++)
                FinalList[i] = new double[4];

            for (int i = 0; i < LengthOfList; i++)
            {
                FinalList[i][0] = List[i][0];
                FinalList[i][1] = List[i][1];
                FinalList[i][2] = List[i][2];
                FinalList[i][3] = List[i][3];
            }

            //dodanie ostatniego kroku// to niejest potrzebne w wersji od nouda do nouda
            //FinalList[LengthOfList][0] = Route[RouteLength - 1][1];
            //FinalList[LengthOfList][1] = Distance + _listOfTracks[Route[RouteLength - 1][2]].GetLength;
            //FinalList[LengthOfList][2] = 0;
            //FinalList[LengthOfList][3] = -1;

            return FinalList;

        }


        public double[][] List_Of_Route_Steps_BetweenIntersections(int[][] Route, double start_speed) //start speed [m/s] is defined to be lower that first track by intersection, end track speed is speed of the last track.
        {
            int RouteLength = Route.GetLength(0);
            double SpeedOne;
            double SpeedTwo;
            double Distance = 0;
            int k = 0;          //ilcznik stepow

            double[][] List = new double[Route.GetLength(0)][];
            for (int i = 0; i < Route.GetLength(0); i++)
                List[i] = new double[4];

            //poczatkowe przyspieszenie// generowane przez wjazd wolniejszego odgalezeinie skrzyzowania 
            List[k][0] = Route[0][0];                                       //node 
            List[k][1] = 0;                                                 //odleglosc od poczatku danej drogi do noda harakterystycznego 
            List[k][2] = start_speed;                                       //predkosc charakterystyczna dla danego stepu (poczatkowa dla przyspieszenia, koncowa dla hamowania)
            List[k][3] = 1;                                                 //rodzaj stepu (-1 hamowanie ,1 przyspieszenie)
            k = 1;


            for (int i = 0; i < RouteLength - 1; i++)
            {
                SpeedOne = _listOfTracks[Route[i][2]].GetSpeedLimit();
                SpeedTwo = _listOfTracks[Route[i + 1][2]].GetSpeedLimit();


                Distance += _listOfTracks[Route[i][2]].Length;

                if (SpeedTwo < SpeedOne)
                {
                    List[k][0] = Route[i][1];
                    List[k][1] = Distance;
                    List[k][2] = SpeedTwo;
                    List[k][3] = -1;        //negative step
                    k++;
                }

                if (SpeedTwo > SpeedOne)
                {
                    List[k][0] = Route[i][1];
                    List[k][1] = Distance;
                    List[k][2] = SpeedOne;
                    List[k][3] = 1;        //positive step
                    k++;
                }
            }

            int LengthOfList = 1;

            for (int i = 0; i < List.GetLength(0); i++)
            {
                if (List[i][0] == 0 & List[i][1] == 0 & List[i][2] == 0 & List[i][3] == 0)
                {
                    LengthOfList = i;
                    break;
                }
            }

            double[][] FinalList = new double[LengthOfList][];
            for (int i = 0; i < LengthOfList; i++)
                FinalList[i] = new double[4];

            for (int i = 0; i < LengthOfList; i++)
            {
                FinalList[i][0] = List[i][0];
                FinalList[i][1] = List[i][1];
                FinalList[i][2] = List[i][2];
                FinalList[i][3] = List[i][3];
            }

            //dodanie ostatniego kroku// to niejest potrzebne w wersji od nouda do nouda
            //FinalList[LengthOfList][0] = Route[RouteLength - 1][1];
            //FinalList[LengthOfList][1] = Distance + _listOfTracks[Route[RouteLength - 1][2]].GetLength;
            //FinalList[LengthOfList][2] = 0;
            //FinalList[LengthOfList][3] = -1;

            return FinalList;
        }

        public bool Is_this_acceleration_step(double[][] steps, int step_number)
        {
            // steps discription
            //List[k][0] = Route[0][0];                                       //node 
            //List[k][1] = 0;                                                 //odleglosc
            //List[k][2] = 0;                                                 //predkosc charakterystyczna dla danego stepu (poczatkowa dla przyspieszenia, koncowa dla hamowania)
            //List[k][3] = 1;                                                 //rodzaj stepu (-1 hamowanie ,1 przyspieszenie)

            if (steps[step_number][3] == 1)
                return true;
            else
                return false;
        }
        public bool Is_this_brake_step(double[][] steps, int step_number)
        {
            // steps discription
            //List[k][0] = Route[0][0];                                       //node 
            //List[k][1] = 0;                                                 //odleglosc
            //List[k][2] = 0;                                                 //predkosc charakterystyczna dla danego stepu (poczatkowa dla przyspieszenia, koncowa dla hamowania)
            //List[k][3] = 1;                                                 //rodzaj stepu (-1 hamowanie ,1 przyspieszenie)

            if (steps[step_number][3] == -1)
                return true;
            else
                return false;
        }

        public List<double[]> Route_Base_MatrixBetweenStations(int[][] Route, double a)
        {
            //przygotowanie listy zmian predkosci na drodze ktora bedzie jechac kapsula (int[][] Route)
            var Steps = List_Of_Route_Steps_BetweenStations(Route);

            var result = new List<double[]>();
            int current_route_node;

            // X0 odleglosc harakterystyczna1 poczatek
            // X1 odleglosc charakterystyczna2 koniec,
            // V predkosc na poczatku, przyspieszenie na poczatku,
            // rodzaj lini (-1 brake, 0 track, 1 accelerate)
            // node 1(poczatek dla a i t)
            // node 2 koniec dla T i B
            // na poczatku a ma tylko 1 X, b ma tylko 2 X, T obie. po przecieciach A i B tez maja obie.


            //wstawienie wszystkich trackow ktore sa obecne w spisie drogi 
            result = Add_All_Tracks_To_Result(result);

            //laczenie trackow o identycznych predkosciach, teraz kiedy chce dodac modyfikacje profilu moze lepiej tego nierobic ?
            //kedy metoda jest wylaczona, profil zawiera poprostu spis wszystkich odcinkow drogi, a nie tylko logiczne sekcje
            //result = Consolidate_Track_with_The_same_speed(result);

            //dodanie skokow z listy steps
            for (int i = 0; i < Steps.Length; i++)
            {
                //type of step
                if (Steps[i][3] == 1)//acceleration
                {
                    //search for track that starts at step node
                    var Index_of_A = Search_For_Track_Which_starts_with_Node(result, (int)Steps[i][0]);
                    //add acceleration before that node 
                    result.Insert(Index_of_A, new double[9]);
                    result[Index_of_A][0] = Steps[i][1];
                    result[Index_of_A][1] = 0;
                    result[Index_of_A][2] = Steps[i][2];    //predkoscod od ktorejprzspiesza dana sekcja akcelerate
                    result[Index_of_A][3] = a;//acceleration
                    result[Index_of_A][4] = 1;//type of line
                    result[Index_of_A][5] = Steps[i][0];//node 1
                    result[Index_of_A][6] = -2;//node 2
                }
                else// brake
                {
                    //search for track that ends at step node
                    var Index_of_B = Search_For_Track_Which_ends_with_Node(result, (int)Steps[i][0]);
                    //add brake after that node
                    result.Insert(Index_of_B+1, new double[9]);
                    result[Index_of_B + 1][0] = 0;
                    result[Index_of_B + 1][1] = Steps[i][1]; 
                    result[Index_of_B + 1][2] = Steps[i][2];    //predkosc do ktorej hamuje dana sekcja brake
                    result[Index_of_B + 1][3] = a;//acceleration
                    result[Index_of_B + 1][4] = -1;//type of line
                    result[Index_of_B + 1][5] = -2;//node 1
                    result[Index_of_B + 1][6] = Steps[i][0];//node 2
                }
            }


            //metoda wrzucajaca wszystko w macierz wynikowa
            return result;
        }

        public List<double[]> Route_Base_MatrixBetweenStations_velocity_only(int[][] Route, double a)
        {
            //przygotowanie listy zmian predkosci na drodze ktora bedzie jechac kapsula (int[][] Route)
            var Steps = List_Of_Route_Steps_BetweenStations(Route);

            var result = new List<double[]>();
            //int current_route_node;

            // X0 odleglosc harakterystyczna1 poczatek
            // X1 odleglosc charakterystyczna2 koniec,
            // V predkosc na poczatku, przyspieszenie na poczatku,
            // rodzaj lini (-1 brake, 0 track, 1 accelerate)
            // node 1(poczatek dla a i t)
            // node 2 koniec dla T i B
            // na poczatku a ma tylko 1 X, b ma tylko 2 X, T obie. po przecieciach A i B tez maja obie.


            //wstawienie wszystkich trackow ktore sa obecne w spisie drogi 
            result = Add_All_Tracks_To_Result(result);
            return result;
        }

        public List<double[]> Route_Base_MatrixBetweenNoudes_velocity_only(int[][] Route)
        {
            //przygotowanie listy zmian predkosci na drodze ktora bedzie jechac kapsula (int[][] Route)
            var Steps = List_Of_Route_Steps_BetweenNodes(Route);

            var result = new List<double[]>();
            int current_route_node;

            // X0 odleglosc harakterystyczna1 poczatek
            // X1 odleglosc charakterystyczna2 koniec,
            // V predkosc na poczatku, przyspieszenie na poczatku,
            // rodzaj lini (-1 brake, 0 track, 1 accelerate)
            // node 1(poczatek dla a i t)
            // node 2 koniec dla T i B
            // a ma tylko X0, b ma tylko X1, T obiei po przecieciach z A i B tez maja obie, bez zmian


            //wstawienie wszystkich trackow ktore sa obecne w spisie drogi 
            result = Add_All_Tracks_To_Result(result);

            //laczenie trackow o identycznych predkosciach, teraz kiedy chce dodac modyfikacje profilu moze lepiej tego nierobic ?
            //kedy metoda jest wylaczona, profil zawiera poprostu spis wszystkich odcinkow drogi, a nie tylko logiczne sekcje
            //result = Consolidate_Track_with_The_same_speed(result);

            //dodanie skokow z listy steps
            /*
            for (int i = 0; i < Steps.Length; i++)
            {
                //type of step
                if (Steps[i][3] == 1)//acceleration
                {
                    //search for track that starts at step node
                    var Index_of_A = Search_For_Track_Which_starts_with_Node(result, (int)Steps[i][0]);
                    //add acceleration before that node 
                    result.Insert(Index_of_A, new double[9]);
                    result[Index_of_A][0] = Steps[i][1];
                    result[Index_of_A][1] = 0;
                    result[Index_of_A][2] = Steps[i][2];    //predkoscod od ktorejprzspiesza dana sekcja akcelerate
                    result[Index_of_A][3] = a;//acceleration
                    result[Index_of_A][4] = 1;//type of line
                    result[Index_of_A][5] = Steps[i][0];//node 1
                    result[Index_of_A][6] = -2;//node 2
                }
                else// brake
                {
                    //search for track that ends at step node
                    var Index_of_B = Search_For_Track_Which_ends_with_Node(result, (int)Steps[i][0]);
                    //add brake after that node
                    result.Insert(Index_of_B + 1, new double[9]);
                    result[Index_of_B + 1][0] = 0;
                    result[Index_of_B + 1][1] = Steps[i][1];
                    result[Index_of_B + 1][2] = Steps[i][2];    //predkosc do ktorej hamuje dana sekcja brake
                    result[Index_of_B + 1][3] = a;//acceleration
                    result[Index_of_B + 1][4] = -1;//type of line
                    result[Index_of_B + 1][5] = -2;//node 1
                    result[Index_of_B + 1][6] = Steps[i][0];//node 2
                }
            }
            */
            //metoda wrzucajaca wszystko w macierz wynikowa
            
            return result;
        }

        public List<double[]> Route_Base_MatrixBetweenNoudes(int[][] Route, double a)
        {
            //przygotowanie listy zmian predkosci na drodze ktora bedzie jechac kapsula (int[][] Route)
            var Steps = List_Of_Route_Steps_BetweenNodes(Route);

            var result = new List<double[]>();
            int current_route_node;

            // X0 odleglosc harakterystyczna1 poczatek
            // X1 odleglosc charakterystyczna2 koniec,
            // V predkosc na poczatku, przyspieszenie na poczatku,
            // rodzaj lini (-1 brake, 0 track, 1 accelerate)
            // node 1(poczatek dla a i t)
            // node 2 koniec dla T i B
            // a ma tylko X0, b ma tylko X1, T obiei po przecieciach z A i B tez maja obie, bez zmian


            //wstawienie wszystkich trackow ktore sa obecne w spisie drogi 
            result = Add_All_Tracks_To_Result(result);

            //laczenie trackow o identycznych predkosciach, teraz kiedy chce dodac modyfikacje profilu moze lepiej tego nierobic ?
            //kedy metoda jest wylaczona, profil zawiera poprostu spis wszystkich odcinkow drogi, a nie tylko logiczne sekcje
            //result = Consolidate_Track_with_The_same_speed(result);

            //dodanie skokow z listy steps
            for (int i = 0; i < Steps.Length; i++)
            {
                //type of step
                if (Steps[i][3] == 1)//acceleration
                {
                    //search for track that starts at step node
                    var Index_of_A = Search_For_Track_Which_starts_with_Node(result, (int)Steps[i][0]);
                    //add acceleration before that node 
                    result.Insert(Index_of_A, new double[9]);
                    result[Index_of_A][0] = Steps[i][1];
                    result[Index_of_A][1] = 0;
                    result[Index_of_A][2] = Steps[i][2];    //predkoscod od ktorejprzspiesza dana sekcja akcelerate
                    result[Index_of_A][3] = a;//acceleration
                    result[Index_of_A][4] = 1;//type of line
                    result[Index_of_A][5] = Steps[i][0];//node 1
                    result[Index_of_A][6] = -2;//node 2
                }
                else// brake
                {
                    //search for track that ends at step node
                    var Index_of_B = Search_For_Track_Which_ends_with_Node(result, (int)Steps[i][0]);
                    //add brake after that node
                    result.Insert(Index_of_B + 1, new double[9]);
                    result[Index_of_B + 1][0] = 0;
                    result[Index_of_B + 1][1] = Steps[i][1];
                    result[Index_of_B + 1][2] = Steps[i][2];    //predkosc do ktorej hamuje dana sekcja brake
                    result[Index_of_B + 1][3] = a;//acceleration
                    result[Index_of_B + 1][4] = -1;//type of line
                    result[Index_of_B + 1][5] = -2;//node 1
                    result[Index_of_B + 1][6] = Steps[i][0];//node 2
                }
            }


            //metoda wrzucajaca wszystko w macierz wynikowa
            return result;
        }
        public List<double[]> Route_Base_MatrixBetweenIntersections(int[][] Route, double a, double start_speed)
        {
            //przygotowanie listy zmian predkosci na drodze ktora bedzie jechac kapsula (int[][] Route)
            var Steps = List_Of_Route_Steps_BetweenIntersections(Route, start_speed);

            var result = new List<double[]>();
            int current_route_node;

            // X0 odleglosc harakterystyczna1 poczatek
            // X1 odleglosc charakterystyczna2 koniec,
            // V predkosc na poczatku, przyspieszenie na poczatku,
            // rodzaj lini (-1 brake, 0 track, 1 accelerate)
            // node 1(poczatek dla a i t)
            // node 2 koniec dla T i B
            // na poczatku a ma tylko 1 X, b ma tylko 2 X, T obie. po przecieciach A i B tez maja obie.


            //wstawienie wszystkich trackow ktore sa obecne w spisie drogi 
            result = Add_All_Tracks_To_Result(result);

            //laczenie trackow o identycznych predkosciach, teraz kiedy chce dodac modyfikacje profilu moze lepiej tego nierobic ?
            //kedy metoda jest wylaczona, profil zawiera poprostu spis wszystkich odcinkow drogi, a nie tylko logiczne sekcje
            //result = Consolidate_Track_with_The_same_speed(result);

            //dodanie skokow z listy steps
            for (int i = 0; i < Steps.Length; i++)
            {
                //type of step
                if (Steps[i][3] == 1)//acceleration
                {
                    //search for track that starts at step node
                    var Index_of_A = Search_For_Track_Which_starts_with_Node(result, (int)Steps[i][0]);
                    //add acceleration before that node 
                    result.Insert(Index_of_A, new double[9]);
                    result[Index_of_A][0] = Steps[i][1];
                    result[Index_of_A][1] = 0;
                    result[Index_of_A][2] = Steps[i][2];    //predkoscod od ktorejprzspiesza dana sekcja akcelerate
                    result[Index_of_A][3] = a;//acceleration
                    result[Index_of_A][4] = 1;//type of line
                    result[Index_of_A][5] = Steps[i][0];//node 1
                    result[Index_of_A][6] = -2;//node 2
                }
                else// brake
                {
                    //search for track that ends at step node
                    var Index_of_B = Search_For_Track_Which_ends_with_Node(result, (int)Steps[i][0]);
                    //add brake after that node
                    result.Insert(Index_of_B + 1, new double[9]);
                    result[Index_of_B + 1][0] = 0;
                    result[Index_of_B + 1][1] = Steps[i][1];
                    result[Index_of_B + 1][2] = Steps[i][2];    //predkosc do ktorej hamuje dana sekcja brake
                    result[Index_of_B + 1][3] = a;//acceleration
                    result[Index_of_B + 1][4] = -1;//type of line
                    result[Index_of_B + 1][5] = -2;//node 1
                    result[Index_of_B + 1][6] = Steps[i][0];//node 2
                }
            }


            //metoda wrzucajaca wszystko w macierz wynikowa
            return result;
        }

        public List<double[]> Add_All_Tracks_To_Result(List<double[]> result)
        {
            for (int i = 0; i < _route.Length; i++)
            {
                result.Add(new double[9]);
                result[i][0] = Distance_To_Node_On_Route(_route[i][0]);
                result[i][1] = Distance_To_Node_On_Route(_route[i][1]);
                result[i][2] = _listOfTracks[_route[i][2]].SpeedLimit;
                result[i][3] = 0;//acceleration
                result[i][4] = 0;//type of line
                result[i][5] = _route[i][0];//node 1
                result[i][6] = _route[i][1];//node 2
            }
            return result;
        }

        public List<double[]> Consolidate_Track_with_The_same_speed(List<double[]> result)
        {
            int k = 0;
            while (k < (result.Count - 1))
            {
                if (result[k][2] == result[k + 1][2])
                {
                    result[k][1] = result[k + 1][1];
                    result[k][6] = result[k + 1][6];//node 2
                    result.RemoveAt(k + 1);
                }
                else
                    k++;
            }
            return result;
        }

        public int Search_For_Track_Which_starts_with_Node(List<double[]> res, int Node)
        {
            for(int i = 0; i < res.Count; i++)
            {
                if (res[i][5] == Node)
                {
                    return i;
                    break;
                }


            }
            return -1;

        }

        public int Search_For_Track_Which_ends_with_Node(List<double[]> res, int Node)
        {
            for (int i = 0; i < res.Count; i++)
            {
                if (res[i][6] == Node)
                {
                    return i;
                    break;
                }


            }
            return -1;

        }

        public double [][] Convert_To_Matrix(List<double[]> InPut)
        {
            var Matrix = new double[InPut.Count][];
            for(int i = 0; i < InPut.Count; i++)
            {
                Matrix[i] = InPut[i];
            }
            return Matrix;
        }
        public double Distance_To_Node_On_Route(int node)
        {
            double Distance=0;

            for(int i = 0; i < _route.Length; i++)
            {
                if (node == _route[i][0])
                    break;
                Distance += _listOfTracks[_route[i][2]].Length;
            }

            return Distance;
        }

    }




}
