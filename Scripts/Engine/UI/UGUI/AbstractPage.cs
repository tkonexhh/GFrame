using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AbstractPage : MonoBehaviour
    {
        [SerializeField] protected AbstractPage m_ParentPage;

        protected Canvas m_Canvas;
        public Canvas Canvas
        {
            get { return m_Canvas; }
        }

        private bool m_HasInitUI = false;
        private bool m_HasOpen = false;
        private int m_PanelID = -1;
        protected int m_UIID;



        public int UIID
        {
            get { return m_UIID; }
            set
            {
                m_UIID = value;
                m_PanelID = value;
            }
        }


        public int GetParentPanelID()
        {
            if (m_ParentPage == null)
            {
                return m_PanelID;
            }
            return m_ParentPage.GetParentPanelID();
        }

        public void SendViewEvent(ViewEvent key, params object[] args)
        {
            UIMgr.S.uiEventSystem.Send(GetParentPanelID(), key, args);
        }

        public void CloseSelfPanel()
        {
            SendViewEvent(ViewEvent.Action_ClosePanel);
        }

        public void HideSelfPanel()
        {
            SendViewEvent(ViewEvent.Action_HidePanel);
        }

        public void ShowSelfPanel()
        {
            SendViewEvent(ViewEvent.Action_ShowPanel);
        }


        #region life time

        private void Awake()
        {
            if (!m_HasInitUI)
            {
                m_HasInitUI = true;
                m_Canvas = transform.AddMissingComponent<Canvas>();
                RegisterParentPanelEvent();
                OnUIInit();
            }
        }

        private void OnDestroy()
        {
            ClosePage();

            UnRegisterParentPanelEvent();

            BeforDestroy();
        }

        #endregion

        private void RegisterParentPanelEvent()
        {
            int panelID = GetParentPanelID();
            Debug.LogError("panelID" + panelID);
            if (panelID > 0)
            {
                UIMgr.S.uiEventSystem.Register(panelID, OnParentPanelEvent);
            }
        }

        private void UnRegisterParentPanelEvent()
        {
            int panelID = GetParentPanelID();
            if (panelID > 0)
            {
                UIMgr.S.uiEventSystem.UnRegister(panelID, OnParentPanelEvent);
            }
        }

        protected void OnParentPanelEvent(int key, params object[] args)
        {
            Debug.LogError("OnParentPanelEvent");
            if (args == null || args.Length <= 0)
            {
                return;
            }

            ViewEvent e = (ViewEvent)args[0];
            switch (e)
            {
                case ViewEvent.OnPanelClose:
                    ClosePage();
                    break;
                case ViewEvent.OnPanelOpen:
                    OpenPage();
                    break;
                case ViewEvent.OnParamUpdate:
                    //ERunner.Run(OnParamUpdate);
                    OnParamUpdate();
                    break;
                case ViewEvent.OnSortingLayerUpdate:
                    //ERunner.Run(OnSortingLayerUpdate);
                    OnSortingLayerUpdate();
                    break;
                default:
                    break;
            }

            OnViewEvent(e, args);
        }

        private void ClosePage()
        {
            if (m_HasOpen)
            {
                m_HasOpen = false;
                OnClose();
            }
        }

        private void OpenPage()
        {
            if (!m_HasOpen)
            {
                m_HasOpen = false;
                OnOpen();
            }
        }

        #region 子类重载
        //面板初始化第一次打开时）


        protected virtual void OnUIInit()
        {

        }

        /************************************************************************/
        /* 面板开启进入，可重入界面会多次进入*/
        /************************************************************************/
        protected virtual void OnOpen()
        {

        }

        //面板被关闭的时候进入
        protected virtual void OnClose()
        {

        }

        protected virtual void OnSortingLayerUpdate()
        {

        }

        protected virtual void OnParamUpdate()
        {

        }

        protected virtual void OnViewEvent(ViewEvent e, object[] args)
        {

        }

        protected virtual void BeforDestroy()
        {

        }

        #endregion
    }
}




