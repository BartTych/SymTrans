using System;
using System.Collections.Generic;
using System.Linq;

namespace Symulation
{
    public class CityDataStorage
    {
        private Node[] _NodeList;
        private Track[] _listOfTracks;
        private Station[] _StationList;

        int ActiveNode;
        
        private int WorkDirectionalNodeNumber;
        private List<int> ListOfNodesConnectedToEvaluatedTrack;
        private int EvaluatedNodeForTrackWhichIsHavingDirectionEstablished;
        private int NodeOnOthereSideOfEvaluatedNode;
        private Track TrackFromWhichDirectionalityIsCopied;

        private int NumberOfTracksWithDefinedDirectionality;
        private List<int> ListOfTracksConnectedToEvaluatedNode;
        //private List<int> _ListaAktywnychNodow;

        private List<double> ListOfAllSpeedLimitsInCity;
        private double WorkSpeedLimit;


        bool IsGivenRoundOfCalculatingSpeedSuccesfull = true;
        private double SpeedUnderConsideration;

        public CityDataStorage()
        {
            _NodeList = new Node[1000];
            _listOfTracks = new Track[1000];
            _StationList = new Station[100];
            ListOfAllSpeedLimitsInCity = new List<double>();
            //_ListaAktywnychNodow = new List<int>();
            //ActiveNode = new int();
        }
        public int AddNodeToCityMap()
        {
            var Number = GetNumberOfDefinedNodes();
            _NodeList[Number] = (new Node(Number));
            return Number;
        }
        public void AddNodeToCityMap(int Number)
        {
            _NodeList[Number] = (new Node(Number));
        }

        //public void AddNodeToCityMap(int Number, int TrackNumber1)
        //{
        //    _NodeList[Number] = (new Node(Number, TrackNumber1));
        //    _listOfTracks[TrackNumber1].AssignNumberOfConnectedNode(Number);
        //    ActiveNode = Number;
        //}

        public void AddNodeToCityMap(int Number, int TrackNumber1, int TrackNumber2)
        {
            _NodeList[Number]=(new Node(Number,TrackNumber1,TrackNumber2));

            _listOfTracks[TrackNumber1].AssignNumberOfConnectedNode(Number,_NodeList[Number]);
            _listOfTracks[TrackNumber2].AssignNumberOfConnectedNode(Number,_NodeList[Number]);
            ActiveNode = Number;
        }
        public void AddNodeToCityMap(int Number, int TrackNumber1, int TrackNumber2, int TrackNumber3)
        {
            _NodeList[Number] = (new Node(Number, TrackNumber1, TrackNumber2,TrackNumber3));
            _listOfTracks[TrackNumber1].AssignNumberOfConnectedNode(Number, _NodeList[Number]);
            _listOfTracks[TrackNumber2].AssignNumberOfConnectedNode(Number, _NodeList[Number]);
            _listOfTracks[TrackNumber3].AssignNumberOfConnectedNode(Number, _NodeList[Number]);
            ActiveNode = Number;
        }
        public void AddNodeToCityMap(int Number, int TrackNumber1, int TrackNumber2, int TrackNumber3,int TrackNumber4)
        {
            _NodeList[Number] = (new Node(Number, TrackNumber1, TrackNumber2, TrackNumber3,TrackNumber4));
            _listOfTracks[TrackNumber1].AssignNumberOfConnectedNode(Number, _NodeList[Number]);
            _listOfTracks[TrackNumber2].AssignNumberOfConnectedNode(Number, _NodeList[Number]);
            _listOfTracks[TrackNumber3].AssignNumberOfConnectedNode(Number, _NodeList[Number]);
            _listOfTracks[TrackNumber4].AssignNumberOfConnectedNode(Number, _NodeList[Number]);
            ActiveNode = Number;
        }

        public CityDataStorage DeepCopyCity()
        {
            CityDataStorage City = (CityDataStorage)this.MemberwiseClone();

            City._NodeList=new Node[City._NodeList.Length];
            for (int i=0; i < this._NodeList.Length; i++)
            {
                if(_NodeList[i]!=null)
                City._NodeList[i] = _NodeList[i].DeepCopy();
            }

            City._listOfTracks=new Track[this._listOfTracks.Length];
            for (int i = 0; i < this._listOfTracks.Length; i++)
            {
                if (_listOfTracks [i] != null)
                    City._listOfTracks[i] = _listOfTracks[i].DeepCopy();
            }

            City._StationList = new Station[this._StationList.Length];
            for(int i=0; i< this._StationList.Length; i++)
            {
                if(_StationList[i]!=null)
                    City._StationList[i]= _StationList[i].DeepCopy();
            }

            return City;
        }

