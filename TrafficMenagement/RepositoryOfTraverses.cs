using System.Collections.Generic;

namespace Symulation
{
    public class RepositoryOfTraverses
    {
        private List<Traverse> _list_of_traverses;

        //kazda sekcja ma takie repozytorium, gdzie sa przechowywane wszystkie traversy w przyszlosc przez dana sekcje 
        // pytanie czy chce to podzielic na kilka i miec liste oddzielnie dla roznych permutacji ?
        // nawet jesli tak to ten obiekt zostanie taki sam 

        public RepositoryOfTraverses()
        {
            _list_of_traverses = new List<Traverse>();
        }


        public int get_next_free_number_of_travers()
        {
            return _list_of_traverses.Count;    
        }

        public void add_new_traverse_to_repository(Traverse traverse)
        {
            _list_of_traverses.Add(traverse);
        }


    }

    
}
