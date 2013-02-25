using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skewrl.Core.AzureStorage;
using Skewrl.Core.Commands;
using System.Threading.Tasks;
using Skewrl.Core.Logging;
using Skewrl.Core.Extensions;

namespace Skewrl.Core.QueueHandlers
{
    public static class QueueHandler
    {
        public static QueueHandler<T> For<T>(IAzureQueue<T> queue) where T : AzureQueueMessage
        {
            return QueueHandler<T>.For(queue);
        }
    }

    public class QueueHandler<T> : GenericQueueHandler<T> where T : AzureQueueMessage
    {
        private readonly IAzureQueue<T> queue;
        private TimeSpan interval;

        protected QueueHandler(IAzureQueue<T> queue)
        {
            this.queue = queue;
            this.interval = TimeSpan.FromMilliseconds(200);
        }

        public static QueueHandler<T> For(IAzureQueue<T> queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }

            return new QueueHandler<T>(queue);
        }

        public QueueHandler<T> Every(TimeSpan intervalBetweenRuns)
        {
            this.interval = intervalBetweenRuns;

            return this;
        }

        public virtual void Do(ICommand<T> command)
        {
            Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        this.Cycle(command);
                    }
                },
                TaskCreationOptions.LongRunning);
        }

        protected void Cycle(ICommand<T> command)
        {
            try
            {
                GenericQueueHandler<T>.ProcessMessages(this.queue, this.queue.GetMessages(1), command.Run);

                this.Sleep(this.interval);
            }
            catch (TimeoutException ex)
            {
                TraceHelper.TraceWarning(ex.TraceInformation());
            }
            catch (Exception ex)
            {
                // no exception should get here - we don't want the handler to stop (we log it as ERROR)
                TraceHelper.TraceError(ex.TraceInformation());
            }
        }
    }
}
