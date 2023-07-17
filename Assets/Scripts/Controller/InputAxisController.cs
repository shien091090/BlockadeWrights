using UnityEngine;

namespace GameCore
{
    public class InputAxisController : IInputAxisController
    {
        public float GetHorizontalAxis()
        {
            return Input.GetAxis("Horizontal");
        }

        public float GetVerticalAxis()
        {
            return Input.GetAxis("Vertical");
        }
    }
}