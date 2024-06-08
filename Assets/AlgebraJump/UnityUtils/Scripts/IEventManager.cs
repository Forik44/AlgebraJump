using System;

namespace AlgebraJump.UnityUtils
{
    public interface IEventManager
    {
        float DeltaTime { get; }
        IDisposable Subscribe(EUnityEvent eventType, Action action);
        IDisposable SubscribeEachSecond(Action<float> action);
    }
}