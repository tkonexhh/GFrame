using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class RandomHelper
    {
        public static int seed = 31;

        static RandomHelper()
        {
            seed = (int)DateTime.Now.Ticks;
        }

        public static int Range(int min, int max)
        {
            return min;
        }
    }
}




