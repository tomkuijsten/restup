# Webservice for Universal Windows Apps

Release build: [![Build status](https://ci.appveyor.com/api/projects/status/jhdlmysux72qej51?svg=true)](https://ci.appveyor.com/project/tomkuijsten/restup-hwwpp)

Dev build: [![Build status](https://ci.appveyor.com/api/projects/status/h6faknf1b7b20994?svg=true)](https://ci.appveyor.com/project/tomkuijsten/restup-frwxx)

NuGet downloads: [![NuGet downloads](https://img.shields.io/nuget/vpre/restup.svg)](https://www.nuget.org/packages/Restup/)

### Goal
Provide a HTTP server supporting static html and REST in order to keep Windows IoT projects going. Let's hope there will be a native solution soon, although there aren't any signs for that yet (if you have one, let me [know](https://github.com/tomkuijsten/restup/issues/new)).

### How this started

When the raspberry pi 2 was released, all windows developers were filled with joy when Microsoft announced the support of windows 10 for this neat device. After a couple of beta builds, we got the RTM version. A crucial piece for this platform is missing, WCF. It might be supported in the future ([see post](https://social.msdn.microsoft.com/Forums/en-US/f462d578-368b-4218-b57e-19cd8852fd0c/wcf-hosting-in-windows-iot?forum=WindowsIoT)), but until then we need some simple webservice implementation to keep our projects going.

The first couple of alpha and beta versions supported hosting REST controllers only, but since beta2 static files are supported as well. This also introduced a way to add your custom RouteHandler if you need anything that's not supported out-of-the-box. Take a look at the [wiki](https://github.com/tomkuijsten/restup/wiki) for details.

The REST implementation is using the guidelines from: https://github.com/tfredrich/RestApiTutorial.com.

### How you can start

Read the [wiki](https://github.com/tomkuijsten/restup/wiki), it explains it all.

### Latest release notes (rc1)
 - CORS is supported now (read the Wiki for configuration options)
 - Removed "Devkoes." from namespaces
 - Url arrays implemented (#16)
 - Query parameters are supported (#49)
 - Couple of api changes (beta4 to rc1), read about them [here](https://github.com/tomkuijsten/restup/wiki/Migrate-Beta4-to-rc1)
