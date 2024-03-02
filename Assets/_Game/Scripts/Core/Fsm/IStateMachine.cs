using System;

namespace Faraway.Core.FSM
{
    public interface IStateMachine<TEnumState> where TEnumState : struct, Enum
    {
        bool Initialized { get; }

        bool Enabled { get; set; }

        TEnumState PrevState { get; }

        TEnumState CurrentState { get; }

        void SetState(TEnumState state);
    }
}