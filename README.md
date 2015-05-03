# ChatApp

I put a lot of heart in writing this application and I was enjoying it. Thanks for taking the time to reviewing it. I love writing simple, clean and quality code. I will not go into details here because the code must speak by himself.

Main points:

- Authentication. There are 3 authentication types ( global, per browser tab using query strings and per session using Forms Auth). You can change them in appSettings from web.config.

- Security. Use Forms Authentication and HubUsernameProvider=FromUserIdentity (appSettings)

- Thread Safety. Clients will not miss a message.

- Architecture. Layers: Domain, Application, Presentation, Persistence. 

- Test Driven Development when creating the domain model.

- Support for having many connections for the user. The user leaves the chat only when the last connection close.

- Not Null Code Convention. I explicit mark that something can be null by using the class Maybe. For the other objects I follow the convention that they must not be null.
