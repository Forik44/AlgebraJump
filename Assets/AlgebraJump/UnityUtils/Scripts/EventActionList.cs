using System;
using System.Collections.Generic;
using UnityEngine.Profiling;

namespace AlgebraJump.UnityUtils
{
    internal sealed class EventAction : IDisposable
    {
        internal Action Action;
        internal bool IsDisposed { get; private set; }

        internal EventAction(Action action)
        {
            Action = action;
        }

        public void Dispose()
        {
            Action = null;
            IsDisposed = true;
        }
    }

    public sealed class EventActionList
    {
        private readonly List<EventAction> _actionsList = new();

        internal void InvokeActions()
        {
            for (var i = 0; i < _actionsList.Count; i++)
            {
                EventAction eventAction = _actionsList[i];
                if (!eventAction.IsDisposed)
                {
                    eventAction.Action.Invoke();
                }
            }
        }
        
        internal void RemoveActions()
        {
            for (int i = _actionsList.Count - 1; i >= 0; i--)
            {
                EventAction eventAction = _actionsList[i];
                if (eventAction.IsDisposed)
                    _actionsList.RemoveAt(i);
            }
        }

        internal void Add(Action action)
        {
            var newAction = new EventAction(action);
            _actionsList.Add(newAction);
        }

        internal void Remove(Action action)
        {
            for (int i = _actionsList.Count - 1; i >= 0; i--)
            {
                EventAction eventAction = _actionsList[i];
                if (eventAction.Action == action)
                    eventAction.Dispose();
            }
        }

        internal void Clear()
        {
            _actionsList.Clear();
        }
    }
}