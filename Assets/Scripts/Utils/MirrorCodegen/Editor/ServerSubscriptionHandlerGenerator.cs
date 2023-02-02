using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Utils.MirrorCodegen.Editor
{
    internal static class ServerSubscriptionHandlerGenerator
    {
        [InitializeOnLoadMethod]
        private static void Generate()
        {
            var path = $"{Application.dataPath}/Scripts/_Generated/ServerSubscriptionHandler.cs";

            var propertiesBuilder = new StringBuilder();
            var subscriptionsBuilder = new StringBuilder();

            var subscriptionHandlers =
                from assembly in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from type in assembly.GetTypes()
                where type.IsDefined(typeof(ServerSubscriptionHandler), false)
                select type;

            var namespaces = new HashSet<string>();
            propertiesBuilder.AppendLine($"private CompositeDisposable serverSubscriptions = new();");
            foreach (var handler in subscriptionHandlers)
            {
                namespaces.Add(handler.Namespace);
                var propertyName = "m_" + handler.Name.ToLower();
                propertiesBuilder.AppendLine($"[Inject] private {handler.Name} {propertyName};");
                var subscriptions = handler
                    .GetMethods()
                    .Where(method => method.GetCustomAttributes(typeof(ServerSubscription), true).Length > 0)
                    .Where(method => method.ReturnType == typeof(IDisposable))
                    .Where(method => method.GetParameters().Length == 0);

                foreach (var subscription in subscriptions)
                {
                    var disposableName = $"{propertyName}Disposable";
                    subscriptionsBuilder.AppendLine($"var {disposableName} = {propertyName}.{subscription.Name}();");
                    subscriptionsBuilder.AppendLine($"serverSubscriptions.Add({disposableName});");
                }
            }

            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using Zenject;");
            builder.AppendLine("using Mirror;");
            builder.AppendLine("using UniRx;");
            foreach (var usage in namespaces) builder.AppendLine($"using {usage};");
            builder.AppendLine("public class ServerSubscriptionHandler: NetworkBehaviour {");
            builder.AppendLine(propertiesBuilder.ToString());
            builder.AppendLine("    " + "public override void OnStartServer() {");
            builder.AppendLine("    " + "    " + subscriptionsBuilder);
            builder.AppendLine("    " + "}");
            builder.AppendLine("    " + "public override void OnStopServer() {");
            builder.AppendLine("    " + "    " + "if(!serverSubscriptions.IsDisposed)");
            builder.AppendLine("    " + "    " + "serverSubscriptions.Dispose();");
            builder.AppendLine("    " + "    " + "serverSubscriptions = new CompositeDisposable();");
            builder.AppendLine("    " + "}");
            builder.AppendLine("}");

            File.WriteAllText(path, builder.ToString());
            Debug.Log("ServerSubscriptionHandler updated!");
        }
    }
}