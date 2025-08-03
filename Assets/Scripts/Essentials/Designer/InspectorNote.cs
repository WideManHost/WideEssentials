using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Just a note for designers, destroys on start if it's a build.
    /// 
    /// !!! PLEASE DO NOT REFERENCE THIS CLASS AS I'M SURE IT WILL RAISE ERRORS (AND HELL) !!!
    /// </summary>
    [AddComponentMenu("Miscellaneous/Inspector Note")]
    public class InspectorNote : MonoBehaviour
    {
        [TextArea(10, 1000)]
        public string Comment = "Enter Information here";

        private void Start()
        {
            if (!Application.isEditor)
            {
                Destroy(gameObject);
            }
        }
    }
}
