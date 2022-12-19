using System.ComponentModel.DataAnnotations;

namespace CakesByRAndRCore.Models
{
    public class TestModels
    {
    }

    public class StudentModel
    {
        [Required]
        public string Name
        {
            get;
            set;
        }
        [Required]
        public string Email
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }

        public class Result
        {
            public bool Success
            {
                get;
                set;
            }
            public string Data
            {
                get;
                set;
            }


        }
    }
}
