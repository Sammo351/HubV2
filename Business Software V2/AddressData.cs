using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Software_V2
{
    [System.Serializable]
    public class AddressData : INotifyPropertyChanged
    {

        private string _number;
        public string Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged("Number"); }
        }

        private string _street;
        public string Street
        {
            get { return _street; }
            set { _street = value; OnPropertyChanged("Street"); }
        }

        private string _suburb;
        public string Suburb
        {
            get { return _suburb; }
            set { _suburb = value; OnPropertyChanged("Suburb"); }
        }

        private string _postCode;
        public string PostCode
        {
            get { return _postCode; }
            set { _postCode = value; OnPropertyChanged("PostCode"); }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged("State"); }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set { _country = value; OnPropertyChanged("Country"); }
        }

        public AddressData(string number, string street, string suburb, string postcode, string state, string country)
        {
            Number = number;
            this.Street = street;
            this.Suburb = suburb;
            this.PostCode = postcode;
            this.State = state;
            this.Country = country;
        }

        public override string ToString()
        {
            return $"{Number} {Street}, {Suburb}, {PostCode}, {State}, {Country}";
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
