using System.Collections;
using Multiplayer.Weapons.domain.model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Multiplayer.Weapons.presentation.ui
{
    public class WeaponListItem : MonoBehaviour
    {
        [Header("Timer")] [SerializeField] private float setStateTimer = 0.5f;
        [SerializeField] private AnimationCurve setStateEvaluateCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("selectedIndicator")] [SerializeField]
        private GameObject selectedIndicator;

        [Header("Icon")] [SerializeField] private Image icon;
        [SerializeField] private Color activeIconColor = Color.white;
        [SerializeField] private Color disabledIconColor = Color.gray;

        [Header("Text")] [SerializeField] private Text weaponNameText;
        [SerializeField] private Color activeNameTextColor = Color.white;
        [SerializeField] private Color disabledNameTextColor = Color.gray;

        private bool selectedState = false;

        public void Setup(Weapon weapon, Sprite preview, bool selected = false)
        {
            StopAllCoroutines();
            weaponNameText.text = weapon.Name;
            icon.sprite = preview;
            icon.color = selected ? activeIconColor : disabledIconColor;
            weaponNameText.color = selected ? activeNameTextColor : disabledNameTextColor;
            selectedIndicator.SetActive(selected);
            selectedState = selected;
        }

        public void SetSelectedState(bool selected)
        {
            if (selected == selectedState) return;
            StopAllCoroutines();
            StartCoroutine(SetSelectedStateEnumerator(selected));
        }

        private IEnumerator SetSelectedStateEnumerator(bool selected)
        {
            var timer = setStateTimer;
            var initialIconColor = icon.color;
            var initialTextColor = weaponNameText.color;
            var targetIconColor = selected ? activeIconColor : disabledIconColor;
            var targetTextColor = selected ? activeNameTextColor : disabledNameTextColor;
            while (timer > 0)
            {
                var progress = setStateEvaluateCurve.Evaluate(1f - timer / setStateTimer);
                icon.color = Color.Lerp(initialIconColor, targetIconColor, progress);
                weaponNameText.color = Color.Lerp(initialTextColor, targetTextColor, progress);
                timer -= Time.deltaTime;
                yield return null;
            }

            icon.color = targetIconColor;
            weaponNameText.color = targetTextColor;

            selectedIndicator.SetActive(selected);
            selectedState = selected;
        }

        public class Factory : PlaceholderFactory<WeaponListItem>
        {
        }
    }
}