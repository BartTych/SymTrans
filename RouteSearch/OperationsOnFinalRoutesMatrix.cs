namespace Symulation
{
    public class OperationsOnFinalRoutesMatrix
    {

        public int[,] ExtractOneRoute(int[][][] AllRoutes, int RouteNumberIndex)
        {
            int lengthOfRoute=0;

            for (int i = 0; i < AllRoutes.GetLength(0); i++)
            {
                if (AllRoutes[i][ 0][ RouteNumberIndex] == 0 & AllRoutes[i][ 1][ RouteNumberIndex] == 0 & AllRoutes[i][ 2][ RouteNumberIndex] == 0)
                    break;
                else
                    lengthOfRoute++;
            }

            int[,] Route = new int[lengthOfRoute, 3];

            for(int i = 0; i < lengthOfRoute; i++)
            {
                Route[i, 0] = AllRoutes[i][ 0][ RouteNumberIndex];
                Route[i, 1] = AllRoutes[i][ 1][ RouteNumberIndex];
                Route[i, 2] = AllRoutes[i][ 2][ RouteNumberIndex];
            }

            return Route;
        }

        public int HowManyRoutesWhereFound (int[,,] AllRoutes)
        {
            int numberOfRoutes=0;

            for (int i =0;i<AllRoutes.GetLength(2); i++)
            {
                if (AllRoutes[0, 0, i] == 0 & AllRoutes[0, 1, i] == 0 & AllRoutes[0, 2, i] == 0)
                    break;
                else
                    numberOfRoutes++;
            }

            return numberOfRoutes;
        }



    }
}
