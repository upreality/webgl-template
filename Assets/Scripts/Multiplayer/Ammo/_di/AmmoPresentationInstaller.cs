using Multiplayer.Ammo.presentation;
using Multiplayer.Ammo.presentation.navigator;
using Multiplayer.Weapons.presentation;
using Zenject;

namespace Multiplayer.Ammo._di
{
    public class AmmoPresentationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ReloadNavigator>().ToSelf().AsSingle();

            Container
                .Bind<WeaponSelectionController.IWeaponSelectionAvailableProvider>()
                .To<ReloadWeaponSelectionAvailableProvider>().AsSingle();
        }
    }
}