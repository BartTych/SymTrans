using System;
using System.Collections.Generic;

namespace Symulation
{
    public class FastestProfile
    {
        private int[][] _Route;
        private CityDataStorage _City;

        

        public FastestProfile(int [][]Route, CityDataStorage City)
        {
            _Route = Route;
            _City = City;


        }


        public void ProfileBetweenStations(double a)
        {
            var base_mtrix = new RouteQueries(_Route, _City);                               //tworze objekt RouteQueries na bazie drogi(_Route) i miasta (_City)
            
            // ok jesli chce przejsc na system gdzie niema przyspierszen
            // tylko jest okreslona predkosc to juz base profile bedzie inny
            // bo niebedzie zawierac tylko spis trackow z predkosciami  
            // to juz jest wlasciwie gotowy profil 
            // jedyne co moge zerobic to uproscic to co ma taka sama predkosc
            // narazie oznacza to ze bedzie tylko jedna sekcje tutaj ze stalo predkoscia
            
            var base_profile = base_mtrix.Route_Base_MatrixBetweenStations_velocity_only(_Route, a);       //tworze macierz i opisem jak bedzie biegla droga od skrzyzowania gdzie predkosc definiuje wolniejsze ramie do konca

            Console.WriteLine("base_matrix:  ");
            for (int i = 0; i < base_profile.Count; i++)
            {
                Console.Write(base_profile[i][0].ToString("N1") + "\t");
                Console.Write(base_profile[i][1].ToString("N1") + "\t");
                Console.Write(base_profile[i][2].ToString("N1") + "\t");
                Console.Write(base_profile[i][3].ToString("N1") + "\t");
                Console.Write(base_profile[i][4].ToString("N0") + "\t");
                Console.Write(base_profile[i][5].ToString("N0") + "\t");
                Console.WriteLine(base_profile[i][6].ToString("N0") + "\t");
            }
            
            var (_profile, KodBledu) = Create_Profile(base_profile);                         //tworze profil
            


            Console.WriteLine("wynik tworzenia profilu");
            for (int i = 0; i < _profile.Count; i++)
            {
                Console.Write(_profile[i][0].ToString("N1") + "\t");
                Console.Write(_profile[i][1].ToString("N1") + "\t");
                Console.Write(_profile[i][2].ToString("N1") + "\t");
                Console.Write(_profile[i][3].ToString("N1") + "\t");
                Console.Write(_profile[i][4].ToString("N0") + "\t");
                Console.Write(_profile[i][5].ToString("N0") + "\t");
                Console.Write(_profile[i][6].ToString("N0") + "\t");
                Console.Write(_profile[i][7].ToString("N2") + "\t");
                Console.WriteLine(_profile[i][8].ToString("N2") + "\t");
            }

            var test = check_profile_distances(_profile);
            Console.Write("wynik sprawdzenia odleglosci   ");
            Console.WriteLine(test);

            test = check_profile_times(_profile);
            Console.Write("wynik sprawdzenia czasu   ");
            Console.WriteLine(test);
            
            //return new List<double[]> test;
        }

        //tu mam metode profile between intersection ale ona raczej sie nieprzyda bo jest zbyt primitywna
        //public (int error_code,List<double[]> profile) ProfileBetweenIntersections(double a, double start_speed)
        //{
        //    var base_mtrix = new RouteQueries(_Route, _City);                               //tworze objekt RouteQueries na bazie drogi(_Route) i miasta (_City)
        //    var base_profile = base_mtrix.Route_Base_MatrixBetweenIntersections (_Route, a, start_speed);       //tworze macierz i opisem jak bedzie biegla droga

        //    Console.WriteLine("base_matrix:  ");
        //    for (int i = 0; i < base_profile.Count; i++)
        //    {
        //        Console.Write(base_profile[i][0].ToString("N1") + "\t");
        //        Console.Write(base_profile[i][1].ToString("N1") + "\t");
        //        Console.Write(base_profile[i][2].ToString("N1") + "\t");
        //        Console.Write(base_profile[i][3].ToString("N1") + "\t");
        //        Console.Write(base_profile[i][4].ToString("N0") + "\t");
        //        Console.Write(base_profile[i][5].ToString("N0") + "\t");
        //        Console.WriteLine(base_profile[i][6].ToString("N0") + "\t");
        //    }

        //    var (_profile, KodBledu) = Create_Profile(base_profile);                         //tworze profil
        //    switch (KodBledu)
        //    {
        //        case 0:

