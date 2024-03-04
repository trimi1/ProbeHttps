using NSubstitute;
using ProbeHttps.Https.Handler;
using ProbeHttps.Interface;

namespace Tests
{
    [TestFixture]
    public class UnicastHandlerTests
    {
        private IAnalyzer _mockAnalyzer;
        private IBuilder _mockBuilder;
        private UnicastHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockAnalyzer = Substitute.For<IAnalyzer>();
            _mockBuilder = Substitute.For<IBuilder>();
            _handler = new UnicastHandler(8080);
        }

        [Test]
        public async Task AnalyzeAndRespond_CallsAnalyzerAndBuilder_WhenMessageIsValid()
        {
            var message = "valid message";
            var mockStreamWriter = new StreamWriter(Stream.Null);
            _mockAnalyzer.Analyze(message).Returns(1);
            _mockBuilder.BuildStatusMessage(Arg.Any<string>(), Arg.Any<string>()).Returns("response");

            await _handler.AnalyzeAndRespond(message, mockStreamWriter);

            _mockAnalyzer.Received(1).Analyze(message);
            _mockBuilder.Received(1).BuildStatusMessage(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}