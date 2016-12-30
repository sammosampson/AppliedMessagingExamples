namespace SimpleEventSourcing.Messages
{
    using AppliedSystems.Messaging.Messages;

    public class DepositMoneyIntoAccount : ICommand
    {
        public string AccountNumber { get; }
        public decimal ToDeposit { get; }

        public DepositMoneyIntoAccount(string accountNumber, decimal toDeposit)
        {
            AccountNumber = accountNumber;
            ToDeposit = toDeposit;
        }
    }
}