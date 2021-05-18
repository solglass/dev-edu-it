using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTest.Models.InputModels
{
    public class OrderedThemeInputModel
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }
}
