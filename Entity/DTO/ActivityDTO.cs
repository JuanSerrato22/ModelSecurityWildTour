using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Entity.DTO
{
    public class ActivityDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string? Name { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder 1000 caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [StringLength(100, ErrorMessage = "La categoría no puede exceder 100 caracteres")]
        public string? Category { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Price { get; set; }

        [JsonConverter(typeof(TimeSpanHHmmConverter))]
        public TimeSpan DurationHours { get; set; }
    }

    // Conversor personalizado para usar múltiples formatos
    public class TimeSpanHHmmConverter : JsonConverter<TimeSpan>
    {
        private static readonly string[] Formats = { @"hh\:mm", @"h\:mm", @"mm" };

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
                return TimeSpan.Zero;

            // Limpiar el valor de entrada
            value = value.Trim().ToLowerInvariant();

            // Manejar formato "X horas" o "X hora"
            if (value.Contains("hora"))
            {
                var numberPart = value.Replace("horas", "").Replace("hora", "").Trim();
                if (decimal.TryParse(numberPart, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var hours))
                {
                    return TimeSpan.FromHours((double)hours);
                }
            }

            // Manejar formato "X minutos" o "X minuto"
            if (value.Contains("minuto"))
            {
                var numberPart = value.Replace("minutos", "").Replace("minuto", "").Trim();
                if (decimal.TryParse(numberPart, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var minutes))
                {
                    return TimeSpan.FromMinutes((double)minutes);
                }
            }

            // Intentar parsear como formato "HH:mm" o "H:mm"
            foreach (var format in Formats)
            {
                if (TimeSpan.TryParseExact(value, format, null, out var result))
                    return result;
            }

            // Intentar parsear como número decimal (horas)
            if (decimal.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var hoursDecimal))
            {
                return TimeSpan.FromHours((double)hoursDecimal);
            }

            // Intentar parsear como TimeSpan estándar
            if (TimeSpan.TryParse(value, out var timeSpan))
                return timeSpan;

            throw new JsonException($"Unable to convert \"{value}\" to TimeSpan. Expected formats: HH:mm, H:mm, decimal hours, or 'X horas'/'X minutos'.");
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(@"hh\:mm"));
        }
    }


}