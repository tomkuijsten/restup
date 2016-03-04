# Webservice for Universal Windows Apps

[![Build status](https://ci.appveyor.com/api/projects/status/1aj7614fb0o1bjdy?svg=true)](https://ci.appveyor.com/project/tomkuijsten/restup)

[![NuGet downloads](https://img.shields.io/nuget/dt/restup.svg)](https://www.nuget.org/packages/Restup/)

# Release notes (beta2)
 - Added RouteHandlers to support custom handling of a request (big thanks to @Jark)
 - Added static file handler (big thanks to @Jark)
 - ...

# Intro

When the raspberry pi 2 was released, all windows 10 users were filled with joy when Microsoft announced the support of windows 10 for this neat device. After a couple of beta builds, we got the RTM version a couple of weeks ago. A crucial piece for this platform is missing, WCF. It might be supported in the future ([see post](https://social.msdn.microsoft.com/Forums/en-US/f462d578-368b-4218-b57e-19cd8852fd0c/wcf-hosting-in-windows-iot?forum=WindowsIoT)), but until then I would need some simple webservice implementation to keep my projects going.

The first couple of alpha and beta versions supported hosting REST controllers only, but since beta2 static files are supported as well. This also introduced a way to add your custom RouteHandler if you need anything that's not supported out-of-the-box. Take a look at the [wiki](https://github.com/tomkuijsten/restup/wiki) for details.

The REST implementation is using the guidelines from: https://github.com/tfredrich/RestApiTutorial.com.

# Getting started

Read the [wiki](https://github.com/tomkuijsten/restup/wiki), it explains it all.
