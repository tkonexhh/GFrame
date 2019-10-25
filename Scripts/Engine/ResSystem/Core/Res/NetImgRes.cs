using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class NetImgRes : AbstractRes
    {
        public const string PREFIX_KEY = "NetImage:";
        private string m_Url;
        private string m_HashCode;


        public static NetImgRes Allocate(string name)
        {
            NetImgRes res = ObjectPool<NetImgRes>.S.Allocate();
            if (res != null)
            {
                res.name = name;
                res.SetUrl(name.Substring(9));
            }
            return res;
        }

        public void SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }
            m_Url = url;
            m_HashCode = string.Format("CacheImg_{0}", m_Url.GetHashCode());
        }

        public override bool LoadSync()//同步加载
        {

            return false;
        }

        public override void LoadAsync()//异步加载
        {
            DownloadImage();
        }

        IEnumerator DownloadImage()
        {
            WWW www = new WWW(m_Url);
            yield return www;

            m_Asset = www.texture;
            Debug.LogError(m_Asset);
            www.Dispose();
            www = null;
        }

        public override void Recycle2Cache()
        {
            ObjectPool<NetImgRes>.S.Recycle(this);
        }

        public override void OnCacheReset()
        {

        }
    }
}




