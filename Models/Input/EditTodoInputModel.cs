using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.Input
{
    public class EditTodoInputModel
    {
        [Required(ErrorMessage = "Id is required!")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        [StringLength(60, MinimumLength = 6, ErrorMessage = "Minimum 6 characteres and maximum 60")]
        public string? Title { get; set; }
    }
}
