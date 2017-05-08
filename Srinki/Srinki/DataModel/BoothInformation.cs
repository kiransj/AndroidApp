using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Srinki.DataModel
{
    [JsonObject(MemberSerialization.OptIn)]
    class BoothInformation : Information
    {
        [JsonProperty]
        public int boothNumber { get; set; }
        [JsonProperty]
        public int wardNumber { get; set; }
        [JsonProperty]
        public int population { get; set; }
        [JsonProperty]
        public string address { get; set; }
        [JsonProperty]
        public string locality { get; set; }

        static List<BoothInformation> boothInformation = null;

        public BoothInformation()
        {

        }

        public override void UpdateInformation()
        {
            updateBoothInformation();
        }

        public void writeDataToFile()
        {
            if (boothInformation == null) throw new Exception("booth information not initialized. Try updating data");

            var data = JsonConvert.SerializeObject(boothInformation);
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "boothInformation.json");
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, data);
        }

        public bool readDataFromFile()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "boothInformation.json");
            if (!File.Exists(filename)) return false;
            var data = JsonConvert.DeserializeObject<List<BoothInformation>>(File.ReadAllText(filename));
            if (data.Count > 0)
                boothInformation = data;
            return data.Count > 0;
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

        public int getBoothCount()
        {
            if (boothInformation == null) throw new Exception("booth information not initialized. Try updating data");
            return boothInformation.Count;
        }
    }
}
