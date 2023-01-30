using System;

namespace Utils.MirrorCodegen
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class RoomParamAttribute : Attribute
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class ParamGetterAttribute : Attribute{}
        [AttributeUsage(AttributeTargets.Method)]
        public class ParamGetterFlowAttribute : Attribute{}
        [AttributeUsage(AttributeTargets.Method)]
        public class ParamSetterAttribute : Attribute{}
    }

}