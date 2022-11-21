using Core.Auth.data;
using Core.Auth.domain;
using Core.Auth.presentation;
using UnityEngine;
using Zenject;

namespace Core.Auth._di
{
    [CreateAssetMenu(menuName = "Installers/AuthInstaller")]
    public class AuthInstaller: ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAuthRepository>()
#if PLAYFAB
            .To<PlayfabAuthRepository>()
#endif
            .To<AlwaysLoggedInAuthRepository>()
            .AsSingle();
            
            Container.BindInterfacesAndSelfTo<AutoLoginService>().AsSingle();
        }
    }
}