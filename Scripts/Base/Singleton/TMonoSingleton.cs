using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{



    public abstract class TMonoSingleton<T> : MonoBehaviour, ISingleton where T : TMonoSingleton<T>, new()
    {

        protected static T instance = null;
        public static T S
        {
            get
            {
                return Instance();
            }
        }

        public virtual void OnSingletonInit()
        {

        }

        public static T Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    string instanceName = typeof(T).Name;
                    GameObject instanceGo = GameObject.Find(instanceName);
                    if (instanceGo == null)
                    {
                        instanceGo = new GameObject(instanceName);
                    }

                    instance = instanceGo.AddComponent<T>();

                    instance.OnSingletonInit();
                    DontDestroyOnLoad(instanceGo);
                }



            }

            return instance;
        }
        // // Start is called before the first frame update
        // void Start()
        // {

        // }

        // // Update is called once per frame
        // void Update()
        // {

        // }
    }
}
