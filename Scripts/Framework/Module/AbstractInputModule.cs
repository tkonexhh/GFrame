using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AbstractInputModule : AbstractModule
    {
        protected IKeyboardInputter m_KeyBoardInputer;
        protected override void OnComAwake()
        {
            m_KeyBoardInputer = new KeyboardInputter();
            RegisterKeyboard();
        }

        protected virtual void RegisterKeyboard()
        {

        }

        protected override void OnComLateUpdate(float dt)
        {
            m_KeyBoardInputer.OnLateUpdate();
        }

    }
}




