using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipApp
{
    public class SearchItem
    {
        public string GeneralSearch { get; set; }
        public string DisciplineSearch { get; set; }
        public string Location { get; set; }
        List<SearchItem> saved_searches = new List<SearchItem>();

        public SearchItem() { }
        public SearchItem(string searchbar, string fieldbar, string locationbar)
        {
            this.GeneralSearch = searchbar;
            this.DisciplineSearch = fieldbar;
            this.Location = locationbar;
        }

    }
}