        //            for (int i = 0; i < _profile.Count; i++)
        //            {
        //                Console.Write(_profile[i][0].ToString("N1") + "\t");
        //                Console.Write(_profile[i][1].ToString("N1") + "\t");
        //                Console.Write(_profile[i][2].ToString("N1") + "\t");
        //                Console.Write(_profile[i][3].ToString("N1") + "\t");
        //                Console.Write(_profile[i][4].ToString("N0") + "\t");
        //                Console.Write(_profile[i][5].ToString("N0") + "\t");
        //                Console.Write(_profile[i][6].ToString("N0") + "\t");
        //                Console.Write(_profile[i][7].ToString("N2") + "\t");
        //                Console.WriteLine(_profile[i][8].ToString("N2") + "\t");
        //            }
        //            return (KodBledu, _profile);

        //        case 3:
        //            Console.WriteLine("Kapsula niedala rady przyspieszyc");
        //            return (KodBledu, _profile);

        //        case -3:
        //            Console.WriteLine("Kapsula niedala rady zachamowac na wjezdzie");
        //            return (KodBledu, _profile);
        //    }

        //    return (-10, _profile); //error in error generation process
        //}
        public (int error_code, List<double[]> profile) ProfileBetweenNodes(double a)
        {
            var base_mtrix = new RouteQueries(_Route, _City);                 //tworze objekt RouteQueries na bazie drogi(_Route) i miasta (_City)
            var base_profile = base_mtrix.Route_Base_MatrixBetweenNoudes(_Route, a);       //tworze macierz i opisem jak bedzie biegla droga
            //Console.WriteLine("base_matrix:  ");
            for (int i = 0; i < base_profile.Count; i++)
            {
                //Console.Write(base_profile[i][0].ToString("N1") + "\t");
                //Console.Write(base_profile[i][1].ToString("N1") + "\t");
                //Console.Write(base_profile[i][2].ToString("N1") + "\t");
                //Console.Write(base_profile[i][3].ToString("N1") + "\t");
                //Console.Write(base_profile[i][4].ToString("N0") + "\t");
                //Console.Write(base_profile[i][5].ToString("N0") + "\t");
                //Console.WriteLine(base_profile[i][6].ToString("N0") + "\t");
            }

            var (_profile, KodBledu) = Create_Profile(base_profile);                         //tworze profil
            //Console.WriteLine("wynik tworzenia profilu");

            switch (KodBledu)
            {
                case 0:
                    for (int i = 0; i < _profile.Count; i++)
                    {
                        //Console.Write(_profile[i][0].ToString("N1") + "\t");
                        //Console.Write(_profile[i][1].ToString("N1") + "\t");
                        //Console.Write(_profile[i][2].ToString("N1") + "\t");
                        //Console.Write(_profile[i][3].ToString("N1") + "\t");
                        //Console.Write(_profile[i][4].ToString("N0") + "\t");
                        //Console.Write(_profile[i][5].ToString("N0") + "\t");
                        //Console.Write(_profile[i][6].ToString("N0") + "\t");
                        //Console.Write(_profile[i][7].ToString("N2") + "\t");
                        //Console.WriteLine(_profile[i][8].ToString("N2") + "\t");
                    }
                    return (KodBledu, _profile);

                case 3:
                    //Console.WriteLine("Kapsula niedala rady przyspieszyc");
                    return (KodBledu, _profile);

                case -3:
                    //Console.WriteLine("Kapsula niedala rady zachamowac na wjezdzie");
                    return (KodBledu, _profile);
            }

            return (-10, _profile); //error in error generation process
        }
        /// <summary>
        /// Simplification of route, where sections with same speed are merged
        /// </summary>
        /// <param name="Base"></param>
        /// <returns></returns>
        public (List<double[]>, int KodBledu) Create_Profile_velocity_only(List<double[]> Base)
        {
                // ide po colosci i merguje zekcje jesli jest taka sama predkosc 
                // pytanie czy to mergowanie jest do czegos wogule potrzebne ?
                // napewno to ulatwia czytanie ale czy cos pozatym ?
                // maze tak naprawde to pozniej tylko komplikuje i musze to spowrotem rozkladac 
                // wiec tak naprawde to narazie to zostawie bez mergowanie dla uproszczenia 

                // licze czasy po jakim znajdzie sie dana kapsola na paczatku i koncu danego odcinka
                // zostalo tylko to do zrobienia

                Base = Time_Calculation_For_Base(Base);
                return (Base, 0);

        }

