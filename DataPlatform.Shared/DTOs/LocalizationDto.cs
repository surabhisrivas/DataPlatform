namespace DataPlatform.Shared.DTOs
{
    public class LocalizationDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = "";
        public string Language { get; set; } = "";
        public string Value { get; set; } = "";
    }

    public class CreateLocalizationDto
    {
        public string Key { get; set; } = "";
        public string Language { get; set; } = "";
        public string Value { get; set; } = "";
    }
}