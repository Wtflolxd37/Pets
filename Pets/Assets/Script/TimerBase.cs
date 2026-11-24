using System;
using UnityEngine;

namespace Dino.Utils
{
    public class TimerBase : MonoBehaviour
    {
        public float CurrentTimer => _currentTimer;
        public bool IsTimerActive => _isTimerActive;

        public Action OnTimerStart;
        public Action OnTimerEnd;
        public Action<float> OnTimerUpdate;
        public Action OnTimerLoop;

        private bool _isTimerActive;
        private bool _isLoopable;
        private bool _isCountdown;

        private float _currentTimer;
        private float _loopTimer;
        private float _updateInterval = 1f;
        private float _elapsedTime;
        private float _timerValue;

        private void Update()
        {
            if (!_isTimerActive)
                return;

            if (_isCountdown)
                _currentTimer -= Time.deltaTime;
            else
                _currentTimer += Time.deltaTime;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _updateInterval)
            {
                OnTimerUpdate?.Invoke(_currentTimer);
                _elapsedTime = 0f;
            }

            bool finished = _isCountdown
                ? _currentTimer <= 0f
                : _currentTimer >= _timerValue;

            if (!finished)
                return;

            if (_isLoopable)
            {
                if (_isCountdown)
                    _currentTimer = _loopTimer;
                else
                    _currentTimer = 0f;

                OnTimerLoop?.Invoke();
            }
            else
            {
                FinishedTimer();
            }
        }

        public float GetTimePercentage()
        {
            if (_timerValue <= 0f)
                return 0f;

            return _currentTimer / _timerValue;
        }

        public void StartTimer(
            float timer,
            string message = "",
            bool isCountDown = false,
            float intervalUpdate = 1f)
        {
            _timerValue = timer;
            _updateInterval = Mathf.Max(0.01f, intervalUpdate);
            _isCountdown = isCountDown;

            _currentTimer = isCountDown ? timer : 0f;
            _elapsedTime = 0f;

            _isLoopable = false;
            _isTimerActive = true;

            OnTimerStart?.Invoke();

            if (!string.IsNullOrEmpty(message))
                Debug.Log("Timer Started: " + message);
        }

        public void StartLoopTimer(
            float timer,
            string message = "",
            bool isCountDown = false,
            float intervalUpdate = 1f)
        {
            _loopTimer = Mathf.Max(0.01f, timer);
            _timerValue = _loopTimer;

            _updateInterval = Mathf.Max(0.01f, intervalUpdate);
            _isCountdown = isCountDown;

            _currentTimer = isCountDown ? _loopTimer : 0f;
            _elapsedTime = 0f;

            _isLoopable = true;
            _isTimerActive = true;

            OnTimerStart?.Invoke();

            if (!string.IsNullOrEmpty(message))
                Debug.Log("Loop Timer Started: " + message);
        }

        public void ChangeLoopTimer(float timer)
        {
            _loopTimer = Mathf.Max(0.1f, timer);
            _timerValue = _loopTimer;
            _currentTimer = _isCountdown ? _loopTimer : 0f;
            _elapsedTime = 0f;
        }

        public void PauseTimer()
        {
            _isTimerActive = false;
        }

        public void ResetTimer()
        {
            _currentTimer = 0f;
            _elapsedTime = 0f;
        }

        public void StopTimer(bool destroy = false)
        {
            _isTimerActive = false;
            ResetTimer();

            if (destroy)
                Destroy(this);
        }

        private void FinishedTimer()
        {
            _isTimerActive = false;
            ResetTimer();

            OnTimerEnd?.Invoke();
        }
    }
}