using Core.User.domain;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.User.presentation
{
    public class UserNameText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [Inject] private ICurrentUserNameRepository currentUserNameRepository;

        private void Start() => currentUserNameRepository
            .GetUserNameFlow()
            .Subscribe(userName => text.text = userName)
            .AddTo(this);
    }
}