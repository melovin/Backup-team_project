using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace test_nuget
{
	public class HelloJob : IJob
	{
		public Task Execute(IJobExecutionContext context)
		{
            Console.WriteLine("Ahoj");
			return Task.CompletedTask;
		}
	}
}
