using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Utils.ZenjectCodegen.Editor
{
    internal static class SceneSubscriptionsHandlerGenerator
    {
        [InitializeOnLoadMethod]
        private static void Generate()
        {
            var path = $"{Application.dataPath}/Scripts/_Generated/SceneSubscriptionsHandler.cs";

            var propertiesBuilder = new StringBuilder();
            var subscriptionsBuilder = new StringBuilder();

            var subscriptionHandlers =
                from assembly in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from type in assembly.GetTypes()
                where type.IsDefined(typeof(SceneSubscriptionHandler), false)
                select type;

            var namespaces = new HashSet<string>();
            foreach (var handler in subscriptionHandlers)
            {
                namespaces.Add(handler.Namespace);
                var propertyName = "m_" + handler.Name.ToLower();
                propertiesBuilder.AppendLine($"[Inject] private {handler.Name} {propertyName};");
                var subscriptions = handler
                    .GetMethods()
                    .Where(method => method.GetCustomAttributes(typeof(SceneSubscription), true).Length > 0)
                    .Where(method => method.ReturnType == typeof(IDisposable))
                    .Where(method => method.GetParameters().Length == 0);

                foreach (var subscription in subscriptions)
                {
                    subscriptionsBuilder.AppendLine($"{propertyName}.{subscription.Name}().AddTo(this);");
                }
            }

            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using Zenject;");
            builder.AppendLine("using UniRx;");
            foreach (var usage in namespaces) builder.AppendLine($"using {usage};");
            builder.AppendLine("public class SceneSubscriptionsHandler: MonoBehaviour {");
            builder.AppendLine(propertiesBuilder.ToString());
            builder.AppendLine("    " + "private void Awake() {");
            builder.AppendLine("    " + subscriptionsBuilder);
            builder.AppendLine("    " + "}");
            builder.AppendLine("}");

            File.WriteAllText(path, builder.ToString());
            Debug.Log("SceneSubscriptionsHandler updated!");
        }
    }
}