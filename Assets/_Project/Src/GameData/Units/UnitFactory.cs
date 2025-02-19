using Reflex.Core;
using Reflex.Injectors;
using Services.Gameplay.Units;
using UnityEngine;

namespace GameData.Units
{
    public class UnitFactory
    {
        private readonly Container _container;
        private readonly GameObject _unitPrefab;

        public UnitFactory(Container container, GameObject unitPrefab)
        {
            _container = container;
            _unitPrefab = unitPrefab;
        }

        public Unit Create(Vector3 position)
        {
            var unit = Object.Instantiate(_unitPrefab, position, Quaternion.identity);
            var unitComponent = unit.GetComponent<Unit>();

            GameObjectInjector.InjectObject(unit, _container);

            return unitComponent;
        }
    }


}