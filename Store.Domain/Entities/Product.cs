using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;


namespace Store.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [NotMapped]
        public Boolean Checked { get; set; }

        [Required(ErrorMessage = "Введите название.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Добавьте описание.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Введите цену >0.00.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Укажите категорию.")]
        public string Category { get; set; }
    }
}
