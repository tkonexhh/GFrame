using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class LanguageDefine
    {
        public static ZoneInfo CHINA = new ZoneInfo("cn");


        public class ZoneInfo
        {
            public string name;

            public ZoneInfo(string name)
            {
                this.name = name;
            }
        }
    }
}




