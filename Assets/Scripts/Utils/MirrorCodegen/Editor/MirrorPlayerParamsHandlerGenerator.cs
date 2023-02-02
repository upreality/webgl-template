using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ModestTree;
using UnityEditor;
using UnityEngine;
using static Utils.MirrorCodegen.PlayerParamAttribute;

namespace Utils.MirrorCodegen.Editor
{
    internal static class MirrorPlayerParamsHandlerGenerator
    {
        [InitializeOnLoadMethod]
        private static void Generate()
        {
            var path = $"{Application.dataPath}/Scripts/_Generated/MirrorPlayerParamsHandler.cs";

            var interfaces = new HashSet<string>();
            var propertiesBuilder = new StringBuilder();
            var methodsBuilder = new StringBuilder();

            var playerParamHandlers =
                from assembly in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from type in assembly.GetTypes()
                where type.IsDefined(typeof(PlayerParamAttribute), false)
                select type;

            var implementationData = new List<PlayerParamsImplementationData>();
            foreach (var handler in playerParamHandlers)
            {
                if (!ParsePlayerParamsImplementationData(handler, out var data))
                    continue;

                implementationData.Add(data);
            }

            var namespaces = new HashSet<string>();

            foreach (var implData in implementationData)
            {
                namespaces.Add(implData.Namespace);
                namespaces.Add(implData.ParamType.Namespace);
                interfaces.Add(implData.Name);

                var hasFlowGetter = !implData.GetterFlowName.IsEmpty() && !implData.GetterFlowPlayerIdArgName.IsEmpty();

                var paramTypeName = implData.ParamType.Name;

                var propertyShortName = implData.Name;
                if (propertyShortName.StartsWith('I'))
                    propertyShortName = propertyShortName[1..];

                if (propertyShortName.EndsWith("Repository"))
                    propertyShortName = propertyShortName.Remove(propertyShortName.Length - 10);

                var propertyFullName = "m_playerIdTo" + propertyShortName;

                var propLine =
                    $"[SerializeField] private readonly SyncReactiveDictionary<string,{paramTypeName}> {propertyFullName} = new();";
                propertiesBuilder.AppendLine(propLine);

                var getterDefinition =
                    $"public {paramTypeName} {implData.GetterName}(string {implData.GetterPlayerIdArgName})";
                var getterDelegation = $"=> {propertyFullName}[{implData.GetterPlayerIdArgName}];";
                methodsBuilder.AppendLine(getterDefinition + getterDelegation);
                methodsBuilder.AppendLine(string.Empty);

                var setterParamDefinition =
                    $"string {implData.SetterPlayerIdArgName}, {paramTypeName} {implData.SetterParamName}";
                methodsBuilder.AppendLine($"public void {implData.SetterName}({setterParamDefinition})");
                methodsBuilder.AppendLine("{");
                methodsBuilder.AppendLine("if(!isServer) return;");
                methodsBuilder.AppendLine(
                    $"{propertyFullName}[{implData.SetterPlayerIdArgName}] = {implData.SetterParamName};");
                methodsBuilder.AppendLine("}");
                methodsBuilder.AppendLine(string.Empty);

                if (!hasFlowGetter)
                    continue;

                var getterFlowDefinition =
                    $"public IObservable<{paramTypeName}> {implData.GetterFlowName}(string {implData.GetterFlowPlayerIdArgName})";
                var getterFlowDelegate = $"=> {propertyFullName}.GetItemFlow({implData.GetterFlowPlayerIdArgName});";
                methodsBuilder.AppendLine(getterFlowDefinition + getterFlowDelegate);
                methodsBuilder.AppendLine(string.Empty);
            }

            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using Mirror;");
            builder.AppendLine("using Utils.MirrorUtils;");
            builder.AppendLine("using Utils.Reactive;");
            builder.AppendLine("");
            foreach (var usage in namespaces) builder.AppendLine($"using {usage};");
            builder.AppendLine("public class MirrorPlayerParamsHandler: NetworkBehaviour");
            foreach (var interfaceName in interfaces)
                builder.AppendLine($",{interfaceName}");
            builder.AppendLine("{");
            builder.AppendLine(propertiesBuilder.ToString());
            builder.AppendLine(methodsBuilder.ToString());
            builder.AppendLine("}");

            File.WriteAllText(path, builder.ToString());
            Debug.Log("MirrorPlayerParamsHandler updated!");
        }

