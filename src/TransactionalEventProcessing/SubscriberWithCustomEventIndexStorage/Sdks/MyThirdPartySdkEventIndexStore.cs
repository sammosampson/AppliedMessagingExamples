namespace SubscriberWithCustomEventIndexStorage.Sdks
{
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure.Events.Streams;

    public class MyThirdPartySdkEventIndexStore : IEventIndexStore
    {
        private readonly MyThirdPartySdk sdk;

        public MyThirdPartySdkEventIndexStore(MyThirdPartySdk sdk)
        {
            this.sdk = sdk;
        }

        public void Store(string stream, string owningProcess, int index)
        {
            sdk.StoreEventIndex(EventIndexStorageStreamId.Parse(stream, owningProcess), index);
        }

        public NotRequired<int> GetIndex(string stream, string owningProcess)
        {
            EventIndexStorageStreamId key = EventIndexStorageStreamId.Parse(stream, owningProcess);
            return !sdk.HasEventIndex(key) ? new NotRequired<int>() : sdk.GetEventIndex(key);
        }
    }
}