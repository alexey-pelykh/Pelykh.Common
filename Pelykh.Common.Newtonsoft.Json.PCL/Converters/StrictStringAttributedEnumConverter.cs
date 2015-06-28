using System;

namespace Pelykh.Common.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts (strictly) an <see cref="Enum"/> to and from its string value specified by <see cref="JsonEnumValueAttribute"/>.
    /// </summary>
    public class StrictStringAttributedEnumConverter : StringAttributedEnumConverter
    {
        public StrictStringAttributedEnumConverter()
            : base(true)
        {
        }
    }
}
