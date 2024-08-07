#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading.Tasks;
using Sienar.Infrastructure;

namespace Sienar.Hooks;

/// <exclude />
public abstract class UserInRoleAccessValidator<T> : IAccessValidator<T>
{
	private readonly IUserAccessor _userAccessor;
	protected string Role = string.Empty;

	protected UserInRoleAccessValidator(IUserAccessor userAccessor)
	{
		_userAccessor = userAccessor;
	}

	/// <inheritdoc />
	public async Task Validate(
		AccessValidationContext context,
		ActionType actionType,
		T? input)
	{
		if (await _userAccessor.UserInRole(Role))
		{
			context.Approve();
		}
	}
}