using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Software_V2
{
    [System.Serializable]
    public class DataCompany : INotifyPropertyChanged
    {
        string _abn;
        public string ABN
        {
            get { return _abn; }

            set
            {
                if (_abn != value)
                {
                    _abn = value;
                    OnPropertyChanged("ABN");
                }
            }
        }

        string _companyName;
        public string CompanyName
        {
            get { return _companyName; }

            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                    OnPropertyChanged("CompanyName");
                }
            }
        }

        string _officeNumber;
        public string OfficeNumber
        {
            get { return _officeNumber; }

            set
            {
                if (_officeNumber != value)
                {
                    _officeNumber = value;
                    OnPropertyChanged("OfficeNumber");
                }
            }
        }

        ObservableCollection<DataContact> _contacts;
        public ObservableCollection<DataContact> Contacts
        {
            get { return _contacts; }
            set
            {
                if (_contacts != value)
                {
                    _contacts = value;
                    OnPropertyChanged("Contacts");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [System.Serializable]
    public class DataContact : INotifyPropertyChanged
    {
        private string Name;

        public string _name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("Name"); }
        }

        private string PhoneNumber;

        public string _phoneNumber
        {
            get { return PhoneNumber; }
            set { PhoneNumber = value; OnPropertyChanged("PhoneNumber"); }
        }

        private string Email;

        public string _email
        {
            get { return Email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
