using System;
using System.Collections.Generic;
namespace BuidIt.Domain;

public sealed class BuildStage
{
	public string Name { get; }
	public Dictionary<ResourceId, int> Requirements { get; }
	public double DurationSec { get; }

	public BuildStage(string name, Dictionary<ResourceId,int> reqs, double durationSec)
		=> (Name, Requirements, DurationSec) = (name, reqs, durationSec);
}
