using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{
    public class eResState
    {
        public const short kWaiting = 0;
        public const short kLoading = 1;
        public const short kReady = 2;
        public const short kDisposing = 4;

        public static bool isReady(short value)
        {
            return value == kReady;
        }
    }

    public class eResType
    {
        public const short kAssetBundle = 0;
        public const short kABAsset = 1;
        // public const short kABScene = 2;
        // public const short kInternal = 3;
        // public const short kNetImageRes = 4;
        // public const short kHotUpdateRes = 5;
    }

    public interface IRes : IRefCounter, IPoolType
    {
        string name
        {
            get;
        }

        UnityEngine.Object asset
        {
            get;
            //set;
        }


        bool ReleaseRes();

        void RegisterResListener(Action<bool, IRes> listener);
        void UnRegisterResListener(Action<bool, IRes> listener);


        bool LoadSync();//同步加载
        void LoadAsync();//异步加载

        string[] GetDependResList();
    }
}




