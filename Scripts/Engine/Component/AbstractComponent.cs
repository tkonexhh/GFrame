using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AbstractComponent : IComponent
    {
        private AbstractActor m_Actor;
        public AbstractActor actor
        {
            get { return m_Actor; }
        }

        public void OnAwake(AbstractActor actor)
        {
            m_Actor = actor;
            OnComAwake();
        }
        public void OnStart()
        {

        }
        public void OnEnable()
        {

        }
        public void OnUpdate(float dt)
        {
            OnComUpdate(dt);
        }
        public void OnLateUpdate(float dt)
        {
            OnComLateUpdate(dt);
        }
        public void OnDisable()
        {

        }
        public void OnDestroy()
        {
            OnComDestroy();
        }

        #region 子类

        protected virtual void OnComAwake()
        {

        }

        protected virtual void OnComUpdate(float dt)
        {

        }

        protected virtual void OnComLateUpdate(float dt)
        {

        }

        protected virtual void OnComDestroy()
        {

        }
        #endregion
    }
}




