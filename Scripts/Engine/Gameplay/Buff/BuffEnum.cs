using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GFrame
{

    // public class BuffEnum
    // {

    // }

    //Buff触发条件
    //当XXX时
    public enum BuffTriggerType
    {

        OnHurt,//当角色收到攻击时
    }

    //Buff触发前置条件
    //当玩家收到攻击时，并且玩家血量在多少以上
    public enum BuffTriggerPreConditionType
    {
        HpPercent,
    }
}




