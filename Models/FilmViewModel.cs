using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Film.Models
{
    public class FilmViewModel//editing film
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле назва не може бути пустим")]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле анотація не може бути пустим")]
        [Display(Name = "Анотація")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле режисер не може бути пустим")]
        [Display(Name = "Режисер")]
        public string Director { get; set; }

        [Required(ErrorMessage = "Поле сценарист не може бути пустим")]
        [Display(Name = "Сценарист")]
        public string Scenario { get; set; }

        [Required(ErrorMessage = "Поле тип сценарію не може бути пустим")]
        [Display(Name = "Тип сценарію")]
        public string ScenarioType { get; set; }
        
        [DataType(DataType.Upload)]
        [Display(Name = "Зображення")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Додайте жанри")]
        [Display(Name = "Жанри")]
        public int[] CategoryId { get; set; }
    }
}