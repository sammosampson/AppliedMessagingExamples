namespace SimpleEventSourcing.Messages
{
    using AppliedSystems.Messaging.Messages;

    public class WithdrawMoneyFromAccount : ICommand
    {
        public string AccountNumber { get; }
        public decimal ToWithdraw { get; }

        public WithdrawMoneyFromAccount(string accountNumber, decimal toWithdraw)
        {
            AccountNumber = accountNumber;
            ToWithdraw = toWithdraw;
        }
    }
}