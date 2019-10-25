using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AbstractSqliteDataClass
    {
        public KeyValuePair<string, string> GetAttrStr()
        {
            var type = this.GetType();
            var fields = ReflectionHelper.GetFields(type);
            string field = "";
            string value = "";
            //Debug.LogError(fields.Count);
            for (int i = 0; i < fields.Count; i++)
            {
                field += fields[i].Name + (i == fields.Count - 1 ? "" : ",");
                value += fields[i].GetValue(this) + (i == fields.Count - 1 ? "" : ",");
            }
            //Debug.LogError(field + "--" + value);
            return new KeyValuePair<string, string>(field, value);
        }
    }
}




