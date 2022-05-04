using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Seregich.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Seregich.Controllers
{

    public class Book
    {
        public int bookNumber;
        public string Author;
        public int year;
        public string Category;
    }

    public class Library
    {
        public List<Book> Books;

        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        public Book FindBookByBookNumber(int number)
        {
            return Books.Where(i => i.bookNumber == number).FirstOrDefault();
        }

        public void DeleteBookByBookNumber(int number)
        {
            var book = Books.Where(i => i.bookNumber == number).FirstOrDefault();
            if (book != null)
            {
                Books.Remove(book);
            }
        }
    }

   
    public static class Lbr
    {
        public static Library library = new Library() { Books = new List<Book>() { new Book() { Author = "Евссев", bookNumber = 1, Category = "Романтика", year = 2002 } } };
    }


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult FirstTask()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SecondTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SecondTask(string inputText)
        {
            try
            {
                string res = "";

                foreach (char chr in inputText)
                {
                    if (chr == ' ')
                    {
                        res += '_';
                        continue;
                    }
                    res += chr;
                }

                ViewBag.Result = "Результат:" + res;
            }
            catch
            {
                ViewBag.Result = "Ошибка!";
            }
            return View();
        }


        ///////////////////////////////////////
        [HttpGet]
        public IActionResult ThirdTask()
        {
            return View(Lbr.library.Books);
        }

        [HttpPost]
        public IActionResult ThirdTask(string inputText, string type, string bNumber, string bAuthor, string bYear, string bCategory)
        {
            if (type == "Search")
            {
                try
                {
                    Book book = Lbr.library.FindBookByBookNumber(Convert.ToInt32(inputText));
                    ViewBag.Result = $"Результат: Автор: {book.Author}, Год: {book.year}";
                }
                catch
                {
                    ViewBag.Result = $"Не найдено!";
                }
            }
            else if (type == "Delete")
            {
                try
                {
                    Lbr.library.DeleteBookByBookNumber(Convert.ToInt32(inputText));
                    ViewBag.Result = $"Результат: Книга была удалена!";
                }
                catch
                {
                    ViewBag.Result = $"Произошла ошибка!";
                }
            }

            else if (type == "Create")
            {
                try
                {
                    Book book = new Book() { Author = bAuthor, Category = bCategory, bookNumber = Convert.ToInt32(bNumber), year = Convert.ToInt32(bYear) };
                    Lbr.library.AddBook(book);
                    ViewBag.Result = $"Результат: Книга была создана!";
                }
                catch
                {
                    ViewBag.Result = $"Произошла ошибка при создании!";
                }
            }
            return View(Lbr.library.Books);
        }
    }
}
