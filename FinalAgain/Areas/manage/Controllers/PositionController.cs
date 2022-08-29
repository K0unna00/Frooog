using FinalAgain.DAL;
using FinalAgain.Helpers;
using FinalAgain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalAgain.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize]
    public class PositionController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;

        public PositionController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page=1)
        {
            var data = _context.Positions.ToList();
            ViewBag.pageMax = (data.Count%2==0?data.Count/2:(data.Count/2)+1);
            ViewBag.startCount = page*2-1;
            ViewBag.currentPage = page;
            if(page!=1)
                data = _context.Positions.Skip(page*2-2).Take(2).ToList();
            else
                data = _context.Positions.Skip(0).Take(2).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Position pos)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (pos.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is Required !");
                return View();
            }
            if(pos.ImageFile.ContentType!="image/png" && pos.ImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("ImageFile", "Input png or jpg/jpeg");
                return View();
            }
            pos.Image = FileManager.Save(_env.WebRootPath, "uploads/Positions", pos.ImageFile);
            _context.Positions.Add(pos);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int id)
        {
            var existsPos = _context.Positions.FirstOrDefault(x => x.Id == id);
            if (existsPos == null)
            {
                return RedirectToAction("Error", "dashBoard");
            }
            return View(existsPos);
        }
        [HttpPost]
        public IActionResult Edit(Position pos)
        {
            var existsPos = _context.Positions.FirstOrDefault(x => x.Id == pos.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (pos.ImageFile.ContentType != "image/png" && pos.ImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("ImageFile", "Input png or jpg/jpeg");
                return View();
            }
            if (pos.ImageFile != null)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/positions", existsPos.Image);
                existsPos.Image = FileManager.Save(_env.WebRootPath, "uploads/positions", pos.ImageFile);
            }
            existsPos.Name = pos.Name;
            existsPos.Desc = pos.Desc;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            var existsPos = _context.Positions.FirstOrDefault(x => x.Id == id);
            if (existsPos==null)
            {
                return RedirectToAction("Error", "dashBoard");
            }
            return View(existsPos);
        }
        [HttpPost]
        public IActionResult Delete(Position pos)
        {
            var existsPos = _context.Positions.FirstOrDefault(x => x.Id == pos.Id);
            if (existsPos == null)
            {
                return RedirectToAction("Error", "dashBoard");
            }
            if(existsPos.Image!=null)
                FileManager.Delete(_env.WebRootPath, "uploads/positions", existsPos.Image);
            _context.Positions.Remove(existsPos);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
