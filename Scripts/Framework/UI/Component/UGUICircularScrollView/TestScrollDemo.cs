using System.Collections;
using System.Collections.Generic;
using GFrame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestScrollDemo : MonoBehaviour
{

    public UICircularScrollView VerticalScroll_1;
    public UICircularScrollView VerticalScroll_2;
    public UICircularScrollView HorizontalScroll_1;
    public UICircularScrollView HorizontalScroll_2;

    public Button btn;

    // Use this for initialization
    void Start()
    {
        btn.onClick.AddListener(OnClickSeeOtherScroll);
        StartScrollView();
    }

    void OnClickSeeOtherScroll()
    {
        SceneManager.LoadScene("OtherDemo");
    }


    public void StartScrollView()
    {
        VerticalScroll_1.Init(NormalCallBack);
        VerticalScroll_1.SetCount(50);

        VerticalScroll_2.Init(NormalCallBack);
        VerticalScroll_2.SetCount(50);

        HorizontalScroll_1.Init(NormalCallBack);
        HorizontalScroll_1.SetCount(50);

        HorizontalScroll_2.Init(NormalCallBack);
        HorizontalScroll_2.SetCount(50);
    }

    private void NormalCallBack(GameObject cell, int index)
    {
        cell.transform.Find("Text1").GetComponent<Text>().text = index.ToString();
        cell.transform.Find("Text2").GetComponent<Text>().text = index.ToString();
    }


}
