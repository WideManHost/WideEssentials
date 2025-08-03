using Unity.Mathematics;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Clamps the rotation of a transform within the parameters set.
    /// Credit my homie frootluips for making this for us in Semester 3. Saved our asses.
    /// </summary>
    [DisallowMultipleComponent]
    public class RotationClamp : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] bool3 axes = true;
        public Vector3 minimum;
        public Vector3 maximum;

        private Vector3 _angles, _newAngles;
        const float FULL_ROTATION = 360f;

        // Update is called once per frame
        void LateUpdate()
        {
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            _angles = transform.localRotation.eulerAngles;
            _newAngles = _angles;

            if (axes.x)
            {
                _newAngles.x = ClampAngle(_angles.x, minimum.x, maximum.x);
            }

            if (axes.y)
            {
                _newAngles.y = ClampAngle(_angles.y, minimum.y, maximum.y);
            }

            if (axes.z)
            {
                _newAngles.z = ClampAngle(_angles.z, minimum.z, maximum.z);
            }

            transform.localRotation = Quaternion.Euler(_newAngles);
        }

        static float ConvertAngle(float angle) => angle > 180 ? angle - FULL_ROTATION : angle;

        public static float ClampAngle(float angle, float min, float max)
        {
            angle = ConvertAngle(angle);
            min = ConvertAngle(min);
            max = ConvertAngle(max);

            return Mathf.Clamp(angle, min, max);
        }
    }
}
