using Carter.OpenApi;
using EmailService.Models;
using EmailService.Models.Operations;

namespace EmailService.Modules.Metadata
{
    public class PostEmail : RouteMetaData
    {
        private const string TagInfo = "Email";
        private const string DescriptionInfo = "Post an email message to queue for delivery";

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = "A response if it was successful and a reason why it wasnt.",
                Response = typeof(PostEmailResponse),
            },
            new RouteMetaDataResponse
            {
                Code = 500,
                Description = "A response if an internal server error is detected.",
                Response = typeof(FailedResponse),
            }
        };

        public override RouteMetaDataRequest[] Requests { get; } =
        {
            new RouteMetaDataRequest
            {
                Request = typeof(PostEmailRequest),
            }
        };

        public override string Description => DescriptionInfo;

        public override string Tag => TagInfo;

        public override string OperationId => nameof(PostEmail);
    }
}
