using System;
using System.Collections.Generic;
namespace BuidIt.Domain;

public sealed class Recipe
{
	public ResourceId Output { get; }
	public Dictionary<ResourceId, int> Inputs { get; }
	public double DurationSec { get; }
	public string? RequiredMachine { get; }

	public Recipe(ResourceId output, Dictionary<ResourceId,int> inputs, double durationSec, string? requiredMachine = null)
		=> (Output, Inputs, DurationSec, RequiredMachine) = (output, inputs, durationSec, requiredMachine);
}
