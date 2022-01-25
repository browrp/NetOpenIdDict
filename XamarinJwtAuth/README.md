# XamarinJwtAuth

## Summary
This project is a Xamarin Forms project that allows the user to do two things.
1. Create a User by posting the information to an ASP.Net Core Identity Endpoint
2. Login using the Resource Owner Password Grant (ROPG) Flow.

This is a Xamarin Project that calls into an OpenIdDict/OpenIDConnect endpoint using the Resource Owner Password Grant flow.
There is a lot of "manual" code here such as constructing the calls and handling the responses and parsing out the payload
from a successful and unsuccessful auth.  In addition, there is some code there to create a user by posting to an api endpoint.

This code is basic and can be expanded upon.  For instance, the user can determine if a user was created successfully or
not based on the return data from the Web Application.  The information can be customized therein.  
