using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Sienar.Extensions;
using Sienar.Services;

namespace Sienar.Pages;

public abstract class UpsertPage<TModel> : FormPage<TModel>
	where TModel : new()
{
	[Parameter]
	public Guid? Id { get; set; }

	[Inject]
	protected IEntityReader<TModel> Reader { get; set; } = default!;

	[Inject]
	protected IEntityWriter<TModel> Writer { get; set; } = default!;

	protected abstract string Name { get; }
	protected bool IsEditing => Id.HasValue;
	protected string Title => IsEditing
		? $"Edit {Name}"
		: $"Add {typeof(TModel).GetEntityName()}";
	
	protected string SubmitText => IsEditing
		? $"Update {Name}"
		: $"Add {typeof(TModel).GetEntityName()}";

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		if (IsEditing)
		{
			await SubmitRequest(
				async () => Model = await Reader.Read(Id!.Value) ?? new());
		}
	}

	protected override async Task OnSubmit()
	{
		if (IsEditing)
		{
			await SubmitRequest(() => Writer.Update(Model));
		}
		else
		{
			await SubmitRequest(() => Writer.Create(Model));
		}

		if (WasSuccessful)
		{
			await OnSuccess();
		}
	}

	protected abstract Task OnSuccess();
}