using System;

namespace GenericCompany.Common.Model
{
    public interface IWithTimestamps
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
