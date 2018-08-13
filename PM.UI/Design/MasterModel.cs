using PM.UI.Model;


namespace PM.UI.Design
{
    public class MasterModel : IMasterModel
    {
        public string UserName { get { return "<CURRENT USER>"; } }
        public string AppDirectory { get { return "<CURRENT APP DIR>"; } }
    }
}
