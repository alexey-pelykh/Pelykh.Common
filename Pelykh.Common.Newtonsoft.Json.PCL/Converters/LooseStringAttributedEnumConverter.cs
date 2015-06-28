using System;

namespace Pelykh.Common.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts (loosely) an <see cref="Enum"/> to and from its string value specified by <see cref="JsonEnumValueAttribute"/>.
    /// </summary>
    public class LooseStringAttributedEnumConverter : StringAttributedEnumConverter
    {
        public LooseStringAttributedEnumConverter()
            : base(false)
        {
        }
    }
}
