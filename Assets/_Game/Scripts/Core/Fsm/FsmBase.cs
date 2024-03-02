using System;
using System.Collections.Generic;
using UnityEngine;

namespace Faraway.Core.FSM
{
    /// <summary>
    /// More complex FSM with enum state, and IState(struct or class)
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="TEnum"></typeparam>
    public class FSMBase<TState, TEnum> : IDisposable,
        IStateMachine<TEnum>
        where TState : IState
        where TEnum : struct, Enum
    {
        public event Action<TEnum, TEnum> OnStateChanged;

        private readonly Stack<TState> _exitStates;
        private readonly Dictionary<TEnum, TState> _states = new();

        private TEnum _prevStateType;
        private TEnum _currentStateType;

        public TEnum CurrentState => _currentStateType;
        public TEnum PrevState => _prevStateType;

        private bool _initialized;
        private bool _enabled = true;

        private readonly bool _showLogs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showLogs"></param>
        public FSMBase(bool showLogs = false)
        {
            _showLogs = showLogs;
            _exitStates = new Stack<TState>(1);
        }


        /// <summary>
        /// Enable/Disable SetState
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public bool Initialized => _initialized;

        /// <summary>
        /// Add New State
        /// Can't add after Initialize call
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void AddState(TEnum id, TState state)
        {
            if (_initialized) throw new Exception($"State must be added before Initialize call");
            _states.Add(id, state);
            StateAddInternal(id, state);
        }
        
        public void Initialize(TEnum startupState = default)
        {
            if (_initialized) return;
            _initialized = true;
            BeforeStateChange(startupState);
            InternalChangeState(startupState);
        }

 


        /// <summary>
        /// Main FSM method for transitions
        /// Check removed states with fallback states
        /// need Enabled and Initialized before call
        /// </summary>
        /// <param name="stateType"></param>
        public void SetState(TEnum stateType)
        {
            if (!_enabled || !_initialized)
            {
                Debug.Assert(!_enabled, $"{this.GetType().Name} is Disabled, can't change state:{stateType}");
                Debug.Assert(!_initialized,
                    $"{this.GetType().Name} is not Initialized, can't change state:{stateType}");
                return;
            }
            BeforeStateChange(stateType);
            InternalChangeState(stateType);
        }


        /// <summary>
        /// After than need create new instance for FSM
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var statesValue in _states)
            {
                statesValue.Value.Dispose();
            }

            _states.Clear();
            _exitStates.Clear();
        }


        private void InternalChangeState(TEnum stateType)
        {
            if (EqualityComparer<TEnum>.Default.Equals(_currentStateType, stateType)) return;

            _prevStateType = _currentStateType;
            if (_exitStates.Count > 0)
            {
                _exitStates.Pop().OnExit();
            }
            
            if (_states.TryGetValue(_currentStateType, out var state))
            {
                _exitStates.Push(state);
                state.OnEnter();
            }
            else
            {
                Log($"{this.GetType().Name} transition failed:{_currentStateType} not exist!", true);
            }

            OnStateChanged?.Invoke(_prevStateType, _currentStateType);
        }

        private void Log(string message, bool error = false)
        {
            if (!_showLogs) return;
            LogInternal(message, error);
        }

        protected virtual void LogInternal(string message, bool error = false)
        {
            if (error)
            {
                Debug.LogError(message);
            }
            else
            {
                Debug.Log($"{this.GetType().Name} transition from:{_prevStateType}, to:{_currentStateType}");
            }
        }
        
        
        protected TState GetState(TEnum stateId)
        {
            return _states.GetValueOrDefault(stateId);
        }
        
        protected virtual void BeforeStateChange(TEnum state)
        {
            
        }

        protected virtual void StateAddInternal(TEnum stateId, TState state)
        {
        }
    }
}