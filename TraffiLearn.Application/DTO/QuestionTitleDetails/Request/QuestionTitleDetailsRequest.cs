namespace TraffiLearn.Application.DTO.QuestionTitleDetails.Request
{
    public sealed record QuestionTitleDetailsRequest(
        int? TicketNumber,
        int? QuestionNumber);
}
