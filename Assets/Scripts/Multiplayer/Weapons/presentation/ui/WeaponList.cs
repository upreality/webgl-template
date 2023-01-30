using System.Linq;
using JetBrains.Annotations;
using Multiplayer.Weapons.domain.model;
using Multiplayer.Weapons.domain.repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.Weapons.presentation.ui
{
    public class WeaponList : MonoBehaviour
    {
        [SerializeField] private Transform listRoot;
        [Inject] private WeaponListItem.Factory weaponItemFactory;

        [Inject] private IWeaponsRepository weaponsRepository;
        [Inject] private ISelectedWeaponRepository selectedWeaponRepository;
        [Inject] private IWeaponPreviewRepository previewRepository;

        [CanBeNull] private WeaponListItem selectedItem;

        private void Awake()
        {
            if (listRoot == null)
                listRoot = transform;

            var items = weaponsRepository
                .GetAvailableWeapons()
                .ToDictionary(weapon => weapon.ID, CreateItem);

            selectedWeaponRepository
                .GetSelectedWeaponFlow()
                .Select(selected => items[selected.ID])
                .Subscribe(SetSelected)
                .AddTo(this);
        }

        private void SetSelected(WeaponListItem nextSelectedItem)
        {
            if (selectedItem != null && selectedItem != nextSelectedItem) selectedItem.SetSelectedState(false);
            nextSelectedItem.SetSelectedState(true);
            selectedItem = nextSelectedItem;
        }

        private WeaponListItem CreateItem(Weapon weapon)
        {
            var item = weaponItemFactory.Create();
            item.transform.SetParent(listRoot);
            item.Setup(weapon, previewRepository.GetWeaponPreview(weapon.ID));
            return item;
        }
    }
}