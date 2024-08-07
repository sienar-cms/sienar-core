namespace Sienar.Infrastructure;

/// <summary>
/// Used to track access requests that should fail by default and require explicit approval
/// </summary>
public class AccessValidationContext
{
	/// <summary>
	/// Indicates whether the access request is approved or not
	/// </summary>
	public bool CanAccess { get; private set; }

	/// <summary>
	/// Informs the context that the access request is approved
	/// </summary>
	public void Approve() => CanAccess = true;
}