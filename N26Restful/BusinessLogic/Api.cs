using N26Restful.Helper;
using N26Restful.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N26Restful.BusinessLogic
{
    [Serializable]
    public class Api
    {
        private static Dictionary<long, List<double>> transactionalData { get; set; }
        private static object lock1 = new object(), lock2 = new object();
        private static Statistics stats = new Statistics();
        //private static double sum = 0, avg = 0, min = 0, max = 0;
        //private static int count = 0;
        private static bool initialFlag = false;

        public static bool addToTransactionalData(long timestamp, double amount)
        {
            try
            {
                lock (lock1)
                {
                    if (transactionalData == null)
                    {
                        transactionalData = new Dictionary<long, List<double>>();
                    }
                    if (transactionalData.ContainsKey(timestamp))
                    {
                        List<double> value = transactionalData[timestamp];
                        value.Add(amount);
                    }
                    else
                    {
                        List<double> value = new List<double>();
                        value.Add(amount);
                        transactionalData.Add(timestamp, value);
                    }
                    lock (lock2)
                    {
                        stats.Sum += amount;
                        stats.Count++;
                        stats.Avg = stats.Sum / stats.Count;
                        if (!initialFlag)
                        {
                            stats.Min = amount;
                            initialFlag = true;
                            CleanupThread.periodicTask();
                        }
                        else
                        {
                            stats.Min = stats.Min > amount ? amount : stats.Min;
                        }
                        stats.Max = stats.Max < amount ? amount : stats.Max;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Statistics getStats()
        {
            lock (lock2)
            {
                return stats;
            }
        }

        public static bool deleteFromTransactionalData()
        {
            try
            {
                if (transactionalData.Count > 0)
                {
                    long _60SecOldTime = UtcHelper.ToUnixTime(DateTime.UtcNow.AddSeconds(-60));
                    lock (lock1)
                    {
                        var toBeDeleted = transactionalData.Where(x => x.Key < _60SecOldTime).ToList();
                        if (toBeDeleted.Count > 0)
                        {
                            lock (lock2)
                            {
                                foreach (var item in toBeDeleted)
                                {
                                    transactionalData.Remove(item.Key);
                                    foreach (var value in item.Value)
                                    {
                                        stats.Sum -= value;
                                        stats.Count--;
                                    }
                                }
                                stats.Avg = stats.Sum / stats.Count;
                                if (transactionalData.Count > 0)
                                {
                                    stats.Min = transactionalData.Min(x => x.Value.Min());
                                    stats.Max = transactionalData.Max(x => x.Value.Max());
                                }
                                else
                                {
                                    stats.Min = 0;
                                    stats.Max = 0;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}