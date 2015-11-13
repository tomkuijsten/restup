# REST uwp


[![Build status](https://ci.appveyor.com/api/projects/status/1aj7614fb0o1bjdy?svg=true)](https://ci.appveyor.com/project/tomkuijsten/restup)
(failing due to problems on appveyor with universal apps)

[Download NuGet package](https://www.nuget.org/packages/Restup/1.0.0-alpha1)

REST webserver implementation for universal windows platform (UWP) apps.

Using guidelines from:

https://github.com/tfredrich/RestApiTutorial.com

More sample controllers can be found in the [WebServerHostTest](https://github.com/tomkuijsten/restup/tree/master/src/WebServerHostTest) project:

# Intro

When the raspberry pi 2 was released, all windows 10 users were filled with joy when Microsoft announced the support of windows 10 for this neat device. After a couple of beta builds, we got the RTM version a couple of weeks ago. A crucial piece for this platform is missing, WCF. It might be supported in the future ([see post](https://social.msdn.microsoft.com/Forums/en-US/f462d578-368b-4218-b57e-19cd8852fd0c/wcf-hosting-in-windows-iot?forum=WindowsIoT)), but untill then I would need some simple REST implementation to keep my projects going. I decided to implement a simple HTTP REST service.

# Quick tutorial

We're all coders, so I'll explain in a way a coder understands best... sample code :)

```cs
using Devkoes.Restup.WebServer;

private async Task InitializeWebServer()
{
    RestWebServer webserver = new RestWebServer(80); //defaults to 8800
    webserver.RegisterController<ParametersController>();

    await webserver.StartServerAsync();
}

```
```cs

[RestController(InstanceCreationType.Singleton)]
public class ParametersController
{
  [UriFormat("/parameters/{parameterId}")] 
  public PostResponse CreateUser(int parameterId, [FromBody] string parameterValue) 
  {
    return new PostResponse(PostResponse.ResponseStatus.Created, $"/users/{userId}"); 
  }
}
```

# More
## Controller creation types
You can choose to have one instance of your rest controller for the whole application cycle, or one per call. This is controller by the RestController class attribute.

    [RestController(InstanceCreationType.Singleton)]
    [RestController(InstanceCreationType.PerCall)]

## REST verb
The return type of your method is used to determine the verb of the REST request it will respond to. There are four (duh!) available verb types:
* GetResponse (get)
* PostResponse (create)
* PutResponse (update)
* DeleteResponse (delete)

## Url matching
You can use the `UriFormatAttribute` on your method to define the uri you want to match on. All strings between `{` and `}` will be handled as input parameter and should have a corresponding method parameter.

## FromBody
You can use the `FromBodyAttribute` on a method parameter. Restup will deserialize the http body  and use that as value for the parameter.

*Note: there can only be one `FromBodyAttribute` per method and it should always be the last parameter.*

## Methods used for REST request
All public methods in the controller which have the `UriFormatAttribute` and one of the verb responses as return type will be indexed as REST method.

## Serializing/deserializing
There are two content types supported: xml and json. The .NET internal xml serializer is used for XML. For Json I've used the incredible lib from Newtonsoft. By default all your types are serializable by both serializers. Just createa class/struct with properties and it will be serialized correctly.
### Http request
If your http request has a body, the "Content-Type" header is used to determine the serializer.
### Http response
If your controller method returns a GetResponse, the http request header "Accept" is used to determine the serializer.

*Note: all serializers default to Json if headers are missing.*
