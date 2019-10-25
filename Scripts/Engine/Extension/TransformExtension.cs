using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public static class TransformExtension
    {
        public static void SetX(this Transform transform, float x)
        {
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        public static void SetY(this Transform transform, float y)
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        public static void SetZ(this Transform transform, float z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }

        public static void SetLocalX(this Transform transform, float x)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        public static void SetLocalY(this Transform transform, float y)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        public static void SetLocalZ(this Transform transform, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }

        public static void LookAtXZ(this Transform transform, Vector3 targrt)
        {
            transform.LookAt(new Vector3(targrt.x, transform.position.y, targrt.z));
        }

        public static void SetLocalPos(this Transform transform, Vector3 pos)
        {
            transform.localPosition = pos;
        }

        public static void SetPos(this Transform transform, Vector3 pos)
        {
            transform.position = pos;
        }

        public static void ResetLocalAngle(this Transform transform)
        {
            transform.localEulerAngles = Vector3.zero;
        }

        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        static public List<Transform> GetChildTrsList(this Transform trsRoot)
        {
            List<Transform> parts = new List<Transform>();
            for (int i = 0; i < trsRoot.childCount; i++)
                parts.Add(trsRoot.GetChild(i));
            return parts;
        }

        static public void RemoveAllChild(this Transform trsRoot)
        {
            var childs = GetChildTrsList(trsRoot);
            for (int i = childs.Count - 1; i >= 0; i--)
                GameObject.Destroy(childs[i].gameObject);
        }
    }
}




