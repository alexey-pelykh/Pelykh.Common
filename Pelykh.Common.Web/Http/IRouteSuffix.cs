namespace Pelykh.Common.Web.Http
{
    public interface IRouteSuffix
    {
        string Suffix { get; }
        RouteSuffixType SuffixType { get; }
    }
}
