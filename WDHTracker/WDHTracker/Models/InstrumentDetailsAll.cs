using System;
using System.Collections.Generic;
using System.Text;

namespace WDHTracker.Models
{
    class InstrumentDetailsAll
    {
        public int InstrumentID { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }

        public bool Calibrated { get; set; }
        public string NextCalib { get; set; }
        public string LastCalibration { get; set; }
        public string CalibrationNumber { get; set; }
        public string CalibrationDescription { get; set; }

        public int? LocationID { get; set; }
        public string LocationName { get; set; }
    }
}
