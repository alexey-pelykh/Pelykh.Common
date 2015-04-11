using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Routing;

namespace Pelykh.Common.Web.Http
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RouteExAttribute : Attribute, IDirectRouteFactory, IHttpRouteInfoProvider
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Template { get; private set; }
        public RouteSuffixType AppendedRouteSuffixType
        {
            get
            {
                return appendRouteSuffix.Value;
            }
            set
            {
                appendRouteSuffix = value;
            }
        }
        public bool AppendRouteSuffix
        {
            get
            {
                return appendRouteSuffix.HasValue;
            }
            set
            {
                if (!value)
                    appendRouteSuffix = null;
            }
        }

        private RouteSuffixType? appendRouteSuffix;

        public RouteExAttribute()
        {
            Template = String.Empty;
        }

        public RouteExAttribute(string templateFormat, params object[] arguments)
        {
            if (templateFormat == null)
                throw new ArgumentNullException("templateFormat");

            var safeTemplateFormat = Regex.Replace(templateFormat, @"({[^}\d]+\d?[^}]})", @"{$1}");
            var formattedTemplate = string.Format(safeTemplateFormat, arguments);
            Template = Regex.Replace(formattedTemplate, @"{({[^}\d]+\d?[^}]})}", @"$1");
        }

        RouteEntry IDirectRouteFactory.CreateRoute(DirectRouteFactoryContext context)
        {
            Contract.Assert(context != null);

            var template = Template;
            if (AppendRouteSuffix)
            {
                var controllerDescriptor = context.Actions.First().ControllerDescriptor;
                var routeSuffixAttributes = controllerDescriptor.GetCustomAttributes<IRouteSuffix>(inherit: false);
                foreach (var routeSuffixAttribute in routeSuffixAttributes)
                {
                    if (routeSuffixAttribute.SuffixType != AppendedRouteSuffixType)
                        continue;

                    if (string.IsNullOrEmpty(template))
                        template = routeSuffixAttribute.Suffix;
                    else
                        template += "/" + routeSuffixAttribute.Suffix;

                    break;
                }
            }
            IDirectRouteBuilder builder = context.CreateBuilder(template);
            Contract.Assert(builder != null);

            builder.Name = Name;
            builder.Order = Order;
            return builder.Build();
        }
    }
}
