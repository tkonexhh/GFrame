using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class FSMStateMachine<T>
    {
        private T m_Entity;
        private FSMState<T> m_CurrentState;
        private FSMState<T> m_PreviousState;

        public FSMState<T> currentState
        {
            get { return m_CurrentState; }
        }

        public FSMState<T> previousState
        {
            get { return m_PreviousState; }
        }

        public FSMStateMachine(T entity)
        {
            m_Entity = entity;
            m_CurrentState = m_PreviousState = null;
        }


        #region 状态控制
        public void UpdateState(float dt)
        {
            if (currentState != null)
            {
                currentState.Execute(m_Entity, dt);
            }
        }

        public void SetCurrentState(FSMState<T> state)
        {
            if (m_CurrentState == state)
            {
                Log.w("FSM:Set Same State");
            }
            if (state == null)
                return;
            if (m_CurrentState != null)
            {
                m_CurrentState.Exit(m_Entity);
                m_PreviousState = m_CurrentState;
            }


            m_CurrentState = state;
            m_CurrentState.Enter(m_Entity);
            Log.i("StateChange:{0}->{1}", m_PreviousState, m_CurrentState);
        }
        #endregion
    }
}
