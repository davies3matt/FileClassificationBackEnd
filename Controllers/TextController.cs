using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class TextController : ControllerBase
    {
        List<Donations> myList;
        private readonly DataContext _context;

        public TextController(DataContext context)
        {
            _context = context;

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


    }
}