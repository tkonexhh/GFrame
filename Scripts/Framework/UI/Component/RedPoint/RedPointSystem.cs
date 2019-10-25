using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class RedPointSystem : TMonoSingleton<RedPointSystem>
    {
        public delegate void OnPointNumChange(RedPointNode node);
        RedPointNode m_RootNode;
        static Dictionary<string, RedPointNode> m_RedPointMap = new Dictionary<string, RedPointNode>();

        public override void OnSingletonInit()
        {
            m_RootNode = new RedPointNode("Root");
        }

        public void AddRedPointNode(RedPointNode node)
        {
            m_RootNode.AddChild(node);
            m_RedPointMap.Add(node.Name, node);
        }

        public void Refesh()
        {
            foreach (var key in m_RedPointMap.Keys)
            {
                m_RedPointMap[key].Refesh();
            }
        }

        public RedPointNode GetRedPointNode(string key)
        {
            return m_RootNode.GetRedPointNode(key);
        }

    }
}
