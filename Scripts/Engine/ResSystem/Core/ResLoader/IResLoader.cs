using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public interface IResLoader
    {
        bool Add2Load(string name, Action<bool, IRes> listener = null);
        void ReleaseAllRes();
    }
}




