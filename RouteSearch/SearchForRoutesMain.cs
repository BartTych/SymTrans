using System.Collections.Generic;
using Symulation;

namespace Symulation
{







    public class SearchForRoutesMain
    {
        private CityDataStorage CityMap;
        private readonly Node[] _listOfNodes;
        private CreatingRouteDificultyMap CreateDificultyMap;

        private double[][] MatrixOfDifficultyForNodesToFinishStation;
        private int[][][] FinalRoutes;


        public SearchForRoutesMain(CityDataStorage mapOfCity)
        {
            CityMap = mapOfCity;
            _listOfNodes = CityMap.GetListOFAllNodesInCity();
            CreateDificultyMap = new CreatingRouteDificultyMap(mapOfCity);

            MatrixOfDifficultyForNodesToFinishStation = new double[CityMap.NumberOfNodes()][];
            FinalRoutes = new int[1][][];//, 3,200];
            for (int i = 0; i < 1; i++)
            {
                FinalRoutes[i] = new int[3][];
                for (int j = 0; j < 3; j++)
                    FinalRoutes[i][j] = new int[200];
            }
        }


        public int[][][] SeekRouteBetweenStations (int StartStationNumber , int FinishStationNumber)     
        {

            var CreateDificultyMap = new CreatingRouteDificultyMap(CityMap);
            MatrixOfDifficultyForNodesToFinishStation = CreateDificultyMap.DistanceMappingInSearchBetweenStations(FinishStationNumber);
            //System.Console.WriteLine("Distance maping :");
            //for (int i = 0; i < MatrixOfDifficultyForNodesToFinishStation.Length; i++)
            //{
            //    System.Console.Write("node : " + i +"   ");
            //    System.Console.Write(MatrixOfDifficultyForNodesToFinishStation[i][0]+"   ");
            //    System.Console.Write(MatrixOfDifficultyForNodesToFinishStation[i][1] + "   ");
            //    System.Console.WriteLine(MatrixOfDifficultyForNodesToFinishStation[i][2] + "   ");
            //}


            var Search = new SearchAlgorithm(CityMap);

            FinalRoutes[0]= Search.ShortestRouteBetweenStations(StartStationNumber, FinishStationNumber, MatrixOfDifficultyForNodesToFinishStation);
            

            return FinalRoutes;       
        }

        public int[][][] SeekRouteBetweenNodes(int StartNode, int FinishNode)      //algorithm searches shortest route
        {

            //var CreateDificultyMap = new CreatingRouteDificultyMap(CityMap);
            MatrixOfDifficultyForNodesToFinishStation = CreateDificultyMap.DistanceMappingInSearchBetweenNodes(FinishNode);
            //System.Console.WriteLine("Distance maping :");
            //for (int i = 0; i < MatrixOfDifficultyForNodesToFinishStation.Length; i++)
            //{
            //    System.Console.Write("node : " + i +"   ");
            //    System.Console.Write(MatrixOfDifficultyForNodesToFinishStation[i][0]+"   ");
            //    System.Console.Write(MatrixOfDifficultyForNodesToFinishStation[i][1] + "   ");
            //    System.Console.WriteLine(MatrixOfDifficultyForNodesToFinishStation[i][2] + "   ");
            //}


            var Search = new SearchAlgorithm(CityMap);

            FinalRoutes[0] = Search.ShortestRouteBetweenNodes(StartNode, FinishNode, MatrixOfDifficultyForNodesToFinishStation);


            return FinalRoutes;
        }

    }
}
