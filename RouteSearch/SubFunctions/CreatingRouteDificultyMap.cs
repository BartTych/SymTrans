using System.Collections.Generic;

namespace Symulation
{
    class CreatingRouteDificultyMap
    {
        private readonly CityDataStorage CityMap;
        private readonly Track[] _listOfTracks;
        private readonly Node [] _listOfNodes;
        private readonly Station[] _listOfStations;
        int NodeCount;

        private readonly bool[] WorkMatrixOfLeadingNodesDuringRouteSearch;
        private readonly int[][] WorkMartixOfDistanceToNodeDuringRouteSearch;
        private bool AllLeadingNodesAreClosed;
        private List<int> TracksConectedToNode;
        private int NodeNumberOnOtherSideOfTrack;

        public CreatingRouteDificultyMap(CityDataStorage mapOfCity)
        {
            this.CityMap = mapOfCity;
            _listOfTracks = CityMap.GetListOfAllTracksInCity();
            _listOfNodes = CityMap.GetListOFAllNodesInCity();
            _listOfStations = CityMap.GetListOfAllStationsInCity();

            WorkMatrixOfLeadingNodesDuringRouteSearch = new bool[CityMap.NumberOfNodes()];
            WorkMartixOfDistanceToNodeDuringRouteSearch = new int[CityMap.NumberOfNodes()][];
            AllLeadingNodesAreClosed = false;
            for (int n = 0; n < CityMap.NumberOfNodes(); n++)
                WorkMartixOfDistanceToNodeDuringRouteSearch[n] = new int[3];
        }
        


        public double[][] DistanceMappingInSearchBetweenStations(int Finish_Station_Number)
        {

            var ActiveNodes = new List<int>();
            var DistanceMap = new double [CityMap.NumberOfNodes()][];
            int Active_work_Node;
            int Active_work_Track;

            for(int i =0 ;i< CityMap.NumberOfNodes();i++)
                DistanceMap[i] = new double[3] {-1,-1,-1};

            ActiveNodes.Add(_listOfStations[Finish_Station_Number].NumberOfAttachedNode);
            DistanceMap[_listOfStations[Finish_Station_Number].NumberOfAttachedNode][0] = 1;  //ustalam dystans startu jako 1

            while (ActiveNodes.Count != 0)
            {
                //work node
                Active_work_Node = ActiveNodes[ActiveNodes.Count-1];  //ostatni na liscie
                //usuwam active node z listy
                ActiveNodes.Remove(Active_work_Node);//usuwany jest work node z listy aktywnych nodow. jesli sie powtarzaja to jest usuwany pierwszy co jest ok

                var possibleTurns = PossibleTurns(Active_work_Node, (int)DistanceMap[Active_work_Node][2]);

                if(possibleTurns != null)
                foreach(int n in possibleTurns)
                {
                    Active_work_Track = TrackNumberConnectingTwoNodes(Active_work_Node, n);
                    //wiem ze to jest dozwolony skret, kierunkowosc sie zgadza

                    //sprawdzam czy jest juz zdefiniowana odleglosc
                    if (DistanceMap[n][0] == -1)
                    {
                        goto Work;
                    }
                    else if (DistanceMap[n][0] > DistanceMap[Active_work_Node][0] + _listOfTracks[Active_work_Track].Length)
                    {
                        goto Work;
                    }
                    else
                        continue;


                    Work:
                        //dodaje dany node do listy aktywnych
                        ActiveNodes.Add(n);
                        //definiuje
                        DistanceMap[n][0] = DistanceMap[Active_work_Node][0] + _listOfTracks[Active_work_Track].Length;
                        DistanceMap[n][2] = Active_work_Track;


                }

            }
            return DistanceMap;

        }
        public double[][] DistanceMappingInSearchBetweenNodes(int Finish_Node_Number)
        {

            var ActiveNodes = new List<int>();
            var DistanceMap = new double[CityMap.NumberOfNodes()][];
            int Active_work_Node;
            int Active_work_Track;

            for (int i = 0; i < CityMap.NumberOfNodes(); i++)
                DistanceMap[i] = new double[3] { -1, -1, -1 }; //dystance,   ,numer tracku z ktrego dojechal algorytm do tego miejsca

            ActiveNodes.Add(Finish_Node_Number);
            DistanceMap[Finish_Node_Number][0] = 1;  //ustalam dystans startu algorytmu (finish node) jako 1 zeby odroznic, niepamietam dokladnie o co chodzilo

            while (ActiveNodes.Count != 0)
            {
                //work node
                Active_work_Node = ActiveNodes[ActiveNodes.Count - 1];  //ostatni na liscie
                //usuwam active node z listy
                ActiveNodes.Remove(Active_work_Node);//usuwany jest work node z listy aktywnych nodow. jesli sie powtarzaja to jest usuwany pierwszy co jest ok

                var possibleTurns = PossibleTurns(Active_work_Node, (int)DistanceMap[Active_work_Node][2]);

                if (possibleTurns != null)
                    foreach (int n in possibleTurns)
                    {
                        Active_work_Track = TrackNumberConnectingTwoNodes(Active_work_Node, n);
                        //wiem ze to jest dozwolony skret, kierunkowosc sie zgadza

                        //sprawdzam czy jest juz zdefiniowana odleglosc
                        if (DistanceMap[n][0] == -1)
                        {
                            goto Work;
                        }
                        else if (DistanceMap[n][0] > DistanceMap[Active_work_Node][0] + _listOfTracks[Active_work_Track].Length)
                        {
                            goto Work;
                        }
                        else
                            continue;


                        Work:
                        //dodaje dany node do listy aktywnych
                        ActiveNodes.Add(n);
                        //definiuje
                        DistanceMap[n][0] = DistanceMap[Active_work_Node][0] + _listOfTracks[Active_work_Track].Length;
                        DistanceMap[n][2] = Active_work_Track;
                    }
            }
            return DistanceMap;

        }

        private List<int> PossibleTurns (int node, int Incoming_Track)  //wyzyca liste nodow gdzie mozna skrecic
        {
            var Possible_Turns = new List<int>();

            var Tracks = _listOfNodes[node].GetNumbersOfAllConnectedTracks();
            Tracks.Remove(Incoming_Track);

            foreach (int n in Tracks)
            {
                if(CheckIfDirectionalityIsOk(node,n))
                Possible_Turns.Add(_listOfTracks[n].NodeOnOthereSideOfTrack(node));
            }


            return Possible_Turns;


        }

        private bool CheckIfDirectionalityIsOk(int node, int Track)
        {
            if (_listOfTracks[Track].DirectionalNode != node)
                return true;
            else
                return false;
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

    }

}
