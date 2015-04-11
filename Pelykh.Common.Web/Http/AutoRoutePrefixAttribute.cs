using System;

namespace Pelykh.Common.Web.Http
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AutoRoutePrefixAttribute : Attribute
    {
        public AutoRoutePrefixMode Mode { get; set; }
        public bool ExcludeControllerName { get; set; }

        public AutoRoutePrefixAttribute()
        {
            Mode = AutoRoutePrefixMode.BeforeDefaultPrefix;
            ExcludeControllerName = false;
        }
    }
}
