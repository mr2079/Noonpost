using Microsoft.AspNetCore.Mvc.Filters;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using IAuthorizationFilter = Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter;

namespace Infrastructure.Security;

public class PermissionCheckerAttribute
{
    //public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    //{
    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {

    //    }
    //}
}