        public (List<double[]> profile,int KodBledu ) Create_Profile(List<double[]> Base)
        {
            int Current_Step = 0;
            int temp_Add = 1;
            int KodBledu = 0;


        //Base matrix
        // [0]: X0 odleglosc charakterystyczna(poczatek dla przyspieszenia, koniec dla hamowania) , dla track to jest poczatek 
        // [1]: X1 odleglosc charakterystyczna2(tylko dla track, oznacza koniec tracku)
        // [2]: V predkosc charakterystyczna (poczatkowa dla przyspieszenia, ograniczenie dla T, koncowa dla B)
        // [3]: przyspieszenie do jakiego zdolna jest kapsola
        // [4]: rodzaj lini (-1 brake, 0 track, 1 accelerate)
        // [5]: node 1  
        // [6]: node 2
        // 7 & 8 are added at the end of that method
        // [7]: czas poczatku danego odcinka
        // [8]: czas konca danego odcinka

            //dla tracku sa 

        alfa:
            if (Current_Step < Base.Count)
            {

                if (Base[Current_Step][4] == 1)//jesli przyspieszanie. Tu jest modyfikacja. Dla wersji z Noudami na poczatku i koncu. moze sie zdazyc ze kapsula nieosiagnie predkosci na wyjsciu co jest bledem.
                {
                deltaA:

                    switch (Base[Current_Step + temp_Add][4])//sprawdzam co jest dalej (-1,0,1)
                    {
                        case 0://dalej jest T
                            //funkcja sprawdzajaca przeciecie AT
                            var res_AT = Intersection_AT(Base, Current_Step);

                            if (res_AT[0] == 1)//jest przeciecie z sekcja T
                            {
                                //funkcja przycinajaca AT
                                Base = Trim_AT(Base, Current_Step, res_AT);

                                Current_Step++;
                               

                                goto alfa;

                            }
                            else  //brak przeciecia z sekcja T
                            {
                                if (Current_Step == (Base.Count - 2)) // niema przciecia i ta sekcja t jest ostatnia wiec w wersji no to node oznacza ze kapsula niedala rady przyspieszyc
                                                                        //jest to troche niepokojace czy niepowinienem utworzyc nowej wersji tej metody tylko dla szukania pomiedzy profilu pomiedzy noudami
                                {
                                    KodBledu = 3;// jakis znacznik bledu, kapsula niedala rady przyspieszyc do predkosci wyjazdu
                                    break;
                                }
                                Base.RemoveAt(Current_Step + 1);
                                
                                //w takiej sytuacji brakuje tutaj dodania do planu drogi sekcji na ktorej w calosci jest przyspieszenie kontynuowane w kolejnej lub kolejnych sekcjach
                                // w tej chwili jest tak ze jesli kapsola przyspiesza przez wiele trackow to w koncowym wyniku, wyglada to jak jeden bardzo dlugi track
                                // moze to byc potrzebne do przeprowadzania przejazdu w dalszej czesci dzialania softu.
                                // to tak zostanie i dodam inna metode ktora wlaczy brakujace noudy w opis przejazdu 
                                goto deltaA;
                            }

                        case 1:
                            //funkcja sprawdzajaca przeciecie AA . Narazie a jest zawsze takie samo wiec przeciecie niejest mozliwe
                            //wiec wywalam ta sekcje przejazdu
                            Base.RemoveAt(Current_Step + 1); //del A
                            //temp_Add++;
                            goto deltaA;

                        case -1: //dalej jest hamowanie
                            //funkcja sprawdzajaca czy jest przeciacie AB
                            //tutaj dodatkowo trzeba sprawdzic co sie dzieje jesli niema przeciecia pomiedzy kryzwymi A i B w prawidlowym zakresie
                            //jesl potencjalne przeciecie jest za sekcja B to jest ok i algorytm moze isc do przodu ale jesli przeciecie potencjalne jest z tylu przed A
                            //to oznacza ze trzaba przelaczyc algoytm w tryb szukanie przeciecia w przeciwnym kierunku z punku widzenia B
                            var res_AB = Intersection_AB(Base, Current_Step);
                            
                            if (res_AB[0] == 1)//jesli jest przeciecie na odcinku pomiedzy A i B
                            {
                                //funkcja przycinajaca AB
                                Base = Trim_AB(Base, Current_Step, res_AB);

                                //temp_Add = 1;
                                Current_Step = Current_Step + 2;
                                goto alfa;
                            }
                            else// jesli nie
                            {
                                    //go to next step and change to rewerse mode
                                if (res_AB[1] == -1)
                                {
                                    Current_Step++;
                                    goto alfa;
                                }
                                else  //algorytm goes forward, przeciecie jest za koncem B, czyli juz z kolejna sekcja
                                Base.RemoveAt(Current_Step + 1);//del B
                                //temp_Add++;
                                goto deltaA;

                            }
                    }
                }
                else if (Base[Current_Step][4] == -1) //current step to hamowanie. tu jest modyfikacja dla wersji jazdy pomiadzy noudami. Jelis a jest male lub odleglosc krotka
                                                      //to kapsula moze niedac rady zahamowac majac predkosc na wejsciu. oznacza to ze sekcja hamujaca musi miec dodatkowe sprwdzenie czy przeciecie
                                                      //nienastapilo przed poczatkiem drogi
                {
                deltaB:
                    switch (Base[Current_Step - temp_Add][4]) //jaki jest poprzedni krok
                    {
                        case 0://poprzedni krok to T
                            //funkcja sprawdzajaca przeciecie BT
                            var res_BT = Intersection_BT(Base, Current_Step);

                            if (res_BT[0] == 1)//jesli jest przeciecie , tutaj brakuje jednej rzeczy jesli jest przeciecie z T to jaka mam pewnosc ze tak naprawde prawidlowe
                                               //niejest przeciecie z A ktore jest wczesniej ? to jest ok bo jak robie trim AT wczesniej to sekcja T zaczyna sie teraz od konca przespieszania
                                               //wiec jesli niedojdzie do przeciecia w nowym zakresie T to widomo ze misi byc przeciecie z A i algoryt przechodzi do nastepnego kroku
                                               //wyrzuca T
                            {
                                //funkcja przycinajaca BT
                                Base = Trim_BT(Base, Current_Step,res_BT);
                                //temp_Add = 1;
                                Current_Step++;
                                goto alfa;

                            }
                            else //jesli nie ma przeciecia w prawidlowym zakresie
                            {
                                if (Current_Step == 1)
                                {
                                    KodBledu = -3;  //jakies oznaczenie ze nastapil blad. Kapsula niedala rady zachamowac z predkosci wejsciowej
                                    break;
                                    
                                }


                                Base.RemoveAt(Current_Step - 1);
                                //temp_Add++;
                                Current_Step--;
                                goto deltaB;
                            }



                        case 1://poprzedni krok to A
                            //funkcja sprawdzajac czy jest przeciecie BA
                            var res_BA = Intersection_BA(Base, Current_Step);
                            if (res_BA[0] == 1)//jesli jest przeciecie
                            {
                                //funkcja przycinajaca BA
                                Base = Trim_BA(Base, Current_Step, res_BA);
                                temp_Add = 1;
                                Current_Step++;
                                goto alfa;

                            }
                            //jesli niema przeciecia
                            else
                            {
                                Base.RemoveAt(Current_Step - 1); //del A
                                //temp_Add++;
                                Current_Step--;
                                goto deltaB;
                            }

                        case -1:// poprzedni krok to B. Usuwam B bo przeciecie jest niemozliwe a jest narazie zawsze takie samo

                            Base.RemoveAt(Current_Step - 1); //del B
                            //temp_Add++;
                            Current_Step--;
                            goto deltaB;



                    }
                }
                else //jesli dany step to T , ide dalej do przodu, prowadze obliczenia zawsze z punktu widzenia sekcji gdzie sa zmiany
                {
                    Current_Step++;
                    goto alfa;
                }
            }
            if(KodBledu==0)

            Base = Time_Calculation_For_Base(Base);
            //sRouteTiming(Base);

            return (Base,KodBledu);
        }