        private static bool ParsePlayerParamsImplementationData(Type handler, out PlayerParamsImplementationData data)
        {
            data = default;
            if (!FindAttributedMethod<PlayerParamGetterAttribute>(handler, out var getter))
                return false;

            var paramType = getter.ReturnType;
            if (!paramType.IsValueType)
                return false;

            var getterParameters = getter.GetParameters();
            if (getterParameters.Length != 1)
                return false;

            var firstGetterParam = getterParameters[0];
            if (firstGetterParam.ParameterType != typeof(string))
                return false;

            var getterPlayerIdArgName = firstGetterParam.Name;

            if (!FindAttributedMethod<PlayerParamSetterAttribute>(handler, out var setter))
                return false;

            if (setter.ReturnType != typeof(void))
                return false;

            var setterParameters = setter.GetParameters().ToList();
            if (setterParameters.Count != 2)
                return false;

            var firstSetterParam = setterParameters[0];
            if (firstSetterParam.ParameterType != typeof(string))
                return false;

            var setterPlayerIdArgName = firstSetterParam.Name;

            var setterArg = setterParameters[1];
            if (setterArg.ParameterType != paramType)
                return false;

            var hasFlowGetter = FindFlowGetter(handler, paramType, out var flowGetter, out var flowGetterPlayerId);

            data = new PlayerParamsImplementationData
            {
                Name = handler.Name,
                Namespace = handler.Namespace,
                ParamType = paramType,
                GetterName = getter.Name,
                GetterPlayerIdArgName = getterPlayerIdArgName,
                SetterName = setter.Name,
                SetterPlayerIdArgName = setterPlayerIdArgName,
                SetterParamName = setterArg.Name,
                GetterFlowName = hasFlowGetter ? flowGetter : string.Empty,
                GetterFlowPlayerIdArgName = hasFlowGetter ? flowGetterPlayerId : string.Empty
            };
            return true;
        }

        private struct PlayerParamsImplementationData
        {
            public string Name;
            public string Namespace;
            public Type ParamType;
            public string GetterName;
            public string GetterPlayerIdArgName;
            public string SetterName;
            public string SetterPlayerIdArgName;
            public string SetterParamName;
            public string GetterFlowName;
            public string GetterFlowPlayerIdArgName;
        }

        private static bool FindFlowGetter(
            Type handler,
            Type flowParamType,
            out string getterFlowName,
            out string getterFlowParamName
        )
        {
            getterFlowName = default;
            getterFlowParamName = default;
            if (!FindAttributedMethod<PlayerParamGetterFlowAttribute>(handler, out var getterFlow))
                return false;

            var paramType = getterFlow.ReturnType;
            if (!paramType.IsGenericType && paramType.GetGenericTypeDefinition() == typeof(IObservable<>))
                return false;

            var genericArg = paramType.GetGenericArguments().FirstOrDefault();
            if (genericArg != flowParamType)
                return false;

            var parameters = getterFlow.GetParameters();
            if (parameters.Length != 1)
                return false;

            var firstParameter = parameters[0];
            if (firstParameter.ParameterType != typeof(string))
                return false;

            getterFlowParamName = firstParameter.Name;
            getterFlowName = getterFlow.Name;
            return true;
        }

        private static bool FindAttributedMethod<T>(Type handler, out MethodInfo method) where T : Attribute
        {
            var matchingMethods = handler
                .GetMethods()
                .Where(method => method.IsDefined(typeof(T), false))
                .ToList();

            var hasSingleMethod = matchingMethods.Count == 1;
            method = hasSingleMethod ? matchingMethods.First() : default;
            return hasSingleMethod;
        }
    }
}