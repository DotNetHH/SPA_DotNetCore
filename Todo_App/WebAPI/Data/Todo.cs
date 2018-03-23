using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public class Todo
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Text { get; set; }
        public DateTimeOffset ErstellZeitpunkt { get; set; }
        public TodoArt Typ { get; set; }
        // public List<Feld> Felder { get; set; }
    }
}
