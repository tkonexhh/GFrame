using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public enum BindType
    {
        Transform,
        GameObject,
        Text,
        Image,
        Button,
        Toggle,
        Slider,
    }
    public class AutoBind : MonoBehaviour
    {
        public BindType type;
        public string attrName = "";

        private void Reset()
        {
            attrName = transform.name;
        }

    }
}




