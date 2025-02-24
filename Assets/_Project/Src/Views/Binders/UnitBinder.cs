using Reflex.Attributes;
using Services.Gameplay.Units;
using Storage;
using Unity.Behavior;
using UnityEngine;

namespace Views.Binders
{
    [SelectionBase]
    public class UnitBinder : MonoBehaviour
    {
        [SerializeField] private GameObject selectionCircle;
        [SerializeField] private Unit selfData;
        [SerializeField] private UnitSelectionColliderBinder selfSelection;
        [SerializeField] private BehaviorGraphAgent selfAgent;

        [Inject]
        public void Bind(GameplayState state)
        {
            selfData.SetSelectionCircle(selectionCircle);
            selfData.Init();
            Debug.Log("Bind");

            state.AddUnit(selfData);
        }
    }
}