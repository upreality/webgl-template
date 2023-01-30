using System;

namespace Utils.ZenjectCodegen
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneSubscriptionHandler : Attribute{}
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SceneSubscription : Attribute{}
}