using System.Threading.Tasks;
using Amazon.Lambda.CloudWatchEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace TestLambdas.Tests
{
    public class RaiseSQSFunctionTests
    {
        [Fact]
        public async Task TestToUpperFunction()
        {
            var function = new TriggerFromEventBridge();
            var context = new TestLambdaContext();
            var result = await function.HandlerEvent(new CloudWatchEvent<InputEvent>
            {
                Detail = new InputEvent
                {
                    Value = "Things"
                }
            }, context);

            Assert.True(result);
        }
    }
}