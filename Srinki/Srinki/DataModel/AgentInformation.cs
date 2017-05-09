using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Srinki.DataModel
{
    [JsonObject(MemberSerialization.OptIn)]
    class AgentInformation : Information
    {
        [JsonProperty]
        public int boothNumber;
        [JsonProperty]
        public string agentName;
        [JsonProperty]
        public string phoneNumber;
        [JsonProperty]
        public string address;


        static List<AgentInformation> agentInformation = null;

        public AgentInformation()
        {

        }

        public override void UpdateInformation()
        {
            updateAgentInformation();
        }

        public void updateAgentInformation()
        {
            var googleSheet = new GoogleSheetApi();
            const string agentInformationpage = "AgentInformation";
            var values = googleSheet.getSheetData(agentInformationpage, "A3:D");
            var agentInformation_tmp = new List<AgentInformation>();
            foreach (var row in values)
            {
                // Print columns A and E, which correspond to indices 0 and 4.
                Console.WriteLine("{0}, {1}, {2}, {3}", row[0], row[1], row[2], row[3]);
                agentInformation_tmp.Add(new AgentInformation()
                {
                    boothNumber = Int32.Parse(row[0].ToString()),
                    agentName = row[1].ToString(),
                    phoneNumber = row[2].ToString(),
                    address = row[3].ToString()
                });
            }

            agentInformation = agentInformation_tmp;
        }



        public void writeDataToFile()
        {
            if (agentInformation == null) throw new Exception("booth information not initialized. Try updating data");

            var data = JsonConvert.SerializeObject(agentInformation);
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "agentInformation.json");
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, data);
        }

        public bool readDataFromFile()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "agentInformation.json");
            if (!File.Exists(filename)) return false;
            var data = JsonConvert.DeserializeObject<List<AgentInformation>>(File.ReadAllText(filename));
            if (data.Count > 0)
                agentInformation = data;
            return data.Count > 0;
        }

        public List<AgentInformation> getAgentInformation(int boothNumber)
        {
            if (agentInformation == null)
                throw new Exception("agent information not initialized. Try updating data");
            return agentInformation.Where(x => x.boothNumber == boothNumber).ToList();
        }
    }
}
