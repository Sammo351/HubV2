﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Google.Cloud.Firestore;

namespace Business_Software_V2
{
    [System.Serializable]
    [FirestoreData]
    public class DataCompany : INotifyPropertyChanged
    {

        string _abn;
        [FirestoreProperty]
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


        string _email;
        [FirestoreProperty]
        public string Email
        {
            get { return _email; }

            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        string _companyName;
        [FirestoreProperty]
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
        [FirestoreProperty]
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

        ObservableCollection<DataContact> _contacts = new ObservableCollection<DataContact>();
        public ObservableCollection<DataContact> Contacts
        {
            get { if (_contacts == null) _contacts = new ObservableCollection<DataContact>(); return _contacts; }
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
        private string _name;

        public DataContact(string name, string email, string phone)
        {
            this.Name = name; this.Email = email; this.PhoneNumber = phone;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged("PhoneNumber"); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
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
