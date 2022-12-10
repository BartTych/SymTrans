using System;
using System.Collections.Generic;
using System.Numerics;

namespace Symulation
{
    public class Track
    {
        public int Node1_number { get; set; }
        public Node Node1 { get; set; }

        private bool IsNode1Attached;

        public int Node2_number { get; set; }
        public Node Node2 { get; set; }

        private bool IsNode2Attached;

        
        public int Number { get; set; }
        // public double ConstructionLength { get; set; } //to jest odleglosc pomiadzy noudami w lini prostej. Info do ustalania pozycji noudow w mapie miasta.
        // wrzucilem to do geometrii 
        public double Length { get; set; } // to jest rzeczywista dlugosc tracku, ta wartosc powinnna byc docelowo obslugiwana przez object geometria
                                           // narazie jest generowane przez geometrie niewiem czy to wystarczy
                                           // napewno powinienem zmieniac geometrie w modyfikowanych trackach i generowac geometri dla nowo tworzonych trackow
                                           // w modach 
                                           // w modach track jest dodawany tylko w 2 metodach wiec niepowinno byc z tym problemu
                                           // prawdopodobnie nigdzie indziej nielastepuje zmiana dlugosci / geometri
                                           // ta metoda jest uzywana w wielu miejscach a;e nierobi to problemu

        public TrackGeometry geometry { get; set; }

        public bool Reference_track_during_city_def { get; set; }
        public bool Is_this_track_leading_to_station { get; set; }


        // jesli maja byc rozne rodzaje krzywizn jak to zrealizowac
        // moge tutaj dowac jakis rodzaj danych z opisem krzywizny
        // i wtedy metody liczace pozycje nowego noudu i liczace pozycje po drodze beda sie odwolywac do tego rodzaju danych 
        // jesli to zrobie odpowiednio to bede mogl deniniowac dowolnie krzywizne
        // problem w tym ze jesli bedzie zakret zdefiniowany i bede doawac nowy noude, to musze tez zmienic definicje w traku
        // ktory jest skracany, bo jesli to jest zakret to katy sie zmienia , itd
        // to moze sie okazac dosc skompikowane o nie!!!
        // musze to przemyslec zeby sie niewladowac w jakiesz badziewie



        public int DirectionalNode;
        private bool IsDirectionalNodeDefined;    //directionality node number is on the side from wich you can drive 

        public double SpeedLimit;
        private bool IsSpeedLimitDefined;

        public int SectionNumber;
        public bool IsSectionNumberAttached;

        private List<int> ListOfPermutations;


        // store node data of attached nodes
        // during modyfications 



        public Track(int TorNumber, double Length)
        {
            Number = TorNumber;
            this.Length = Length;
            ListOfPermutations = new List<int>();
            Reference_track_during_city_def = false;
        }

        /// <summary>
        /// ctor for track with straight geometry
        /// </summary>
        /// <param name="TorNumber"></param>
        /// <param name="construction_length"></param>
        /// <param name="construction_angle"></param>
        public Track(int TorNumber, double construction_length, double construction_angle)
        {
            Number = TorNumber;
            ListOfPermutations = new List<int>();
            var _geometry = new straight_geometry();
            _geometry._construction_length = construction_length;
            _geometry._construction_angle = construction_angle;
            geometry = _geometry;
            Length = geometry.calculate_real_length();
            Reference_track_during_city_def = false;
        }

        /// <summary>
        /// ctor for track with curved geometry, minus radius defines side 
        /// </summary>
        /// <param name="TorNumber"></param>
        /// <param name="construction_length"></param>
        /// <param name="construction_angle"></param>
        /// <param name="curve_radius"></param>
        public Track(int TorNumber, double construction_length, double construction_angle, float curve_radius)
        {
            Number = TorNumber;
            //dlugosc rzeczywista bedzie policzona zaleznie od promienia i odleglosci konstrukcyjnej 
            ListOfPermutations = new List<int>();
            var _geometry = new curved_geometry();
            _geometry._construction_length = construction_length;
            _geometry._construction_angle = construction_angle;
            _geometry._radius = curve_radius;
            geometry = _geometry;
            Length = geometry.calculate_real_length();
            Reference_track_during_city_def = false;
        }

