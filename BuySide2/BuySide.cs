using System;
using System.Collections.Generic;

namespace Gateway.Model
{
	internal class BuySide
    {
	    private readonly int expectedStepCount;
	    private readonly List<object> steps = new List<object>();

	    public BuySide(IBuySideState state)
	    {
		    Id = state.Id;
		    expectedStepCount = state.ExpectedCount;
	    }

	    public SellSide SellSide { get; private set; }

	    private bool IsCompleted => steps.Count == expectedStepCount;

		public int Id { get; }

	    public void Add(object step)
	    {
		    if (IsCompleted)
			    throw new InvalidOperationException();

			steps.Add(step);
			if (IsCompleted)
				SellSide = SellSide.CreateNew(Id);
	    }

	    public void Cancel()
		{
			if (SellSide == null)
				throw new InvalidOperationException();
			SellSide.Cancel();
		}
    }
}
