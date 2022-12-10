using System.Collections.Generic;
using System.Linq;

namespace Symulation
{
    public class MenagementOfCitySections
    {

        private CityDataStorage CityMap;
        private CityDataStorage CityMapCopy;
        private RepositoryOfPermutations repositoryOfPermutations;
        private Track[] _listOfTracks;
        private Node[] _listOfNodes;
        private Station[] _listOfStations;
        private ModOfCityMap _modOfcity;
        private List<CitySection> _listOfSections;



        public MenagementOfCitySections(CityDataStorage mapOfCity)
        {
            CityMap = mapOfCity;
            CityMapCopy = CityMap.DeepCopyCity();
            repositoryOfPermutations = new RepositoryOfPermutations(mapOfCity);
            _listOfTracks = CityMap.GetListOfAllTracksInCity();
            _listOfNodes = CityMap.GetListOFAllNodesInCity();
            _listOfStations = CityMap.GetListOfAllStationsInCity();
            _modOfcity = new ModOfCityMap(CityMap);
        }

        //add new connector section   type: sorting v1.0

        //v1.0
        public void AddNewSortingConnectorSectionToCity(int StartTrack, double Length)
        {

            var (AffectedTracks, DistanceIntoAffectedTrack, error_code) = _modOfcity.SearchForLocationOfOthereSideOfMod_AlongTraffic(StartTrack, Length);
            var track_with_new_node = AffectedTracks.Last();
            if (error_code != 0)
            {
                //tu bedzie implementacja tego co zrobic jesli jest error
            }



            System.Console.WriteLine("Number of tracks affected by mod: ");
            if (AffectedTracks.Count > 0)
            {
                foreach (int n in AffectedTracks)
                {
                    System.Console.WriteLine(n);
                }
            }
            else
            {
                System.Console.WriteLine("0");
            }

            // modify city

            var (new_node, new_track, is_new_track_created) = _modOfcity.AddNewNodeToTrackAlongTraffic(StartTrack, DistanceIntoAffectedTrack);

            // add Section to City list of sections 
            // Have to define repository of that data 


        }
        // to jest tylko jako test dodawania w przeciwnym kierunku
        public void AddNewSortingConnectorSectionToCityTheWrongWay(int StartTrack, double Length)
        {

            //var liczba = ChangeSpeedOfTrack()
            // Read Tracks attached to node
            //var Nodes = _listOfNodes[StartNode].GetNumbersOfAllConnectedTracks();
            // if node is intersection print error
            //if (Nodes.Count > 2)
            //{
            //    System.Console.WriteLine("Start node of Sorting Section is intersection");
            //}

            // check where new node is located (number of track) and if there is no intersections along the way
            var (AffectedTracks, DistanceIntoAffectedTrack, error_code) = _modOfcity.SearchForLocationOfOthereSideOfMod_WrongWay(StartTrack, Length);

            System.Console.WriteLine("Number of tracks affected by mod: ");
            if (AffectedTracks.Count > 0)
            {
                foreach (int n in AffectedTracks)
                {
                    System.Console.WriteLine(n);
                }
            }
            else
            {
                System.Console.WriteLine("0");
            }

            // modify city

            var (LastNode, affectedTrack, is_new_track_created) = _modOfcity.AddNewNodeToTrackTheWrongWay(StartTrack, DistanceIntoAffectedTrack);

            // add Section to City list of sections 
            // Have to define repository of that data 


        }


        public struct PodConfig  //wstepna proba opisu tego co sie dzieje na wejsciu w trawersie przez sekcje
        {
            public List<int> Numer { get; set; }
            public List<int> Odleglosc { get; set; }
            public List<byte> Kierunek { get; set; }

        }

        



       

       



       
    
    }

    

}
