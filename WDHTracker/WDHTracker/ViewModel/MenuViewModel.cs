using System;
using System.Collections.Generic;
using System.Text;

namespace WDHTracker.ViewModel
{
    public class MenuViewModel
    {
        #region singleton
        public static MenuViewModel Instance => _instance ?? (_instance = new MenuViewModel());
        static MenuViewModel _instance;
        MenuViewModel()
        {
            ListItems.Add("About this app");
            ListItems.Add("Logout");
            ListItems.Add("3rd Item <3");
        }
        #endregion

        #region fields
        IList<string> _listItems = new List<string>();
        #endregion

        #region properties
        public IList<string> ListItems
        {
            get { return _listItems; }
            set { _listItems = value; }
        }
        #endregion
    }
}
  