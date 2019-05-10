using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Octacom.Odiss.Core.Identity.Validators
{
    public class OdissPasswordValidator : PasswordValidator, IIdentityValidator<string>
    {
        public int MaximumPasswordLength { get; set; } = int.MaxValue;
        public int MinimumPasswordStrength { get; set; }

        public override Task<IdentityResult> ValidateAsync(string item)
        {
            if (item.Length > MaximumPasswordLength)
            {
                var result = new IdentityResult("Password exceeds the maximum length that is allowed");
                return Task.FromResult(result);
            }

            // TODO - Write logic to validate MinimumPasswordStrength (if the begin using this feature)

            return base.ValidateAsync(item);
        }
    }
}
