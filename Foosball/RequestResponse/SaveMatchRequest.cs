using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Old;

namespace Foosball.RequestResponse
{
    public class SaveMatchesRequest : BaseRequest
    {
        [Required]
        public List<Match> Matches { get; set; }
    }
}