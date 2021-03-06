﻿using System;
using System.Collections.Generic;
using System.Text;
using Srinki.DataModel;
using System.Linq;
using Xamarin.Forms;

namespace Srinki.services
{
    class DisplayItem
    {
        public int boothNumber { get; set; }
        public string Text { get; set; }
        public string Detail { get; set; }
        public string phoneNumber { get; set; }
        public int population { get; set; }

        public DisplayItem()
        {
            Text = Detail = phoneNumber = "";
            boothNumber = population = 0;
        }
    }

    class DataService
    {
        RoadInformation roadInformation;
        BoothInformation boothInformation;
        AgentInformation agentInformation;
        ContactsInformation contactsInformation;
        static DataService dataService = null;
        bool dataStatus;

        public bool DataStatus
        {
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
            contactsInformation = new ContactsInformation();
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
            contactsInformation.UpdateInformation();
            saveDataToFile();
            dataStatus = true;

            Properties.setLastUpdatedTime();
        }

        public string saveDataToFile()
        {
            roadInformation.writeDataToFile();
            boothInformation.writeDataToFile();
            agentInformation.writeDataToFile();
            contactsInformation.writeDataToFile();
            return "";
        }

        public void readDataFromFile()
        {
            dataStatus = roadInformation.readDataFromFile();
            dataStatus = boothInformation.readDataFromFile() && dataStatus;
            dataStatus = agentInformation.readDataFromFile() && dataStatus;
            dataStatus = contactsInformation.readDataFromFile() && dataStatus;
        }

        public List<BoothInformation> GetBoothInformation(string search)
        {
            return boothInformation.getBoothInformation(search);
        }


        public BoothInformation GetBoothInformation(int boothNumber)
        {
            return boothInformation.getBoothInformation(boothNumber);
        }



        public List<DisplayItem> GetRoadInformationDisplayItems(int boothNumber)
        {
            var roadList = roadInformation.getRoadInformation(boothNumber);

            //Check input and throw expection if required
            if (roadList.Count == 0) throw new Exception("Invalid booth Numbers " + boothNumber);

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
                agents.Add(new DisplayItem
                {
                    phoneNumber = agent.phoneNumber,
                    Text = "Agent " + count,
                    Detail = agent.agentName + ", " + agent.phoneNumber + "\n" + agent.address,
                    boothNumber = agent.boothNumber
                });
                count++;
            }

            return agents;
        }

        public List<DisplayItem> GetAgentInformationDisplayItemsByName(string name)
        {
            var agentList = agentInformation.getAgentInformationByName(name);

            // Get the list booth in the list. Ideally there should be only one booth in the list            
            var agents = new List<DisplayItem>();
            int count = 1;
            foreach (var agent in agentList)
            {
                agents.Add(new DisplayItem
                {
                    phoneNumber = agent.phoneNumber,
                    Text = "Booth Number " + agent.boothNumber,
                    Detail =  agent.agentName + ", " + agent.phoneNumber + "\n" + agent.address,
                    boothNumber = agent.boothNumber
                });
                count++;
            }

            return agents;
        }

        public List<DisplayItem> GetBoothInformationDisplayItems(int boothNumber)
        {
            // Get the list booth in the list. Ideally there should be only one booth in the list
            var booth = boothInformation.getBoothInformation(boothNumber);
            var boothItems = new List<DisplayItem>();

            boothItems.Add(new DisplayItem() { Text = "Booth Address", Detail = booth.address });
            boothItems.Add(new DisplayItem() { Text = "Booth Number", Detail = booth.boothNumber.ToString() });
            boothItems.Add(new DisplayItem() { Text = "Booth Population", Detail = booth.population.ToString() });
            boothItems.Add(new DisplayItem() { Text = "Ward Number", Detail = booth.wardNumber.ToString() });
            boothItems.Add(new DisplayItem() { Text = "Locality", Detail = booth.locality });

            boothItems.ForEach(x => x.boothNumber = boothNumber);

            try
            {
                boothItems.AddRange(GetAgentInformationDisplayItems(boothNumber));
            }
            catch(Exception ex)
            {
                //Dont have to do anything as agent information is optional
            }

            // Sort the details before addings road information. Road information should not be sorted
            boothItems = boothItems.OrderBy(x => x.Text).ToList();
            try
            {              
                boothItems.AddRange(GetRoadInformationDisplayItems(boothNumber));
            }
            catch (Exception ex)
            {
                //Dont have to do anything as road information and agent information is optional
            }
            


            return boothItems;
        }

        public int getNumberOfBooths()
        {
            return boothInformation.getBoothCount();
        }

        public List<int> getAllWardNumbers()
        {
            return boothInformation.getAllWardNumbers();
        }

        public List<DisplayItem> getBoothInformationDisplayItemByWard(int wardNumber)
        {
            var booths = boothInformation.getBoothsByWard(wardNumber);
            List<DisplayItem> list = new List<DisplayItem>();

            foreach(var booth in booths)
            {
                list.Add(new DisplayItem()
                {
                    Text = "Booth Number " + booth.boothNumber.ToString(),
                    Detail = string.Format("Population {0}\nLocality {1}\nAddress {2}", booth.population, booth.locality, booth.address),
                    boothNumber = booth.boothNumber,
                    population = booth.population
                });
            }
            return list;
        }

        public List<DisplayItem> getBoothInformationDisplayItemByAddress(string address)
        {
            var booths = boothInformation.getBoothsByAddress(address);
            List<DisplayItem> list = new List<DisplayItem>();

            foreach (var booth in booths)
            {
                list.Add(new DisplayItem()
                {
                    Text = "Booth Number " + booth.boothNumber.ToString(),
                    Detail = string.Format("Population {0}\nLocality {1}\nAddress {2}", booth.population, booth.locality, booth.address),
                    boothNumber = booth.boothNumber,
                    population = booth.population
                });
            }
            return list;
        }

        public List<DisplayItem> getBoothInformationDisplayItemByRoad(string address)
        {
            var roads = roadInformation.getRoadsByAddress(address);
            List<DisplayItem> list = new List<DisplayItem>();

            foreach (var road in roads)
            {
                list.Add(new DisplayItem()
                {
                    Text = "Booth Number " + road.boothNumber.ToString(),
                    Detail = string.Format("{0}", road.address),
                    boothNumber = road.boothNumber
                });
            }

            return list;
        }

        public List<DisplayItem> getContactsInformationByWard(int wardNumber)
        {
            List<DisplayItem> list = new List<DisplayItem>();
            var contacts = contactsInformation.getContactInformation(wardNumber);

            foreach(var contact in contacts)
            {
                list.Add(new DisplayItem()
                {
                    Text = contact.position,
                    Detail = string.Format("{0},{1}", contact.name, contact.phoneNumber),
                    phoneNumber = contact.phoneNumber                    
                });
            }
            return list;
        }
    }
}
