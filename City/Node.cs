using System.Collections.Generic;

namespace Symulation
{
    public class Node
    {
        private int _connectedTrackNumber1;
        private int _connectedTrackNumber2;
        private int _connectedTrackNumber3;
        private int _connectedTrackNumber4;

        //private int _Dir1;
        //private bool _Dir1A;
        //private int _Dir2;
        //private bool _Dir2A;
        //private int _Dir3;
        //private bool _Dir3A;
        //private int _Dir4;
        //private bool _Dir4A;

        private bool _IsTrack1Connected;
        private bool _IsTrack2Connected;
        private bool _IsTrack3Connected;
        private bool _IsTrack4Connected;
        //private bool _IsPositionOfNodeEstablished;

        private double _positionX;
        private double _positionY;
        private bool _is_position_defined;

        //private bool AllConectionsAreAllowed;
        //private int [][] AllowedConnections= new int[2][];
        

        public int Number { get; set; }
        
       

        public Node(int NodeNumber)
        {
            Number = NodeNumber;
            _is_position_defined = false;
        }


        public Node(int NodeNumber, int NumerToru1, int NumerToru2)
        {
            this.Number = NodeNumber;
            _connectedTrackNumber1 = NumerToru1;
            _connectedTrackNumber2 = NumerToru2;
            _IsTrack1Connected = true;
            _IsTrack2Connected = true;
            _is_position_defined = false;
           
        }
        public Node(int NodeNumber, int NumerToru1, int NumerToru2, int NumerToru3)
        {
            this.Number = NodeNumber;

            _connectedTrackNumber1 = NumerToru1;
            _connectedTrackNumber2 = NumerToru2;
            _connectedTrackNumber3 = NumerToru3;
            _IsTrack1Connected = true;
            _IsTrack2Connected = true;
            _IsTrack3Connected = true;

            _is_position_defined = false;
        }
        public Node(int NodeNumber, int NumerToru1, int NumerToru2, int NumerToru3, int NumerToru4)
        {
            this.Number = NodeNumber;
            _connectedTrackNumber1 = NumerToru1;
            _connectedTrackNumber2 = NumerToru2;
            _connectedTrackNumber3 = NumerToru3;
            _connectedTrackNumber4 = NumerToru4;
            _IsTrack1Connected = true;
            _IsTrack2Connected = true;
            _IsTrack3Connected = true;
            _IsTrack4Connected = true;

            _is_position_defined = false;
        }

        public Node DeepCopy()
        {
            Node new_node = (Node)this.MemberwiseClone();

            new_node.Number = this.Number;
            new_node._connectedTrackNumber1 = this._connectedTrackNumber1;
            new_node._connectedTrackNumber2 = this._connectedTrackNumber2;
            new_node._connectedTrackNumber3 = this._connectedTrackNumber3;
            new_node._connectedTrackNumber4 = this._connectedTrackNumber4;

            new_node._IsTrack1Connected = this._IsTrack1Connected;
            new_node._IsTrack2Connected = this._IsTrack2Connected;
            new_node._IsTrack3Connected = this._IsTrack3Connected;
            new_node._IsTrack4Connected = this._IsTrack4Connected;


            return new_node;
        }

        public void ConnectTrack(int TrackNumber)
        {
            if (_IsTrack1Connected == false)
            {
                _connectedTrackNumber1 = TrackNumber;
                _IsTrack1Connected = true;
            }
            else if(_IsTrack2Connected == false)
            {
                _connectedTrackNumber2 = TrackNumber;
                _IsTrack2Connected = true;
            }
            else if (_IsTrack3Connected == false)
            {
                _connectedTrackNumber3 = TrackNumber;
                _IsTrack3Connected = true;
            }
            else if (_IsTrack4Connected == false)
            {
                _connectedTrackNumber4 = TrackNumber;
                _IsTrack4Connected = true;
            }
            else
                System.Console.WriteLine("Error, allready 4 track attached to Node");

        }
        public void DisconnectTrack(int TrackNumber)
        {
            if (_connectedTrackNumber1 == TrackNumber)
                _IsTrack1Connected = false;
            else if (_connectedTrackNumber2 == TrackNumber)
                _IsTrack2Connected = false;
            else if (_connectedTrackNumber3 == TrackNumber)
                _IsTrack3Connected = false;
            else if (_connectedTrackNumber4 == TrackNumber)
                _IsTrack4Connected = false;
            else
                System.Console.WriteLine("Error, specyfied track not attached to node");
        }

        public List<int> GetNumbersOfAllConnectedTracks()
        {
            var AllConnections = new List<int>();
            if (_IsTrack1Connected==true)
                AllConnections.Add(_connectedTrackNumber1);
            if (_IsTrack2Connected==true)
                AllConnections.Add(_connectedTrackNumber2);
            if (_IsTrack3Connected==true)
                AllConnections.Add(_connectedTrackNumber3);
            if (_IsTrack4Connected==true)
                AllConnections.Add(_connectedTrackNumber4);

            return AllConnections;
        }

        public int get_number_of_connected_tracks()
        {
            var list = GetNumbersOfAllConnectedTracks();
            return list.Count;
        }
        
        public void define_position(float x,float y)
        {
            _positionX = x;
            _positionY = y;
            _is_position_defined=true;
        }

        public (float,float) return_node_position()
        {
            return ((float)_positionX, (float)_positionY);
        }

        public bool is_position_defined()
        {
            return _is_position_defined;
        }

        /// <summary>
        /// Work only for connector node. So so node which are connected to two tracks 
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public int get_track_also_connected_to_node(int track_number)
        {

            if (get_number_of_connected_tracks() > 2)
                return -2;

            var connected_tracks = GetNumbersOfAllConnectedTracks();

            if (!connected_tracks.Contains(track_number))
                return -1;

            connected_tracks.Remove(track_number);
            return connected_tracks[0];
            
        }
    }
}
