using Multiplayer.PlayerInput.data.Repositories;
using Multiplayer.PlayerInput.domain;
using Multiplayer.PlayerInput.domain.repositories;
using Multiplayer.PlayerInput.presentation.InputRestriction;
using UnityEngine;
using Zenject;

namespace Multiplayer.PlayerInput._di
{
    [CreateAssetMenu(fileName = "Player Input Installer", menuName = "Installers/Player Input Installer", order = 0)]
    public class PlayerInputInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container.Bind<IPlayerInputRepository>().To<PlayerInputDefaultRepository>().AsSingle();
            Container.Bind<IInputStateRepository>().To<InputStateDefaultRepository>().AsSingle();
            Container.Bind<IInputMaskRepository>().To<InputMaskDefaultRepository>().AsSingle();
            
            //Domain
            Container.Bind<PlayerInputSuspendedUseCase>().ToSelf().AsSingle();
            Container.Bind<PlayerInputUseCase>().ToSelf().AsSingle();
            
            //Presentation
            Container.Bind<PlayerInputStateNavigator>().ToSelf().AsSingle();
        }
    }
}