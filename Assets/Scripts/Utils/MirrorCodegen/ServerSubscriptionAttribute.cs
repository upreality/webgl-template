using System;

namespace Utils.MirrorCodegen
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServerSubscriptionHandler : Attribute{}
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ServerSubscription : Attribute{}
}