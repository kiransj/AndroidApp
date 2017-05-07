using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Srinki.DataModel
{

    class BoothInformationDisplayItem
    {
        public int boothNumber;
        public string Text { get; set; }
        public string Detail { get; set; }
    }
    class BoothInformation
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

        public List<BoothInformation> getBoothInformation()
        {
            if (boothInformation == null)
                throw new Exception("booth information not initialized");
            return boothInformation;
        }

        public List<BoothInformationDisplayItem> GetBoothInformationDisplayItems(int boothNumber)
        {
            if (boothInformation == null) updateBoothInformation();
            var boothList = boothInformation.Where(x => x.boothNumber == boothNumber).ToList();
            if (boothList.Count != 1) throw new Exception("More than one booth with booth Number " + boothNumber);

            var booth = boothList.First();
            var boothItems = new List<BoothInformationDisplayItem>();
            boothItems.Add(new BoothInformationDisplayItem() { Text = "Booth Address", Detail = booth.address });
            boothItems.Add(new BoothInformationDisplayItem(){ Text =  "Booth Number", Detail = string.Format("{0}", booth.boothNumber) });
            boothItems.Add(new BoothInformationDisplayItem() { Text = "Booth Population", Detail = string.Format("{0}", booth.population) });
            boothItems.Add(new BoothInformationDisplayItem() { Text = "Locality", Detail = booth.locality });
            boothItems.Add(new BoothInformationDisplayItem() { Text = "Ward Number", Detail = string.Format("{0}", booth.wardNumber) });
                       
            return boothItems;
        }
    }


}
