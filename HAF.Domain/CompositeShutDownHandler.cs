using System;
using System.Collections.Generic;
using System.Linq;

namespace  HAF.Domain
{
    public class CompositeShutDownHandler : IApplicationShutDownHandler
    {
        private readonly IApplicationShutDownHandler[] _shutDownHandlers;

        public CompositeShutDownHandler(IEnumerable<IApplicationShutDownHandler> shutDownHandlers)
        {
            if (shutDownHandlers == null)
                throw new ArgumentNullException(nameof(shutDownHandlers));
            _shutDownHandlers = shutDownHandlers.ToArray();
        }

        public void Handle()
        {
            foreach (var shutDownHandler in _shutDownHandlers)
                shutDownHandler.Handle();
        }
    }
}