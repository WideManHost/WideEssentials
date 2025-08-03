using System;
using UnityEngine;


namespace WideWade
{
    /// <summary>
    /// A SO that contains lighting information
    /// </summary>
    [CreateAssetMenu(fileName = "Lighting Conditions", menuName = "Wide Essentials/LightingConditions")]
    [Serializable]
    public class LightingConditions : ScriptableObject
    {
        [Header("Fog Settings")]
        public bool fogEnabled;
        public Color fogColor = Color.gray;
        public FogMode fogMode = FogMode.Linear;
        public float fogStartDistance = 0f;
        public float fogEndDistance = 300f;
        public float fogDensity = 0.01f;

        [Header("Directional Light Settings")]
        public Color lightColor = Color.white;
        public float lightIntensity = 1.0f;
        public Vector3 lightRotation = new Vector3(50f, -30f, 0f);

        [Header("Skybox Settings")]
        public Material skyboxMaterial;
    }

}
