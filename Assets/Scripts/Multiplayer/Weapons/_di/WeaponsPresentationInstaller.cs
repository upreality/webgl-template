using Multiplayer.Weapons.presentation.ui;
using UnityEngine;
using Zenject;

namespace Multiplayer.Weapons._di
{
    public class WeaponsPresentationInstaller : MonoInstaller
    {
        [SerializeField] private WeaponListItem listItemPrefab;

        public override void InstallBindings()
        {
            //UI
            Container.BindFactory<WeaponListItem, WeaponListItem.Factory>().FromComponentInNewPrefab(listItemPrefab);
        }
    }
}