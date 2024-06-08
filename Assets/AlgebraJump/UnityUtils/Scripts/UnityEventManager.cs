using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AlgebraJump.UnityUtils
{
    public sealed class UnityEventManager : IEventManager, IDisposable
    {
        private readonly UnityEventHolder _eventHolder;
        public float DeltaTime => Time.deltaTime;

        public UnityEventManager()
        {
            _eventHolder = new GameObject("UnityEventHolder").AddComponent<UnityEventHolder>();
            Object.DontDestroyOnLoad(_eventHolder);
        }

        public IDisposable Subscribe(EUnityEvent eventType, Action action)
        {
            switch (eventType)
            {
                case EUnityEvent.Update:
                    _eventHolder.OnUpdateActions.Add(action);
                    break;
                case EUnityEvent.FixedUpdate:
                    _eventHolder.OnFixedUpdateActions.Add(action);
                    break;
                case EUnityEvent.LateUpdate:
                    _eventHolder.OnLateUpdateActions.Add(action);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }

            return new DisposableActionHolder(() =>
            {
                Unsubscribe(eventType, action);
            });
        }

        public IDisposable SubscribeEachSecond(Action<float> action)
        {
            _eventHolder.OnEachSecondUpdateActionsWithParam.Add(action);
            return new DisposableActionHolder(() =>
            {
                _eventHolder.OnEachSecondUpdateActionsWithParam.Remove(action);
            });
        }

        private void Unsubscribe(EUnityEvent eventType, Action action)
        {
            switch (eventType)
            {
                case EUnityEvent.Update:
                    _eventHolder.OnUpdateActions.Remove(action);
                    break;
                case EUnityEvent.FixedUpdate:
                    _eventHolder.OnFixedUpdateActions.Remove(action);
                    break;
                case EUnityEvent.LateUpdate:
                    _eventHolder.OnLateUpdateActions.Remove(action);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
            }
        }

        public void Dispose()
        {
            _eventHolder.Clear();
            Object.Destroy(_eventHolder.gameObject);
        }
    }
    
    public sealed class DisposableActionHolder : IDisposable
    {
        private readonly Action _disposeAction;
        public DisposableActionHolder(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }
        
        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}