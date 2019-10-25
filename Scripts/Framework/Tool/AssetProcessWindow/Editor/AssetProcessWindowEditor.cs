using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame.UnityEditor
{
    [CustomEditor(typeof(AssetProcessWindow))]
    public class AssetProcessWindowEditor : Editor
    {
        AssetProcessWindow m_AssetProcessWindow;
        public void OnEnable()
        {
            m_AssetProcessWindow = (AssetProcessWindow)target;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            if (GUILayout.Button("批量修改资源！"))
            {
                Debug.LogError("OnInspectorGUI");
            }
        }
    }
}




