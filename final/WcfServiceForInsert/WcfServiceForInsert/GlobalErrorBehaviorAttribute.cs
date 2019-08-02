using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
namespace WcfServiceForInsert
{
    public class GlobalErrorBehaviorAttribute  :Attribute, IServiceBehavior
    {
        private readonly Type errorHandlerType;
        public GlobalErrorBehaviorAttribute(Type errorHandlerType)
        {
            this.errorHandlerType = errorHandlerType;
        }
        void IServiceBehavior.Validate(ServiceDescription description,
            ServiceHostBase serviceHostBase)
        {
        }
        void IServiceBehavior.AddBindingParameters(ServiceDescription description,
                    ServiceHostBase serviceHostBase,
                    Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
        }
        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription description,
        ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler;

            try
            {
                errorHandler = (IErrorHandler)Activator.CreateInstance(errorHandlerType);
            }
            catch (MissingMethodException e)
            {
                throw new ArgumentException("The errorHandlerType specified in the ErrorBehaviorAttribute constructor must have a public empty constructor.", e);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("The errorHandlerType specified in the ErrorBehaviorAttribute constructor must implement System.ServiceModel.Dispatcher.IErrorHandler.", e);
            }

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }
    }
}