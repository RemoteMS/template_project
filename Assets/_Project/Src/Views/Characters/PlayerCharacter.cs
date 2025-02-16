using Reflex.Attributes;
using Services.ModelViews;
using UniRx;
using UnityEngine;

namespace Views.Characters
{
    public class PlayerCharacter : MonoBehaviour
    {
        private IPlayerController _playerController;

        private readonly CompositeDisposable _disposable = new();

        [Inject]
        public void Inject(IPlayerController playerController)
        {
            Debug.LogWarning($"playerController Inject - {playerController}");
            _playerController = playerController;
            _playerController.CharacterMovement.Subscribe(Move).AddTo(_disposable);
        }

        private void Move(Vector2 move)
        {
            Debug.LogWarning($"Move from {transform.position} to {move}");
            transform.position += new Vector3(move.x, 0f, move.y) * Time.deltaTime;
        }


        private void OnDestroy()
        {
            _disposable?.Dispose();
        }

    }
}