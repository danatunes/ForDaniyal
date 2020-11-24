using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Models;
using Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication17.Controllers
{
   
    
        [Authorize]
        [Route("api/[controller]")]
        [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _testRepository;

            public QuestionController(IQuestionRepository testRepository)
            {
            _testRepository = testRepository;
            }          

            [AllowAnonymous]
            [HttpPost("addQuestion")]
            public async Task<ActionResult> AddQuestion(QuestionAddDto questionAdd)
            {
            var question = new Question(
                questionAdd.QuestionN,
                questionAdd.Option1,
                questionAdd.Option2, 
                questionAdd.Option3,
                questionAdd.Option4,
                questionAdd.CorrectAns,
                questionAdd.SId);


                if (await _testRepository.AddQuestion(question))
                {
                    return Ok("A new question was added successfully!");
                }

                return BadRequest("Oops, something wring happened!");
            }
            
            [AllowAnonymous]
            [HttpGet]
            public List<Question> getTests()
            {
            return _testRepository.getTests();
            }


            [AllowAnonymous]
            [HttpGet("{id}")]
            public List<Question> getTest(Guid id)
            {
            if (id != null)
            {
                return _testRepository.getTest(id);
            }
            return null;
            }


            [AllowAnonymous]
            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateQuestion(Guid id, Question question)
            {
                question.QId = id;
                if (await _testRepository.UpdateQuestion(question))
                {
                    return Ok("Question was updated");
                }
                return NoContent();
            }

            [AllowAnonymous]
            [HttpGet("getAnswer/{id}")]
            public List<string> getAnswer(Guid id)
            {
            if (id != null)
            {
                return _testRepository.getAnswer(id);
            }
            return null;
            }

        
            [AllowAnonymous]
            [HttpDelete("{id}")]
            public async Task<ActionResult> DQuestion(Guid id)
            {
                if (await _testRepository.DeleteQuestion(id))
                {
                    return Ok("Question was deleted");
                }
                return BadRequest("Some problem occured during deletion the question");
            }





    }

}