        public bool check_profile_distances(List<double[]> profile)
        {
            var length = profile.Count;
            bool result = true;

            for (int i = 0; i < length; i++)
            {
                if(profile[i][0] >= profile[i][1])
                {
                    result = false;
                }

            }

            return result;
        }

        public bool check_profile_times(List<double[]> profile)
        {
            var length = profile.Count;
            bool result = true;

            for (int i = 0; i < length; i++)
            {
                if (profile[i][7] >= profile[i][8])
                {
                    result = false;
                }

            }

            return result;
        }

        public double [][] RouteTiming(List<double[]> Profile )
        {
            var querie = new RouteQueries(_Route, _City);
            var Summary = querie.SummaryOfRoute(_Route);// we wczesniejszej czesci algorytmu moglem zrobic summary numeru nouda i odleglosci , tez juz moge dodac pozostale zmienne
                                                                                                                                        //przyspieszenie, predkosc , czas                                                                                


            for(int i = 0; i < Summary.Length; i++)
            {
                int Node_Number = (int)Summary[i][0];

                //sprawdzam w ktorej sekcji jest dany node
                int Secition_Index = In_Which_section_Of_Base_distance_Of_ProfileIs(Profile, Summary[i][1]);
                //wrzucam wynik do summary
                
                Summary[i][2] = Calculate_Velocity_for_Node_In_Profile(Profile, Secition_Index, Summary[i][1]);
                Summary[i][3] = Calculate_acceleration_for_Node_In_Profile(Profile, Secition_Index, Summary[i][1]);
                Summary[i][4] = Calculate_Time_for_Node_In_Profile(Profile, Secition_Index, Summary[i][1]);

            }

            
            //Console.WriteLine("summary of route");
            //for (int i = 0; i < Summary.Length; i++)
            //{
            //    Console.Write(Summary[i][0] + "\t");
            //    Console.Write(Summary[i][1] + "\t");
            //    Console.Write(Summary[i][2].ToString("N2") + "\t");
            //    Console.Write(Summary[i][3].ToString("N2") + "\t");
            //    Console.WriteLine(Summary[i][4].ToString("N2") + "\t");

            //}

            return Summary;

        }  //niepamietam po co jest ta metoda, prawdopodobnie do wyrzucenia :)


