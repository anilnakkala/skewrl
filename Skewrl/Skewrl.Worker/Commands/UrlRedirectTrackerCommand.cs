using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skewrl.Core.Commands;
using Skewrl.Core.AzureStorage;
using Skewrl.Core.Data;
using Skewrl.Core.Data.Model;

namespace Skewrl.Worker.Commands
{
    public class UrlRedirectTrackerCommand : ICommand<UrlRedirectMessage>
    {
        private IUrlTrackerDataSource _UrlTrackerDataSource;

        public UrlRedirectTrackerCommand(IUrlTrackerDataSource UrlTrackerDataSource)
        {
            _UrlTrackerDataSource = UrlTrackerDataSource;
        }

        public bool Run(UrlRedirectMessage message)
        {
            _UrlTrackerDataSource.UpdateVisitData(
                new UrlVisited
                {
                    UrlKey = message.UrlKey,
                    AcceptLanguage = message.AcceptLanguage,
                    ActualUrl = message.ActualUrl,
                    DateVisited = message.DateVisited,
                    VisitorIP = message.VisitorIP,
                    UserAgent = message.UserAgent,
                    Referer = message.Referer
                }
                );
            return true;
        }
    }
}
