using Newtonsoft.Json;
using Srinki.services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Srinki.DataModel
{
    [JsonObject(MemberSerialization.OptIn)]
    class RoadInformation : Information
    {
        [JsonProperty]
        public int boothNumber { get; set; }

        [JsonProperty]
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

        public List<RoadInformation> getRoadsByAddress(string address)
        {
            if (roadInformation == null) throw new Exception("booth information not initialized. Try updating data");
            return roadInformation.Where(x => (x.address.IndexOf(address, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();
        }

        public void writeDataToFile()
        {
            if (roadInformation == null) throw new Exception("booth information not initialized. Try updating data");

            var data = JsonConvert.SerializeObject(roadInformation);
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "roadInformation.json");
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, data);
        }

        public bool readDataFromFile()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "roadInformation.json");
            if (!File.Exists(filename)) return false;
            var data = JsonConvert.DeserializeObject<List<RoadInformation>>(File.ReadAllText(filename));
            if (data.Count > 0) roadInformation = data;

            return data.Count > 0;
        }
    }
}
