using System;
using System.ComponentModel.DataAnnotations;

namespace FinApi.Requests
{
    public class PortfolioRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
