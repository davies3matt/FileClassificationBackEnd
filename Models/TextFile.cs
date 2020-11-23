using System;

namespace c_sharp_grad_backend.Models
{
    public class TextFile
    {
        public int Id { get; set; }
        public byte[] AvatarOne { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EthnicGroup { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string CellNumber { get; set; }
        public string IDNumber { get; set; }
    }
}
