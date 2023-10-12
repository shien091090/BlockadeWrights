using UnityEngine;

namespace GameCore
{
    public class TimeManager : ITimeManager
    {
        public float DeltaTime => Time.deltaTime;
    }
}