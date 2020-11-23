using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using c_sharp_grad_backend.Data;
using c_sharp_grad_backend.Dtos;
using c_sharp_grad_backend.Models;
using c_sharp_grad_backend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace c_sharp_grad_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationsController : ControllerBase
    {
        List<Donations> myList;
        private readonly DataContext _context;

        public DonationsController(DataContext context)
        {
            _context = context;

            myList = new List<Donations>();
        }

        //place this attribute above your service to secure it
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = await _context.TableDonations.ToListAsync();
            return Ok(values);
        }


        [HttpPost("adddonation")]
        public async Task<IActionResult> AddDonation(Donations donation)
        {
            await _context.TableDonations.AddAsync(donation);
            await _context.SaveChangesAsync();

            var values = await _context.TableUserProfiles.FirstOrDefaultAsync(x => x.Username == donation.Username);

            string cell = values.Cell;
            string username = values.Firstname;
            string amount = donation.Amount.ToString();

            var message = String.Format("Dear {0}, you have donated R{1} to the COVID-19 cause.", username , amount);


            return StatusCode(201);
        }

        

        [HttpPost("userdonations")]
        public async Task<IActionResult> UserDonations(UserForDonationDto userForDonationDto)
        {
            myList.Clear();
            
            var values = await _context.TableDonations.ToListAsync();
            

            foreach (var item in values)
            {
                if (item.Username == userForDonationDto.Username)
                {
                    myList.Add(item);
                }
            }


            return Ok(myList);
        }


        [HttpPost("posttext")]
        public async Task<IActionResult> AddText(TextFile textfile)
        {
            var encodedString = Convert.ToBase64String(textfile.AvatarOne);
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);

            var helper = new TextMatchHelper();
            var helperResponse = helper.PostText(decodedString);

            return Ok(helperResponse);
        }



        [HttpPost("writetext")]
        public async Task<IActionResult> AddText(List<TextFile> textfile)
        {
            //var guy = textfile;

            //await _context.TableDonations.AddAsync(donation);

            await _context.TableTextFiles.AddRangeAsync(textfile);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}