using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Srinki.DataModel
{

    class DisplayItem
    {
        public int boothNumber;
        public string Text { get; set; }
        public string Detail { get; set; }
    }
    class BoothInformation : Information
    {
        public int boothNumber { get; set; }
        public int wardNumber  { get; set; }
        public int population  { get; set; }
        public string address  { get; set; }
        public string locality { get; set; }

        static List<BoothInformation> boothInformation = null;        

        public BoothInformation()
        {

        }

        public override void UpdateInformation()
        {
            updateBoothInformation();
        }

        public void updateBoothInformation()
        {
            var googleSheet = new GoogleSheetApi();
            const string boothInformationpage = "ElectionBooths";
            var values = googleSheet.getSheetData(boothInformationpage, "A3:E");
            var boothInformation_tmp = new List<BoothInformation>();
            foreach (var row in values)
            {
                // Print columns A and E, which correspond to indices 0 and 4.
                Console.WriteLine("{0}, {1}, {2}, {3}", row[0], row[1], row[2], row[3]);
                boothInformation_tmp.Add(new BoothInformation()
                {
                    boothNumber = Int32.Parse(row[0].ToString()),
                    wardNumber = Int32.Parse(row[1].ToString()),
                    population = Int32.Parse(row[2].ToString()),
                    address = row[3].ToString(),
                    locality = row[4].ToString()
                });
            }

            boothInformation = boothInformation_tmp;
        }

        public BoothInformation getBoothInformation(int boothNumber)
        {
            if (boothInformation == null) throw new Exception("booth information not initialized. Try updating data");
            var booth = boothInformation.Where(x => x.boothNumber == boothNumber).ToList();

            //Check input and throw expection if required
            if (booth.Count == 0) throw new Exception("Valid booth Numbers are between 1 to " + boothInformation.Count);
            if (booth.Count > 1) throw new Exception("More than one booth with booth Number " + boothNumber);

            return booth.First();
        }

        public List<DisplayItem> GetBoothInformationDisplayItems(int boothNumber)
        {            
            // Get the list booth in the list. Ideally there should be only one booth in the list
            var booth = getBoothInformation(boothNumber);
            var boothItems = new List<DisplayItem>();
            boothItems.Add(new DisplayItem() { Text = "Ward Number", Detail = string.Format("{0}", booth.wardNumber, boothNumber = booth.boothNumber) });            
            boothItems.Add(new DisplayItem() { Text =  "Booth Number", Detail = string.Format("{0}", booth.boothNumber, boothNumber = booth.boothNumber) });
            boothItems.Add(new DisplayItem() { Text = "Booth Population", Detail = string.Format("{0}", booth.population, boothNumber = booth.boothNumber) });
            boothItems.Add(new DisplayItem() { Text = "Locality", Detail = booth.locality, boothNumber = booth.boothNumber });
            boothItems.Add(new DisplayItem() { Text = "Booth Address", Detail = booth.address, boothNumber = booth.boothNumber });

            boothItems = boothItems.OrderBy(x => x.Text).ToList();
            boothItems.AddRange((new RoadInformation()).GetRoadInformationDisplayItems(boothNumber));

            return boothItems;
        }
    }
}
