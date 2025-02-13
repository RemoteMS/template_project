using UniRx;

namespace Storage
{
    public interface IGameState
    {
        public IReadOnlyReactiveCollection<IData> Contex { get; }
        void Save(IData data);
    }

    public interface IData
    {
    }
}