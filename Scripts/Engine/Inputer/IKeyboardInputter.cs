using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public interface IKeyboardInputter
    {
        void RegisterKeyCodeMonitor(KeyCode code, Run begin, Run press, Run end);//普通点击
        void RegisterShortcuts(KeyCode[] code, Run end);//快捷键
        void RegisterKeyCodeQueue(KeyCode[] code, Run end);//连招


        void OnLateUpdate();
    }
}




