

namespace eMotoCare.BO.DTO.Responses
{
    public class PartTypeResponse
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }
    }
}
