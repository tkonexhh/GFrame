using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public interface IEventReceiver
    {
        void OnEventHandler(int key, params object[] args);
    }
}
