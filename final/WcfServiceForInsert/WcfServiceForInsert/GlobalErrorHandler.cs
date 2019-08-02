using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
namespace WcfServiceForInsert
{
    public class GlobalErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return true;
        }
        public void ProvideFault(Exception error,
           System.ServiceModel.Channels.MessageVersion version,
           ref System.ServiceModel.Channels.Message fault)
        {
            var newEx = new FaultException(
                string.Format("Exception caught at Service Application GlobalErrorHandler{0}Method: {1}{2}Message:{3}",
                Environment.NewLine, error.TargetSite.Name, Environment.NewLine, error.Message));

            MessageFault msgFault = newEx.CreateMessageFault();
            fault = Message.CreateMessage(version, msgFault, newEx.Action);
        }
    }
}