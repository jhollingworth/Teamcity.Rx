using System;

namespace Teamcity.Rx
{
    internal class EmptyDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}