using System;
using System.Collections.Generic;
using System.Text;
using Srinki.DataModel;
using System.Linq;
using Xamarin.Forms;

namespace Srinki.services
{
    class DisplayItem
    {
        public int boothNumber;
        public string Text { get; set; }
        public string Detail { get; set; }
    }

    class DataService
    {
        RoadInformation roadInformation;
        BoothInformation boothInformation;
        AgentInformation agentInformation;
        static DataService dataService = null;        
        bool dataStatus;

        public bool DataStatus {
            get
            {
                return dataStatus;
            }
        }

        DataService()
        {
            roadInformation = new RoadInformation();
            boothInformation = new BoothInformation();
            agentInformation = new AgentInformation();
        }
        

        public static DataService getDataService()
        {
            if (dataService == null) dataService = new DataService();
            return dataService;
        }


        public void UpdateData()
        {
            roadInformation.UpdateInformation();
            boothInformation.UpdateInformation();
            agentInformation.UpdateInformation();
            Application.Current.Properties["lastUpdatedTime"] = DateTime.Now.ToString("dd-MM-yy HH:mm");
            Application.Current.SavePropertiesAsync();
            saveDataToFile();
            dataStatus = true;
        }

        public string saveDataToFile()
        {
            roadInformation.writeDataToFile();
            boothInformation.writeDataToFile();
            agentInformation.writeDataToFile();
            return "";
        }

        public void readDataFromFile()
        {
            dataStatus = roadInformation.readDataFromFile();            
            dataStatus = boothInformation.readDataFromFile() && dataStatus;
            dataStatus = agentInformation.readDataFromFile() && dataStatus;
        }


        public List<DisplayItem> GetRoadInformationDisplayItems(int boothNumber)
        {
            var roadList = roadInformation.getRoadInformation(boothNumber);

            //Check input and throw expection if required
            if (roadList.Count == 0) throw new Exception("InValid booth Numbers " + boothNumber);

            // Get the list booth in the list. Ideally there should be only one booth in the list            
            var roads = new List<DisplayItem>();
            int count = 1;
            foreach (var road in roadList)
            {
                roads.Add(new DisplayItem { Text = "Road " + count, Detail = road.address, boothNumber = road.boothNumber });
                count++;
            }

            return roads;
        }

        public List<DisplayItem> GetAgentInformationDisplayItems(int boothNumber)
        {
            var agentList = agentInformation.getAgentInformation(boothNumber);

            //Check input and throw expection if required
            if (agentList.Count == 0) throw new Exception("InValid booth Numbers " + boothNumber);

            // Get the list booth in the list. Ideally there should be only one booth in the list            
            var agents = new List<DisplayItem>();
            int count = 1;
            foreach (var agent in agentList)
            {
                agents.Add(new DisplayItem { Text = "Agent " + count, Detail = agent.agentName + ", " + agent.phoneNumber, boothNumber = agent.boothNumber });
                count++;
            }

            return agents;
        }
 
        public List<DisplayItem> GetBoothInformationDisplayItems(int boothNumber)
        {
            // Get the list booth in the list. Ideally there should be only one booth in the list
            var booth = boothInformation.getBoothInformation(boothNumber);
            var boothItems = new List<DisplayItem>();

            boothItems.AddRange(GetAgentInformationDisplayItems(boothNumber));

            boothItems.Add(new DisplayItem() { Text = "Booth Address", Detail = booth.address, boothNumber = booth.boothNumber });
            boothItems.Add(new DisplayItem() { Text = "Booth Number", Detail = string.Format("{0}", booth.boothNumber, boothNumber = booth.boothNumber) });
            boothItems.Add(new DisplayItem() { Text = "Booth Population", Detail = string.Format("{0}", booth.population, boothNumber = booth.boothNumber) });
            boothItems.Add(new DisplayItem() { Text = "Ward Number", Detail = string.Format("{0}", booth.wardNumber, boothNumber = booth.boothNumber) });
            boothItems.Add(new DisplayItem() { Text = "Locality", Detail = booth.locality, boothNumber = booth.boothNumber });
            

            boothItems = boothItems.OrderBy(x => x.Text).ToList();
            try
            {
                boothItems.AddRange(GetRoadInformationDisplayItems(boothNumber));                
            }
            catch(Exception)
            {

            }

            return boothItems;
        }

        public int getNumberOfBooths()
        {
            return boothInformation.getBoothCount();
        }
    }
}
