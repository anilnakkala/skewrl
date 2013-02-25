//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================

using System.Globalization;
using System;
using System.Text;

namespace Skewrl.Core.Extensions
{
    public static class ExceptionExtensions
    {
        private const string Line = "==============================================================================";

        public static string TraceInformation(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            var exceptionInformation = new StringBuilder();

            exceptionInformation.Append(BuildMessage(exception));

            Exception inner = exception.InnerException;

            while (inner != null)
            {
                exceptionInformation.Append(Environment.NewLine);
                exceptionInformation.Append(Environment.NewLine);
                exceptionInformation.Append(BuildMessage(inner));
                inner = inner.InnerException;
            }

            return exceptionInformation.ToString();
        }

        private static string BuildMessage(Exception exception)
        {
            return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1}{2}:{3}{4}{5}{6}{7}",
                    Line,
                    Environment.NewLine,
                    exception.GetType().Name,
                    exception.Message,
                    Environment.NewLine,
                    exception.StackTrace,
                    Environment.NewLine,
                    Line);
        }
    }

}
