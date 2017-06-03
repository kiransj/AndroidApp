using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Srinki.DataModel
{
    [JsonObject(MemberSerialization.OptIn)]
    class ContactsInformation : Information
    {
        [JsonProperty]
        public int wardNumber;
        [JsonProperty]
        public string position;
        [JsonProperty]
        public string name;
        [JsonProperty]
        public string phoneNumber;
        [JsonProperty]
        public string Notes;

        static List<ContactsInformation> contactInformation = null;

        public ContactsInformation()
        {

        }

        public override void UpdateInformation()
        {
            updateContactsInformation();
        }

        public void updateContactsInformation()
        {
            var googleSheet = new GoogleSheetApi();
            const string contactInformationpage = "Contacts";
            var values = googleSheet.getSheetData(contactInformationpage, "A3:E");
            var contactInformation_tmp = new List<ContactsInformation>();
            foreach (var row in values)
            {                
                // Print columns A and E, which correspond to indices 0 and 4.
                Console.WriteLine("{0}, {1}, {2}, {3}", row[0], row[1], row[2], row[3]);
                contactInformation_tmp.Add(new ContactsInformation()
                {
                    wardNumber = Int32.Parse(row[0].ToString()),
                    position = row[1].ToString(),
                    name = row[2].ToString(),
                    phoneNumber = row[3].ToString(),
                    Notes = row.Count == 5 ? row[4].ToString() : ""
                });
            }

            contactInformation = contactInformation_tmp;
        }

        public void writeDataToFile()
        {
            if (contactInformation == null) throw new Exception("contact information not initialized. Try updating data");

            var data = JsonConvert.SerializeObject(contactInformation);
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "contactsInformation.json");
            if (File.Exists(filename)) File.Delete(filename);
            File.WriteAllText(filename, data);
        }

        public bool readDataFromFile()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var filename = Path.Combine(path, "contactsInformation.json");
            if (!File.Exists(filename)) return false;
            var data = JsonConvert.DeserializeObject<List<ContactsInformation>>(File.ReadAllText(filename));
            if (data.Count > 0)
                contactInformation = data;
            return data.Count > 0;
        }

        public List<ContactsInformation> getContactInformation(int wardNumber)
        {
            if (contactInformation == null)
                throw new Exception("agent information not initialized. Try updating data");
            return contactInformation.Where(x => x.wardNumber == wardNumber).ToList();
        }

    }
}
