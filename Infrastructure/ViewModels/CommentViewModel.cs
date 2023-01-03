using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModels;

public class CreateCommentViewModel
{
    public Guid ArticleId { get; set; }
    public Guid UserId { get; set; }
    //public Guid? ParentId { get; set; }
    [Display(Name = "متن دیدگاه")]
    [Required(ErrorMessage = "فیلد {0} الزامی می باشد")]
    public string CommentText { get; set; } = string.Empty;
}
