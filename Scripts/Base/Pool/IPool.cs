using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public interface IPool<T>
    {
        T Allocate();
        bool Recycle(T obj);
    }
}




