using AutoMapper;
using de_exceptional_closures.Controllers;
using MediatR;
using Moq;

namespace de_exceptional_closures_unitTests.Controllers
{
    public class ClosureControllerTest
    {
        private readonly ClosureController controller;

        public ClosureControllerTest()
        {

            var mockMediator = new Mock<IMediator>();
            var mockMapper = new Mock<IMapper>();

            controller = new ClosureController(mockMediator.Object, mockMapper.Object);
        }
    }
}