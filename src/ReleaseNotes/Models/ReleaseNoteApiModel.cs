﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes.Models
{
    public class ReleaseNoteApiModel
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
