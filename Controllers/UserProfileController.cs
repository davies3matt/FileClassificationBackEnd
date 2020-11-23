using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using c_sharp_grad_backend.Data;
using c_sharp_grad_backend.Dtos;
using c_sharp_grad_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace c_sharp_grad_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly DataContext _context;

        public UserProfileController(DataContext context)
        {
            _context = context;
        }

        //place this attribute above your service to secure it
        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Get(UserForProfileDto userForProfileDto)
        {
            var values = await _context.TableUserProfiles.FirstOrDefaultAsync(x => x.Username == userForProfileDto.Username);

            return Ok(values);
        }




        [HttpPost("addprofile")]
        public async Task<IActionResult> AddProfile(UserProfile userProfile)
        {
            await _context.TableUserProfiles.AddAsync(userProfile);
            await _context.SaveChangesAsync();


            return StatusCode(201);
        }


        [HttpPut("editprofile")]
        public async Task<IActionResult> EditProfile(UserProfile userProfile)
        {
            //await _context.TableUserProfiles.AddAsync(userProfile);
            //await _context.SaveChangesAsync();

            var result = _context.TableUserProfiles.SingleOrDefault(b => b.Username == userProfile.Username);
            if (result != null)
            {
                result.Firstname = userProfile.Firstname;
                result.Lastname = userProfile.Lastname;
                result.AvatarOne = userProfile.AvatarOne;
                result.AvatarTwo = userProfile.AvatarTwo;
                result.AvatarThree = userProfile.AvatarThree;
                result.Email = userProfile.Email;
                result.Cell = userProfile.Cell;
                result.AddressOne = userProfile.AddressOne;
                result.AddressTwo = userProfile.AddressTwo;
                result.Country = userProfile.Country;
                result.Zip = userProfile.Zip;
                result.NameOnCard = userProfile.NameOnCard;
                result.PaymentType = userProfile.PaymentType;
                result.CardNumber = userProfile.CardNumber;
                result.CVV = userProfile.CVV;
                result.ExpirationOnCard = userProfile.ExpirationOnCard;
                result.ExtraPropOne = userProfile.ExtraPropOne;
                result.ExtraPropTwo = userProfile.ExtraPropTwo;
                result.ExtraPropThree = userProfile.ExtraPropThree;
                result.ExtraPropFour = userProfile.ExtraPropFour;




                _context.SaveChanges();
            }

            return StatusCode(200);
        }

    }
}