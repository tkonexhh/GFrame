using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GFrame;

public class RedDemo : MonoBehaviour
{
    [SerializeField] RedPointUI m_User;
    [SerializeField] RedPointUI m_Task;
    [SerializeField] RedPointUI m_TaskSingle;
    [SerializeField] RedPointUI m_TaskDaily;

    RedPointNode node_DailyTask;
    RedPointNode node_SingleTask;
    void Start()
    {
        RedPointNode node_player = new RedPointNode("Player");
        node_player.BindUI(m_User);


        RedPointNode node_Task = new RedPointNode("Task");
        node_Task.BindUI(m_Task);
        node_SingleTask = new RedPointNode("Task.Single");
        node_SingleTask.BindUI(m_TaskSingle);
        node_DailyTask = new RedPointNode("Task.Daily");
        node_DailyTask.BindUI(m_TaskDaily);
        node_DailyTask.SetNum(2);
        node_Task.AddChild(node_SingleTask);
        node_Task.AddChild(node_DailyTask);
        //RedPointSystem.S.AddRedPointNode(node_Task);

        node_player.AddChild(node_Task);
        // RedPointSystem.S.GetRedPointNode("Player").AddChild(node_SingleTask);
        // RedPointSystem.S.GetRedPointNode("Player").AddChild(node_DailyTask);



        RedPointSystem.S.Refesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            node_SingleTask.SetNum(node_SingleTask.Num + 1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            node_SingleTask.SetNum(node_SingleTask.Num - 1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            node_DailyTask.SetNum(node_DailyTask.Num + 1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            node_DailyTask.SetNum(node_DailyTask.Num - 1);
        }
    }
}
