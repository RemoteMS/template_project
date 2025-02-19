using Reflex.Attributes;
using Services.Gameplay.Units;
using Storage;
using UnityEngine;

namespace Views.Binders
{
    [SelectionBase]
    public class UnitBinder : MonoBehaviour
    {
        [SerializeField] private GameObject selectionCircle;
        [SerializeField] private Unit selfData;
        [SerializeField] private UnitSelectionColliderBinder selfSelection;

        [Inject]
        public void Bind(GameplayState state)
        {
            selfData.SetSelectionCircle(selectionCircle);
            Debug.Log("Bind");
            selfData.Init();
            state.AddUnit(selfData);
        }
    }
}