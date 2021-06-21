using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Business_Software_V2
{
    [System.Serializable]
    public class DataJob : INotifyPropertyChanged
    {
        public static string JobDirectory
        {
            get { return @"D:\Desktop\Hub Test Folder\Jobs"; }
        }

        private AddressData _address;
        public AddressData Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("Address"); }
        }

        private string _supervisor;
        public string Supervisor
        {
            get { return _supervisor; }
            set { _supervisor = value; OnPropertyChanged("Supervisor"); }
        }

        public Guid guid;

        public DataJob()
        {
            guid = Guid.NewGuid();
        }

        public static DataJob Load(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DataJob>(json);
        }

        public string WriteJobToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public ObservableCollection<string> Documents = new ObservableCollection<string>();


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
