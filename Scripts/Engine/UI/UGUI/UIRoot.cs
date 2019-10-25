using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class UIRoot : MonoBehaviour
    {
        [SerializeField] Transform m_PanelRoot;
        [SerializeField] Transform m_HideRoot;
        [SerializeField] Camera m_Camera;
        [SerializeField] Canvas m_RootCanvas;


        public const int FLOAT_PANEL_INDEX = 20000001;
        public const int TOP_PANEL_INDEX = 10000000;
        private const int PANEL_ORDER_STEP = 10;

        private Vector3 m_NextHidePos = new Vector3(5000, 5000, 0);
        private int m_AutoPanelOrder = 10;
        private int m_TopPanelOrder = TOP_PANEL_INDEX;

        public Transform panelRoot
        {
            get { return m_PanelRoot; }
        }

        public Transform hideRoot
        {
            get { return m_HideRoot; }
        }

        public Camera uiCamera
        {
            get { return m_Camera; }
        }

        public Canvas rootCanvas
        {
            get { return m_RootCanvas; }
        }


        public int RequirePanelSortingOrder(PanelType type)
        {
            switch (type)
            {
                case PanelType.Auto:
                    m_AutoPanelOrder += PANEL_ORDER_STEP;
                    return m_AutoPanelOrder;
                case PanelType.Top:
                    m_TopPanelOrder += PANEL_ORDER_STEP;
                    return m_TopPanelOrder;
                case PanelType.Bottom:
                    return 0;
            }
            return m_AutoPanelOrder;
        }

        // public void ReleasePanelSortingOrder(int sortingIndex)
        // {
        //     if(m_AutoPanelOrder==sortingIndex)
        //     {

        //     }
        // }

        public Vector3 RequireNextHidePos()
        {
            m_NextHidePos.y += 1000;
            return m_NextHidePos;
        }

        public void ReleaseFreePos(Vector3 pos)
        {
            if (m_NextHidePos.y == pos.y)
            {
                m_NextHidePos.y -= 1000;
            }
        }


    }
}




