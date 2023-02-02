using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ModestTree;
using UnityEditor;
using UnityEngine;
using static Utils.MirrorCodegen.RoomParamAttribute;

namespace Utils.MirrorCodegen.Editor
{
    internal static class MirrorRoomParamsHandlerGenerator
    {
        [InitializeOnLoadMethod]
        private static void Generate()
        {
            var path = $"{Application.dataPath}/Scripts/_Generated/MirrorRoomParamsHandler.cs";

            var interfaces = new HashSet<string>();
            var propertiesBuilder = new StringBuilder();
            var awakeBuilder = new StringBuilder();
            var methodsBuilder = new StringBuilder();

            var roomParamHandlers =
                from assembly in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from type in assembly.GetTypes()
                where type.IsDefined(typeof(RoomParamAttribute), false)
                select type;

            var implementationData = new List<RoomParamsImplementationData>();
            foreach (var handler in roomParamHandlers)
            {
                if (!ParseRoomParamsImplementationData(handler, out var data))
                    continue;

                implementationData.Add(data);
            }

            var namespaces = new HashSet<string>();

            foreach (var implData in implementationData)
            {
                namespaces.Add(implData.Namespace);
                namespaces.Add(implData.ParamType.Namespace);
                interfaces.Add(implData.Name);

                var hasFlowGetter = !implData.GetterFlowName.IsEmpty();

                var hookMethodName = implData.Name + "Hook";
                var paramTypeName = implData.ParamType.Name;

                var propertyShortName = implData.Name;
                if (propertyShortName.StartsWith('I'))
                    propertyShortName = propertyShortName[1..];

                if (propertyShortName.EndsWith("Repository"))
                    propertyShortName = propertyShortName.Remove(propertyShortName.Length - 10);
                
                var propertyFullName = "m_" + propertyShortName + "Param";
                var syncAttr = hasFlowGetter ? $"[SyncVar(hook = nameof({hookMethodName}))]" : "[SyncVar]";
                var propLine = $"[SerializeField]{syncAttr} private {paramTypeName} {propertyFullName};";
                propertiesBuilder.AppendLine(propLine);

                var subjPropertyName = propertyFullName + "Subject";
                if (hasFlowGetter)
                {
                    var subjPropertyTypeText = $"BehaviorSubject<{paramTypeName}>";
                    var subjPropLine = $"private {subjPropertyTypeText} {subjPropertyName};";
                    propertiesBuilder.AppendLine(subjPropLine);
                    awakeBuilder.AppendLine($"{subjPropertyName} = new {subjPropertyTypeText}({propertyFullName});");
                }

                var getterDefinition = $"public {paramTypeName} {implData.GetterName}()";
                var getterDelegation = $"=> {propertyFullName};";
                methodsBuilder.AppendLine(getterDefinition + getterDelegation);
                methodsBuilder.AppendLine(string.Empty);

                var setterParamDefinition = $"{paramTypeName} {implData.SetterParamName}";
                methodsBuilder.AppendLine($"public void {implData.SetterName}({setterParamDefinition})");
                methodsBuilder.AppendLine("{");
                // methodsBuilder.AppendLine("if(!isServer) return;");
                methodsBuilder.AppendLine($"{propertyFullName} = {implData.SetterParamName};");
                if (hasFlowGetter)
                    methodsBuilder.AppendLine($"{subjPropertyName}.OnNext({implData.SetterParamName});");
                methodsBuilder.AppendLine("}");
                methodsBuilder.AppendLine(string.Empty);

                if (!hasFlowGetter)
                    continue;

                var getterFlowDefinition = $"public IObservable<{paramTypeName}> {implData.GetterFlowName}()";
                var getterFlowDelegate = $"=> {subjPropertyName};";
                methodsBuilder.AppendLine(getterFlowDefinition + getterFlowDelegate);

                var hookDefinition = $"private void {hookMethodName}({paramTypeName} first, {paramTypeName} second)";
                methodsBuilder.AppendLine(hookDefinition);
                methodsBuilder.AppendLine("{");
                methodsBuilder.AppendLine($"{subjPropertyName}.OnNext(second);");
                methodsBuilder.AppendLine("}");
                methodsBuilder.AppendLine(string.Empty);
            }

            var builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using UnityEngine;");
            builder.AppendLine("using UniRx;");
            builder.AppendLine("using Mirror;");
            builder.AppendLine("");
            foreach (var usage in namespaces) builder.AppendLine($"using {usage};");
            builder.AppendLine("public class MirrorRoomParamsHandler: NetworkBehaviour");
            foreach (var interfaceName in interfaces) 
                builder.AppendLine($",{interfaceName}");
            builder.AppendLine("{");
            builder.AppendLine(propertiesBuilder.ToString());
            builder.AppendLine("    " + "private void Awake() {");
            builder.AppendLine("    " + awakeBuilder);
            builder.AppendLine("    " + "}");
            builder.AppendLine(methodsBuilder.ToString());
            builder.AppendLine("}");

            File.WriteAllText(path, builder.ToString());
            Debug.Log("MirrorRoomParamsHandler updated!");
        }

        private static bool ParseRoomParamsImplementationData(Type handler, out RoomParamsImplementationData data)
        {
            data = default;
            if (!FindAttributedMethod<ParamGetterAttribute>(handler, out var getter))
                return false;

            var paramType = getter.ReturnType;
            if (!paramType.IsValueType)
                return false;

            if (!getter.GetParameters().IsEmpty())
                return false;

            if (!FindAttributedMethod<ParamSetterAttribute>(handler, out var setter))
                return false;

            if (setter.ReturnType != typeof(void))
                return false;

            var setterParameters = setter.GetParameters().ToList();
            if (setterParameters.Count != 1)
                return false;

            var setterArg = setterParameters[0];
            if (setterArg.ParameterType != paramType)
                return false;

            var hasFlowGetter = FindFlowGetter(handler, paramType, out var flowGetter);

            data = new RoomParamsImplementationData
            {
                Name = handler.Name,
                Namespace = handler.Namespace,
                ParamType = paramType,
                GetterName = getter.Name,
                SetterName = setter.Name,
                SetterParamName = setterArg.Name,
                GetterFlowName = hasFlowGetter ? flowGetter : string.Empty
            };
            return true;
        }

        private struct RoomParamsImplementationData
        {
            public string Name;
            public string Namespace;
            public Type ParamType;
            public string GetterName;
            public string SetterName;
            public string SetterParamName;
            public string GetterFlowName;
        }

        private static bool FindFlowGetter(Type handler, Type flowParamType, out string getterFlowName)
        {
            getterFlowName = default;
            if (!FindAttributedMethod<ParamGetterFlowAttribute>(handler, out var getterFlow))
                return false;

            var paramType = getterFlow.ReturnType;
            if (!paramType.IsGenericType && paramType.GetGenericTypeDefinition() == typeof(IObservable<>))
                return false;

            var genericArg = paramType.GetGenericArguments().FirstOrDefault();
            if (genericArg != flowParamType)
                return false;

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