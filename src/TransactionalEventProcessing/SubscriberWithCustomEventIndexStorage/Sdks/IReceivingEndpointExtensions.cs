namespace SubscriberWithCustomEventIndexStorage.Sdks
{
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    public static class IReceivingEndpointExtensions
    {
        public static TReceivingEndpoint WithMyThirdPartySdkEventIndexStorage<TReceivingEndpoint>(this TReceivingEndpoint endpoint) where TReceivingEndpoint : IReceivingEndpoint
        {
            endpoint.EventIndexStoreType = typeof(MyThirdPartySdkEventIndexStore);
            endpoint.TransactionProviderType = typeof(MyThirdPartySdkTransactionProvider);
            return endpoint;
        }
    }
}