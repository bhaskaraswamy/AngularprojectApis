using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularprojectApis.Data;
using AngularprojectApis.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace AngularprojectApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApiContext _context;

        public UsersController(ApiContext context)
        {
            _context = context;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string Email,string password)
        { 
            if(Email =="" && password == "")
            {
                return NotFound("Item not found");
            }
            var user = await _context.Users.FirstOrDefaultAsync(a=>a.Email == Email && a.Password == password);
            if(user == null)
            {
                return NotFound("You are unauthorised");
            }
            return Ok(user);
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            var email = await _context.Users.FirstOrDefaultAsync(a => a.Email == users.Email);
            if (email == null)
            {
                _context.Users.Add(users);
                await _context.SaveChangesAsync();
            }
            else
            {
                return Unauthorized("Email already Exist");
            }

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> SendOtptoMail(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var User = _context.Users.Find(id);
            if (User != null)
            {
                Random random = new Random();
                int fourDigitNumber = random.Next(1000, 9999);

                User.OTP = (fourDigitNumber).ToString();

                _context.Entry(User).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("LoginOtp", "loginOtpEmail@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("User", User.Email));
                emailMessage.Subject = "OTP";
                emailMessage.Body = new TextPart("plain") { Text = (fourDigitNumber).ToString() };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("bhaskaraswamy99@gmail.com", "jbhytaxmmcgrudzu");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }

            return Ok();

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyOTP(int id,string otp)
        {

            if(id == 0 && otp==null)
            {
                return BadRequest();
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user==null)
            {
                return BadRequest();
            }
            if(user.OTP == otp)
            {
                return Ok("you are welcome");
            }
            else
            {
               return BadRequest("You enter incorrect OTP");
            }
        }

        

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
