using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_Website.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Du måste skriva ett Namn")]
        [StringLength(30, ErrorMessage ="Avsändar namnet får max vara 30 tecken")]
        public string SenderName { get; set; }

        [Required(ErrorMessage ="Meddelandet måste innehålla något")]
        public string MessageText { get; set; }
        public bool Read {  get; set; }

        public int? SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        [ValidateNever]
        public virtual User Sender { get; set; }

        public int ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        [ValidateNever]
        public virtual User Receiver { get; set; }
    }
}
