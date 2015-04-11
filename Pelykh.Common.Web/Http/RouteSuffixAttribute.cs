using System;

namespace Pelykh.Common.Web.Http
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RouteSuffixAttribute : Attribute, IRouteSuffix
    {
        public string Suffix { get; private set; }
        public RouteSuffixType SuffixType { get; private set; }

        public RouteSuffixAttribute(string suffix, RouteSuffixType suffixType = RouteSuffixType.Default)
        {
            Suffix = suffix;
            SuffixType = suffixType;
        }
    }
}
