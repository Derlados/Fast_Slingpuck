using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulties : MonoBehaviour
{
    public class Begginer
    {
        public const string mode = "Begginer";
        public const float speed = 15;
        public const float accuracyAI = 0.2f;
        public const float timeRest = 1.2f;
    }
    public class Skilled
    {
        public const string mode = "Skilled";
        public const float speed = 30;
        public const float accuracyAI = 0.1f;
        public const float timeRest = 1;
    }
    public class Master
    {
        public const string mode = "Master";
        public const float speed = 75;
        public const float accuracyAI = 0.05f;
        public const float timeRest = 0.6f;
    }
    public class God
    {
        public const string mode = "God";
        public const float speed = 115;
        public const float accuracyAI = 0.02f;
        public const float timeRest = 0.4f;
    }

    public class Chinese
    {
        public const string mode = "Chinese";
        public const float speed = 200;
        public const float accuracyAI = 0.001f;
        public const float timeRest = 0.22f;
    }
}
