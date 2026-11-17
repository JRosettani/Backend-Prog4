using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Backend_Prog4.Utils.Filters;


public class AuthOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var Attributes = context.ApiDescription.CustomAttributes();

        var IsAuthorize = Attributes.Any(attr => attr.GetType() == typeof(AuthorizeAttribute));
        var IsAllowAnonymous = Attributes.Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute)); ;

        if (!IsAuthorize || IsAllowAnonymous) return;

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "token"
                        }
                    },
                    Array.Empty<string>()
                }
            }
        };
    }
}