        public double Calculate_acceleration_for_Node_In_Profile(List<double[]> Profile, int section_Index, double distance)  //section index to sekcja profilu, distance
        {

            int Type = What_type_of_Section(Profile, section_Index);
            bool isThisALastSection = false;
            if (Profile.Count == (section_Index + 1))
                isThisALastSection = true;


            if (isThisALastSection)
            {
                var B = Profile[section_Index];
                return -B[3];
            }
            else
            {


                switch (Type)
                {
                    case 0:
                        
                        var T = Profile[section_Index];
                        if(T[1]==distance)
                            T = Profile[section_Index + 1];
                        return T[3];

                    case 1:
                        var A = Profile[section_Index];
                        if (A[1] == distance)
                            A = Profile[section_Index + 1];
                        return A[3];

                    case -1:
                        var B = Profile[section_Index];
                        if (B[1] == distance)
                            B = Profile[section_Index + 1];
                        return -B[3];

                }

                return -1;

            }

        }
        public double Calculate_Time_for_Node_In_Profile(List<double[]> Profile, int section_Index, double distance)
        {

            int Type = What_type_of_Section(Profile, section_Index);

            switch (Type)
            {
                case 0:

                    var T = Profile[section_Index];

                    double T0 = (distance - T[0]) / T[2];
                    return T0+T[7];

                case 1:

                    var A = Profile[section_Index];

                    var A1 = 0.5 * A[3];
                    var B1 = A[2];
                    var C1 = A[0] - distance;

                    var del = Math.Sqrt(Math.Pow(B1, 2) - 4 * A1 * C1);
                    double T1 = (-B1 + del) / (2 * A1);
                    return T1 + A[7];

                case -1:
                    var B = Profile[section_Index];

                    var A2 = -0.5 * B[3];
                    var B2 = -B[2];
                    var C2 = B[1] - distance;

                    var del1 = Math.Sqrt(Math.Pow(B2, 2) - 4 * A2 * C2);
                    double T_1 = (-B2 - del1) / (2 * A2);
                    return B[8]-T_1;

            }

            return -1;



        }
        public double Calculate_Velocity_for_Node_In_Profile(List<double[]> Profile, int section_Index, double distance)
        {

            int Type = What_type_of_Section(Profile, section_Index);

            switch (Type)
            {
                case 0:

                    var T = Profile[section_Index];

                    return T[2];

                case 1:

                    var A = Profile[section_Index];

                    var A1 = 0.5 * A[3];
                    var B1 = A[2];
                    var C1 = A[0] - distance;

                    var del = Math.Sqrt(Math.Pow(B1, 2) - 4 * A1 * C1);
                    double T1 = (-B1 + del) / (2 * A1);
                    return T1 * A[3] + A[2];

                case -1:
                    var B = Profile[section_Index];

                    var A2 = -0.5 * B[3];
                    var B2 = -B[2];
                    var C2 = B[1] - distance;

                    var del1 = Math.Sqrt(Math.Pow(B2, 2) - 4 * A2 * C2);
                    double T_1 = (-B2 - del1) / (2 * A2);
                    return B[2] + T_1 * B[3];

            }

            return -1;

        }

