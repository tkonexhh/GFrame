using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class I18Mgr : TSingleton<I18Mgr>
    {
        private SystemLanguage m_Language = SystemLanguage.Unknown;


        public void Init()
        {
            CheckLanguageEnvironment();
            Log.i("#Init[I18Mgr]");
        }

        private void CheckLanguageEnvironment()
        {

        }
    }
}




