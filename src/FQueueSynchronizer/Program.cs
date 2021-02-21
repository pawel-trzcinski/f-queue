using System;
using System.Diagnostics;
using System.Threading;
using dotnet_etcd;
using Etcdserverpb;
using Google.Protobuf;

namespace FQueueSynchronizer
{
    public static class Program
    {
        private const string SERVER_URI = "http://127.0.0.1:2379";
        private const string LE = "leader-election";
        private const int TTL = 5;
        private static readonly long LEASE_ID = new Random().Next();
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public static void Main(string[] args)
        {
#warning TODO - onyl and always one argument - etcd URI
            try
            {
                using (EtcdClient client = new EtcdClient(SERVER_URI))
                {
                    var transactionRequest = new TxnRequest();
                    transactionRequest.Compare.Add
                    (
                        new Compare
                        {
                            Key = ByteString.CopyFromUtf8("leader-election"),
                            Target = Compare.Types.CompareTarget.Create,
                            Result = Compare.Types.CompareResult.Greater,
                            CreateRevision = 0
                        }
                    );

                    transactionRequest.Success.Add(new RequestOp
                    {
                        RequestDeleteRange = new DeleteRangeRequest
                        {
                            Key = ByteString.CopyFromUtf8("leader-election")
                        }
                    });

                    var response = client.Transaction(transactionRequest);
                    WriteMessage(response.ToString());

                    var leaseGrantResponse = client.LeaseGrant(new LeaseGrantRequest {TTL = TTL, ID = LEASE_ID});
                    WriteMessage(leaseGrantResponse.ToString());
                    client.LeaseKeepAlive(LEASE_ID, tokenSource.Token);
                }

                int index = 0;
                while (true)
                {
                    ++index;
                    using (EtcdClient client = new EtcdClient(SERVER_URI))
                    {
                        RangeResponse response = client.Get(LE);

                        WriteMessage($"Value in DB: {(response.Kvs.Count == 0 ? String.Empty : response.Kvs[0].Value.ToStringUtf8())}");
                    }

                    string newValue = Guid.NewGuid().ToString("N");
                    WriteMessage($"Requested new value: {newValue}");

                    Thread.Sleep(TimeSpan.FromMilliseconds(100));

                    using (EtcdClient client = new EtcdClient(SERVER_URI))
                    {
                        var transactionRequest = new TxnRequest();
                        transactionRequest.Compare.Add
                        (
                            new Compare
                            {
                                Key = ByteString.CopyFromUtf8("leader-election"),
                                Target = Compare.Types.CompareTarget.Create,
                                Result = Compare.Types.CompareResult.Greater,
                                CreateRevision = 0
                            }
                        );

                        transactionRequest.Failure.Add(new RequestOp
                        {
                            RequestPut = new PutRequest
                            {
                                Key = ByteString.CopyFromUtf8("leader-election"),
                                Value = ByteString.CopyFromUtf8(newValue),
                                Lease = LEASE_ID
                            }
                        });

                        var response = client.Transaction(transactionRequest);
                        WriteMessage(response.ToString());
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(2));

                    if (index % 10 == 0)
                    {
                        WriteMessage("Stopping KeepAlive");
                        tokenSource.Cancel();

                        WriteMessage($"Sleeping for {TTL + 1}s");
                        Thread.Sleep(TimeSpan.FromSeconds(TTL + 1));

                        //Console.WriteLine("Revoking lease");
                        //using (EtcdClient client = new EtcdClient(SERVER_URI))
                        //{
                        //    client.LeaseRevoke(new LeaseRevokeRequest { ID = LEASE_ID });
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                WriteMessage(ex.ToString());
            }
        }

        private static void WriteMessage(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}