using System;
using Reflex.Attributes;
using UniRx;

namespace Views
{
    public interface IGameplayModelView
    {
    }

    public class GameplayModelView : IGameplayModelView, IModelView, IDisposable
    {
        public IReadOnlyReactiveProperty<int> TestProp => _testProp;
        private readonly ReactiveProperty<int> _testProp;

        readonly CompositeDisposable _disposable = new CompositeDisposable();

        // add model
        [ReflexConstructor]
        public GameplayModelView()
        {
            _testProp = new ReactiveProperty<int>();

            _testProp.AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}