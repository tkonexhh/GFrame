using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{
    public enum PanelType : byte
    {
        Bottom,
        Auto,
        Top,
    }


    [RequireComponent(typeof(Canvas))]
    public class AbstractPanel : AbstractPage
    {

    }
}