        public int In_Which_section_Of_Base_distance_Of_ProfileIs(List<double[]> Profile ,double distnce)
        {
            for(int i = 0; i < Profile.Count; i++)
            {
                if(distnce >= Profile[i][0] & distnce <= Profile[i][1])
                {
                    return i;
                    
                }

            }
            return -1;

        }

        public int What_type_of_Section(List<double[]> Profile,int index)
        {

            return (int)Profile[index][4];

        }

        public List<double[]> Time_Calculation_For_Base(List<double[]> Base)            //ta metoda uzupelnia obliczenia czasu, tak zeby wszystkie komorki byly wypelnione 
        {

            for(int i = 0; i < Base.Count; i++)
            {
                switch (Base[i][4])  // type of section
                {
                    case 1: //acceleration
                        if(i != 0)//jesli to jest pierwsze przyspieszenie to niema indeksu i -1, zostaiam zero jako czas poczatku
                            Base[i][7] = Base[i - 1][8];
                        Base[i][8] = Base[i][7] + Base[i][8];
                        continue;

                    case 0: //track
                        if (i != 0)//jesli pierwszy jest track w wersji do drogi miedzy noudami to niema indexu i -1, zostawiam zero jako poczatek drogi
                            Base[i][7] = Base[i - 1][8];
                        Base[i][8] = Base[i][7] + (Base[i][1] - Base[i][0]) / Base[i][2];
                        continue;

                    case -1: //brake
                        Base[i][7] = Base[i - 1][8];
                        Base[i][8] = Base[i][7] + Base[i][8];
                        continue;

                }
            }

            return Base;
        }

        public List<double[]> Trim_AT(List<double[]>Base,int CurentStep, double[] res)
        {
            Base[CurentStep][1] = res[1];   // distence at end of acceleration section
            Base[CurentStep][8] = res[2];   // time at end

            Base[CurentStep+1][0] = res[1];     //begining of next section

            return Base;
        }

        public List<double[]> Trim_AB(List<double[]> Base, int CurentStep, double[] res)
        {
            Base[CurentStep][1] = res[1];       //distence ate end
            Base[CurentStep][8] = res[2];       //time at end

            Base[CurentStep + 1][0] = res[1];   //begining of next section
            Base[CurentStep + 1][8] = res[3];

            return Base;
        }

        public List<double[]> Trim_BA(List<double[]> Base, int CurentStep, double[] res)
        {
            Base[CurentStep][0] = res[1];
            Base[CurentStep][8] = res[3];

            Base[CurentStep -1][1] = res[1];
            Base[CurentStep - 1][8] = res[2];

            return Base;
        }

        public List<double[]> Trim_BT(List<double[]> Base, int CurentStep, double[] res)
        {
            Base[CurentStep][0] = res[1];
            Base[CurentStep][8] = res[2];

            Base[CurentStep - 1][1] = res[1];

            return Base;
        }

        //tutaj jest liczony czas jki mija od poczatku danej sekcji do jej konca
        //w funkcji Time_Calculation_For_Base jest to przeliczane na czas wzgladem poczatku przejazdu

        public double[] Intersection_AT(List<double[]> Base, int Current_Step)
        {
            var A = Base[Current_Step];
            var T = Base[Current_Step + 1];

            double del_T = (T[2] - A[2]) / A[3];
            double X = A[0] + A[2] * del_T + 0.5 * A[3] * Math.Pow(del_T, 2);

            if(X>=T[0] && X <= T[1]) //przeciecie jest w ramach odleglosci badanego tracku
                return new double[3] { 1, X ,del_T};
            else
                return new double[3] { -1, 0 ,0};

            // result {1 jest przeciecie, -1 niema przeciecia: x odleglosc przeciecia : czas od poczatku przyspieszenia do przeciecia}

            // jest tu problem bo jesli jest przeciecie z T a dalej jest B
            // to tak naprawde moze dojs do sytuacji kiedy prawudlowe jest pzeciecie AB z pominieciem T
            // czy ja to teraz uwzgledniam ?
        }

