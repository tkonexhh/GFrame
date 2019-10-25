using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AssetRes : BaseRes
    {
        private string m_AssetBundleName;
        public AssetRes(string name) : base(name)
        {

        }

        public AssetRes()
        {

        }

        public static AssetRes Allocate(string name)
        {
            AssetRes res = ObjectPool<AssetRes>.S.Allocate();
            if (res != null)
            {
                res.name = name;
                res.InitAssetBundleName();
            }
            return res;
        }

        protected void InitAssetBundleName()
        {
            AssetData asset = AssetDataTable.S.GetAssetData(m_Name);
            if (asset == null)
            {
                Log.e("#Not Find AssetData For Asset:" + m_Name);
                return;
            }

            string assetBundleName = AssetDataTable.S.GetAssetBundleNameByAssetName(asset.assetName);
            if (string.IsNullOrEmpty(assetBundleName))
            {
                Log.e("#Not Find AssetBundle In Table:" + asset.assetName);
                return;
            }

            m_AssetBundleName = assetBundleName;
        }

        public override bool LoadSync()//同步加载
        {
            if (string.IsNullOrEmpty(m_AssetBundleName))
            {
                return false;
            }

            AssetBundleRes abRes = ResMgr.S.GetRes(m_AssetBundleName) as AssetBundleRes;
            //AssetBundleRes abRes = ResMgr.S.GetRes<AssetBundleRes>(m_AssetBundleName);

            if (abRes == null || abRes.assetBundle == null)
            {
                Log.e("#Failed to Load Asset,Not Find AB :" + m_AssetBundleName);
                return false;
            }

            Object asset = abRes.assetBundle.LoadAsset(m_Name);
            if (asset == null)
            {
                Log.e("#Failed To Load Assset:" + m_Name);
                return false;
            }
            m_Asset = asset;
            return true;
        }
        public override void LoadAsync()//异步加载
        {

        }

        public override void Recycle2Cache()
        {
            ObjectPool<AssetRes>.S.Recycle(this);
        }

        public override void OnCacheReset()
        {
            m_AssetBundleName = null;
        }
    }
}




