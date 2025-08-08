using System;
using System.Collections.Generic;
namespace BuidIt.Domain;

public enum ResourceType { Raw, Intermediate, Final }

public sealed record ResourceId(string Value);

public sealed class ResourceDef
{
	public ResourceId Id { get; }
	public string Name { get; }
	public ResourceType Type { get; }

	public ResourceDef(ResourceId id, string name, ResourceType type)
		=> (Id, Name, Type) = (id, name, type);
}