        public double[] Intersection_AB(List<double[]> Base, int Current_Step)
        {
            var A = Base[Current_Step];
            var B = Base[Current_Step + 1];

            double del_T = (-B[2] + A[2]) / A[3];
            double X2 = B[1] - B[2] * del_T - 0.5 * B[3] * Math.Pow(del_T, 2);
            double X = ( X2 + A[0]) / 2.0;

            var A1 = 0.5 * A[3];
            var B1 = A[2];
            var C1 = A[0] - X;

            var del = Math.Sqrt(Math.Pow(B1, 2) - 4 * A1 * C1);
            double T1 = (-B1 + del) / (2 * A1);


            if (X >= A[0] && X <= B[1])
                return new double[4] { 1, X , T1 , T1 + del_T };
            else
                if(X< A[0])     //sprawdzenie gdzie jest przeciecie, przed czy za przyspieszeniem 
                return new double[4] { -1, -1,0,0 };
                else
                return new double[4] { -1, 1, 0, 0 };
            //result {1 jest przeciecie, -1 niema przeciecia: x odleglosc przeciecia, jeli niema przeciecia -1 oznacza przed, 1 oznacza za : czas od poczatku przyspieszenia do przeciecia, czas od przeciecia do konca hamowania }
        }

        public double[] Intersection_BA(List<double[]> Base, int Current_Step) //przeliczenie przeciecia BA oznacza ze obiczenie odbywa sie wstecz, z punktu widzenia hamowania
        {
            var A = Base[Current_Step - 1];
            var B = Base[Current_Step];

            double del_T = (-B[2] + A[2]) / A[3];           //roznica predkosci po hamowaniu i na poczatku przyspieszania
            double X2 = B[1] - B[2] * del_T - 0.5 * B[3] * Math.Pow(del_T, 2);  //odleglosc przy jakiej jest taka sama predkosc obu scierzek przyspieszenia
            double X = (X2 + A[0]) / 2.0;                                          //srodek czyli gdzie spotykaja sie dwie zmodyfikowane scierzki 

            var A1 = 0.5 * A[3];
            var B1 = A[2];
            var C1 = A[0] - X;

            var del = Math.Sqrt(Math.Pow(B1, 2) - 4 * A1 * C1);
            double T1 = (-B1 + del) / (2 * A1);




            if (X >= A[0] && X <= B[1])
                return new double[4] { 1, X, T1, T1 + del_T };
            else
                return new double[4] { -1, 0,0,0 };


        }

        public double[] Intersection_BT(List<double[]> Base, int Current_Step)
        {
            var B = Base[Current_Step];
            var T = Base[Current_Step -1];

            double del_T = (T[2] - B[2]) / B[3];
            double X = B[1] - B[2] * del_T - 0.5 * B[3] * Math.Pow(del_T, 2);
            //double T1 = (X - T[0])/T[2];

            if (X >= T[0] && X <= T[1])
                return new double[3] { 1, X ,del_T};
            else
                return new double[3] { -1, 0 , 0 };


        }

      

        public void Steps()
        {

            var step = new RouteQueries(_Route,_City);

            var wynik = step.List_Of_Route_Steps_BetweenStations(_Route);
            Console.WriteLine("Steps matrix");
            for (int i = 0; i < wynik.Length; i++)
            {
                Console.Write(wynik[i][0] + "  ");
                Console.Write(wynik[i][1] + "  ");
                Console.Write(wynik[i][2] + "  ");
                Console.WriteLine(wynik[i][3] + "  ");
            }

        }

        public void Base()//przygotowuje i wyswietla base matrix
        {

            var base_mtrix = new RouteQueries(_Route, _City);

            var wynik = base_mtrix.Route_Base_MatrixBetweenStations(_Route, 5);
            Console.WriteLine("base matrix");
            for (int i = 0; i < wynik.Count; i++)
            {
                Console.Write(wynik[i][0] +"\t");
                Console.Write(wynik[i][1] + "\t");
                Console.Write(wynik[i][2].ToString("N1") + "\t");
                Console.Write(wynik[i][3] + "\t");
                Console.Write(wynik[i][4] + "\t");
                Console.Write(wynik[i][5] + "\t");
                Console.WriteLine(wynik[i][6] + "\t");
            }
            Console.WriteLine('\n');

        }
    
    }

}
