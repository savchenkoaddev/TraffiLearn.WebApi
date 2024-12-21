using MediatR;
using Newtonsoft.Json;
using Stripe.Checkout;
using TraffiLearn.Application.Abstractions.Payments;
using TraffiLearn.Application.Webhooks.Stripe.Events.ChangeSubscriptionCompleted;
using TraffiLearn.Application.Webhooks.Stripe.Events.RenewSubscriptionCompleted;
using TraffiLearn.Application.Webhooks.Stripe.Extensions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Webhooks.Stripe.Commands
{
    internal sealed class HandleStripeWebhookCommandHandler
        : IRequestHandler<HandleStripeWebhookCommand, Result>
    {
        private readonly IStripeWebhookService _stripeWebhookService;
        private readonly IPublisher _publisher;

        public HandleStripeWebhookCommandHandler(
            IStripeWebhookService stripeWebhookService,
            IPublisher publisher)
        {
            _stripeWebhookService = stripeWebhookService;
            _publisher = publisher;
        }

        public async Task<Result> Handle(
            HandleStripeWebhookCommand request,
            CancellationToken cancellationToken)
        {
            var stripeEvent = _stripeWebhookService.ValidateStripeEvent(
                json: request.Payload,
                signatureHeader: request.Signature);

            var eventType = stripeEvent.GetStripeEventType();

            if (eventType == StripeEventType.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;

                if (session is null)
                {
                    throw new InvalidOperationException(
                        "Session is null in Stripe webhook event");
                }

                INotification? notification;

                notification = GetNotification(session);

                await _publisher.Publish(notification, cancellationToken);
            }

            return Result.Success();
        }

        private static INotification? GetNotification(Session session)
        {
            var action = ExtractAction(session);

            return action switch
            {
                CheckoutSessionAction.ChangePlan => GetChangeSubscriptionCompletedEvent(session),
                CheckoutSessionAction.RenewPlan => GetRenewSubscriptionCompletedEvent(session),

                _ => null
            };
        }

        private static CheckoutSessionAction ExtractAction(Session session)
        {
            var actionString = session.Metadata["action"];

            if (Enum.TryParse<CheckoutSessionAction>(actionString, out var action))
            {
                return action;
            }

            throw new InvalidOperationException(
                $"Action from the metadata is not of type {nameof(CheckoutSessionAction)}.");
        }

        private static ChangeSubscriptionCompletedEvent? GetChangeSubscriptionCompletedEvent(
            Session session)
        {
            var ids = ExtractIds(session, "subscriptionPlanId", "userId");

            var subscriptionPlanId = ids["subscriptionPlanId"];
            var userId = ids["userId"];

            var metadata = JsonConvert.SerializeObject(session.Metadata);

            return new ChangeSubscriptionCompletedEvent(
                SubscriptionPlanId: subscriptionPlanId,
                UserId: userId,
                Metadata: metadata);
        }

        private static RenewSubscriptionCompletedEvent? GetRenewSubscriptionCompletedEvent(
            Session session)
        {
            var ids = ExtractIds(session, "userId");

            var userId = ids["userId"];

            var metadata = JsonConvert.SerializeObject(session.Metadata);

            return new RenewSubscriptionCompletedEvent(
                UserId: userId,
                Metadata: metadata);
        }

        private static Dictionary<string, Guid> ExtractIds(
            Session session, params string[] idNames)
        {
            var result = new Dictionary<string, Guid>();

            foreach (var idName in idNames)
            {
                if (!Guid.TryParse(session.Metadata[idName], out var id))
                {
                    throw new InvalidOperationException(
                        "Ids from the metadata are not of type Guid.");
                }

                result.Add(idName, id);
            }

            return result;
        }
    }
}
