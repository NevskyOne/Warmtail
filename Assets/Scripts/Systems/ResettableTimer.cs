using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Systems
{
    public class ResettableTimer
    {
        private float _time;
        private bool _running;
        private CancellationTokenSource _cts;
        private readonly Action _onComplete;

        public ResettableTimer(float time, Action onComplete)
        {
            _time = time;
            _onComplete = onComplete;
        }

        public void Start()
        {
            Stop();
            _cts = new CancellationTokenSource();
            _running = true;
            Run(_cts.Token).Forget();
        }

        public void Reset(float newTime)
        {
            _time = newTime;
            Start();
        }

        public void Stop()
        {
            if (_running)
            {
                _running = false;
                _cts?.Cancel();
                _cts?.Dispose();
            }
        }

        private async UniTaskVoid Run(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_time), cancellationToken: token);
                if (!_running) return;
                _onComplete?.Invoke();
            }
            catch
            {
                // ignored
            }
        }
    }
}