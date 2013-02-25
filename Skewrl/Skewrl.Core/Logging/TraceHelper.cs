//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Permissions;

namespace Skewrl.Core.Logging
{
    public class TraceHelper
    {
        private static readonly TraceSource Trace;

        static TraceHelper()
        {
            Trace = new TraceSource("TraceSource", SourceLevels.Information);
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public static void Configure(SourceLevels sourceLevels)
        {
            Trace.Switch.Level = sourceLevels;
        }

        public static void TraceVerbose(string format, params object[] args)
        {
            Trace.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        public static void TraceInformation(string format, params object[] args)
        {
            Trace.TraceEvent(TraceEventType.Information, 0, format, args);
        }

        public static void TraceWarning(string format, params object[] args)
        {
            Trace.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        public static void TraceError(string format, params object[] args)
        {
            Trace.TraceEvent(TraceEventType.Error, 0, format, args);
        }
    }
}
