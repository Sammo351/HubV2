using Business_Software_V2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Software_V2
{
    [System.Serializable]
    public class DataInvoice : INotifyPropertyChanged
    {
        string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            
                set {
                    if (_filePath != value)
                    {
                    _filePath = value;
                        OnPropertyChanged("FilePath");
                    }
                }
        }

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

        public string DisplayName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }

        public string Date
        {
            get { return File.GetCreationTime(FilePath).Date.ToShortDateString(); }
        }

        public string CompanyName
        {
            get { return ABNHelper.GetCompanyName(ABN); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    
    }
}
