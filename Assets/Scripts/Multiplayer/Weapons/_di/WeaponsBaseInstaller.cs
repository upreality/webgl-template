using Multiplayer.Weapons.data;
using Multiplayer.Weapons.data.dao;
using Multiplayer.Weapons.domain;
using Multiplayer.Weapons.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.Weapons._di
{
    [CreateAssetMenu(fileName = "WeaponsBaseInstaller", menuName = "Installers/WeaponsBaseInstaller")]
    public class WeaponsBaseInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private WeaponEntitiesSODao weaponEntitiesSoDao;

        public override void InstallBindings()
        {
            //Data
            Container.Bind<IWeaponEntitiesDao>().FromInstance(weaponEntitiesSoDao).AsSingle();
            Container.Bind<IWeaponsRepository>().To<WeaponsRepository>().AsSingle();
            Container.Bind<ISelectedWeaponRepository>().To<SelectedWeaponDefaultRepository>().AsSingle();
            Container.Bind<IWeaponPreviewRepository>().To<WeaponPreviewRepository>().AsSingle();
            Container.Bind<IWeaponPrefabRepository>().To<WeaponPrefabRepository>().AsSingle();
            //Domain
            Container.Bind<ScrollWeaponsUseCase>().ToSelf().AsSingle();
            Container.Bind<SelectWeaponUseCase>().ToSelf().AsSingle();
            Container.Bind<DefaultWeaponUseCase>().ToSelf().AsSingle();
        }
    }
}