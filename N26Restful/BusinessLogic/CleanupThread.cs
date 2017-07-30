using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace N26Restful.BusinessLogic
{
    public class CleanupThread
    {
        public static void periodicTask()
        {
            IObservable<long> observable = Observable.Interval(TimeSpan.FromSeconds(1));
            CancellationTokenSource source = new CancellationTokenSource();
            Action action = (() => { Api.deleteFromTransactionalData(); });
            observable.Subscribe(x => {
                Task task = new Task(action); task.Start();
            }, source.Token);
        }
    }
}