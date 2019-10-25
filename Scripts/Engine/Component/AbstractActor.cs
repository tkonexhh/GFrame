using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class AbstractActor : MonoBehaviour
    {

        private List<IComponent> m_LstComponent = new List<IComponent>();

        private bool m_HasAwake = false;
        private bool m_HasStart = false;

        #region MonoBehaviour

        private void Awake()
        {
            OnActorAwake();
            for (int i = m_LstComponent.Count - 1; i >= 0; --i)
            {
                AwakeComponent(m_LstComponent[i]);
            }
            m_HasAwake = true;
        }


        private void Start()
        {
            OnActorStart();
            for (int i = m_LstComponent.Count - 1; i >= 0; --i)
            {
                StartComponent(m_LstComponent[i]);
            }
            m_HasStart = true;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            for (int i = m_LstComponent.Count - 1; i >= 0; --i)
            {
                m_LstComponent[i].OnUpdate(dt);
            }
        }

        private void LateUpdate()
        {
            float dt = Time.deltaTime;
            for (int i = m_LstComponent.Count - 1; i >= 0; --i)
            {
                m_LstComponent[i].OnLateUpdate(dt);
            }
        }

        private void OnDestroy()
        {
            for (int i = m_LstComponent.Count - 1; i >= 0; --i)
            {
                m_LstComponent[i].OnDestroy();
            }
            m_LstComponent.Clear();

        }
        #endregion




        public void AddComponent(IComponent component)
        {

            if (component == null)
                return;

            if (GetCom(component.GetType()) != null)
            {
                Log.w("Already Add Component:" + name);
                return;
            }

            m_LstComponent.Add(component);
            OnAddComponent(component);

            if (m_HasAwake)
            {
                AwakeComponent(component);
            }
            if (m_HasStart)
            {
                StartComponent(component);
            }
        }

        public T AddComponent<T>() where T : IComponent, new()
        {
            T component = new T();
            AddComponent(component);
            return component;
        }


        public T GetCom<T>() where T : IComponent
        {
            for (int i = m_LstComponent.Count - 1; i >= 0; --i)
            {
                if (m_LstComponent[i] is T)
                {
                    return (T)m_LstComponent[i];
                }
            }
            return default(T);
        }

        private IComponent GetCom(Type t)
        {
            for (int i = m_LstComponent.Count - 1; i >= 0; --i)
            {
                if (m_LstComponent[i].GetType() == t)
                {
                    return m_LstComponent[i];
                }
            }
            return null;
        }

        #region 
        protected virtual void OnActorAwake()
        {

        }

        protected virtual void OnActorStart()
        {

        }

        protected virtual void OnAddComponent(IComponent component)
        {

        }
        #endregion

        #region IComponent 生命周期
        private void AwakeComponent(IComponent component)
        {
            component.OnAwake(this);
        }

        private void StartComponent(IComponent component)
        {
            component.OnStart();
        }

        #endregion


    }
}




