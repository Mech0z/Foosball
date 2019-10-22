using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Old;

namespace Models.RequestResponses
{
    public class SaveMatchesRequest
    {
        [Required]
        public List<Match>? Matches { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}