using UnityEngine;
using System.Collections;

namespace WideWade
{
    /// <summary>
    /// Handles loading lighting conditions. More of an interfacer than anything.
    /// </summary>
    public class URPLightingManager : StaticManager<URPLightingManager>
    {
        [SerializeField]
        private Light _directionalLight;
        [SerializeField]
        private bool _applyConditionsOnStart = false;
        [SerializeField]
        private LightingConditions _currentConditions;

        private Coroutine _transitionCoroutine;

        private void Start()
        {
            if (_currentConditions != null && _applyConditionsOnStart)
            {
                RenderSettings.sun = _directionalLight;
                ApplyLightingSettings(_currentConditions, 5);
            }
        }


        #region Public Methods

        /// <summary>
        /// Applies the inputted lighting to the scene
        /// </summary>
        /// <param name="conditions"></param>
        public void ApplyLightingSettings(LightingConditions conditions)
        {
            if (conditions == null)
            {
                Debug.LogWarning("Lighting settings is null.");
            }
            else
            {
                // Apply Fog Settings
                RenderSettings.fog = conditions.fogEnabled;
                RenderSettings.fogColor = conditions.fogColor;
                RenderSettings.fogMode = conditions.fogMode;
                RenderSettings.fogStartDistance = conditions.fogStartDistance;
                RenderSettings.fogEndDistance = conditions.fogEndDistance;
                RenderSettings.fogDensity = conditions.fogDensity;

                // Apply Directional Light Settings
                if (_directionalLight != null)
                {
                    _directionalLight.color = conditions.lightColor;
                    _directionalLight.intensity = conditions.lightIntensity;
                    _directionalLight.transform.rotation = Quaternion.Euler(conditions.lightRotation);
                }

                // Apply Skybox
                if (conditions.skyboxMaterial != null)
                {
                    RenderSettings.skybox = conditions.skyboxMaterial;
                    // Yeah so aparently you need to do this
                    DynamicGI.UpdateEnvironment();
                }
            }
        }

        /// <summary>
        /// Applies the inputted Lighting to the scene but lerps the changes.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        public void ApplyLightingSettings(LightingConditions target, float duration)
        {
            if (_transitionCoroutine != null)
            {
                // stop the current coroutine so that we can start a new one
                StopCoroutine(_transitionCoroutine);
            }    

            _transitionCoroutine = StartCoroutine(LerpRoutine(target, duration));
        }

        private IEnumerator LerpRoutine(LightingConditions target, float duration)
        {
            if (target != null)
            {
                // Initial values
                Color initialFogColor = RenderSettings.fogColor;
                float initialFogStart = RenderSettings.fogStartDistance;
                float initialFogEnd = RenderSettings.fogEndDistance;
                float initialFogDensity = RenderSettings.fogDensity;

                Color initialLightColor = _directionalLight != null ? _directionalLight.color : Color.white;
                float initialLightIntensity = _directionalLight != null ? _directionalLight.intensity : 1f;
                Quaternion initialLightRotation = _directionalLight != null ? _directionalLight.transform.rotation : Quaternion.identity;
                Quaternion targetRotation = Quaternion.Euler(target.lightRotation);

                float currentTime = 0f;

                // Enable fog and set fog mode immediately, otherwise the fade in looks like dogshit.
                RenderSettings.fog = target.fogEnabled;
                RenderSettings.fogMode = target.fogMode;

                while (currentTime < duration)
                {
                    float t = currentTime / duration;

                    RenderSettings.fogColor = Color.Lerp(initialFogColor, target.fogColor, t);
                    RenderSettings.fogStartDistance = Mathf.Lerp(initialFogStart, target.fogStartDistance, t);
                    RenderSettings.fogEndDistance = Mathf.Lerp(initialFogEnd, target.fogEndDistance, t);
                    RenderSettings.fogDensity = Mathf.Lerp(initialFogDensity, target.fogDensity, t);

                    if (_directionalLight != null)
                    {
                        _directionalLight.color = Color.Lerp(initialLightColor, target.lightColor, t);
                        _directionalLight.intensity = Mathf.Lerp(initialLightIntensity, target.lightIntensity, t);
                        _directionalLight.transform.rotation = Quaternion.Slerp(initialLightRotation, targetRotation, t);
                    }

                    currentTime += Time.deltaTime;

                    yield return new WaitForSeconds(Time.deltaTime);
                }

                // Apply final settings
                ApplyLightingSettings(target);

                yield return null;
            }

           
        }

        #endregion

        #region Static Methods

        public static void ChangeLighting(LightingConditions conditions)
        {
            Instance.ApplyLightingSettings(conditions);
        }

        public static void ChangeLighting(LightingConditions conditions, float lerpDuration)
        {
            Instance.ApplyLightingSettings(conditions, lerpDuration);
        }

        #endregion
    }
}