        public void CalculateDirectionalityOfTracksInCity()
        {
            foreach (Track T in _listOfTracks)
            {
                if (T != null)
                {
                    if (T.IsDirectionDefined() == true)
                        NumberOfTracksWithDefinedDirectionality++;
                }
            }

            while (NumberOfTracksWithDefinedDirectionality < NumberOfTracks())
            {
                foreach (Track Tr in _listOfTracks)
                {
                    if (Tr != null)
                    {
                        if (Tr.IsDirectionDefined() == false)
                        {
                            ListOfNodesConnectedToEvaluatedTrack = Tr.GetNumbersOfConnectedNodes();
                        }
                        else
                            continue;
                    }
                    else
                        continue;

                    for (int i = 0; i <= 1; i++)
                    {

                        EvaluatedNodeForTrackWhichIsHavingDirectionEstablished = ListOfNodesConnectedToEvaluatedTrack[i];
                        NodeOnOthereSideOfEvaluatedNode = ListOfNodesConnectedToEvaluatedTrack[1 - i];

                        ListOfTracksConnectedToEvaluatedNode = _NodeList[EvaluatedNodeForTrackWhichIsHavingDirectionEstablished].GetNumbersOfAllConnectedTracks();
                        //remove from that list track which is leading to start or stop if that is an intersection 
                        if(ListOfTracksConnectedToEvaluatedNode.Count>2)
                            remove_track_leading_to_start_from_list(ListOfTracksConnectedToEvaluatedNode);

                        ListOfTracksConnectedToEvaluatedNode.Remove(Tr.Number);
                        TrackFromWhichDirectionalityIsCopied = _listOfTracks[ListOfTracksConnectedToEvaluatedNode[0]];


                        //tu brakuje przejscia przez skzyzowanie jesli juz 2 kierunki sa ustalone
                        //wymaga to szczegolego przypadku kiedy w skrzyzowaniu 3 , 2 ustalone kierunki sa zgodnie
                        //to oznacza ze 3 kierunek musi byc przeciwny

                        //co z skzyzowaniem 4 np jesli dodam w przyzszlosci obsluge przeciecia torow ?
                        if (ListOfTracksConnectedToEvaluatedNode.Count == 1 & TrackFromWhichDirectionalityIsCopied.IsDirectionDefined())
                        {
                            WorkDirectionalNodeNumber = TrackFromWhichDirectionalityIsCopied.GetDirectionalNode();

                            if (WorkDirectionalNodeNumber == EvaluatedNodeForTrackWhichIsHavingDirectionEstablished)
                            {
                                Tr.SetDirectionalNode(NodeOnOthereSideOfEvaluatedNode);
                                NumberOfTracksWithDefinedDirectionality++;
                                break;
                            }
                            else
                            {
                                Tr.SetDirectionalNode(EvaluatedNodeForTrackWhichIsHavingDirectionEstablished);
                                NumberOfTracksWithDefinedDirectionality++;
                                break;
                            }
                        }
                    }
                }
            }

            void remove_track_leading_to_start_from_list(List<int> List_of_tracks)
            {
                for(int i = 0; i < List_of_tracks.Count; i++)
                {
                    if (_listOfTracks[List_of_tracks[i]].Is_this_track_leading_to_station)
                    {
                        List_of_tracks.Remove(List_of_tracks[i]);
                        break;

                    }
                }
                //return List_of_tracks;
            }
        }

