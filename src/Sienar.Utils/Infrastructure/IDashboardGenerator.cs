namespace Sienar.Infrastructure;

/// <summary>
/// The <see cref="IAuthorizedLinkAggregator{TLink}"/> used to generate a list of <see cref="DashboardLink">dashboard links</see> the user is authorized to see
/// </summary>
public interface IDashboardGenerator : IAuthorizedLinkAggregator<DashboardLink>;