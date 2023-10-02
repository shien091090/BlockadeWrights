using UnityEngine;

namespace GameCore
{
    public class EditorPathHint
    {
        private readonly LineRenderer lineRenderer;
        private Transform nextPathHint;
        public Transform Transform { get; }

        public EditorPathHint(Transform transform, LineRenderer lineRenderer)
        {
            Transform = transform;
            this.lineRenderer = lineRenderer;
        }

        public void SetNextPathHint(Transform nextPathHint)
        {
            this.nextPathHint = nextPathHint;
        }

        public void RefreshDrawPathHint()
        {
            if (nextPathHint == null)
                return;

            lineRenderer.SetPosition(0, Transform.position);
            lineRenderer.SetPosition(1, nextPathHint.position);
        }
    }
}