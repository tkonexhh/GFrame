// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace GFrame
// {

//     public class AbstractApplicationMgr<T> : TMonoSingleton<T> where T : TMonoSingleton<T>
//     {

//         public Action onApplicationUpdate = null;
//         public Action onApplicationOnGUI = null;
//         private void Start()
//         {
//             StartGame();
//         }

//         private void StartGame()
//         {
//             InitGameEnvironment();
//         }

//         protected virtual void InitGameEnvironment()
//         {

//         }
//     }
// }
