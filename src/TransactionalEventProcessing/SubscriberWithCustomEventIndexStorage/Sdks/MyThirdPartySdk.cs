namespace SubscriberWithCustomEventIndexStorage.Sdks
{
    using System;
    using System.Collections.Generic;

    public class MyThirdPartySdk
    {
        private readonly Dictionary<string, int> eventIndices;

        public MyThirdPartySdk()
        {
            eventIndices = new Dictionary<string, int>();
        }

        public void ExecuteSdkActionInTransaction(Action toRun)
        {
            try
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine("Starting sdk transaction");
                toRun();
                Console.WriteLine("Completing sdk transaction");
                Console.WriteLine("--------------------------");
            }
            catch (Exception)
            {
                Console.WriteLine("Rolling back sdk transaction");
                throw;
            }
        }

        public void StoreEventIndex(string key, int index)
        {
            Console.WriteLine("Storing event index {0} for key {1} in sdk", index, key);
            eventIndices[key] = index;
        }

        public bool HasEventIndex(string key)
        {
            return eventIndices.ContainsKey(key);
        }

        public int GetEventIndex(string key)
        {
            Console.WriteLine("Getting event index for key {0} from sdk", key);
            return eventIndices[key];
        }

        public void SaveRisk(Guid policyNumber, string tenantId, string risk)
        {
            Console.WriteLine("Saving risk {0} in sdk for policy number {1}, tenant {2}", risk, policyNumber, tenantId);
        }
    }
}