        //public Track(int TorNumber)
        //{
        //    Number = TorNumber;
        //    ListOfPermutations = new List<int>();
        //}

        public Track DeepCopy()
        {
            Track new_track = (Track)MemberwiseClone();

            new_track.Node1_number = Node1_number;
            new_track.IsNode1Attached = IsNode1Attached;
            new_track.Node2_number = Node2_number;
            new_track.IsNode2Attached = IsNode2Attached;

            new_track.Number = Number;
            new_track.Length = Length;

            new_track.DirectionalNode = DirectionalNode;
            new_track.IsDirectionalNodeDefined = IsDirectionalNodeDefined;

            new_track.SpeedLimit = SpeedLimit;
            new_track.IsSpeedLimitDefined = IsSpeedLimitDefined;

            new_track.SectionNumber = SectionNumber;
            new_track.ListOfPermutations = new List<int>(ListOfPermutations);

            var (len, ang) = geometry.return_con_angle_and_con_length();
            var geo = new straight_geometry();
            geo._construction_length = len;
            geo._construction_angle = ang;
            new_track.geometry = geo;

            return new_track;
        }
        public void ChangeLength(double Length)
        {
            this.Length = Length;
        }
        public void AddPermutationToList(int permutation_number)
        {
            ListOfPermutations.Add(permutation_number);
        }
        public bool IsPermutationPresent(int permutation_number)
        {
            return ListOfPermutations.Contains(permutation_number);
        }
        public List<int> GetListOfPermutations()
        {
            return ListOfPermutations;
        }
        public int get_number_of_section()
        {
            return SectionNumber;
        }
        public void set_number_of_section(int section)
        {
            SectionNumber = section;
            IsSectionNumberAttached = true;
        }
        public bool is_section_number_attached()
        {
            return IsSectionNumberAttached;
        }

        public void AssignNumberOfConnectedNode(int Number)
        {
            if (IsNode1Attached == false)
            {
                Node1_number = Number;
                IsNode1Attached = true;
            }
            else if (IsNode2Attached == false)
            {
                Node2_number = Number;
                IsNode2Attached = true;
            }
            else
                System.Console.WriteLine("blad, proba dodania trzeciego Nodu-u do Track");
        }

        public void AssignNumberOfConnectedNode(int Number, Node node)
        {
            if (IsNode1Attached == false)
            {
                Node1_number = Number;
                Node1 = node;
                IsNode1Attached = true;
            }
            else if (IsNode2Attached == false)
            {
                Node2_number = Number;
                Node2 = node;
                IsNode2Attached = true;
            }
            else
                System.Console.WriteLine("blad, proba dodania trzeciego Nodu-u do Track");
        }

