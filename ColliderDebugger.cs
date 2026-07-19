using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

namespace JollyDecorations
{
    public class ColliderDebugger : MonoBehaviour
    {
        private Material _lineMaterial;
        private bool _enabled = false;

        private void Start()
        {
            _lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        private void Update()
        {
            if (Keyboard.current.f8Key.wasPressedThisFrame)
            {
                _enabled = !_enabled;
            }
        }

        private void OnRenderObject()
        {
            if (!_enabled || _lineMaterial == null)
                return;

            _lineMaterial.SetPass(0);

            GL.Begin(GL.LINES);

            foreach (BoxCollider box in FindObjectsOfType<BoxCollider>())
            {
                if (box.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    DrawBox(box);
                }
            }

            GL.End();
        }

        private void DrawBox(BoxCollider box)
        {
            Vector3 center = box.center;
            Vector3 half = box.size * 0.5f;

            Vector3[] localCorners =
            {
                center + new Vector3(-half.x, -half.y, -half.z),
                center + new Vector3( half.x, -half.y, -half.z),
                center + new Vector3( half.x, -half.y,  half.z),
                center + new Vector3(-half.x, -half.y,  half.z),

                center + new Vector3(-half.x,  half.y, -half.z),
                center + new Vector3( half.x,  half.y, -half.z),
                center + new Vector3( half.x,  half.y,  half.z),
                center + new Vector3(-half.x,  half.y,  half.z)
            };

            Matrix4x4 matrix = box.transform.localToWorldMatrix;

            Vector3[] worldCorners = new Vector3[8];

            for (int i = 0; i < localCorners.Length; i++)
            {
                worldCorners[i] = matrix.MultiplyPoint3x4(localCorners[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                Line(worldCorners[i], worldCorners[(i + 1) % 4]);
                Line(worldCorners[i + 4], worldCorners[((i + 1) % 4) + 4]);
                Line(worldCorners[i], worldCorners[i + 4]);
            }
        }

        private void Line(Vector3 a, Vector3 b)
        {
            GL.Vertex(a);
            GL.Vertex(b);
        }
    }
}