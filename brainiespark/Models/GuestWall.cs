using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace brainiespark.Models
{
    public class GuestWall
    {
        public GuestWall()
        {
            Name = "Guest";
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}