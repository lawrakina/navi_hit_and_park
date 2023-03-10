using System;

namespace EventSystem.Runtime.Core.Dispose
{
    public class DisposeContainer
    {
        private Action _disposeAction;

        public DisposeContainer(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }
        
        public void Invoke()
        {
            _disposeAction?.Invoke();
            _disposeAction = null;
        }
    }
}
