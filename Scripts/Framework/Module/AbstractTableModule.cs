using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AbstractTableModule : AbstractModule
    {
        private bool m_IsTableLoadFinish = false;
        protected override void OnComAwake()
        {
            InitPreloadTableData();
            m_IsTableLoadFinish = true;
            actor.StartCoroutine(TableMgr.S.ReadAll());
        }


        public virtual void InitPreloadTableData()
        {

        }

    }
}




