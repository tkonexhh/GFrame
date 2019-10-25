using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GFrame
{

    public class RedPointUI : MonoBehaviour
    {
        [SerializeField] Image m_Image;
        [SerializeField] Text m_TxtNum;
        [SerializeField] bool m_NeedShowNum = true;

        private void Start()
        {
            if (m_Image == null)
            {
                m_Image = GetComponent<Image>();
            }


            if (m_NeedShowNum)
            {
                if (m_TxtNum == null)
                {
                    m_TxtNum = GetComponentInChildren<Text>();
                }
            }
            else
            {
                if (m_TxtNum != null)
                {
                    m_TxtNum.gameObject.SetActive(false);
                }
            }


        }

        public void UpdateUI(int num)
        {
            m_Image.gameObject.SetActive(num > 0);

            if (m_NeedShowNum)
            {
                if (num > 0)
                    if (m_TxtNum != null)
                    {
                        m_TxtNum.text = num.ToString();
                    }
            }
            else
            {
                if (m_TxtNum != null)
                    m_TxtNum.gameObject.SetActive(false);
            }

        }


    }
}