        /// <summary>
        /// Remove node connected to city
        /// </summary>
        /// <param name="Number"></param>
        public void RemoveNodeConnectedToTrack(int Number)
        {
            if (Node1_number == Number)
            {
                IsNode1Attached = false;
            }
            else if (Node2_number == Number)
            {
                IsNode2Attached = false;
            }
            else
                System.Console.WriteLine("blad usuniecia, podany node niejest przyczepiony do tracku");
        }
        public void SetDirectionalNode(int DirectionNode)
        {
            DirectionalNode = DirectionNode;
            IsDirectionalNodeDefined = true;
        }
        public void RemoveDirectinalNodeDefinition()
        {
            IsDirectionalNodeDefined = false;
        }
        public bool IsDirectionDefined()
        {
            return IsDirectionalNodeDefined;
        }
        public int GetDirectionalNode()
        {
            return DirectionalNode;
        }
        public void SetSpeedLimit(double Speed)
        {
            SpeedLimit = Speed;
            IsSpeedLimitDefined = true;
        }
        public double GetSpeedLimit()
        {
            return SpeedLimit;
        }
        public bool IsTrackSpeedLimitDefined()
        {
            return IsSpeedLimitDefined;
        }
        public List<int> GetNumbersOfConnectedNodes()
        {
            var Lista = new List<int>();
            Lista.Add(Node1_number);
            Lista.Add(Node2_number);
            return Lista;
        }
        public bool AreBothNodesAttached()
        {
            if (IsNode1Attached & IsNode2Attached)
                return true;
            else
                return false;
        }
        public bool both_attached_nodes_have_defined_position()
        {

            if (AreBothNodesAttached())
            {
                if (Node1.is_position_defined() && Node2.is_position_defined())
                {
                    return true;
                }
                else
                    return false;

            }
            else
            {
                return false;
            }
        }
        public bool one_and_only_one_node_with_defined_position()
        {
            if (Node1.is_position_defined() ^ Node2.is_position_defined())
            {
                return true;
            }
            else
                return false;
        }
        public bool no_node_have_def_position()
        {
            if (!Node1.is_position_defined() && !Node2.is_position_defined())
                return true;
            else
                return false;
        }
        public void calculate_position_of_not_defined_node()
        {
            int number_of_node_with_definition = -1;
            if (Node1.is_position_defined())
                number_of_node_with_definition = Node1.Number;
            if (Node2.is_position_defined())
                number_of_node_with_definition = Node2.Number;

            if (number_of_node_with_definition == DirectionalNode)
                calculate_position_of_not_directional_node_and_assing();

            if (number_of_node_with_definition != DirectionalNode)
                calculate_position_of_directional_node_and_assing();
        }
        public void calculate_check_for_ref_track()
        {
            //licze translacje jak byla zaplanowana 
            var (planed_x_trans, planned_y_trans) = geometry.calculate_translation_from_directional_node_to_end_node();
            var (planed_length, planed_ang) = geometry.return_con_angle_and_con_length();

            //licze translacje jaka wyszla na podstawie reszty trackow w danej sekcji
            var (effective_x_trans, effective_y_trans) = calculate_translation_based_on_nodes_position();
            var effective_length = calculate_length_based_on_nodes_positon();
            var effective_angle = calculate_angle_based_on_nodes_position();

            
            Console.WriteLine("Track ref numer: {0} planed x: {1} planed y: {2} effect x: {3} effect y: {4} ", Number, planed_x_trans, planned_y_trans, Math.Round(effective_x_trans, 2), Math.Round(effective_y_trans, 2));
            Console.WriteLine("difference x: {0}, difference y: {1}", Math.Round((planed_x_trans - effective_x_trans), 2), Math.Round((planned_y_trans - effective_y_trans)), 2);
            Console.WriteLine("planed length: {0} effective length {1}  planed angle {2}  effective angle {3}", planed_length, effective_length, planed_ang, effective_angle);
            Console.WriteLine("");


            if (geometry.what_is_the_type_of_geometry() == type_of_geometry.straight) //straight
            {
                straight_geometry geo = new straight_geometry();
                geo._construction_length = effective_length;
                geo._construction_angle = effective_angle;
                geometry = geo;
            }

            if (geometry.what_is_the_type_of_geometry() == type_of_geometry.curved) //curved
            {
                curved_geometry geo = new curved_geometry();
                geo._construction_length = effective_length;
                geo._construction_angle = effective_angle;
                geometry = geo;
            }

            Length = geometry.calculate_real_length();
            
            // dodatkowo jesli zaakceptuje to co sie dzieje na ref trackach to powinienem przypisac dla nich wynikowe wartosci jako bazowe do dalszej symulacji
            // co to oznacza
            // geometria powinna byc zmodyfikowana
            // odleglosc konstrukcyjna
            // kat konstrukcyjny
            // dlugosc rzeczywista wyliczona jeszcze raz juz na bazie nowej geometrii 
        }
        private (float,float) calculate_translation_based_on_nodes_position()
        {
            if (Node1.Number == DirectionalNode)
            {
                var (pos_1_x, pos_1_y) = Node1.return_node_position();
                var (pos_2_x, pos_2_y) = Node2.return_node_position();
                return (pos_2_x - pos_1_x, pos_2_y - pos_1_y);
            }

            if (Node2.Number == DirectionalNode)
            {
                var (pos_1_x, pos_1_y) = Node1.return_node_position();
                var (pos_2_x, pos_2_y) = Node2.return_node_position();
                return (pos_1_x - pos_2_x, pos_1_y - pos_2_y);
            }

            return (0, 0);
        }
        private float calculate_angle_based_on_nodes_position()
        {
            var (effective_x_trans, effective_y_trans) = calculate_translation_based_on_nodes_position();

            // twoze liczbe zespolona z x jako real i y jako im
            // i licze kat tej liczby
            var vector = new Complex(effective_x_trans, effective_y_trans);
            return (float)(vector.Phase * 180 / Math.PI);
        }
        private float calculate_length_based_on_nodes_positon()
        {
            var (pos_1_x, pos_1_y) = Node1.return_node_position();
            var (pos_2_x, pos_2_y) = Node2.return_node_position();

            return ((float)(Math.Pow(Math.Pow(Math.Abs(pos_1_x - pos_2_x), 2) + Math.Pow(Math.Abs(pos_1_y - pos_2_y), 2),0.5)));
        }
        private void calculate_position_of_not_directional_node_and_assing()
        {
            var (trans_x, trans_y) = geometry.calculate_translation_from_directional_node_to_end_node();


            //position of directional node
            if (Node1.Number != DirectionalNode)
            {
                var (pos_x, pos_y) = Node2.return_node_position();
                Node1.define_position(pos_x + trans_x, pos_y + trans_y);
            }
            if (Node2.Number != DirectionalNode)
            {
                var (pos_x, pos_y) = Node1.return_node_position();
                Node2.define_position(pos_x + trans_x, pos_y + trans_y);
            }

        }
        private void calculate_position_of_directional_node_and_assing()
        {
            var (trans_x, trans_y) = geometry.calculate_translation_from_directional_node_to_end_node();

            //position of directional node
            if (Node1.Number == DirectionalNode)
            {
                var (pos_x, pos_y) = Node2.return_node_position();
                Node1.define_position(pos_x - trans_x, pos_y - trans_y);
            }
            if (Node2.Number == DirectionalNode)
            {
                var (pos_x, pos_y) = Node1.return_node_position();
                Node2.define_position(pos_x - trans_x, pos_y - trans_y);
            }

        }
        public int NodeOnOthereSideOfTrack(int NodeOnSideWherePodIs)
        {
            //tu powinno byc sprawdzenie czy sa oba noudy wogule podlaczone 
            int NodeNumberOnOtherSideOfTrack;
            List<int> NodesConnectedToTrack;
            NodesConnectedToTrack = this.GetNumbersOfConnectedNodes();

            if (NodesConnectedToTrack[0] == NodeOnSideWherePodIs)
            {
                NodeNumberOnOtherSideOfTrack = NodesConnectedToTrack[1];
            }
            else
            {
                NodeNumberOnOtherSideOfTrack = NodesConnectedToTrack[0];
            }

            return NodeNumberOnOtherSideOfTrack;
        }

