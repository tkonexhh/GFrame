using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public interface IPoolAble
    {
        void OnCacheReset();//回收的时候调用
    }
}




