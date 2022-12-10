using System.Collections.Generic;

namespace Symulation
{
    public abstract class Traverse // jak bede rozwijac symulacje to pojawia sie nowe rodzaje slotow w innych sekcjach
    {
        
        public abstract int Number { get; set; }
        public abstract int Permutation { get; set; }

        public abstract double Size { get; set; }
        public abstract int entrance_node { get; set; }
        public abstract int exit_node { get; set; }

        public abstract double begining_start_time { get; set; }
        public abstract double begining_end_time { get; set; }

        public abstract double exit_start_time { get; set; }
        public abstract double exit_end_time { get; set; }

        public abstract void add_entrance_window(Window window);
        public abstract List<Window> get_entrance_windows_list();

        public abstract int get_order_index_of_entrance_window(Window win);

        public abstract void add_exit_window(Window window);
        public abstract List <Window> get_exit_windows_list();

        public abstract void sort_list_of_windows();

        public abstract List<Window> get_list_af_active_exit_windows();

    }

  
    
    public class DC_Traverse:Traverse
    {
        private List<Window> _list_of_entrance_windows;
        private List<Window> _list_of_exit_windows;

        public override int Number { get; set; }
        public override int Permutation { get; set;}
        public override double Size { get; set; }
        public override double begining_start_time { get ; set ; }
        public override double begining_end_time { get; set; }
        public override double exit_start_time { get; set; }
        public override double exit_end_time { get ; set ; }
        public override int entrance_node { get; set; }
        public override int exit_node { get; set; }

        public DC_Traverse()
        {
            _list_of_entrance_windows = new List<Window>();
            _list_of_exit_windows = new List<Window>();
        }

        public override void add_entrance_window(Window window)
        {
            _list_of_entrance_windows.Add(window);
        }

        public override List<Window> get_entrance_windows_list()
        {
            return _list_of_entrance_windows;
        }

        public override int get_order_index_of_entrance_window(Window win)
        {
            for(int i = 0; i < _list_of_entrance_windows.Count; i++)
            {
                if(_list_of_entrance_windows[i] == win)
                    return i;
            }

            return -1;
        }

        public override void add_exit_window(Window window)
        {
            _list_of_exit_windows.Add(window);
        }

        public override List<Window> get_exit_windows_list()
        {
            return _list_of_exit_windows;
        }
        public override void sort_list_of_windows()
        {
            _list_of_entrance_windows.Sort((x, y) => x.start_time.CompareTo(y.start_time));
            _list_of_exit_windows.Sort((x, y) => x.start_time.CompareTo(y.start_time));
        }

        public override List<Window> get_list_af_active_exit_windows()
        {
            List<Window> list_of_active_win = new List<Window>();

            for(int i = 0; i < _list_of_exit_windows.Count; i++)
            {
                if (_list_of_exit_windows[i].window_is_deactivated == false)
                    list_of_active_win.Add(_list_of_exit_windows[i]);
            }

            return list_of_active_win;
        }

    }

    public class Sort_Traverse :Traverse
    {
        private List<Window> _list_of_entrance_windows;
        private List<Window> _list_of_exit_windows;

        public override int Number { get; set; }
        public override int Permutation { get; set; }
        public override double Size { get; set; }
        public override double begining_start_time { get; set; }
        public override double begining_end_time { get; set; }
        public override double exit_start_time { get; set; }
        public override double exit_end_time { get; set; }

        public override int entrance_node { get; set; }

        public override int exit_node { get; set; }

        public Sort_Traverse()
        {
            _list_of_entrance_windows = new List<Window>();
            _list_of_exit_windows = new List<Window>();
        }
    
        public override void sort_list_of_windows()
        {
            _list_of_entrance_windows.Sort((x, y) => x.start_time.CompareTo(y.start_time));
            _list_of_exit_windows.Sort((x, y) => x.start_time.CompareTo(y.start_time));
        }

        public override void add_entrance_window(Window window)
        {
            _list_of_entrance_windows.Add(window);
        }
        public override List<Window> get_entrance_windows_list()
        {
            return _list_of_entrance_windows;
        }

        public override int get_order_index_of_entrance_window(Window win)
        {
            for (int i = 0; i < _list_of_entrance_windows.Count; i++)
            {
                if (_list_of_entrance_windows[i] == win)
                    return i;
            }

            return -1;
        }

        public override void add_exit_window(Window window)
        {
            _list_of_exit_windows.Add(window);
        }

        public override List<Window> get_exit_windows_list()
        {
            return _list_of_exit_windows;
        }

        public override List<Window> get_list_af_active_exit_windows()
        {
            List<Window> list_of_active_win = new List<Window>();

            for (int i = 0; i < _list_of_exit_windows.Count; i++)
            {
                if (_list_of_exit_windows[i].window_is_deactivated == false)
                    list_of_active_win.Add(_list_of_exit_windows[i]);
            }

            return list_of_active_win;
        }

    }
}
