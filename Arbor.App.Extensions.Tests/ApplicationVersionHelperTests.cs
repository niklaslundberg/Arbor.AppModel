using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arbor.App.Extensions.Application;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.App.Extensions.Tests
{
    public class ApplicationVersionHelperTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public ApplicationVersionHelperTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        [Fact]
        public void Do()
        {
            var applicationVersionInfo = ApplicationVersionHelper.GetAppVersion();

            _outputHelper.WriteLine(applicationVersionInfo.AssemblyFullName);
        }
    }
}
