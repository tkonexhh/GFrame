using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class UIMgr : TMonoSingleton<UIMgr>
    {
        private const string UIROOTPATH = "Resources/UI/UIRoot";

        private EventSystem m_UIEventSystem = ObjectPool<EventSystem>.S.Allocate();
        private UIRoot m_UIRoot;

        public UIRoot uiRoot
        {
            get
            {
                return m_UIRoot;
            }
        }

        public EventSystem uiEventSystem
        {
            get
            {
                return m_UIEventSystem;
            }
        }
        public override void OnSingletonInit()
        {
            if (m_UIRoot == null)
            {
                UIRoot root = GameObject.FindObjectOfType<UIRoot>();
                if (root == null)
                {
                    root = LoadUIRoot();
                }

                m_UIRoot = root;
                if (m_UIRoot == null)
                    Log.e("Error:UIRoot Is Null.");
            }
        }

        public void Init() { }

        private UIRoot LoadUIRoot()
        {
            ResLoader loader = ResLoader.Allocate("UIMgr");
            // loader.Add2Load(UIROOTPATH);
            // loader.LoadSync();

            UnityEngine.Object uiRootObj = loader.LoadSync(UIROOTPATH);
            if (uiRootObj == null)
            {
                Log.e("Failed To Load UIRoot at" + UIROOTPATH);
                return null;
            }
            GameObject uiRootGo = GameObject.Instantiate(uiRootObj as GameObject);
            return uiRootGo.GetComponent<UIRoot>();
        }


        public void OpenPanel<T>(T uiID, params object[] args) where T : IConvertible
        {
            OpenPanel(uiID, PanelType.Auto, null, args);
        }

        public void OpenPanel<T>(T uiID, PanelType panelType, Action<AbstractPanel> listener, params object[] args) where T : IConvertible
        {
            UIData uIData = UIDataTable.Get(uiID);
            if (uIData == null)
            {
                Log.e("#Not find UIID:" + uiID);
                return;
            }

            if (m_Loader == null)
                m_Loader = ResLoader.Allocate("UIMGR");

            GameObject prefab = m_Loader.LoadSync(uIData.fullPath) as GameObject;
            GameObject obj = GameObject.Instantiate(prefab);
            var panel = obj.GetComponent<AbstractPanel>();
            if (panel == null) return;

            obj.transform.SetParent(uiRoot.panelRoot);
            obj.transform.Reset();

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.SetAnchor(AnchorPresets.StretchAll);
            rect.SetSize(new Vector2(uiRoot.rootCanvas.pixelRect.width, uiRoot.rootCanvas.pixelRect.height));

        }

        ResLoader m_Loader;

    }
}




