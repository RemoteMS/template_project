using UnityEngine;

namespace Services.Gameplay.Units
{
    public class UnitSelectionColliderBinder : MonoBehaviour
    {
        public Unit SelfUnit => _selfUnit;
        [SerializeField] private Unit _selfUnit;
    }
}