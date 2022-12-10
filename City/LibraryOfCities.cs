using System.Collections.Generic;

namespace Symulation
{
    public class LibraryOfCities
    {
        public CityDataStorage First_test(double speed)
        {
            var CityMap = new CityDataStorage();

            CityMap.AddStraightTrackElementToCityMap(0, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(1, 400, 0);
            CityMap.AddStraightRefTrackElementToCityMap(2, 400 - 83, 0);
            CityMap.AddStraightTrackElementToCityMap(3, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(4, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(5, 800, 0);
            CityMap.AddStraightTrackElementToCityMap(6, 560, -45);
            CityMap.AddStraightRefTrackElementToCityMap(7, 560, 45);
            CityMap.AddStraightTrackElementToCityMap(8, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(9, 800, 90);
            CityMap.AddStraightTrackElementToCityMap(10, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(11, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(12, 560, 135);
            CityMap.AddStraightRefTrackElementToCityMap(13, 560, 225);
            CityMap.AddStraightTrackElementToCityMap(14, 800, 180);
            CityMap.AddStraightTrackElementToCityMap(15, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(16, 800 - 83, 180);
            CityMap.AddStraightTrackElementToCityMap(17, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(18, 560, 135);
            CityMap.AddStraightRefTrackElementToCityMap(19, 560, 225);
            CityMap.AddStraightTrackElementToCityMap(20, 800, 180);
            CityMap.AddStraightTrackElementToCityMap(21, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(22, 560, 225);
            CityMap.AddStraightTrackElementToCityMap(23, 560, -45);
            CityMap.AddStraightTrackElementToCityMap(24, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(25, 800, 0);
            CityMap.AddStraightTrackElementToCityMap(26, 560, -45);
            CityMap.AddStraightRefTrackElementToCityMap(27, 560, 45);

            CityMap.AddNodeToCityMap(0, 23, 24);
            CityMap.define_position_of_node(0, 0, 0);
            CityMap.AddNodeToCityMap(1, 24, 25, 26);
            CityMap.AddNodeToCityMap(2, 26, 27);
            CityMap.AddNodeToCityMap(3, 25, 27, 0);
            CityMap.AddNodeToCityMap(4, 0, 1);
            CityMap.AddNodeToCityMap(5, 1, 2);
            CityMap.AddNodeToCityMap(6, 2, 3);
            CityMap.AddNodeToCityMap(7, 3, 4);
            CityMap.AddNodeToCityMap(8, 4, 5, 6);
            CityMap.AddNodeToCityMap(9, 6, 7);
            CityMap.AddNodeToCityMap(10, 5, 7, 8);
            CityMap.AddNodeToCityMap(11, 8, 9);
            CityMap.AddNodeToCityMap(12, 9, 10);
            CityMap.AddNodeToCityMap(13, 10, 11);
            CityMap.AddNodeToCityMap(14, 11, 14, 12);
            CityMap.AddNodeToCityMap(15, 12, 13);
            CityMap.AddNodeToCityMap(16, 13, 14, 15);
            CityMap.AddNodeToCityMap(17, 15, 16);
            CityMap.AddNodeToCityMap(18, 16, 17);
            CityMap.AddNodeToCityMap(19, 17, 18, 20);
            CityMap.AddNodeToCityMap(20, 18, 19);
            CityMap.AddNodeToCityMap(21, 19, 20, 21);
            CityMap.AddNodeToCityMap(22, 21, 22);
            CityMap.AddNodeToCityMap(23, 22, 23);

            CityMap.DefineDirectionForTrack(1, 4);
            CityMap.DefineDirectionForTrack(9, 11);
            CityMap.DefineDirectionForTrack(16, 17);
            CityMap.DefineDirectionForTrack(22, 22);

            CityMap.DefineDirectionForTrack(26, 1);
            CityMap.DefineDirectionForTrack(27, 2);
            CityMap.DefineDirectionForTrack(25, 1);

            CityMap.DefineDirectionForTrack(6, 8);
            CityMap.DefineDirectionForTrack(7, 9);
            CityMap.DefineDirectionForTrack(5, 8);

            CityMap.DefineDirectionForTrack(12, 14);
            CityMap.DefineDirectionForTrack(13, 15);
            CityMap.DefineDirectionForTrack(14, 14);

            CityMap.DefineDirectionForTrack(18, 19);
            CityMap.DefineDirectionForTrack(19, 20);
            CityMap.DefineDirectionForTrack(20, 19);

            CityMap.DefineSpeedForTrack_Km_h(16, speed);

            CityMap.AddStationToCityMap(0, 2, 15, 15);
            CityMap.AddStationToCityMap(1, 9, 15, 15);
            CityMap.AddStationToCityMap(2, 15, 15, 15);
            CityMap.AddStationToCityMap(3, 20, 15, 15);

            return CityMap;
        }

        public CityDataStorage Second_test(double speed)
        {
            var CityMap = new CityDataStorage();

            CityMap.AddStraightTrackElementToCityMap(0, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(1, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(2, 100 - 21, 0);

            CityMap.AddStraightTrackElementToCityMap(3, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(4, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(5, 200, 0);
            CityMap.AddStraightTrackElementToCityMap(6, 140, -45);
            CityMap.AddStraightRefTrackElementToCityMap(7, 140, 45);

            CityMap.AddStraightTrackElementToCityMap(8, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(9, 280, 90);
            CityMap.AddStraightTrackElementToCityMap(10, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(11, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(12, 140, 135);
            CityMap.AddStraightRefTrackElementToCityMap(13, 140, 225);

            CityMap.AddStraightTrackElementToCityMap(14, 200, 180);
            CityMap.AddStraightTrackElementToCityMap(15, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(16, 200 - 21, 180);
            CityMap.AddStraightTrackElementToCityMap(17, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(18, 140, 135);
            CityMap.AddStraightRefTrackElementToCityMap(19, 140, 225);

            CityMap.AddStraightTrackElementToCityMap(20, 200, 180);
            CityMap.AddStraightTrackElementToCityMap(21, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(22, 140, 270);
            CityMap.AddStraightTrackElementToCityMap(23, 140, 270);
            CityMap.AddStraightTrackElementToCityMap(24, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(25, 200, 0);
            CityMap.AddStraightTrackElementToCityMap(26, 140, -45);
            CityMap.AddStraightRefTrackElementToCityMap(27, 140, 45);


            //nody plus siec polaczen z trackami
            //def polozenia nouda zero
            CityMap.AddNodeToCityMap(0, 23, 24);
            CityMap.define_position_of_node(0, 0, 0);
            CityMap.AddNodeToCityMap(1, 24, 25, 26);
            CityMap.AddNodeToCityMap(2, 26, 27);
            CityMap.AddNodeToCityMap(3, 25, 27, 0);
            CityMap.AddNodeToCityMap(4, 0, 1);
            CityMap.AddNodeToCityMap(5, 1, 2);
            CityMap.AddNodeToCityMap(6, 2, 3);
            CityMap.AddNodeToCityMap(7, 3, 4);
            CityMap.AddNodeToCityMap(8, 4, 5, 6);
            CityMap.AddNodeToCityMap(9, 6, 7);
            CityMap.AddNodeToCityMap(10, 5, 7, 8);
            CityMap.AddNodeToCityMap(11, 8, 9);
            CityMap.AddNodeToCityMap(12, 9, 10);
            CityMap.AddNodeToCityMap(13, 10, 11);
            CityMap.AddNodeToCityMap(14, 11, 14, 12);
            CityMap.AddNodeToCityMap(15, 12, 13);
            CityMap.AddNodeToCityMap(16, 13, 14, 15);
            CityMap.AddNodeToCityMap(17, 15, 16);
            CityMap.AddNodeToCityMap(18, 16, 17);
            CityMap.AddNodeToCityMap(19, 17, 18, 20);
            CityMap.AddNodeToCityMap(20, 18, 19);
            CityMap.AddNodeToCityMap(21, 19, 20, 21);
            CityMap.AddNodeToCityMap(22, 21, 22);
            CityMap.AddNodeToCityMap(23, 22, 23);




            CityMap.DefineDirectionForTrack(1, 4);
            CityMap.DefineDirectionForTrack(9, 11);
            CityMap.DefineDirectionForTrack(16, 17);
            CityMap.DefineDirectionForTrack(22, 22);

            CityMap.DefineDirectionForTrack(26, 1);
            CityMap.DefineDirectionForTrack(27, 2);
            CityMap.DefineDirectionForTrack(25, 1);

            CityMap.DefineDirectionForTrack(6, 8);
            CityMap.DefineDirectionForTrack(7, 9);
            CityMap.DefineDirectionForTrack(5, 8);

            CityMap.DefineDirectionForTrack(12, 14);
            CityMap.DefineDirectionForTrack(13, 15);
            CityMap.DefineDirectionForTrack(14, 14);

            CityMap.DefineDirectionForTrack(18, 19);
            CityMap.DefineDirectionForTrack(19, 20);
            CityMap.DefineDirectionForTrack(20, 19);

            CityMap.DefineSpeedForTrack_Km_h(16, speed);

            CityMap.AddStationToCityMap(0, 2, 15, 15);
            CityMap.AddStationToCityMap(1, 9, 15, 15);
            CityMap.AddStationToCityMap(2, 15, 15, 15);
            CityMap.AddStationToCityMap(3, 20, 15, 15);

            return CityMap;
        }

        public CityDataStorage Zwiekszanie_wydajnosci(double speed)
        {
            var CityMap = new CityDataStorage();

            CityMap.AddStraightTrackElementToCityMap(0, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(1, 400, 0);
            CityMap.AddStraightRefTrackElementToCityMap(2, 100 - 21, 0);


            CityMap.AddStraightTrackElementToCityMap(3, 300, 0);
            CityMap.AddStraightTrackElementToCityMap(4, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(5, 200, 0);
            CityMap.AddStraightTrackElementToCityMap(6, 140, -45);
            CityMap.AddStraightRefTrackElementToCityMap(7, 140, 45);

            CityMap.AddStraightTrackElementToCityMap(8, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(9, 480, 90);
            CityMap.AddStraightTrackElementToCityMap(10, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(11, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(12, 140, 135);
            CityMap.AddStraightRefTrackElementToCityMap(13, 140, 225);

            CityMap.AddStraightTrackElementToCityMap(14, 200, 180);
            CityMap.AddStraightTrackElementToCityMap(15, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(16, 700 - 21, 180);
            CityMap.AddStraightTrackElementToCityMap(17, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(18, 140, 135);
            CityMap.AddStraightRefTrackElementToCityMap(19, 140, 225);

            CityMap.AddStraightTrackElementToCityMap(20, 200, 180);
            CityMap.AddStraightTrackElementToCityMap(21, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(22, 240, 270);
            CityMap.AddStraightTrackElementToCityMap(23, 240, 270);
            CityMap.AddStraightTrackElementToCityMap(24, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(25, 200, 0);
            CityMap.AddStraightTrackElementToCityMap(26, 140, -45);
            CityMap.AddStraightRefTrackElementToCityMap(27, 140, 45);


            //nody plus siec polaczen z trackami
            //def polozenia nouda zero
            CityMap.AddNodeToCityMap(0, 23, 24);
            CityMap.define_position_of_node(0, 0, 0);
            CityMap.AddNodeToCityMap(1, 24, 25, 26);
            CityMap.AddNodeToCityMap(2, 26, 27);
            CityMap.AddNodeToCityMap(3, 25, 27, 0);
            CityMap.AddNodeToCityMap(4, 0, 1);
            CityMap.AddNodeToCityMap(5, 1, 2);
            CityMap.AddNodeToCityMap(6, 2, 3);
            CityMap.AddNodeToCityMap(7, 3, 4);
            CityMap.AddNodeToCityMap(8, 4, 5, 6);
            CityMap.AddNodeToCityMap(9, 6, 7);
            CityMap.AddNodeToCityMap(10, 5, 7, 8);
            CityMap.AddNodeToCityMap(11, 8, 9);
            CityMap.AddNodeToCityMap(12, 9, 10);
            CityMap.AddNodeToCityMap(13, 10, 11);
            CityMap.AddNodeToCityMap(14, 11, 14, 12);
            CityMap.AddNodeToCityMap(15, 12, 13);
            CityMap.AddNodeToCityMap(16, 13, 14, 15);
            CityMap.AddNodeToCityMap(17, 15, 16);
            CityMap.AddNodeToCityMap(18, 16, 17);
            CityMap.AddNodeToCityMap(19, 17, 18, 20);
            CityMap.AddNodeToCityMap(20, 18, 19);
            CityMap.AddNodeToCityMap(21, 19, 20, 21);
            CityMap.AddNodeToCityMap(22, 21, 22);
            CityMap.AddNodeToCityMap(23, 22, 23);


            CityMap.DefineDirectionForTrack(1, 4);
            CityMap.DefineDirectionForTrack(9, 11);
            CityMap.DefineDirectionForTrack(16, 17);
            CityMap.DefineDirectionForTrack(22, 22);

            CityMap.DefineDirectionForTrack(26, 1);
            CityMap.DefineDirectionForTrack(27, 2);
            CityMap.DefineDirectionForTrack(25, 1);

            CityMap.DefineDirectionForTrack(6, 8);
            CityMap.DefineDirectionForTrack(7, 9);
            CityMap.DefineDirectionForTrack(5, 8);

            CityMap.DefineDirectionForTrack(12, 14);
            CityMap.DefineDirectionForTrack(13, 15);
            CityMap.DefineDirectionForTrack(14, 14);

            CityMap.DefineDirectionForTrack(18, 19);
            CityMap.DefineDirectionForTrack(19, 20);
            CityMap.DefineDirectionForTrack(20, 19);

            CityMap.DefineSpeedForTrack_Km_h(16, speed);

            CityMap.AddStationToCityMap(0, 2, 15, 15);
            CityMap.AddStationToCityMap(1, 9, 15, 15);
            CityMap.AddStationToCityMap(2, 15, 15, 15);
            CityMap.AddStationToCityMap(3, 20, 15, 15);

            return CityMap;
        }
        public CityDataStorage RozgalezianieDC(double speed)
        {
            var CityMap = new CityDataStorage();


            double dlugosc_szalona = 50;

            CityMap.AddStraightRefTrackElementToCityMap(0, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(1, dlugosc_szalona, 0);
            CityMap.AddStraightTrackElementToCityMap(2, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(3, 71, 45);
            CityMap.AddStraightRefTrackElementToCityMap(4, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(5, dlugosc_szalona, 0);
            CityMap.AddStraightTrackElementToCityMap(6, 400, 0);

            CityMap.AddStraightTrackElementToCityMap(26, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(25, 300, 180);
            CityMap.AddStraightTrackElementToCityMap(24, dlugosc_szalona, 180);
            CityMap.AddStraightTrackElementToCityMap(22, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(23, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(21, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(20, dlugosc_szalona, 180);
            CityMap.AddStraightTrackElementToCityMap(19, 300, 180);
            CityMap.AddStraightTrackElementToCityMap(17, 100, 180);

            CityMap.AddStraightTrackElementToCityMap(15, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(14, dlugosc_szalona, 0);
            CityMap.AddStraightTrackElementToCityMap(13, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(12, 71, 45);
            CityMap.AddStraightRefTrackElementToCityMap(11, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(10, dlugosc_szalona, 0);
            CityMap.AddStraightRefTrackElementToCityMap(9, 400, 0);

            CityMap.AddStraightTrackElementToCityMap(7, 100, 90);
            CityMap.AddStraightTrackElementToCityMap(8, 100, -90);
            CityMap.AddStraightTrackElementToCityMap(16, 100, 90);
            CityMap.AddStraightTrackElementToCityMap(18, 100, -90);


            //nody plus siec polaczen z trackami
            //def polozenia nouda zero
            CityMap.AddNodeToCityMap(0, 0, 18);
            CityMap.define_position_of_node(0, 0, 0);
            CityMap.AddNodeToCityMap(1, 0, 1);
            CityMap.AddNodeToCityMap(2, 1, 2, 3);
            CityMap.AddNodeToCityMap(3, 3, 4);
            CityMap.AddNodeToCityMap(4, 2, 4, 5);
            CityMap.AddNodeToCityMap(5, 5, 6);
            CityMap.AddNodeToCityMap(6, 6, 7);
            CityMap.AddNodeToCityMap(7, 7, 8, 26);
            CityMap.AddNodeToCityMap(8, 25, 26);
            CityMap.AddNodeToCityMap(9, 24, 25);
            CityMap.AddNodeToCityMap(10, 22, 23, 24);
            CityMap.AddNodeToCityMap(11, 20, 21, 22);
            CityMap.AddNodeToCityMap(12, 21, 23);
            CityMap.AddNodeToCityMap(13, 19, 20);
            CityMap.AddNodeToCityMap(14, 17, 19);
            CityMap.AddNodeToCityMap(15, 16, 17, 18);
            CityMap.AddNodeToCityMap(16, 15, 16);
            CityMap.AddNodeToCityMap(17, 14, 15);
            CityMap.AddNodeToCityMap(18, 12, 13, 14);
            CityMap.AddNodeToCityMap(19, 11, 12);
            CityMap.AddNodeToCityMap(20, 10, 11, 13);
            CityMap.AddNodeToCityMap(21, 9, 10);
            CityMap.AddNodeToCityMap(22, 8, 9);


            CityMap.DefineDirectionForTrack(2, 2);
            CityMap.DefineDirectionForTrack(3, 2);
            CityMap.DefineDirectionForTrack(4, 3);
            CityMap.DefineDirectionForTrack(1, 1);
            CityMap.DefineDirectionForTrack(5, 4);


            CityMap.DefineDirectionForTrack(24, 9);
            CityMap.DefineDirectionForTrack(23, 10);
            CityMap.DefineDirectionForTrack(21, 12);
            CityMap.DefineDirectionForTrack(22, 10);
            CityMap.DefineDirectionForTrack(20, 11);


            CityMap.DefineDirectionForTrack(15, 16);
            CityMap.DefineDirectionForTrack(13, 18);
            CityMap.DefineDirectionForTrack(12, 18);
            CityMap.DefineDirectionForTrack(11, 19);
            CityMap.DefineDirectionForTrack(10, 20);

            CityMap.DefineDirectionForTrack(7, 6);
            CityMap.DefineDirectionForTrack(8, 22);
            CityMap.DefineDirectionForTrack(18, 15);
            CityMap.DefineDirectionForTrack(16, 15);

            CityMap.DefineSpeedForTrack_Km_h(16, speed);

            CityMap.AddStationToCityMap(0, 3, 15, 15);
            CityMap.AddStationToCityMap(1, 12, 15, 15);
            CityMap.AddStationToCityMap(2, 19, 15, 15);

            return CityMap;
        }





        public CityDataStorage osiem_przystankow(double speed)
        {
            var CityMap = new CityDataStorage();

            //DC sekcje dolna czesc
            CityMap.AddStraightTrackElementToCityMap(0, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(1, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(2, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(3, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(4, 100, 0);

            CityMap.AddStraightTrackElementToCityMap(6, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(9, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(7, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(8, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(10, 100, 0);

            CityMap.AddStraightTrackElementToCityMap(12, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(13, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(15, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(14, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(16, 100, 0);

            CityMap.AddStraightTrackElementToCityMap(18, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(19, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(22, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(21, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(20, 100, 0);


            //DC sekcje gorna czesc 
            CityMap.AddStraightTrackElementToCityMap(24, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(27, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(26, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(25, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(28, 100, 180);

            CityMap.AddStraightTrackElementToCityMap(30, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(33, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(31, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(32, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(34, 100, 180);


            CityMap.AddStraightTrackElementToCityMap(36, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(37, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(38, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(39, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(40, 100, 180);

            CityMap.AddStraightTrackElementToCityMap(42, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(45, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(43, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(44, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(46, 100, 180);

            //Sort sekcje
            CityMap.AddStraightTrackElementToCityMap(5, 400, 0);
            CityMap.AddStraightRefTrackElementToCityMap(11, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(17, 200, 0);
            CityMap.AddStraightTrackElementToCityMap(48, 200, 0);

            CityMap.AddStraightTrackElementToCityMap(29, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(35, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(41, 400, 180);

            CityMap.AddStraightTrackElementToCityMap(23, 400, 90);
            CityMap.AddStraightTrackElementToCityMap(47, 400, 270);



            //nody plus siec polaczen z trackami
            //def polozenia nouda zero
            CityMap.AddNodeToCityMap(0, 0, 47);
            CityMap.define_position_of_node(0, 0, 0);
            CityMap.AddNodeToCityMap(1, 0, 1, 2);
            CityMap.AddNodeToCityMap(2, 1, 3, 4);
            CityMap.AddNodeToCityMap(3, 4, 5);
            CityMap.AddNodeToCityMap(4, 5, 6);
            CityMap.AddNodeToCityMap(5, 6, 7, 9);
            CityMap.AddNodeToCityMap(6, 8, 9, 10);
            CityMap.AddNodeToCityMap(7, 10, 11);
            CityMap.AddNodeToCityMap(8, 11, 12);
            CityMap.AddNodeToCityMap(9, 12, 13, 15);
            CityMap.AddNodeToCityMap(10, 13, 14, 16);
            CityMap.AddNodeToCityMap(11, 16, 17);
            CityMap.AddNodeToCityMap(12, 48, 18);
            CityMap.AddNodeToCityMap(13, 18, 19, 22);
            CityMap.AddNodeToCityMap(14, 19, 20, 21);
            CityMap.AddNodeToCityMap(15, 20, 23);
            CityMap.AddNodeToCityMap(16, 23, 24);
            CityMap.AddNodeToCityMap(17, 24, 27, 26);
            CityMap.AddNodeToCityMap(18, 27, 28, 25);
            CityMap.AddNodeToCityMap(19, 28, 29);
            CityMap.AddNodeToCityMap(20, 29, 30);
            CityMap.AddNodeToCityMap(21, 30, 33, 31);
            CityMap.AddNodeToCityMap(22, 33, 34, 32);
            CityMap.AddNodeToCityMap(23, 34, 35);
            CityMap.AddNodeToCityMap(24, 35, 36);
            CityMap.AddNodeToCityMap(25, 36, 37, 38);
            CityMap.AddNodeToCityMap(26, 37, 40, 39);
            CityMap.AddNodeToCityMap(27, 40, 41);
            CityMap.AddNodeToCityMap(28, 41, 42);
            CityMap.AddNodeToCityMap(29, 42, 45, 43);
            CityMap.AddNodeToCityMap(30, 45, 46, 44);
            CityMap.AddNodeToCityMap(31, 46, 47);
            CityMap.AddNodeToCityMap(32, 2, 3);
            CityMap.AddNodeToCityMap(33, 7, 8);
            CityMap.AddNodeToCityMap(34, 14, 15);
            CityMap.AddNodeToCityMap(35, 21, 22);
            CityMap.AddNodeToCityMap(36, 25, 26);
            CityMap.AddNodeToCityMap(37, 31, 32);
            CityMap.AddNodeToCityMap(38, 38, 39);
            CityMap.AddNodeToCityMap(39, 43, 44);
            CityMap.AddNodeToCityMap(40, 17, 48);

            //directionality dol
            CityMap.DefineDirectionForTrack(0, 0);
            CityMap.DefineDirectionForTrack(2, 1);
            CityMap.DefineDirectionForTrack(1, 1);
            CityMap.DefineDirectionForTrack(3, 32);
            CityMap.DefineDirectionForTrack(4, 2);


            CityMap.DefineDirectionForTrack(7, 5);
            CityMap.DefineDirectionForTrack(9, 5);
            CityMap.DefineDirectionForTrack(8, 33);
            CityMap.DefineDirectionForTrack(10, 6);

            CityMap.DefineDirectionForTrack(15, 9);
            CityMap.DefineDirectionForTrack(14, 34);
            CityMap.DefineDirectionForTrack(13, 9);
            CityMap.DefineDirectionForTrack(16, 10);

            CityMap.DefineDirectionForTrack(22, 13);
            CityMap.DefineDirectionForTrack(21, 35);
            CityMap.DefineDirectionForTrack(19, 13);
            CityMap.DefineDirectionForTrack(20, 14);

            //directionality gora
            CityMap.DefineDirectionForTrack(26, 17);
            CityMap.DefineDirectionForTrack(25, 36);
            CityMap.DefineDirectionForTrack(27, 17);
            CityMap.DefineDirectionForTrack(28, 18);

            CityMap.DefineDirectionForTrack(31, 21);
            CityMap.DefineDirectionForTrack(32, 37);
            CityMap.DefineDirectionForTrack(33, 21);
            CityMap.DefineDirectionForTrack(34, 22);

            CityMap.DefineDirectionForTrack(38, 25);
            CityMap.DefineDirectionForTrack(39, 38);
            CityMap.DefineDirectionForTrack(37, 25);
            CityMap.DefineDirectionForTrack(40, 26);

            CityMap.DefineDirectionForTrack(43, 29);
            CityMap.DefineDirectionForTrack(44, 39);
            CityMap.DefineDirectionForTrack(45, 29);
            CityMap.DefineDirectionForTrack(46, 30);


            CityMap.DefineSpeedForTrack_Km_h(11, speed);

            CityMap.AddStationToCityMap(0, 32, 15, 15);
            CityMap.AddStationToCityMap(1, 33, 15, 15);
            CityMap.AddStationToCityMap(2, 34, 15, 15);
            CityMap.AddStationToCityMap(3, 35, 15, 15);
            CityMap.AddStationToCityMap(4, 36, 15, 15);
            CityMap.AddStationToCityMap(5, 37, 15, 15);
            CityMap.AddStationToCityMap(6, 38, 15, 15);
            CityMap.AddStationToCityMap(7, 39, 15, 15);



            return CityMap;
        }

        public CityDataStorage dwanascie_przystankow_1_0(double speed)
        {
            var CityMap = new CityDataStorage();

            //DC sekcje dolna czesc
            CityMap.AddStraightTrackElementToCityMap(0, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(1, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(2, 100, 270);
            CityMap.AddStraightTrackElementToCityMap(3, 100, 270);
            CityMap.AddStraightTrackElementToCityMap(4, 100, 0);

            CityMap.AddStraightTrackElementToCityMap(6, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(9, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(7, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(8, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(10, 100, 0);

            CityMap.AddStraightTrackElementToCityMap(12, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(13, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(15, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(14, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(16, 100, 0);

            CityMap.AddStraightTrackElementToCityMap(18, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(19, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(22, 100, 270);
            CityMap.AddStraightTrackElementToCityMap(21, 100, 270);
            CityMap.AddStraightTrackElementToCityMap(20, 100, 0);


            //DC sekcje gorna czesc 
            CityMap.AddStraightTrackElementToCityMap(24, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(27, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(26, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(25, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(28, 100, 180);

            CityMap.AddStraightTrackElementToCityMap(30, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(33, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(31, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(32, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(34, 100, 180);


            CityMap.AddStraightTrackElementToCityMap(36, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(37, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(38, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(39, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(40, 100, 180);

            CityMap.AddStraightTrackElementToCityMap(42, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(45, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(43, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(44, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(46, 100, 180);

            //Sort sekcje
            CityMap.AddStraightTrackElementToCityMap(5, 400, 0);
            CityMap.AddStraightRefTrackElementToCityMap(11, 400, 0);
            CityMap.AddStraightTrackElementToCityMap(17, 200, 0);
            CityMap.AddStraightTrackElementToCityMap(48, 200, 0);

            CityMap.AddStraightTrackElementToCityMap(29, 200, 180);
            CityMap.AddStraightTrackElementToCityMap(35, 400, 180);
            CityMap.AddStraightTrackElementToCityMap(41, 400, 180);

            CityMap.AddStraightTrackElementToCityMap(23, 400, 90);
            CityMap.AddStraightTrackElementToCityMap(47, 400, 270);

            CityMap.AddStraightRefTrackElementToCityMap(49, 200, 270);
            CityMap.AddStraightTrackElementToCityMap(50, 200, 270);

            CityMap.AddStraightTrackElementToCityMap(51, 100, 270);
            CityMap.AddStraightTrackElementToCityMap(52, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(53, 100, 270);
            CityMap.AddStraightRefTrackElementToCityMap(54, 300, 270);
            CityMap.AddStraightTrackElementToCityMap(55, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(56, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(57, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(58, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(59, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(60, 150, 0);
            CityMap.AddStraightTrackElementToCityMap(61, 150, 0);
            CityMap.AddStraightTrackElementToCityMap(62, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(63, 71, 45);
            CityMap.AddStraightRefTrackElementToCityMap(64, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(65, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(66, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(67, 150, 0);
            CityMap.AddStraightTrackElementToCityMap(68, 150, 0);
            CityMap.AddStraightTrackElementToCityMap(69, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(70, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(71, 71, 45);
            CityMap.AddStraightRefTrackElementToCityMap(72, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(73, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(74, 300, 90);
            CityMap.AddStraightTrackElementToCityMap(75, 100, 90);
            CityMap.AddStraightTrackElementToCityMap(76, 100, 90);
            CityMap.AddStraightTrackElementToCityMap(77, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(78, 300, 180);
            CityMap.AddStraightTrackElementToCityMap(79, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(80, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(81, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(82, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(83, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(84, 300, 180);
            CityMap.AddStraightTrackElementToCityMap(85, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(86, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(87, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(88, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(89, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(90, 300, 180);
            CityMap.AddStraightTrackElementToCityMap(91, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(92, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(93, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(94, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(95, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(96, 300, 180);
            CityMap.AddStraightTrackElementToCityMap(97, 150, 90);
            CityMap.AddStraightTrackElementToCityMap(98, 150, 90);
            CityMap.AddStraightTrackElementToCityMap(99, 200, 180);


            //nody plus siec polaczen z trackami
            //def polozenia nouda zero
            CityMap.AddNodeToCityMap(0, 0, 47);
            CityMap.define_position_of_node(0, 0, 0);
            CityMap.AddNodeToCityMap(1, 0, 1, 2);
            CityMap.AddNodeToCityMap(2, 1, 4);
            CityMap.AddNodeToCityMap(3, 4, 5);
            CityMap.AddNodeToCityMap(4, 5, 6);
            CityMap.AddNodeToCityMap(5, 6, 7, 9);
            CityMap.AddNodeToCityMap(6, 8, 9, 10);
            CityMap.AddNodeToCityMap(7, 10, 11);
            CityMap.AddNodeToCityMap(8, 11, 12);
            CityMap.AddNodeToCityMap(9, 12, 13, 15);
            CityMap.AddNodeToCityMap(10, 13, 14, 16);
            CityMap.AddNodeToCityMap(11, 16, 17);
            CityMap.AddNodeToCityMap(12, 48, 18);
            CityMap.AddNodeToCityMap(13, 18, 19);
            CityMap.AddNodeToCityMap(14, 19, 20, 21);
            CityMap.AddNodeToCityMap(15, 20, 23);
            CityMap.AddNodeToCityMap(16, 23, 24);
            CityMap.AddNodeToCityMap(17, 24, 27, 26);
            CityMap.AddNodeToCityMap(18, 27, 28, 25);
            CityMap.AddNodeToCityMap(19, 28, 99);
            CityMap.AddNodeToCityMap(20, 29, 30);
            CityMap.AddNodeToCityMap(21, 30, 33, 31);
            CityMap.AddNodeToCityMap(22, 33, 34, 32);
            CityMap.AddNodeToCityMap(23, 34, 35);
            CityMap.AddNodeToCityMap(24, 35, 36);
            CityMap.AddNodeToCityMap(25, 36, 37, 38);
            CityMap.AddNodeToCityMap(26, 37, 40, 39);
            CityMap.AddNodeToCityMap(27, 40, 41);
            CityMap.AddNodeToCityMap(28, 41, 42);
            CityMap.AddNodeToCityMap(29, 42, 45, 43);
            CityMap.AddNodeToCityMap(30, 45, 46, 44);
            CityMap.AddNodeToCityMap(31, 46, 47);
            CityMap.AddNodeToCityMap(32, 2, 3);
            CityMap.AddNodeToCityMap(33, 7, 8);
            CityMap.AddNodeToCityMap(34, 14, 15);
            CityMap.AddNodeToCityMap(35, 21, 22);
            CityMap.AddNodeToCityMap(36, 25, 26);
            CityMap.AddNodeToCityMap(37, 31, 32);
            CityMap.AddNodeToCityMap(38, 38, 39);
            CityMap.AddNodeToCityMap(39, 43, 44);
            CityMap.AddNodeToCityMap(40, 17, 48);
            CityMap.AddNodeToCityMap(41, 3, 49);
            CityMap.AddNodeToCityMap(42, 49, 50);
            CityMap.AddNodeToCityMap(43, 50, 51);
            CityMap.AddNodeToCityMap(44, 51, 52, 53);
            CityMap.AddNodeToCityMap(45, 52, 96);
            CityMap.AddNodeToCityMap(46, 96, 95);
            CityMap.AddNodeToCityMap(47, 95, 92, 94);
            CityMap.AddNodeToCityMap(48, 94, 93);
            CityMap.AddNodeToCityMap(49, 92, 91, 93);
            CityMap.AddNodeToCityMap(50, 91, 90);
            CityMap.AddNodeToCityMap(51, 90, 89);
            CityMap.AddNodeToCityMap(52, 89, 86, 88);
            CityMap.AddNodeToCityMap(53, 88, 87);
            CityMap.AddNodeToCityMap(54, 85, 86, 87);
            CityMap.AddNodeToCityMap(55, 85, 84);
            CityMap.AddNodeToCityMap(56, 84, 83);
            CityMap.AddNodeToCityMap(57, 83, 82, 81);
            CityMap.AddNodeToCityMap(58, 81, 80);
            CityMap.AddNodeToCityMap(59, 82, 80, 79);
            CityMap.AddNodeToCityMap(60, 79, 78);
            CityMap.AddNodeToCityMap(61, 78, 77);
            CityMap.AddNodeToCityMap(62, 77, 76, 75);
            CityMap.AddNodeToCityMap(63, 75, 74);
            CityMap.AddNodeToCityMap(64, 70, 73, 72);
            CityMap.AddNodeToCityMap(65, 70, 69, 71);
            CityMap.AddNodeToCityMap(66, 72, 71);
            CityMap.AddNodeToCityMap(67, 69, 68);
            CityMap.AddNodeToCityMap(68, 67, 68);
            CityMap.AddNodeToCityMap(69, 66, 67);
            CityMap.AddNodeToCityMap(70, 66, 65, 64);
            CityMap.AddNodeToCityMap(71, 63, 64);
            CityMap.AddNodeToCityMap(72, 63, 65, 62);
            CityMap.AddNodeToCityMap(73, 61, 62);
            CityMap.AddNodeToCityMap(74, 60, 61);
            CityMap.AddNodeToCityMap(75, 59, 60);
            CityMap.AddNodeToCityMap(76, 58, 59, 57);
            CityMap.AddNodeToCityMap(77, 58, 56);
            CityMap.AddNodeToCityMap(78, 56, 55, 57);
            CityMap.AddNodeToCityMap(79, 55, 54);
            CityMap.AddNodeToCityMap(80, 53, 54);
            CityMap.AddNodeToCityMap(81, 97, 76);
            CityMap.AddNodeToCityMap(82, 97, 98);
            CityMap.AddNodeToCityMap(83, 22, 98);
            CityMap.AddNodeToCityMap(84, 29, 99);
            CityMap.AddNodeToCityMap(85, 73, 74);



            //directionality dol
            CityMap.DefineDirectionForTrack(0, 0);
            CityMap.DefineDirectionForTrack(2, 1);
            CityMap.DefineDirectionForTrack(1, 1);
            CityMap.DefineDirectionForTrack(3, 32);
            CityMap.DefineDirectionForTrack(4, 2);


            CityMap.DefineDirectionForTrack(7, 5);
            CityMap.DefineDirectionForTrack(9, 5);
            CityMap.DefineDirectionForTrack(8, 33);
            CityMap.DefineDirectionForTrack(10, 6);

            CityMap.DefineDirectionForTrack(15, 9);
            CityMap.DefineDirectionForTrack(14, 34);
            CityMap.DefineDirectionForTrack(13, 9);
            CityMap.DefineDirectionForTrack(16, 10);

            CityMap.DefineDirectionForTrack(22, 83);
            CityMap.DefineDirectionForTrack(21, 35);
            CityMap.DefineDirectionForTrack(19, 13);
            CityMap.DefineDirectionForTrack(20, 14);

            //directionality gora
            CityMap.DefineDirectionForTrack(26, 17);
            CityMap.DefineDirectionForTrack(25, 36);
            CityMap.DefineDirectionForTrack(27, 17);
            CityMap.DefineDirectionForTrack(28, 18);

            CityMap.DefineDirectionForTrack(31, 21);
            CityMap.DefineDirectionForTrack(32, 37);
            CityMap.DefineDirectionForTrack(33, 21);
            CityMap.DefineDirectionForTrack(34, 22);

            CityMap.DefineDirectionForTrack(38, 25);
            CityMap.DefineDirectionForTrack(39, 38);
            CityMap.DefineDirectionForTrack(37, 25);
            CityMap.DefineDirectionForTrack(40, 26);

            CityMap.DefineDirectionForTrack(43, 29);
            CityMap.DefineDirectionForTrack(44, 39);
            CityMap.DefineDirectionForTrack(45, 29);
            CityMap.DefineDirectionForTrack(46, 30);

            CityMap.DefineDirectionForTrack(95, 47);
            CityMap.DefineDirectionForTrack(89, 52);
            CityMap.DefineDirectionForTrack(83, 57);
            CityMap.DefineDirectionForTrack(77, 62);


            CityMap.DefineDirectionForTrack(80, 59);
            CityMap.DefineDirectionForTrack(81, 58);
            CityMap.DefineDirectionForTrack(82, 59);
            CityMap.DefineDirectionForTrack(87, 54);
            CityMap.DefineDirectionForTrack(88, 53);
            CityMap.DefineDirectionForTrack(86, 54);
            CityMap.DefineDirectionForTrack(93, 49);
            CityMap.DefineDirectionForTrack(94, 48);
            CityMap.DefineDirectionForTrack(92, 49);

            CityMap.DefineDirectionForTrack(52, 45);


            CityMap.DefineDirectionForTrack(53, 44);
            CityMap.DefineDirectionForTrack(51, 43);
            CityMap.DefineDirectionForTrack(55, 79);
            CityMap.DefineDirectionForTrack(56, 78);
            CityMap.DefineDirectionForTrack(57, 78);
            CityMap.DefineDirectionForTrack(58, 77);
            CityMap.DefineDirectionForTrack(59, 76);
            CityMap.DefineDirectionForTrack(63, 72);
            CityMap.DefineDirectionForTrack(64, 71);
            CityMap.DefineDirectionForTrack(65, 72);
            CityMap.DefineDirectionForTrack(67, 69);
            CityMap.DefineDirectionForTrack(71, 65);
            CityMap.DefineDirectionForTrack(72, 66);
            CityMap.DefineDirectionForTrack(70, 65);
            CityMap.DefineDirectionForTrack(73, 64);







            CityMap.DefineSpeedForTrack_Km_h(11, speed);

            CityMap.AddStationToCityMap(0, 39, 15, 15);
            CityMap.AddStationToCityMap(1, 38, 15, 15);
            CityMap.AddStationToCityMap(2, 37, 15, 15);
            CityMap.AddStationToCityMap(3, 36, 15, 15);
            CityMap.AddStationToCityMap(4, 34, 15, 15);
            CityMap.AddStationToCityMap(5, 33, 15, 15);
            CityMap.AddStationToCityMap(6, 48, 15, 15);
            CityMap.AddStationToCityMap(7, 53, 15, 15);
            CityMap.AddStationToCityMap(8, 58, 15, 15);
            CityMap.AddStationToCityMap(9, 77, 15, 15);
            CityMap.AddStationToCityMap(10, 71, 15, 15);
            CityMap.AddStationToCityMap(11, 66, 15, 15);



            return CityMap;
        }

        public CityDataStorage two_rings_v_1_0(double speed)
        {
            var CityMap = new CityDataStorage();

            //DC sekcje dolna czesc
            CityMap.AddStraightTrackElementToCityMap(3, 100, 0);
            CityMap.AddStraightTrackElementToCityMap(1, 100, 0);
            CityMap.AddStraightRefTrackElementToCityMap(0, 71, -45);
            CityMap.AddStraightTrackElementToCityMap(2, 71, 45);
            CityMap.AddStraightTrackElementToCityMap(19, 100, 0);



            //DC sekcje gorna czesc 
            CityMap.AddStraightTrackElementToCityMap(11, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(14, 100, 180);
            CityMap.AddStraightRefTrackElementToCityMap(12, 71, 135);
            CityMap.AddStraightTrackElementToCityMap(13, 71, 225);
            CityMap.AddStraightTrackElementToCityMap(15, 100, 180);

            //DC z rozgalezieniem prawa strona
            CityMap.AddStraightTrackElementToCityMap(6, 100, 90);
            CityMap.AddStraightTrackElementToCityMap(9, 200, 180);

            //DC z rozgalezieniem lewa strona
            CityMap.AddStraightTrackElementToCityMap(17, 100, 180);
            CityMap.AddStraightTrackElementToCityMap(18, 100, 270);


            //Sort sekcje
            CityMap.AddStraightTrackElementToCityMap(8, 200, 0);
            CityMap.AddStraightRefTrackElementToCityMap(7, 200, 0);

            CityMap.AddStraightTrackElementToCityMap(5, 200, 0);
            CityMap.AddStraightTrackElementToCityMap(4, 200, 0);


            CityMap.AddStraightTrackElementToCityMap(10, 200, 180);
            CityMap.AddStraightTrackElementToCityMap(16, 300, 180);

            CityMap.AddStraightTrackElementToCityMap(23, 400, 90);
            CityMap.AddStraightTrackElementToCityMap(47, 400, 270);



            //nody plus siec polaczen z trackami
            //def polozenia nouda zero
            CityMap.AddNodeToCityMap(0, 18, 5);
            CityMap.define_position_of_node(0, 0, 0);
            CityMap.AddNodeToCityMap(1, 5, 4);
            CityMap.AddNodeToCityMap(2, 4, 3);
            CityMap.AddNodeToCityMap(3, 3, 1, 0);
            CityMap.AddNodeToCityMap(4, 1, 19, 2);
            CityMap.AddNodeToCityMap(5, 0, 2);
            CityMap.AddNodeToCityMap(6, 19, 8);
            CityMap.AddNodeToCityMap(7, 8, 7);
            CityMap.AddNodeToCityMap(8, 7, 6);
            CityMap.AddNodeToCityMap(9, 6, 9);
            CityMap.AddNodeToCityMap(10, 9, 10);
            CityMap.AddNodeToCityMap(11, 10, 11);
            CityMap.AddNodeToCityMap(12, 12, 13);
            CityMap.AddNodeToCityMap(13, 11, 14, 12);
            CityMap.AddNodeToCityMap(14, 14, 13, 15);
            CityMap.AddNodeToCityMap(15, 15, 16);
            CityMap.AddNodeToCityMap(16, 16, 17);
            CityMap.AddNodeToCityMap(17, 17, 18);





            //directionality dol
            CityMap.DefineDirectionForTrack(0, 3);
            CityMap.DefineDirectionForTrack(2, 5);
            CityMap.DefineDirectionForTrack(1, 3);
            CityMap.DefineDirectionForTrack(19, 4);
            CityMap.DefineDirectionForTrack(3, 2);

            //directionality gora
            CityMap.DefineDirectionForTrack(15, 14);
            CityMap.DefineDirectionForTrack(12, 13);
            CityMap.DefineDirectionForTrack(13, 12);
            CityMap.DefineDirectionForTrack(14, 13);

            //prawy DC rozgalezienie
            CityMap.DefineDirectionForTrack(6, 8);
            CityMap.DefineDirectionForTrack(9, 9);

            //lewy DC rozgalezienie
            CityMap.DefineDirectionForTrack(17, 16);
            CityMap.DefineDirectionForTrack(18, 17);

            CityMap.DefineSpeedForTrack_Km_h(8, speed);

            CityMap.AddStationToCityMap(0, 5, 15, 15);
            CityMap.AddStationToCityMap(1, 12, 15, 15);



            return CityMap;
        }


        //public CityDataStorage Testowe()
        //{
        //    var CityMap = new CityDataStorage();

        //    CityMap.AddTrackElementToCityMap(0,25);       //0
        //    CityMap.AddTrackElementToCityMap(1,25);       //1
        //    CityMap.AddTrackElementToCityMap(2,25);       //2
        //    CityMap.AddTrackElementToCityMap(3,25);       //3
        //    CityMap.AddTrackElementToCityMap(4,25);       //4
        //    CityMap.AddTrackElementToCityMap(5,50);       //5
        //    CityMap.AddTrackElementToCityMap(6,50);       //6
        //    CityMap.AddTrackElementToCityMap(7,25);       //7
        //    CityMap.AddTrackElementToCityMap(8,25);       //8
        //    CityMap.AddTrackElementToCityMap(9,50);       //9
        //    CityMap.AddTrackElementToCityMap(10,25);      //10
        //    CityMap.AddTrackElementToCityMap(11,25);      //11
        //    CityMap.AddTrackElementToCityMap(12,25);      //12
        //    CityMap.AddTrackElementToCityMap(13,25);      //13
        //    CityMap.AddTrackElementToCityMap(14,25);      //14


        //    CityMap.AddNodeToCityMap(0,10,0);

        //    CityMap.AddNodeToCityMap(1,0,1,11);

        //    CityMap.AddNodeToCityMap(2,1, 2, 13);

        //    CityMap.AddNodeToCityMap(3, 3, 14);

        //    CityMap.AddNodeToCityMap(4,3, 4);

        //    CityMap.AddNodeToCityMap(5,4,5);

        //    CityMap.AddNodeToCityMap(6,6,7);

        //    CityMap.AddNodeToCityMap(7,7,8);

        //    CityMap.AddNodeToCityMap(8,8,9,5,14);

        //    CityMap.AddNodeToCityMap(9,9, 10);

        //    CityMap.AddNodeToCityMap(10,11,12);

        //    CityMap.AddNodeToCityMap(11,12,13);
        //    CityMap.AddNodeToCityMap(12, 2, 6);


        //    CityMap.DefineDirectionForTrack(0, 0);
        //    CityMap.DefineDirectionForTrack(3, 3);
        //    CityMap.DefineDirectionForTrack(7, 6);
        //    CityMap.DefineDirectionForTrack(12, 10);
        //    CityMap.DefineDirectionForTrack(1, 1);
        //    CityMap.DefineDirectionForTrack(2, 2);

        //    CityMap.DefineSpeedForTrack_Km_h(0, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(1, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(2, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(3, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(4, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(5, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(9, 80);
        //    CityMap.DefineSpeedForTrack_Km_h(10, 110);
        //    CityMap.DefineSpeedForTrack_Km_h(11, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(13, 120);


        //    CityMap.AddStationToCityMap(0,0);
        //    CityMap.AddStationToCityMap(1,4);
        //    CityMap.AddStationToCityMap(2,7);
        //    CityMap.AddStationToCityMap(3,6);

        //    return CityMap;
        //}
        //public CityDataStorage Czernie_Vice()
        //{
        //    var CityMap = new CityDataStorage();

        //    for (int i = 0; i <= 62; i++)
        //    {
        //        CityMap.AddTrackElementToCityMap(i, 250);
        //    }

        //    CityMap.AddTrackElementToCityMap(63, 300);
        //    CityMap.AddTrackElementToCityMap(64, 30);

        //    //CityMap.AddTrackElementToCityMap(65, 30);
        //    //CityMap.AddTrackElementToCityMap(66, 30);
        //    //CityMap.AddTrackElementToCityMap(67, 30);


        //    CityMap.AddNodeToCityMap(0,16,1);
        //    CityMap.AddNodeToCityMap(1,1,2 );
        //    CityMap.AddNodeToCityMap(2,2,3 );
        //    CityMap.AddNodeToCityMap(3,3,4 );
        //    CityMap.AddNodeToCityMap(4,5,17,4);
        //    CityMap.AddNodeToCityMap(5,5,6);
        //    CityMap.AddNodeToCityMap(6,6,7);
        //    CityMap.AddNodeToCityMap(7,7,8 );
        //    CityMap.AddNodeToCityMap(8,8,9,60 );
        //    CityMap.AddNodeToCityMap(9,60,59);
        //    CityMap.AddNodeToCityMap(10,9,10 );
        //    CityMap.AddNodeToCityMap(11, 10,11);
        //    CityMap.AddNodeToCityMap(12,11,12 );
        //    CityMap.AddNodeToCityMap(13,12,13 );
        //    CityMap.AddNodeToCityMap(14,13,14 );
        //    CityMap.AddNodeToCityMap(15,14,15 );
        //    CityMap.AddNodeToCityMap(16,15,16 );
        //    CityMap.AddNodeToCityMap(17,17,18 );
        //    CityMap.AddNodeToCityMap(18,18,19,32 );
        //    CityMap.AddNodeToCityMap(19,31,32 );
        //    CityMap.AddNodeToCityMap(20,31,30 );
        //    CityMap.AddNodeToCityMap(21,29,30 );
        //    CityMap.AddNodeToCityMap(22,28,29 );
        //    CityMap.AddNodeToCityMap(23,27,28 );
        //    CityMap.AddNodeToCityMap(24,26,27 );
        //    CityMap.AddNodeToCityMap(25,25,26 );
        //    CityMap.AddNodeToCityMap(26,24,25 );
        //    CityMap.AddNodeToCityMap(27,23,24 );
        //    CityMap.AddNodeToCityMap(28,22,23,33 );
        //    CityMap.AddNodeToCityMap(29,21,22);
        //    CityMap.AddNodeToCityMap(30,20,21 );
        //    CityMap.AddNodeToCityMap(31,19,20 );
        //    CityMap.AddNodeToCityMap(32,33,34);
        //    CityMap.AddNodeToCityMap(33,34,35 );
        //    CityMap.AddNodeToCityMap(34,35,36 );
        //    CityMap.AddNodeToCityMap(35,36,37 );
        //    CityMap.AddNodeToCityMap(36, 37,38);
        //    CityMap.AddNodeToCityMap(37, 38,39);
        //    CityMap.AddNodeToCityMap(38,39,40,46 );
        //    CityMap.AddNodeToCityMap(39,0,40,41 );
        //    CityMap.AddNodeToCityMap(40,41,42,64 );
        //    CityMap.AddNodeToCityMap(41,42,43 );
        //    CityMap.AddNodeToCityMap(42,43,44 );
        //    CityMap.AddNodeToCityMap(43,44,52 );
        //    CityMap.AddNodeToCityMap(44,52,51 );
        //    CityMap.AddNodeToCityMap(45,51,50 );
        //    CityMap.AddNodeToCityMap(46,50,49 );
        //    CityMap.AddNodeToCityMap(47,48,49,53 );
        //    CityMap.AddNodeToCityMap(48,47,48 );
        //    CityMap.AddNodeToCityMap(49,45,47 );
        //    CityMap.AddNodeToCityMap(50,45,46 );
        //    CityMap.AddNodeToCityMap(51,53,54 );
        //    CityMap.AddNodeToCityMap(52,54,55 );
        //    CityMap.AddNodeToCityMap(53,55,56 );
        //    CityMap.AddNodeToCityMap(54,56,57 );
        //    CityMap.AddNodeToCityMap(55,57,58 );
        //    CityMap.AddNodeToCityMap(56,58,59 );
        //    CityMap.AddNodeToCityMap(57,0,61 );
        //    CityMap.AddNodeToCityMap(58,61,62 );
        //    CityMap.AddNodeToCityMap(59,62,63 );
        //    CityMap.AddNodeToCityMap(60,63,64 );


        //    CityMap.DefineDirectionForTrack(14, 14);
        //    //CityMap.DefineDirectionForTrack(65, 5);
        //    CityMap.DefineDirectionForTrack(7, 7);
        //    CityMap.DefineDirectionForTrack(6, 6);
        //    CityMap.DefineDirectionForTrack(55, 52);
        //    CityMap.DefineDirectionForTrack(17, 4);
        //    CityMap.DefineDirectionForTrack(28, 22);
        //    CityMap.DefineDirectionForTrack(20, 31);
        //    CityMap.DefineDirectionForTrack(34, 32);
        //    CityMap.DefineDirectionForTrack(47, 49);
        //    CityMap.DefineDirectionForTrack(44, 42);
        //    CityMap.DefineDirectionForTrack(62, 58);
        //    CityMap.DefineDirectionForTrack(41, 39);
        //    CityMap.DefineDirectionForTrack(40, 38);
        //    CityMap.DefineDirectionForTrack(48, 48);
        //    CityMap.DefineDirectionForTrack(5, 5);
        //    //CityMap.DefineDirectionForTrack(66, 31);
        //    CityMap.DefineDirectionForTrack(19, 18);
        //    CityMap.DefineDirectionForTrack(39, 37);
        //    //CityMap.DefineDirectionForTrack(67, 47);

        //    CityMap.DefineSpeedForTrack_Km_h(45, 120);
        //    CityMap.DefineSpeedForTrack_Km_h(26, 100);
        //    CityMap.DefineSpeedForTrack_Km_h(50, 100);
        //    CityMap.DefineSpeedForTrack_Km_h(31, 100);
        //    CityMap.DefineSpeedForTrack_Km_h(57, 100);
        //    CityMap.DefineSpeedForTrack_Km_h(10, 100);
        //    //CityMap.DefineSpeedForTrack_Km_h(39, 100);
        //    //CityMap.DefineSpeedForTrack_Km_h(34, 120);
        //    //CityMap.DefineSpeedForTrack_Km_h(4, 45);
        //    //CityMap.DefineSpeedForTrack_Km_h(32, 50);
        //    //CityMap.DefineSpeedForTrack_Km_h(23, 50);
        //    //CityMap.DefineSpeedForTrack_Km_h(0, 20);
        //    //CityMap.DefineSpeedForTrack_Km_h(40, 60);
        //    //CityMap.DefineSpeedForTrack_Km_h(42, 60);
        //    //CityMap.DefineSpeedForTrack_Km_h(49, 60);


        //    //CityMap.DefineSpeedForTrack_Km_h(63, 60);
        //    //CityMap.DefineSpeedForTrack_Km_h(64, 66);

        //    //CityMap.DefineSpeedForTrack_Km_h(65, 40);
        //    //CityMap.DefineSpeedForTrack_Km_h(66, 40);
        //    //CityMap.DefineSpeedForTrack_Km_h(67, 40);

        //    //CityMap.DefineSpeedForTrack_Km_h(33, 30);
        //    //CityMap.DefineSpeedForTrack_Km_h(38, 33);
        //    //CityMap.DefineSpeedForTrack_Km_h(37, 120);


        //    CityMap.AddStationToCityMap(0, 23);
        //    CityMap.AddStationToCityMap(1, 15);
        //    CityMap.AddStationToCityMap(2, 43);
        //    CityMap.AddStationToCityMap(3, 59);
        //    CityMap.AddStationToCityMap(4, 45);


        //    return CityMap;
        //}
        //public CityDataStorage Rozgalezienie()
        //{
        //    var CityMap = new CityDataStorage();

        //    for (int i = 0; i <= 44; i++)
        //    {
        //        CityMap.AddTrackElementToCityMap(i, 400);
        //    }

        //    //CityMap.AddTrackElementToCityMap(63, 300);
        //    //CityMap.AddTrackElementToCityMap(64, 30);

        //    //CityMap.AddTrackElementToCityMap(65, 30);
        //    //CityMap.AddTrackElementToCityMap(66, 30);
        //    //CityMap.AddTrackElementToCityMap(67, 30);


        //    CityMap.AddNodeToCityMap(0, 0, 33, 34);
        //    CityMap.AddNodeToCityMap(1, 0, 26, 1);
        //    CityMap.AddNodeToCityMap(2, 1, 2, 8);
        //    CityMap.AddNodeToCityMap(3,2,7,3);
        //    CityMap.AddNodeToCityMap(4,3,4,6);
        //    CityMap.AddNodeToCityMap(5,4,5);
        //    CityMap.AddNodeToCityMap(6,5,16);
        //    CityMap.AddNodeToCityMap(7,16,17);
        //    CityMap.AddNodeToCityMap(8,15,17,18);
        //    CityMap.AddNodeToCityMap(9,15,6);
        //    CityMap.AddNodeToCityMap(10,7,13,12);
        //    CityMap.AddNodeToCityMap(11,13,14);
        //    CityMap.AddNodeToCityMap(12,11,12);
        //    CityMap.AddNodeToCityMap(13,18,14,19);
        //    CityMap.AddNodeToCityMap(14,19,11,20);
        //    CityMap.AddNodeToCityMap(15,10,20,21);
        //    CityMap.AddNodeToCityMap(16,9,10);
        //    CityMap.AddNodeToCityMap(17,8,9);
        //    CityMap.AddNodeToCityMap(18,26,24,27);
        //    CityMap.AddNodeToCityMap(19,24,23);
        //    CityMap.AddNodeToCityMap(20,22,23,25);
        //    CityMap.AddNodeToCityMap(21,25,29);
        //    CityMap.AddNodeToCityMap(22,30,28,29);
        //    CityMap.AddNodeToCityMap(23,28,27);
        //    CityMap.AddNodeToCityMap(24,31,32,30);
        //    CityMap.AddNodeToCityMap(25,31,33);
        //    CityMap.AddNodeToCityMap(26,32,40);
        //    CityMap.AddNodeToCityMap(27,21,22);
        //    CityMap.AddNodeToCityMap(28,39,40,41);
        //    CityMap.AddNodeToCityMap(29,41,42);
        //    CityMap.AddNodeToCityMap(30,38,39,44);
        //    CityMap.AddNodeToCityMap(31,44,43);
        //    CityMap.AddNodeToCityMap(32,42,34,35);
        //    CityMap.AddNodeToCityMap(33,35,36,43);
        //    CityMap.AddNodeToCityMap(34,36,37);
        //    CityMap.AddNodeToCityMap(35,37,38);


        //    CityMap.DefineDirectionForTrack(0, 0);
        //    CityMap.DefineDirectionForTrack(1, 1);
        //    CityMap.DefineDirectionForTrack(2, 2);
        //    CityMap.DefineDirectionForTrack(3,3);
        //    CityMap.DefineDirectionForTrack(4,4);
        //    CityMap.DefineDirectionForTrack(5,5);
        //    CityMap.DefineDirectionForTrack(15,8);
        //    CityMap.DefineDirectionForTrack(13,11);
        //    CityMap.DefineDirectionForTrack(7,10);
        //    CityMap.DefineDirectionForTrack(12,12);
        //    CityMap.DefineDirectionForTrack(9,16);
        //    CityMap.DefineDirectionForTrack(18,8);
        //    CityMap.DefineDirectionForTrack(19,13);
        //    CityMap.DefineDirectionForTrack(20,14);
        //    CityMap.DefineDirectionForTrack(21,15);
        //    CityMap.DefineDirectionForTrack(23,20);
        //    CityMap.DefineDirectionForTrack(25,20);
        //    CityMap.DefineDirectionForTrack(28,22);
        //    CityMap.DefineDirectionForTrack(26,18);
        //    CityMap.DefineDirectionForTrack(30,22);
        //    CityMap.DefineDirectionForTrack(31,24);
        //    CityMap.DefineDirectionForTrack(34,0);
        //    CityMap.DefineDirectionForTrack(35,32);
        //    CityMap.DefineDirectionForTrack(36,33);
        //    CityMap.DefineDirectionForTrack(44,30);
        //    CityMap.DefineDirectionForTrack(39,30);
        //    CityMap.DefineDirectionForTrack(41,28);
        //    CityMap.DefineDirectionForTrack(40,28);

        //    CityMap.DefineSpeedForTrack_Km_h(1, 120);

        //    CityMap.DefineSpeedForTrack_Km_h(2, 84);

        //    CityMap.DefineSpeedForTrack_Km_h(27, 97);
        //    CityMap.DefineSpeedForTrack_Km_h(26, 77);

        //    CityMap.DefineSpeedForTrack_Km_h(9, 109);
        //    CityMap.DefineSpeedForTrack_Km_h(8, 102);

        //    CityMap.DefineSpeedForTrack_Km_h(12, 87);
        //    CityMap.DefineSpeedForTrack_Km_h(13, 92);

        //    CityMap.DefineSpeedForTrack_Km_h(7, 49);

        //    CityMap.DefineSpeedForTrack_Km_h(6, 110);



        //    CityMap.AddStationToCityMap(0, 17, 20);
        //    CityMap.AddStationToCityMap(1, 6, 15);
        //    CityMap.AddStationToCityMap(2, 35);
        //    CityMap.AddStationToCityMap(3, 12);
        //    CityMap.AddStationToCityMap(4, 21);

        //    //tu dodaje metode ktora zrobi przy stacjach kawalki z predkoscia jakies 5 km/h

        //    return CityMap;
        //}

        //public CityDataStorage Rozgalezienie_stacja()
        //{
        //    var CityMap = new CityDataStorage();

        //    for (int i = 0; i <= 48; i++)
        //    {
        //        CityMap.AddTrackElementToCityMap(i, 400);
        //    }

        //    //CityMap.AddTrackElementToCityMap(63, 300);
        //    //CityMap.AddTrackElementToCityMap(64, 30);

        //    //CityMap.AddTrackElementToCityMap(65, 30);
        //    //CityMap.AddTrackElementToCityMap(66, 30);
        //    //CityMap.AddTrackElementToCityMap(67, 30);


        //    CityMap.AddNodeToCityMap(0, 0, 33, 34);
        //    CityMap.AddNodeToCityMap(1, 0, 26, 1);
        //    CityMap.AddNodeToCityMap(2, 48, 2, 8);
        //    CityMap.AddNodeToCityMap(3, 2, 7, 3);
        //    CityMap.AddNodeToCityMap(4, 3, 4, 6);
        //    CityMap.AddNodeToCityMap(5, 4, 5);
        //    CityMap.AddNodeToCityMap(6, 5, 16);
        //    CityMap.AddNodeToCityMap(7, 16, 17);
        //    CityMap.AddNodeToCityMap(8, 15, 17, 18);
        //    CityMap.AddNodeToCityMap(9, 15, 6);
        //    CityMap.AddNodeToCityMap(10, 7, 13, 12);
        //    CityMap.AddNodeToCityMap(11, 13, 14);
        //    CityMap.AddNodeToCityMap(12, 11, 12);
        //    CityMap.AddNodeToCityMap(13, 18, 14, 19);
        //    CityMap.AddNodeToCityMap(14, 19, 11, 20);
        //    CityMap.AddNodeToCityMap(15, 47, 20, 21);
        //    CityMap.AddNodeToCityMap(16, 9, 10, 46);
        //    CityMap.AddNodeToCityMap(17, 8, 9, 45);
        //    CityMap.AddNodeToCityMap(18, 26, 24, 27);
        //    CityMap.AddNodeToCityMap(19, 24, 23);
        //    CityMap.AddNodeToCityMap(20, 22, 23, 25);
        //    CityMap.AddNodeToCityMap(21, 25, 29);
        //    CityMap.AddNodeToCityMap(22, 30, 28, 29);
        //    CityMap.AddNodeToCityMap(23, 28, 27);
        //    CityMap.AddNodeToCityMap(24, 31, 32, 30);
        //    CityMap.AddNodeToCityMap(25, 31, 33);
        //    CityMap.AddNodeToCityMap(26, 32, 40);
        //    CityMap.AddNodeToCityMap(27, 21, 22);
        //    CityMap.AddNodeToCityMap(28, 39, 40, 41);
        //    CityMap.AddNodeToCityMap(29, 41, 42);
        //    CityMap.AddNodeToCityMap(30, 38, 39, 44);
        //    CityMap.AddNodeToCityMap(31, 44, 43);
        //    CityMap.AddNodeToCityMap(32, 42, 34, 35);
        //    CityMap.AddNodeToCityMap(33, 35, 36, 43);
        //    CityMap.AddNodeToCityMap(34, 36, 37);
        //    CityMap.AddNodeToCityMap(35, 37, 38);
        //    CityMap.AddNodeToCityMap(36, 45, 46);
        //    CityMap.AddNodeToCityMap(37, 10, 47);
        //    CityMap.AddNodeToCityMap(38, 1, 48);


        //    CityMap.DefineDirectionForTrack(0, 0);
        //    CityMap.DefineDirectionForTrack(1, 1);
        //    CityMap.DefineDirectionForTrack(2, 2);
        //    CityMap.DefineDirectionForTrack(3, 3);
        //    CityMap.DefineDirectionForTrack(4, 4);
        //    CityMap.DefineDirectionForTrack(5, 5);
        //    CityMap.DefineDirectionForTrack(15, 8);
        //    CityMap.DefineDirectionForTrack(13, 11);
        //    CityMap.DefineDirectionForTrack(7, 10);
        //    CityMap.DefineDirectionForTrack(12, 12);
        //    CityMap.DefineDirectionForTrack(9, 16);
        //    CityMap.DefineDirectionForTrack(8, 17);
        //    CityMap.DefineDirectionForTrack(10, 37);
        //    CityMap.DefineDirectionForTrack(18, 8);
        //    CityMap.DefineDirectionForTrack(19, 13);
        //    CityMap.DefineDirectionForTrack(20, 14);
        //    CityMap.DefineDirectionForTrack(21, 15);
        //    CityMap.DefineDirectionForTrack(23, 20);
        //    CityMap.DefineDirectionForTrack(25, 20);
        //    CityMap.DefineDirectionForTrack(28, 22);
        //    CityMap.DefineDirectionForTrack(26, 18);
        //    CityMap.DefineDirectionForTrack(30, 22);
        //    CityMap.DefineDirectionForTrack(31, 24);
        //    CityMap.DefineDirectionForTrack(34, 0);
        //    CityMap.DefineDirectionForTrack(35, 32);
        //    CityMap.DefineDirectionForTrack(36, 33);
        //    CityMap.DefineDirectionForTrack(44, 30);
        //    CityMap.DefineDirectionForTrack(39, 30);
        //    CityMap.DefineDirectionForTrack(41, 28);
        //    CityMap.DefineDirectionForTrack(40, 28);
        //    CityMap.DefineDirectionForTrack(46, 16);
        //    CityMap.DefineDirectionForTrack(45, 36);

        //    CityMap.DefineSpeedForTrack_Km_h(1, 120);

        //    CityMap.DefineSpeedForTrack_Km_h(2, 84);

        //    CityMap.DefineSpeedForTrack_Km_h(45, 67);
        //    //CityMap.DefineSpeedForTrack_Km_h(26, 77);

        //    CityMap.DefineSpeedForTrack_Km_h(9, 109);
        //    //CityMap.DefineSpeedForTrack_Km_h(8, 102);

        //    //CityMap.DefineSpeedForTrack_Km_h(12, 87);
        //    //CityMap.DefineSpeedForTrack_Km_h(13, 92);

        //    CityMap.DefineSpeedForTrack_Km_h(7, 49);

        //    //CityMap.DefineSpeedForTrack_Km_h(6, 110);



        //    CityMap.AddStationToCityMap(0, 17, 20, 10);
        //    CityMap.AddStationToCityMap(1, 6, 15, 10);
        //    CityMap.AddStationToCityMap(2, 35, 10, 10);
        //    CityMap.AddStationToCityMap(3, 12, 10, 10);
        //    CityMap.AddStationToCityMap(4, 21, 10, 10);
        //    CityMap.AddStationToCityMap(5, 36, 15, 15);

        //    //tu dodaje metode ktora zrobi przy stacjach kawalki z predkoscia jakies 5 km/h

        //    return CityMap;
        //}

        public (CityDataStorage city, List<map_module> list_of_city_modules) MiastoModulowe_v_1()
        {
            var interpreter = new Interpreter_of_map_definition();
            var modules_connections = new List<external_connection>();
            var modules_repository = new repositoty_of_modules();



            var lista_modulow = modules_repository.miasto_modulowe_1();



            //module number - track number <-> module number - track number
            modules_connections.Add(new external_connection(1, 5, 2, 1));
            modules_connections.Add(new external_connection(2, 3, 3, 1));
            modules_connections.Add(new external_connection(3, 5, 4, 1));
            modules_connections.Add(new external_connection(4, 3, 5, 1));
            modules_connections.Add(new external_connection(5, 5, 6, 1));
            modules_connections.Add(new external_connection(6, 3, 7, 1));
            modules_connections.Add(new external_connection(7, 5, 8, 1));
            modules_connections.Add(new external_connection(8, 3, 9, 1));
            modules_connections.Add(new external_connection(9, 5, 10, 1));
            modules_connections.Add(new external_connection(10, 3, 11, 1));
            modules_connections.Add(new external_connection(11, 5, 12, 1));
            modules_connections.Add(new external_connection(12, 3, 13, 1));
            modules_connections.Add(new external_connection(13, 5, 14, 1));
            modules_connections.Add(new external_connection(14, 3, 15, 1));
            modules_connections.Add(new external_connection(15, 5, 16, 1));
            modules_connections.Add(new external_connection(16, 3, 17, 1));
            modules_connections.Add(new external_connection(17, 5, 18, 1));
            modules_connections.Add(new external_connection(18, 3, 19, 1));



            modules_connections.Add(new external_connection(19, 5, 20, 1));
            modules_connections.Add(new external_connection(20, 3, 21, 5));

            modules_connections.Add(new external_connection(21, 1, 22, 3));
            modules_connections.Add(new external_connection(22, 1, 23, 5));
            modules_connections.Add(new external_connection(23, 1, 24, 3));
            modules_connections.Add(new external_connection(24, 1, 25, 5));
            modules_connections.Add(new external_connection(25, 1, 26, 3));
            modules_connections.Add(new external_connection(26, 1, 27, 5));
            modules_connections.Add(new external_connection(27, 1, 28, 3));
            modules_connections.Add(new external_connection(28, 1, 29, 5));
            modules_connections.Add(new external_connection(29, 1, 30, 3));
            modules_connections.Add(new external_connection(30, 1, 31, 5));
            modules_connections.Add(new external_connection(31, 1, 32, 3));
            modules_connections.Add(new external_connection(32, 1, 33, 5));
            modules_connections.Add(new external_connection(33, 1, 34, 3));
            modules_connections.Add(new external_connection(34, 1, 35, 5));
            modules_connections.Add(new external_connection(35, 1, 36, 3));
            modules_connections.Add(new external_connection(36, 1, 37, 5));
            modules_connections.Add(new external_connection(37, 1, 38, 3));
            modules_connections.Add(new external_connection(38, 1, 39, 5));




            modules_connections.Add(new external_connection(39, 1, 40, 1));
            modules_connections.Add(new external_connection(40, 3, 1, 1));



            //modules_connections.Add(new external_connection(2, 1, 3, 4));
            //modules_connections.Add(new external_connection(2, 1, 3, 4));



            // translation of map modules def to City Map
            var CityMap = interpreter.translate_modules_def_to_city_map(lista_modulow, modules_connections);


            return (CityMap, lista_modulow);
        }

        public (CityDataStorage city, List<map_module> list_of_city_modules) MiastoModulowe_v_2()
        {
            var interpreter = new Interpreter_of_map_definition();
            var modules_connections = new List<external_connection>();
            var modules_repository = new repositoty_of_modules();



            var lista_modulow = modules_repository.miasto_modulowe_2();



            //module number - track number <-> module number - track number
            modules_connections.Add(new external_connection(1, 5, 2, 1));
            modules_connections.Add(new external_connection(2, 3, 3, 1));
            modules_connections.Add(new external_connection(3, 5, 4, 1));
            modules_connections.Add(new external_connection(4, 3, 5, 1));
            modules_connections.Add(new external_connection(5, 5, 6, 1));
            modules_connections.Add(new external_connection(6, 3, 7, 1));
            modules_connections.Add(new external_connection(7, 5, 8, 1));
            modules_connections.Add(new external_connection(8, 3, 9, 1));
            modules_connections.Add(new external_connection(9, 5, 10, 1));
            modules_connections.Add(new external_connection(10, 3, 11, 1));
            modules_connections.Add(new external_connection(11, 5, 12, 1));
            modules_connections.Add(new external_connection(12, 3, 13, 1));
            modules_connections.Add(new external_connection(13, 5, 14, 1));
            modules_connections.Add(new external_connection(14, 3, 15, 1));
            modules_connections.Add(new external_connection(15, 5, 16, 1));
            modules_connections.Add(new external_connection(16, 3, 17, 1));
            modules_connections.Add(new external_connection(17, 5, 18, 1));
            modules_connections.Add(new external_connection(18, 3, 19, 1));
            modules_connections.Add(new external_connection(19, 5, 20, 1));
            modules_connections.Add(new external_connection(20, 3, 21, 1));
            modules_connections.Add(new external_connection(21, 5, 22, 1));
            modules_connections.Add(new external_connection(22, 3, 23, 1));



            modules_connections.Add(new external_connection(23, 5, 24, 1));
            modules_connections.Add(new external_connection(24, 3, 25, 5));

            modules_connections.Add(new external_connection(25, 1, 26, 3));
            modules_connections.Add(new external_connection(26, 1, 27, 5));
            modules_connections.Add(new external_connection(27, 1, 28, 3));
            modules_connections.Add(new external_connection(28, 1, 29, 5));
            modules_connections.Add(new external_connection(29, 1, 30, 3));
            modules_connections.Add(new external_connection(30, 1, 31, 5));
            modules_connections.Add(new external_connection(31, 1, 32, 3));
            modules_connections.Add(new external_connection(32, 1, 33, 5));
            modules_connections.Add(new external_connection(33, 1, 34, 3));
            modules_connections.Add(new external_connection(34, 1, 35, 5));
            modules_connections.Add(new external_connection(35, 1, 36, 3));
            modules_connections.Add(new external_connection(36, 1, 37, 5));
            modules_connections.Add(new external_connection(37, 1, 38, 3));
            modules_connections.Add(new external_connection(38, 1, 39, 5));
            modules_connections.Add(new external_connection(39, 1, 40, 3));
            modules_connections.Add(new external_connection(40, 1, 41, 5));
            modules_connections.Add(new external_connection(41, 1, 42, 3));
            modules_connections.Add(new external_connection(42, 1, 43, 5));
            modules_connections.Add(new external_connection(43, 1, 44, 3));
            modules_connections.Add(new external_connection(44, 1, 45, 5));
            modules_connections.Add(new external_connection(45, 1, 46, 3));
            modules_connections.Add(new external_connection(46, 1, 47, 5));




            modules_connections.Add(new external_connection(47, 1, 48, 1));
            modules_connections.Add(new external_connection(48, 3, 1, 1));



            //modules_connections.Add(new external_connection(2, 1, 3, 4));
            //modules_connections.Add(new external_connection(2, 1, 3, 4));



            // translation of map modules def to City Map
            var CityMap = interpreter.translate_modules_def_to_city_map(lista_modulow, modules_connections);


            return (CityMap, lista_modulow);
        }
    }

    public class repositoty_of_modules
    {

        public List<map_module> miasto_modulowe_1()
        {
            var lista_modulow = new List<map_module>();

            lista_modulow.Add(DC_with_station_horisontal_left_to_right(1));
            lista_modulow.Add(Sort_horisontal_left_to_right(2));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(3));
            lista_modulow.Add(Sort_horisontal_left_to_right(4));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(5));
            lista_modulow.Add(Sort_horisontal_left_to_right(6));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(7));
            lista_modulow.Add(Sort_horisontal_left_to_right(8));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(9));
            lista_modulow.Add(Sort_horisontal_left_to_right(10));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(11));
            lista_modulow.Add(Sort_horisontal_left_to_right(12));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(13));
            lista_modulow.Add(Sort_horisontal_left_to_right(14));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(15));
            lista_modulow.Add(Sort_horisontal_left_to_right(16));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(17));
            lista_modulow.Add(Sort_horisontal_left_to_right(18));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(19));




            lista_modulow.Add(Sort_vartical_down_to_up(20));

            lista_modulow.Add(DC_with_station_horisontal_right_to_left(21));
            lista_modulow.Add(Sort_horisontal_right_to_left(22));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(23));
            lista_modulow.Add(Sort_horisontal_right_to_left(24));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(25));
            lista_modulow.Add(Sort_horisontal_right_to_left(26));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(27));
            lista_modulow.Add(Sort_horisontal_right_to_left(28));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(29));
            lista_modulow.Add(Sort_horisontal_right_to_left(30));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(31));
            lista_modulow.Add(Sort_horisontal_right_to_left(32));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(33));
            lista_modulow.Add(Sort_horisontal_right_to_left(34));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(35));
            lista_modulow.Add(Sort_horisontal_right_to_left(36));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(37));
            lista_modulow.Add(Sort_horisontal_right_to_left(38));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(39));



            lista_modulow.Add(Sort_vartical_up_to_down(40));


            return lista_modulow;
        }

        public List<map_module> miasto_modulowe_2()
        {
            var lista_modulow = new List<map_module>();

            lista_modulow.Add(DC_with_station_horisontal_left_to_right(1));
            lista_modulow.Add(Sort_horisontal_left_to_right(2));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(3));
            lista_modulow.Add(Sort_horisontal_left_to_right(4));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(5));
            lista_modulow.Add(Sort_horisontal_left_to_right(6));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(7));
            lista_modulow.Add(Sort_horisontal_left_to_right(8));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(9));
            lista_modulow.Add(Sort_horisontal_left_to_right(10));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(11));
            lista_modulow.Add(Sort_horisontal_left_to_right(12));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(13));
            lista_modulow.Add(Sort_horisontal_left_to_right(14));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(15));
            lista_modulow.Add(Sort_horisontal_left_to_right(16));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(17));
            lista_modulow.Add(Sort_horisontal_left_to_right(18));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(19));
            lista_modulow.Add(Sort_horisontal_left_to_right(20));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(21));
            lista_modulow.Add(Sort_horisontal_left_to_right(22));
            lista_modulow.Add(DC_with_station_horisontal_left_to_right(23));

            lista_modulow.Add(Sort_vartical_down_to_up(24));

            lista_modulow.Add(DC_with_station_horisontal_right_to_left(25));
            lista_modulow.Add(Sort_horisontal_right_to_left(26));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(27));
            lista_modulow.Add(Sort_horisontal_right_to_left(28));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(29));
            lista_modulow.Add(Sort_horisontal_right_to_left(30));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(31));
            lista_modulow.Add(Sort_horisontal_right_to_left(32));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(33));
            lista_modulow.Add(Sort_horisontal_right_to_left(34));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(35));
            lista_modulow.Add(Sort_horisontal_right_to_left(36));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(37));
            lista_modulow.Add(Sort_horisontal_right_to_left(38));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(39));
            lista_modulow.Add(Sort_horisontal_right_to_left(40));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(41));
            lista_modulow.Add(Sort_horisontal_right_to_left(42));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(43));
            lista_modulow.Add(Sort_horisontal_right_to_left(44));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(45));
            lista_modulow.Add(Sort_horisontal_right_to_left(46));
            lista_modulow.Add(DC_with_station_horisontal_right_to_left(47));

            lista_modulow.Add(Sort_vartical_up_to_down(48));


            return lista_modulow;
        }


        public map_module DC_with_station_horisontal_left_to_right(int number)
        {
            var module = new map_module();
            module.number = number;
            module.type_of_section = type_of_section.DC;

            module.definition_of_tracks = new List<straight_track_def>
            {
                new straight_track_def(1, 0, 100, false),
                new straight_track_def(2, 0, 100, false),
                new straight_track_def(3, -45, 71, true),
                new straight_track_def(4, 45, 71, false),
                new straight_track_def(5, 0, 100, false)
            };

            module.add_speed_limit(1, 50);
            module.In_tracks = new List<int> { 1 };
            module.Out_tracks = new List<int> { 5 };

            module.internal_connections = new List<internal_connection>
            {
                // track_number - track_number
                // or track - track - track
                new internal_connection(1, 2, 3),
                new internal_connection(2, 4, 5),

            };

            module.is_there_station_in_module = true;
            //tu musze uwazac na definicje startu i konca. Kolejnosc ma znaczenie.
            module.station_definition = new station_def(3, 4);

            module.is_priority_dir_def = true;
            module.priority_direction = (1, 5);

            return module;
        }

        public map_module DC_with_station_horisontal_right_to_left(int number)
        {

            var module = new map_module();
            module.number = number;
            module.type_of_section = type_of_section.DC;

            module.definition_of_tracks = new List<straight_track_def>
            {
                new straight_track_def(1, 180, 100, false),
                new straight_track_def(2, 180, 100, false),
                new straight_track_def(3, 135, 71, true),
                new straight_track_def(4, 225, 71, false),
                new straight_track_def(5, 180, 100, false)
            };

            module.In_tracks = new List<int> { 5 };
            module.Out_tracks = new List<int> { 1 };

            module.internal_connections = new List<internal_connection>
            {
                // track_number - track_number
                // or track - track - track
                new internal_connection(1, 2, 3),
                new internal_connection(2, 4, 5),

            };

            module.is_there_station_in_module = true;
            //tu musze uwazac na definicje startu i konca. Kolejnosc ma znaczenie
            module.station_definition = new station_def(4, 3);

            module.is_priority_dir_def = true;
            module.priority_direction = (5, 1);

            return module;
        }

        public map_module Sort_vartical_down_to_up(int number)
        {

            var module = new map_module();
            module.number = number;
            module.type_of_section = type_of_section.Sort;

            module.definition_of_tracks = new List<straight_track_def>
            {
                new straight_track_def(1, 90, 200, false),
                new straight_track_def(2, 90, 200, false),
                new straight_track_def(3, 90, 200, true),
            };

            module.In_tracks = new List<int> { 1 };
            module.Out_tracks = new List<int> { 3 };

            module.internal_connections = new List<internal_connection>
            {
                // track_number - track_number
                // or track - track - track
                new internal_connection(1, 2),
                new internal_connection(2, 3)

            };
            return module;
        }

        public map_module Sort_vartical_up_to_down(int number)
        {
            var module = new map_module();
            module.number = number;
            module.type_of_section = type_of_section.Sort;

            module.definition_of_tracks = new List<straight_track_def>
            {
                new straight_track_def(1, -90, 200, false),
                new straight_track_def(2, -90, 200, false),
                new straight_track_def(3, -90, 200, false),
            };

            module.In_tracks = new List<int> { 1 };
            module.Out_tracks = new List<int> { 3 };

            module.internal_connections = new List<internal_connection>
            {
                // track_number - track_number
                // or track - track - track
                new internal_connection(1, 2),
                new internal_connection(2, 3)

            };
            return module;
        }

        public map_module Sort_horisontal_left_to_right(int number)
        {
            var module = new map_module();
            module.number = number;
            module.type_of_section = type_of_section.Sort;

            module.definition_of_tracks = new List<straight_track_def>
            {
                new straight_track_def(1, 0, 200, false),
                new straight_track_def(2, 0, 200, false),
                new straight_track_def(3, 0, 200, false),
            };

            module.In_tracks = new List<int> { 1 };
            module.Out_tracks = new List<int> { 3 };

            module.internal_connections = new List<internal_connection>
            {
                // track_number - track_number
                // or track - track - track
                new internal_connection(1, 2),
                new internal_connection(2, 3)

            };
            return module;
        }

        public map_module Sort_horisontal_right_to_left(int number)
        {
            var module = new map_module();
            module.number = number;
            module.type_of_section = type_of_section.Sort;

            module.definition_of_tracks = new List<straight_track_def>
            {
                new straight_track_def(1, 180, 200, false),
                new straight_track_def(2, 180, 200, false),
                new straight_track_def(3, 180, 200, false),
            };

            module.In_tracks = new List<int> { 3 };
            module.Out_tracks = new List<int> { 1 };

            module.internal_connections = new List<internal_connection>
            {
                // track_number - track_number
                // or track - track - track
                new internal_connection(1, 2),
                new internal_connection(2, 3)

            };
            return module;
        }

    }

        public class Interpreter_of_map_definition
        {

            public CityDataStorage translate_modules_def_to_city_map(List<map_module> modules, List<external_connection> map_modules_connections)
            {
                var CityMap = new CityDataStorage();


                // mam spis wszystkich modulow
                // moge zaczac od dodania wszystkich trackow zdefiniownych we wszystkich modulach mapy
                add_tracks_of_all_modules_to_city_map(CityMap, modules);

                // kolejny krok to dodanie noudow za pomoca wszystkich definicji connection pomiedzy tackami
                // tu jednoczesnie bedzie dodanie noudow do definicji trackow
                // ta funkcjonalnosc jest juz ogarnieta przez city_data_storage 

                add_nodes_of_all_modules_internal_connections_to_city_map(CityMap, modules);

                add_all_stations_in_all_modules(CityMap, modules);

                //kolejny krok to dodanie noudow polaczen pomiedzy modulami
                //tu jest tez translacja priority dir na numery mapy
                add_nodes_of_modules_IO_connections_to_city_map(CityMap, map_modules_connections, modules);




                return CityMap;
            }

            public map_module return_module_with_number(int number, List<map_module> modules)
            {

                for (int i = 0; i < modules.Count; i++)
                {
                    if (modules[i].number == number)
                        return modules[i];


                }
                return null; //error no such map module on the list
            }

            public void add_tracks_of_all_modules_to_city_map(CityDataStorage CityMap, List<map_module> modules)
            {
                for (int i = 0; i < modules.Count; i++)
                {
                    add_tracks_of_module_to_city_map(CityMap, modules[i]);
                }
            }

            public void add_tracks_of_module_to_city_map(CityDataStorage CityMap, map_module module)
            {
                var tracks = module.definition_of_tracks;

                for (int i = 0; i < tracks.Count; i++)
                {
                    int next_free_track_number = CityMap.GetNumberOfTracksDefinedInCity();

                    if (tracks[i].ref_track == true)
                        CityMap.AddStraightRefTrackElementToCityMap(next_free_track_number, tracks[i].length, tracks[i].angle);
                    else
                        CityMap.AddStraightTrackElementToCityMap(next_free_track_number, tracks[i].length, tracks[i].angle);

                    if (module.does_track_have_speed_def(tracks[i].number))
                        CityMap.DefineSpeedForTrack_Km_h(next_free_track_number, module.get_speed_for_track(tracks[i].number));

                    module.add_track_to_translation(tracks[i].number, next_free_track_number);

                }

            }

            public void add_nodes_of_all_modules_internal_connections_to_city_map(CityDataStorage CityMap, List<map_module> modules)
            {
                for (int i = 0; i < modules.Count; i++)
                {
                    add_nodes_of_module_internal_connections_to_city_map(CityMap, modules[i]);
                }
            }

            public void add_nodes_of_module_internal_connections_to_city_map(CityDataStorage CityMap, map_module module)
            {
                var nodes_def = module.internal_connections;
                int next_free_node_number;
                int track_1, track_2, track_3, track_4;

                for (int i = 0; i < nodes_def.Count; i++)
                {
                    next_free_node_number = CityMap.GetNumberOfDefinedNodes();



                    (track_1, track_2, track_3, track_4) = nodes_def[i].get_numbers_of_connected_tracks();

                    if (track_3 == -1 && track_4 == -1)
                    {
                        track_1 = module.translete_internal_track_number_to_city_map(track_1);
                        track_2 = module.translete_internal_track_number_to_city_map(track_2);

                        CityMap.AddNodeToCityMap(next_free_node_number, track_1, track_2);

                    }
                    else if (track_4 == -1) // three track are connected
                    {
                        track_1 = module.translete_internal_track_number_to_city_map(track_1);
                        track_2 = module.translete_internal_track_number_to_city_map(track_2);
                        track_3 = module.translete_internal_track_number_to_city_map(track_3);

                        CityMap.AddNodeToCityMap(next_free_node_number, track_1, track_2, track_3);

                    }
                    else //four tracks are connected
                    {
                        track_1 = module.translete_internal_track_number_to_city_map(track_1);
                        track_2 = module.translete_internal_track_number_to_city_map(track_2);
                        track_3 = module.translete_internal_track_number_to_city_map(track_3);
                        track_4 = module.translete_internal_track_number_to_city_map(track_4);

                        CityMap.AddNodeToCityMap(next_free_node_number, track_1, track_2, track_3, track_4);

                    }

                    if (next_free_node_number == 0)
                        CityMap.define_position_of_node(next_free_node_number, 0, 0);
                }

            }

            public void add_nodes_of_modules_IO_connections_to_city_map(CityDataStorage CityMap, List<external_connection> list_of_connection, List<map_module> list_of_modules)
            {
                // jade po kolei po wszystkich external connection

                for (int i = 0; i < list_of_connection.Count; i++)
                {
                    // sprawdzam kolejny wolny noude
                    int next_free_node_number = CityMap.GetNumberOfDefinedNodes();

                    var (first_module, first_IO, second_module, second_IO) = list_of_connection[i].return_connection_data();

                    var track_1_module = return_module_with_number(first_module, list_of_modules);
                    int track_1 = track_1_module.translete_internal_track_number_to_city_map(first_IO);
                    track_1_module.add_O_node(next_free_node_number);
                    if (track_1_module.is_there_priority_track_with_number(first_IO))
                    {
                        track_1_module.add_priority_end_node(next_free_node_number);
                    }



                    var track_2_module = return_module_with_number(second_module, list_of_modules);
                    int track_2 = track_2_module.translete_internal_track_number_to_city_map(second_IO);
                    track_2_module.add_I_node(next_free_node_number);
                    if (track_2_module.is_there_priority_track_with_number(second_IO))
                    {
                        track_2_module.add_priority_start_node(next_free_node_number);
                    }

                    // twoze noude z numerami trackow
                    CityMap.AddNodeToCityMap(next_free_node_number, track_1, track_2);


                    // definiuje kierunek trackow ktore przypinam
                    if (track_1_module.is_there_I_track_with_number(first_IO))
                    {
                        define_track_directionality_from_node(next_free_node_number, track_1);
                        define_track_directionality_to_node(next_free_node_number, track_2);
                    }
                    if (track_1_module.is_there_O_track_with_number(first_IO))
                    {
                        define_track_directionality_from_node(next_free_node_number, track_2);
                        define_track_directionality_to_node(next_free_node_number, track_1);
                    }

                    track_1_module.if_track_is_priority_translate_to_map_node(first_IO, next_free_node_number);
                    track_2_module.if_track_is_priority_translate_to_map_node(second_IO, next_free_node_number);


                }



                void define_track_directionality_from_node(int node, int track)
                {
                    CityMap.DefineDirectionForTrack(track, node);
                }

                void define_track_directionality_to_node(int node, int track)
                {
                    int node_on_the_othere_side = CityMap.get_number_of_node_on_the_othere_side_of_track(track, node);
                    CityMap.DefineDirectionForTrack(track, node_on_the_othere_side);
                }




            }

            public void add_all_stations_in_all_modules(CityDataStorage CityMap, List<map_module> modules)
            {
                for (int i = 0; i < modules.Count; i++)
                {
                    add_station_per_module_definition(CityMap, modules[i]);
                }
            }

            public void add_station_per_module_definition(CityDataStorage CityMap, map_module module)
            {

                //sprawdzam czy jest stacja w danym module
                if (module.is_there_station_in_module == true)
                {
                    // odczytuje koleny wolny node 
                    int next_free_node = CityMap.GetNumberOfDefinedNodes();

                    //odczytuje tracki
                    var (connected_track_1, connected_track_2) = module.station_definition.get_numbers_of_connected_tracks();

                    // tlumacze numery trackow stacji
                    connected_track_1 = module.translete_internal_track_number_to_city_map(connected_track_1);
                    connected_track_2 = module.translete_internal_track_number_to_city_map(connected_track_2);

                    CityMap.AddNodeToCityMap(next_free_node, connected_track_1, connected_track_2);

                    int next_free_station_number = CityMap.NumberOfStations();

                    CityMap.AddStationToCityMap(next_free_station_number, next_free_node);

                    // definiowanie kierunkowosci trackow stacji
                    define_track_directionality_from_station(next_free_node, connected_track_2);
                    define_track_directionality_to_station(next_free_node, connected_track_1);


                    //zaznaczanie tarckow ktore sa dojazdowe do stacji
                    //oznacza top tez ze moze byc wiecej niz i jade az do skzyzowania
                    mark_station_access_tracks(connected_track_1, next_free_node);
                    mark_station_access_tracks(connected_track_2, next_free_node);


                    module.add_station_translation(next_free_station_number, next_free_node);

                    module.add_I_node(next_free_node);
                    module.add_O_node(next_free_node);
                }

                void define_track_directionality_from_station(int node, int track)
                {
                    CityMap.DefineDirectionForTrack(track, node);
                }

                void define_track_directionality_to_station(int node, int track)
                {
                    int node_on_the_othere_side = CityMap.get_number_of_node_on_the_othere_side_of_track(track, node);
                    CityMap.DefineDirectionForTrack(track, node_on_the_othere_side);
                }

                void mark_station_access_tracks(int track_number, int station_node)
                {
                    int current_node_1 = station_node;
                    int current_track_number = track_number;

                    while (true)
                    {
                        int current_node_2 = CityMap.get_number_of_node_on_the_othere_side_of_track(track_number, current_node_1);

                        CityMap.mark_track_as_acces_to_station(current_track_number);

                        if (CityMap.is_node_an_intersection(current_node_2))
                        {
                            break;
                        }

                        current_track_number = CityMap.get_number_of_track_also_connected_to_node(current_node_2, current_track_number);
                        current_node_1 = current_node_2;

                    }



                }

                // dodaje stacje do sieci miasta

                // zaznaczam wszystkie tracki ktore prowadza do stacji
                //jest to potrzebne przy ustalaniu kierunkowosci 

            }

            public void add_priority_dir_if_def_to_module(map_module module)
            {
                if (module.is_priority_dir_def == true)
                {
                    var (start_track, end_track) = module.get_priority_dir();
                    // translate tracks to node of the low level map





                }


            }
            // translating map modules to low level definition of map

            // jak to zrobic
            // mam spis wszystkich modulow
            // moge zaczac od dodania wszystkich trackow zdefiniownych we wszystkich modulach mapy
            //to bedzie dosc proste
            //sprawdzam jaki jest kolejny dotepny numer w mapie
            //dodaje track z jego wszystkim danymi
            // kolejny krok to dodanie noudow za pomoca wszystkich definicji connection pomiedzy tackami
            // tu jednoczesnie bedzie dodanie noudow do definicji trackow
            // dodanie noudow na granicy modulow
            //doaje nowy noude (odrazu dodaje do niego przypiete tracki
            // dodanie noudow kierunkowych do definicji trackow do ktorych sa przypiete sa noudy laczace moduly
            // tu bedzie klopot z auto definicja kierunku na przejezdzie na wprost w sekcjach gdzie jest jest
            // przystanek
            // jak to zrobic automatycznie ?
            // np zaznaczyc tracki dojazdu do stacji (bedzie to sie dzialo podczas definicji kierunku
            // przez polaczenie z noudem wejscia IO )
            // jedynym wyzwaniem jest to ze jesli do pierwszego skzyzowania ze stacji prowadzi wiecej niz jeden track to powinienem zaznaczyc
            // wszystkie.
            // dam rade bo juz beda wszystkie polaczenia pomiedzy trackami wmodule
            //wiec zaczynajac od trucku z definicji przypiecia nouda IO-track sprawdzam kolejny przypiety
            //definiuje jego kierunek
            // i tak az natrafie na skrzyzowanie, wtedy sie zatrzymuje.

            // podczas ustalania kierunku przez skrzyzowanie tracku dojazdu do stacji beda traktowane jak by ich niebylo
            // dzieki temu moge ustalic kierunek przejazdu bez dodatkowego recznego ustalania kierunkowosci. cool :)
            //

            // naraze zrobie tak ze node zero, bedzie miec automatycznie definicje pozycji (0,0) czylli bedzie
            // tez odniesieniem dla calej reszty obliczen pozycji w miescie.
        }

        public class map_module
        {
            public int number { get; set; }
            public type_of_section type_of_section { get; set; }

            public List<int> In_tracks { get; set; }
            public List<int> Out_tracks { get; set; }

            private List<int> List_of_I_nodes;
            private List<int> List_of_O_nodes;
            private List<(int, double)> List_of_speed_limits;

            public List<straight_track_def> definition_of_tracks { get; set; }
            public List<internal_connection> internal_connections { get; set; }
            //public List<IO_def> definition_of_IO_connections { get; set; }

            public bool is_there_station_in_module { get; set; }

            /// <summary>
            /// Priority track-track in internal numeration
            /// </summary>
            public (int, int) priority_direction { get; set; }
            public bool is_priority_dir_def { get; set; }

            //tu mam to zupelnie pominiete a bez tego tlumaczenia to niezadziala 
            private int priority_start_node { get; set; }
            private int priority_end_node { get; set; }

            /// <summary>
            /// Station definition takes two numbers. Number of tracks connected to station
            /// First is end track, seconf is start track. 
            /// </summary>
            public station_def station_definition { get; set; }

            public List<(int, int)> track_translation_to_map_numeration;
            //public List<(int, int)> IO_node_translation_to_map_numeration;
            private (int, int) Station_number_translation;

            public map_module()
            {
                track_translation_to_map_numeration = new List<(int, int)>();
                List_of_speed_limits = new List<(int, double)>();
                List_of_I_nodes = new List<int>();
                List_of_O_nodes = new List<int>();
            }

            public void add_track_to_translation(int module_track_number, int map_number)
            {
                track_translation_to_map_numeration.Add((module_track_number, map_number));
            }

            public void add_station_translation(int number, int number_of_node)
            {
                Station_number_translation = (number, number_of_node);
            }

            public int translete_internal_track_number_to_city_map(int number)
            {

                for (int i = 0; i < track_translation_to_map_numeration.Count; i++)
                {
                    if (track_translation_to_map_numeration[i].Item1 == number)
                        return track_translation_to_map_numeration[i].Item2;


                }
                return -1; //error no number found on list
            }

            public bool is_there_I_track_with_number(int number)
            {
                if (In_tracks.Contains(number))
                    return true;
                else
                    return false;
            }

            public bool is_there_O_track_with_number(int number)
            {
                if (Out_tracks.Contains(number))
                    return true;
                else
                    return false;
            }

            public bool is_there_priority_track_with_number(int number)
            {
                if (priority_direction.Item1 == number || priority_direction.Item2 == number)
                    return true;
                else
                    return false;

            }

            public void add_priority_start_node(int node)
            {
                priority_start_node = node;
            }

            public void add_priority_end_node(int node)
            {
                priority_end_node = node;
            }

            public bool is_priority_direction_def()
            {
                if (is_priority_dir_def)
                    return true;
                else
                    return false;
            }

            public (int, int) get_priority_dir()
            {
                return (priority_start_node, priority_end_node);
            }

            public void add_I_node(int node)
            {
                List_of_I_nodes.Add(node);
            }

            public List<int> get_I_nodes_numbers()
            {
                return List_of_I_nodes;

            }

            public void add_O_node(int node)
            {
                List_of_O_nodes.Add(node);
            }

            public List<int> get_O_nodes_numbers()
            {
                return List_of_O_nodes;

            }

            public void add_speed_limit(int track, double speed)
            {
                List_of_speed_limits.Add((track, speed));
            }

            public bool does_track_have_speed_def(int track)
            {

                for (int i = 0; i < List_of_speed_limits.Count; i++)
                {
                    if (List_of_speed_limits[i].Item1 == track)
                        return true;
                }
                return false;

            }

            public double get_speed_for_track(int track)
            {
                for (int i = 0; i < List_of_speed_limits.Count; i++)
                {
                    if (List_of_speed_limits[i].Item1 == track)
                        return List_of_speed_limits[i].Item2;
                }

                return -1; //error
            }

            public void if_track_is_priority_translate_to_map_node(int internal_track_num, int map_node)
            {
                if (is_priority_dir_def)
                {
                    if (priority_direction.Item1 == internal_track_num)
                        priority_start_node = map_node;
                    if (priority_direction.Item2 == internal_track_num)
                        priority_end_node = map_node;
                }
            }
        }

        /// <summary>
        /// This definition is sutable olny for strait track element
        /// </summary>
        public class straight_track_def
        {


            public int number { get; set; }
            public bool ref_track { get; set; }

            //alngle coordinate sytem 
            public double angle { get; set; }
            public double length { get; set; }


            public straight_track_def(int number)
            {
                this.number = number;
            }



            public straight_track_def(int number, double angle, double length, bool ref_track)
            {
                this.number = number;
                this.ref_track = ref_track;
                this.angle = angle;
                this.length = length;
            }
        }

        public struct internal_connection
        {
            public int track_1 { get; set; }
            public int track_2 { get; set; }
            public int track_3 { get; set; }
            public int track_4 { get; set; }

            public internal_connection(int number_1, int number_2)
            {
                track_1 = number_1;
                track_2 = number_2;
                track_3 = -1; //marker of no connection
                track_4 = -1; //marker of no connection
            }
            public internal_connection(int number_1, int number_2, int number_3)
            {
                track_1 = number_1;
                track_2 = number_2;
                track_3 = number_3;
                track_4 = -1; //marker of no connection
            }
            public internal_connection(int number_1, int number_2, int number_3, int number_4)
            {
                track_1 = number_1;
                track_2 = number_2;
                track_3 = number_3;
                track_4 = number_4;
            }

            public (int, int, int, int) get_numbers_of_connected_tracks()
            {
                return (track_1, track_2, track_3, track_4);
            }

        }

        public struct station_def
        {
            private int track_1;
            private int track_2;

            public station_def(int number_1, int number_2)
            {
                track_1 = number_1;
                track_2 = number_2;
            }

            public (int, int) get_numbers_of_connected_tracks()
            {
                return (track_1, track_2);
            }
        }

        public struct IO_def
        {
            public int node { get; set; }
            public int track { get; set; }

            public IO_def(int node, int track)
            {
                this.node = node;
                this.track = track;
            }
        }

        public struct fast_line
        {
            private int in_node;
            private int out_node;

            public fast_line(int in_node, int out_node)
            {
                this.in_node = in_node;
                this.out_node = out_node;
            }

            public (int, int) return_fast_line_def()
            {
                return (in_node, out_node);
            }

        }

        public enum type_of_section
        {
            DC,
            Sort
        }

        public struct external_connection
        {
            public int first_module_number { get; set; }
            public int first_module_IO_node { get; set; }
            public int second_module_number { get; set; }
            public int second_module_IO_number { get; set; }

            public external_connection(int first_module_number, int first_module_IO_node, int sec_module_number, int sec_module_IO_number)
            {

                this.first_module_number = first_module_number;
                this.first_module_IO_node = first_module_IO_node;
                second_module_number = sec_module_number;
                second_module_IO_number = sec_module_IO_number;


            }


            public (int, int, int, int) return_connection_data()
            {
                return (first_module_number, first_module_IO_node, second_module_number, second_module_IO_number);
            }

        }
    

}

