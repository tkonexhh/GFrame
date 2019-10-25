using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame
{


    public class FSMState<T>
    {
        public virtual string stateName
        {
            get { return this.GetType().Name; }
        }

        public virtual void Enter(T entity)
        {
            Log.i(entity.ToString() + ":" + this.GetType().Name + "-->" + "Enter");
        }

        public virtual void Execute(T entity, float dt)
        {

        }

        public virtual void Exit(T entity)
        {
            Log.i(entity.ToString() + ":" + this.GetType().Name + "-->" + "Exit");
        }
    }
}
