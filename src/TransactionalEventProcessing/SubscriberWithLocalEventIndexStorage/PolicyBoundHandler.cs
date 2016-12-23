namespace SubscriberWithLocalEventIndexStorage
{
    using System;
    using AppliedSystems.Core;
    using AppliedSystems.Data.StoredProcedures;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using Messages;

    public class PolicyBoundHandler : IEventHandler<PolicyBound>
    {
        private readonly StoredProcedureRunnerFactory storedProcedureRunnerFactory;

        public PolicyBoundHandler(StoredProcedureRunnerFactory storedProcedureRunnerFactory)
        {
            this.storedProcedureRunnerFactory = storedProcedureRunnerFactory;
        }

        public void Handle(PolicyBound message)
        {
            Console.WriteLine("Handling policybound {0}", message.Describe());

            storedProcedureRunnerFactory
                .ForStoredProcedureNamed("sproc_insert_risk")
                .WithVarCharInputParameter("@PolicyNumber", message.PolicyNumber.ToString(), 50)
                .WithVarCharInputParameter("@TenantID", message.TenantId, 25)
                .WithVarCharInputParameter("@Risk", message.Risk, 1000)
                .Execute();
        }
    }
}