using CrudProject.Data;
using CrudProject.Models;
using CrudProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudProject.Controllers
{
    //[Authorize]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.OrderByDescending(e => e.StudentId).ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["Course"] = _context.CourseManagement.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student model)
        {
            // Check for existing User
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingStudent != null)
            {
                ModelState.AddModelError("", "Email already exists");
            }

            
            if (!Validation.IsValidEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid Email");
            }

            if (!Validation.IsValidPhone(model.Phone))
            {
                ModelState.AddModelError("Phone", "Invalid Phone Number");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Course"] = _context.CourseManagement.ToList();
                return View(model);
            }

            // Find selected course
            var course = await _context.CourseManagement
                .FirstOrDefaultAsync(k => k.CourseId == model.CourseId);

            if (course == null)
            {
                ModelState.AddModelError("CourseId", "Invalid Course Selected");
                ViewData["Course"] = _context.CourseManagement.ToList();
                return View(model);
            }

            model.Course = course.CourseName;
            model.Fees = course.Fees;

            var newStudent = new Student
            {
                Name = model.Name,
                Email = model.Email,
                Course = model.Course,
                CourseId= model.CourseId,
                Fees = model.Fees,
                EnrollmentDate = model.EnrollmentDate,
                Phone = model.Phone,
                Address = model.Address
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["Course"] = _context.CourseManagement.ToList();

            return View(student);

        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (!Validation.IsValidEmail(student.Email))
            {
                ModelState.AddModelError("Email", "Invalid Email Format");
            }
            
            if (!Validation.IsValidPhone(student.Phone))
            {
                ModelState.AddModelError("Phone", "Invalid Phone Number");
            }
           
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Course"] = _context.CourseManagement.ToList();

            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }

       ////AddCourse
       // //GET
       // public async Task<IActionResult> AddCourse(int? id)
       // {
       //     return View();
       // }

       // //Post
       // [HttpPost]
       // public async Task<IActionResult> AddCourse( CourseManagement model)
       // {
       //     var course = await _context.Students.FindAsync();
       //     if(course != null)
       //     {
       //         ModelState.AddModelError("Course", "Course already exist");
       //         return View();
       //     }
            
       //     //add course

       //     CourseManagement Addcourse = new CourseManagement
       //     {
       //         CourseName = model.CourseName,
       //         Fees = model.Fees,
       //         Semester = model.Semester
       //     };

       //     _context.CourseManagement.Add(Addcourse);
       //     await _context.SaveChangesAsync();
       //     return RedirectToAction("Index");

       // }


        [HttpGet]
        public IActionResult GetCourseFees(int courseId)
        {
            var course = _context.CourseManagement
                                 .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return Json(0);

            return Json(course.Fees);
        }


        public class Validation
        {
            public static bool IsValidEmail(string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return false;
                }
                for (int i = 0; i < email.Length; i++)
                {
                    if (email[i] == ' ')
                        return false;                                               
                }

                int atCnt = 0;
                int atIndex = -1;

                for (int i = 0; i < email.Length; i++)
                {
                    if (email[i] == '@')
                    {
                        atCnt++;
                        atIndex = i;
                    }
                }

                if (atCnt != 1)
                    return false;                                                 

                if (atIndex == 0 || atIndex == email.Length - 1)
                    return false;                                                  

                bool dotFound = false;

                for (int i = atIndex + 1; i < email.Length; i++)
                {
                    if (email[i] == '.')
                    {
                        dotFound = true;
                        break;
                    }
                }

                if (!dotFound)
                    return false;                                                  

                if (email[atIndex + 1] == '.' || email[email.Length - 1] == '.')
                    return false;

                if (!char.IsLetterOrDigit(email[0]))
                    return false;                                                 

                for (int i = 0; i < email.Length; i++)
                {
                    char c = email[i];

                    if (!(char.IsLetterOrDigit(c) || c == '@' || c == '.' || c == '_' || c == '-'))
                    {
                        return false;                                             
                    }
                }

                int dotCount = 0;

                for (int i = 0; i < email.Length; i++)
                {
                    if (email[i] == '.')
                    {
                        dotCount++;
                    }
                }

                if (dotCount >= 3)
                    return false;                                                 


                return true;                                                      


            }

            

            public static bool IsValidPhone(string phone)
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return false;

                if (phone.Length != 10)
                    return false;

                for (int i = 0; i < phone.Length; i++)
                {
                    if (!char.IsDigit(phone[i]))
                        return false;
                }

                return true;
            }
        }
        
    }
}

