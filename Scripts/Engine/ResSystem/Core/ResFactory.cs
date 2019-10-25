using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class ResFactory
    {
        public delegate IRes ResCreator(string name);

        public interface IResCreatorWrap
        {
            bool CheckResType(string name);
            IRes CreateRes(string name);
        }

        class ResCreatorWrap : IResCreatorWrap
        {
            private string m_Key;
            private ResCreator m_Creator;

            public ResCreatorWrap(string key, ResCreator creator)
            {
                m_Key = key;
                m_Creator = creator;
            }

            public bool CheckResType(string name)
            {
                return name.StartsWith(m_Key);
            }
            public IRes CreateRes(string name)
            {
                return m_Creator(name);
            }
        }

        class AssetResCreatorWrap : IResCreatorWrap
        {
            public bool CheckResType(string name)
            {
                AssetData data = AssetDataTable.S.GetAssetData(name);
                return data != null;
            }
            public IRes CreateRes(string name)
            {
                AssetData data = AssetDataTable.S.GetAssetData(name);
                switch (data.assetType)
                {
                    case eResType.kAssetBundle:
                        return AssetBundleRes.Allocate(name);
                    case eResType.kABAsset:
                        return AssetRes.Allocate(name);
                    default:
                        return null;
                }
            }
        }

        class LocalAssetResCreatorWrap : IResCreatorWrap
        {
            public bool CheckResType(string name)
            {
                FolderData data = FolderDataTable.S.GetAssetData(name);
                return data != null;
            }
            public IRes CreateRes(string name)
            {
                //FolderData data = FolderDataTable.S.GetAssetData(name);
                return LocalRes.Allocate(name);
            }
        }


        private static AssetResCreatorWrap s_AssetResCreatorWrap;
        private static LocalAssetResCreatorWrap s_LocalAssetResCreatorWrap;

        private static List<IResCreatorWrap> s_CreatorList;
        static ResFactory()
        {
            Log.i("#Init[ResFactory]");
            s_AssetResCreatorWrap = new AssetResCreatorWrap();
            s_LocalAssetResCreatorWrap = new LocalAssetResCreatorWrap();
            s_CreatorList = new List<IResCreatorWrap>();

            RegisterResCreate(InternalRes.PREFIX_KEY, InternalRes.Allocate);
            RegisterResCreate(NetImgRes.PREFIX_KEY, NetImgRes.Allocate);
        }

        public static void RegisterResCreate(string key, ResCreator creator)
        {
            if (creator == null || string.IsNullOrEmpty(key))
            {
                Log.e("#Register InValid Creator.");
                return;
            }

            RegisterResCreateWarp(new ResCreatorWrap(key, creator));
        }

        public static void RegisterResCreateWarp(IResCreatorWrap wrap)
        {
            if (wrap == null)
            {
                Log.e("Register InValid Wrap.");
                return;
            }

            s_CreatorList.Add(wrap);
        }

        public static IRes Create(string name)
        {
            if (s_AssetResCreatorWrap.CheckResType(name))
            {
                return s_AssetResCreatorWrap.CreateRes(name);
            }
            else if (s_LocalAssetResCreatorWrap.CheckResType(name))
            {
                return s_LocalAssetResCreatorWrap.CreateRes(name);
            }
            else
            {
                for (int i = s_CreatorList.Count - 1; i >= 0; --i)
                {
                    if (s_CreatorList[i].CheckResType(name))
                    {
                        return s_CreatorList[i].CreateRes(name);
                    }
                }
            }
            Log.e("#Not Find ResCreater For Res:" + name);
            return null;
        }
    }
}




