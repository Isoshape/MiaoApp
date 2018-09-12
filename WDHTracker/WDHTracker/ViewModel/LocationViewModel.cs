using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WDHTracker.ViewModel
{
   public class LocationViewModel
    {
      public ObservableCollection<LocationDetails> Name { get; set; }

        public LocationViewModel()
        {
            Name = new ObservableCollection<LocationDetails>(); // init. your items 
        }
    }
}
