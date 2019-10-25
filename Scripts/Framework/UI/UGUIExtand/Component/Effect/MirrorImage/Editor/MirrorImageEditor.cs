﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GFrame.UnityEditor
{

    [CustomEditor(typeof(MirrorImage))]
    public class MirrorImageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MirrorImage mirror = target as MirrorImage;
            EditorGUILayout.Space();
            if (GUILayout.Button("Set Native Size"))
            {
                mirror.SetNativeSize();
            }
        }
    }
}
