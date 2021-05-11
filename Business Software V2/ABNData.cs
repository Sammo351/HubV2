using System;
using System.ComponentModel;

namespace Business_Software_V2
{
    public class ABNData : INotifyPropertyChanged
    {
        private string _abn;
        public string ABN
        {
            get
            {
                return _abn;
            }
            set
            {
                _abn = value; NotifyPropertyChanged("ABN");
            }
        }

        private string _companyName;
        public string CompanyName
        {
            get
            {
                return _companyName;
            }
            set
            {
                _companyName = value; NotifyPropertyChanged("CompanyName");
            }
        }

        private string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value; NotifyPropertyChanged("Phone");
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value; NotifyPropertyChanged("Email");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

