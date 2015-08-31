# REST uwp

REST webserver implementation for universal windows platform (UWP) apps.

Mostly following the guidelines from:

https://github.com/tfredrich/RestApiTutorial.com

# Intro

When the raspberry pi 2 was released, all windows 10 users were filled with joy when Microsoft announced the support of windows 10 for this neat device. After a couple of beta builds, we got the RTM version a couple of weeks ago. A crucial piece for this platform is missing, WCF. It might be supported in the future ([see post](https://social.msdn.microsoft.com/Forums/en-US/f462d578-368b-4218-b57e-19cd8852fd0c/wcf-hosting-in-windows-iot?forum=WindowsIoT)), but untill then I would need some simple REST implementation to keep my projects going. I decided to implement a simple HTTP REST service.

# Usage

```cs
[UriFormat("/users/{userId}")] 
public PostResponse CreateUser(int userId, [FromBody] User user) 
{
  return new PostResponse(PostResponse.ResponseStatus.Created, $"/users/{userId}"); 
} 
'''
