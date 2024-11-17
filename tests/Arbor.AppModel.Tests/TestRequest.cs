using System;
using MediatR;

namespace Arbor.AppModel.Tests;

public class TestRequest(Guid newGuid) : IRequest<Unit>
{
    public Guid Id { get; set; } = newGuid;

    public override string ToString() => base.ToString() + " " + Id;
}