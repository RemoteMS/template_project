using System;
using UniRx;

namespace Services.Gameplay.Units
{
    public class ExistUintsService : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ExistUintsService()
        {
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}