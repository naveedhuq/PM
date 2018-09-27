using System;


namespace PM.Model
{
    public static class Enumerators
    {
        public enum CustomerTypeEnum
        {
            Personal,
            Business
        }

        public enum GenderTypeEnum
        {
            Male,
            Female
        }

        public enum MessageTokenEnum
        {
            SelectedCustomerChanged,
            DocumentClipboardChanged,
            ApplyFilterInvoked,
            CloseUserControl
        }


    }
}
