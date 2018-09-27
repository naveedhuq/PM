using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class ExcelDocumentViewModel : ViewModelBase
    {
        public DelegateCommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Messenger.Default.Send<object>(
                        message: this,
                        token: MessageTokenEnum.CloseUserControl);
                });
            }
        }
    }
}
