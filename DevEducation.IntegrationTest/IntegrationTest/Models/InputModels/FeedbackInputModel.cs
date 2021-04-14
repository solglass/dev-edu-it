using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTest.Models.InputModels
{
    public class FeedbackInputModel
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public int UnderstandingLevelId { get; set; }
        
    }
}
