using System;
using PM.Backend.Shared;


namespace PM.UI.Model
{
    public class MasterModel : IMasterModel
    {
        public string UserName
        {
            get { return SharedUtils.Instance.UserName; }
        }

        public string AppDirectory
        {
            get { return SharedUtils.Instance.AppDirectory; }
        }
    }
}
