using System;

namespace WDHTracker.Models
{
    class InstrumentDetails
    {
        public int InstrumentID { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string ItemNumber { get; set; }
        public string SerialNo { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public DateTime DateAssigned { get; set; }

        public CalibrationContent CalibrationStatus { get; set; }
        public FixedLocationContent FixedLocationStatus { get; set; }
    }

    public class CalibrationContent
    {
        public string CalibrationStatus { get; set; }
        public string NextCalib { get; set; }
        private double Days { get; set; }
    }

    public class FixedLocationContent
    {
        public string LocationStatus { get; set; }
        public string FixedLocationID { get; set; }
        public string CurrentLocationID { get; set; }
        public string LocationName { get; set; }
    }
}
