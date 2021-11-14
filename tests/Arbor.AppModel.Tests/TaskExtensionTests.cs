using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Tasks;
using FluentAssertions;
using Xunit;

namespace Arbor.AppModel.Tests
{
    public class TaskExtensionTests
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly List<int> _order = new();

        public TaskExtensionTests() => _cancellationTokenSource = new CancellationTokenSource();

        [Fact]
        public async Task AwaitToken()
        {
            _order.Add(0);
            _ = Task.Run(Wait);
            _order.Add(1);

            await _cancellationTokenSource.Token;
            _order.Add(3);

            await _cancellationTokenSource.Token;
            _order.Add(4);

            _order.Should().BeInAscendingOrder();

            _cancellationTokenSource.Token.IsCancellationRequested.Should().BeTrue();
        }

        private async Task Wait()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            _order.Add(2);

            _cancellationTokenSource.Cancel();
        }
    }
}