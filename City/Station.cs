namespace Symulation
{
    public class Station
    {
        public Station(int StationNumber,int NodeNumberToAttachStationTo)
        {
            Number = StationNumber;
            NumberOfAttachedNode = NodeNumberToAttachStationTo;
        }

        public Station(int StationNumber, int NodeNumberToAttachStation, double min_separation_of_arriving_traverses)
        {
            Number = StationNumber;
            NumberOfAttachedNode = NodeNumberToAttachStation;
            this.min_separation_of_arriving_traverses = min_separation_of_arriving_traverses;
        }

        public Station(int StationNumber, int NodeNumberToAttachStation, double min_separation_of_arriving_traverses, double min_separation_of_departing_traverses)
        {
            Number = StationNumber;
            NumberOfAttachedNode = NodeNumberToAttachStation;
            this.min_separation_of_arriving_traverses = min_separation_of_arriving_traverses;
            this.min_separation_of_departing_traverses = min_separation_of_departing_traverses;
        }

        public int Number { get;}
        public int NumberOfAttachedNode { get; }

        public double min_separation_of_arriving_traverses { get; set; }
        public double min_separation_of_departing_traverses { get; set; }


        public Station DeepCopy()
        {
            var new_station = new Station(this.Number, this.NumberOfAttachedNode);
            return new_station;
        }

    }
}
