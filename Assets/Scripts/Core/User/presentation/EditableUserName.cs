using System;
using System.Collections.Generic;
using Core.User.domain;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;
using static Core.User.domain.ICurrentUserNameRepository;
using static Core.User.domain.ValidateUserNameUseCase;

namespace Core.User.presentation
{
    public class EditableUserName : MonoBehaviour
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private InputField editNameField;
        [SerializeField] private Text errorText;
        [SerializeField] private GameObject errorMark;
        [SerializeField] private UnityEvent onSuccessfulChangeUserName;

        [Inject] private ICurrentUserNameRepository currentUserNameRepository;
        [Inject] private ValidateUserNameUseCase validateUserNameUseCase;

        [CanBeNull] private IDisposable setupDisposable;

        private string unableToSetUsernameErrorText = "Error while updating name :(";
        private string unableToSetUsernameNotAvailableText = "Username not available, try another one";

        private Dictionary<UserNameValidState, string> errorTexts = new()
        {
            [UserNameValidState.Valid] = "",
            [UserNameValidState.TooShort] = "Name is too short!",
            [UserNameValidState.TooLong] = "Name is too long!",
            [UserNameValidState.IsEmpty] = "Name is empty!",
            [UserNameValidState.ContainsInvalidCharacters] = "Name contains invalid characters!"
        };

        private void OnEnable()
        {
            confirmButton.interactable = false;
            errorMark.SetActive(false);
            setupDisposable = currentUserNameRepository.GetUserNameFlow().First().Subscribe(Setup);
        }

        private void OnDisable() => setupDisposable?.Dispose();

        private void Setup(string username)
        {
            editNameField.text = username;
            editNameField.onValueChanged.RemoveAllListeners();
            editNameField.onValueChanged.AddListener(OnNameChanged);
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(SubmitUserName);
        }

        private void SubmitUserName()
        {
            var userName = editNameField.text;
            if (validateUserNameUseCase.Validate(userName) != UserNameValidState.Valid)
                return;

            confirmButton.interactable = false;
            currentUserNameRepository.UpdateUserName(userName).Subscribe(OnUsernameUpdated).AddTo(this);
        }

        private void OnUsernameUpdated(UpdateUserNameResult result)
        {
            if (result != UpdateUserNameResult.Success)
            {
                errorText.text = result == UpdateUserNameResult.NotAvailable
                    ? unableToSetUsernameNotAvailableText
                    : unableToSetUsernameErrorText;
                errorMark.SetActive(true);
                errorText.enabled = true;
                return;
            }

            onSuccessfulChangeUserName?.Invoke();
        }

        private void OnNameChanged(string newValue)
        {
            var state = validateUserNameUseCase.Validate(newValue);
            errorText.text = errorTexts[state];
            var isValid = state == UserNameValidState.Valid;
            errorText.enabled = !isValid;
            errorMark.SetActive(!isValid);
            confirmButton.interactable = isValid;
        }
    }
}