using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public interface IComponent
    {
        AbstractActor actor
        {
            get;
        }
        void OnAwake(AbstractActor actor);
        void OnStart();
        void OnEnable();
        void OnUpdate(float dt);
        void OnLateUpdate(float dt);
        void OnDisable();
        void OnDestroy();
    }
}




