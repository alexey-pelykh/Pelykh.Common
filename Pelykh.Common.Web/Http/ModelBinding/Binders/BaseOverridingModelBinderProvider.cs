using System.Web.Http.ModelBinding;

namespace Pelykh.Common.Web.Http.ModelBinding.Binders
{
    public abstract class BaseOverridingModelBinderProvider<TOriginalProvider> : ModelBinderProvider
        where TOriginalProvider : ModelBinderProvider
    {
        public TOriginalProvider OriginalProvider { get; protected set; }

        public BaseOverridingModelBinderProvider(TOriginalProvider originalProvider)
        {
            OriginalProvider = originalProvider;
        }
    }
}
