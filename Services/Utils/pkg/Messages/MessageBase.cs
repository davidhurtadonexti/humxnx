using pkg.Interfaces;

namespace pkg.Messages
{
	public class MessageBase : IMessages
    {
        (int Code, string Message)[] errorMessages = {
                (204, "The server has successfully fulfilled the request and that there is no additional content to send in the response payload body."),
                (400, "The server cannot process the request due to malformed request sintax, invalid request."),
                (401, "The request has not been applied because it lacks valid authentication credentials for the target resource"),
                (404, "The server not found the target resource"),
                (409, "The request could not be completed due to a conflict with the current state of the resource."),
                (500, "The server encountered an unexpected condition that prevented it from fulfilling the request."),
                (502, "The server, while acting as a gateway or proxy, received an invalid response from an inbound server it accessed while attempting to fulfill the request."),
                (503, "The server is currently unable to handle the request due to a temporary overload or scheduled maintenance, which will likely be alleviated after some delay.")
        };

        public string GetHTTPStatusMessage(int code)
		{
            if (code >= 0 && code < errorMessages.Length)
            {
                return errorMessages[code].Message;
            }
            else
            {
                return null;
            }
        }
	}
}

