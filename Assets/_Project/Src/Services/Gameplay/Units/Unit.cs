using UniRx;
using UnityEngine;

namespace Services.Gameplay.Units
{
    public class Unit : MonoBehaviour, ISelectable
    {
        public int Id => id;
        [SerializeField] private int id;

        [SerializeField] private GameObject _selectionCircle;
        [SerializeField] private ReactiveProperty<bool> _isSelected;

        private readonly CompositeDisposable _disposables = new();

        public void Init()
        {
            _isSelected = new ReactiveProperty<bool>(false).AddTo(_disposables);

            _isSelected
                .Subscribe(isSelected => _selectionCircle.SetActive(isSelected))
                .AddTo(_disposables);
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public void SetSelected()
        {
            _isSelected.Value = true;
        }

        public void SetUnselected()
        {
            _isSelected.Value = false;
        }

        public void SetSelectionCircle(GameObject selectionCircle)
        {
            _selectionCircle = selectionCircle;
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}