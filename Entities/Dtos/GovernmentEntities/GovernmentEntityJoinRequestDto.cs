namespace gov_API.DTOs.GovernmentEntities
{
    public class GovernmentEntityJoinRequestDto
    {
        public string EntityName { get; set; } = string.Empty;

        public string EntityEmail { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string AdminFullName { get; set; } = string.Empty;

        public string AdminEmail { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public List<ReadinessAnswerDto> ReadinessAnswers { get; set; } = new();
    }
}