using System;
using JetBrains.Annotations;
using Multiplayer.Weapons.domain.repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.Weapons.presentation
{
    public class WeaponSelectionInstantPresenter : MonoBehaviour
    {
        [Inject] private ISelectedWeaponRepository selectedWeaponRepository;
        [Inject] private IWeaponPrefabRepository weaponPrefabRepository;

        private IDisposable selectWeaponSubscription;

        [SerializeField] private Transform spawnRoot;

        [CanBeNull] private GameObject selectedWeapon;

        private void OnEnable()
        {
            selectWeaponSubscription = selectedWeaponRepository
                .GetSelectedWeaponFlow()
                .Select(weapon => weapon.ID)
                .Select(weaponPrefabRepository.GetWeaponPrefab)
                .Subscribe(SpawnWeapon);
        }

        private void SpawnWeapon(GameObject weapon)
        {
            if (selectedWeapon != null) Destroy(selectedWeapon);
            if (spawnRoot != null) selectedWeapon = Instantiate(weapon, spawnRoot).gameObject;
        }

        private void OnDisable() => selectWeaponSubscription.Dispose();
    }
}