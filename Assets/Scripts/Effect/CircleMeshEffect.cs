using System;
using UnityEngine;

namespace GameCore
{
    public class CircleMeshEffect : MonoBehaviour
    {
        private const int SEGMENTS = 40;

        [SerializeField] private Color circleColor;

        private float radius;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private Mesh mesh;
        private bool isDrawn;

        public void ShowEffect(float newRadius)
        {
            if (Math.Abs(newRadius - radius) > 0.1f)
                isDrawn = false;

            radius = newRadius;

            if (!isDrawn)
                DrawFilledCircle();

            meshRenderer.enabled = true;
        }

        public void HideEffect()
        {
            meshRenderer.enabled = false;
        }

        private void DrawFilledCircle()
        {
            Vector3[] vertices = new Vector3[SEGMENTS + 1];
            int[] triangles = new int[SEGMENTS * 3];

            vertices[0] = Vector3.zero;
            for (int i = 1; i < SEGMENTS + 1; i++)
            {
                float angle = 2 * Mathf.PI * (i - 1) / SEGMENTS;
                float x = radius * Mathf.Cos(angle);
                float y = radius * Mathf.Sin(angle);
                vertices[i] = new Vector3(x, y, 0);
            }

            for (int i = 0; i < SEGMENTS; i++)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = (i + 2 <= SEGMENTS) ?
                    i + 2 :
                    1;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            meshRenderer.material.color = circleColor;
            isDrawn = true;
        }

        private void Awake()
        {
            mesh = new Mesh();
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            isDrawn = false;
        }
    }
}