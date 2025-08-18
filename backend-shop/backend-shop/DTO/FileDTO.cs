using System.ComponentModel.DataAnnotations;

namespace backend_shop.DTO
{
    public class FileDTO
    {
        public required string Name { get; set; }

        [Required]
        public string ContentType { get; set; } = "";

        [Required]
        public byte[] Content { get; set; } = [];
    }
}
