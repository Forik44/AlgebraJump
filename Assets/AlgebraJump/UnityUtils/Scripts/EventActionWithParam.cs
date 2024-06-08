using System;
using System.Collections.Generic;

namespace AlgebraJump.UnityUtils
{
    public class EventActionWithParam<T> : IDisposable
    {
        public Action<T> Action;
        public bool IsDisposed { get; private set; }

        public EventActionWithParam(Action<T> action)
        {
            Action = action;
        }

        public void Dispose()
        {
            Action = null;
            IsDisposed = true;
        }
    }

    public class EventActionWithParamList<T>
    {
        private readonly List<EventActionWithParam<T>> _actionsList = new();

        public void InvokeActions(T value)
        {
            for (var i = 0; i < _actionsList.Count; i++)
            {
                EventActionWithParam<T> eventActionWithParam = _actionsList[i];
                if (!eventActionWithParam.IsDisposed)
                {
                    eventActionWithParam.Action.Invoke(value);
                }
            }
        }

        public void Add(Action<T> action)
        {
            EventActionWithParam<T> newActionWithParam = new EventActionWithParam<T>(action);
            _actionsList.Add(newActionWithParam);
        }

        public void Remove(Action<T> action)
        {
            for (int i = _actionsList.Count - 1; i >= 0; i--)
            {
                EventActionWithParam<T> eventActionWithParam = _actionsList[i];
                if (eventActionWithParam.Action == action)
                    eventActionWithParam.Dispose();
            }
        }
        public void Clear()
        {
            _actionsList.Clear();
        }

        public void RemoveActions()
        {
            for (int i = _actionsList.Count - 1; i >= 0; i--)
            {
                EventActionWithParam<T> eventAction = _actionsList[i];
                if (eventAction.IsDisposed)
                {
                    _actionsList.RemoveAt(i);
                }
            }
        }
    }
}