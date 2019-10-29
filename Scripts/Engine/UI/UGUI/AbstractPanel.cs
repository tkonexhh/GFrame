using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{
    public enum PanelType : byte
    {
        Bottom,
        Auto,
        Top,
    }


    [RequireComponent(typeof(Canvas))]
    public class AbstractPanel : AbstractPage
    {
        public void OnPanelOpen(bool firstOpen, params object[] args)
        {

        }

        protected override void OnViewEvent(ViewEvent e, object[] args)
        {
            Debug.LogError(e);
            switch (e)
            {
                case ViewEvent.Action_ClosePanel:
                    if (m_ParentPage == null)
                    {
                        UIMgr.S.ClosePanel(this);
                    }
                    break;
                case ViewEvent.Action_HidePanel:
                    if (m_ParentPage == null)
                    {
                        //customVisibleFlag = false;
                    }
                    break;
                case ViewEvent.Action_ShowPanel:
                    if (m_ParentPage == null)
                    {
                        //customVisibleFlag = true;
                    }
                    break;
                default:
                    base.OnViewEvent(e, args);
                    break;
            }
        }
    }
}




