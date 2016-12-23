namespace SubscriberWithCustomEventIndexStorage.Sdks
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Transactions;

    public class MyThirdPartySdkTransactionProvider : ITransactionProvider
    {
        private readonly MyThirdPartySdk sdk;

        public MyThirdPartySdkTransactionProvider(MyThirdPartySdk sdk)
        {
            this.sdk = sdk;
        }

        public void RunInTransaction(Action toRun)
        {
            sdk.ExecuteSdkActionInTransaction(toRun);
        }
    }
}