using UltEvents;
using UnityEngine;

namespace WideWade
{
    /// <summary>
    /// Fires an event every second until it's over
    /// </summary>
    public class Countdown : MonoBehaviour
    {
        [Header("Settings")]
        public int startTime;
        public bool destroyOnFinish = true;

        [Header("Events")]
        public UltEvent<int> onSecondPassed;

        private float _timer = 1;
        private int _secondsLeft;

        private void Update()
        {
            if (_secondsLeft > 0)
            {
                _timer -= Time.deltaTime;

                if (_timer >= 1f)
                {
                    _timer += 1f;
                    _secondsLeft--;
                    onSecondPassed.Invoke(_secondsLeft);
                }
            }
            else
            {
                if (destroyOnFinish)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
