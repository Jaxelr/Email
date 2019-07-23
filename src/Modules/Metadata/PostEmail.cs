using System;
using Carter.OpenApi;
using EmailService.Entities.Operations;

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
                Description = $"A response if it was succesful and a reason why it wasnt.",
                Response = typeof(PostEmailResponse)
            }
        };

        public override Type Request => typeof(PostEmailRequest);

        public override string Description => DescriptionInfo;

        public override string Tag => TagInfo;

        public override string OperationId => nameof(PostEmail);
    }
}
