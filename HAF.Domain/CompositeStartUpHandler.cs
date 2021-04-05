using System;
using System.Collections.Generic;
using System.Linq;

namespace  HAF.Domain
{
    public class CompositeStartUpHandler : IApplicationStartUpHandler
    {
        private readonly IApplicationStartUpHandler[] _startUpHandlers;

        public CompositeStartUpHandler(IEnumerable<IApplicationStartUpHandler> startUpHandlers)
        {
            if (startUpHandlers == null)
                throw new ArgumentNullException(nameof(startUpHandlers));
            _startUpHandlers = startUpHandlers.ToArray();
        }

        public void Handle()
        {
            foreach (var startUpHandler in _startUpHandlers)
                startUpHandler.Handle();
        }
    }
}