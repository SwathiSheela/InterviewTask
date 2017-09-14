using System.ServiceModel;

namespace DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFormat" in both code and config file together.
    [ServiceContract]
    public interface IFormat
    {

       //To format Price value in Words
        [OperationContract]
        string FormatPrice(string price);
        // TODO: Add your service operations here
    }
    
}
