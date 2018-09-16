using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM.Shared;

namespace PM.Model
{
    public class LookupItem
    {
        public enum LookupTypesEnum
        {
            CustomerType,
            ContactItemType,
            TypeOfCompany,
            ServiceType,
            DefaultFolder,
            ExtensionToImageMapping,
            DocumentType
        }


        public string LookupType { get; set; }
        public int SortOrder { get; set; }
        public string LookupName { get; set; }


        public static ObservableCollection<string> GetLookupStrings(LookupTypesEnum LookupType)
        {
            return
            new ObservableCollection<string>
            (
                from x in DBHelper.Instance.LookupsRepository
                where x.LookupType == LookupType.ToString()
                orderby x.SortOrder
                select x.LookupName
            );
        }


        public static ObservableCollection<LookupItem> GetLookupItems(LookupTypesEnum LookupType)
        {
            return new ObservableCollection<LookupItem>
            (
                from x in DBHelper.Instance.LookupsRepository
                where x.LookupType == LookupType.ToString()
                orderby x.SortOrder
                select x
            );
        }

    }
}
