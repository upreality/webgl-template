using Core.User.domain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.User.presentation
{
    public class UserNameText : MonoBehaviour
    {
        [SerializeField] private Text text;
        [Inject] private ICurrentUserNameRepository currentUserNameRepository;

        private void Start() => currentUserNameRepository
            .GetUserNameFlow()
            .Subscribe(userName => text.text = userName)
            .AddTo(this);
    }
}