using MI.PIMS.UI.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Middlewares
{
    //public class PermissionAuthorizationPolicyProvider
    //    : DefaultAuthorizationPolicyProvider
    //{
    //    public PermissionAuthorizationPolicyProvider(
    //        IOptions<AuthorizationOptions> options)
    //        : base(options)
    //    {

    //    }

    //    public override async Task<AuthorizationPolicy?> GetPolicyAsync(
    //        string policyName)
    //    {
    //        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

    //        if (policy is not null)
    //        {
    //            return policy;
    //        }

    //        return new AuthorizationPolicyBuilder()
    //            .AddRequirements(new AccessGlobalGroupRequirement(policyName))
    //            .Build();
    //    }
    //}
}
