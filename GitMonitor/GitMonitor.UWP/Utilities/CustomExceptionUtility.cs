using System;

namespace GitMonitor.UWP.Utilities
{
    [Serializable]
    internal class CustomExceptionUtility : Exception
    {
        public CustomExceptionUtility(string message) : base(message)
        {
        }
    }
}