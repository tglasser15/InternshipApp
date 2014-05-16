using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp
{
    public class SearchItem
    {
        //searches by property
        public string GeneralSearch { get; set; }
        public string DisciplineSearch { get; set; }
        public string Location { get; set; }
        

        public SearchItem() { }
        //intializing
        public SearchItem(string searchbar, string fieldbar, string locationbar)
        {
            this.GeneralSearch = searchbar;
            this.DisciplineSearch = fieldbar;
            this.Location = locationbar;
        }

    }
}
