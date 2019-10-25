using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AutoBindHelper
    {
        static public string AutoName(AutoBind bind)
        {
            string attrName = bind.attrName;
            if (!attrName.StartsWith("m_"))
            {
                attrName = "m_" + attrName;
            }
            return attrName;
        }
    }
}




