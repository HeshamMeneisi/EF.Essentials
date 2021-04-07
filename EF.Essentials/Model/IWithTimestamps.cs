using System;

namespace EF.Essentials.Model
{
    public interface IWithTimestamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
