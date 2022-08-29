using FinalAgain.DAL;
using FinalAgain.Models;
using FinalAgain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalAgain.Controllers
{
    public class FooormController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDBContext _context;

        public FooormController(AppDBContext context , UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Fooorms.OrderByDescending(x => x.ViewCount).Take(10).ToList();
            return View(data);
        }
        public IActionResult CreateFooorm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateFooorm(Fooorm form)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                form.CreatedAt = DateTime.Now;
                form.User = user;
                form.UserId = user.Id;
                _context.Fooorms.Add(form);
                _context.SaveChanges();
                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult ShowFooormItem([FromBody] int id)
        {
            var data = _context.Fooorms.FirstOrDefault(x => x.Id == id);
            data.ViewCount += 1;
            _context.SaveChanges();
            return Ok(data);
        }
        public IActionResult ShowFroomAnswers([FromBody] int id)
        {
            var answers = _context.FooormAnswers.Include(x=>x.User).Where(x=>x.FooormId==id).ToList();
            return Ok(answers);
        }
        public IActionResult SetMessage([FromBody] FooormAnswers answer)
        {
            AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var fooorm = _context.Fooorms.FirstOrDefault(x => x.Id == answer.FooormId);
            fooorm.AnswerCount += 1;
            answer.User = user;
            answer.UserId = user.Id;
            answer.Fooorm = fooorm;
            answer.CreatedAt = DateTime.Now;
            _context.FooormAnswers.Add(answer);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult SortTopQuestions()
        {
            var data=_context.Fooorms.OrderByDescending(x => x.ViewCount).Take(10).ToList();
            return Ok(data);
        }
        public IActionResult SortLatestQuestions()
        {
            var data = _context.Fooorms.OrderByDescending(x => x.CreatedAt).Take(10).ToList();
            return Ok(data);
        }
        public IActionResult SetUsefulMessage([FromBody] UserfulMessageViewModel model)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (user.Id == model.UserId)
            {
                var question = _context.FooormAnswers.FirstOrDefault(x => x.Id == model.TextId);
                if (question.IsUseful == false)
                {
                    question.IsUseful = true;
                    _context.SaveChanges();
                    return Ok(true);
                }
                question.IsUseful = false;
                _context.SaveChanges();
                return Ok(true);
            }
            return Ok(false);
        }
        public IActionResult SearchFooorm([FromBody] string value)
        {
            var data = _context.Fooorms.Where(x => x.Header.Contains(value)).ToList();
            return Ok(data);
        }
    }
}