        /// <summary>
        /// Dziala tylko dla straigth geometry, wymaga przerobienia jak pojewi sie curved
        /// </summary>
        /// <param name="x"></param>
        public void redefine_geometry_by_moving_start_with_new_point_at_x_from_start_side(double x)
        {
            var (len, ang) = geometry.return_con_angle_and_con_length();
            var new_geo = new straight_geometry();
            new_geo._construction_length = len - x;
            new_geo._construction_angle = ang;
            Length = len - x;
            geometry = new_geo;
        }
        /// <summary>
        /// Dziala tylko dla straigth geometry, wymaga przerobienia jak pojawi sie curved
        /// </summary>
        /// <param name="x"></param>
        public void redefine_geometry_by_moving_end_with_new_point_at_x_from_end_side(double x)
        {
            var (len, ang) = geometry.return_con_angle_and_con_length();
            var new_geo = new straight_geometry();
            new_geo._construction_length = len - x;
            new_geo._construction_angle = ang;
            Length = len - x;
            geometry = new_geo;
        }


        public (float, float) calculate_position_of_point_at_x_from_start(double x)
        {
            //get directional node
            if (DirectionalNode == Node1_number)
            {
                var (pos_x, pos_y) = geometry.calculate_position_of_point_at_distance_x_from_start(x, Node1);
                return (pos_x, pos_y);
            }
            if(DirectionalNode == Node2_number)
            {
                var (pos_x, pos_y) = geometry.calculate_position_of_point_at_distance_x_from_start(x, Node2);
                return (pos_x, pos_y);
            }

            //error
            return (0, 0);
        }

