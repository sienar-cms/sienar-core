using System;
using System.Threading.Tasks;
using Sienar.Data;

namespace Sienar.Pages;

public abstract class FormPage<TModel> : ActionPage<TModel>
	where TModel : new()
{
	protected readonly DateTime FormStarted = DateTime.Now;

	protected abstract Task OnSubmit();

	protected void Reset()
	{
		Model = new TModel();
	}

	protected void SetFormCompletionTime(Honeypot honeypot)
	{
		honeypot.TimeToComplete = DateTime.Now - FormStarted;
	}
}