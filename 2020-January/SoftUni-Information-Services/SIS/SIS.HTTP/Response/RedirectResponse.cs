namespace SIS.HTTP.Response
{
    using Enums;
    using Models;

    public class RedirectResponse : HttpResponse
    {
        /// <summary>
        /// Create RedirectResponse to allow direct redirections from one page to another by the browser. This ensures that for example when a form is submitted, user would be redirected to a safe page, where refreshing wouldn't re-send the form.d
        /// </summary>
        /// <param name="newLocation">Location to which client should be redirected. By default it's homepage</param>
        /// <param name="shouldPreserveRequestType">Based on this parameter the response code would be either TemporaryRedirect or Found. Found is always GET.</param>
        public RedirectResponse(string newLocation = "/", bool shouldPreserveRequestType = false)
        {
            Headers.Add(new Header("Location", newLocation));
            StatusCode = shouldPreserveRequestType ? HttpResponseCode.TemporaryRedirect : HttpResponseCode.Found;
        }
    }
}