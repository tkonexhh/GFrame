using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public static class GameObjectExtension
    {
        public static void DestroySelf(this GameObject obj)
        {
            GameObject.Destroy(obj);
        }

        public static void SetEnable(this GameObject obj, bool enable)
        {
            if (obj.activeSelf == enable)
            {
                return;
            }
            obj.SetActive(enable);
        }

        public static void SetX(this GameObject obj, float x)
        {
            obj.transform.SetX(x);
        }

        public static void SetY(this GameObject obj, float y)
        {
            obj.transform.SetY(y);
        }

        public static void SetZ(this GameObject obj, float z)
        {
            obj.transform.SetZ(z);
        }

        public static void SetLocalX(this GameObject obj, float x)
        {
            obj.transform.SetLocalX(x);
        }

        public static void SetLocalY(this GameObject obj, float y)
        {
            obj.transform.SetLocalY(y);
        }

        public static void SetLocalZ(this GameObject obj, float z)
        {
            obj.transform.SetLocalZ(z);
        }

        public static void LookAtXZ(this GameObject obj, Vector3 targrt)
        {
            obj.transform.LookAtXZ(targrt);
        }

        public static GameObject SetLocalPos(this GameObject obj, Vector3 pos)
        {
            obj.transform.SetLocalPos(pos);
            return obj;
        }

        public static GameObject SetPos(this GameObject obj, Vector3 pos)
        {
            obj.transform.SetPos(pos);
            return obj;
        }

        public static GameObject ResetLocalAngle(this GameObject obj)
        {
            obj.transform.ResetLocalAngle();
            return obj;
        }


        public static void IterateGameObject(this GameObject obj, Action<GameObject> handle)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            queue.Enqueue(obj);
            while (queue.Count != 0)
            {
                GameObject tmpGo = queue.Dequeue();
                foreach (Transform t in tmpGo.transform)
                {
                    queue.Enqueue(t.gameObject);
                }

                if (handle != null)
                {
                    handle(obj);
                }
            }
        }


        public static GameObject Reset(this GameObject obj)
        {
            obj.transform.Reset();
            return obj;
        }

        public static T AddMissingComponent<T>(this GameObject obj) where T : Component
        {
            T comp = obj.GetComponent<T>();
            if (comp == null)
            {
                comp = obj.AddComponent<T>();
            }

            return comp;
        }

        public static T AddMissingComponent<T>(this Transform trans) where T : Component
        {
            T comp = trans.GetComponent<T>();
            if (comp == null)
            {
                comp = trans.gameObject.AddComponent<T>();
            }

            return comp;
        }

    }
}




