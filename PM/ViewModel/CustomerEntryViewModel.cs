using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.ViewModel
{
    public class CustomerEntryViewModel : ViewModelBase
    {
        public string Label
        {
            get { return GetProperty(() => Label); }
            set { SetProperty(() => Label, value); }
        }

        
    }
}
