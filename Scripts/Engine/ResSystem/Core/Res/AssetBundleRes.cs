using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AssetBundleRes : AbstractRes
    {
        public static AssetBundleRes Allocate(string name)
        {
            AssetBundleRes res = ObjectPool<AssetBundleRes>.S.Allocate();
            if (res != null)
            {
                res.name = name;
                res.InitAssetBundleName();
            }
            return res;
        }

        public AssetBundle assetBundle
        {
            get
            {
                return (AssetBundle)m_Asset;
            }
            set { m_Asset = value; }
        }


        public AssetBundleRes(string name) : base(name)
        {

        }
        public AssetBundleRes()
        {

        }

        private void InitAssetBundleName()
        {
            string path = ProjectPathConfig.AssetBundleName2FullPath(m_Name);
            AssetBundleCreateRequest abcR = AssetBundle.LoadFromFileAsync(path);
            assetBundle = abcR.assetBundle;
        }


        public override bool LoadSync()//同步加载
        {
            return false;
        }
        public override void LoadAsync()//异步加载
        {

        }

        public override void Recycle2Cache()
        {
            ObjectPool<AssetBundleRes>.S.Recycle(this);
        }

        public override void OnCacheReset()
        {

        }
    }
}




