using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Topics.Update
{
    public sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTopicCommandHandler(
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId);

            if (topic is null)
            {
                throw new ArgumentException("Topic has not been found");
            }

            topic.Update(
                number: TopicNumber.Create(request.TopicNumber),
                title: TopicTitle.Create(request.Title));

            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
