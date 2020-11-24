using Core.Dtos;
using Core.Models;
using Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication17.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _userRepository;

        public SubjectController(ISubjectRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // [Authorize(Roles = Role.Teacher)]
        [AllowAnonymous]
        [HttpPost("addSubject")]
        public async Task<ActionResult> AddSubject(SubjectAddDto subjectAdd)
        {
            var subject = new Subject(subjectAdd.Name);
         
            if (await _userRepository.AddSubject(subject))
            {
                return Ok("A new subject was added successfully!");
            }

            return BadRequest("Oops, somthing wring happened!");
        }
      
      
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DSubject(Guid id)
        {
            if (await _userRepository.DeleteSubject(id))
            {
                return Ok("Subject was deleted");
            }
            return BadRequest("Some problem occured during deletion the subject");
        }
       
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(Guid id , Subject subject)
        {
            subject.Id_s = id;
            if (await _userRepository.UpdateSubject(subject))
            {
                return Ok("Subject was updated");
            }
            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet]
        public List<Subject> index()
        {
            return _userRepository.mainPage();
        }

    
    }
}
