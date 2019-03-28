using System.ComponentModel.DataAnnotations;

namespace Interceptors.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}