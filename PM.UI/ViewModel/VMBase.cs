using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using dx = DevExpress.Mvvm;

namespace PM.UI.ViewModel
{
    public abstract class VMBase : dx.ViewModelBase
    {
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            RaisePropertiesChanged(propertyName);
            return true;
        }

    }
}
