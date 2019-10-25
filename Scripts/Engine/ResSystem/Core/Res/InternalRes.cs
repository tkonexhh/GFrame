using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class InternalRes : AbstractRes//加载以esources开头的资源
    {
        public const string PREFIX_KEY = "Resources/";
        public static InternalRes Allocate(string name)
        {
            InternalRes res = ObjectPool<InternalRes>.S.Allocate();
            if (res != null)
            {
                res.name = name;
            }
            return res;
        }


        public static string Name2Path(string name)
        {
            return name.Substring(10);
        }

        public override bool LoadSync()//同步加载
        {
            if (string.IsNullOrEmpty(m_Name))
            {
                return false;
            }

            m_Asset = Resources.Load(Name2Path(m_Name));
            if (m_Asset != null)
            {
                return true;
            }
            return false;
        }

        public override void LoadAsync()//异步加载
        {

        }

        public override void Recycle2Cache()
        {
            ObjectPool<InternalRes>.S.Recycle(this);
        }

        public override void OnCacheReset()
        {

        }
    }
}




