using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Srinki.DataModel
{
    class RoadInformation : Information
    {
        public int boothNumber { get; set; }
        public string address { get; set; }

        static List<RoadInformation> roadInformation = null;

        public override void UpdateInformation()
        {
            updateRoadInformation();
        }

        public void updateRoadInformation()
        {
            var googleSheet = new GoogleSheetApi();
            const string boothInformationpage = "BoothToRoad";
            var values = googleSheet.getSheetData(boothInformationpage, "A2:B");
            var roadInformation_tmp = new List<RoadInformation>();
            foreach (var row in values)
            {
                // Print columns A and E, which correspond to indices 0 and 4.
                Console.WriteLine("{0}, {1}", row[0], row[1]);
                roadInformation_tmp.Add(new RoadInformation()
                {
                    boothNumber = Int32.Parse(row[0].ToString()),
                    address = row[1].ToString()                    
                });
            }

            roadInformation = roadInformation_tmp;
        }

        public List<RoadInformation> getRoadInformation(int boothNumber)
        {
            if (roadInformation == null)
                throw new Exception("road information not initialized. Try updating data");
            return roadInformation.Where(x => x.boothNumber == boothNumber).ToList();            
        }

        public List<DisplayItem> GetRoadInformationDisplayItems(int boothNumber)
        {
            var roadList = getRoadInformation(boothNumber);

            //Check input and throw expection if required
            if (roadList.Count == 0) throw new Exception("InValid booth Numbers " + boothNumber);            

            // Get the list booth in the list. Ideally there should be only one booth in the list            
            var roads = new List<DisplayItem>();
            int count = 1;
            foreach(var road in roadList)
            {
                roads.Add(new DisplayItem { Text = "Road " + count, Detail = road.address, boothNumber = road.boothNumber });
                count++;
            }

            return roads;
        }
    }
}
