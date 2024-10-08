using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Converters
{
    public class AnswerJsonConverter : JsonConverter<Answer>
    {
        public override void WriteJson(
            JsonWriter writer,
            Answer value,
            JsonSerializer serializer)
        {
            var jsonObject = new JObject
            {
                { nameof(Answer.Text), value.Text },
                { nameof(Answer.IsCorrect), value.IsCorrect }
            };
            jsonObject.WriteTo(writer);
        }

        public override Answer ReadJson(
            JsonReader reader,
            Type objectType,
            Answer existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var text = jsonObject[nameof(Answer.Text)]?.ToString();

            var isCorrect = jsonObject[nameof(Answer.IsCorrect)]?.ToObject<bool>() ?? false;

            return Answer.Create(
                text, isCorrect).Value;
        }
    }
}
