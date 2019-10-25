using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GFrame
{

    public class RedPointNode
    {
        private string m_NodeName;
        private bool m_Value;
        private int m_PointNum = 0;

        public RedPointNode parent = null;
        private RedPointUI m_UI;
        private RedPointSystem.OnPointNumChange numChangeFunc;

        //下一层红点节点
        private Dictionary<string, RedPointNode> m_ChildMap = new Dictionary<string, RedPointNode>();

        public int Num
        {
            get { return m_PointNum; }
        }

        public string Name
        {
            get { return m_NodeName; }
        }

        public RedPointNode(string name)
        {
            m_NodeName = name;
        }


        public void AddChild(RedPointNode node)
        {
            if (!m_ChildMap.ContainsKey(node.m_NodeName))
            {
                m_ChildMap.Add(node.m_NodeName, node);
                node.parent = this;
            }
            else
            {
                Log.w("Already add child:" + node.m_NodeName);
            }

            Refesh();
        }

        public void SetNum(int num)
        {
            if (num < 0) return;
            m_PointNum = num;
            UpdateUI();
            if (parent != null)
            {
                parent.Refesh();
            }
        }

        public void Refesh()
        {
            int num = 0;
            foreach (var key in m_ChildMap.Keys)
            {
                m_Value |= m_ChildMap[key].m_Value;
                num += m_ChildMap[key].m_PointNum;
            }

            m_PointNum = num;
            UpdateUI();
            if (parent != null)
            {
                parent.Refesh();
            }
        }
        public void BindUI(RedPointUI ui)
        {
            m_UI = ui;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_UI != null)
            {
                m_UI.UpdateUI(m_PointNum);
            }
        }

        public RedPointNode GetRedPointNode(string key)
        {
            RedPointNode node = null;
            if (m_ChildMap.TryGetValue(key, out node))
            {
                return node;
            }

            Log.e("Not Find RedPoint:" + key);
            return null;
        }
    }
}
