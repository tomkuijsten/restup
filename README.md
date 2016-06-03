# Webservice for Universal Windows Apps

[![Build status](https://ci.appveyor.com/api/projects/status/1aj7614fb0o1bjdy?svg=true)](https://ci.appveyor.com/project/tomkuijsten/restup) [![NuGet downloads](https://img.shields.io/nuget/vpre/restup.svg)](https://www.nuget.org/packages/Restup/)

### Goal
Provide a HTTP server supporting html and REST in order to keep Windows IoT projects going. Let's hope there will be a native solution soon, although there aren't any signs for that yet (if you have one, let me [know](https://github.com/tomkuijsten/restup/issues/new)).

### How this started

When the raspberry pi 2 was released, all windows 10 users were filled with joy when Microsoft announced the support of windows 10 for this neat device. After a couple of beta builds, we got the RTM version a couple of weeks ago. A crucial piece for this platform is missing, WCF. It might be supported in the future ([see post](https://social.msdn.microsoft.com/Forums/en-US/f462d578-368b-4218-b57e-19cd8852fd0c/wcf-hosting-in-windows-iot?forum=WindowsIoT)), but until then I would need some simple webservice implementation to keep my projects going.

The first couple of alpha and beta versions supported hosting REST controllers only, but since beta2 static files are supported as well. This also introduced a way to add your custom RouteHandler if you need anything that's not supported out-of-the-box. Take a look at the [wiki](https://github.com/tomkuijsten/restup/wiki) for details.

The REST implementation is using the guidelines from: https://github.com/tfredrich/RestApiTutorial.com.

### How you can start

Read the [wiki](https://github.com/tomkuijsten/restup/wiki), it explains it all.

### Latest release notes (beta2)
- Introducing RouteHandlers
- RestWebServer is obsolete, see [wiki](https://github.com/tomkuijsten/restup/wiki) for more info
- Static files can be served now
- Basic url query strings can be used
- The headed demo has a testing capabilities now 
