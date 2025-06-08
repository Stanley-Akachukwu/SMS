using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SMS.ApiService.Converters
{
    public class NullableUlidToStringConverter : ValueConverter<Ulid?, string>
    {
        public NullableUlidToStringConverter()
            : base(
                ulid => ulid.HasValue ? ulid.Value.ToString() : null,
                str => string.IsNullOrEmpty(str) ? (Ulid?)null : Ulid.Parse(str))
        {
        }
    }
}
