using UnityEngine;

namespace GameCore
{
    public class InputKeyController : IInputKeyController
    {
        public bool GetBuildKeyDown()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}