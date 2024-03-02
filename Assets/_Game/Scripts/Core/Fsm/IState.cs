using System;

namespace Faraway.Core.FSM
{
    public interface IState :  IDisposable
    {
        void OnEnter();

        void OnExit();
    }
}