        public void CalculateSpeedLimitForTracks()
        {
            foreach (Track T in _listOfTracks)
            {
                if (T != null)
                {

                    if (T.IsTrackSpeedLimitDefined() == true)
                        WorkSpeedLimit = T.GetSpeedLimit();

                    if (ListOfAllSpeedLimitsInCity.Contains(WorkSpeedLimit) == false)
                    {
                        ListOfAllSpeedLimitsInCity.Add(WorkSpeedLimit);
                    }
                }
            }

            ListOfAllSpeedLimitsInCity.Sort();
            ListOfAllSpeedLimitsInCity.Reverse();
            //Console.WriteLine("list Predkosci");
            //for (int i = 0; i < ListOfAllSpeedLimitsInCity.Count; i++)
            //{
            //    Console.WriteLine(ListOfAllSpeedLimitsInCity[i]);
            //}


            for (int s = 0; s < ListOfAllSpeedLimitsInCity.Count; s++)
            {
                SpeedUnderConsideration = ListOfAllSpeedLimitsInCity[s];
                IsGivenRoundOfCalculatingSpeedSuccesfull = true;

                while (IsGivenRoundOfCalculatingSpeedSuccesfull)
                {
                    IsGivenRoundOfCalculatingSpeedSuccesfull = false;

                    foreach (Track Tr in _listOfTracks)
                    {
                        if (Tr != null)
                        {
                            if (Tr.GetSpeedLimit() != SpeedUnderConsideration)
                                continue;

                            if (Tr.IsTrackSpeedLimitDefined() != true)
                                continue;

                            var TrackConnectedToTrack = this.ListAllTracksConnectedToGivenTrack(Tr.Number);

                            foreach (int n in TrackConnectedToTrack)
                            {
                                if (_listOfTracks[n].IsTrackSpeedLimitDefined() == false)
                                {
                                    _listOfTracks[n].SetSpeedLimit(SpeedUnderConsideration);
                                    IsGivenRoundOfCalculatingSpeedSuccesfull = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CalculatePositionOfNodes(bool check)
        {
            // robie liste trackow ktore niemaja zdefiniowanej pozycji obu nodow ale niesa referencyjne
            // robie liste trackow referencyjnych

            var list_of_tracks_with_not_defined_nodes_position = new List<Track>();
            var list_of_ref_tracks = new List<Track>();
            for (int i = 0; i < NumberOfTracks(); i++)
                if (_listOfTracks[i].Reference_track_during_city_def)
                    list_of_ref_tracks.Add(_listOfTracks[i]);
                else
                    list_of_tracks_with_not_defined_nodes_position.Add(_listOfTracks[i]);


            // dobrze dodac tutaj jakies sprawdzenie, np czy wszyskie tracki maja 2 nody np 
            // takie podstawe sanity check czy popelnilem bledy podczas definiowania miasta
            // czesc checku powinna byc we wczesniejszyf fazach tworzenia mista

            int index = 0;
            while (true)
            {

                if(list_of_tracks_with_not_defined_nodes_position.Count == 0) break;
                //nothing is defined
                if (list_of_tracks_with_not_defined_nodes_position[index].no_node_have_def_position())
                {
                    // go to next node
                    index++;

                    continue;
                }

                //one node is defined
                if (list_of_tracks_with_not_defined_nodes_position[index].one_and_only_one_node_with_defined_position())
                {
                    // calculate position of node 2
                    list_of_tracks_with_not_defined_nodes_position[index].calculate_position_of_not_defined_node();
                    // remove track from list
                    list_of_tracks_with_not_defined_nodes_position.RemoveAt(index);
                    // reset index
                    index = 0;
                    continue;
                }

                //two nodes are defined
                if (list_of_tracks_with_not_defined_nodes_position[index].both_attached_nodes_have_defined_position())
                {
                    // remove track from list
                    list_of_tracks_with_not_defined_nodes_position.RemoveAt(index);
                    // reset index
                    index = 0;
                    continue;
                }
            }

            //dla ref nodow licze pozycje i ja wyswietlam i licze wynikowa translacje i je wyswietlam i porownuje do tego co bylo zaplanowane 
            if(check)
            for (int i = 0; i < list_of_ref_tracks.Count; i++)
            {
                list_of_ref_tracks[i].calculate_check_for_ref_track();
                // dla kazdego tracku tutaj odpalam sprawdzenie. sprawdzenie jest zdefinowane w tracku 
                // i redefiniuje geometrie do wartoscu wynikowych
                // wiec jesli odchylki beda male to to maiasto badzie dzialac 
            }


                // jade po kolei przez tracki i
                // jesli dwa nody maja zdefiniowana pozycje to usuwam track z listy
                // jesli jeden node ma zdefiniowana pozycje to licze pozycje drugiego
                // i usuwam ten track z listy
                // jak niz niejest zdefiniowane to jede dalej
                // jade tak az do konca

                // pytanie jak zrobic krazenia w kolko po liscie ktora dynamicznie zmienia dlugosc 
                // ide z indexem w gore az trafie na taki co juz ma 2 objekty zdefiniowany lub jeden i zeruje index

                // dla ref trackow, wyswietlam info o tym jak wyglada pozycja i dlugosc w stosunku do tego co bylo zaplanowane  

                // gotowe

        }

        public void define_position_of_node(int number, float x, float y)
        {
            _NodeList[number].define_position(x, y);
        }

        public int GetNumberOfDefinedNodes()
        {
            int numberOfNodes = -1;
            for (int i = 0; i < 1000; i++)
            {
                if(_NodeList[i]==null)
                {
                    numberOfNodes = i;
                    break;
                }
            }
            return numberOfNodes;
        }
        public List<int> GetListOfAllTracksConnectedToNode(int NodeNumber)
        {
            return _NodeList[NodeNumber].GetNumbersOfAllConnectedTracks();
        }
        public (bool is_node_an_intersection, int type) is_node_an_intersection_plus_type(int node_number)
        {
            bool is_node_an_intersection = false;
            int intersection_type;
            var connected_tracks = _NodeList[node_number].GetNumbersOfAllConnectedTracks();
            if(connected_tracks.Count <= 2)
            {
                return (is_node_an_intersection, -1);  //not an intersection, -1 not an intersection type
            }
            else
            {
                is_node_an_intersection=true;
                intersection_type=check_type_of_intersection(node_number,connected_tracks);

                return(is_node_an_intersection, intersection_type);
            }


        }
        public bool is_node_an_intersection(int node_number)
        {
            var connected_tracks = _NodeList[node_number].GetNumbersOfAllConnectedTracks();
            if (connected_tracks.Count <= 2)
            {
                return false;  //not an intersection, -1 not a intersection type
            }
            else
            {
                return true;
            }


        }

        private int check_type_of_intersection(int node, List<int> connected_tracks)
        {
            //1 convergence. 2 in 1 out
            //2 divergence. 1 in 2 out
            //3 more types currently not available
            //-1 error
            int number_of_in = 0;
            int number_of_out = 0;
            int dir_noude;
            for (int i = 0;i < connected_tracks.Count; i++)
            {
                dir_noude = _listOfTracks[connected_tracks[i]].GetDirectionalNode();
                if(dir_noude==node)
                    number_of_out += 1;
                else 
                    number_of_in += 1;
            }

            if(number_of_in==2 & number_of_out == 1)
            {
                return 1;
            }
            else if(number_of_in == 1 & number_of_out == 2)
            {
                return 2;
            }
            else
                return -1;

        }
        public void DisconnectNodeFromTrack(int TrackNumber,int NodeNumber)
        {
            //remove from list of attached nodes for Track
            _listOfTracks[TrackNumber].RemoveNodeConnectedToTrack(NodeNumber);
            //remove track from list of attached track for node
            _NodeList[NodeNumber].DisconnectTrack(TrackNumber);
        }
        public List<int> get_numbers_of_attached_nodes_for_track(int track)
        {
            return _listOfTracks[track].GetNumbersOfConnectedNodes();
        }
        // attach existing node to track
        public void AttachNodeToTrack(int NodeNumber, int TrackNumber)
        {
            //add node to list of attached nodes for track
                _listOfTracks[TrackNumber].AssignNumberOfConnectedNode(NodeNumber, _NodeList[NodeNumber]);
                //add track to list of attached tracks for node
            _NodeList[NodeNumber].ConnectTrack(TrackNumber);
        }


        public int GetNodeNumberOnTheOthereSideOfTrack(int TrackNumber,int Node)
        {
            return _listOfTracks[TrackNumber].NodeOnOthereSideOfTrack(Node);
        }
        public (int number_of_track, int error_code) get_number_of_next_track_along_traffic_direction(int trackNumber)
        {
            int error = 0;
            var node1 = _listOfTracks[trackNumber].GetDirectionalNode();
            node1 = GetNodeNumberOnTheOthereSideOfTrack(trackNumber, node1);
            var List = _NodeList[node1].GetNumbersOfAllConnectedTracks();
            List.Remove(trackNumber);
            if (List.Count > 1)
            {
                error = 1; // next track alng the way in city is after intersection
            }
            return (List[0], error);
        }
        public (int number_of_track, int error) get_number_of_next_track_the_wrong_way(int trackNumber)
        {
            int error = 0;
            var node1 = _listOfTracks[trackNumber].GetDirectionalNode();
            var List = _NodeList[node1].GetNumbersOfAllConnectedTracks();
            List.Remove(trackNumber);
            if (List.Count > 1)
            {
                error = -1; // next track alng the way in city is after intersection
            }
            return (List[0], error);
        }
        
        public void AddStraightTrackElementToCityMap(int number,double Length, double Angle)
        {
            _listOfTracks[number]=new Track(number, Length, Angle);
        }

        public void AddStraightRefTrackElementToCityMap(int number, double Length, double Angle)
        {
            var track = new Track(number, Length, Angle);
            track.Reference_track_during_city_def = true;

            _listOfTracks[number] = track;
        }

        public void AddCurvedTrackElementToCityMap(int number, double Length, double Angle)
        {

        }

        public int AddStraightTrackElementToCityMap_With_Next_Number(double Length, double Angle)
        {
            //check what is next number of track
            int number = GetNumberOfTracksDefinedInCity();

            AddStraightTrackElementToCityMap(number, Length , Angle);

            return number;
        }

        public int AddCurvedTrackElementToCityMap_With_Next_Number(double Length, double Angle)
        {
            //check what is next number of track
            int number = GetNumberOfTracksDefinedInCity();

            AddCurvedTrackElementToCityMap(number, Length, Angle);

            return number;
        }

        public double GetLenghtOfTrack(int TrackNumber)
        {
            return _listOfTracks[TrackNumber].Length;
        }
        public void ChangeLengthOfTrack(int TrackNumber,double NewLength)
        {
            _listOfTracks[TrackNumber].ChangeLength(NewLength);
        }

        public void Shorten_track_length(int TrackNumber, double Shorten_by)
        {
            var current_length = _listOfTracks[TrackNumber].Length;

            _listOfTracks[TrackNumber].ChangeLength(current_length - Shorten_by);
        }
        public void DefineSpeedForTrack_Km_h(int TrackNumber, double Speed)
        {
            double Speed_Przeliczone = (Speed * 10) / 36;
            _listOfTracks[TrackNumber].SetSpeedLimit(Speed_Przeliczone);
        }
        public void DefineSpeedForTrack_m_s(int TrackNumber, double Speed)
        {
            _listOfTracks[TrackNumber].SetSpeedLimit(Speed);
        }
        public void DefineDirectionForTrack(int TrackNumber, int DirectionalNode)
        {
            _listOfTracks[TrackNumber].SetDirectionalNode(DirectionalNode);
        }
        public int GetDirectionalityNodeForTrack(int TrackNumber)
        {
            return _listOfTracks[TrackNumber].GetDirectionalNode();
        }
        public void RemoveDirectionDefinitionForAffectedTrack(int TrackNumber)
        {
            int DirNodeNumber;
            DirNodeNumber = _listOfTracks[TrackNumber].GetDirectionalNode();
            _listOfTracks[TrackNumber].RemoveDirectinalNodeDefinition();
            //_TrackList[TrackNumber].RemoveNodeConnectedToTrack(DirNodeNumber);
            //var Lista = _NodeList[DirNodeNumber].GetNumbersOfAllConnectedTracks();
            //_TrackList[Lista[0]].RemoveNodeConnectedToTrack(DirNodeNumber);
        }
        public int GetNumberOfTracksDefinedInCity()
        {
            int numberOfTracks = -1;
            for (int i = 0; i < 1000; i++)
            {
                if (_listOfTracks[i] == null)
                {
                    numberOfTracks = i;
                    break;
                }
            }
            return numberOfTracks;
        }
        public void AddStationToCityMap(int Number, int NumberOfNodeToAttachTo)
        {
            _StationList[Number] = new Station(Number,NumberOfNodeToAttachTo);
        }
        public void AddStationToCityMap(int Number, int NumberOfNodeToAttachTo, double min_arrival_separation)
        {
            _StationList[Number] = new Station(Number, NumberOfNodeToAttachTo, min_arrival_separation);
        }
        public void AddStationToCityMap(int Number, int NumberOfNodeToAttachTo, double min_arrival_separation, double min_departure_separation)
        {
            _StationList[Number] = new Station(Number, NumberOfNodeToAttachTo ,min_arrival_separation, min_departure_separation);
        }
        
        public (int number, int error_code) Get_number_of_node_along_traffic(int node_number)
        {

            if (is_node_a_divergence(node_number)) // that means: it is a divergence intersection so there is no way to tell which way to go
                return (-1, 1);
            else if (is_node_a_connector_node(node_number) | is_node_a_convergence(node_number))
            {
                var list = GetListOfAllTracksConnectedToNode(node_number);
                var track = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (_listOfTracks[list[i]].GetDirectionalNode() == node_number)
                    {
                        track = list[i];
                        break;
                    }
                }

                var node = _listOfTracks[track].NodeOnOthereSideOfTrack(node_number);
                return (node, 0);
            }

            else
                return (-1, 2); // unknown exception error
        }
        
        public bool is_node_a_station_node(int node)
        {
            int numer_of_stations = NumberOfStations();
           for(int i = 0; i < numer_of_stations; i++)
           {
                if (_StationList[i].NumberOfAttachedNode == node)
                {
                    return true;
                }

           }
           return false;
        }
        
        public double get_station_departure_separation(int node)
        {
            return _StationList[node].min_separation_of_departing_traverses;
        }
        
        public double get_station_arrival_separation_by_node(int node)
        {
            for (int i = 0; i < NumberOfStations(); i++)
            {
               if(_StationList[i].NumberOfAttachedNode == node)
               {
                   return _StationList[i].min_separation_of_arriving_traverses;


               }
            }
            return 0;

        }
        private bool is_node_a_connector_node(int number)
        {
            var list = GetListOfAllTracksConnectedToNode(number);

            if (list.Count == 2)
                return true;
            else
                return false;
        }
        private bool is_node_a_divergence(int number)
        {
            var list = GetListOfAllTracksConnectedToNode(number);

            if (list.Count != 3)
                return false;
            else
            {
                var number_of_out_tracks_from_intersection_node = count_number_of_out_track(number);

                if (number_of_out_tracks_from_intersection_node == 2)
                    return true;
                else
                    return false;

            }
        }
        private int count_number_of_out_track(int node_number)
        {
            var number_of_out_tracks_from_intersection_node = 0;
            var list = GetListOfAllTracksConnectedToNode(node_number);
            foreach (int n in list)
            {
                if (GetDirectionalityNodeForTrack(n) == node_number)
                {
                    number_of_out_tracks_from_intersection_node += 1;
                }
            }
            return number_of_out_tracks_from_intersection_node;
        }
        private bool is_node_a_convergence(int number)
        {
            var list = GetListOfAllTracksConnectedToNode(number);

            if (list.Count != 3)
                return false;
            else
            {
                var number_of_out_tracks_from_intersection_node = count_number_of_out_track(number);

                if (number_of_out_tracks_from_intersection_node == 1)
                    return true;
                else
                    return false;

            }
        }
        public Node[] GetListOFAllNodesInCity()
        {
            return _NodeList;
        }
        public Track[] GetListOfAllTracksInCity()
        {
            return _listOfTracks;
        }
        public Station[] GetListOfAllStationsInCity()
        {
            return _StationList;
        }
        public bool DoAllNodeHavePositionsDefined()
        {
            
            return true;
        }
        
        public bool DoAllTreackHaveDirectionalityDefined()
        {
            //missing implementation
            return true;
        }
        public bool DoAllTrackHaveSpeedLimit()
        {
            //missing implementation
            return true;
        }
        public bool DoAllTrackHaveTwoNodesAttached()
        {
            //missing implementation
            return true;
        }
        public bool CheckIfTwoNodesArDefinedForTrack(int TrackNumber)
        {
            return _listOfTracks[TrackNumber].AreBothNodesAttached();
        }
        public bool CheckIfAllTrackhaveTwoNodesAttached()
        {
             bool AreAllTracksAttached = true;
            foreach(Track T in _listOfTracks) 
            {
                if (T.AreBothNodesAttached() == false)
                {
                    AreAllTracksAttached = false;
                    break;
                }
            }

            return AreAllTracksAttached;
        }
        public List<int> ListAllTracksConnectedToGivenTrack(int TrackNumber)
        {
            var NodesConnectedToTrack = new List<int>(); 
            NodesConnectedToTrack.Add(_listOfTracks[TrackNumber].Node1_number);
            NodesConnectedToTrack.Add(_listOfTracks[TrackNumber].Node2_number);

            var TrackConnectedToNode1 = _NodeList[NodesConnectedToTrack[0]].GetNumbersOfAllConnectedTracks();
            var TrackConnectedToNode2 = _NodeList[NodesConnectedToTrack[1]].GetNumbersOfAllConnectedTracks();

            var AllTracksConnectedToTrack = new List<int>();

            foreach(int track in TrackConnectedToNode1)
            {
                if (AllTracksConnectedToTrack.Contains(track) == false)
                    AllTracksConnectedToTrack.Add(track);
            }

            foreach (int track in TrackConnectedToNode2)
            {
                if (AllTracksConnectedToTrack.Contains(track) == false)
                    AllTracksConnectedToTrack.Add(track);
            }

            AllTracksConnectedToTrack.Remove(TrackNumber);

            return AllTracksConnectedToTrack;
        }
        public double GetSpeedLimitForTrack_m_s (int TrackNumber)
        {
            return _listOfTracks[TrackNumber].GetSpeedLimit();
        }
        
        public (float,float) calculate_position_of_point_at_x_from_start_of_track(int trackNumber, double x)
        {
            var(pos_x,pos_y) =_listOfTracks[trackNumber].calculate_position_of_point_at_x_from_start(x);
            return (pos_x, pos_y);
        }

        public (float, float ,float) calculate_position_and_angle_of_point_at_x_from_start_of_track(int track_number, double x)
        {
            var (pos_x, pos_y, ang) = _listOfTracks[track_number].calculate_position__and_tangent_angle_of_point_at_x_from_start(x);

            return (pos_x, pos_y, ang);
        }

        public (float,float) calculate_position_of_point_at_x_from_end_of_track(int trackNumber, double x)
        {
            var (pos_x, pos_y) = _listOfTracks[trackNumber].calculate_position_of_point_at_x_from_end(x);
            return (pos_x, pos_y);
        }

        public void redefine_track_geometry_by_moving_start_point_by_x(int track, double x)
        {
            _listOfTracks[track].redefine_geometry_by_moving_start_with_new_point_at_x_from_start_side(x);
        }

        public void redefine_track_geometry_by_moving_end_point_by_x(int track, double x)
        {
            _listOfTracks[track].redefine_geometry_by_moving_end_with_new_point_at_x_from_end_side(x);
        }

        public (float,float) get_construction_geomertu_data_for_track(int track)
        {
            var(len, ang) = _listOfTracks[track].geometry.return_con_angle_and_con_length();
            return (len, ang);
        }

        public int NumberOfNodes()
        {
            int j = 0;
            for (int i = 0; i < _NodeList.Length; i++)
            {
                if (_NodeList[i] == null)
                    return i;
            }
            return j;
        }
        public int NumberOfTracks()
        {
            int j = 0;
            for(int i = 0; i < _listOfTracks.Length; i++)
            {
                if (_listOfTracks[i] == null)
                    return i;
            }
            return j;
        }
        public int NumberOfStations()
        {
            int j = 0;
            for (int i = 0; i < _StationList.Length; i++)
            {
                if (_StationList[i] == null)
                    return i;
            }
            return j;
        }
       
        public void add_permutation_to_track(int track, int permutation)
        {
            _listOfTracks[track].AddPermutationToList(permutation);
        }
        public List<int> get_list_of_permutations_number_for_track(int track)
        {
            return _listOfTracks[track].GetListOfPermutations();
        }
        public bool is_section_for_track_defined(int track)
        {
            return _listOfTracks[track].is_section_number_attached();
        }
        public void add_section_to_track(int track, int section)
        {
            _listOfTracks[track].set_number_of_section(section);
        }
        public int get_number_of_section_for_track(int track)
        {
            return _listOfTracks[track].get_number_of_section();
        }
        public int get_section_number_of_track(int track_number)
        {
            return _listOfTracks[track_number].SectionNumber;
        }
        public int get_node_attached_to_station(int station_number)
        {
            return _StationList[station_number].NumberOfAttachedNode;
        }
        public int get_number_of_node_on_the_othere_side_of_track(int track,int node)
        {
            return _listOfTracks[track].NodeOnOthereSideOfTrack(node);
        }
        public void mark_track_as_acces_to_station(int number)
        {
            _listOfTracks[number].Is_this_track_leading_to_station = true;
        }
        /// <summary>
        /// Works only for connector nodes with two tracks connected. otherevise return error
        /// </summary>
        /// <param name="node"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        public int get_number_of_track_also_connected_to_node(int node, int track)
        {
            return _NodeList[node].get_track_also_connected_to_node(track);
        }
    }

}
