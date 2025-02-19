using System;
using UniRx;
using UnityEngine;
using Unit = Services.Gameplay.Units.Unit;

namespace Storage
{
    public class GameplayState : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private ReactiveDictionary<int, Unit> selectables;

        private int Increment = 0;

        public GameplayState()
        {
            selectables = new ReactiveDictionary<int, Unit>().AddTo(_disposables);
        }

        public void AddUnit(Unit unit)
        {
            var id = Increment++;
            unit.SetId(id);
            selectables[id] = unit;

            Debug.Log($"Added unit {id}");
        }

        public void RemoveUnit(Unit unit)
        {
            selectables.Remove(unit.Id);
        }


        public void Dispose()
        {
            selectables.Clear();

            _disposables?.Dispose();
        }
    }
}