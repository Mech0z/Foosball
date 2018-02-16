using System.Collections.Generic;
using FoosballCore.OldLogic;
using FoosballCore.RequestResponse;
using Microsoft.AspNetCore.Mvc;
using Models.Old;
using Repository;

namespace FoosballCore.Controllers
{
    public class SeasonsAdministrationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISeasonLogic _seasonLogic;

        public SeasonsAdministrationController(IUserRepository userRepository, ISeasonLogic seasonLogic)
        {
            _userRepository = userRepository;
            _seasonLogic = seasonLogic;
        }

        //[HttpPost]
        //public IActionResult StartNewSeason(VoidRequest request)
        //{
        //    var validated = _userRepository.ValidateAndHasRole(new User
        //    {
        //        Email = request.Email,
        //        Password = request.Password
        //    }, "Admin");

        //    if (!validated)
        //    {
        //        return Unauthorized();
        //    }
            
        //    var seasonName = _seasonLogic.StartNewSeason();

        //    return Ok(seasonName);
        //}

        //[HttpPost]
        //public IActionResult GetSeasons(VoidRequest request)
        //{
        //    var validated = _userRepository.ValidateAndHasRole(new User
        //    {
        //        Email = request.Email,
        //        Password = request.Password
        //    }, "Admin");

        //    if (!validated)
        //    {
        //        return Unauthorized();
        //    }

        //    List<Season> seasons = _seasonLogic.GetSeasons();
        //    return Ok(seasons);
        //}
    }
}