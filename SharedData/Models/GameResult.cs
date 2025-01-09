using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Models
{
    public class GameResult
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public int KillScore { get; set; }
        public int ClearScore { get; set; }
        public float PlayTime { get; set; }
        public DateTime Date { get; set; }
    }
}
