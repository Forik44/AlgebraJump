using UnityEngine;

namespace AlgebraJump.UnityUtils
{
    public class UnityEventHolder : MonoBehaviour
    {
        public EventActionList OnUpdateActions = new();
        public EventActionList OnLateUpdateActions = new();
        public EventActionWithParamList<float> OnEachSecondUpdateActionsWithParam = new();
        public EventActionList OnFixedUpdateActions = new();
        public EventActionList OnGizmosActions = new();

        private float _secondElapsedTime;
        
        public void Clear()
        {
            OnUpdateActions.Clear();
            OnLateUpdateActions.Clear();
            OnEachSecondUpdateActionsWithParam.Clear();
            OnFixedUpdateActions.Clear();
            OnGizmosActions.Clear();
        }
        
        internal void Update()
        {
            OnUpdateActions.InvokeActions();
            _secondElapsedTime += Time.unscaledDeltaTime;
            if (_secondElapsedTime >= 1f)
            {
                OnEachSecondUpdateActionsWithParam.InvokeActions(_secondElapsedTime);
                _secondElapsedTime = 0;
            }
        }

        private void LateUpdate()
        {
            OnLateUpdateActions.InvokeActions();
     
            OnUpdateActions.RemoveActions();
            OnLateUpdateActions.RemoveActions();
            OnEachSecondUpdateActionsWithParam.RemoveActions();
            OnFixedUpdateActions.RemoveActions();
            OnGizmosActions.RemoveActions();
        }
        
        private void FixedUpdate()
        {
            OnFixedUpdateActions.InvokeActions();
        }
    }
}