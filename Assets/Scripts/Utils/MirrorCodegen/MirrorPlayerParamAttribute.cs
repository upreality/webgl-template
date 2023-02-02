using System;

namespace Utils.MirrorCodegen
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class PlayerParamAttribute : Attribute
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class PlayerParamGetterAttribute : Attribute{}
        [AttributeUsage(AttributeTargets.Method)]
        public class PlayerParamGetterFlowAttribute : Attribute{}
        [AttributeUsage(AttributeTargets.Method)]
        public class PlayerParamSetterAttribute : Attribute{}
    }

}