        public (float, float, float) calculate_position__and_tangent_angle_of_point_at_x_from_start(double x)
        {
            if (DirectionalNode == Node1_number)
            {
                var (pos_x, pos_y, ang) = geometry.calculate_position_and_angle_of_point_at_distance_x_from_start(x, Node1);
                return (pos_x, pos_y, ang);
            }
            
            if (DirectionalNode == Node2_number)
            {
                var (pos_x, pos_y, ang) = geometry.calculate_position_and_angle_of_point_at_distance_x_from_start(x, Node2);
                return (pos_x, pos_y, ang);
            }

            //error
            return (0, 0, 0);
        }




        public (float, float) calculate_position_of_point_at_x_from_end(double x)
        {
            //get directional node
            if (DirectionalNode == Node1_number)
            {
                var (pos_x, pos_y) = geometry.calculate_position_of_point_at_distance_x_from_end(x, Node2);
                return (pos_x, pos_y);
            }
            if (DirectionalNode == Node2_number)
            {
                var (pos_x, pos_y) = geometry.calculate_position_of_point_at_distance_x_from_end(x, Node1);
                return (pos_x, pos_y);
            }

            //error
            return (0, 0);
        }

        public (float, float , float) return_position_and_angle_of_start()
        {
            return calculate_position__and_tangent_angle_of_point_at_x_from_start(0);
        }

        public (float, float , float) return_position_and_angle_of_end()
        {
            return calculate_position__and_tangent_angle_of_point_at_x_from_start(Length);
        }

        public (float , float , float) return_position_and_angle_at_center()
        {
            return calculate_position__and_tangent_angle_of_point_at_x_from_start(Length / 2);
        }


    }

    public abstract class TrackGeometry
    {
        public abstract type_of_geometry what_is_the_type_of_geometry();
        /// <summary>
        /// Return length [m], angle [deg]
        /// </summary>
        /// <returns></returns>
        public abstract (float, float) return_con_angle_and_con_length();
        public abstract float calculate_real_length();
        public abstract (float, float) calculate_position_of_point_at_distance_x_from_start(double x, Node node1);
        public abstract (float, float, float) calculate_position_and_angle_of_point_at_distance_x_from_start(double x, Node node1);
        public abstract (float, float) calculate_position_of_point_at_distance_x_from_end(double x, Node node2);
        public abstract (float, float) calculate_translation_from_directional_node_to_end_node();
    }


    public class straight_geometry : TrackGeometry
    {
        public double _construction_length { get; set; }
        public double _construction_angle { get; set; }


        public override type_of_geometry what_is_the_type_of_geometry()
        {
            return type_of_geometry.straight;
        }

