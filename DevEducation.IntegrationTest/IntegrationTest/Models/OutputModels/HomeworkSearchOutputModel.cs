﻿using System.Collections.Generic;

namespace IntegrationTest.Models.OutputModels
{
    public class HomeworkSearchOutputModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string DeadlineDate { get; set; }
        public bool IsOptional { get; set; }
        public int GroupId { get; set; }
        public List<ThemeOutputModel> Themes { get; set; }
        public List<TagOutputModel> Tags { get; set; }
    }
}
