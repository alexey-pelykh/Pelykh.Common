using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Pelykh.Common.Web.Http.Routing
{
    public class DirectRouteProvider : DefaultDirectRouteProvider
    {
        public string ControllersNamespace { get; private set; }

        public DirectRouteProvider(string controllersNamespace)
        {
            ControllersNamespace = controllersNamespace;
        }

        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
        }

        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            var routePrefix = base.GetRoutePrefix(controllerDescriptor);

            var autoRoutePrefixAttribute =
                controllerDescriptor.GetCustomAttributes<AutoRoutePrefixAttribute>(inherit: true).FirstOrDefault();
            if (autoRoutePrefixAttribute != null)
            {
                var autoRoutePrefix = string.Empty;

                var namespacePath = controllerDescriptor.ControllerType.Namespace.Replace(ControllersNamespace, "");
                if (namespacePath.Length > 0)
                {
                    autoRoutePrefix = string.Join("/", namespacePath
                        .Substring(1)
                        .Split(new char[] { '.' }, StringSplitOptions.None)
                        .Select(x => x.ToCamelCase()));
                }

                if (!autoRoutePrefixAttribute.ExcludeControllerName)
                {
                    var controllerName = controllerDescriptor.ControllerName.ToCamelCase();
                    if (string.IsNullOrEmpty(autoRoutePrefix))
                        autoRoutePrefix = controllerName;
                    else
                        autoRoutePrefix += "/" + controllerName;
                }

                if (string.IsNullOrEmpty(routePrefix) ||
                    autoRoutePrefixAttribute.Mode == AutoRoutePrefixMode.ReplaceDefaultPrefix)
                {
                    routePrefix = autoRoutePrefix;
                }
                else if (autoRoutePrefixAttribute.Mode == AutoRoutePrefixMode.AfterDefaultPrefix)
                {
                    routePrefix = routePrefix + "/" + autoRoutePrefix;
                }
                else if (autoRoutePrefixAttribute.Mode == AutoRoutePrefixMode.BeforeDefaultPrefix)
                {
                    routePrefix = autoRoutePrefix + "/" + routePrefix;
                }
            }

            return routePrefix;
        }
    }
}