        public override (float, float) return_con_angle_and_con_length()
        {
            return ((float)_construction_length, (float)_construction_angle);
        }

        public override float calculate_real_length()
        {
            return (float)_construction_length;
        }
        /// <summary>
        /// Node1 is directional node
        /// </summary>
        /// <param name="x"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        public override (float, float) calculate_position_of_point_at_distance_x_from_start(double x, Node node1)
        {
            var (trans_x, trans_y) = calculate_translation_from_directional_node_to_end_node();

            trans_x = (float)(trans_x * x / _construction_length);
            trans_y = (float)(trans_y * x / _construction_length);

            var (base_x, base_y) = node1.return_node_position();

            return (base_x + trans_x, base_y + trans_y );
        }


        public override (float, float, float) calculate_position_and_angle_of_point_at_distance_x_from_start(double x, Node node1)
        {
            var (trans_x, trans_y) = calculate_translation_from_directional_node_to_end_node();

            trans_x = (float)(trans_x * x / _construction_length);
            trans_y = (float)(trans_y * x / _construction_length);

            var (base_x, base_y) = node1.return_node_position();

            return (base_x + trans_x, base_y + trans_y, (float)_construction_angle);
        }

        public override (float, float) calculate_position_of_point_at_distance_x_from_end(double x, Node node2)
        {
            var (trans_x, trans_y) = calculate_translation_from_directional_node_to_end_node();

            trans_x = (float)(trans_x * x / _construction_length);
            trans_y = (float)(trans_y * x / _construction_length);

            var (base_x, base_y) = node2.return_node_position();

            return (base_x - trans_x, base_y - trans_y);
        }

        public override (float, float) calculate_translation_from_directional_node_to_end_node()
        {
            float translation_x;
            float translation_y;

            translation_x = (float)(Math.Cos(_construction_angle * Math.PI / 180) * _construction_length);
            translation_y = (float)(Math.Sin(_construction_angle * Math.PI / 180) * _construction_length);

            return (translation_x, translation_y);
        }
    }


    public class curved_geometry : TrackGeometry
    {
        public double _construction_length { get; set; }
        public double _construction_angle { get; set; }

        public float _radius { get; set; }

        //public curved_geometry(double construction_length, double construction_angle)
        //{
        //    _construction_length = construction_length;
        //    _construction_angle = construction_angle;
        //}

        public override type_of_geometry what_is_the_type_of_geometry()
        {
            return type_of_geometry.curved;
        }

        public override (float, float) return_con_angle_and_con_length()
        {
            //jak bede implementowac krzywe tracji to to zmienie
            throw new NotImplementedException();
        }

        public override float calculate_real_length()
        {
           //jak wprowadze do uzycia ta geometrie to tutaj bedzie przeliczenie odleglosci z ksztaltu krzywizny itp 
            return (float)_construction_length;
        }

        public override (float, float) calculate_position_of_point_at_distance_x_from_start(double x, Node node1)
        {
            // get position of start node

            // get position of end node

            // calculate or read position of circle center
            // get or calculate angle of start node around circle center

            // calculate angle of change by distance
            // calculate new angle around center 
            // based on new angle, calculate position on requested point

            // calculate position on new point
            // that is based
            throw new NotImplementedException();
        }

        public override (float, float, float) calculate_position_and_angle_of_point_at_distance_x_from_start(double x, Node node1)
        {
            throw new NotImplementedException();
        }

        public override (float, float) calculate_position_of_point_at_distance_x_from_end(double x, Node node2)
        {
            throw new NotImplementedException();
        }

        public override (float, float) calculate_translation_from_directional_node_to_end_node()
        {
            float translation_x;
            float translation_y;

            translation_x = (float)(Math.Sin(_construction_angle) * _construction_length);
            translation_y = (float)(Math.Cos(_construction_angle) * _construction_length);

            return (translation_x, translation_y);
        }

    }

    public enum type_of_geometry {
        straight,
        curved
    };

}
