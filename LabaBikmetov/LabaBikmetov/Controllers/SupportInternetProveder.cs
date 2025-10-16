using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LabaBikmetov.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupportInternetProveder : ControllerBase
    {

        private readonly ILogger<SupportInternetProveder> _logger;

        public SupportInternetProveder(ILogger<SupportInternetProveder> logger)
        {
            _logger = logger;
        }

        
        [HttpGet("GetAllUsers")]
        public List<string> GetAllUsers(string AdminUser, string pass) {
            List<string> error = new List<string>();    
            if (System.IO.File.Exists("user.csv"))
            {
                var bd = System.IO.File.ReadAllLines("user.csv").ToList();
                var line = bd.Where(x => x.Contains(AdminUser) && x.Contains(pass) && x.Contains("ADMIN")).FirstOrDefault();
                if (line == null)
                {
                    error.Add("Херня");
                    return error;
                }
                return bd;
            }
            else
            {
                error.Add("Херня");
                return error;
            }
                
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(string login, string pass)
        {
            
            if (System.IO.File.Exists("user.csv"))
            {
                int i = System.IO.File.ReadAllText("user.csv").Length;
                System.IO.File.AppendAllText("user.csv", string.Join(";", i, login,pass,""));
            }
            else
                return BadRequest("БД не найдена");
            return Ok();
        }

        [HttpDelete("DeleteUserRequest")]
        public IActionResult DeleteUserRequest(Guid guidRequest)
        {
            if (System.IO.File.Exists("bd.csv"))
            {
                var bd = System.IO.File.ReadAllLines("bd.csv").ToList();
                if (bd.Count > 0)
                {
                    string? line = bd.Where(x => x.Contains(guidRequest.ToString())).FirstOrDefault();
                    if (line == null)
                        return BadRequest("Заявка не найдена");
                    int id = bd.IndexOf(line);
                    bd.RemoveAt(id);
                    System.IO.File.WriteAllLines("bd.csv", bd);
                }
                else
                {
                    return BadRequest("БД пустая");
                }

            }
            else
                return BadRequest("БД не найдена");
            return Ok();
        }

        [HttpPost("CreateUserRequest")]
        public IActionResult CreateUserRequest(int UserID, string? comment)
        {
            if (System.IO.File.Exists("bd.csv"))
            {
                System.IO.File.AppendAllText("bd.csv", new DataDTO(UserID, comment).ToString());
            }
            else
                return BadRequest("БД не найдена");
            return Ok();
        }

        [HttpPut("ChangeCommentUserRequest")]
        public IActionResult ChangeCommentUserRequest(Guid guidRequest, string? comment)
        {
            if (System.IO.File.Exists("bd.csv"))
            {
                var bd = System.IO.File.ReadAllLines("bd.csv").ToList();
                if (bd.Count > 0)
                {
                    string? line = bd.Where(x => x.Contains(guidRequest.ToString())).FirstOrDefault();
                    int id = bd.IndexOf(line);
                    if (line == null)
                        return BadRequest("Заявка не найдена");
                    string[] data = line.Split(';');
                    data[3] = comment;
                    string newLine = string.Join(";", data);
                    bd.Add(newLine);
                    System.IO.File.WriteAllLines("bd.csv", bd);

                }
                else
                    return BadRequest("БД пустая не возможно изменить комментарий для записи");


            }
            else
                return BadRequest("БД не найдена");
            return Ok();
        }

        
        /// <summary>
        /// Получение заявки по id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetUserRequest")]
        public List<string> GetUserRequest(int userId)
        {
            List<string> error = new List<string>();
            if (System.IO.File.Exists("bd.csv"))
            {
                var bd = System.IO.File.ReadAllLines("bd.csv");
                if (bd.Length > 0)
                {
                    List<string> line = bd.Where(x => x.Contains(Convert.ToString(userId))).ToList();
                    if (line != null)
                    {
                        return line;
                    }
                    else
                    {
                        error.Add("Не найдена заявка");
                        return error;
                    }
                }
                error.Add("База данных пустая");
                return error;
            }
            else {
                error.Add("База данных не найдена");
                return error; 
            }
        }
    }
}
