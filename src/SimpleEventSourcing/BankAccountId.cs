namespace SimpleEventSourcing
{
    using AppliedSystems.Domain.EventSourced;

    public class BankAccountId : AggregateId
    {
        public static BankAccountId FromAccountNumber(string accountNumber)
        {
            return new BankAccountId(accountNumber);
        }

        private readonly string accountNumber;

        private BankAccountId(string accountNumber)
        {
            this.accountNumber = accountNumber;
        }

        public override string ConvertToStreamName()
        {
            return $"bankaccount-{accountNumber}";
        }

        public override string ToString()
        {
            return ConvertToStreamName();
        }
    }
}