using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulation
{
    class SearchAlgorithm
    {
        private readonly Node[] _listOfNodes;
        private readonly Track[] _listOfTracks;
        private readonly Station[] _listOfStations;

        int[][] Route;

        public SearchAlgorithm(CityDataStorage mapOfCity)
        {
            _listOfNodes = mapOfCity.GetListOFAllNodesInCity();
            _listOfTracks = mapOfCity.GetListOfAllTracksInCity();
            _listOfStations = mapOfCity.GetListOfAllStationsInCity();


            

        }

        public int[][] Initialize_Route_Matrix()
        {
            Route = new int[1000][];
            for (int i = 0; i < 1000; i++)
            {
                Route[i] = new int[3];
            }
            return Route;
        }

        public int[][] ShortestRouteBetweenStations(int StartStation,int FinishStation,double[][] DifficultyMatrix)
        {
            Route = Initialize_Route_Matrix();
            int CurrenNode = _listOfStations[StartStation].NumberOfAttachedNode;
            double CurrentDistance = DifficultyMatrix[CurrenNode][0];
            bool IsFinishReached = false;
            int BestTurn;
            int i=0;
            int nextRouteTrack=0;
            int FinishNode = _listOfStations[FinishStation].NumberOfAttachedNode;
            int Previous_Track_In_Route=-1;


            while(IsFinishReached != true)
            {
                Route[i][0] = CurrenNode;
                
                BestTurn = ChooseShortestTurnNode(CurrentDistance, PossibleTurns(CurrenNode, Previous_Track_In_Route), DifficultyMatrix);
                Route[i][1] = BestTurn;

                Previous_Track_In_Route = TrackNumberConnectingTwoNodes(CurrenNode, BestTurn);
                Route[i][2] = Previous_Track_In_Route;

                CurrenNode = BestTurn;
                CurrentDistance = DifficultyMatrix[CurrenNode][0];
                if (CurrenNode == FinishNode)
                    IsFinishReached = true;
                i++;
            }

            return To_Final_Matrix(Route);
            

        }

        public int[][] ShortestRouteBetweenNodes(int StartNode, int FinishNode, double[][] DifficultyMatrix)
        {
            Route = Initialize_Route_Matrix();
            int CurrenNode = StartNode;
            double CurrentDistance = DifficultyMatrix[CurrenNode][0];
            bool IsFinishReached = false;
            int BestTurn;
            int i = 0;
            int nextRouteTrack = 0;
            //int FinishNode = _listOfStations[FinishStation].NumberOfAttachedNode;
            int Previous_Track_In_Route = -1;


            while (IsFinishReached != true)
            {
                Route[i][0] = CurrenNode;

                BestTurn = ChooseShortestTurnNode(CurrentDistance, PossibleTurns(CurrenNode, Previous_Track_In_Route), DifficultyMatrix);
                Route[i][1] = BestTurn;

                Previous_Track_In_Route = TrackNumberConnectingTwoNodes(CurrenNode, BestTurn);
                Route[i][2] = Previous_Track_In_Route;

                CurrenNode = BestTurn;
                CurrentDistance = DifficultyMatrix[CurrenNode][0];
                if (CurrenNode == FinishNode)
                    IsFinishReached = true;
                i++;
            }

            return To_Final_Matrix(Route);


        }

        public int[][] To_Final_Matrix(int [][] Route)
        {
            int dlugosc = 0;
            for (int k = 0; k < 1000; k++)
            {

                if (Route[k][0] == 0 & Route[k][1] == 0 & Route[k][2] == 0)
                {
                    dlugosc = k;
                    break;
                }

            }

            var FinalRoute = new int[dlugosc][];
            for (int k = 0; k < dlugosc; k++)
            {
                FinalRoute[k] = new int[3];
            }

            for (int k = 0; k < dlugosc; k++)
            {
                FinalRoute[k][0] = Route[k][0];
                FinalRoute[k][1] = Route[k][1];
                FinalRoute[k][2] = Route[k][2];
            }


            return FinalRoute;


        }

        private int TrackNumberConnectingTwoNodes(int Node1, int Node2)
        {
            var N1 = _listOfNodes[Node1].GetNumbersOfAllConnectedTracks();
            var N2 = _listOfNodes[Node2].GetNumbersOfAllConnectedTracks();

            foreach (int n in N1)
            {
                if (N2.Contains(n))
                    return n;
            }
            return -1;
        }

        private int ChooseShortestTurnNode(double Current_Distance, List<int> Lista, double[][] DifficultyMatrix)
        {
            double difficultyOfTurnNode;
            var Odleglosc = new List<double>();
            foreach (int node in Lista)
            {
                difficultyOfTurnNode = DifficultyMatrix[node][0];
                Odleglosc.Add(Current_Distance - difficultyOfTurnNode);
            }

            return Lista[Odleglosc.IndexOf(Odleglosc.Max())];
        }

        private List<int> PossibleTurns(int node, int Initial_Track)
        {
            var Possible_turns = new List<int>();
            int NodeOnOthereSide;
            var ConnectedTracks = _listOfNodes[node].GetNumbersOfAllConnectedTracks();
            if (Initial_Track != -1)
            {
                ConnectedTracks.Remove(Initial_Track);
            }
            foreach (int T in ConnectedTracks)
            {
                if (CheckIfDirectionalityIsOk(node, T))
                {
                    NodeOnOthereSide = _listOfTracks[T].NodeOnOthereSideOfTrack(node);
                    Possible_turns.Add(NodeOnOthereSide);
                }
                else
                    continue;

            }
            return Possible_turns;
        }


        private bool CheckIfDirectionalityIsOk(int node, int Track)
        {
            if (_listOfTracks[Track].DirectionalNode == node)
                return true;
            else
                return false;
        }

    }
}
